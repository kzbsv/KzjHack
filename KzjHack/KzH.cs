#region Copyright
// Copyright (c) 2019 TonesNotes
// Distributed under the Open BSV software license, see the accompanying file LICENSE.
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using KzBsv;
using KzjHack.Controls;
using KZJ;

namespace KzjHack
{
    static class KzH
    {
        static KzjHackSettings _S;
        /// <summary>
        /// Application Settings.
        /// </summary>
        public static KzjHackSettings S => _S;

        static MainForm _MainForm;
        static UiLayoutManager _UiMgr;

        static System.Windows.Forms.TextBox _tbMessage;
        static LogControl _logControl;
        static MnemonicControlNonUI _mnemonicControl;
        static WalletControl _walletControl;
        static Wallet _wallet;

        internal static bool _enableZmqTask;
        internal static bool _enableBlockHeadersTask;
        internal static bool _enableVerifyRawBlocksTask;

        public static KzRpcClient GetKzRpc()
        {
            var auth = _S.BitcoinSvRpcUsername + ":" + _S.BitcoinSvRpcPassword;
            var uri = new Uri(_S.BitcoinSvRpcAddress);

            var kzrpc = new KzRpcClient(auth, uri);

            return kzrpc;
        }


        static void _MainForm_Load(object sender, EventArgs e)
        {
            _UiMgr = _MainForm._UiMgr;
            _UiMgr.SetOkCancel(_MainForm.okCancelControl);

            _logControl = _MainForm.logControl;
            _tbMessage = _MainForm.tbMessage;

            SetDefaultMessage();

            _mnemonicControl = new MnemonicControlNonUI();
            _mnemonicControl.SetUiControl(_MainForm.mnemonicControl);

            _walletControl = _MainForm.walletControl;
            _wallet = new Wallet();
            _wallet.SetRelatedControls(_walletControl, _mnemonicControl);

            _MainForm.actionsMain.buttonSettings.Click += ButtonSettings_Click;
            _MainForm.tasksControl.cbZMQ.CheckedChanged += CbZMQ_CheckedChanged;
            _MainForm.tasksControl.cbBlockHeaders.CheckedChanged += CbBlockHeaders_CheckedChanged;
            _MainForm.tasksControl.cbVerifyRawBlocks.CheckedChanged += CbVerifyRawBlocks_CheckedChanged;
            RawBlocks.Done += RawBlocks_Done;
            _MainForm.timer1.Tick += Timer1_Tick;

            _UiMgr.LayoutsChanged += _UiMgr_LayoutsChanged;
        }

