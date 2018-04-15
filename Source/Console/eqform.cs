//=================================================================
// eqform.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2003-2013  FlexRadio Systems
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//
// You may contact us via email at: gpl@flexradio.com.
// Paper mail may be sent to: 
//    FlexRadio Systems
//    4616 W. Howard Lane  Suite 1-150
//    Austin, TX 78728
//    USA
//=================================================================

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PowerSDR
{
	/// <summary>
	/// Summary description for EQForm.
	/// </summary>
	public class EQForm : System.Windows.Forms.Form
	{
		#region Variable Declaration

		private Console console;
		private System.Windows.Forms.GroupBoxTS grpRXEQ;
		private System.Windows.Forms.GroupBoxTS grpTXEQ;
		private System.Windows.Forms.TrackBarTS tbRXEQ1;
		private System.Windows.Forms.TrackBarTS tbRXEQ2;
		private System.Windows.Forms.TrackBarTS tbRXEQ3;
		private System.Windows.Forms.TrackBarTS tbTXEQ3;
		private System.Windows.Forms.TrackBarTS tbTXEQ1;
		private System.Windows.Forms.TrackBarTS tbTXEQ2;
		private System.Windows.Forms.LabelTS lblRXEQ0dB;
		private System.Windows.Forms.LabelTS lblTXEQ0dB;
		private System.Windows.Forms.LabelTS lblRXEQ1;
		private System.Windows.Forms.LabelTS lblRXEQ2;
		private System.Windows.Forms.LabelTS lblRXEQ3;
		private System.Windows.Forms.LabelTS lblTXEQ3;
		private System.Windows.Forms.LabelTS lblTXEQ2;
		private System.Windows.Forms.LabelTS lblTXEQ1;
		private System.Windows.Forms.LabelTS lblRXEQPreamp;
		private System.Windows.Forms.LabelTS lblTXEQPreamp;
		private System.Windows.Forms.CheckBoxTS chkTXEQEnabled;
		private System.Windows.Forms.TrackBarTS tbRXEQPreamp;
		private System.Windows.Forms.TrackBarTS tbTXEQPreamp;
		private System.Windows.Forms.CheckBoxTS chkRXEQEnabled;
		private System.Windows.Forms.PictureBox picRXEQ;
		private System.Windows.Forms.PictureBox picTXEQ;
		private System.Windows.Forms.ButtonTS btnTXEQReset;
		private System.Windows.Forms.ButtonTS btnRXEQReset;
		private System.Windows.Forms.LabelTS lblRXEQ15db;
		private System.Windows.Forms.LabelTS lblTXEQ15db;
		private System.Windows.Forms.LabelTS lblRXEQminus12db;
		private System.Windows.Forms.LabelTS lblTXEQminus12db;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.CheckBoxTS chkTXEQ160Notch;
		private System.Windows.Forms.TrackBarTS tbRXEQ4;
		private System.Windows.Forms.TrackBarTS tbRXEQ5;
		private System.Windows.Forms.TrackBarTS tbRXEQ6;
		private System.Windows.Forms.TrackBarTS tbRXEQ7;
		private System.Windows.Forms.TrackBarTS tbRXEQ8;
		private System.Windows.Forms.TrackBarTS tbRXEQ9;
		private System.Windows.Forms.TrackBarTS tbRXEQ10;
		private System.Windows.Forms.LabelTS lblRXEQ15db2;
		private System.Windows.Forms.LabelTS lblRXEQ0dB2;
		private System.Windows.Forms.LabelTS lblRXEQminus12db2;
		private System.Windows.Forms.LabelTS lblTXEQ15db2;
		private System.Windows.Forms.LabelTS lblTXEQ0dB2;
		private System.Windows.Forms.LabelTS lblTXEQminus12db2;
		private System.Windows.Forms.RadioButtonTS rad3Band;
		private System.Windows.Forms.RadioButtonTS rad10Band;
		private System.Windows.Forms.TrackBarTS tbTXEQ4;
		private System.Windows.Forms.TrackBarTS tbTXEQ5;
		private System.Windows.Forms.TrackBarTS tbTXEQ6;
		private System.Windows.Forms.TrackBarTS tbTXEQ7;
		private System.Windows.Forms.TrackBarTS tbTXEQ8;
		private System.Windows.Forms.TrackBarTS tbTXEQ9;
		private System.Windows.Forms.TrackBarTS tbTXEQ10;
		private System.Windows.Forms.LabelTS lblRXEQ4;
		private System.Windows.Forms.LabelTS lblRXEQ5;
		private System.Windows.Forms.LabelTS lblRXEQ6;
		private System.Windows.Forms.LabelTS lblRXEQ7;
		private System.Windows.Forms.LabelTS lblRXEQ8;
		private System.Windows.Forms.LabelTS lblRXEQ9;
		private System.Windows.Forms.LabelTS lblRXEQ10;
		private System.Windows.Forms.LabelTS lblTXEQ4;
		private System.Windows.Forms.LabelTS lblTXEQ5;
		private System.Windows.Forms.LabelTS lblTXEQ6;
		private System.Windows.Forms.LabelTS lblTXEQ7;
		private System.Windows.Forms.LabelTS lblTXEQ8;
		private System.Windows.Forms.LabelTS lblTXEQ9;
		private System.Windows.Forms.LabelTS lblTXEQ10;
        private GroupBoxTS grpTXEQ28;
        private LabelTS lbl2821;
        private LabelTS lbl2828;
        private LabelTS lbl2827;
        private TrackBarTS tbTX28EQ28;
        private TrackBarTS tbTX28EQ27;
        private LabelTS lbl2826;
        private TrackBarTS tbTX28EQ26;
        private TrackBarTS tbTX28EQ25;
        private LabelTS lbl2825;
        private LabelTS lbl2824;
        private LabelTS lbl2823;
        private TrackBarTS tbTX28EQ24;
        private TrackBarTS tbTX28EQ23;
        private LabelTS lbl2822;
        private TrackBarTS tbTX28EQ22;
        private LabelTS lbl2820;
        private TrackBarTS tbTX28EQ21;
        private TrackBarTS tbTX28EQ20;
        private LabelTS lbl2819;
        private TrackBarTS tbTX28EQ19;
        private TrackBarTS tbTX28EQ18;
        private LabelTS lbl2818;
        private LabelTS lbl2817;
        private LabelTS lbl2816;
        private TrackBarTS tbTX28EQ17;
        private TrackBarTS tbTX28EQ16;
        private LabelTS lbl2815;
        private TrackBarTS tbTX28EQ15;
        private LabelTS lbl287;
        private LabelTS lbl2814;
        private LabelTS lbl2813;
        private TrackBarTS tbTX28EQ14;
        private TrackBarTS tbTX28EQ13;
        private LabelTS lbl2812;
        private TrackBarTS tbTX28EQ12;
        private TrackBarTS tbTX28EQ11;
        private LabelTS lbl2811;
        private LabelTS lbl2810;
        private LabelTS lbl289;
        private TrackBarTS tbTX28EQ10;
        private TrackBarTS tbTX28EQ9;
        private LabelTS lbl288;
        private TrackBarTS tbTX28EQ8;
        private LabelTS lbl286;
        private TrackBarTS tbTX28EQ7;
        private TrackBarTS tbTX28EQ6;
        private LabelTS lbl285;
        private TrackBarTS tbTX28EQ5;
        private TrackBarTS tbTX28EQ4;
        private LabelTS lbl284;
        private LabelTS lbl283;
        private LabelTS lbl282;
        private TrackBarTS tbTX28EQ3;
        private TrackBarTS tbTX28EQ2;
        private LabelTS lbl281;
        private TrackBarTS tbTX28EQ1;
        private RadioButtonTS rad28Band;
        private ButtonTS btnTXEQReset28;
        private LabelTS labelTS3;
        private LabelTS labelTS2;
        private LabelTS labelTS1;
        private LabelTS labelTS6;
        private LabelTS labelTS5;
        private LabelTS labelTS4;
        private System.ComponentModel.IContainer components;
		
		#endregion

		#region Constructor and Destructor

		public EQForm(Console c)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			console = c;
			Common.RestoreForm(this, "EQForm", false);
            
			tbRXEQ_Scroll(this, EventArgs.Empty);
			tbTXEQ_Scroll(this, EventArgs.Empty);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EQForm));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblRXEQ9 = new System.Windows.Forms.LabelTS();
            this.lblRXEQ5 = new System.Windows.Forms.LabelTS();
            this.lblRXEQ1 = new System.Windows.Forms.LabelTS();
            this.lblTXEQ10 = new System.Windows.Forms.LabelTS();
            this.lblTXEQ7 = new System.Windows.Forms.LabelTS();
            this.lblTXEQ8 = new System.Windows.Forms.LabelTS();
            this.lblTXEQ9 = new System.Windows.Forms.LabelTS();
            this.lblTXEQ4 = new System.Windows.Forms.LabelTS();
            this.lblTXEQ5 = new System.Windows.Forms.LabelTS();
            this.lblTXEQ6 = new System.Windows.Forms.LabelTS();
            this.lblTXEQ1 = new System.Windows.Forms.LabelTS();
            this.lblTXEQ2 = new System.Windows.Forms.LabelTS();
            this.lblTXEQ3 = new System.Windows.Forms.LabelTS();
            this.rad10Band = new System.Windows.Forms.RadioButtonTS();
            this.rad3Band = new System.Windows.Forms.RadioButtonTS();
            this.grpRXEQ = new System.Windows.Forms.GroupBoxTS();
            this.lblRXEQ15db2 = new System.Windows.Forms.LabelTS();
            this.lblRXEQ0dB2 = new System.Windows.Forms.LabelTS();
            this.lblRXEQminus12db2 = new System.Windows.Forms.LabelTS();
            this.lblRXEQ10 = new System.Windows.Forms.LabelTS();
            this.tbRXEQ10 = new System.Windows.Forms.TrackBarTS();
            this.lblRXEQ7 = new System.Windows.Forms.LabelTS();
            this.lblRXEQ8 = new System.Windows.Forms.LabelTS();
            this.tbRXEQ7 = new System.Windows.Forms.TrackBarTS();
            this.tbRXEQ8 = new System.Windows.Forms.TrackBarTS();
            this.tbRXEQ9 = new System.Windows.Forms.TrackBarTS();
            this.tbRXEQ4 = new System.Windows.Forms.TrackBarTS();
            this.tbRXEQ5 = new System.Windows.Forms.TrackBarTS();
            this.tbRXEQ6 = new System.Windows.Forms.TrackBarTS();
            this.lblRXEQ4 = new System.Windows.Forms.LabelTS();
            this.lblRXEQ6 = new System.Windows.Forms.LabelTS();
            this.picRXEQ = new System.Windows.Forms.PictureBox();
            this.btnRXEQReset = new System.Windows.Forms.ButtonTS();
            this.chkRXEQEnabled = new System.Windows.Forms.CheckBoxTS();
            this.tbRXEQ1 = new System.Windows.Forms.TrackBarTS();
            this.tbRXEQ2 = new System.Windows.Forms.TrackBarTS();
            this.tbRXEQ3 = new System.Windows.Forms.TrackBarTS();
            this.lblRXEQ2 = new System.Windows.Forms.LabelTS();
            this.lblRXEQ3 = new System.Windows.Forms.LabelTS();
            this.lblRXEQPreamp = new System.Windows.Forms.LabelTS();
            this.tbRXEQPreamp = new System.Windows.Forms.TrackBarTS();
            this.lblRXEQ15db = new System.Windows.Forms.LabelTS();
            this.lblRXEQ0dB = new System.Windows.Forms.LabelTS();
            this.lblRXEQminus12db = new System.Windows.Forms.LabelTS();
            this.grpTXEQ = new System.Windows.Forms.GroupBoxTS();
            this.lblTXEQ15db2 = new System.Windows.Forms.LabelTS();
            this.lblTXEQ0dB2 = new System.Windows.Forms.LabelTS();
            this.lblTXEQminus12db2 = new System.Windows.Forms.LabelTS();
            this.tbTXEQ10 = new System.Windows.Forms.TrackBarTS();
            this.tbTXEQ7 = new System.Windows.Forms.TrackBarTS();
            this.tbTXEQ8 = new System.Windows.Forms.TrackBarTS();
            this.tbTXEQ9 = new System.Windows.Forms.TrackBarTS();
            this.tbTXEQ4 = new System.Windows.Forms.TrackBarTS();
            this.tbTXEQ5 = new System.Windows.Forms.TrackBarTS();
            this.tbTXEQ6 = new System.Windows.Forms.TrackBarTS();
            this.chkTXEQ160Notch = new System.Windows.Forms.CheckBoxTS();
            this.picTXEQ = new System.Windows.Forms.PictureBox();
            this.btnTXEQReset = new System.Windows.Forms.ButtonTS();
            this.chkTXEQEnabled = new System.Windows.Forms.CheckBoxTS();
            this.tbTXEQ1 = new System.Windows.Forms.TrackBarTS();
            this.tbTXEQ2 = new System.Windows.Forms.TrackBarTS();
            this.tbTXEQ3 = new System.Windows.Forms.TrackBarTS();
            this.lblTXEQPreamp = new System.Windows.Forms.LabelTS();
            this.tbTXEQPreamp = new System.Windows.Forms.TrackBarTS();
            this.lblTXEQ15db = new System.Windows.Forms.LabelTS();
            this.lblTXEQ0dB = new System.Windows.Forms.LabelTS();
            this.lblTXEQminus12db = new System.Windows.Forms.LabelTS();
            this.grpTXEQ28 = new System.Windows.Forms.GroupBoxTS();
            this.labelTS6 = new System.Windows.Forms.LabelTS();
            this.labelTS5 = new System.Windows.Forms.LabelTS();
            this.labelTS4 = new System.Windows.Forms.LabelTS();
            this.labelTS3 = new System.Windows.Forms.LabelTS();
            this.labelTS2 = new System.Windows.Forms.LabelTS();
            this.labelTS1 = new System.Windows.Forms.LabelTS();
            this.lbl2815 = new System.Windows.Forms.LabelTS();
            this.lbl2814 = new System.Windows.Forms.LabelTS();
            this.btnTXEQReset28 = new System.Windows.Forms.ButtonTS();
            this.lbl2825 = new System.Windows.Forms.LabelTS();
            this.lbl2821 = new System.Windows.Forms.LabelTS();
            this.lbl2828 = new System.Windows.Forms.LabelTS();
            this.lbl2827 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ28 = new System.Windows.Forms.TrackBarTS();
            this.tbTX28EQ27 = new System.Windows.Forms.TrackBarTS();
            this.lbl2826 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ26 = new System.Windows.Forms.TrackBarTS();
            this.tbTX28EQ25 = new System.Windows.Forms.TrackBarTS();
            this.lbl2824 = new System.Windows.Forms.LabelTS();
            this.lbl2823 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ24 = new System.Windows.Forms.TrackBarTS();
            this.tbTX28EQ23 = new System.Windows.Forms.TrackBarTS();
            this.lbl2822 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ22 = new System.Windows.Forms.TrackBarTS();
            this.lbl2820 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ21 = new System.Windows.Forms.TrackBarTS();
            this.tbTX28EQ20 = new System.Windows.Forms.TrackBarTS();
            this.lbl2819 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ19 = new System.Windows.Forms.TrackBarTS();
            this.tbTX28EQ18 = new System.Windows.Forms.TrackBarTS();
            this.lbl2818 = new System.Windows.Forms.LabelTS();
            this.lbl2817 = new System.Windows.Forms.LabelTS();
            this.lbl2816 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ17 = new System.Windows.Forms.TrackBarTS();
            this.tbTX28EQ16 = new System.Windows.Forms.TrackBarTS();
            this.tbTX28EQ15 = new System.Windows.Forms.TrackBarTS();
            this.lbl287 = new System.Windows.Forms.LabelTS();
            this.lbl2813 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ14 = new System.Windows.Forms.TrackBarTS();
            this.tbTX28EQ13 = new System.Windows.Forms.TrackBarTS();
            this.lbl2812 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ12 = new System.Windows.Forms.TrackBarTS();
            this.tbTX28EQ11 = new System.Windows.Forms.TrackBarTS();
            this.lbl2811 = new System.Windows.Forms.LabelTS();
            this.lbl2810 = new System.Windows.Forms.LabelTS();
            this.lbl289 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ10 = new System.Windows.Forms.TrackBarTS();
            this.tbTX28EQ9 = new System.Windows.Forms.TrackBarTS();
            this.lbl288 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ8 = new System.Windows.Forms.TrackBarTS();
            this.lbl286 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ7 = new System.Windows.Forms.TrackBarTS();
            this.tbTX28EQ6 = new System.Windows.Forms.TrackBarTS();
            this.lbl285 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ5 = new System.Windows.Forms.TrackBarTS();
            this.tbTX28EQ4 = new System.Windows.Forms.TrackBarTS();
            this.lbl284 = new System.Windows.Forms.LabelTS();
            this.lbl283 = new System.Windows.Forms.LabelTS();
            this.lbl282 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ3 = new System.Windows.Forms.TrackBarTS();
            this.tbTX28EQ2 = new System.Windows.Forms.TrackBarTS();
            this.lbl281 = new System.Windows.Forms.LabelTS();
            this.tbTX28EQ1 = new System.Windows.Forms.TrackBarTS();
            this.rad28Band = new System.Windows.Forms.RadioButtonTS();
            this.grpRXEQ.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRXEQ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQPreamp)).BeginInit();
            this.grpTXEQ.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTXEQ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQPreamp)).BeginInit();
            this.grpTXEQ28.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ28)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ27)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ26)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ25)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ24)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ23)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ22)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ21)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ20)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ19)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ18)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblRXEQ9
            // 
            this.lblRXEQ9.Image = null;
            this.lblRXEQ9.Location = new System.Drawing.Point(400, 56);
            this.lblRXEQ9.Name = "lblRXEQ9";
            this.lblRXEQ9.Size = new System.Drawing.Size(40, 16);
            this.lblRXEQ9.TabIndex = 123;
            this.lblRXEQ9.Text = "High";
            this.lblRXEQ9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblRXEQ9, "1500-6000Hz");
            // 
            // lblRXEQ5
            // 
            this.lblRXEQ5.Image = null;
            this.lblRXEQ5.Location = new System.Drawing.Point(240, 56);
            this.lblRXEQ5.Name = "lblRXEQ5";
            this.lblRXEQ5.Size = new System.Drawing.Size(40, 16);
            this.lblRXEQ5.TabIndex = 116;
            this.lblRXEQ5.Text = "Mid";
            this.lblRXEQ5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblRXEQ5, "400-1500Hz");
            // 
            // lblRXEQ1
            // 
            this.lblRXEQ1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRXEQ1.Image = null;
            this.lblRXEQ1.Location = new System.Drawing.Point(80, 56);
            this.lblRXEQ1.Name = "lblRXEQ1";
            this.lblRXEQ1.Size = new System.Drawing.Size(40, 16);
            this.lblRXEQ1.TabIndex = 43;
            this.lblRXEQ1.Text = "Low";
            this.lblRXEQ1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblRXEQ1, "0-400Hz");
            // 
            // lblTXEQ10
            // 
            this.lblTXEQ10.Image = null;
            this.lblTXEQ10.Location = new System.Drawing.Point(440, 56);
            this.lblTXEQ10.Name = "lblTXEQ10";
            this.lblTXEQ10.Size = new System.Drawing.Size(40, 16);
            this.lblTXEQ10.TabIndex = 127;
            this.lblTXEQ10.Text = "16K";
            this.lblTXEQ10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblTXEQ10, "1500-6000Hz");
            this.lblTXEQ10.Visible = false;
            // 
            // lblTXEQ7
            // 
            this.lblTXEQ7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTXEQ7.Image = null;
            this.lblTXEQ7.Location = new System.Drawing.Point(320, 56);
            this.lblTXEQ7.Name = "lblTXEQ7";
            this.lblTXEQ7.Size = new System.Drawing.Size(40, 16);
            this.lblTXEQ7.TabIndex = 123;
            this.lblTXEQ7.Text = "2K";
            this.lblTXEQ7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblTXEQ7, "0-400Hz");
            this.lblTXEQ7.Visible = false;
            // 
            // lblTXEQ8
            // 
            this.lblTXEQ8.Image = null;
            this.lblTXEQ8.Location = new System.Drawing.Point(360, 56);
            this.lblTXEQ8.Name = "lblTXEQ8";
            this.lblTXEQ8.Size = new System.Drawing.Size(40, 16);
            this.lblTXEQ8.TabIndex = 124;
            this.lblTXEQ8.Text = "4K";
            this.lblTXEQ8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblTXEQ8, "400-1500Hz");
            this.lblTXEQ8.Visible = false;
            // 
            // lblTXEQ9
            // 
            this.lblTXEQ9.Image = null;
            this.lblTXEQ9.Location = new System.Drawing.Point(400, 56);
            this.lblTXEQ9.Name = "lblTXEQ9";
            this.lblTXEQ9.Size = new System.Drawing.Size(40, 16);
            this.lblTXEQ9.TabIndex = 125;
            this.lblTXEQ9.Text = "High";
            this.lblTXEQ9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblTXEQ9, "1500-6000Hz");
            // 
            // lblTXEQ4
            // 
            this.lblTXEQ4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTXEQ4.Image = null;
            this.lblTXEQ4.Location = new System.Drawing.Point(200, 56);
            this.lblTXEQ4.Name = "lblTXEQ4";
            this.lblTXEQ4.Size = new System.Drawing.Size(40, 16);
            this.lblTXEQ4.TabIndex = 117;
            this.lblTXEQ4.Text = "250";
            this.lblTXEQ4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblTXEQ4, "0-400Hz");
            this.lblTXEQ4.Visible = false;
            // 
            // lblTXEQ5
            // 
            this.lblTXEQ5.Image = null;
            this.lblTXEQ5.Location = new System.Drawing.Point(240, 56);
            this.lblTXEQ5.Name = "lblTXEQ5";
            this.lblTXEQ5.Size = new System.Drawing.Size(40, 16);
            this.lblTXEQ5.TabIndex = 118;
            this.lblTXEQ5.Text = "Mid";
            this.lblTXEQ5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblTXEQ5, "400-1500Hz");
            // 
            // lblTXEQ6
            // 
            this.lblTXEQ6.Image = null;
            this.lblTXEQ6.Location = new System.Drawing.Point(280, 56);
            this.lblTXEQ6.Name = "lblTXEQ6";
            this.lblTXEQ6.Size = new System.Drawing.Size(40, 16);
            this.lblTXEQ6.TabIndex = 119;
            this.lblTXEQ6.Text = "1K";
            this.lblTXEQ6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblTXEQ6, "1500-6000Hz");
            this.lblTXEQ6.Visible = false;
            // 
            // lblTXEQ1
            // 
            this.lblTXEQ1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTXEQ1.Image = null;
            this.lblTXEQ1.Location = new System.Drawing.Point(80, 56);
            this.lblTXEQ1.Name = "lblTXEQ1";
            this.lblTXEQ1.Size = new System.Drawing.Size(40, 16);
            this.lblTXEQ1.TabIndex = 74;
            this.lblTXEQ1.Text = "Low";
            this.lblTXEQ1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblTXEQ1, "0-400Hz");
            // 
            // lblTXEQ2
            // 
            this.lblTXEQ2.Image = null;
            this.lblTXEQ2.Location = new System.Drawing.Point(120, 56);
            this.lblTXEQ2.Name = "lblTXEQ2";
            this.lblTXEQ2.Size = new System.Drawing.Size(40, 16);
            this.lblTXEQ2.TabIndex = 75;
            this.lblTXEQ2.Text = "63";
            this.lblTXEQ2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblTXEQ2, "400-1500Hz");
            this.lblTXEQ2.Visible = false;
            // 
            // lblTXEQ3
            // 
            this.lblTXEQ3.Image = null;
            this.lblTXEQ3.Location = new System.Drawing.Point(160, 56);
            this.lblTXEQ3.Name = "lblTXEQ3";
            this.lblTXEQ3.Size = new System.Drawing.Size(40, 16);
            this.lblTXEQ3.TabIndex = 76;
            this.lblTXEQ3.Text = "125";
            this.lblTXEQ3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.lblTXEQ3, "1500-6000Hz");
            this.lblTXEQ3.Visible = false;
            // 
            // rad10Band
            // 
            this.rad10Band.Image = null;
            this.rad10Band.Location = new System.Drawing.Point(144, 8);
            this.rad10Band.Name = "rad10Band";
            this.rad10Band.Size = new System.Drawing.Size(120, 24);
            this.rad10Band.TabIndex = 3;
            this.rad10Band.Text = "10-Band Equalizer";
            this.rad10Band.CheckedChanged += new System.EventHandler(this.rad10Band_CheckedChanged);
            // 
            // rad3Band
            // 
            this.rad3Band.Checked = true;
            this.rad3Band.Image = null;
            this.rad3Band.Location = new System.Drawing.Point(16, 8);
            this.rad3Band.Name = "rad3Band";
            this.rad3Band.Size = new System.Drawing.Size(120, 24);
            this.rad3Band.TabIndex = 2;
            this.rad3Band.TabStop = true;
            this.rad3Band.Text = "3-Band Equalizer";
            this.rad3Band.CheckedChanged += new System.EventHandler(this.rad3Band_CheckedChanged);
            // 
            // grpRXEQ
            // 
            this.grpRXEQ.Controls.Add(this.lblRXEQ15db2);
            this.grpRXEQ.Controls.Add(this.lblRXEQ0dB2);
            this.grpRXEQ.Controls.Add(this.lblRXEQminus12db2);
            this.grpRXEQ.Controls.Add(this.lblRXEQ10);
            this.grpRXEQ.Controls.Add(this.tbRXEQ10);
            this.grpRXEQ.Controls.Add(this.lblRXEQ7);
            this.grpRXEQ.Controls.Add(this.lblRXEQ8);
            this.grpRXEQ.Controls.Add(this.lblRXEQ9);
            this.grpRXEQ.Controls.Add(this.tbRXEQ7);
            this.grpRXEQ.Controls.Add(this.tbRXEQ8);
            this.grpRXEQ.Controls.Add(this.tbRXEQ9);
            this.grpRXEQ.Controls.Add(this.tbRXEQ4);
            this.grpRXEQ.Controls.Add(this.tbRXEQ5);
            this.grpRXEQ.Controls.Add(this.tbRXEQ6);
            this.grpRXEQ.Controls.Add(this.lblRXEQ4);
            this.grpRXEQ.Controls.Add(this.lblRXEQ5);
            this.grpRXEQ.Controls.Add(this.lblRXEQ6);
            this.grpRXEQ.Controls.Add(this.picRXEQ);
            this.grpRXEQ.Controls.Add(this.btnRXEQReset);
            this.grpRXEQ.Controls.Add(this.chkRXEQEnabled);
            this.grpRXEQ.Controls.Add(this.tbRXEQ1);
            this.grpRXEQ.Controls.Add(this.tbRXEQ2);
            this.grpRXEQ.Controls.Add(this.tbRXEQ3);
            this.grpRXEQ.Controls.Add(this.lblRXEQ1);
            this.grpRXEQ.Controls.Add(this.lblRXEQ2);
            this.grpRXEQ.Controls.Add(this.lblRXEQ3);
            this.grpRXEQ.Controls.Add(this.lblRXEQPreamp);
            this.grpRXEQ.Controls.Add(this.tbRXEQPreamp);
            this.grpRXEQ.Controls.Add(this.lblRXEQ15db);
            this.grpRXEQ.Controls.Add(this.lblRXEQ0dB);
            this.grpRXEQ.Controls.Add(this.lblRXEQminus12db);
            this.grpRXEQ.Location = new System.Drawing.Point(8, 40);
            this.grpRXEQ.Name = "grpRXEQ";
            this.grpRXEQ.Size = new System.Drawing.Size(528, 224);
            this.grpRXEQ.TabIndex = 1;
            this.grpRXEQ.TabStop = false;
            this.grpRXEQ.Text = "Receive Equalizer";
            // 
            // lblRXEQ15db2
            // 
            this.lblRXEQ15db2.Image = null;
            this.lblRXEQ15db2.Location = new System.Drawing.Point(483, 78);
            this.lblRXEQ15db2.Name = "lblRXEQ15db2";
            this.lblRXEQ15db2.Size = new System.Drawing.Size(32, 16);
            this.lblRXEQ15db2.TabIndex = 126;
            this.lblRXEQ15db2.Text = "15dB";
            this.lblRXEQ15db2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRXEQ0dB2
            // 
            this.lblRXEQ0dB2.Image = null;
            this.lblRXEQ0dB2.Location = new System.Drawing.Point(483, 134);
            this.lblRXEQ0dB2.Name = "lblRXEQ0dB2";
            this.lblRXEQ0dB2.Size = new System.Drawing.Size(32, 16);
            this.lblRXEQ0dB2.TabIndex = 127;
            this.lblRXEQ0dB2.Text = "  0dB";
            this.lblRXEQ0dB2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRXEQminus12db2
            // 
            this.lblRXEQminus12db2.Image = null;
            this.lblRXEQminus12db2.Location = new System.Drawing.Point(480, 178);
            this.lblRXEQminus12db2.Name = "lblRXEQminus12db2";
            this.lblRXEQminus12db2.Size = new System.Drawing.Size(34, 16);
            this.lblRXEQminus12db2.TabIndex = 128;
            this.lblRXEQminus12db2.Text = "-12dB";
            this.lblRXEQminus12db2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRXEQ10
            // 
            this.lblRXEQ10.Image = null;
            this.lblRXEQ10.Location = new System.Drawing.Point(440, 56);
            this.lblRXEQ10.Name = "lblRXEQ10";
            this.lblRXEQ10.Size = new System.Drawing.Size(40, 16);
            this.lblRXEQ10.TabIndex = 125;
            this.lblRXEQ10.Text = "16K";
            this.lblRXEQ10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblRXEQ10.Visible = false;
            // 
            // tbRXEQ10
            // 
            this.tbRXEQ10.AutoSize = false;
            this.tbRXEQ10.LargeChange = 3;
            this.tbRXEQ10.Location = new System.Drawing.Point(448, 72);
            this.tbRXEQ10.Maximum = 15;
            this.tbRXEQ10.Minimum = -12;
            this.tbRXEQ10.Name = "tbRXEQ10";
            this.tbRXEQ10.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbRXEQ10.Size = new System.Drawing.Size(32, 128);
            this.tbRXEQ10.TabIndex = 124;
            this.tbRXEQ10.TickFrequency = 3;
            this.tbRXEQ10.Visible = false;
            this.tbRXEQ10.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
            // 
            // lblRXEQ7
            // 
            this.lblRXEQ7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRXEQ7.Image = null;
            this.lblRXEQ7.Location = new System.Drawing.Point(320, 56);
            this.lblRXEQ7.Name = "lblRXEQ7";
            this.lblRXEQ7.Size = new System.Drawing.Size(40, 16);
            this.lblRXEQ7.TabIndex = 121;
            this.lblRXEQ7.Text = "2K";
            this.lblRXEQ7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblRXEQ7.Visible = false;
            // 
            // lblRXEQ8
            // 
            this.lblRXEQ8.Image = null;
            this.lblRXEQ8.Location = new System.Drawing.Point(360, 56);
            this.lblRXEQ8.Name = "lblRXEQ8";
            this.lblRXEQ8.Size = new System.Drawing.Size(40, 16);
            this.lblRXEQ8.TabIndex = 122;
            this.lblRXEQ8.Text = "4K";
            this.lblRXEQ8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblRXEQ8.Visible = false;
            // 
            // tbRXEQ7
            // 
            this.tbRXEQ7.AutoSize = false;
            this.tbRXEQ7.LargeChange = 3;
            this.tbRXEQ7.Location = new System.Drawing.Point(328, 72);
            this.tbRXEQ7.Maximum = 15;
            this.tbRXEQ7.Minimum = -12;
            this.tbRXEQ7.Name = "tbRXEQ7";
            this.tbRXEQ7.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbRXEQ7.Size = new System.Drawing.Size(32, 128);
            this.tbRXEQ7.TabIndex = 118;
            this.tbRXEQ7.TickFrequency = 3;
            this.tbRXEQ7.Visible = false;
            this.tbRXEQ7.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
            // 
            // tbRXEQ8
            // 
            this.tbRXEQ8.AutoSize = false;
            this.tbRXEQ8.LargeChange = 3;
            this.tbRXEQ8.Location = new System.Drawing.Point(368, 72);
            this.tbRXEQ8.Maximum = 15;
            this.tbRXEQ8.Minimum = -12;
            this.tbRXEQ8.Name = "tbRXEQ8";
            this.tbRXEQ8.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbRXEQ8.Size = new System.Drawing.Size(32, 128);
            this.tbRXEQ8.TabIndex = 119;
            this.tbRXEQ8.TickFrequency = 3;
            this.tbRXEQ8.Visible = false;
            this.tbRXEQ8.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
            // 
            // tbRXEQ9
            // 
            this.tbRXEQ9.AutoSize = false;
            this.tbRXEQ9.LargeChange = 3;
            this.tbRXEQ9.Location = new System.Drawing.Point(408, 72);
            this.tbRXEQ9.Maximum = 15;
            this.tbRXEQ9.Minimum = -12;
            this.tbRXEQ9.Name = "tbRXEQ9";
            this.tbRXEQ9.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbRXEQ9.Size = new System.Drawing.Size(32, 128);
            this.tbRXEQ9.TabIndex = 120;
            this.tbRXEQ9.TickFrequency = 3;
            this.tbRXEQ9.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
            // 
            // tbRXEQ4
            // 
            this.tbRXEQ4.AutoSize = false;
            this.tbRXEQ4.LargeChange = 3;
            this.tbRXEQ4.Location = new System.Drawing.Point(208, 72);
            this.tbRXEQ4.Maximum = 15;
            this.tbRXEQ4.Minimum = -12;
            this.tbRXEQ4.Name = "tbRXEQ4";
            this.tbRXEQ4.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbRXEQ4.Size = new System.Drawing.Size(32, 128);
            this.tbRXEQ4.TabIndex = 112;
            this.tbRXEQ4.TickFrequency = 3;
            this.tbRXEQ4.Visible = false;
            this.tbRXEQ4.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
            // 
            // tbRXEQ5
            // 
            this.tbRXEQ5.AutoSize = false;
            this.tbRXEQ5.LargeChange = 3;
            this.tbRXEQ5.Location = new System.Drawing.Point(248, 72);
            this.tbRXEQ5.Maximum = 15;
            this.tbRXEQ5.Minimum = -12;
            this.tbRXEQ5.Name = "tbRXEQ5";
            this.tbRXEQ5.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbRXEQ5.Size = new System.Drawing.Size(32, 128);
            this.tbRXEQ5.TabIndex = 113;
            this.tbRXEQ5.TickFrequency = 3;
            this.tbRXEQ5.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
            // 
            // tbRXEQ6
            // 
            this.tbRXEQ6.AutoSize = false;
            this.tbRXEQ6.LargeChange = 3;
            this.tbRXEQ6.Location = new System.Drawing.Point(288, 72);
            this.tbRXEQ6.Maximum = 15;
            this.tbRXEQ6.Minimum = -12;
            this.tbRXEQ6.Name = "tbRXEQ6";
            this.tbRXEQ6.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbRXEQ6.Size = new System.Drawing.Size(32, 128);
            this.tbRXEQ6.TabIndex = 114;
            this.tbRXEQ6.TickFrequency = 3;
            this.tbRXEQ6.Visible = false;
            this.tbRXEQ6.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
            // 
            // lblRXEQ4
            // 
            this.lblRXEQ4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRXEQ4.Image = null;
            this.lblRXEQ4.Location = new System.Drawing.Point(200, 56);
            this.lblRXEQ4.Name = "lblRXEQ4";
            this.lblRXEQ4.Size = new System.Drawing.Size(40, 16);
            this.lblRXEQ4.TabIndex = 115;
            this.lblRXEQ4.Text = "250";
            this.lblRXEQ4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblRXEQ4.Visible = false;
            // 
            // lblRXEQ6
            // 
            this.lblRXEQ6.Image = null;
            this.lblRXEQ6.Location = new System.Drawing.Point(280, 56);
            this.lblRXEQ6.Name = "lblRXEQ6";
            this.lblRXEQ6.Size = new System.Drawing.Size(40, 16);
            this.lblRXEQ6.TabIndex = 117;
            this.lblRXEQ6.Text = "1K";
            this.lblRXEQ6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblRXEQ6.Visible = false;
            // 
            // picRXEQ
            // 
            this.picRXEQ.BackColor = System.Drawing.Color.Black;
            this.picRXEQ.Location = new System.Drawing.Point(88, 24);
            this.picRXEQ.Name = "picRXEQ";
            this.picRXEQ.Size = new System.Drawing.Size(384, 24);
            this.picRXEQ.TabIndex = 111;
            this.picRXEQ.TabStop = false;
            this.picRXEQ.Paint += new System.Windows.Forms.PaintEventHandler(this.picRXEQ_Paint);
            // 
            // btnRXEQReset
            // 
            this.btnRXEQReset.Image = null;
            this.btnRXEQReset.Location = new System.Drawing.Point(16, 200);
            this.btnRXEQReset.Name = "btnRXEQReset";
            this.btnRXEQReset.Size = new System.Drawing.Size(56, 20);
            this.btnRXEQReset.TabIndex = 110;
            this.btnRXEQReset.Text = "Reset";
            this.btnRXEQReset.Click += new System.EventHandler(this.btnRXEQReset_Click);
            // 
            // chkRXEQEnabled
            // 
            this.chkRXEQEnabled.Image = null;
            this.chkRXEQEnabled.Location = new System.Drawing.Point(16, 24);
            this.chkRXEQEnabled.Name = "chkRXEQEnabled";
            this.chkRXEQEnabled.Size = new System.Drawing.Size(72, 16);
            this.chkRXEQEnabled.TabIndex = 109;
            this.chkRXEQEnabled.Text = "Enabled";
            this.chkRXEQEnabled.CheckedChanged += new System.EventHandler(this.chkRXEQEnabled_CheckedChanged);
            // 
            // tbRXEQ1
            // 
            this.tbRXEQ1.AutoSize = false;
            this.tbRXEQ1.LargeChange = 3;
            this.tbRXEQ1.Location = new System.Drawing.Point(88, 72);
            this.tbRXEQ1.Maximum = 15;
            this.tbRXEQ1.Minimum = -12;
            this.tbRXEQ1.Name = "tbRXEQ1";
            this.tbRXEQ1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbRXEQ1.Size = new System.Drawing.Size(32, 128);
            this.tbRXEQ1.TabIndex = 4;
            this.tbRXEQ1.TickFrequency = 3;
            this.tbRXEQ1.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
            // 
            // tbRXEQ2
            // 
            this.tbRXEQ2.AutoSize = false;
            this.tbRXEQ2.LargeChange = 3;
            this.tbRXEQ2.Location = new System.Drawing.Point(128, 72);
            this.tbRXEQ2.Maximum = 15;
            this.tbRXEQ2.Minimum = -12;
            this.tbRXEQ2.Name = "tbRXEQ2";
            this.tbRXEQ2.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbRXEQ2.Size = new System.Drawing.Size(32, 128);
            this.tbRXEQ2.TabIndex = 5;
            this.tbRXEQ2.TickFrequency = 3;
            this.tbRXEQ2.Visible = false;
            this.tbRXEQ2.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
            // 
            // tbRXEQ3
            // 
            this.tbRXEQ3.AutoSize = false;
            this.tbRXEQ3.LargeChange = 3;
            this.tbRXEQ3.Location = new System.Drawing.Point(168, 72);
            this.tbRXEQ3.Maximum = 15;
            this.tbRXEQ3.Minimum = -12;
            this.tbRXEQ3.Name = "tbRXEQ3";
            this.tbRXEQ3.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbRXEQ3.Size = new System.Drawing.Size(32, 128);
            this.tbRXEQ3.TabIndex = 6;
            this.tbRXEQ3.TickFrequency = 3;
            this.tbRXEQ3.Visible = false;
            this.tbRXEQ3.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
            // 
            // lblRXEQ2
            // 
            this.lblRXEQ2.Image = null;
            this.lblRXEQ2.Location = new System.Drawing.Point(120, 56);
            this.lblRXEQ2.Name = "lblRXEQ2";
            this.lblRXEQ2.Size = new System.Drawing.Size(40, 16);
            this.lblRXEQ2.TabIndex = 44;
            this.lblRXEQ2.Text = "63";
            this.lblRXEQ2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblRXEQ2.Visible = false;
            // 
            // lblRXEQ3
            // 
            this.lblRXEQ3.Image = null;
            this.lblRXEQ3.Location = new System.Drawing.Point(160, 56);
            this.lblRXEQ3.Name = "lblRXEQ3";
            this.lblRXEQ3.Size = new System.Drawing.Size(40, 16);
            this.lblRXEQ3.TabIndex = 45;
            this.lblRXEQ3.Text = "125";
            this.lblRXEQ3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblRXEQ3.Visible = false;
            // 
            // lblRXEQPreamp
            // 
            this.lblRXEQPreamp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRXEQPreamp.Image = null;
            this.lblRXEQPreamp.Location = new System.Drawing.Point(8, 56);
            this.lblRXEQPreamp.Name = "lblRXEQPreamp";
            this.lblRXEQPreamp.Size = new System.Drawing.Size(48, 16);
            this.lblRXEQPreamp.TabIndex = 74;
            this.lblRXEQPreamp.Text = "Preamp";
            this.lblRXEQPreamp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tbRXEQPreamp
            // 
            this.tbRXEQPreamp.AutoSize = false;
            this.tbRXEQPreamp.LargeChange = 3;
            this.tbRXEQPreamp.Location = new System.Drawing.Point(16, 72);
            this.tbRXEQPreamp.Maximum = 15;
            this.tbRXEQPreamp.Minimum = -12;
            this.tbRXEQPreamp.Name = "tbRXEQPreamp";
            this.tbRXEQPreamp.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbRXEQPreamp.Size = new System.Drawing.Size(32, 128);
            this.tbRXEQPreamp.TabIndex = 35;
            this.tbRXEQPreamp.TickFrequency = 3;
            this.tbRXEQPreamp.Scroll += new System.EventHandler(this.tbRXEQ_Scroll);
            // 
            // lblRXEQ15db
            // 
            this.lblRXEQ15db.Image = null;
            this.lblRXEQ15db.Location = new System.Drawing.Point(56, 78);
            this.lblRXEQ15db.Name = "lblRXEQ15db";
            this.lblRXEQ15db.Size = new System.Drawing.Size(32, 16);
            this.lblRXEQ15db.TabIndex = 40;
            this.lblRXEQ15db.Text = "15dB";
            this.lblRXEQ15db.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRXEQ0dB
            // 
            this.lblRXEQ0dB.Image = null;
            this.lblRXEQ0dB.Location = new System.Drawing.Point(56, 134);
            this.lblRXEQ0dB.Name = "lblRXEQ0dB";
            this.lblRXEQ0dB.Size = new System.Drawing.Size(32, 16);
            this.lblRXEQ0dB.TabIndex = 41;
            this.lblRXEQ0dB.Text = "  0dB";
            this.lblRXEQ0dB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRXEQminus12db
            // 
            this.lblRXEQminus12db.Image = null;
            this.lblRXEQminus12db.Location = new System.Drawing.Point(52, 178);
            this.lblRXEQminus12db.Name = "lblRXEQminus12db";
            this.lblRXEQminus12db.Size = new System.Drawing.Size(34, 16);
            this.lblRXEQminus12db.TabIndex = 42;
            this.lblRXEQminus12db.Text = "-12dB";
            this.lblRXEQminus12db.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpTXEQ
            // 
            this.grpTXEQ.Controls.Add(this.lblTXEQ15db2);
            this.grpTXEQ.Controls.Add(this.lblTXEQ0dB2);
            this.grpTXEQ.Controls.Add(this.lblTXEQminus12db2);
            this.grpTXEQ.Controls.Add(this.tbTXEQ10);
            this.grpTXEQ.Controls.Add(this.lblTXEQ10);
            this.grpTXEQ.Controls.Add(this.tbTXEQ7);
            this.grpTXEQ.Controls.Add(this.tbTXEQ8);
            this.grpTXEQ.Controls.Add(this.tbTXEQ9);
            this.grpTXEQ.Controls.Add(this.lblTXEQ7);
            this.grpTXEQ.Controls.Add(this.lblTXEQ8);
            this.grpTXEQ.Controls.Add(this.lblTXEQ9);
            this.grpTXEQ.Controls.Add(this.tbTXEQ4);
            this.grpTXEQ.Controls.Add(this.tbTXEQ5);
            this.grpTXEQ.Controls.Add(this.tbTXEQ6);
            this.grpTXEQ.Controls.Add(this.lblTXEQ4);
            this.grpTXEQ.Controls.Add(this.lblTXEQ5);
            this.grpTXEQ.Controls.Add(this.lblTXEQ6);
            this.grpTXEQ.Controls.Add(this.chkTXEQ160Notch);
            this.grpTXEQ.Controls.Add(this.picTXEQ);
            this.grpTXEQ.Controls.Add(this.btnTXEQReset);
            this.grpTXEQ.Controls.Add(this.chkTXEQEnabled);
            this.grpTXEQ.Controls.Add(this.tbTXEQ1);
            this.grpTXEQ.Controls.Add(this.tbTXEQ2);
            this.grpTXEQ.Controls.Add(this.tbTXEQ3);
            this.grpTXEQ.Controls.Add(this.lblTXEQ1);
            this.grpTXEQ.Controls.Add(this.lblTXEQ2);
            this.grpTXEQ.Controls.Add(this.lblTXEQ3);
            this.grpTXEQ.Controls.Add(this.lblTXEQPreamp);
            this.grpTXEQ.Controls.Add(this.tbTXEQPreamp);
            this.grpTXEQ.Controls.Add(this.lblTXEQ15db);
            this.grpTXEQ.Controls.Add(this.lblTXEQ0dB);
            this.grpTXEQ.Controls.Add(this.lblTXEQminus12db);
            this.grpTXEQ.Location = new System.Drawing.Point(8, 272);
            this.grpTXEQ.Name = "grpTXEQ";
            this.grpTXEQ.Size = new System.Drawing.Size(528, 224);
            this.grpTXEQ.TabIndex = 1;
            this.grpTXEQ.TabStop = false;
            this.grpTXEQ.Text = "Stage 3: Transmit Equalizer";
            // 
            // lblTXEQ15db2
            // 
            this.lblTXEQ15db2.Image = null;
            this.lblTXEQ15db2.Location = new System.Drawing.Point(483, 78);
            this.lblTXEQ15db2.Name = "lblTXEQ15db2";
            this.lblTXEQ15db2.Size = new System.Drawing.Size(32, 16);
            this.lblTXEQ15db2.TabIndex = 129;
            this.lblTXEQ15db2.Text = "15dB";
            this.lblTXEQ15db2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTXEQ0dB2
            // 
            this.lblTXEQ0dB2.Image = null;
            this.lblTXEQ0dB2.Location = new System.Drawing.Point(483, 134);
            this.lblTXEQ0dB2.Name = "lblTXEQ0dB2";
            this.lblTXEQ0dB2.Size = new System.Drawing.Size(32, 16);
            this.lblTXEQ0dB2.TabIndex = 128;
            this.lblTXEQ0dB2.Text = "  0dB";
            this.lblTXEQ0dB2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTXEQminus12db2
            // 
            this.lblTXEQminus12db2.Image = null;
            this.lblTXEQminus12db2.Location = new System.Drawing.Point(480, 178);
            this.lblTXEQminus12db2.Name = "lblTXEQminus12db2";
            this.lblTXEQminus12db2.Size = new System.Drawing.Size(42, 16);
            this.lblTXEQminus12db2.TabIndex = 130;
            this.lblTXEQminus12db2.Text = "-12dB";
            this.lblTXEQminus12db2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbTXEQ10
            // 
            this.tbTXEQ10.AutoSize = false;
            this.tbTXEQ10.LargeChange = 3;
            this.tbTXEQ10.Location = new System.Drawing.Point(448, 72);
            this.tbTXEQ10.Maximum = 15;
            this.tbTXEQ10.Minimum = -12;
            this.tbTXEQ10.Name = "tbTXEQ10";
            this.tbTXEQ10.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTXEQ10.Size = new System.Drawing.Size(32, 128);
            this.tbTXEQ10.TabIndex = 126;
            this.tbTXEQ10.TickFrequency = 3;
            this.tbTXEQ10.Visible = false;
            this.tbTXEQ10.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
            // 
            // tbTXEQ7
            // 
            this.tbTXEQ7.AutoSize = false;
            this.tbTXEQ7.LargeChange = 3;
            this.tbTXEQ7.Location = new System.Drawing.Point(328, 72);
            this.tbTXEQ7.Maximum = 15;
            this.tbTXEQ7.Minimum = -12;
            this.tbTXEQ7.Name = "tbTXEQ7";
            this.tbTXEQ7.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTXEQ7.Size = new System.Drawing.Size(32, 128);
            this.tbTXEQ7.TabIndex = 120;
            this.tbTXEQ7.TickFrequency = 3;
            this.tbTXEQ7.Visible = false;
            this.tbTXEQ7.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
            // 
            // tbTXEQ8
            // 
            this.tbTXEQ8.AutoSize = false;
            this.tbTXEQ8.LargeChange = 3;
            this.tbTXEQ8.Location = new System.Drawing.Point(368, 72);
            this.tbTXEQ8.Maximum = 15;
            this.tbTXEQ8.Minimum = -12;
            this.tbTXEQ8.Name = "tbTXEQ8";
            this.tbTXEQ8.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTXEQ8.Size = new System.Drawing.Size(32, 128);
            this.tbTXEQ8.TabIndex = 121;
            this.tbTXEQ8.TickFrequency = 3;
            this.tbTXEQ8.Visible = false;
            this.tbTXEQ8.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
            // 
            // tbTXEQ9
            // 
            this.tbTXEQ9.AutoSize = false;
            this.tbTXEQ9.LargeChange = 3;
            this.tbTXEQ9.Location = new System.Drawing.Point(408, 72);
            this.tbTXEQ9.Maximum = 15;
            this.tbTXEQ9.Minimum = -12;
            this.tbTXEQ9.Name = "tbTXEQ9";
            this.tbTXEQ9.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTXEQ9.Size = new System.Drawing.Size(32, 128);
            this.tbTXEQ9.TabIndex = 122;
            this.tbTXEQ9.TickFrequency = 3;
            this.tbTXEQ9.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
            // 
            // tbTXEQ4
            // 
            this.tbTXEQ4.AutoSize = false;
            this.tbTXEQ4.LargeChange = 3;
            this.tbTXEQ4.Location = new System.Drawing.Point(208, 72);
            this.tbTXEQ4.Maximum = 15;
            this.tbTXEQ4.Minimum = -12;
            this.tbTXEQ4.Name = "tbTXEQ4";
            this.tbTXEQ4.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTXEQ4.Size = new System.Drawing.Size(32, 128);
            this.tbTXEQ4.TabIndex = 114;
            this.tbTXEQ4.TickFrequency = 3;
            this.tbTXEQ4.Visible = false;
            this.tbTXEQ4.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
            // 
            // tbTXEQ5
            // 
            this.tbTXEQ5.AutoSize = false;
            this.tbTXEQ5.LargeChange = 3;
            this.tbTXEQ5.Location = new System.Drawing.Point(248, 72);
            this.tbTXEQ5.Maximum = 15;
            this.tbTXEQ5.Minimum = -12;
            this.tbTXEQ5.Name = "tbTXEQ5";
            this.tbTXEQ5.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTXEQ5.Size = new System.Drawing.Size(32, 128);
            this.tbTXEQ5.TabIndex = 115;
            this.tbTXEQ5.TickFrequency = 3;
            this.tbTXEQ5.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
            // 
            // tbTXEQ6
            // 
            this.tbTXEQ6.AutoSize = false;
            this.tbTXEQ6.LargeChange = 3;
            this.tbTXEQ6.Location = new System.Drawing.Point(288, 72);
            this.tbTXEQ6.Maximum = 15;
            this.tbTXEQ6.Minimum = -12;
            this.tbTXEQ6.Name = "tbTXEQ6";
            this.tbTXEQ6.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTXEQ6.Size = new System.Drawing.Size(32, 128);
            this.tbTXEQ6.TabIndex = 116;
            this.tbTXEQ6.TickFrequency = 3;
            this.tbTXEQ6.Visible = false;
            this.tbTXEQ6.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
            // 
            // chkTXEQ160Notch
            // 
            this.chkTXEQ160Notch.Image = null;
            this.chkTXEQ160Notch.Location = new System.Drawing.Point(104, 200);
            this.chkTXEQ160Notch.Name = "chkTXEQ160Notch";
            this.chkTXEQ160Notch.Size = new System.Drawing.Size(96, 16);
            this.chkTXEQ160Notch.TabIndex = 113;
            this.chkTXEQ160Notch.Text = "160Hz Notch";
            this.chkTXEQ160Notch.Visible = false;
            // 
            // picTXEQ
            // 
            this.picTXEQ.BackColor = System.Drawing.Color.Black;
            this.picTXEQ.Location = new System.Drawing.Point(88, 24);
            this.picTXEQ.Name = "picTXEQ";
            this.picTXEQ.Size = new System.Drawing.Size(384, 24);
            this.picTXEQ.TabIndex = 112;
            this.picTXEQ.TabStop = false;
            this.picTXEQ.Click += new System.EventHandler(this.picTXEQ_Click);
            this.picTXEQ.Paint += new System.Windows.Forms.PaintEventHandler(this.picTXEQ_Paint);
            // 
            // btnTXEQReset
            // 
            this.btnTXEQReset.Image = null;
            this.btnTXEQReset.Location = new System.Drawing.Point(16, 200);
            this.btnTXEQReset.Name = "btnTXEQReset";
            this.btnTXEQReset.Size = new System.Drawing.Size(56, 20);
            this.btnTXEQReset.TabIndex = 107;
            this.btnTXEQReset.Text = "Reset";
            this.btnTXEQReset.Click += new System.EventHandler(this.btnTXEQReset_Click);
            // 
            // chkTXEQEnabled
            // 
            this.chkTXEQEnabled.Image = null;
            this.chkTXEQEnabled.Location = new System.Drawing.Point(16, 24);
            this.chkTXEQEnabled.Name = "chkTXEQEnabled";
            this.chkTXEQEnabled.Size = new System.Drawing.Size(72, 16);
            this.chkTXEQEnabled.TabIndex = 106;
            this.chkTXEQEnabled.Text = "Enabled";
            this.chkTXEQEnabled.CheckedChanged += new System.EventHandler(this.chkTXEQEnabled_CheckedChanged);
            // 
            // tbTXEQ1
            // 
            this.tbTXEQ1.AutoSize = false;
            this.tbTXEQ1.LargeChange = 3;
            this.tbTXEQ1.Location = new System.Drawing.Point(88, 72);
            this.tbTXEQ1.Maximum = 15;
            this.tbTXEQ1.Minimum = -12;
            this.tbTXEQ1.Name = "tbTXEQ1";
            this.tbTXEQ1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTXEQ1.Size = new System.Drawing.Size(32, 128);
            this.tbTXEQ1.TabIndex = 4;
            this.tbTXEQ1.TickFrequency = 3;
            this.tbTXEQ1.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
            // 
            // tbTXEQ2
            // 
            this.tbTXEQ2.AutoSize = false;
            this.tbTXEQ2.LargeChange = 3;
            this.tbTXEQ2.Location = new System.Drawing.Point(128, 72);
            this.tbTXEQ2.Maximum = 15;
            this.tbTXEQ2.Minimum = -12;
            this.tbTXEQ2.Name = "tbTXEQ2";
            this.tbTXEQ2.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTXEQ2.Size = new System.Drawing.Size(32, 128);
            this.tbTXEQ2.TabIndex = 5;
            this.tbTXEQ2.TickFrequency = 3;
            this.tbTXEQ2.Visible = false;
            this.tbTXEQ2.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
            // 
            // tbTXEQ3
            // 
            this.tbTXEQ3.AutoSize = false;
            this.tbTXEQ3.LargeChange = 3;
            this.tbTXEQ3.Location = new System.Drawing.Point(168, 72);
            this.tbTXEQ3.Maximum = 15;
            this.tbTXEQ3.Minimum = -12;
            this.tbTXEQ3.Name = "tbTXEQ3";
            this.tbTXEQ3.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTXEQ3.Size = new System.Drawing.Size(32, 128);
            this.tbTXEQ3.TabIndex = 6;
            this.tbTXEQ3.TickFrequency = 3;
            this.tbTXEQ3.Visible = false;
            this.tbTXEQ3.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
            // 
            // lblTXEQPreamp
            // 
            this.lblTXEQPreamp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTXEQPreamp.Image = null;
            this.lblTXEQPreamp.Location = new System.Drawing.Point(8, 56);
            this.lblTXEQPreamp.Name = "lblTXEQPreamp";
            this.lblTXEQPreamp.Size = new System.Drawing.Size(48, 16);
            this.lblTXEQPreamp.TabIndex = 105;
            this.lblTXEQPreamp.Text = "Preamp";
            this.lblTXEQPreamp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tbTXEQPreamp
            // 
            this.tbTXEQPreamp.AutoSize = false;
            this.tbTXEQPreamp.LargeChange = 3;
            this.tbTXEQPreamp.Location = new System.Drawing.Point(16, 72);
            this.tbTXEQPreamp.Maximum = 15;
            this.tbTXEQPreamp.Minimum = -12;
            this.tbTXEQPreamp.Name = "tbTXEQPreamp";
            this.tbTXEQPreamp.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTXEQPreamp.Size = new System.Drawing.Size(32, 128);
            this.tbTXEQPreamp.TabIndex = 36;
            this.tbTXEQPreamp.TickFrequency = 3;
            this.tbTXEQPreamp.Scroll += new System.EventHandler(this.tbTXEQ_Scroll);
            // 
            // lblTXEQ15db
            // 
            this.lblTXEQ15db.Image = null;
            this.lblTXEQ15db.Location = new System.Drawing.Point(56, 80);
            this.lblTXEQ15db.Name = "lblTXEQ15db";
            this.lblTXEQ15db.Size = new System.Drawing.Size(32, 16);
            this.lblTXEQ15db.TabIndex = 43;
            this.lblTXEQ15db.Text = "15dB";
            this.lblTXEQ15db.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTXEQ0dB
            // 
            this.lblTXEQ0dB.Image = null;
            this.lblTXEQ0dB.Location = new System.Drawing.Point(56, 134);
            this.lblTXEQ0dB.Name = "lblTXEQ0dB";
            this.lblTXEQ0dB.Size = new System.Drawing.Size(32, 16);
            this.lblTXEQ0dB.TabIndex = 0;
            this.lblTXEQ0dB.Text = "  0dB";
            this.lblTXEQ0dB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTXEQminus12db
            // 
            this.lblTXEQminus12db.Image = null;
            this.lblTXEQminus12db.Location = new System.Drawing.Point(52, 176);
            this.lblTXEQminus12db.Name = "lblTXEQminus12db";
            this.lblTXEQminus12db.Size = new System.Drawing.Size(34, 16);
            this.lblTXEQminus12db.TabIndex = 45;
            this.lblTXEQminus12db.Text = "-12dB";
            this.lblTXEQminus12db.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpTXEQ28
            // 
            this.grpTXEQ28.Controls.Add(this.labelTS6);
            this.grpTXEQ28.Controls.Add(this.labelTS5);
            this.grpTXEQ28.Controls.Add(this.labelTS4);
            this.grpTXEQ28.Controls.Add(this.labelTS3);
            this.grpTXEQ28.Controls.Add(this.labelTS2);
            this.grpTXEQ28.Controls.Add(this.labelTS1);
            this.grpTXEQ28.Controls.Add(this.lbl2815);
            this.grpTXEQ28.Controls.Add(this.lbl2814);
            this.grpTXEQ28.Controls.Add(this.btnTXEQReset28);
            this.grpTXEQ28.Controls.Add(this.lbl2825);
            this.grpTXEQ28.Controls.Add(this.lbl2821);
            this.grpTXEQ28.Controls.Add(this.lbl2828);
            this.grpTXEQ28.Controls.Add(this.lbl2827);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ28);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ27);
            this.grpTXEQ28.Controls.Add(this.lbl2826);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ26);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ25);
            this.grpTXEQ28.Controls.Add(this.lbl2824);
            this.grpTXEQ28.Controls.Add(this.lbl2823);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ24);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ23);
            this.grpTXEQ28.Controls.Add(this.lbl2822);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ22);
            this.grpTXEQ28.Controls.Add(this.lbl2820);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ21);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ20);
            this.grpTXEQ28.Controls.Add(this.lbl2819);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ19);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ18);
            this.grpTXEQ28.Controls.Add(this.lbl2818);
            this.grpTXEQ28.Controls.Add(this.lbl2817);
            this.grpTXEQ28.Controls.Add(this.lbl2816);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ17);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ16);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ15);
            this.grpTXEQ28.Controls.Add(this.lbl287);
            this.grpTXEQ28.Controls.Add(this.lbl2813);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ14);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ13);
            this.grpTXEQ28.Controls.Add(this.lbl2812);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ12);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ11);
            this.grpTXEQ28.Controls.Add(this.lbl2811);
            this.grpTXEQ28.Controls.Add(this.lbl2810);
            this.grpTXEQ28.Controls.Add(this.lbl289);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ10);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ9);
            this.grpTXEQ28.Controls.Add(this.lbl288);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ8);
            this.grpTXEQ28.Controls.Add(this.lbl286);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ7);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ6);
            this.grpTXEQ28.Controls.Add(this.lbl285);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ5);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ4);
            this.grpTXEQ28.Controls.Add(this.lbl284);
            this.grpTXEQ28.Controls.Add(this.lbl283);
            this.grpTXEQ28.Controls.Add(this.lbl282);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ3);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ2);
            this.grpTXEQ28.Controls.Add(this.lbl281);
            this.grpTXEQ28.Controls.Add(this.tbTX28EQ1);
            this.grpTXEQ28.Location = new System.Drawing.Point(8, 502);
            this.grpTXEQ28.Name = "grpTXEQ28";
            this.grpTXEQ28.Size = new System.Drawing.Size(790, 200);
            this.grpTXEQ28.TabIndex = 131;
            this.grpTXEQ28.TabStop = false;
            this.grpTXEQ28.Text = "Stage 3:  Alternate 28 Band Transmit Equalizer";
            // 
            // labelTS6
            // 
            this.labelTS6.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTS6.Image = null;
            this.labelTS6.Location = new System.Drawing.Point(-3, 175);
            this.labelTS6.Name = "labelTS6";
            this.labelTS6.Size = new System.Drawing.Size(42, 16);
            this.labelTS6.TabIndex = 245;
            this.labelTS6.Text = "-15dB";
            this.labelTS6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTS5
            // 
            this.labelTS5.Image = null;
            this.labelTS5.Location = new System.Drawing.Point(-3, 63);
            this.labelTS5.Name = "labelTS5";
            this.labelTS5.Size = new System.Drawing.Size(32, 16);
            this.labelTS5.TabIndex = 244;
            this.labelTS5.Text = "15dB";
            this.labelTS5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTS4
            // 
            this.labelTS4.Image = null;
            this.labelTS4.Location = new System.Drawing.Point(-3, 119);
            this.labelTS4.Name = "labelTS4";
            this.labelTS4.Size = new System.Drawing.Size(32, 16);
            this.labelTS4.TabIndex = 243;
            this.labelTS4.Text = "  0dB";
            this.labelTS4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTS3
            // 
            this.labelTS3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTS3.Image = null;
            this.labelTS3.Location = new System.Drawing.Point(752, 175);
            this.labelTS3.Name = "labelTS3";
            this.labelTS3.Size = new System.Drawing.Size(42, 16);
            this.labelTS3.TabIndex = 131;
            this.labelTS3.Text = "-15dB";
            this.labelTS3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTS2
            // 
            this.labelTS2.Image = null;
            this.labelTS2.Location = new System.Drawing.Point(752, 119);
            this.labelTS2.Name = "labelTS2";
            this.labelTS2.Size = new System.Drawing.Size(32, 16);
            this.labelTS2.TabIndex = 131;
            this.labelTS2.Text = "  0dB";
            this.labelTS2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTS1
            // 
            this.labelTS1.Image = null;
            this.labelTS1.Location = new System.Drawing.Point(752, 63);
            this.labelTS1.Name = "labelTS1";
            this.labelTS1.Size = new System.Drawing.Size(32, 16);
            this.labelTS1.TabIndex = 131;
            this.labelTS1.Text = "15dB";
            this.labelTS1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl2815
            // 
            this.lbl2815.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2815.Image = null;
            this.lbl2815.Location = new System.Drawing.Point(387, 44);
            this.lbl2815.Name = "lbl2815";
            this.lbl2815.Size = new System.Drawing.Size(32, 16);
            this.lbl2815.TabIndex = 217;
            this.lbl2815.Text = "833";
            this.lbl2815.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl2815.Visible = false;
            // 
            // lbl2814
            // 
            this.lbl2814.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2814.Image = null;
            this.lbl2814.Location = new System.Drawing.Point(364, 44);
            this.lbl2814.Name = "lbl2814";
            this.lbl2814.Size = new System.Drawing.Size(26, 16);
            this.lbl2814.TabIndex = 214;
            this.lbl2814.Text = "667";
            this.lbl2814.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl2814.Visible = false;
            // 
            // btnTXEQReset28
            // 
            this.btnTXEQReset28.Image = null;
            this.btnTXEQReset28.Location = new System.Drawing.Point(723, 19);
            this.btnTXEQReset28.Name = "btnTXEQReset28";
            this.btnTXEQReset28.Size = new System.Drawing.Size(56, 20);
            this.btnTXEQReset28.TabIndex = 131;
            this.btnTXEQReset28.Text = "Reset";
            this.btnTXEQReset28.Click += new System.EventHandler(this.btnTXEQReset28_Click);
            // 
            // lbl2825
            // 
            this.lbl2825.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2825.Image = null;
            this.lbl2825.Location = new System.Drawing.Point(647, 44);
            this.lbl2825.Name = "lbl2825";
            this.lbl2825.Size = new System.Drawing.Size(26, 16);
            this.lbl2825.TabIndex = 236;
            this.lbl2825.Text = "8k";
            this.lbl2825.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl2825.Visible = false;
            // 
            // lbl2821
            // 
            this.lbl2821.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2821.Image = null;
            this.lbl2821.Location = new System.Drawing.Point(540, 44);
            this.lbl2821.Name = "lbl2821";
            this.lbl2821.Size = new System.Drawing.Size(31, 16);
            this.lbl2821.TabIndex = 228;
            this.lbl2821.Text = "3.3k";
            this.lbl2821.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl2821.Visible = false;
            // 
            // lbl2828
            // 
            this.lbl2828.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2828.Image = null;
            this.lbl2828.Location = new System.Drawing.Point(726, 44);
            this.lbl2828.Name = "lbl2828";
            this.lbl2828.Size = new System.Drawing.Size(26, 16);
            this.lbl2828.TabIndex = 242;
            this.lbl2828.Text = "16k";
            this.lbl2828.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl2828.Visible = false;
            // 
            // lbl2827
            // 
            this.lbl2827.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2827.Image = null;
            this.lbl2827.Location = new System.Drawing.Point(696, 44);
            this.lbl2827.Name = "lbl2827";
            this.lbl2827.Size = new System.Drawing.Size(34, 16);
            this.lbl2827.TabIndex = 241;
            this.lbl2827.Text = "13.3k";
            this.lbl2827.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl2827.Visible = false;
            // 
            // tbTX28EQ28
            // 
            this.tbTX28EQ28.AutoSize = false;
            this.tbTX28EQ28.BackColor = System.Drawing.SystemColors.Window;
            this.tbTX28EQ28.Location = new System.Drawing.Point(723, 63);
            this.tbTX28EQ28.Maximum = 15;
            this.tbTX28EQ28.Minimum = -15;
            this.tbTX28EQ28.Name = "tbTX28EQ28";
            this.tbTX28EQ28.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ28.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ28.TabIndex = 240;
            this.tbTX28EQ28.TickFrequency = 5;
            this.tbTX28EQ28.Visible = false;
            this.tbTX28EQ28.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // tbTX28EQ27
            // 
            this.tbTX28EQ27.AutoSize = false;
            this.tbTX28EQ27.BackColor = System.Drawing.SystemColors.Window;
            this.tbTX28EQ27.Location = new System.Drawing.Point(699, 63);
            this.tbTX28EQ27.Maximum = 15;
            this.tbTX28EQ27.Minimum = -15;
            this.tbTX28EQ27.Name = "tbTX28EQ27";
            this.tbTX28EQ27.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ27.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ27.TabIndex = 239;
            this.tbTX28EQ27.TickFrequency = 5;
            this.tbTX28EQ27.Visible = false;
            this.tbTX28EQ27.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // lbl2826
            // 
            this.lbl2826.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2826.Image = null;
            this.lbl2826.Location = new System.Drawing.Point(660, 44);
            this.lbl2826.Name = "lbl2826";
            this.lbl2826.Size = new System.Drawing.Size(42, 16);
            this.lbl2826.TabIndex = 229;
            this.lbl2826.Text = "10.6k";
            this.lbl2826.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl2826.Visible = false;
            // 
            // tbTX28EQ26
            // 
            this.tbTX28EQ26.AutoSize = false;
            this.tbTX28EQ26.BackColor = System.Drawing.SystemColors.Window;
            this.tbTX28EQ26.Location = new System.Drawing.Point(673, 63);
            this.tbTX28EQ26.Maximum = 15;
            this.tbTX28EQ26.Minimum = -15;
            this.tbTX28EQ26.Name = "tbTX28EQ26";
            this.tbTX28EQ26.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ26.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ26.TabIndex = 238;
            this.tbTX28EQ26.TickFrequency = 5;
            this.tbTX28EQ26.Visible = false;
            this.tbTX28EQ26.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // tbTX28EQ25
            // 
            this.tbTX28EQ25.AutoSize = false;
            this.tbTX28EQ25.BackColor = System.Drawing.SystemColors.Window;
            this.tbTX28EQ25.Location = new System.Drawing.Point(648, 63);
            this.tbTX28EQ25.Maximum = 15;
            this.tbTX28EQ25.Minimum = -15;
            this.tbTX28EQ25.Name = "tbTX28EQ25";
            this.tbTX28EQ25.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ25.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ25.TabIndex = 237;
            this.tbTX28EQ25.TickFrequency = 5;
            this.tbTX28EQ25.Visible = false;
            this.tbTX28EQ25.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // lbl2824
            // 
            this.lbl2824.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2824.Image = null;
            this.lbl2824.Location = new System.Drawing.Point(616, 44);
            this.lbl2824.Name = "lbl2824";
            this.lbl2824.Size = new System.Drawing.Size(38, 16);
            this.lbl2824.TabIndex = 235;
            this.lbl2824.Text = "6.7k";
            this.lbl2824.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl2824.Visible = false;
            // 
            // lbl2823
            // 
            this.lbl2823.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2823.Image = null;
            this.lbl2823.Location = new System.Drawing.Point(588, 44);
            this.lbl2823.Name = "lbl2823";
            this.lbl2823.Size = new System.Drawing.Size(34, 16);
            this.lbl2823.TabIndex = 234;
            this.lbl2823.Text = "5.3k";
            this.lbl2823.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl2823.Visible = false;
            // 
            // tbTX28EQ24
            // 
            this.tbTX28EQ24.AutoSize = false;
            this.tbTX28EQ24.BackColor = System.Drawing.SystemColors.Window;
            this.tbTX28EQ24.Location = new System.Drawing.Point(621, 63);
            this.tbTX28EQ24.Maximum = 15;
            this.tbTX28EQ24.Minimum = -15;
            this.tbTX28EQ24.Name = "tbTX28EQ24";
            this.tbTX28EQ24.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ24.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ24.TabIndex = 233;
            this.tbTX28EQ24.TickFrequency = 5;
            this.tbTX28EQ24.Visible = false;
            this.tbTX28EQ24.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // tbTX28EQ23
            // 
            this.tbTX28EQ23.AutoSize = false;
            this.tbTX28EQ23.BackColor = System.Drawing.SystemColors.Window;
            this.tbTX28EQ23.Location = new System.Drawing.Point(595, 63);
            this.tbTX28EQ23.Maximum = 15;
            this.tbTX28EQ23.Minimum = -15;
            this.tbTX28EQ23.Name = "tbTX28EQ23";
            this.tbTX28EQ23.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ23.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ23.TabIndex = 232;
            this.tbTX28EQ23.TickFrequency = 5;
            this.tbTX28EQ23.Visible = false;
            this.tbTX28EQ23.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // lbl2822
            // 
            this.lbl2822.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2822.Image = null;
            this.lbl2822.Location = new System.Drawing.Point(565, 44);
            this.lbl2822.Name = "lbl2822";
            this.lbl2822.Size = new System.Drawing.Size(32, 16);
            this.lbl2822.TabIndex = 230;
            this.lbl2822.Text = "4k";
            this.lbl2822.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl2822.Visible = false;
            // 
            // tbTX28EQ22
            // 
            this.tbTX28EQ22.AutoSize = false;
            this.tbTX28EQ22.BackColor = System.Drawing.SystemColors.Window;
            this.tbTX28EQ22.Location = new System.Drawing.Point(571, 63);
            this.tbTX28EQ22.Maximum = 15;
            this.tbTX28EQ22.Minimum = -15;
            this.tbTX28EQ22.Name = "tbTX28EQ22";
            this.tbTX28EQ22.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ22.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ22.TabIndex = 231;
            this.tbTX28EQ22.TickFrequency = 5;
            this.tbTX28EQ22.Visible = false;
            this.tbTX28EQ22.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // lbl2820
            // 
            this.lbl2820.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2820.Image = null;
            this.lbl2820.Location = new System.Drawing.Point(512, 44);
            this.lbl2820.Name = "lbl2820";
            this.lbl2820.Size = new System.Drawing.Size(34, 16);
            this.lbl2820.TabIndex = 227;
            this.lbl2820.Text = "2.6k";
            this.lbl2820.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl2820.Visible = false;
            // 
            // tbTX28EQ21
            // 
            this.tbTX28EQ21.AutoSize = false;
            this.tbTX28EQ21.BackColor = System.Drawing.SystemColors.Window;
            this.tbTX28EQ21.Location = new System.Drawing.Point(542, 63);
            this.tbTX28EQ21.Maximum = 15;
            this.tbTX28EQ21.Minimum = -15;
            this.tbTX28EQ21.Name = "tbTX28EQ21";
            this.tbTX28EQ21.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ21.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ21.TabIndex = 226;
            this.tbTX28EQ21.TickFrequency = 5;
            this.tbTX28EQ21.Visible = false;
            this.tbTX28EQ21.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // tbTX28EQ20
            // 
            this.tbTX28EQ20.AutoSize = false;
            this.tbTX28EQ20.BackColor = System.Drawing.SystemColors.Window;
            this.tbTX28EQ20.Location = new System.Drawing.Point(517, 63);
            this.tbTX28EQ20.Maximum = 15;
            this.tbTX28EQ20.Minimum = -15;
            this.tbTX28EQ20.Name = "tbTX28EQ20";
            this.tbTX28EQ20.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ20.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ20.TabIndex = 225;
            this.tbTX28EQ20.TickFrequency = 5;
            this.tbTX28EQ20.Visible = false;
            this.tbTX28EQ20.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // lbl2819
            // 
            this.lbl2819.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2819.Image = null;
            this.lbl2819.Location = new System.Drawing.Point(489, 44);
            this.lbl2819.Name = "lbl2819";
            this.lbl2819.Size = new System.Drawing.Size(32, 16);
            this.lbl2819.TabIndex = 215;
            this.lbl2819.Text = "2k";
            this.lbl2819.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl2819.Visible = false;
            // 
            // tbTX28EQ19
            // 
            this.tbTX28EQ19.AutoSize = false;
            this.tbTX28EQ19.BackColor = System.Drawing.SystemColors.Window;
            this.tbTX28EQ19.Location = new System.Drawing.Point(492, 63);
            this.tbTX28EQ19.Maximum = 15;
            this.tbTX28EQ19.Minimum = -15;
            this.tbTX28EQ19.Name = "tbTX28EQ19";
            this.tbTX28EQ19.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ19.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ19.TabIndex = 224;
            this.tbTX28EQ19.TickFrequency = 5;
            this.tbTX28EQ19.Visible = false;
            this.tbTX28EQ19.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // tbTX28EQ18
            // 
            this.tbTX28EQ18.AutoSize = false;
            this.tbTX28EQ18.BackColor = System.Drawing.SystemColors.Window;
            this.tbTX28EQ18.Location = new System.Drawing.Point(467, 63);
            this.tbTX28EQ18.Maximum = 15;
            this.tbTX28EQ18.Minimum = -15;
            this.tbTX28EQ18.Name = "tbTX28EQ18";
            this.tbTX28EQ18.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ18.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ18.TabIndex = 223;
            this.tbTX28EQ18.TickFrequency = 5;
            this.tbTX28EQ18.Visible = false;
            this.tbTX28EQ18.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // lbl2818
            // 
            this.lbl2818.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2818.Image = null;
            this.lbl2818.Location = new System.Drawing.Point(459, 44);
            this.lbl2818.Name = "lbl2818";
            this.lbl2818.Size = new System.Drawing.Size(33, 16);
            this.lbl2818.TabIndex = 222;
            this.lbl2818.Text = "1.6k";
            this.lbl2818.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl2818.Visible = false;
            // 
            // lbl2817
            // 
            this.lbl2817.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2817.Image = null;
            this.lbl2817.Location = new System.Drawing.Point(432, 44);
            this.lbl2817.Name = "lbl2817";
            this.lbl2817.Size = new System.Drawing.Size(31, 16);
            this.lbl2817.TabIndex = 221;
            this.lbl2817.Text = "1.3k";
            this.lbl2817.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl2817.Visible = false;
            // 
            // lbl2816
            // 
            this.lbl2816.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2816.Image = null;
            this.lbl2816.Location = new System.Drawing.Point(407, 44);
            this.lbl2816.Name = "lbl2816";
            this.lbl2816.Size = new System.Drawing.Size(34, 16);
            this.lbl2816.TabIndex = 220;
            this.lbl2816.Text = "1k";
            this.lbl2816.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl2816.Visible = false;
            // 
            // tbTX28EQ17
            // 
            this.tbTX28EQ17.AutoSize = false;
            this.tbTX28EQ17.BackColor = System.Drawing.SystemColors.Window;
            this.tbTX28EQ17.Location = new System.Drawing.Point(440, 63);
            this.tbTX28EQ17.Maximum = 15;
            this.tbTX28EQ17.Minimum = -15;
            this.tbTX28EQ17.Name = "tbTX28EQ17";
            this.tbTX28EQ17.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ17.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ17.TabIndex = 219;
            this.tbTX28EQ17.TickFrequency = 5;
            this.tbTX28EQ17.Visible = false;
            this.tbTX28EQ17.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // tbTX28EQ16
            // 
            this.tbTX28EQ16.AutoSize = false;
            this.tbTX28EQ16.BackColor = System.Drawing.SystemColors.Window;
            this.tbTX28EQ16.Location = new System.Drawing.Point(414, 63);
            this.tbTX28EQ16.Maximum = 15;
            this.tbTX28EQ16.Minimum = -15;
            this.tbTX28EQ16.Name = "tbTX28EQ16";
            this.tbTX28EQ16.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ16.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ16.TabIndex = 218;
            this.tbTX28EQ16.TickFrequency = 5;
            this.tbTX28EQ16.Visible = false;
            this.tbTX28EQ16.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // tbTX28EQ15
            // 
            this.tbTX28EQ15.AutoSize = false;
            this.tbTX28EQ15.BackColor = System.Drawing.SystemColors.Window;
            this.tbTX28EQ15.Location = new System.Drawing.Point(390, 63);
            this.tbTX28EQ15.Maximum = 15;
            this.tbTX28EQ15.Minimum = -15;
            this.tbTX28EQ15.Name = "tbTX28EQ15";
            this.tbTX28EQ15.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ15.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ15.TabIndex = 216;
            this.tbTX28EQ15.TickFrequency = 5;
            this.tbTX28EQ15.Visible = false;
            this.tbTX28EQ15.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // lbl287
            // 
            this.lbl287.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl287.Image = null;
            this.lbl287.Location = new System.Drawing.Point(183, 44);
            this.lbl287.Name = "lbl287";
            this.lbl287.Size = new System.Drawing.Size(26, 16);
            this.lbl287.TabIndex = 200;
            this.lbl287.Text = "125";
            this.lbl287.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl287.Visible = false;
            // 
            // lbl2813
            // 
            this.lbl2813.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2813.Image = null;
            this.lbl2813.Location = new System.Drawing.Point(334, 44);
            this.lbl2813.Name = "lbl2813";
            this.lbl2813.Size = new System.Drawing.Size(34, 16);
            this.lbl2813.TabIndex = 213;
            this.lbl2813.Text = "500";
            this.lbl2813.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl2813.Visible = false;
            // 
            // tbTX28EQ14
            // 
            this.tbTX28EQ14.AutoSize = false;
            this.tbTX28EQ14.BackColor = System.Drawing.SystemColors.Window;
            this.tbTX28EQ14.Location = new System.Drawing.Point(361, 63);
            this.tbTX28EQ14.Maximum = 15;
            this.tbTX28EQ14.Minimum = -15;
            this.tbTX28EQ14.Name = "tbTX28EQ14";
            this.tbTX28EQ14.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ14.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ14.TabIndex = 212;
            this.tbTX28EQ14.TickFrequency = 5;
            this.tbTX28EQ14.Visible = false;
            this.tbTX28EQ14.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // tbTX28EQ13
            // 
            this.tbTX28EQ13.AutoSize = false;
            this.tbTX28EQ13.BackColor = System.Drawing.SystemColors.Window;
            this.tbTX28EQ13.Location = new System.Drawing.Point(336, 63);
            this.tbTX28EQ13.Maximum = 15;
            this.tbTX28EQ13.Minimum = -15;
            this.tbTX28EQ13.Name = "tbTX28EQ13";
            this.tbTX28EQ13.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ13.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ13.TabIndex = 211;
            this.tbTX28EQ13.TickFrequency = 5;
            this.tbTX28EQ13.Visible = false;
            this.tbTX28EQ13.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // lbl2812
            // 
            this.lbl2812.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2812.Image = null;
            this.lbl2812.Location = new System.Drawing.Point(308, 44);
            this.lbl2812.Name = "lbl2812";
            this.lbl2812.Size = new System.Drawing.Size(32, 16);
            this.lbl2812.TabIndex = 201;
            this.lbl2812.Text = "416";
            this.lbl2812.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl2812.Visible = false;
            // 
            // tbTX28EQ12
            // 
            this.tbTX28EQ12.AutoSize = false;
            this.tbTX28EQ12.BackColor = System.Drawing.SystemColors.Window;
            this.tbTX28EQ12.Location = new System.Drawing.Point(311, 63);
            this.tbTX28EQ12.Maximum = 15;
            this.tbTX28EQ12.Minimum = -15;
            this.tbTX28EQ12.Name = "tbTX28EQ12";
            this.tbTX28EQ12.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ12.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ12.TabIndex = 210;
            this.tbTX28EQ12.TickFrequency = 5;
            this.tbTX28EQ12.Visible = false;
            this.tbTX28EQ12.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // tbTX28EQ11
            // 
            this.tbTX28EQ11.AutoSize = false;
            this.tbTX28EQ11.BackColor = System.Drawing.SystemColors.Window;
            this.tbTX28EQ11.Location = new System.Drawing.Point(286, 63);
            this.tbTX28EQ11.Maximum = 15;
            this.tbTX28EQ11.Minimum = -15;
            this.tbTX28EQ11.Name = "tbTX28EQ11";
            this.tbTX28EQ11.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ11.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ11.TabIndex = 209;
            this.tbTX28EQ11.TickFrequency = 5;
            this.tbTX28EQ11.Visible = false;
            this.tbTX28EQ11.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // lbl2811
            // 
            this.lbl2811.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2811.Image = null;
            this.lbl2811.Location = new System.Drawing.Point(285, 44);
            this.lbl2811.Name = "lbl2811";
            this.lbl2811.Size = new System.Drawing.Size(26, 16);
            this.lbl2811.TabIndex = 208;
            this.lbl2811.Text = "333";
            this.lbl2811.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl2811.Visible = false;
            // 
            // lbl2810
            // 
            this.lbl2810.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2810.Image = null;
            this.lbl2810.Location = new System.Drawing.Point(256, 44);
            this.lbl2810.Name = "lbl2810";
            this.lbl2810.Size = new System.Drawing.Size(26, 16);
            this.lbl2810.TabIndex = 207;
            this.lbl2810.Text = "250";
            this.lbl2810.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl2810.Visible = false;
            // 
            // lbl289
            // 
            this.lbl289.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl289.Image = null;
            this.lbl289.Location = new System.Drawing.Point(226, 44);
            this.lbl289.Name = "lbl289";
            this.lbl289.Size = new System.Drawing.Size(34, 16);
            this.lbl289.TabIndex = 206;
            this.lbl289.Text = "208";
            this.lbl289.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl289.Visible = false;
            // 
            // tbTX28EQ10
            // 
            this.tbTX28EQ10.AutoSize = false;
            this.tbTX28EQ10.BackColor = System.Drawing.SystemColors.Window;
            this.tbTX28EQ10.Location = new System.Drawing.Point(259, 63);
            this.tbTX28EQ10.Maximum = 15;
            this.tbTX28EQ10.Minimum = -15;
            this.tbTX28EQ10.Name = "tbTX28EQ10";
            this.tbTX28EQ10.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ10.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ10.TabIndex = 205;
            this.tbTX28EQ10.TickFrequency = 5;
            this.tbTX28EQ10.Visible = false;
            this.tbTX28EQ10.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // tbTX28EQ9
            // 
            this.tbTX28EQ9.AutoSize = false;
            this.tbTX28EQ9.BackColor = System.Drawing.SystemColors.Window;
            this.tbTX28EQ9.Location = new System.Drawing.Point(233, 63);
            this.tbTX28EQ9.Maximum = 15;
            this.tbTX28EQ9.Minimum = -15;
            this.tbTX28EQ9.Name = "tbTX28EQ9";
            this.tbTX28EQ9.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ9.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ9.TabIndex = 204;
            this.tbTX28EQ9.TickFrequency = 5;
            this.tbTX28EQ9.Visible = false;
            this.tbTX28EQ9.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // lbl288
            // 
            this.lbl288.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl288.Image = null;
            this.lbl288.Location = new System.Drawing.Point(203, 44);
            this.lbl288.Name = "lbl288";
            this.lbl288.Size = new System.Drawing.Size(32, 16);
            this.lbl288.TabIndex = 202;
            this.lbl288.Text = "166";
            this.lbl288.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl288.Visible = false;
            // 
            // tbTX28EQ8
            // 
            this.tbTX28EQ8.AutoSize = false;
            this.tbTX28EQ8.BackColor = System.Drawing.SystemColors.Window;
            this.tbTX28EQ8.Location = new System.Drawing.Point(209, 63);
            this.tbTX28EQ8.Maximum = 15;
            this.tbTX28EQ8.Minimum = -15;
            this.tbTX28EQ8.Name = "tbTX28EQ8";
            this.tbTX28EQ8.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ8.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ8.TabIndex = 203;
            this.tbTX28EQ8.TickFrequency = 5;
            this.tbTX28EQ8.Visible = false;
            this.tbTX28EQ8.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // lbl286
            // 
            this.lbl286.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl286.Image = null;
            this.lbl286.Location = new System.Drawing.Point(150, 44);
            this.lbl286.Name = "lbl286";
            this.lbl286.Size = new System.Drawing.Size(34, 16);
            this.lbl286.TabIndex = 199;
            this.lbl286.Text = "104";
            this.lbl286.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl286.Visible = false;
            // 
            // tbTX28EQ7
            // 
            this.tbTX28EQ7.AutoSize = false;
            this.tbTX28EQ7.BackColor = System.Drawing.SystemColors.Window;
            this.tbTX28EQ7.Location = new System.Drawing.Point(180, 63);
            this.tbTX28EQ7.Maximum = 15;
            this.tbTX28EQ7.Minimum = -15;
            this.tbTX28EQ7.Name = "tbTX28EQ7";
            this.tbTX28EQ7.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ7.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ7.TabIndex = 198;
            this.tbTX28EQ7.TickFrequency = 5;
            this.tbTX28EQ7.Visible = false;
            this.tbTX28EQ7.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // tbTX28EQ6
            // 
            this.tbTX28EQ6.AutoSize = false;
            this.tbTX28EQ6.BackColor = System.Drawing.SystemColors.Window;
            this.tbTX28EQ6.Location = new System.Drawing.Point(155, 63);
            this.tbTX28EQ6.Maximum = 15;
            this.tbTX28EQ6.Minimum = -15;
            this.tbTX28EQ6.Name = "tbTX28EQ6";
            this.tbTX28EQ6.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ6.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ6.TabIndex = 197;
            this.tbTX28EQ6.TickFrequency = 5;
            this.tbTX28EQ6.Visible = false;
            this.tbTX28EQ6.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // lbl285
            // 
            this.lbl285.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl285.Image = null;
            this.lbl285.Location = new System.Drawing.Point(127, 44);
            this.lbl285.Name = "lbl285";
            this.lbl285.Size = new System.Drawing.Size(32, 16);
            this.lbl285.TabIndex = 187;
            this.lbl285.Text = "83";
            this.lbl285.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl285.Visible = false;
            // 
            // tbTX28EQ5
            // 
            this.tbTX28EQ5.AutoSize = false;
            this.tbTX28EQ5.BackColor = System.Drawing.SystemColors.Window;
            this.tbTX28EQ5.Location = new System.Drawing.Point(130, 63);
            this.tbTX28EQ5.Maximum = 15;
            this.tbTX28EQ5.Minimum = -15;
            this.tbTX28EQ5.Name = "tbTX28EQ5";
            this.tbTX28EQ5.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ5.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ5.TabIndex = 196;
            this.tbTX28EQ5.TickFrequency = 5;
            this.tbTX28EQ5.Visible = false;
            this.tbTX28EQ5.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // tbTX28EQ4
            // 
            this.tbTX28EQ4.AutoSize = false;
            this.tbTX28EQ4.BackColor = System.Drawing.SystemColors.Window;
            this.tbTX28EQ4.Location = new System.Drawing.Point(105, 63);
            this.tbTX28EQ4.Maximum = 15;
            this.tbTX28EQ4.Minimum = -15;
            this.tbTX28EQ4.Name = "tbTX28EQ4";
            this.tbTX28EQ4.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ4.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ4.TabIndex = 195;
            this.tbTX28EQ4.TickFrequency = 5;
            this.tbTX28EQ4.Visible = false;
            this.tbTX28EQ4.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // lbl284
            // 
            this.lbl284.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl284.Image = null;
            this.lbl284.Location = new System.Drawing.Point(104, 44);
            this.lbl284.Name = "lbl284";
            this.lbl284.Size = new System.Drawing.Size(26, 16);
            this.lbl284.TabIndex = 194;
            this.lbl284.Text = "63";
            this.lbl284.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl284.Visible = false;
            // 
            // lbl283
            // 
            this.lbl283.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl283.Image = null;
            this.lbl283.Location = new System.Drawing.Point(75, 44);
            this.lbl283.Name = "lbl283";
            this.lbl283.Size = new System.Drawing.Size(26, 16);
            this.lbl283.TabIndex = 193;
            this.lbl283.Text = "54";
            this.lbl283.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl283.Visible = false;
            // 
            // lbl282
            // 
            this.lbl282.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl282.Image = null;
            this.lbl282.Location = new System.Drawing.Point(45, 44);
            this.lbl282.Name = "lbl282";
            this.lbl282.Size = new System.Drawing.Size(34, 16);
            this.lbl282.TabIndex = 192;
            this.lbl282.Text = "43";
            this.lbl282.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl282.Visible = false;
            // 
            // tbTX28EQ3
            // 
            this.tbTX28EQ3.AutoSize = false;
            this.tbTX28EQ3.BackColor = System.Drawing.SystemColors.Window;
            this.tbTX28EQ3.Location = new System.Drawing.Point(78, 63);
            this.tbTX28EQ3.Maximum = 15;
            this.tbTX28EQ3.Minimum = -15;
            this.tbTX28EQ3.Name = "tbTX28EQ3";
            this.tbTX28EQ3.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ3.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ3.TabIndex = 191;
            this.tbTX28EQ3.TickFrequency = 5;
            this.tbTX28EQ3.Visible = false;
            this.tbTX28EQ3.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // tbTX28EQ2
            // 
            this.tbTX28EQ2.AutoSize = false;
            this.tbTX28EQ2.BackColor = System.Drawing.SystemColors.Window;
            this.tbTX28EQ2.Location = new System.Drawing.Point(52, 63);
            this.tbTX28EQ2.Maximum = 15;
            this.tbTX28EQ2.Minimum = -15;
            this.tbTX28EQ2.Name = "tbTX28EQ2";
            this.tbTX28EQ2.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ2.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ2.TabIndex = 190;
            this.tbTX28EQ2.TickFrequency = 5;
            this.tbTX28EQ2.Visible = false;
            this.tbTX28EQ2.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // lbl281
            // 
            this.lbl281.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl281.Image = null;
            this.lbl281.Location = new System.Drawing.Point(19, 44);
            this.lbl281.Name = "lbl281";
            this.lbl281.Size = new System.Drawing.Size(32, 16);
            this.lbl281.TabIndex = 188;
            this.lbl281.Text = "32";
            this.lbl281.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl281.Visible = false;
            // 
            // tbTX28EQ1
            // 
            this.tbTX28EQ1.AutoSize = false;
            this.tbTX28EQ1.BackColor = System.Drawing.SystemColors.Window;
            this.tbTX28EQ1.Location = new System.Drawing.Point(28, 63);
            this.tbTX28EQ1.Maximum = 15;
            this.tbTX28EQ1.Minimum = -15;
            this.tbTX28EQ1.Name = "tbTX28EQ1";
            this.tbTX28EQ1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbTX28EQ1.Size = new System.Drawing.Size(23, 128);
            this.tbTX28EQ1.TabIndex = 189;
            this.tbTX28EQ1.TickFrequency = 5;
            this.tbTX28EQ1.Visible = false;
            this.tbTX28EQ1.Scroll += new System.EventHandler(this.tbTX28EQ15_Scroll);
            // 
            // rad28Band
            // 
            this.rad28Band.Image = null;
            this.rad28Band.Location = new System.Drawing.Point(270, 8);
            this.rad28Band.Name = "rad28Band";
            this.rad28Band.Size = new System.Drawing.Size(146, 24);
            this.rad28Band.TabIndex = 186;
            this.rad28Band.Text = "28-Band TXEqualizer";
            this.rad28Band.CheckedChanged += new System.EventHandler(this.rad28Band_CheckedChanged);
            // 
            // EQForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(654, 608);
            this.Controls.Add(this.rad10Band);
            this.Controls.Add(this.rad3Band);
            this.Controls.Add(this.grpRXEQ);
            this.Controls.Add(this.grpTXEQ);
            this.Controls.Add(this.grpTXEQ28);
            this.Controls.Add(this.rad28Band);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EQForm";
            this.Text = " Equalizer Settings";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.EQForm_Closing);
            this.grpRXEQ.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRXEQ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQ3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRXEQPreamp)).EndInit();
            this.grpTXEQ.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTXEQ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQ3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTXEQPreamp)).EndInit();
            this.grpTXEQ28.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ28)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ27)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ26)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ25)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ24)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ23)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ22)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ21)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ20)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ19)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ18)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbTX28EQ1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		#region Properties

        
    //==========================================================================
		public int NumBands
		{
			get 
			{
                if (rad3Band.Checked) return 3;
                else if (rad10Band.Checked) return 10;
                else return 28;

			}
			set
			{
				switch(value)
				{
					case 3: rad3Band.Checked = true; break;
					case 10: rad10Band.Checked = true; break;
                    case 28: rad28Band.Checked = true; break;
				}
			}
        } // NumBands

        //====================================================================
        public int[] RXEQ
		{
			get
			{
				if(rad3Band.Checked)
				{
					int[] eq = new int[4];
					eq[0] = tbRXEQPreamp.Value;
					eq[1] = tbRXEQ1.Value;
					eq[2] = tbRXEQ5.Value;
					eq[3] = tbRXEQ9.Value;
					return eq;
				}
				else //if(rad10Band.Checked)
				{
					int[] eq = new int[11];
					eq[0] = tbRXEQPreamp.Value;
					eq[1] = tbRXEQ1.Value;
					eq[2] = tbRXEQ2.Value;
					eq[3] = tbRXEQ3.Value;
					eq[4] = tbRXEQ4.Value;
					eq[5] = tbRXEQ5.Value;
					eq[6] = tbRXEQ6.Value;
					eq[7] = tbRXEQ7.Value;
					eq[8] = tbRXEQ8.Value;
					eq[9] = tbRXEQ9.Value;
					eq[10] = tbRXEQ10.Value;
					return eq;
				}
			}

			set
			{
				if(rad3Band.Checked)
				{
					if(value.Length < 4) 
					{
						MessageBox.Show("Error setting RX EQ");
						return;
					}
					tbRXEQPreamp.Value = Math.Max(tbRXEQPreamp.Minimum, Math.Min(tbRXEQPreamp.Maximum, value[0]));
					tbRXEQ1.Value = Math.Max(tbRXEQ1.Minimum, Math.Min(tbRXEQ1.Maximum, value[1]));
					tbRXEQ5.Value = Math.Max(tbRXEQ5.Minimum, Math.Min(tbRXEQ5.Maximum, value[2]));
					tbRXEQ9.Value = Math.Max(tbRXEQ9.Minimum, Math.Min(tbRXEQ9.Maximum, value[3]));					
				}
				else if(rad10Band.Checked)
				{
					if(value.Length < 11)
					{
						MessageBox.Show("Error setting RX EQ");
						return; 
					}
					tbRXEQPreamp.Value = Math.Max(tbRXEQPreamp.Minimum, Math.Min(tbRXEQPreamp.Maximum, value[0]));
					tbRXEQ1.Value = Math.Max(tbRXEQ1.Minimum, Math.Min(tbRXEQ1.Maximum, value[1]));
					tbRXEQ2.Value = Math.Max(tbRXEQ2.Minimum, Math.Min(tbRXEQ2.Maximum, value[2]));
					tbRXEQ3.Value = Math.Max(tbRXEQ3.Minimum, Math.Min(tbRXEQ3.Maximum, value[3]));	
					tbRXEQ4.Value = Math.Max(tbRXEQ4.Minimum, Math.Min(tbRXEQ4.Maximum, value[4]));	
					tbRXEQ5.Value = Math.Max(tbRXEQ5.Minimum, Math.Min(tbRXEQ5.Maximum, value[5]));	
					tbRXEQ6.Value = Math.Max(tbRXEQ6.Minimum, Math.Min(tbRXEQ6.Maximum, value[6]));	
					tbRXEQ7.Value = Math.Max(tbRXEQ7.Minimum, Math.Min(tbRXEQ7.Maximum, value[7]));	
					tbRXEQ8.Value = Math.Max(tbRXEQ8.Minimum, Math.Min(tbRXEQ8.Maximum, value[8]));	
					tbRXEQ9.Value = Math.Max(tbRXEQ9.Minimum, Math.Min(tbRXEQ9.Maximum, value[9]));	
					tbRXEQ10.Value = Math.Max(tbRXEQ10.Minimum, Math.Min(tbRXEQ10.Maximum, value[10]));	
				}

				picRXEQ.Invalidate();
				tbRXEQ_Scroll(this, EventArgs.Empty);
			}
		}

		public int[] TXEQ
		{
			get 
			{
				if(rad3Band.Checked)
				{
					int[] eq = new int[4];
					eq[0] = tbTXEQPreamp.Value;
					eq[1] = tbTXEQ1.Value;
					eq[2] = tbTXEQ5.Value;
					eq[3] = tbTXEQ9.Value;
					return eq;
				}
				else //if(rad10Band.Checked)
				{
                    if (rad28Band.Checked)  // ke9ns add
                    {
                        Debug.WriteLine("4EQ ");

                        int[] eq = new int[29];
                        eq[0] = tbTXEQPreamp.Value;
                        eq[1] = tbTX28EQ1.Value;
                        eq[2] = tbTX28EQ2.Value;
                        eq[3] = tbTX28EQ3.Value;
                        eq[4] = tbTX28EQ4.Value;
                        eq[5] = tbTX28EQ5.Value;
                        eq[6] = tbTX28EQ6.Value;
                        eq[7] = tbTX28EQ7.Value;
                        eq[8] = tbTX28EQ8.Value;
                        eq[9] = tbTX28EQ9.Value;
                        eq[10] = tbTX28EQ10.Value;
                        eq[11] = tbTX28EQ11.Value;
                        eq[12] = tbTX28EQ12.Value;
                        eq[13] = tbTX28EQ13.Value;
                        eq[14] = tbTX28EQ14.Value;
                        eq[15] = tbTX28EQ15.Value;
                        eq[16] = tbTX28EQ16.Value;
                        eq[17] = tbTX28EQ17.Value;
                        eq[18] = tbTX28EQ18.Value;
                        eq[19] = tbTX28EQ19.Value;
                        eq[20] = tbTX28EQ20.Value;
                        eq[21] = tbTX28EQ21.Value;
                        eq[22] = tbTX28EQ22.Value;
                        eq[23] = tbTX28EQ23.Value;
                        eq[24] = tbTX28EQ24.Value;
                        eq[25] = tbTX28EQ25.Value;
                        eq[26] = tbTX28EQ26.Value;
                        eq[27] = tbTX28EQ27.Value;
                        eq[28] = tbTX28EQ28.Value;

                       return eq;
                    }
                    else
                    {
                        int[] eq = new int[11];
                        eq[0] = tbTXEQPreamp.Value;
                        eq[1] = tbTXEQ1.Value;
                        eq[2] = tbTXEQ2.Value;
                        eq[3] = tbTXEQ3.Value;
                        eq[4] = tbTXEQ4.Value;
                        eq[5] = tbTXEQ5.Value;
                        eq[6] = tbTXEQ6.Value;
                        eq[7] = tbTXEQ7.Value;
                        eq[8] = tbTXEQ8.Value;
                        eq[9] = tbTXEQ9.Value;
                        eq[10] = tbTXEQ10.Value;
                        return eq;
                    }
				}
			}
			set
			{
				if(rad3Band.Checked)
				{
					if(value.Length < 4)
					{
						MessageBox.Show("Error setting TX EQ");
						return;
					}
					tbTXEQPreamp.Value = Math.Max(tbTXEQPreamp.Minimum, Math.Min(tbTXEQPreamp.Maximum, value[0]));
					tbTXEQ1.Value = Math.Max(tbTXEQ1.Minimum, Math.Min(tbTXEQ1.Maximum, value[1]));
					tbTXEQ5.Value = Math.Max(tbTXEQ5.Minimum, Math.Min(tbTXEQ5.Maximum, value[2]));
					tbTXEQ9.Value = Math.Max(tbTXEQ9.Minimum, Math.Min(tbTXEQ9.Maximum, value[3]));
				}
				else if(rad10Band.Checked)
				{
					if(value.Length < 11)
					{
						MessageBox.Show("Error setting TX EQ");
						return;
					}
					tbTXEQPreamp.Value = Math.Max(tbTXEQPreamp.Minimum, Math.Min(tbTXEQPreamp.Maximum, value[0]));
					tbTXEQ1.Value = Math.Max(tbTXEQ1.Minimum, Math.Min(tbTXEQ1.Maximum, value[1]));
					tbTXEQ2.Value = Math.Max(tbTXEQ2.Minimum, Math.Min(tbTXEQ2.Maximum, value[2]));
					tbTXEQ3.Value = Math.Max(tbTXEQ3.Minimum, Math.Min(tbTXEQ3.Maximum, value[3]));
					tbTXEQ4.Value = Math.Max(tbTXEQ4.Minimum, Math.Min(tbTXEQ4.Maximum, value[4]));
					tbTXEQ5.Value = Math.Max(tbTXEQ5.Minimum, Math.Min(tbTXEQ5.Maximum, value[5]));
					tbTXEQ6.Value = Math.Max(tbTXEQ6.Minimum, Math.Min(tbTXEQ6.Maximum, value[6]));
					tbTXEQ7.Value = Math.Max(tbTXEQ7.Minimum, Math.Min(tbTXEQ7.Maximum, value[7]));
					tbTXEQ8.Value = Math.Max(tbTXEQ8.Minimum, Math.Min(tbTXEQ8.Maximum, value[8]));
					tbTXEQ9.Value = Math.Max(tbTXEQ9.Minimum, Math.Min(tbTXEQ9.Maximum, value[9]));
					tbTXEQ10.Value = Math.Max(tbTXEQ10.Minimum, Math.Min(tbTXEQ10.Maximum, value[10]));
				}
                else if (rad28Band.Checked)
                {
                    if (value.Length < 28)
                    {
                        MessageBox.Show("Error setting TX EQ");
                        return;
                    }
                    tbTXEQPreamp.Value = Math.Max(tbTXEQPreamp.Minimum, Math.Min(tbTXEQPreamp.Maximum, value[0]));
                    tbTX28EQ1.Value = Math.Max(tbTX28EQ1.Minimum, Math.Min(tbTX28EQ1.Maximum, value[1]));
                    tbTX28EQ2.Value = Math.Max(tbTX28EQ2.Minimum, Math.Min(tbTX28EQ2.Maximum, value[2]));
                    tbTX28EQ3.Value = Math.Max(tbTX28EQ3.Minimum, Math.Min(tbTX28EQ3.Maximum, value[3]));
                    tbTX28EQ4.Value = Math.Max(tbTX28EQ4.Minimum, Math.Min(tbTX28EQ4.Maximum, value[4]));
                    tbTX28EQ5.Value = Math.Max(tbTX28EQ5.Minimum, Math.Min(tbTX28EQ5.Maximum, value[5]));
                    tbTX28EQ6.Value = Math.Max(tbTX28EQ6.Minimum, Math.Min(tbTX28EQ6.Maximum, value[6]));
                    tbTX28EQ7.Value = Math.Max(tbTX28EQ7.Minimum, Math.Min(tbTX28EQ7.Maximum, value[7]));
                    tbTX28EQ8.Value = Math.Max(tbTX28EQ8.Minimum, Math.Min(tbTX28EQ8.Maximum, value[8]));
                    tbTX28EQ9.Value = Math.Max(tbTX28EQ9.Minimum, Math.Min(tbTX28EQ9.Maximum, value[9]));
                    tbTX28EQ10.Value = Math.Max(tbTX28EQ10.Minimum, Math.Min(tbTX28EQ10.Maximum, value[10]));

                    tbTX28EQ11.Value = Math.Max(tbTX28EQ11.Minimum, Math.Min(tbTX28EQ11.Maximum, value[11]));
                    tbTX28EQ12.Value = Math.Max(tbTX28EQ12.Minimum, Math.Min(tbTX28EQ12.Maximum, value[12]));
                    tbTX28EQ13.Value = Math.Max(tbTX28EQ13.Minimum, Math.Min(tbTX28EQ13.Maximum, value[13]));
                    tbTX28EQ14.Value = Math.Max(tbTX28EQ14.Minimum, Math.Min(tbTX28EQ14.Maximum, value[14]));
                    tbTX28EQ15.Value = Math.Max(tbTX28EQ15.Minimum, Math.Min(tbTX28EQ15.Maximum, value[15]));
                    tbTX28EQ16.Value = Math.Max(tbTX28EQ16.Minimum, Math.Min(tbTX28EQ16.Maximum, value[16]));
                    tbTX28EQ17.Value = Math.Max(tbTX28EQ17.Minimum, Math.Min(tbTX28EQ17.Maximum, value[17]));
                    tbTX28EQ18.Value = Math.Max(tbTX28EQ18.Minimum, Math.Min(tbTX28EQ18.Maximum, value[18]));
                    tbTX28EQ19.Value = Math.Max(tbTX28EQ19.Minimum, Math.Min(tbTX28EQ19.Maximum, value[19]));
                    tbTX28EQ20.Value = Math.Max(tbTX28EQ20.Minimum, Math.Min(tbTX28EQ20.Maximum, value[20]));

                    tbTX28EQ21.Value = Math.Max(tbTX28EQ21.Minimum, Math.Min(tbTX28EQ21.Maximum, value[21]));
                    tbTX28EQ22.Value = Math.Max(tbTX28EQ22.Minimum, Math.Min(tbTX28EQ22.Maximum, value[22]));
                    tbTX28EQ23.Value = Math.Max(tbTX28EQ23.Minimum, Math.Min(tbTX28EQ23.Maximum, value[23]));
                    tbTX28EQ24.Value = Math.Max(tbTX28EQ24.Minimum, Math.Min(tbTX28EQ24.Maximum, value[24]));
                    tbTX28EQ25.Value = Math.Max(tbTX28EQ25.Minimum, Math.Min(tbTX28EQ25.Maximum, value[25]));
                    tbTX28EQ26.Value = Math.Max(tbTX28EQ26.Minimum, Math.Min(tbTX28EQ26.Maximum, value[26]));
                    tbTX28EQ27.Value = Math.Max(tbTX28EQ27.Minimum, Math.Min(tbTX28EQ27.Maximum, value[27]));
                    tbTX28EQ28.Value = Math.Max(tbTX28EQ28.Minimum, Math.Min(tbTX28EQ28.Maximum, value[28]));
                }


               
                picTXEQ.Invalidate();
				tbTXEQ_Scroll(this, EventArgs.Empty);
                tbTX28EQ15_Scroll(this, EventArgs.Empty);
            }
		} // TXEQ


        public bool RXEQEnabled
		{
			get
			{
				if(chkRXEQEnabled != null) return chkRXEQEnabled.Checked;
				else return false;
			}
			set
			{
				if(chkRXEQEnabled != null) chkRXEQEnabled.Checked = value;
			}
		}

		public bool TXEQEnabled
		{
			get
			{
				if(chkTXEQEnabled != null) return chkTXEQEnabled.Checked;
				else return false;
			}
			set 
			{
				if(chkTXEQEnabled != null) chkTXEQEnabled.Checked = value;
			}
		}

		#endregion

		#region Event Handlers

		private void EQForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Hide();
			e.Cancel = true;
			Common.SaveForm(this, "EQForm");
		}

		private void tbRXEQ_Scroll(object sender, System.EventArgs e)
		{
			int[] rxeq = RXEQ;
			if(rad3Band.Checked)
			{
				console.dsp.GetDSPRX(0, 0).RXEQ3 = rxeq;
				console.dsp.GetDSPRX(0, 1).RXEQ3 = rxeq;
				console.dsp.GetDSPRX(1, 0).RXEQ3 = rxeq;
			}
			else
			{
				console.dsp.GetDSPRX(0, 0).RXEQ10 = rxeq;
				console.dsp.GetDSPRX(0, 1).RXEQ10 = rxeq;
				console.dsp.GetDSPRX(1, 0).RXEQ10 = rxeq;
			}
			picRXEQ.Invalidate();
		}
