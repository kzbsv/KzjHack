using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KZJ;

namespace KzjHack
{
    public partial class MainForm : Form
    {
        internal UiLayoutManager _UiMgr;
        internal KZJ.LogControl logControl;
        internal KZJ.OkCancelControl okCancelControl;

        public MainForm(IEnumerable<XmlUiLayoutRoot> layouts) : this() {

            this.logControl = new KZJ.LogControl();
            this.okCancelControl = new KZJ.OkCancelControl();

            // 
            // logControl
            // 
            this.tabLog.Controls.Add(this.logControl);
            this.logControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logControl.Location = new System.Drawing.Point(3, 3);
            this.logControl.Name = "logControl";
            this.logControl.Size = new System.Drawing.Size(946, 578);
            this.logControl.TabIndex = 0;

            // 
            // okCancelControl
            // 
            this.tabOkCancel.Controls.Add(this.okCancelControl);
            this.okCancelControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.okCancelControl.Location = new System.Drawing.Point(3, 3);
            this.okCancelControl.Name = "okCancelControl";
            this.okCancelControl.Prompt = "";
            this.okCancelControl.Size = new System.Drawing.Size(946, 578);
            this.okCancelControl.TabIndex = 0;
            this.okCancelControl.Title = "";

            _UiMgr = new UiLayoutManager {
                MainForm = this,
                Root = UiRootPanel,
                ButtonTemplate = buttonTabs,
                TabsTemplate = tabControl,
                AllTabs = tabControl.TabPages.AsEnumerable().ToList(),
            };
            _UiMgr.Layouts = layouts; 

            UiRootPanel.Controls.Clear();

            _UiMgr.ActivateLayout("Default");
        }
    }
}
