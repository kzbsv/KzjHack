#region Copyright
// Copyright (c) 2019 TonesNotes
// Distributed under the Open BSV software license, see the accompanying file LICENSE.
#endregion
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using KzBsv;
using KzjHack.Controls;

namespace KzjHack
{
    public class Wallet
    {
        MnemonicControlNonUI _mc;
        WalletControl _wc;

        public string Words { get => _wc.tbWords.Text; set => _wc.tbWords.Text = value; }

        internal void SetRelatedControls(WalletControl walletControl, MnemonicControlNonUI mnemonicControl)
        {
            _wc = walletControl;
            _mc = mnemonicControl;

            _wc.buttonOpen.Click += walletOpen_Click;
            _wc.tbKeyPath.Text = "m/1'/1/1";
        }

        List<KzExtPrivKey> keys = new List<KzExtPrivKey>();

        void walletOpen_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Words)) Words = _mc.Words;

            var entropy = KzMnemonic.FromWords(Words).Entropy;
#if false
            if (entropy.Length != 32) {
                KzH.WriteMessageLine("Require 256 bits of entropy for key creation. 24 Mnemonic words.");
                return;
            }
            var pke = entropy.ToKzUInt256();
#endif

            KzH.WriteMessageLine("");

            var pmpubkey = new KzB58ExtPubKey("xpub661MyMwAqRbcEaJYm4GjL9XnYrwbTR7Rug3oZ66juJHMXYwCYD4Z3RVgyoPhhpU97Ls9fACV3Y7kYqMPxGAA8XWFdPpaXAj3qb8VHnRMU8c").GetKey();

            var master = new KzExtPrivKey();

            //Kz.SetMasterSeed = "Bitcoin Seed";

            master.SetMaster(entropy);

            var kp = KzKeyPath.Parse(_wc.tbKeyPath.Text);

            var key = master.Derive(kp);

            var privkey = key.Derive(int.MaxValue).PrivKey;

            var pubkey = key.GetExtPubKey().ToString();
            var idkey = key.GetExtPubKey().Derive(int.MaxValue).PubKey.ToHex();

            KzH.WriteMessageLine(pubkey);
            KzH.WriteMessageLine(privkey.ToString());
            KzH.WriteMessageLine(privkey.GetPubKey().ToHex());

#if false
            var a2 = key.PrivKey.GetPubKey().ToAddress();

            var m1h = master.Derive(1, true);

            var m1h1 = m1h.Derive(1, false);

            var m1h11 = m1h1.Derive(1, false);

            var privKey1h11 = m1h11.PrivKey;

            var pubKey1h11 = privKey1h11.GetPubKey();

            var address = pubKey1h11.ToAddress();

            // Let's find the transaction that sent $1 to pubKey1h11...

            var pubKeyHash = pubKey1h11.ToHash160();

            var hash = new KzUInt256("000000000000000001889279630996ec4e77c5049a78a13f6081f17a1d58ff9c");
            var height = 581223;

            // In block 000000000000000000a10abb5e59c2e764dbeffa225c3b02c44c376c73ae38fa  # 582818 @ 2019-05-17 23:47:29 🙂
            hash = new KzUInt256("000000000000000000a10abb5e59c2e764dbeffa225c3b02c44c376c73ae38fa");
            height = 582818;

            // In block 000000000000000003a4daaec811226a1aed594429a409b4d68132eb8a9065d2 # 582821
            hash = new KzUInt256("000000000000000003a4daaec811226a1aed594429a409b4d68132eb8a9065d2");
            height = 582821;

            var block = Blocks.GetBlockByHeightAndHash(height, hash, false) as KzBlock;

            var addresses = new[] { pubKeyHash };
            Array.Sort(addresses);
            var txOuts = new List<(KzTransaction tx, KzTxOut o, int i)>(block.GetOutputsSendingToAddresses(addresses));

            var txOut = txOuts[0];

            var pubKey = pubKey1h11;
            var txb = KzBTransaction.P2PKH(new[] { (pubKey, txOut.tx, txOut.i) }, new[] { (pubKey, txOut.o.Value - 300) });
            txb.Sign(new[] { privKey1h11 });

            var rpc = KzH.GetKzRpc();
            try {
                var (txok, txid) = rpc.SendRawTransaction(txb.ToTransaction().ToBytes().ToHex());
            }
            catch (Exception ex) {
                var m = ex.Message;
            }
#endif
        }
    }
}
