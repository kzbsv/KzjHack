#region Copyright
// Copyright (c) 2019 TonesNotes
// Distributed under the Open BSV software license, see the accompanying file LICENSE.
#endregion
using System;
using System.Linq;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using KzBsv;
using KZJ;

namespace KzjHack
{
    /// <summary>
    /// Provides access to historical blocks and block headers.
    /// </summary>
    class Blocks
    {
        /// <summary>
        /// Must hold this lock to access _BlockHeaders, _OrphanHeaders, or PERSITENT STORAGE!
        /// </summary>
        static object _BlockHeadersLock = new object();
        /// <summary>
        /// Indexed by block height. Unknown history will have null value.
        /// There will only be one sequence of known blocks (no gaps with nulls).
        /// The headers in the sequence will always honor HashPrevBlock
        /// Backup persistent storage is under BlocksFolder: 1000 blocks per subfolder. Named by block height.
        /// </summary>
        static List<KzBlockHeader> _BlockHeaders = new List<KzBlockHeader>(1_000_000);
        /// <summary>
        /// As oprphans are encountered, headers are added here. Key is height.
        /// Backup persistent storage is under BlocksFolder's orphans subfolder, then height subfolder. Named by block hash.
        /// </summary>
        static Dictionary<int, List<KzBlockHeader>> _OrphanHeaders = new Dictionary<int, List<KzBlockHeader>>();

        public static string BlocksFolder => KzH.S.BlocksFolder;
        public static DateTime BlocksSince => KzH.S.BlocksSince;

        public static KzBlock GetLatestBlock()
        {
            var kzrpc = KzH.GetKzRpc();
            var bci = kzrpc.GetBlockchainInfo();
            var height = (int)bci.blocks;
            var hash = new KzUInt256(bci.bestblockhash);
            lock (_BlockHeadersLock) {
                var bh = GetBlockByHeightAndHash(height, hash, false);
                return bh as KzBlock;
            }
        }