        static void SetDefaultMessage()
        {
            KzH.WriteMessageLine(@"
Basic Getting Started Instructions for KzjHack: a Bitcoin SV sandbox application.

You'll need access to a Bitcoin SV node for JSON-RPC and ZMQ access to block and transaction data.

Click the ""Settings"" button on the Actions panel to configure access to your node.
Also check the root folder to be used on this machine to save cached block data.

Once configured, switch to the Log panel to monitor the initial data retrieval.
In the Tasks panel, first click Fetch Recent Blocks to fetch recent blocks
from your node and save them locally.

Once that completes, or if you stop it by unchecking its checkbox, as long
as you've retrieved at least on recent block, click the Enable ZMQ checkbox to begin
receiving live transaction and new blocks.

See http://github.com/tonesnotes/KzBitcoinSV for more information.
");
        }

        static KzjHackSettings _editedSettings;

        static void ButtonSettings_Click(object sender, EventArgs e)
        {
            var j = _editedSettings = _S.Clone();
            var g = _MainForm.settingsGrid;
            g.SelectedObject = j;
            g.ToolbarVisible = false;
            _UiMgr.PushOkCancel("Settings", _SettingsOkCancelControl_DialogResult, "Settings", "Click OK to save changes.");
        }

        static void _SettingsOkCancelControl_DialogResult(object sender, ValueEventArgs<System.Windows.Forms.DialogResult> e)
        {
            if (e.Value == System.Windows.Forms.DialogResult.OK) {
                var j = _editedSettings;
                j.Save();
                // Apply changes...
                _S = j;
            }
            _UiMgr.PopOkCancel();
        }

        static void Timer1_Tick(object sender, EventArgs e)
        {
            var timer1 = _MainForm.timer1;
            timer1.Enabled = false;
            try {
                if (_enableZmqTask != Zmq.IsRunning) {
                    if (_enableZmqTask) {
                        WriteLine("ZMQ Start");
                        Zmq.StartZmq();
                    } else {
                        WriteLine("ZMQ Stop");
                        Zmq.StopZmq();
                    }
                }
                if (Blocks.IsStopping) {
                    _MainForm.tasksControl.cbBlockHeaders.Checked = false;
                    Blocks.IsStopping = false;
                }
                if (_enableBlockHeadersTask != Blocks.IsRunning) {
                    if (_enableBlockHeadersTask) {
                        WriteLine("Block Headers Start");
                        Blocks.StartGetOldBlocks();
                    } else {
                        WriteLine("Block Headers Stop");
                        Blocks.StopGetOldBlocks();
                    }
                }
                if (_enableVerifyRawBlocksTask != RawBlocks.IsRunning) {
                    if (_enableVerifyRawBlocksTask) {
                        WriteLine("Verify RawBlocks Start");
                        RawBlocks.StartVerifyRawBlocks();
                    } else {
                        WriteLine("Verify RawBlocks Stop");
                        RawBlocks.StopVerifyRawBlocks();
                    }
                }
            }
            catch (Exception ex) {
                WriteLine($"timer1_Tick: {ex.LastInnerMessage()}");
            }
            finally {
                timer1.Enabled = true;
            }
        }

        static void CbZMQ_CheckedChanged(object sender, EventArgs e)
        {
            _enableZmqTask = _MainForm.tasksControl.cbZMQ.Checked;
        }

        static void CbBlockHeaders_CheckedChanged(object sender, EventArgs e)
        {
            _enableBlockHeadersTask = _MainForm.tasksControl.cbBlockHeaders.Checked;
        }

        static void CbVerifyRawBlocks_CheckedChanged(object sender, EventArgs e) {
            _enableVerifyRawBlocksTask = _MainForm.tasksControl.cbVerifyRawBlocks.Checked;
        }

        static void RawBlocks_Done(object sender, EventArgs e) {
            if (_MainForm.InvokeRequired) {
                _MainForm.BeginInvoke(new Action<object, EventArgs>(RawBlocks_Done), sender, e);
                return;
            }
            _enableVerifyRawBlocksTask = false;
            _MainForm.tasksControl.cbVerifyRawBlocks.Checked = false;
        }

        static void _UiMgr_LayoutsChanged(object sender, ValueEventArgs<IEnumerable<XmlUiLayoutRoot>> e)
        {
            var ui = new KzjHackUiSettings { UiLayouts = new List<XmlUiLayoutRoot>(e.Value) };
            ui.Save();
        }

        public static void WriteLine() => _logControl.WriteLine("");
        public static void WriteLine(string format, params object[] args) => _logControl.WriteLine(format, args);
        public static void Write(string format, params object[] args) => _logControl.Write(format, args);

        public static void WriteMessageLine(string format, params object[] args) 
        {
            try {
                var tb = _tbMessage;
                if (tb == null) return;
                if (tb.InvokeRequired) {
                    tb.BeginInvoke(new Action<string, object[]>(WriteLine), format, args);
                    return;
                }
                string line = string.Format(format, args);
                tb.AppendText(string.Concat(line, "\r\n"));
                if (tb.TextLength > 20000) {
                    tb.Text = tb.Text.Substring(10000);
                }
            }
            catch { }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            _S = KzjHackSettings.Load();
            var ui = KzjHackUiSettings.Load();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var f = _MainForm = new MainForm(ui.UiLayouts);
            _MainForm.Load += _MainForm_Load;
            Application.Run(f);
        }
    }
}
