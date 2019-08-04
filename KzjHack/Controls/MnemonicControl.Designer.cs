namespace KzjHack.Controls
{
    partial class MnemonicControl
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
            this.tbWords = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rbDigitsBase10 = new System.Windows.Forms.RadioButton();
            this.buttonRandom = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbHex = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbDigits = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.rbDigitsBase6 = new System.Windows.Forms.RadioButton();
            this.rbBitLength256 = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.rbBitLength128 = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelMessage = new System.Windows.Forms.Label();
            this.buttonClear = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbWords
            // 
            this.tbWords.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbWords.Location = new System.Drawing.Point(50, 3);
            this.tbWords.Name = "tbWords";
            this.tbWords.Size = new System.Drawing.Size(320, 20);
            this.tbWords.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Words:";
            // 
            // rbDigitsBase10
            // 
            this.rbDigitsBase10.AutoSize = true;
            this.rbDigitsBase10.Checked = true;
            this.rbDigitsBase10.Location = new System.Drawing.Point(75, 3);
            this.rbDigitsBase10.Name = "rbDigitsBase10";
            this.rbDigitsBase10.Size = new System.Drawing.Size(46, 17);
            this.rbDigitsBase10.TabIndex = 2;
            this.rbDigitsBase10.TabStop = true;
            this.rbDigitsBase10.Text = "0...9";
            this.rbDigitsBase10.UseVisualStyleBackColor = true;
            // 
            // buttonRandom
            // 
            this.buttonRandom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRandom.Location = new System.Drawing.Point(376, 3);
            this.buttonRandom.Name = "buttonRandom";
            this.buttonRandom.Size = new System.Drawing.Size(75, 72);
            this.buttonRandom.TabIndex = 4;
            this.buttonRandom.Text = "Random";
            this.buttonRandom.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Hex:";
            // 
            // tbHex
            // 
            this.tbHex.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbHex.Location = new System.Drawing.Point(50, 29);
            this.tbHex.Name = "tbHex";
            this.tbHex.Size = new System.Drawing.Size(320, 20);
            this.tbHex.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Digits:";
            // 
            // tbDigits
            // 
            this.tbDigits.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDigits.Location = new System.Drawing.Point(50, 55);
            this.tbDigits.Name = "tbDigits";
            this.tbDigits.Size = new System.Drawing.Size(320, 20);
            this.tbDigits.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Digit Range:";
            // 
            // rbDigitsBase6
            // 
            this.rbDigitsBase6.AutoSize = true;
            this.rbDigitsBase6.Location = new System.Drawing.Point(127, 3);
            this.rbDigitsBase6.Name = "rbDigitsBase6";
            this.rbDigitsBase6.Size = new System.Drawing.Size(46, 17);
            this.rbDigitsBase6.TabIndex = 10;
            this.rbDigitsBase6.Text = "1...6";
            this.rbDigitsBase6.UseVisualStyleBackColor = true;
            // 
            // rbBitLength256
            // 
            this.rbBitLength256.AutoSize = true;
            this.rbBitLength256.Location = new System.Drawing.Point(118, 3);
            this.rbBitLength256.Name = "rbBitLength256";
            this.rbBitLength256.Size = new System.Drawing.Size(43, 17);
            this.rbBitLength256.TabIndex = 13;
            this.rbBitLength256.Text = "256";
            this.rbBitLength256.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(2, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Bit Length:";
            // 
            // rbBitLength128
            // 
            this.rbBitLength128.AutoSize = true;
            this.rbBitLength128.Checked = true;
            this.rbBitLength128.Location = new System.Drawing.Point(66, 3);
            this.rbBitLength128.Name = "rbBitLength128";
            this.rbBitLength128.Size = new System.Drawing.Size(43, 17);
            this.rbBitLength128.TabIndex = 11;
            this.rbBitLength128.TabStop = true;
            this.rbBitLength128.Text = "128";
            this.rbBitLength128.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbDigitsBase6);
            this.panel1.Controls.Add(this.rbDigitsBase10);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Location = new System.Drawing.Point(3, 81);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(176, 24);
            this.panel1.TabIndex = 14;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rbBitLength256);
            this.panel2.Controls.Add(this.rbBitLength128);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Location = new System.Drawing.Point(202, 81);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(168, 24);
            this.panel2.TabIndex = 15;
            // 
            // labelMessage
            // 
            this.labelMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMessage.Location = new System.Drawing.Point(8, 117);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(443, 22);
            this.labelMessage.TabIndex = 16;
            this.labelMessage.Text = "Enter a value in one of the boxes or click Random.";
            // 
            // buttonClear
            // 
            this.buttonClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClear.Location = new System.Drawing.Point(376, 78);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 27);
            this.buttonClear.TabIndex = 17;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            // 
            // MnemonicControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.labelMessage);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbDigits);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbHex);
            this.Controls.Add(this.buttonRandom);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbWords);
            this.MinimumSize = new System.Drawing.Size(458, 150);
            this.Name = "MnemonicControl";
            this.Size = new System.Drawing.Size(458, 150);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Button buttonRandom;
        private System.Windows.Forms.Label label2;
        internal System.Windows.Forms.TextBox tbHex;
        private System.Windows.Forms.Label label3;
        internal System.Windows.Forms.TextBox tbDigits;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        internal System.Windows.Forms.TextBox tbWords;
        internal System.Windows.Forms.RadioButton rbDigitsBase10;
        internal System.Windows.Forms.RadioButton rbDigitsBase6;
        internal System.Windows.Forms.RadioButton rbBitLength256;
        internal System.Windows.Forms.RadioButton rbBitLength128;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        internal System.Windows.Forms.Label labelMessage;
        internal System.Windows.Forms.Button buttonClear;
    }
}