//===============================================================================
		private void tbTXEQ_Scroll(object sender, System.EventArgs e) //  ALL EQ slider changes come here
		{
			int[] txeq = TXEQ; // ke9ns get all the slider values here and put in array of ints

			if(rad3Band.Checked) 
			{
				console.dsp.GetDSPTX(0).TXEQ3 = txeq;
			}
			else
			{
				console.dsp.GetDSPTX(0).TXEQ10 = txeq; // ke9ns refers to update SetGrphTXEQ10  txeq[0] is the preamp slider;
            }
			picTXEQ.Invalidate();
		} // touched a TX EQ scroll bar


        //=====================================================================================
		private void picRXEQ_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			int[] rxeq = RXEQ;
			if(!chkRXEQEnabled.Checked)
			{
                for (int i = 0; i < rxeq.Length; i++)
                {
                    rxeq[i] = 0;
                }
			}

			Point[] points = new Point[rxeq.Length-1];
			for(int i=1; i<rxeq.Length; i++)
			{
				points[i-1].X = (int)((i-1)*picRXEQ.Width/(float)(rxeq.Length-2));
				points[i-1].Y = picRXEQ.Height/2 - (int)(rxeq[i]*(picRXEQ.Height-6)/2/15.0f +
					tbRXEQPreamp.Value * 3 / 15.0f);
			}

			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			e.Graphics.FillRectangle(new SolidBrush(Color.Black), 0, 0, picRXEQ.Width, picRXEQ.Height);
			e.Graphics.DrawLines(new Pen(Color.LightGreen), points);
		}

        //======================================================================================
		private void picTXEQ_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			int[] txeq = TXEQ;
			if(!chkTXEQEnabled.Checked)
			{
                for (int i = 0; i < txeq.Length; i++)
                {
                    Debug.WriteLine("6EQ " + i);
                    txeq[i] = 0;
                }
			}

			Point[] points = new Point[txeq.Length-1];
			for(int i=1; i<txeq.Length; i++)
			{
				points[i-1].X = (int)((i-1)*picTXEQ.Width/(float)(txeq.Length-2));
				points[i-1].Y = picTXEQ.Height/2 - (int)(txeq[i]*(picTXEQ.Height-6)/2/15.0f +
					tbTXEQPreamp.Value * 3 / 15.0f);
			}

			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			e.Graphics.FillRectangle(new SolidBrush(Color.Black), 0, 0, picTXEQ.Width, picTXEQ.Height);
			e.Graphics.DrawLines(new Pen(Color.LightGreen), points);

        } //  picTXEQ_Paint


        //===========================================================================================
        private void chkRXEQEnabled_CheckedChanged(object sender, System.EventArgs e)
		{
			console.dsp.GetDSPRX(0, 0).RXEQOn = chkRXEQEnabled.Checked;
			picRXEQ.Invalidate();
			console.RXEQ = chkRXEQEnabled.Checked;
		}

        //==================================================
        // ke9ns TX enabled
		private void chkTXEQEnabled_CheckedChanged(object sender, System.EventArgs e)
		{
			console.dsp.GetDSPTX(0).TXEQOn = chkTXEQEnabled.Checked;
			picTXEQ.Invalidate();
			console.TXEQ = chkTXEQEnabled.Checked;
		}

		private void btnRXEQReset_Click(object sender, System.EventArgs e)
		{
			DialogResult dr = MessageBox.Show(
				"Are you sure you want to reset the Receive Equalizer\n"+
				"to flat (zero)?",
				"Are you sure?",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question);
			
			if(dr == DialogResult.No)
				return;

			foreach(Control c in grpRXEQ.Controls)
			{
				if(c.GetType() == typeof(TrackBarTS))
					((TrackBarTS)c).Value = 0;
			}

			tbRXEQ_Scroll(this, EventArgs.Empty);
		}

		private void btnTXEQReset_Click(object sender, System.EventArgs e)
		{
			DialogResult dr = MessageBox.Show(
				"Are you sure you want to reset the Transmit Equalizer\n"+
				"to flat (zero)?",
				"Are you sure?",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question);
			
			if(dr == DialogResult.No)
				return;

			foreach(Control c in grpTXEQ.Controls)
			{
				if(c.GetType() == typeof(TrackBarTS))
					((TrackBarTS)c).Value = 0;
			}
            
            // ke9ns add
           

            tbTXEQ_Scroll(this, EventArgs.Empty);

        } //  btnTXEQReset_Click

        private void chkTXEQ160Notch_CheckedChanged(object sender, System.EventArgs e)
		{
			console.dsp.GetDSPTX(0).Notch160 = chkTXEQ160Notch.Checked;
		}

		private void rad3Band_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rad3Band.Checked)
			{
                this.Size = new Size(560, 540);

                //............................................

                lbl281.Visible = false;
                lbl282.Visible = false;
                lbl283.Visible = false;
                lbl284.Visible = false;
                lbl285.Visible = false;
                lbl286.Visible = false;
                lbl287.Visible = false;
                lbl288.Visible = false;
                lbl289.Visible = false;
                lbl2810.Visible = false;
                lbl2811.Visible = false;
                lbl2812.Visible = false;
                lbl2813.Visible = false;
                lbl2814.Visible = false;
                lbl2815.Visible = false;
                lbl2816.Visible = false;
                lbl2817.Visible = false;
                lbl2818.Visible = false;
                lbl2819.Visible = false;
                lbl2820.Visible = false;
                lbl2821.Visible = false;
                lbl2822.Visible = false;
                lbl2823.Visible = false;
                lbl2824.Visible = false;
                lbl2825.Visible = false;
                lbl2826.Visible = false;
                lbl2827.Visible = false;
                lbl2828.Visible = false;

                tbTX28EQ1.Visible = false; // ke9ns
                tbTX28EQ2.Visible = false;
                tbTX28EQ3.Visible = false;
                tbTX28EQ4.Visible = false;
                tbTX28EQ5.Visible = false; // ke9ns
                tbTX28EQ6.Visible = false;
                tbTX28EQ7.Visible = false;
                tbTX28EQ8.Visible = false;
                tbTX28EQ9.Visible = false;
                tbTX28EQ10.Visible = false;
                tbTX28EQ11.Visible = false; // ke9ns
                tbTX28EQ12.Visible = false;
                tbTX28EQ13.Visible = false;
                tbTX28EQ14.Visible = false;
                tbTX28EQ15.Visible = false; // ke9ns
                tbTX28EQ16.Visible = false;
                tbTX28EQ17.Visible = false;
                tbTX28EQ18.Visible = false;
                tbTX28EQ19.Visible = false;
                tbTX28EQ20.Visible = false;
                tbTX28EQ21.Visible = false; // ke9ns
                tbTX28EQ22.Visible = false;
                tbTX28EQ23.Visible = false;
                tbTX28EQ24.Visible = false;
                tbTX28EQ25.Visible = false; // ke9ns
                tbTX28EQ26.Visible = false;
                tbTX28EQ27.Visible = false;
                tbTX28EQ28.Visible = false;


                lblRXEQ2.Visible = false;
				lblRXEQ3.Visible = false;
				lblRXEQ4.Visible = false;
				lblRXEQ6.Visible = false;
				lblRXEQ7.Visible = false;
				lblRXEQ8.Visible = false;
				lblRXEQ10.Visible = false;

				tbRXEQ2.Visible = false;
				tbRXEQ3.Visible = false;
				tbRXEQ4.Visible = false;
				tbRXEQ6.Visible = false;
				tbRXEQ7.Visible = false;
				tbRXEQ8.Visible = false;
				tbRXEQ10.Visible = false;

				lblRXEQ1.Text = "Low";
				lblRXEQ5.Text = "Mid";
				lblRXEQ9.Text = "High";

				toolTip1.SetToolTip(lblRXEQ1, "0-400Hz");
				toolTip1.SetToolTip(tbRXEQ1, "0-400Hz");
				toolTip1.SetToolTip(lblRXEQ5, "400-1500Hz");
				toolTip1.SetToolTip(tbRXEQ5, "400-1500Hz");
				toolTip1.SetToolTip(lblRXEQ9, "1500-6000Hz");
				toolTip1.SetToolTip(tbRXEQ9, "1500-6000Hz");

				lblTXEQ2.Visible = false;
				lblTXEQ3.Visible = false;
				lblTXEQ4.Visible = false;
				lblTXEQ6.Visible = false;
				lblTXEQ7.Visible = false;
				lblTXEQ8.Visible = false;
				lblTXEQ10.Visible = false;

				tbTXEQ2.Visible = false;
				tbTXEQ3.Visible = false;
				tbTXEQ4.Visible = false;
				tbTXEQ6.Visible = false;
				tbTXEQ7.Visible = false;
				tbTXEQ8.Visible = false;
				tbTXEQ10.Visible = false;

				lblTXEQ1.Text = "Low";
				lblTXEQ5.Text = "Mid";
				lblTXEQ9.Text = "High";

				toolTip1.SetToolTip(lblTXEQ1, "0-400Hz");
				toolTip1.SetToolTip(tbTXEQ1, "0-400Hz");
				toolTip1.SetToolTip(lblTXEQ5, "400-1500Hz");
				toolTip1.SetToolTip(tbTXEQ5, "400-1500Hz");
				toolTip1.SetToolTip(lblTXEQ9, "1500-6000Hz");
				toolTip1.SetToolTip(tbTXEQ9, "1500-6000Hz");

				RXEQ = console.dsp.GetDSPRX(0, 0).RXEQ3;
				TXEQ = console.dsp.GetDSPTX(0).TXEQ3;

				tbRXEQ_Scroll(this, EventArgs.Empty);
				tbTXEQ_Scroll(this, EventArgs.Empty);

				picRXEQ.Invalidate();
				picTXEQ.Invalidate();
              

                console.dsp.GetDSPRX(0, 0).RXEQNumBands = 3;
				console.dsp.GetDSPTX(0).TXEQNumBands = 3;
			}
        } // rad3Band_CheckedChanged

        //========================================================================
        // ke9ns 10 band Enable
        private void rad10Band_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rad10Band.Checked)
			{
                this.Size = new Size(560,540);

                //............................................
                // 28 band
                lbl281.Visible = false;
                lbl282.Visible = false;
                lbl283.Visible = false;
                lbl284.Visible = false;
                lbl285.Visible = false;
                lbl286.Visible = false;
                lbl287.Visible = false;
                lbl288.Visible = false;
                lbl289.Visible = false;
                lbl2810.Visible = false;
                lbl2811.Visible = false;
                lbl2812.Visible = false;
                lbl2813.Visible = false;
                lbl2814.Visible = false;
                lbl2815.Visible = false;
                lbl2816.Visible = false;
                lbl2817.Visible = false;
                lbl2818.Visible = false;
                lbl2819.Visible = false;
                lbl2820.Visible = false;
                lbl2821.Visible = false;
                lbl2822.Visible = false;
                lbl2823.Visible = false;
                lbl2824.Visible = false;
                lbl2825.Visible = false;
                lbl2826.Visible = false;
                lbl2827.Visible = false;
                lbl2828.Visible = false;

                tbTX28EQ1.Visible = false; // ke9ns
                tbTX28EQ2.Visible = false;
                tbTX28EQ3.Visible = false;
                tbTX28EQ4.Visible = false;
                tbTX28EQ5.Visible = false; // ke9ns
                tbTX28EQ6.Visible = false;
                tbTX28EQ7.Visible = false;
                tbTX28EQ8.Visible = false;
                tbTX28EQ9.Visible = false;
                tbTX28EQ10.Visible = false;
                tbTX28EQ11.Visible = false; // ke9ns
                tbTX28EQ12.Visible = false;
                tbTX28EQ13.Visible = false;
                tbTX28EQ14.Visible = false;
                tbTX28EQ15.Visible = false; // ke9ns
                tbTX28EQ16.Visible = false;
                tbTX28EQ17.Visible = false;
                tbTX28EQ18.Visible = false;
                tbTX28EQ19.Visible = false;
                tbTX28EQ20.Visible = false;
                tbTX28EQ21.Visible = false; // ke9ns
                tbTX28EQ22.Visible = false;
                tbTX28EQ23.Visible = false;
                tbTX28EQ24.Visible = false;
                tbTX28EQ25.Visible = false; // ke9ns
                tbTX28EQ26.Visible = false;
                tbTX28EQ27.Visible = false;
                tbTX28EQ28.Visible = false;

                //..................................................

                lblRXEQ1.Visible = true;
                lblRXEQ2.Visible = true;
				lblRXEQ3.Visible = true;
				lblRXEQ4.Visible = true;
                lblRXEQ5.Visible = true;
                lblRXEQ6.Visible = true;
				lblRXEQ7.Visible = true;
				lblRXEQ8.Visible = true;
                lblRXEQ9.Visible = true;
                lblRXEQ10.Visible = true;

                tbRXEQ1.Visible = true;
                tbRXEQ2.Visible = true;
				tbRXEQ3.Visible = true;
				tbRXEQ4.Visible = true;
                tbRXEQ5.Visible = true;
                tbRXEQ6.Visible = true;
				tbRXEQ7.Visible = true;
				tbRXEQ8.Visible = true;
                tbRXEQ9.Visible = true;
                tbRXEQ10.Visible = true;

				lblRXEQ1.Text = "32";
				lblRXEQ5.Text = "500";
				lblRXEQ9.Text = "8K";

				toolTip1.SetToolTip(lblRXEQ1, "");
				toolTip1.SetToolTip(tbRXEQ1, "");
				toolTip1.SetToolTip(lblRXEQ5, "");
				toolTip1.SetToolTip(tbRXEQ5, "");
				toolTip1.SetToolTip(lblRXEQ9, "");
				toolTip1.SetToolTip(tbRXEQ9, "");

                lblTXEQ1.Visible = true;
                lblTXEQ2.Visible = true;
				lblTXEQ3.Visible = true;
				lblTXEQ4.Visible = true;
                lblTXEQ5.Visible = true;
                lblTXEQ6.Visible = true;
				lblTXEQ7.Visible = true;
				lblTXEQ8.Visible = true;
                lblTXEQ9.Visible = true;
                lblTXEQ10.Visible = true;

                tbTXEQ1.Visible = true;
                tbTXEQ2.Visible = true;
				tbTXEQ3.Visible = true;
				tbTXEQ4.Visible = true;
                tbTXEQ5.Visible = true;
                tbTXEQ6.Visible = true;
				tbTXEQ7.Visible = true;
				tbTXEQ8.Visible = true;
                tbTXEQ9.Visible = true;
                tbTXEQ10.Visible = true;

				lblTXEQ1.Text = "32";
				lblTXEQ5.Text = "500";
				lblTXEQ9.Text = "8K";

				toolTip1.SetToolTip(lblTXEQ1, "");
				toolTip1.SetToolTip(tbTXEQ1, "");
				toolTip1.SetToolTip(lblTXEQ5, "");
				toolTip1.SetToolTip(tbTXEQ5, "");
				toolTip1.SetToolTip(lblTXEQ9, "");
				toolTip1.SetToolTip(tbTXEQ9, "");

				RXEQ = console.dsp.GetDSPRX(0, 0).RXEQ10;
				TXEQ = console.dsp.GetDSPTX(0).TXEQ10;

				tbRXEQ_Scroll(this, EventArgs.Empty);
				tbTXEQ_Scroll(this, EventArgs.Empty);

				picRXEQ.Invalidate();
				picTXEQ.Invalidate();
              

                console.dsp.GetDSPRX(0, 0).RXEQNumBands = 10;
				console.dsp.GetDSPTX(0).TXEQNumBands = 10;

            } // if(rad10Band.Checked)

        } // rad10Band_CheckedChanged

        #endregion

        //================================================================
        // ke9ns add
        private void rad28Band_CheckedChanged(object sender, EventArgs e)
        {
            if (rad28Band.Checked)
            {
                this.Size = new Size(822,750);




                lblRXEQ2.Visible = true;
                lblRXEQ3.Visible = true;
                lblRXEQ4.Visible = true;
                lblRXEQ6.Visible = true;
                lblRXEQ7.Visible = true;
                lblRXEQ8.Visible = true;
                lblRXEQ10.Visible = true;

                tbRXEQ2.Visible = true;
                tbRXEQ3.Visible = true;
                tbRXEQ4.Visible = true;
                tbRXEQ6.Visible = true;
                tbRXEQ7.Visible = true;
                tbRXEQ8.Visible = true;
                tbRXEQ10.Visible = true;

                lblRXEQ1.Text = "32";
                lblRXEQ5.Text = "500";
                lblRXEQ9.Text = "8K";

                toolTip1.SetToolTip(lblRXEQ1, "");
                toolTip1.SetToolTip(tbRXEQ1, "");
                toolTip1.SetToolTip(lblRXEQ5, "");
                toolTip1.SetToolTip(tbRXEQ5, "");
                toolTip1.SetToolTip(lblRXEQ9, "");
                toolTip1.SetToolTip(tbRXEQ9, "");

//.........................................................

                lblTXEQ1.Visible = false; // ke9ns
                lblTXEQ2.Visible = false;
                lblTXEQ3.Visible = false;
                lblTXEQ4.Visible = false;
                lblTXEQ5.Visible = false; // ke9ns
                lblTXEQ6.Visible = false;
                lblTXEQ7.Visible = false;
                lblTXEQ8.Visible = false;
                lblTXEQ9.Visible = false;
                lblTXEQ10.Visible = false;

                tbTXEQ1.Visible = false; // ke9ns
                tbTXEQ2.Visible = false;
                tbTXEQ3.Visible = false;
                tbTXEQ4.Visible = false;
                tbTXEQ5.Visible = false; // ke9ns
                tbTXEQ6.Visible = false;
                tbTXEQ7.Visible = false;
                tbTXEQ8.Visible = false;
                tbTXEQ9.Visible = false;
                tbTXEQ10.Visible = false;

                //............................................

                lbl281.Visible = true;
                lbl282.Visible = true;
                lbl283.Visible = true;
                lbl284.Visible = true;
                lbl285.Visible = true;
                lbl286.Visible = true;
                lbl287.Visible = true;
                lbl288.Visible = true;
                lbl289.Visible = true;
                lbl2810.Visible = true;
                lbl2811.Visible = true;
                lbl2812.Visible = true;
                lbl2813.Visible = true;
                lbl2814.Visible = true;
                lbl2815.Visible = true;
                lbl2816.Visible = true;
                lbl2817.Visible = true;
                lbl2818.Visible = true;
                lbl2819.Visible = true;
                lbl2820.Visible = true;
                lbl2821.Visible = true;
                lbl2822.Visible = true;
                lbl2823.Visible = true;
                lbl2824.Visible = true;
                lbl2825.Visible = true;
                lbl2826.Visible = true;
                lbl2827.Visible = true;
                lbl2828.Visible = true;

                tbTX28EQ1.Visible = true; // ke9ns
                tbTX28EQ2.Visible = true;
                tbTX28EQ3.Visible = true;
                tbTX28EQ4.Visible = true;
                tbTX28EQ5.Visible = true; // ke9ns
                tbTX28EQ6.Visible = true;
                tbTX28EQ7.Visible = true;
                tbTX28EQ8.Visible = true;
                tbTX28EQ9.Visible = true;
                tbTX28EQ10.Visible = true;
                tbTX28EQ11.Visible = true; // ke9ns
                tbTX28EQ12.Visible = true;
                tbTX28EQ13.Visible = true; 
                tbTX28EQ14.Visible = true;
                tbTX28EQ15.Visible = true; // ke9ns
                tbTX28EQ16.Visible = true;
                tbTX28EQ17.Visible = true;
                tbTX28EQ18.Visible = true;
                tbTX28EQ19.Visible = true;
                tbTX28EQ20.Visible = true;
                tbTX28EQ21.Visible = true; // ke9ns
                tbTX28EQ22.Visible = true;
                tbTX28EQ23.Visible = true;
                tbTX28EQ24.Visible = true;
                tbTX28EQ25.Visible = true; // ke9ns
                tbTX28EQ26.Visible = true;
                tbTX28EQ27.Visible = true;
                tbTX28EQ28.Visible = true;


                //  lblTXEQ1.Text = "32";
                //   lblTXEQ5.Text = "500";
                //  lblTXEQ9.Text = "8K";

                //   toolTip1.SetToolTip(lblTXEQ1, "");
                //   toolTip1.SetToolTip(tbTXEQ1, "");
                //   toolTip1.SetToolTip(lblTXEQ5, "");
                //   toolTip1.SetToolTip(tbTXEQ5, "");
                //   toolTip1.SetToolTip(lblTXEQ9, "");
                //   toolTip1.SetToolTip(tbTXEQ9, "");

                RXEQ = console.dsp.GetDSPRX(0, 0).RXEQ10;
                TXEQ = console.dsp.GetDSPTX(0).TXEQ28;

                tbRXEQ_Scroll(this, EventArgs.Empty);
                tbTX28EQ15_Scroll(this, EventArgs.Empty);
                //   tbTXEQ28_Scroll(this, EventArgs.Empty);

                picRXEQ.Invalidate();
                picTXEQ.Invalidate();
              

                console.dsp.GetDSPRX(0, 0).RXEQNumBands = 10;
                console.dsp.GetDSPTX(0).TXEQNumBands = 28;

            } // if(rad28Band.Checked)


        } // rad28Band_CheckedChanged

        private void labelTS1_Click(object sender, EventArgs e)
        {

        }

        private void labelTS4_Click(object sender, EventArgs e)
        {

        }

        private void tbTX28EQ1_Scroll(object sender, EventArgs e)
        {

        }

        private void picTXEQ_Click(object sender, EventArgs e)
        {

        }

        //===================================================================
        // ke9ns when you move any of the 28 scroll bars for the 28 band eq  see dsp.cs and update.c and isoband.c
        private void tbTX28EQ15_Scroll(object sender, EventArgs e)
        {
            int[] txeq = TXEQ; // ke9ns get all the slider values here and put in array of ints

            if (rad28Band.Checked)
            {
            
                console.dsp.GetDSPTX(0).TXEQ28 = txeq; // ke9ns refers to update SetGrphTXEQ10  txeq[0] is the preamp slider;
            }
            picTXEQ.Invalidate();

        } // tbTX28EQ15_Scroll


        //==============================================================================
        // ke9ns add
        private void picTXEQ28_Paint(object sender, PaintEventArgs e)
        {
            int[] txeq = TXEQ;

            if (!chkTXEQEnabled.Checked)
            {
                for (int i = 0; i < txeq.Length; i++)
                    txeq[i] = 0;
            }

            Point[] points = new Point[txeq.Length - 1];
            for (int i = 1; i < txeq.Length; i++)
            {
                points[i - 1].X = (int)((i - 1) * picTXEQ.Width / (float)(txeq.Length - 2));
                points[i - 1].Y = picTXEQ.Height / 2 - (int)(txeq[i] * (picTXEQ.Height - 6) / 2 / 15.0f +
                    tbTXEQPreamp.Value * 3 / 15.0f);
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.FillRectangle(new SolidBrush(Color.Black), 0, 0, picTXEQ.Width, picTXEQ.Height);
            e.Graphics.DrawLines(new Pen(Color.LightGreen), points);

        } // picTXEQ28_Paint(

        private void btnTXEQReset28_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(
            "Are you sure you want to reset the Transmit Equalizer\n" +
            "to flat (zero)?",
            "Are you sure?",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

            if (dr == DialogResult.No)
                return;

            foreach (Control c in grpTXEQ28.Controls)
            {
                if (c.GetType() == typeof(TrackBarTS))
                    ((TrackBarTS)c).Value = 0;
            }

            // ke9ns add


            tbTXEQ_Scroll(this, EventArgs.Empty);
        }
    } // EQForm
} // PoweSDR