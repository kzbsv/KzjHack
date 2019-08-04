#region Copyright
// Copyright (c) 2019 TonesNotes
// Distributed under the Open BSV software license, see the accompanying file LICENSE.
#endregion
using KzBsv;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Text;
using System.Threading.Tasks;

namespace KzjHack
{
    static class Zmq
    {
        static async Task ReadTxHashPipeAsync(PipeReader reader)
        {
            // Read Bitcoin transaction ids from a PipeReader
            while (_zmqRunning) {
                var rr = await reader.ReadAsync();
                var buf = rr.Buffer;
                var txId = new KzUInt256();
                buf.ToSpan().CopyTo(txId.Span);
                reader.AdvanceTo(buf.GetPosition(32));
                KzH.WriteLine($"TxId parsed id={txId.ToHex()}");
            }
        }

        static async Task ReadTxPipeAsync(PipeReader reader)
        {
            var tx = new KzTransaction();

            // Read Bitcoin transactions from a PipeReader
            while (_zmqRunning) {
                var rr = await reader.ReadAsync();
                var consumed = 0L;
                var buf = rr.Buffer;
                if (tx.TryReadTransaction(ref buf)) {
                    reader.AdvanceTo(buf.GetPosition(consumed));
                    KzH.WriteLine($"Tx parsed id={tx.TxId.ToHex()}");
                } else {
                    reader.AdvanceTo(buf.Start, buf.End);
                    KzH.WriteLine($"Tx parse failed");
                }
            }

        }

        static async Task ReadBlockHashPipeAsync(PipeReader reader)
        {
            // Read Bitcoin block ids from a PipeReader
            while (_zmqRunning) {
                var rr = await reader.ReadAsync();
                var buf = rr.Buffer;
                var blockId = new KzUInt256();
                buf.ToSpan().CopyTo(blockId.Span);
                reader.AdvanceTo(buf.GetPosition(32));
                KzH.WriteLine($"BlockId parsed id={blockId.ToHex()}");
            }
        }

        static async Task ReadBlockPipeAsync(PipeReader reader)
        {
            var block = new KzBlock();

            // Read Bitcoin block headers from a PipeReader
            while (_zmqRunning) {
                var rr = await reader.ReadAsync();
                var consumed = 0L;
                var buf = rr.Buffer;
                var ros = buf;
                if (block.TryReadBlock(ref buf)) {
                    reader.AdvanceTo(buf.GetPosition(consumed));
                    //KzH.WriteMessageLine($"Block parsed {block.Hash}");
                    Blocks.AddNewBlock(block, ros);
                } else {
                    reader.AdvanceTo(buf.Start, buf.End);
                    KzH.WriteMessageLine($"Block parse failed");
                }
            }

        }

        static bool _zmqRunning;
        static bool _zmqStopping;
        public static bool IsRunning => _zmqRunning;
        public static bool IsStopping => _zmqStopping;

        public static void StartZmq()
        {
            _zmqRunning = true;
            Task.Run(async () => await Zmq.MainZmq(null));
        }

        public static void StopZmq()
        {
            _zmqRunning = false;
        }

        public static async Task MainZmq(string[] args)
        {

            var txhashpipe = new Pipe();
            var txhashreadingtask = ReadTxHashPipeAsync(txhashpipe.Reader);

            var txpipe = new Pipe();
            var txreadingtask = ReadTxPipeAsync(txpipe.Reader);

            var blockhashpipe = new Pipe();
            var blockhashreadingtask = ReadBlockHashPipeAsync(blockhashpipe.Reader);

            var blockpipe = new Pipe();
            var blockreadingtask = ReadBlockPipeAsync(blockpipe.Reader);

            try {
                using (var sub = new SubscriberSocket()) {
                    sub.Options.ReceiveHighWatermark = 1000;
                    sub.Connect(KzH.S.BitcoinSvZmqAddress);
                    //sub.Subscribe("hashtx");
                    //sub.Subscribe("rawtx");
                    sub.Subscribe("hashblock");
                    //sub.Subscribe("rawblock");
                    while (_zmqRunning) {
                        var mm = sub.ReceiveMultipartMessage();
                        var ch = mm[0].ConvertToString();
                        if (ch == "hashtx" || ch == "rawtx" || mm.FrameCount == 3) {
                            var bytes = mm[1].Buffer;
                            var count = BitConverter.ToUInt32(mm[2].Buffer);
                            //KzH.WriteLine($"{ch} {count} {bytes.Length}");
                            switch (ch) {
                                case "hashtx": {
                                        var w = txhashpipe.Writer;
                                        w.Write(bytes); // 32 bytes transaction ID
                                        // count is the transaction number in the mempool.
                                        await w.FlushAsync();
                                    } break;
                                case "rawtx": {
                                        var w = txpipe.Writer;
                                        w.Write(bytes);
                                        await w.FlushAsync();
                                    }
                                    break;
                                case "hashblock": {
                                        var w = blockhashpipe.Writer;
                                        w.Write(bytes);
                                        await w.FlushAsync();
                                    }
                                    break;
                                case "rawblock": {
                                        var w = blockpipe.Writer;
                                        w.Write(bytes);
                                        await w.FlushAsync();
                                    }
                                    break;
                                default: break;
                            }
                        } else {
                            //KzH.WriteLine($"{ch}");
                        }
                    }
                }
            } catch (Exception e) {
                KzH.WriteLine(e.Message);
            }

            //await Task.WhenAll(txreadingtask, blockreadingtask);
        }
    }
}
