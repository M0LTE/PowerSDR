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
        
        public SpotControl SpotForm;                     // ke9ns add  communications with spot.cs 
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
        private RichTextBox richTextBox1;
        private Button button2;
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.chkAlwaysOnTop = new System.Windows.Forms.CheckBoxTS();
            this.SuspendLayout();
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(12, 12);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(394, 88);
            this.textBox3.TabIndex = 9;
            this.textBox3.TabStop = false;
            this.textBox3.Text = resources.GetString("textBox3.Text");
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox1.BackColor = System.Drawing.Color.LightYellow;
            this.textBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBox1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.HideSelection = false;
            this.textBox1.Location = new System.Drawing.Point(12, 144);
            this.textBox1.MaximumSize = new System.Drawing.Size(600, 400);
            this.textBox1.MaxLength = 100000;
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(394, 359);
            this.textBox1.TabIndex = 60;
            this.textBox1.TabStop = false;
            this.textBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseDown);
            this.textBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseUp);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(12, 509);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 61;
            this.button1.Text = "Update List";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(106, 113);
            this.richTextBox1.MaxLength = 20;
            this.richTextBox1.Multiline = false;
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox1.Size = new System.Drawing.Size(175, 25);
            this.richTextBox1.TabIndex = 62;
            this.richTextBox1.Text = "";
            this.richTextBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.richTextBox1_KeyPress);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(299, 115);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 63;
            this.button2.Text = "Search";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // chkAlwaysOnTop
            // 
            this.chkAlwaysOnTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAlwaysOnTop.Image = null;
            this.chkAlwaysOnTop.Location = new System.Drawing.Point(302, 509);
            this.chkAlwaysOnTop.Name = "chkAlwaysOnTop";
            this.chkAlwaysOnTop.Size = new System.Drawing.Size(104, 24);
            this.chkAlwaysOnTop.TabIndex = 59;
            this.chkAlwaysOnTop.Text = "Always On Top";
            this.chkAlwaysOnTop.CheckedChanged += new System.EventHandler(this.chkAlwaysOnTop_CheckedChanged);
            // 
            // SwlControl
            // 
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(418, 542);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.richTextBox1);
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
            bandSwlupdate();
          

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
     
        int[] swl_index = new int[20000];
       
        public void bandSwlupdate()
        {

            int iii = 0;

            string bigmessage = null; // full textbox string (combine 1 and 2)
        
            Debug.WriteLine("swl index size= "+ SpotControl.SWL_Index1);

            richTextBox1.Text = richTextBox1.Text.TrimEnd('\r', '\n');

            DateTime UTCD = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

            SpotControl.UTCNEW1 = Convert.ToInt16(UTCD.ToString("HHmm")); // convert 24hr UTC to int

            for (int ii = 0; ii < SpotControl.SWL_Index1; ii++) // start by checking spots that fall within the mhz range of the panadapter
            {
              
                if ((SpotControl.SWL_Station[ii].IndexOf(richTextBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0) || (richTextBox1.Text == "") || (richTextBox1.Text == " "))
                {
                    if ((SpotControl.SWL_TimeN[ii] <= SpotControl.UTCNEW1) && (SpotControl.SWL_TimeF[ii] >= SpotControl.UTCNEW1))
                    {

                        Debug.WriteLine("station found" + SpotControl.SWL_Freq[ii]);

                        swl_index[iii++] = ii; // keep track of frequencies on at the moment

                        if (SpotControl.SWL_Station[ii].Length > 25) SpotControl.SWL_Station[ii] = SpotControl.SWL_Station[ii].Substring(0, 25);


                        if ((SpotControl.SWL_Station[ii].IndexOf("volmet", StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            SpotControl.SWL_Mode[ii] = "USB";
                        }
                        else if ((SpotControl.SWL_Station[ii].IndexOf("fax", StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            SpotControl.SWL_Mode[ii] = "DIGU";
                        }
                        else if ((SpotControl.SWL_Station[ii].IndexOf("marker", StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            SpotControl.SWL_Mode[ii] = "DIGU";
                        }
                        else if ((SpotControl.SWL_Station[ii].IndexOf("weather", StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            SpotControl.SWL_Mode[ii] = "USB";
                        }
                        else if ((SpotControl.SWL_Station[ii].IndexOf("number", StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            SpotControl.SWL_Mode[ii] = "USB";
                        }
                        else if ((SpotControl.SWL_Station[ii].IndexOf("military", StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            SpotControl.SWL_Mode[ii] = "USB";
                        }
                        else if ((SpotControl.SWL_Station[ii].IndexOf("stanag", StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            SpotControl.SWL_Mode[ii] = "DIGU";
                        }
                        else if ((SpotControl.SWL_Station[ii].IndexOf("uscg", StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            SpotControl.SWL_Mode[ii] = "USB";
                        }
                        else if ((SpotControl.SWL_Station[ii].IndexOf("gander", StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            SpotControl.SWL_Mode[ii] = "USB";
                        }
                        else if ((SpotControl.SWL_Station[ii].IndexOf("Meteo", StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            SpotControl.SWL_Mode[ii] = "DIGU";
                        }
                        else if ((SpotControl.SWL_Station[ii].IndexOf("propag", StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            SpotControl.SWL_Mode[ii] = "DIGU";
                        }
                        else if ((SpotControl.SWL_Station[ii].IndexOf("wx", StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            SpotControl.SWL_Mode[ii] = "USB";
                        }
                        else if ((SpotControl.SWL_Station[ii].IndexOf("cw", StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            SpotControl.SWL_Mode[ii] = "CWU";
                        }
                        else if ((SpotControl.SWL_Station[ii].IndexOf("rtty", StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            SpotControl.SWL_Mode[ii] = "DIGU";
                        }
                        else if ((SpotControl.SWL_Station[ii].IndexOf("SSTV", StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            SpotControl.SWL_Mode[ii] = "DIGU";
                        }
                        else if ((SpotControl.SWL_Station[ii].IndexOf("olivia", StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            SpotControl.SWL_Mode[ii] = "DIGU";
                        }
                        else if ((SpotControl.SWL_Station[ii].IndexOf("navy", StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            SpotControl.SWL_Mode[ii] = "USB";
                        }
                        else if ((SpotControl.SWL_Station[ii].IndexOf("army", StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            SpotControl.SWL_Mode[ii] = "USB";
                        }
                        else if ((SpotControl.SWL_Station[ii].IndexOf("force", StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            SpotControl.SWL_Mode[ii] = "USB";
                        }
                        else if ((SpotControl.SWL_Station[ii].IndexOf("digital", StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            SpotControl.SWL_Mode[ii] = "DIGU";
                        }
                        else if ((SpotControl.SWL_Station[ii].IndexOf("drm", StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            SpotControl.SWL_Mode[ii] = "DIGU";
                        }
                        else if ((SpotControl.SWL_Freq[ii] / 1000000) == 8) SpotControl.SWL_Mode[ii] = "USB";

                        else SpotControl.SWL_Mode[ii] = "SAM";


                        bigmessage += (String.Format("{0:00.000000}", (double)(SpotControl.SWL_Freq[ii]) / 1000000.0) +
                            "  " + SpotControl.SWL_Station[ii].PadRight(25, ' ') + " " + SpotControl.SWL_Loc[ii].PadRight(3, ' ') +
                            " " + SpotControl.SWL_TimeN[ii].ToString().PadLeft(4, '0') + ":" + SpotControl.SWL_TimeF[ii].ToString().PadLeft(4, '0') +
                            " " + "\r\n");


                    } // check time
                } // text search to narrow down

            } // for loop through SWL_Index


            Debug.WriteLine("SWL DONE");


             textBox1.Text = bigmessage; // update screen

          //  textBox1.Focus();
          //  textBox1.Show();


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
                    int ii = textBox1.GetCharIndexFromPosition(e.Location);

                    xxx = (ii / 53); //find row 

                    Debug.WriteLine("ii " + ii);
                    Debug.WriteLine("xxx " + xxx);

                    textBox1.SelectionStart = (xxx * 53);
                    textBox1.SelectionLength = 53;

                 //   console.SaveBand(); // put away last freq you were on before moving

                    Debug.WriteLine("freq " + SpotControl.SWL_Freq[swl_index[xxx]]); 

                    console.VFOAFreq = ((double)SpotControl.SWL_Freq[swl_index[xxx]]) / 1000000.0; // convert to MHZ

                    if (SpotControl.SWL_Mode[swl_index[xxx]] == "AM") console.RX1DSPMode = DSPMode.SAM;
                    else if (SpotControl.SWL_Mode[swl_index[xxx]] == "SAM") console.RX1DSPMode = DSPMode.SAM;
                    else if (SpotControl.SWL_Mode[swl_index[xxx]] == "USB") console.RX1DSPMode = DSPMode.USB;
                    else if (SpotControl.SWL_Mode[swl_index[xxx]] == "DIGU") console.RX1DSPMode = DSPMode.DIGU;


                    textBox1.Focus();
                    textBox1.Show();


                }
                catch
                {
                    Debug.WriteLine("Bad location");

                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                // nothing yet
            }



            button1.Focus();

        } //textBox1_MouseUp

        public static int RIndex1 = 0;

        private void button1_Click(object sender, EventArgs e)
        {
          
            bandSwlupdate();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            bandSwlupdate();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

            bandSwlupdate();
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                bandSwlupdate();
            }
        }
    } // Swlcontrol


} // powersdr
