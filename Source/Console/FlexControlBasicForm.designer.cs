namespace PowerSDR
{
    partial class FlexControlBasicForm
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
            if (fc_interface == null) return;
            fc_interface.Cleanup();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlexControlBasicForm));
            this.picFlexControl = new System.Windows.Forms.PictureBox();
            this.lblAUX1 = new System.Windows.Forms.Label();
            this.lblAUX3 = new System.Windows.Forms.Label();
            this.lblAUX2 = new System.Windows.Forms.Label();
            this.lblKnob = new System.Windows.Forms.Label();
            this.chkAutoDetect = new System.Windows.Forms.CheckBoxTS();
            this.radModeAdvanced = new System.Windows.Forms.RadioButtonTS();
            this.radModeBasic = new System.Windows.Forms.RadioButtonTS();
            this.comboButtonKnob = new System.Windows.Forms.ComboBoxTS();
            this.comboButtonRight = new System.Windows.Forms.ComboBoxTS();
            this.comboButtonMid = new System.Windows.Forms.ComboBoxTS();
            this.comboButtonLeft = new System.Windows.Forms.ComboBoxTS();
            ((System.ComponentModel.ISupportInitialize)(this.picFlexControl)).BeginInit();
            this.SuspendLayout();
            // 
            // picFlexControl
            // 
            this.picFlexControl.Image = ((System.Drawing.Image)(resources.GetObject("picFlexControl.Image")));
            this.picFlexControl.Location = new System.Drawing.Point(111, 55);
            this.picFlexControl.Name = "picFlexControl";
            this.picFlexControl.Size = new System.Drawing.Size(400, 453);
            this.picFlexControl.TabIndex = 0;
            this.picFlexControl.TabStop = false;
            // 
            // lblAUX1
            // 
            this.lblAUX1.AutoSize = true;
            this.lblAUX1.Location = new System.Drawing.Point(24, 132);
            this.lblAUX1.Name = "lblAUX1";
            this.lblAUX1.Size = new System.Drawing.Size(71, 13);
            this.lblAUX1.TabIndex = 9;
            this.lblAUX1.Text = "AUX 1 Action";
            // 
            // lblAUX3
            // 
            this.lblAUX3.AutoSize = true;
            this.lblAUX3.Location = new System.Drawing.Point(526, 132);
            this.lblAUX3.Name = "lblAUX3";
            this.lblAUX3.Size = new System.Drawing.Size(71, 13);
            this.lblAUX3.TabIndex = 10;
            this.lblAUX3.Text = "AUX 3 Action";
            // 
            // lblAUX2
            // 
            this.lblAUX2.AutoSize = true;
            this.lblAUX2.Location = new System.Drawing.Point(267, 11);
            this.lblAUX2.Name = "lblAUX2";
            this.lblAUX2.Size = new System.Drawing.Size(71, 13);
            this.lblAUX2.TabIndex = 11;
            this.lblAUX2.Text = "AUX 2 Action";
            // 
            // lblKnob
            // 
            this.lblKnob.AutoSize = true;
            this.lblKnob.Location = new System.Drawing.Point(12, 278);
            this.lblKnob.Name = "lblKnob";
            this.lblKnob.Size = new System.Drawing.Size(94, 13);
            this.lblKnob.TabIndex = 12;
            this.lblKnob.Text = "Knob Press Action";
            // 
            // chkAutoDetect
            // 
            this.chkAutoDetect.AutoSize = true;
            this.chkAutoDetect.Checked = true;
            this.chkAutoDetect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoDetect.Image = null;
            this.chkAutoDetect.Location = new System.Drawing.Point(12, 481);
            this.chkAutoDetect.Name = "chkAutoDetect";
            this.chkAutoDetect.Size = new System.Drawing.Size(83, 17);
            this.chkAutoDetect.TabIndex = 8;
            this.chkAutoDetect.Text = "Auto-Detect";
            this.chkAutoDetect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkAutoDetect.UseVisualStyleBackColor = true;
            this.chkAutoDetect.CheckedChanged += new System.EventHandler(this.chkAutoDetect_CheckedChanged);
            // 
            // radModeAdvanced
            // 
            this.radModeAdvanced.AutoSize = true;
            this.radModeAdvanced.Image = null;
            this.radModeAdvanced.Location = new System.Drawing.Point(90, 9);
            this.radModeAdvanced.Name = "radModeAdvanced";
            this.radModeAdvanced.Size = new System.Drawing.Size(74, 17);
            this.radModeAdvanced.TabIndex = 7;
            this.radModeAdvanced.Text = "Advanced";
            this.radModeAdvanced.UseVisualStyleBackColor = true;
            this.radModeAdvanced.CheckedChanged += new System.EventHandler(this.radModeAdvanced_CheckedChanged);
            // 
            // radModeBasic
            // 
            this.radModeBasic.AutoSize = true;
            this.radModeBasic.Checked = true;
            this.radModeBasic.Image = null;
            this.radModeBasic.Location = new System.Drawing.Point(12, 9);
            this.radModeBasic.Name = "radModeBasic";
            this.radModeBasic.Size = new System.Drawing.Size(51, 17);
            this.radModeBasic.TabIndex = 6;
            this.radModeBasic.TabStop = true;
            this.radModeBasic.Text = "Basic";
            this.radModeBasic.UseVisualStyleBackColor = true;
            this.radModeBasic.CheckedChanged += new System.EventHandler(this.radModeBasic_CheckedChanged);
            // 
            // comboButtonKnob
            // 
            this.comboButtonKnob.Enabled = false;
            this.comboButtonKnob.FormattingEnabled = true;
            this.comboButtonKnob.Location = new System.Drawing.Point(12, 295);
            this.comboButtonKnob.Name = "comboButtonKnob";
            this.comboButtonKnob.Size = new System.Drawing.Size(93, 21);
            this.comboButtonKnob.TabIndex = 4;
            this.comboButtonKnob.SelectedIndexChanged += new System.EventHandler(this.comboButtonKnob_SelectedIndexChanged);
            // 
            // comboButtonRight
            // 
            this.comboButtonRight.Enabled = false;
            this.comboButtonRight.FormattingEnabled = true;
            this.comboButtonRight.Location = new System.Drawing.Point(517, 150);
            this.comboButtonRight.Name = "comboButtonRight";
            this.comboButtonRight.Size = new System.Drawing.Size(93, 21);
            this.comboButtonRight.TabIndex = 3;
            this.comboButtonRight.SelectedIndexChanged += new System.EventHandler(this.comboButtonRight_SelectedIndexChanged);
            // 
            // comboButtonMid
            // 
            this.comboButtonMid.Enabled = false;
            this.comboButtonMid.FormattingEnabled = true;
            this.comboButtonMid.Location = new System.Drawing.Point(258, 28);
            this.comboButtonMid.Name = "comboButtonMid";
            this.comboButtonMid.Size = new System.Drawing.Size(93, 21);
            this.comboButtonMid.TabIndex = 2;
            this.comboButtonMid.SelectedIndexChanged += new System.EventHandler(this.comboButtonMid_SelectedIndexChanged);
            // 
            // comboButtonLeft
            // 
            this.comboButtonLeft.Enabled = false;
            this.comboButtonLeft.FormattingEnabled = true;
            this.comboButtonLeft.Location = new System.Drawing.Point(12, 150);
            this.comboButtonLeft.Name = "comboButtonLeft";
            this.comboButtonLeft.Size = new System.Drawing.Size(93, 21);
            this.comboButtonLeft.TabIndex = 1;
            this.comboButtonLeft.SelectedIndexChanged += new System.EventHandler(this.comboButtonLeft_SelectedIndexChanged);
            // 
            // FlexControlBasicForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 515);
            this.Controls.Add(this.lblKnob);
            this.Controls.Add(this.lblAUX2);
            this.Controls.Add(this.lblAUX3);
            this.Controls.Add(this.lblAUX1);
            this.Controls.Add(this.chkAutoDetect);
            this.Controls.Add(this.radModeAdvanced);
            this.Controls.Add(this.radModeBasic);
            this.Controls.Add(this.comboButtonKnob);
            this.Controls.Add(this.comboButtonRight);
            this.Controls.Add(this.comboButtonMid);
            this.Controls.Add(this.comboButtonLeft);
            this.Controls.Add(this.picFlexControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FlexControlBasicForm";
            this.Text = "FlexControl - Basic";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FlexControlSimpleForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.picFlexControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picFlexControl;
        private System.Windows.Forms.ComboBoxTS comboButtonLeft;
        private System.Windows.Forms.ComboBoxTS comboButtonMid;
        private System.Windows.Forms.ComboBoxTS comboButtonRight;
        private System.Windows.Forms.ComboBoxTS comboButtonKnob;
        private System.Windows.Forms.RadioButtonTS radModeBasic;
        private System.Windows.Forms.RadioButtonTS radModeAdvanced;
        private System.Windows.Forms.CheckBoxTS chkAutoDetect;
        private System.Windows.Forms.Label lblAUX1;
        private System.Windows.Forms.Label lblAUX3;
        private System.Windows.Forms.Label lblAUX2;
        private System.Windows.Forms.Label lblKnob;
    }
}