        public static IEnumerable<(KzBlock b, KzTransaction tx, KzTxOut o)> GetOutputsSendingToAddresses(KzUInt160[] addresses, DateTime since)
        {
            var v = new KzUInt160();
            foreach (var b in Blocks.GetBlocks()) {
                if (b.TimeWhen < since)
                    break;
                foreach (var tx in b.Txs) {
                    foreach (var o in tx.Vout) {
                        foreach (var op in o.ScriptPubKey.Decode()) {
                            if (op.Code == KzOpcode.OP_PUSH20) {
                                op.Data.ToSpan().CopyTo(v.Span);
                                if (Array.BinarySearch<KzUInt160>(addresses, v) >= 0) {
                                    yield return (b, tx, o);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static IEnumerable<KzBlock> GetBlocksFrom(int height, KzUInt256 hash)
        {
            var b = GetBlockByHeightAndHash(height, hash, headersOnly:false) as KzBlock;
            while (b != null) {
                yield return b;
                b = GetNextBlock(b);
            }
        }

        public static IEnumerable<KzBlock> GetBlocks()
        {
            var b = GetLatestBlock();
            while (b != null) {
                yield return b;
                b = GetNextBlock(b);
            }
        }

        public static KzBlock GetNextBlock(KzBlock b)
        {
            var bh = GetBlockByHeightAndHash(b.Height - 1, b.HashPrevBlock, false);
            return bh as KzBlock;
        }

        /// <summary>
        /// Must hold _BlockHeadersLock.
        /// Checks in memory cache, then persistent storage, then issues JSON-RPC request.
        /// Leaves memory cache and persistent storage updated. 
        /// </summary>
        /// <param name="height"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static KzBlockHeader GetBlockByHeightAndHash(int height, KzUInt256 hash, bool headersOnly = false)
        {
            lock (_BlockHeadersLock) {
                var found = false;
                var inCache = false;
                var onDisk = false;
                var isOrphan = false;
                var bh = new KzBlockHeader();
                var ros = new ReadOnlySequence<byte>();

                var bhs = _BlockHeaders;
                var ohs = _OrphanHeaders;

                // Check main chain in memory cache _BlockHeaders.
                if (height < bhs.Count) {
                    bh = bhs[height];
                    if (bh != null && bh.Hash == hash) { found = true; inCache = true; goto done; }
                }
                // Check orphans in memory cache _OrphanHeaders.
                if (ohs.TryGetValue(height, out List<KzBlockHeader> os)) {
                    bh = os.Find(o => o.Hash == hash);
                    if (bh != null) { found = true; inCache = true; isOrphan = true; goto done; }
                }
                // Check main chain on disk.
                var filename = GetBlockFilename(height);
                bh = LoadBlockFromFilename(height, filename, headersOnly);
                if (bh != null && bh.Hash != hash) {
                    // Demote block on disk to orphan. It's not what we want.
                    var orphanname = GetOrphanFilename(height, bh.Hash);
                    Directory.CreateDirectory(Path.GetDirectoryName(orphanname));
                    File.Move(filename, orphanname);
                    bh = null;
                }
                if (bh != null) { found = true; inCache = false; isOrphan = false; onDisk = true; goto done; }

                // Check orphans on disk.
                filename = GetOrphanFilename(height, hash);
                bh = LoadBlockFromFilename(height, filename, headersOnly);
                if (bh != null) { found = true; inCache = false; isOrphan = true; onDisk = true; goto done; }

                // Request block by JSON-RPC
                var heightConfirmed = false;
                (heightConfirmed, bh, ros) = FetchBlockByHeightAndHash(height, hash, confirmHeight: true, headerOnly: headersOnly);
                if (bh != null) { found = true; inCache = false; isOrphan = !heightConfirmed; onDisk = false; goto done; }

                return null;

            done:
                if (found) {
                    AddBlockToInventory(height, hash, inCache, onDisk, isOrphan, bh, ros);
                }

                return bh;
            }
        }

        public static bool BlocksHeadersOnly => KzH.S.BlocksHeadersOnly;
        public static bool BlocksCheckAll => KzH.S.BlocksCheckAll;

        static bool _Running;
        static bool _Stopping;
        public static bool IsRunning => _Running;
        public static bool IsStopping { get => _Stopping; set => _Stopping = value; }

        public static (bool found, bool isOrphan, KzBlockHeader bh) FindBlockInCacheByHash(KzUInt256 hash, bool orphansOnly = false)
        {
            lock (_BlockHeadersLock) {
                var found = false;
                var isOrphan = false;
                var bh = (KzBlockHeader)null;

                if (!orphansOnly) {
                    var bhs = _BlockHeaders;
                    var height = bhs.Count - 1;
                    while (height > 0 && bhs[height] != null && bhs[height].Hash != hash) height--;
                    if (height >= 0 && bhs[height] != null) { found = true; bh = bhs[height]; goto done; }
                }

                var ohs = _OrphanHeaders;
                var orphanHeights = ohs.Keys.OrderByDescending(k => k);
                foreach (var orphanHeight in orphanHeights) {
                    bh = ohs[orphanHeight].Find(o => o.Hash == hash);
                    if (bh != null) { found = true; isOrphan = true; goto done; }
                }

            done:
                return (found, isOrphan, bh);
            }
        }

        public static (bool found, bool isOrphan, KzBlockHeader bh) FindBlockInCacheByHeightAndHash(int height, KzUInt256 hash, bool orphansOnly = false)
        {
            lock (_BlockHeadersLock) {
                var found = false;
                var isOrphan = false;
                var bh = (KzBlockHeader)null;

                if (!orphansOnly) {
                    var bhs = _BlockHeaders;
                    if (height >= 0 && bhs[height] != null && bhs[height].Hash == hash) { found = true; bh = bhs[height]; goto done; }
                }

                var ohs = _OrphanHeaders;
                bh = ohs[height].Find(o => o.Hash == hash);
                if (bh != null) { found = true; isOrphan = true; goto done; }

            done:
                return (found, isOrphan, bh);
            }
        }

        public static Span<KzBlockHeader> FindOrphanChain(KzBlockHeader block)
        {
            lock (_BlockHeadersLock) {
                var c = new KzBlockHeader[] { block }.ToList();

                while (true) {
                    var (found, isOrphan, bh) = FindBlockInCacheByHeightAndHash(block.Height - 1, block.HashPrevBlock);
                    if (found) {
                        c.Add(bh);
                        block = bh;
                    }
                    if (!found || !isOrphan)
                        break;
                }

                c.Reverse();
                return c.ToArray().AsSpan();
            }
        }

        public static void AddNewBlock(KzBlock block, ReadOnlySequence<byte> ros) { Task.Run(() => AddNewBlockAsync(block, ros)); }

        public static void AddNewBlockAsync(KzBlock block, ReadOnlySequence<byte> ros)
        {
            lock (_BlockHeadersLock) {

                var (found, isOrphan, bh) = FindBlockInCacheByHash(block.HashPrevBlock);
                if (found) {
                    block.Height = bh.Height + 1;
                    var bhs = _BlockHeaders;
                    // If previous block is an orphan, or on main chain but not at the tip, treat this block as an orphan.
                    isOrphan |= block.Height != bhs.Count;
                    AddBlockToInventory(block.Height, block.Hash, inCache:false, onDisk:false, isOrphan, block, ros);
                    var what = isOrphan ? "ORPHAN" : "BLOCK";
                    KzH.WriteLine($"ADD {what} {block.Hash.ToString()} {block.TimeWhen.ToString("yyyy-MM-dd HH:mm:ss")} {block.Height}");
                    if (isOrphan && block.Height >= bhs.Count) {
                        // Since we're just following a fully validating node ;-), we only care about what we perceive as an orphan
                        // when what we see as chain of orphans is longer than our current main chain.
                        // Switch to new longest chain...
                        var newChain = FindOrphanChain(block);
                        var fork = newChain[0].Height; // last common block
                        var oldChain = bhs.GetRange(fork, bhs.Count - fork).ToArray().AsSpan();
                        Trace.Assert(newChain[0].Hash == oldChain[0].Hash);
                        var ohs = _OrphanHeaders;
                        // Demote oldChain[1..] to orphans
                        // - Move in cache: From _BlockHeaders to _OrphanHeaders.
                        // - Move on disk: From main chain to Orphans folder. 
                        foreach (var b in oldChain[1..]) {
                            bhs[b.Height] = null;
                            ohs[b.Height].Add(b);
                            File.Move(GetBlockFilename(b.Height), GetOrphanFilename(b.Height, b.Hash));
                        }
                        // Promote newChain[1..] to main chain.
                        // - Move in cache: To _BlockHeaders from _OrphanHeaders.
                        // - Move on disk: To main chain from Orphans folder. 
                        foreach (var b in newChain[1..]) {
                            while (bhs.Count <= b.Height) bhs.Add(null);
                            bhs[b.Height] = b; ;
                            ohs[b.Height].Remove(b);
                            File.Move(GetOrphanFilename(b.Height, b.Hash), GetBlockFilename(b.Height));
                        }
                        // This shouldn't be the case but...
                        var lastHeight = newChain[^1].Height;
                        if (bhs.Count - 1 > lastHeight) bhs.RemoveRange(lastHeight + 1, bhs.Count - lastHeight - 1);
                        // Log this for now... once UTXO and mempool management is added, it needs to be informed.
                        KzH.WriteLine("RE-ORG:");
                        foreach (var b in oldChain[0..])
                            KzH.WriteLine($"OLD BLOCKS {b.Hash.ToString()} {b.TimeWhen.ToString("yyyy-MM-dd HH:mm:ss")} {b.Height}");
                        foreach (var b in newChain[0..])
                            KzH.WriteLine($"NEW BLOCKS {b.Hash.ToString()} {b.TimeWhen.ToString("yyyy-MM-dd HH:mm:ss")} {b.Height}");
                    }
                } else {
                    // Don't know where this block goes... perhaps we didn't load enough history...
                }
            }
        }

        public static void StartGetOldBlocks()
        {
            _Running = true;
            Task.Run(async () => await GetOldBlocks(BlocksSince, BlocksHeadersOnly, BlocksCheckAll));
        }

        public static void StopGetOldBlocks()
        {
            _Running = false;
            _Stopping = false;
        }

        /// <summary>
        /// Make sure we have the longest chain since a particular point in time.
        /// Uses JSON-RPC to ask a node for the current bestblockhash and height ("blocks" count).
        /// Works backwards from there chaining HashPrevBlock.
        /// Block headers are saved in the _BlockHeaders list.
        /// If an existing header with different hash is encountered:
        /// - It is demoted to _OrphanHeaders and replaced in _BlockHeaders
        /// - An event is signalled which can be used to maintain the mempool.
        /// Blocks are also saved as disk files in the BlocksFolder.
        /// The sequence for fetching a block by height and hash is:
        /// - Hold the _BlockHeadersLock
        /// - Check if _BlockHeaders[height] exists and has correct hash. If so...done.
        /// - If exists but hash is different, check orphans.
        /// - If still not found use JSON-RPC to request it by hash.
        /// - If not at the beginning of time, verify previous blocks (REORG PROCESSING):
        /// --- Fetch new block at target height with required hash.
        /// --- Signal a block replacement event.
        /// --- Continue working backwards until happy or out of time :-)
        /// </summary>
        /// <param name="since"></param>
        /// <param name="headersOnly"></param>
        /// <param name="checkAll"></param>
        /// <returns></returns>
        static Task GetOldBlocks(DateTime since, bool headersOnly = false, bool checkAll = false)
        {
            try {

                var kzrpc = KzH.GetKzRpc();
                var bci = kzrpc.GetBlockchainInfo();
                var height = (int)bci.blocks;
                var hash = new KzUInt256(bci.bestblockhash);
                KzH.WriteLine($"BestBlockHash {hash} {height}");

                var bh = new KzBlockHeader();
                var b = new KzBlock();

                while (_Running) {
                    lock (_BlockHeadersLock) {
                        bh = GetBlockByHeightAndHash(height, hash, headersOnly);
                        if (bh == null) {
                            KzH.WriteMessageLine($"FAILED GetBlockByHeightAndHash({height}, {hash})");
                        }
                        if (bh?.TimeWhen < since)
                            break;
                        height--;
                        hash = bh.HashPrevBlock;
                        KzH.Write(".");
                        if (height % 100 == 0) KzH.WriteLine();
                    }
                }
                KzH.WriteLine();
                var what = bh is KzBlock ? "Block" : "BlockHeader";
                KzH.WriteLine($"{what} {bh.Hash.ToString()} {bh.TimeWhen.ToString("yyyy-MM-dd HH:mm:ss")} {bh.Height}");

            } catch (Exception ex) {
                KzH.WriteLine(ex.LastInnerMessage());
            }
            finally {
                _Running = false;
                _Stopping = true;
                KzH.WriteLine("Block Headers Done");
            }

            return Task.FromResult(0);
        }

        public static (bool found, bool headerOnly, string filename, byte[] raw) LoadRawBlockBytes(int height)
        {
            var found = false;
            var headerOnly = false;
            var raw = (byte[])null;
            var filename = GetBlockFilename(height);
            if (File.Exists(filename)) {
                raw = File.ReadAllBytes(filename);
                found = true;
                headerOnly = raw.Length <= KzBlock.BlockHeaderSize;
            }

            return (found, headerOnly, filename, raw);
        }

        static void AddBlockToInventory(int height, KzUInt256 hash, bool inCache, bool onDisk, bool isOrphan, KzBlockHeader bh, ReadOnlySequence<byte> ros)
        {
            var bhs = _BlockHeaders;
            var ohs = _OrphanHeaders;

            if (!inCache) {
                if (isOrphan) {
                    if (!ohs.TryGetValue(height, out List<KzBlockHeader> os)) { os = new List<KzBlockHeader>(); ohs.Add(height, os); }
                    os.Add(bh);
                } else {
                    if (bhs.Count <= height) {
                        if (bhs.Capacity < height) bhs.Capacity += bhs.Capacity / 10;
                        while (bhs.Count <= height) bhs.Add(null);
                    }
                    bhs[height] = bh;
                }
            }
            if (!onDisk) {
                string filename;
                if (isOrphan) {
                    filename = GetOrphanFilename(height, bh.Hash);
                } else {
                    filename = GetBlockFilename(height);
                }
                Trace.Assert(bh.Hash == hash);
                SaveRawBlock(height, bh.Hash, ros, filename);
            }
        }

        public static void SaveRawBlockIfNew(int height, KzUInt256 hash, ReadOnlySequence<byte> ros, string filename)
        {
            if (!File.Exists(filename) || new FileInfo(filename).Length < ros.Length)
                SaveRawBlock(height, hash, ros, filename);
        }

        public static void SaveRawBlock(int height, KzUInt256 hash, ReadOnlySequence<byte> ros, string filename)
        {
            var bytes = ros.ToArray();
            Directory.CreateDirectory(Path.GetDirectoryName(filename));
            File.WriteAllBytes(filename, bytes);
            //Console.WriteLine($"Block {height} {hash} SAVED.");
        }

        public static string GetBlockFilename(int height)
        {
            var filename = Path.Combine(BlocksFolder, $"RawBlock{height/1000:D3}\\RawBlock{height:D6}.dat");
            return filename;
        }

        public static string GetOrphanFilename(int height, KzUInt256 hash)
        {
            var filename = Path.Combine(BlocksFolder, $"Orphans\\{height:D6}\\RawBlock{hash.ToString()}.dat");
            return filename;
        }

        static (bool heightConfirmed, KzBlockHeader bh, ReadOnlySequence<byte> ros) FetchBlockByHeightAndHash(int height, KzUInt256 hash, bool confirmHeight = false, bool headerOnly = false)
        {
            var heightConfirmed = false;
            var kzrpc = KzH.GetKzRpc();
            if (confirmHeight) {
                var h = kzrpc.GetBlockHash(height);
                if (h == hash.ToString()) heightConfirmed = true;
            }
            var raw = (headerOnly ? kzrpc.GetBlockHeaderRaw(hash) : kzrpc.GetBlockRaw(hash)).GetBytes();
            var ros = new ReadOnlySequence<byte>(raw);
            var bh = new KzBlockHeader { Height = height };
            var tros = ros;
            if (headerOnly && !bh.TryReadBlockHeader(ref tros)) goto fail;
            if (!headerOnly) {
                var b = new KzBlock { Height = height };
                if (!b.TryReadBlock(ref tros)) goto fail;
                bh = b;
            }
            return (heightConfirmed, bh, ros);
        fail:
            return (heightConfirmed, null, ros);
        }

        static KzBlockHeader LoadBlockFromFilename(int height, string filename, bool headerOnly = false)
        {
            var raw = headerOnly ? new byte[KzBlockHeader.BlockHeaderSize] : null;
            var count = 0;
            var bh = new KzBlockHeader { Height = height };
            if (File.Exists(filename)) {
                if (headerOnly) {
                    using (var fs = File.OpenRead(filename)) {
                        count = fs.Read(raw);
                        if (count != raw.Length) return null;
                    }
                } else {
                    raw = File.ReadAllBytes(filename);
                }
                var ros = new ReadOnlySequence<byte>(raw);
                if (headerOnly && bh.TryReadBlockHeader(ref ros)) {
                    return bh;
                }
                if (!headerOnly) {
                    var b = new KzBlock { Height = height };
                    if (b.TryReadBlock(ref ros)) {
                        return b;
                    }
                }
            }

            return null;
        }

    }
}
