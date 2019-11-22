namespace KzjHack
{
    partial class TasksControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbZMQ = new System.Windows.Forms.CheckBox();
            this.cbBlockHeaders = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbVerifyRawBlocks = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cbZMQ
            // 
            this.cbZMQ.AutoSize = true;
            this.cbZMQ.Location = new System.Drawing.Point(3, 46);
            this.cbZMQ.Name = "cbZMQ";
            this.cbZMQ.Size = new System.Drawing.Size(86, 17);
            this.cbZMQ.TabIndex = 0;
            this.cbZMQ.Text = "Enable ZMQ";
            this.cbZMQ.UseVisualStyleBackColor = true;
            // 
            // cbBlockHeaders
            // 
            this.cbBlockHeaders.AutoSize = true;
            this.cbBlockHeaders.Location = new System.Drawing.Point(3, 23);
            this.cbBlockHeaders.Name = "cbBlockHeaders";
            this.cbBlockHeaders.Size = new System.Drawing.Size(126, 17);
            this.cbBlockHeaders.TabIndex = 1;
            this.cbBlockHeaders.Text = "Fetch Recent Blocks";
            this.cbBlockHeaders.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Tasks";
            // 
            // cbVerifyRawBlocks
            // 
            this.cbVerifyRawBlocks.AutoSize = true;
            this.cbVerifyRawBlocks.Location = new System.Drawing.Point(3, 69);
            this.cbVerifyRawBlocks.Name = "cbVerifyRawBlocks";
            this.cbVerifyRawBlocks.Size = new System.Drawing.Size(109, 17);
            this.cbVerifyRawBlocks.TabIndex = 3;
            this.cbVerifyRawBlocks.Text = "Verify RawBlocks";
            this.cbVerifyRawBlocks.UseVisualStyleBackColor = true;
            // 
            // TasksControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbVerifyRawBlocks);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbBlockHeaders);
            this.Controls.Add(this.cbZMQ);
            this.Name = "TasksControl";
            this.Size = new System.Drawing.Size(154, 177);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.CheckBox cbZMQ;
        internal System.Windows.Forms.CheckBox cbBlockHeaders;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.CheckBox cbVerifyRawBlocks;
    }
}
