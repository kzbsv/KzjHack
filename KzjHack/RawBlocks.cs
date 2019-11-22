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
    class RawBlocks
    {
        public static string BlocksFolder => KzH.S.BlocksFolder;
        public static DateTime BlocksSince => KzH.S.BlocksSince;
        public static bool BlocksHeadersOnly => KzH.S.BlocksHeadersOnly;
        public static bool BlocksCheckAll => KzH.S.BlocksCheckAll;

        public static int GetLatestBlockHeight()
        {
            var kzrpc = KzH.GetKzRpc();
            var bci = kzrpc.GetBlockchainInfo();
            var height = (int)bci.blocks;
            return height;
        }

        static (bool ok, ReadOnlySequence<byte> ros) FetchBlockByHeightAndHash(int height, KzUInt256? hash, KzBlock b)
        {
            var kzrpc = KzH.GetKzRpc();
            if (hash == null) {
                hash = new KzUInt256(kzrpc.GetBlockHash(height));
            }
            var raw = kzrpc.GetBlockRaw(hash.Value).GetBytes();
            var ros = new ReadOnlySequence<byte>(raw);
            var bh = new KzBlockHeader { Height = height };
            b.Height = height;
            var tros = ros;
            if (!b.TryReadBlock(ref tros)) goto fail;
            return (true, ros);
            fail:
            return (false, ros);
        }

        static bool _Running;
        static bool _Stopping;
        public static bool IsRunning => _Running;
        public static bool IsStopping { get => _Stopping; set => _Stopping = value; }

        public static void StartVerifyRawBlocks()
        {
            _Running = true;
            Task.Run(async () => await VerifyRawBlocks(BlocksSince, BlocksHeadersOnly, BlocksCheckAll));
        }

        public static void StopVerifyRawBlocks()
        {
            _Running = false;
            _Stopping = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="since"></param>
        /// <param name="headersOnly"></param>
        /// <param name="checkAll"></param>
        /// <returns></returns>
        static Task VerifyRawBlocks(DateTime since, bool headersOnly = false, bool checkAll = false)
        {
            try {

                var height = GetLatestBlockHeight();
                KzH.WriteLine($"Latest Block Height {height}");

                var b = new KzBlock();

                while (_Running) {
                    var files = (string[])null;
                    var files_t = (int?)null;
                    var files_folder = string.Empty;
                    var hash = (KzUInt256?)null;
                    for (var h = height; h >= 0; h--) {
                        var t = h / 1000;
                        if (files_t != t) {
                            files_folder = Path.Combine(BlocksFolder, $"RawBlock{t:D3}");
                            if (!Directory.Exists(files_folder)) {
                                Directory.CreateDirectory(files_folder);
                            }
                            files = Directory.GetFiles(files_folder);
                            files_t = t;
                            KzH.WriteLine($"Verifying folder {files_folder} which initially contained {files.Length} files.");
                        }
                        var filename = Path.Combine(files_folder, $"RawBlock{h:D6}.dat");
                        if (files.Contains(filename)) {
                            // Block already exists in RawBlocks.
                            hash = null; // We won't have hash of next block to be verified.
                        } else {
                            // Block doesn't exist in RawBlocks yet.
                            var (ok, ros) = FetchBlockByHeightAndHash(h, hash, b);
                            if (!ok) {
                                KzH.WriteLine($"Failed to fetch block at height {h}");
                                break;
                            }
                            var bytes = ros.ToArray();
                            File.WriteAllBytes(filename, bytes);
                            hash = b.HashPrevBlock; // Remember hash of next block to be verified.
                            KzH.Write(".");
                            if (h % 100 == 0) KzH.WriteLine($" {h:D6} {DateTime.Now:HH:mm:ss}");
                        }
                    }
                }
                KzH.WriteLine();

            } catch (Exception ex) {
                KzH.WriteLine(ex.LastInnerMessage());
            }
            finally {
                _Running = false;
                _Stopping = true;
                KzH.WriteLine("Verify RawBlocks Done");
            }

            return Task.FromResult(0);
        }
    }
}
