#region Copyright
// Copyright (c) 2019 TonesNotes
// Distributed under the Open BSV software license, see the accompanying file LICENSE.
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using KzjHack.Controls;
using KzBsv;
using KZJ;

namespace KzjHack
{
    class MnemonicControlNonUI
    {
        MnemonicControl _mc;

        KzMnemonic _m;
        public KzMnemonic Mnemonic => _m;

        string _defaultMessage;
        int digitsBase = 10;
        int bitLength = 128;

        bool _updating = false;

        public string Words { get => _mc.tbWords.Text; set => _mc.tbWords.Text = value; }

        public void SetUiControl(MnemonicControl mc)
        {
            _mc = mc;

            _defaultMessage = _mc.labelMessage.Text;

            _mc.tbWords.TextChanged += new System.EventHandler(this.TbWords_TextChanged);
            _mc.rbDigitsBase10.CheckedChanged += new System.EventHandler(this.RbDigitsBase10_CheckedChanged);
            _mc.buttonRandom.Click += new System.EventHandler(this.ButtonRandom_Click);
            _mc.buttonClear.Click += ButtonClear_Click;
            _mc.tbHex.TextChanged += new System.EventHandler(this.TbHex_TextChanged);
            _mc.tbDigits.TextChanged += new System.EventHandler(this.TbDigits_TextChanged);
            _mc.rbDigitsBase6.CheckedChanged += new System.EventHandler(this.RbDigitsBase6_CheckedChanged);
            _mc.rbBitLength256.CheckedChanged += new System.EventHandler(this.RbBitLength256_CheckedChanged);
            _mc.rbBitLength128.CheckedChanged += new System.EventHandler(this.RbBitLength128_CheckedChanged);
        }

        void ButtonClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void TbWords_TextChanged(object sender, EventArgs e)
        {
            if (_updating) return;
            try {
                _m = new KzMnemonic(_mc.tbWords.Text);
                Update(_m, words: false);
                _mc.labelMessage.Text = _defaultMessage;
            } catch (Exception ex) {
                _mc.labelMessage.Text = ex.LastInnerMessage();
            }
        }

        private void TbHex_TextChanged(object sender, EventArgs e)
        {
            if (_updating) return;
            try {
                var hex = KzEncoders.Hex.Decode(_mc.tbHex.Text);
                if (hex.Length == 16) {
                    _mc.rbBitLength128.Checked = true;
                } else if (hex.Length == 32) {
                    _mc.rbBitLength256.Checked = true;
                } else throw new InvalidOperationException("Hex string must be either 16 or 32 bytes long.");
                _m = new KzMnemonic(hex);
                Update(_m, hex: false);
                _mc.labelMessage.Text = _defaultMessage;
            } catch (Exception ex) {
                _mc.labelMessage.Text = ex.LastInnerMessage();
            }
        }

        private void TbDigits_TextChanged(object sender, EventArgs e)
        {
            if (_updating) return;
            try {
                _m = digitsBase == 10 ? KzMnemonic.FromBase10(_mc.tbDigits.Text, length: bitLength) : KzMnemonic.FromBase6(_mc.tbDigits.Text, length: bitLength);
                var hex = _m.Entropy;
                if (hex.Length == 16) {
                    _mc.rbBitLength128.Checked = true;
                } else if (hex.Length == 32) {
                    _mc.rbBitLength256.Checked = true;
                } else throw new InvalidOperationException("Digits string must be either 16 or 32 bytes of data long.");
                Update(_m, digits: false);
                _mc.labelMessage.Text = _defaultMessage;
            } catch (Exception ex) {
                _mc.labelMessage.Text = ex.LastInnerMessage();
            }
        }


        private void RbDigitsBase10_CheckedChanged(object sender, EventArgs e)
        {
            digitsBase = 10;
            if (_updating) return;
            try {
                var words = _mc.tbWords.Text;
                if (string.IsNullOrWhiteSpace(words)) return;
                _m = new KzMnemonic(words);
                Update(_m, words: false);
                _mc.labelMessage.Text = _defaultMessage;
            } catch (Exception ex) {
                _mc.labelMessage.Text = ex.LastInnerMessage();
            }
        }

        private void RbDigitsBase6_CheckedChanged(object sender, EventArgs e)
        {
            digitsBase = 6;
            if (_updating) return;
            try {
                var words = _mc.tbWords.Text;
                if (string.IsNullOrWhiteSpace(words)) return;
                _m = new KzMnemonic(words);
                Update(_m, words: false);
                _mc.labelMessage.Text = _defaultMessage;
            } catch (Exception ex) {
                _mc.labelMessage.Text = ex.LastInnerMessage();
            }
        }

        void SetBitLength(int bits)
        {
            if (bits == 128) _mc.rbBitLength128.Checked = true;
            else if (bits == 256) _mc.rbBitLength256.Checked = true;
            else {
                _mc.rbBitLength128.Checked = false;
                _mc.rbBitLength256.Checked = false;
                return;
            }
            bitLength = bits;
        }

        private void RbBitLength128_CheckedChanged(object sender, EventArgs e)
        {
            bitLength = 128;
            //Clear();
        }

        private void RbBitLength256_CheckedChanged(object sender, EventArgs e)
        {
            bitLength = 256;
            //Clear();
        }

        private void ButtonRandom_Click(object sender, EventArgs e)
        {
            _m = new KzMnemonic(bitLength);
            Update(_m);
        }

        void Update(KzMnemonic m, bool digits = true, bool words = true, bool hex = true)
        {
            if (_updating) return;
            try {
                _updating = true;
                if (words) _mc.tbWords.Text = m.Words;
                if (hex) _mc.tbHex.Text = m.ToHex();
                if (digits)_mc.tbDigits.Text = digitsBase == 6 ? m.ToDigitsBase6() : m.ToDigitsBase10();
                SetBitLength(m.Entropy.Length == 16 ? 128 : m.Entropy.Length == 32 ? 256 : 0);
            }
            finally {
                _updating = false;
            }
        }

        void Clear()
        {
            if (_updating) return;
            try {
                _updating = true;
                _mc.tbWords.Text = "";
                _mc.tbHex.Text = "";
                _mc.tbDigits.Text = "";
                _mc.labelMessage.Text = _defaultMessage;
            }
            finally {
                _updating = false;
            }
        }
    }
}
