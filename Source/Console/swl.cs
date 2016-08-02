//=================================================================
// swl.cs
// created by Darrin Kohn ke9ns
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
//
//=================================================================

//=================================================================

using System;
using System.Diagnostics;
using System.Drawing;

using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace PowerSDR
{
    public class SwlControl : System.Windows.Forms.Form
    {
        
        public static SpotControl SpotForm;                     // ke9ns add  communications with spot.cs 
        public ScanControl ScanForm;                            // ke9ns add freq Scanner function

        public static Console console;   // ke9ns mod  to allow console to pass back values to setup screen
        public Setup setupForm;   // ke9ns communications with setupform  (i.e. allow combometertype.text update from inside console.cs) 

        //   private ArrayList file_list;
        private string wave_folder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\PowerSDR";

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBoxTS grpPlayback;
        private System.Windows.Forms.GroupBox grpPlaylist;
        private System.Windows.Forms.MainMenu mainMenu1;
        private TextBox textBox3;
        private CheckBoxTS chkAlwaysOnTop;
        public TextBox textBox1;
        private Button button1;
        private IContainer components;

     //   public DXMemList dxmemlist;

        #region Constructor and Destructor

        public SwlControl(Console c)
        {
            InitializeComponent();
            console = c;

            Common.RestoreForm(this, "SwlForm", true);

         

         //   bandSwlupdate();


        } // swlcontrol

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SwlControl));
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.chkAlwaysOnTop = new System.Windows.Forms.CheckBoxTS();
            this.SuspendLayout();
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(12, 12);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(266, 54);
            this.textBox3.TabIndex = 9;
            this.textBox3.TabStop = false;
            this.textBox3.Text = "SWL stations coming on air next hour\r\n";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox1.BackColor = System.Drawing.Color.LightYellow;
            this.textBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBox1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(12, 72);
            this.textBox1.MaximumSize = new System.Drawing.Size(600, 400);
            this.textBox1.MaxLength = 100000;
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(266, 389);
            this.textBox1.TabIndex = 60;
            this.textBox1.TabStop = false;
            this.textBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseDown);
            this.textBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseUp);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(165, 467);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 61;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            // 
            // chkAlwaysOnTop
            // 
            this.chkAlwaysOnTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAlwaysOnTop.Image = null;
            this.chkAlwaysOnTop.Location = new System.Drawing.Point(12, 467);
            this.chkAlwaysOnTop.Name = "chkAlwaysOnTop";
            this.chkAlwaysOnTop.Size = new System.Drawing.Size(104, 24);
            this.chkAlwaysOnTop.TabIndex = 59;
            this.chkAlwaysOnTop.Text = "Always On Top";
            this.chkAlwaysOnTop.CheckedChanged += new System.EventHandler(this.chkAlwaysOnTop_CheckedChanged);
            // 
            // SwlControl
            // 
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(290, 500);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.chkAlwaysOnTop);
            this.Controls.Add(this.textBox3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SwlControl";
            this.Text = "SWL";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SwlControl_FormClosing);
            this.Load += new System.EventHandler(this.SwlControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

        #region Properties



        #endregion

        #region Event Handlers







        #endregion

        private void SwlControl_Load(object sender, EventArgs e)
        {
          //  bandSwlupdate();
          

        }


        //=======================================================================================================================
        private void SwlControl_FormClosing(object sender, FormClosingEventArgs e)
        {
           
            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "SwlForm");
         

        }

        //===================================================================================
        //===================================================================================
        //===================================================================================
        string filter, mode;
        double freq;
        double[] freq1 = new double[20];

        string[] filter1 = new string[20];
        string[] mode1 = new string[20];


        int iii = 0; // 0-41 based on last_band

        string[] band_list = {"160M", "80M", "60M", "40M", "30M", "20M", "17M",
                                     "15M", "12M", "10M", "6M", "2M", "WWV", "GEN",
                                      "LMF","120M","90M","61M","49M","41M","31M","25M",
                                     "22M","19M","16M","14M","13M","11M",
                                     "VHF0", "VHF1", "VHF2", "VHF3", "VHF4", "VHF5",
                                     "VHF6", "VHF7", "VHF8", "VHF9", "VHF10", "VHF11",
                                     "VHF12", "VHF13" };


        public void bandSwlupdate()
        {
            string bigmessage = null; // full textbox string (combine 1 and 2)
            string bigmessage1 = null; // each freq string
            string bigmessage2 = null; // each memory string


        } // bandSwlupdate


        //======================================================================== 
       //====================================================================================

        private void chkAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = chkAlwaysOnTop.Checked;
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            textBox1.ShortcutsEnabled = false; // added to eliminate the contextmenu from popping up

        }


        int xxx = 0;

        private void textBox1_MouseUp(object sender, MouseEventArgs e)
        {
            textBox1.ShortcutsEnabled = false;


            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    int ii = textBox1.SelectionStart;
                
                    xxx = (ii / 28); //find row 
                  

                    textBox1.SelectionStart = (xxx * 28);
                    textBox1.SelectionLength = 28;

                    console.SaveBand(); // put away last freq you were on before moving

                 
                    console.SetBand(mode1[xxx], filter1[xxx], freq1[xxx]);

                    console.UpdateWaterfallLevelValues();
                }
                catch
                {
                    Debug.WriteLine("Bad location");

                }
            }
            else if (e.Button == MouseButtons.Right)
            {

            }



            button1.Focus();

        } //textBox1_MouseUp

        public static int RIndex1 = 0;

   


    } // Swlcontrol


} // powersdr
