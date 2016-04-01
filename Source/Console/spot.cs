//=================================================================
// Spot.cs
//=================================================================
// Fractional year:
//g = (360/365.25)*(N + hour/24)    //  N=day number 1-365


// declination:
// D = 0.396372 - 22.91327 * cos(g) + 4.02543 * sin(g) - 0.387205 * cos( 2 * g)+
//   + 0.051967 * sin(2 * g) - 0.154527 * cos( 3 * g) + 0.084798 * sin( 3 * g)


//=================================================================

using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;  // ke9ns add for stringbuilder
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using System.Net.Sockets; // ke9ns add
using System.Threading.Tasks;
using System.Net;

namespace PowerSDR
{
    public class SpotControl : System.Windows.Forms.Form
    {
        private static System.Reflection.Assembly myAssembly2 = System.Reflection.Assembly.GetExecutingAssembly();
      //  public static Stream Map_image = myAssembly2.GetManifestResourceStream("PowerSDR.Resources.picDisplay.png");
        public static Stream Map_image = myAssembly2.GetManifestResourceStream("PowerSDR.Resources.picD1.png");



        public static Console console;   // ke9ns mod  to allow console to pass back values to setup screen

        public static  Setup setupForm;   // ke9ns communications with setupform  (i.e. allow combometertype.text update from inside console.cs) 

      
        //   private ArrayList file_list;
        private string wave_folder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\PowerSDR";

        private Button SWLbutton;
        private Button SSBbutton;
        public TextBox textBox1;
        public TextBox nodeBox;
        private TextBox textBox3;
        public TextBox callBox;
        public TextBox portBox;
        private TextBox statusBox;
        private Button button1;
        public TextBox nameBox;
        private CheckBox checkBoxUSspot;
        private CheckBox checkBoxWorld;
        private TextBox statusBoxSWL;
        private Label label1;
        private Label label2;
        private CheckBoxTS chkAlwaysOnTop;
        public CheckBoxTS chkDXMode;
        public CheckBoxTS chkSUN;
        private ToolTip toolTip1;
        public CheckBoxTS chkGrayLine;
        private IContainer components;


        #region Constructor and Destructor

        public SpotControl(Console c)
        {
            InitializeComponent();
            console = c;

            Common.RestoreForm(this, "SpotForm", true);

        
        }

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpotControl));
            this.SWLbutton = new System.Windows.Forms.Button();
            this.SSBbutton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.nodeBox = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.callBox = new System.Windows.Forms.TextBox();
            this.portBox = new System.Windows.Forms.TextBox();
            this.statusBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.checkBoxUSspot = new System.Windows.Forms.CheckBox();
            this.checkBoxWorld = new System.Windows.Forms.CheckBox();
            this.statusBoxSWL = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkSUN = new System.Windows.Forms.CheckBoxTS();
            this.chkGrayLine = new System.Windows.Forms.CheckBoxTS();
            this.chkDXMode = new System.Windows.Forms.CheckBoxTS();
            this.chkAlwaysOnTop = new System.Windows.Forms.CheckBoxTS();
            this.SuspendLayout();
            // 
            // SWLbutton
            // 
            this.SWLbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SWLbutton.Location = new System.Drawing.Point(93, 486);
            this.SWLbutton.Name = "SWLbutton";
            this.SWLbutton.Size = new System.Drawing.Size(75, 23);
            this.SWLbutton.TabIndex = 2;
            this.SWLbutton.Text = "Spot SWL";
            this.toolTip1.SetToolTip(this.SWLbutton, "Click to turn On/Off Shortwave Spotting to the Panadapter");
            this.SWLbutton.UseVisualStyleBackColor = true;
            this.SWLbutton.Click += new System.EventHandler(this.SWLbutton_Click);
            // 
            // SSBbutton
            // 
            this.SSBbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SSBbutton.Location = new System.Drawing.Point(12, 486);
            this.SSBbutton.Name = "SSBbutton";
            this.SSBbutton.Size = new System.Drawing.Size(75, 23);
            this.SSBbutton.TabIndex = 1;
            this.SSBbutton.Text = "Spot DX";
            this.toolTip1.SetToolTip(this.SSBbutton, "Click to Turn On/Off Dx Cluster Spotting to both this DX text window and Panadapt" +
        "er\r\n");
            this.SSBbutton.UseVisualStyleBackColor = true;
            this.SSBbutton.Click += new System.EventHandler(this.spotSSB_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox1.BackColor = System.Drawing.Color.LightYellow;
            this.textBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBox1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(12, 95);
            this.textBox1.MaximumSize = new System.Drawing.Size(1000, 1000);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(663, 287);
            this.textBox1.TabIndex = 6;
            this.textBox1.TabStop = false;
            this.textBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseUp);
            // 
            // nodeBox
            // 
            this.nodeBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nodeBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nodeBox.Location = new System.Drawing.Point(294, 487);
            this.nodeBox.MaxLength = 50;
            this.nodeBox.Name = "nodeBox";
            this.nodeBox.Size = new System.Drawing.Size(270, 22);
            this.nodeBox.TabIndex = 6;
            this.nodeBox.Text = "spider.ham-radio-deluxe.com";
            this.toolTip1.SetToolTip(this.nodeBox, "Enter in a DX Cluster URL address here");
            this.nodeBox.TextChanged += new System.EventHandler(this.nodeBox_TextChanged);
            this.nodeBox.Leave += new System.EventHandler(this.nodeBox_Leave);
            this.nodeBox.MouseEnter += new System.EventHandler(this.nodeBox_MouseEnter);
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(12, 4);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(663, 85);
            this.textBox3.TabIndex = 8;
            this.textBox3.TabStop = false;
            this.textBox3.Text = resources.GetString("textBox3.Text");
            // 
            // callBox
            // 
            this.callBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.callBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.callBox.Location = new System.Drawing.Point(498, 459);
            this.callBox.MaxLength = 20;
            this.callBox.Name = "callBox";
            this.callBox.Size = new System.Drawing.Size(128, 22);
            this.callBox.TabIndex = 5;
            this.callBox.Text = "Callsign";
            this.toolTip1.SetToolTip(this.callBox, "Enter Your Call sign to login to the DX Cluster here");
            this.callBox.TextChanged += new System.EventHandler(this.callBox_TextChanged);
            this.callBox.Leave += new System.EventHandler(this.callBox_Leave);
            this.callBox.MouseEnter += new System.EventHandler(this.callBox_MouseEnter);
            // 
            // portBox
            // 
            this.portBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.portBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.portBox.Location = new System.Drawing.Point(570, 486);
            this.portBox.MaxLength = 7;
            this.portBox.Name = "portBox";
            this.portBox.Size = new System.Drawing.Size(56, 22);
            this.portBox.TabIndex = 7;
            this.portBox.Text = "8000";
            this.toolTip1.SetToolTip(this.portBox, "Enter in Dx Cluster URL Port# here");
            this.portBox.TextChanged += new System.EventHandler(this.portBox_TextChanged);
            this.portBox.Leave += new System.EventHandler(this.portBox_Leave);
            this.portBox.MouseEnter += new System.EventHandler(this.portBox_MouseEnter);
            // 
            // statusBox
            // 
            this.statusBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.statusBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusBox.Location = new System.Drawing.Point(12, 407);
            this.statusBox.Name = "statusBox";
            this.statusBox.Size = new System.Drawing.Size(156, 22);
            this.statusBox.TabIndex = 11;
            this.statusBox.Text = "Off";
            this.toolTip1.SetToolTip(this.statusBox, "Click to Test connection\r\nIf it goes back to \"Spotting\" then the connection is go" +
        "od");
            this.statusBox.Click += new System.EventHandler(this.statusBox_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(174, 486);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Pause";
            this.toolTip1.SetToolTip(this.button1, "Click to Pause the DX Text window (if spots are coming through too fast)\r\nUpdates" +
        " to the Panadapter will still occur");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // nameBox
            // 
            this.nameBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nameBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameBox.Location = new System.Drawing.Point(364, 459);
            this.nameBox.MaxLength = 25;
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(128, 22);
            this.nameBox.TabIndex = 4;
            this.nameBox.Text = "HB9DRV-9>";
            this.nameBox.Visible = false;
            this.nameBox.TextChanged += new System.EventHandler(this.nameBox_TextChanged);
            this.nameBox.Leave += new System.EventHandler(this.nameBox_Leave);
            this.nameBox.MouseEnter += new System.EventHandler(this.nameBox_MouseEnter);
            // 
            // checkBoxUSspot
            // 
            this.checkBoxUSspot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxUSspot.AutoSize = true;
            this.checkBoxUSspot.Location = new System.Drawing.Point(12, 435);
            this.checkBoxUSspot.Name = "checkBoxUSspot";
            this.checkBoxUSspot.Size = new System.Drawing.Size(163, 17);
            this.checkBoxUSspot.TabIndex = 14;
            this.checkBoxUSspot.Text = "North American Spotters only";
            this.checkBoxUSspot.UseVisualStyleBackColor = true;
            this.checkBoxUSspot.CheckedChanged += new System.EventHandler(this.checkBoxUSspot_CheckedChanged);
            // 
            // checkBoxWorld
            // 
            this.checkBoxWorld.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxWorld.AutoSize = true;
            this.checkBoxWorld.Location = new System.Drawing.Point(12, 458);
            this.checkBoxWorld.Name = "checkBoxWorld";
            this.checkBoxWorld.Size = new System.Drawing.Size(182, 17);
            this.checkBoxWorld.TabIndex = 15;
            this.checkBoxWorld.Text = "Exclude North American Spotters";
            this.checkBoxWorld.UseVisualStyleBackColor = true;
            this.checkBoxWorld.CheckedChanged += new System.EventHandler(this.checkBoxWorld_CheckedChanged);
            // 
            // statusBoxSWL
            // 
            this.statusBoxSWL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.statusBoxSWL.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusBoxSWL.Location = new System.Drawing.Point(202, 407);
            this.statusBoxSWL.Name = "statusBoxSWL";
            this.statusBoxSWL.Size = new System.Drawing.Size(156, 22);
            this.statusBoxSWL.TabIndex = 16;
            this.statusBoxSWL.Text = "Off";
            this.toolTip1.SetToolTip(this.statusBoxSWL, "Status of ShortWave spotter list transfer to PowerSDR memory\r\n");
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 391);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Status of DX Cluster";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(199, 391);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Status of SWL Spotter";
            // 
            // chkSUN
            // 
            this.chkSUN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkSUN.Image = null;
            this.chkSUN.Location = new System.Drawing.Point(376, 421);
            this.chkSUN.Name = "chkSUN";
            this.chkSUN.Size = new System.Drawing.Size(116, 24);
            this.chkSUN.TabIndex = 60;
            this.chkSUN.Text = "SunTracking\r\n";
            this.toolTip1.SetToolTip(this.chkSUN, "Sun will show on Panadapter screen \r\nBut only when using KE9SN6_World 3 only\r\nAnd" +
        " only when RX1 is in Panadapter Mode with RX2 Display OFF");
            this.chkSUN.CheckedChanged += new System.EventHandler(this.chkSUN_CheckedChanged);
            // 
            // chkGrayLine
            // 
            this.chkGrayLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkGrayLine.Image = null;
            this.chkGrayLine.Location = new System.Drawing.Point(483, 421);
            this.chkGrayLine.Name = "chkGrayLine";
            this.chkGrayLine.Size = new System.Drawing.Size(116, 24);
            this.chkGrayLine.TabIndex = 61;
            this.chkGrayLine.Text = "GrayLine Track";
            this.toolTip1.SetToolTip(this.chkGrayLine, "GrayLine will show on Panadapter Display\r\nBut only when using KE9SN6_World skin o" +
        "nly\r\nAnd only when RX1 is in Panadapter Mode with RX2 Display OFF");
            this.chkGrayLine.CheckedChanged += new System.EventHandler(this.chkGrayLine_CheckedChanged);
            // 
            // chkDXMode
            // 
            this.chkDXMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkDXMode.Image = null;
            this.chkDXMode.Location = new System.Drawing.Point(376, 391);
            this.chkDXMode.Name = "chkDXMode";
            this.chkDXMode.Size = new System.Drawing.Size(138, 24);
            this.chkDXMode.TabIndex = 59;
            this.chkDXMode.Text = "Parse \"DX Spot\" Mode";
            // 
            // chkAlwaysOnTop
            // 
            this.chkAlwaysOnTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAlwaysOnTop.Image = null;
            this.chkAlwaysOnTop.Location = new System.Drawing.Point(570, 391);
            this.chkAlwaysOnTop.Name = "chkAlwaysOnTop";
            this.chkAlwaysOnTop.Size = new System.Drawing.Size(104, 24);
            this.chkAlwaysOnTop.TabIndex = 58;
            this.chkAlwaysOnTop.Text = "Always On Top";
            this.chkAlwaysOnTop.CheckedChanged += new System.EventHandler(this.chkAlwaysOnTop_CheckedChanged);
            // 
            // SpotControl
            // 
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(687, 521);
            this.Controls.Add(this.chkGrayLine);
            this.Controls.Add(this.chkSUN);
            this.Controls.Add(this.chkDXMode);
            this.Controls.Add(this.chkAlwaysOnTop);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusBoxSWL);
            this.Controls.Add(this.checkBoxWorld);
            this.Controls.Add(this.checkBoxUSspot);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.statusBox);
            this.Controls.Add(this.portBox);
            this.Controls.Add(this.callBox);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.nodeBox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.SSBbutton);
            this.Controls.Add(this.SWLbutton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1000, 1000);
            this.Name = "SpotControl";
            this.Text = "DX / SWL Spotter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpotControl_FormClosing);
            this.Load += new System.EventHandler(this.SpotControl_Load);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.SpotControl_Layout);
            this.ResumeLayout(false);
            this.PerformLayout();

        }





        #endregion

        #region Properties



        #endregion

        #region Event Handlers







        #endregion



        public static string callB = "callsign";                     // ke9ns add call sign for dx spotter
        public static string nodeB = "spider.ham-radio-deluxe.com";  // ke9ns add node for dx spotter
        public static string portB = "8000";                        // ke9ns add port# 
        public static string nameB = "HB9DRV-9>";                   // ke9ns add port# 

      
        public static string DXCALL  // this is called or set in console
        {
            get { return callB; }
            set
            {
                callB = value;
         
            } // set
        } // callsign

        public static string DXNODE  // this is called or set in console
        {
            get { return nodeB; }
            set
            {
                nodeB = value;
          
            } // set
        } // callsign
        public static string DXNAME  // this is called or set in console
        {
            get { return nameB; }
            set
            {
                nameB = value;
     
            } // set
        } // callsign
        public static string DXPORT  // this is called or set in console
        {
            get { return portB; }
            set
            {
                portB = value;
     
            } // set
        } // callsign



        private void SpotControl_Load(object sender, EventArgs e)
        {
   
            nameBox.Text = nameB;
            callBox.Text = callB;
            nodeBox.Text = nodeB;
            portBox.Text = portB;

          

        }

        //=======================================================================================================================
        private void SpotControl_FormClosing(object sender, FormClosingEventArgs e)
        {

            callB = callBox.Text;
            nodeB = nodeBox.Text;
            portB = portBox.Text;
            nameB = nameBox.Text;
 
     
            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "SpotForm");
            //  console.MemoryList.Save();
          

        }

        public static byte SP_Active = 0;  // 1= DX Spot feature ON
        public static byte SP2_Active = 0; // DX Spot: 0=closed so ok to open again if you want, 1=in process of shutting down
        public static byte SP4_Active = 0; // 1=processing valid DX spot. 0=not processing new DX spot

        public static byte SP1_Active = 0; // SWL active
        public static byte SP3_Active = 0; // 1=SWL database loaded up, so no need to reload if you turn if OFF
       

        //=======================================================================================
        //=======================================================================================
        // ke9ns SWL spotter // www.eibispace.de to get sked.csv file to read
        private void SWLbutton_Click(object sender, EventArgs e)
        {

      
            string file_name = console.AppDataPath + "\\SWL.csv"; // //  sked - b15.csv

            if (!File.Exists(file_name))
            {
                Trace.WriteLine("problem no SWL.CSV file found ");
                statusBoxSWL.ForeColor = Color.Red;
               
                statusBoxSWL.Text = "No SWL.csv file found";

                return;
            }

            if ((SP1_Active == 0) )
            {
                SP1_Active = 1;


                //    Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                //   Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

                Thread t = new Thread(new ThreadStart(SWLSPOTTER));
                    t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                    t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
               


                t.Name = "SWL Spotter Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Lowest;
                t.Start();

              

            }
            else
            {
                SP1_Active = 0; // turn off SWL spotter

                statusBoxSWL.ForeColor = Color.Red;
                statusBoxSWL.Text = "Off";


                if (SP_Active == 0)
                {
                    console.spotterMenu.ForeColor = Color.Red;
                    console.spotterMenu.Text = "Spotter";
                }


            } // SWL not active  



        } // SWLbutton_Click

        // these are pulled from SWL.csv file
        public static string[] SWL_Station = new string[20000];       // Station name
        public static int[] SWL_Freq = new int[20000];              // in hz
        public static byte[] SWL_Band = new byte[20000];              // in Mhz
        public static int[] SWL_BandL = new int[31];              // index for each start of mhz listed in swl.csv

        public static string[] SWL_Lang = new string[20000];          // language of transmitter
        public static int[] SWL_TimeN = new int[20000];                // UTC time of operation ON air
        public static int[] SWL_TimeF = new int[20000];                // UTC time of operation OFF air
        public static string[] SWL_Day = new string[20000];          // days of operation
        public static string[] SWL_Loc = new string[20000];          // location of transmitter
        public static string[] SWL_Target = new string[20000];          // target area of station

        public static int[] SWL_Pos = new int[20000];                // related to W on the panadapter screen

        public static int SWL_Index;  //  max number of spots in memory currently
        public static int SWL_Index1;  // local index that reset back to 0 after reaching max
        public static int SWL_Index3;  //  
        public static int VFOHLast;
    
        public static int Lindex; // low index spot
        public static int Hindex; // high index spot

   
        public static FileStream stream2;          // for reading SWL.csv file
        public static BinaryReader reader2;

        public static byte Flag1 = 0; // flag to skip header line in SWL.csv file


        public static DateTime UTCD = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
        public static string FD = UTCD.ToString("HHmm");                                       // get 24hr 4 digit UTC NOW

        public static int UTCNEW1 = Convert.ToInt16(FD);                                       // convert 24hr UTC to int
        public static int UTCLAST1 = 0;
       
        //=======================================================================================
        //=======================================================================================
        //ke9ns start SWL spotting
        private void SWLSPOTTER()
        {
  
            string file_name = console.AppDataPath + "\\SWL.csv"; // //  sked - b15.csv

            if (File.Exists(file_name))
            {


                stream2 = new FileStream(file_name, FileMode.Open); // open BMP  file
                reader2 = new BinaryReader(stream2, Encoding.ASCII);


                var result = new StringBuilder();

                if (SP3_Active == 0) // dont reset if already scanned in  database
                {
                    SWL_Index1 = 0; // how big is the SWL.CSV data file in lines
                    SWL_Index = 0; // start at 1 mhz
                    Flag1 = 0;
                    
                }

                for (;;)
                {

                    if (SP3_Active == 1) // aleady scanned database
                    {
                        break; // dont rescan database over 
                    }

                    if (SP1_Active == 0)
                    {

                        reader2.Close();    // close  file
                        stream2.Close();   // close stream

                        statusBoxSWL.ForeColor = Color.Black;
                        statusBoxSWL.Text = "Off";

                        if (SP_Active == 0)
                        {
                            console.spotterMenu.ForeColor = Color.White;
                            console.spotterMenu.Text = "Spotter";
                        }

                        return;
                    }

                    statusBoxSWL.ForeColor = Color.Red;
                    statusBoxSWL.Text = "Reading " + SWL_Index1.ToString();


                    if (SP_Active == 0)
                    {
                        console.spotterMenu.ForeColor = Color.Yellow;
                        console.spotterMenu.Text = "Reading";
                    }

                    try
                    {
                        var newChar = (char)reader2.ReadChar();

                        if ((newChar == '\r'))
                        {
                            newChar = (char)reader2.ReadChar(); // read \n char to finishline

                            if (Flag1 == 1)
                            {

                                string[] values = result.ToString().Split(';'); // split line up into segments divided by ;

                                SWL_Freq[SWL_Index1] = (int)(Convert.ToDouble(values[0]) * 1000); // get freq and convert to hz

                                SWL_Band[SWL_Index1] = (byte)(SWL_Freq[SWL_Index1] / 1000000); // get freq and convert to mhz


                                if (SWL_Band[SWL_Index1] > SWL_Index)
                                {
                                    //  Trace.WriteLine("INDEX MHZ " + SWL_Index + " index1 " + SWL_Index1);
                                    SWL_BandL[SWL_Index] = SWL_Index1;                                   // SWL_BandL[0] = highest index under 1mhz, SWL_BandL[1] = highest index under 2mhz
                                    VFOHLast = 0; // refresh pan screen while loading
                                    SWL_Index++;
                                }


                                SWL_TimeN[SWL_Index1] = Convert.ToInt16(values[1].Substring(0, 4)); // get time ON (24hr 4 digit UTC)
                                SWL_TimeF[SWL_Index1] = Convert.ToInt16(values[1].Substring(5, 4)); // get time OFF

                                SWL_Day[SWL_Index1] = values[2]; // get days ON

                                SWL_Loc[SWL_Index1] = values[3]; // get location of station

                                SWL_Station[SWL_Index1] = values[4]; // get station name

                                SWL_Lang[SWL_Index1] = values[5]; // get language

                                SWL_Target[SWL_Index1] = values[6]; // get station target area


                                if (SWL_Index > 0)
                                {
                                    if ((SWL_Station[SWL_Index1 - 1] == SWL_Station[SWL_Index1])&& (SWL_Freq[SWL_Index1 - 1] == SWL_Freq[SWL_Index1]))// if same name and freq then check times
                                    {
                                        if ((SWL_TimeN[SWL_Index1 - 1] == SWL_TimeN[SWL_Index1]) && (SWL_TimeF[SWL_Index1 - 1] == SWL_TimeF[SWL_Index1])) goto BYPASS; // duplicate

                                    }

                                }

                                SWL_Index1++;


                                BYPASS:;  
                                
                                     //   Trace.Write(" freq " + SWL_Freq[SWL_Index1]);
                                    //   Trace.Write(" Band " + SWL_Band[SWL_Index1]);
                                    //   Trace.Write(" ON time " + SWL_TimeN[SWL_Index1]);
                                    //    Trace.Write(" OFF time " + SWL_TimeF[SWL_Index1]);
                                    //    Trace.Write(" days " + SWL_Day[SWL_Index1]);
                                    //   Trace.Write(" LOC " + SWL_Loc[SWL_Index1]);
                                    //  Trace.Write(" Station Name " + SWL_Station[SWL_Index1]);
                                    //  Trace.Write(" Lang " + SWL_Lang[SWL_Index1]);
                                    //   Trace.WriteLine(" target " + SWL_Target[SWL_Index1]);

                                    // remarks
                                    // ? P
                                    // ? Start
                                    // ? Stop 


                                   

                                //  if (SWL_Index1 > 500) goto PASS3; // done with file temporary

                            } // SWL Spots
                            else Flag1 = 1;

                            result = new StringBuilder(); // clean up for next line

                        }
                        else
                        {
                            result.Append(newChar);  // save char
                        }

                    }
                    catch (EndOfStreamException)
                    {
                        SWL_Index1--;
                       // textBox1.Text = "End of SWL FILE at "+ SWL_Index1.ToString();

                        break; // done with file
                    }
                    catch (Exception e)
                    {
                      //  Trace.WriteLine("excpt======== " + e);
                   //     textBox1.Text = e.ToString();

                        break; // done with file
                    }


                } // for loop until end of file is reached


                              // Trace.WriteLine("reached SWL end of file");
              

                reader2.Close();    // close  file
                stream2.Close();   // close stream


                SP3_Active = 1; // done loading swl database (Good)

                statusBoxSWL.ForeColor = Color.Blue;
                statusBoxSWL.Text = "SWL Spotting " + SWL_Index1.ToString();


                if (SP_Active == 0)
                {
                    console.spotterMenu.ForeColor = Color.Yellow;
                    console.spotterMenu.Text = "SWL Spot";
                }

                //==============================================================================
                //==============================================================================
                // ke9ns process lines of SWL data here

            } // if file exists


        } // SWLSPOTTER




       public static string Skin1  = null;

        //=======================================================================================
        //=======================================================================================
        //ke9ns start DX spotting
        private void spotSSB_Click(object sender, EventArgs e)
        {
         

            if ( (SP2_Active == 0) && (SP_Active == 0) && (callBox.Text != "callsign") && (callBox.Text != null) )
            {


                if  ((chkSUN.Checked == true) || (chkGrayLine.Checked == true))
                {
                  
                   console.picDisplay.SizeMode = PictureBoxSizeMode.StretchImage;
                    console.picDisplay.Image = Image.FromStream(Map_image);
                   
                }
               

                Thread t = new Thread(new ThreadStart(SPOTTER));

                   t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                    t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
               


                SP_Active = 1;
                    t.Name = "Spotter Thread";
                    t.IsBackground = true;
                    t.Priority = ThreadPriority.Lowest;
                    t.Start();

                  textBox1.Text = "Clicked to Open DX Spider \r\n";

            }
            else
            {

            


                SP_Active = 0; // turn off DX Spotter

                statusBox.ForeColor = Color.Red;
                console.spotterMenu.ForeColor = Color.Red;

                console.spotterMenu.Text = "Closing";
                statusBox.Text = "Closing";

                textBox1.Text += "Clicked to Close Socket (click again to Force Closed)\r\n";

                if (SP2_Active != 0)
                {
                    textBox1.Text += "Force closed \r\n";

                    SP_writer.Close();
                    SP_reader.Close();
                    networkStream.Close();
                    client.Close();

           
                }

                SP2_Active = 1; // in process of shutting down.

               console.picDisplay.Image = null; // put back original image
              console.picDisplay.Invalidate();

         

            } // turn DX spotting off



        } //  spotSSB_Click


        //====================================================================================================
        //====================================================================================================
        // ke9ns add compute angle to sun (for dusk grayline) // equations in this section below provided by Roland Leigh
        private int SUNANGLE(int lat, int lon)  // ke9ns Thread opeation (runs in en-us culture) opens internet connection to genearte list of dx spots
        {
          
               double N = 0.017214206321 * DateTime.UtcNow.DayOfYear; // 2 * PI / 365 * Day

               double latitude = lat; 
               double longitude = lon;
               latitude = latitude / 57.296;                         // lat * Math.PI / 180;  convert angle to rads

               double EQT = 0.000075 + (0.001868 * Math.Cos(N)) - (0.032077 * Math.Sin(N)) - (0.014615 * Math.Cos(2 * N)) - (0.040849 * Math.Sin(2 * N));

               double th = Math.PI * ((UTC100A / 12.0) - 1.0 + (longitude / 180.0)) + EQT;

               double delta = 0.006918 - (0.399912 * Math.Cos(N)) + (0.070257 * Math.Sin(N)) - (0.006758 * Math.Cos(2 * N)) + (0.000907 * Math.Sin(2 * N)) - (0.002697 * Math.Cos(3 * N)) + (0.00148 * Math.Sin(3 * N));

               double cossza = (Math.Sin(delta) * Math.Sin(latitude)) + (Math.Cos(delta) * Math.Cos(latitude) * Math.Cos(th));

               double SZA = (Math.Atan(-cossza / Math.Sqrt(-cossza * cossza + 1.0)) + 2.0 * Math.Atan(1)) * 57.296;  // ' the 57.296 converts this SZA to degrees

               return (int)Math.Abs(SZA); // 90=horizon, 108=total darkness, 100=dusk
              
            
        } // sunangle

        //====================================================================================================
        //====================================================================================================
        // ke9ns add Thread routine


        public static string[] SP_Time;
        public static string[] SP_Freq;
        public static string[] SP_Call;
        public static TcpClient client;                                               //           ' socket

        public static NetworkStream networkStream;                                   //         ' stream
        public static BinaryWriter SP_writer;
        public static BinaryReader SP_Reader;

        public static StreamReader SP_reader;
        public static StreamWriter SP_Writer;

        public static string message1; // DX messages
        public static string message2; // blank start
        public static string message3; // login messages

        public static string[] DX_Station = new string[1000];       // dx call sign
        public static int[] DX_Freq = new int[1000];                // in hz
        public static string[] DX_Spotter = new string[1000];       // spotter call sign
        public static string[] DX_Message = new string[1000];       // message
        public static int[] DX_Mode = new int[1000];                // mode parse from message string 0=ssb,1=cw,2=rtty,3=psk,4=olivia,5=jt65,6=contesa,7=fsk,8=mt63,9=domi,10=packtor, 11=fm, 12=drm, 13=sstv, 14=am
        public static int[] DX_Mode2 = new int[1000];                // mode2 parse from message string 0=normal , +up in hz or -dn in hz
        public static int[] DX_Time = new int[1000];                // GMT
        public static string[] DX_Grid = new string[1000];          // grid
        public static string[] DX_Age = new string[1000];            // how old is the spot

        public static int DX_Index = 0;                               //  max number of spots in memory currently
        public static int DX_Index1 = 0;                             // local index that reset back to 0 after reaching max

        public static string[] DX_FULLSTRING = new string[1000];       // full undecoded message

        public static byte DX_new = 0;
        public static string DX_temp = "          ";

        public static int UTCNEW = Convert.ToInt16(FD);                        // convert 24hr UTC to int
        public static int UTCLAST = 0;                                        // last utc time for determining when to check again
        public static int UTCLAST2 = 0;                                        // for check every 10 min

        public static double UTC100A = 0;                                       // UTC time in hours.100th of min for grayline

        public static double UTC100 = 0;                                       // UTC time as a factor of 1 (1=2400 UTC, 0= 0000UTC) 
        public static double UTCDAY = 0;                                       // UTC day as a factor of 1 (0=1day, 1=365)
        public static int[] Gray_X = new int[400];                             // for mapping the gray line x and y 
        public static int[] Gray_Y = new int[400];

        public static int Sun_Right1 = 0;
        public static int Sun_Left1 = 0;

        public static int[,] GrayLine_Pos = new int[1000, 3];                      // [0,]=is lat 180 to 0 to -180 (top to bottom)
        public static int[,] GrayLine_Pos1 = new int[1000, 3];                      // [0,]=is lat 180 to 0 to -180 (top to bottom)
        public static int[] GrayLine_Pos2 = new int[1000];
        public static int[] GrayLine_Pos3 = new int[1000];

        public static int zz = 0; // determine the y pixel for this latitude grayline
        private bool detectEncodingFromByteOrderMarks = true;

        private bool pause = false; // true = pause dx spot window update.

        public static int Sun_X = 0;  // position of SUN in picDisplay window (based on ke9ns world map skin)
        public static int Sun_Y = 0;  // position of SUN in picDisplay window (based on ke9ns world map skin) 
        public static int Sun_Top1 = 0;  // position of SUN in picDisplay window (based on ke9ns world map skin)
        public static int Sun_Bot1 = 0;  // position of SUN in picDisplay window (based on ke9ns world map skin) 

        public static bool SUN = false; // true = on
        public static bool GRAYLINE = false; // true = on

        private static int test = 0;

        public static int SFI = 0;
        public static int SN = 0;
        public static int Aindex = 0;
        public static int Kindex = 0;

        private string serverPath;

        private void SPOTTER()  // ke9ns Thread opeation (runs in en-us culture) opens internet connection to genearte list of dx spots
        {

           
            textBox1.Text += "Attempt login to:  NOAA Space Weather Prediction Center \r\n";  

            serverPath = "ftp://ftp.swpc.noaa.gov/pub/latest/wwv.txt";

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(serverPath);

            textBox1.Text += "Attempt to download Space Weather \r\n";

            request.KeepAlive = true;
            request.UsePassive = true;
            request.UseBinary = true;

            request.Method = WebRequestMethods.Ftp.DownloadFile;
            string username = "anonymous";
            string password = "guest";
            request.Credentials = new NetworkCredential(username, password);

            string noaa = null;
                   
            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
             
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                noaa = reader.ReadToEnd();

                reader.Close();
                response.Close();
                Trace.WriteLine("noaa=== " + noaa);

                textBox1.Text += "NOAA Download complete \r\n";

            }
            catch (Exception ex)
            {
                Trace.WriteLine("noaa fault=== " + ex);
                textBox1.Text += "Failed to download Space Weather \r\n";

            }

            //--------------------------------------------------------------------
            if (noaa.Contains("Solar flux ")) // 
            {

                int ind = noaa.IndexOf("Solar flux ") + 11;

                try 
                {
                    SFI = (int)(Convert.ToDouble(noaa.Substring(ind, 3)));
                    Trace.WriteLine("SFI " + SFI);
                }
                catch(Exception)
                {

                }


             } // SFI

            if (noaa.Contains("A-index ")) // 
            {

                int ind = noaa.IndexOf("A-index ") + 8;

                try
                {
                    Aindex = (int)(Convert.ToDouble(noaa.Substring(ind, 2)));
                    Trace.WriteLine("Aindex " + Aindex);
                }
                catch (Exception)
                {

                }


            } // Aindex

           

            //========================================================

            try
                {
                textBox1.Text += "Attempt Opening socket \r\n";

                client = new TcpClient();                  

                client.Connect(nodeBox.Text, Convert.ToInt16(portBox.Text));      // 'EXAMPLE  client.Connect("192.168.0.149", 230) 

                networkStream = client.GetStream();

                SP_reader = new StreamReader(networkStream,Encoding.ASCII,detectEncodingFromByteOrderMarks); //Encoding.UTF8  or detectEncodingFromByteOrderMarks
                SP_writer = new BinaryWriter(networkStream,Encoding.UTF7);

              
               
                var sb = new StringBuilder(message2);
                statusBox.ForeColor = Color.Red;
                console.spotterMenu.ForeColor = Color.Red;

                statusBox.Text = "Socket";
                console.spotterMenu.Text = "Socket";

                textBox1.Text += "Got Socket \r\n";


                for (; SP_Active > 0;) // shut down socket and thread if SP_Active = 1
                {

                    if (SP_Active == 1) // if you shut down dont attempt to read next spot
                    {
                        sb.Append((char)SP_reader.Read(), 1);

                        message3 = sb.ToString();

                        if (message3.Contains("login: "))
                        {
                            textBox1.Text += "Got login: prompt \r\n";

                           
                            sb = new StringBuilder(message2); // clear sb string over

                          
                            char[] message5 = callBox.Text.ToCharArray(); // get your call sign

                            for (int i = 0; i < message5.Length; i++)    // do it this way because telnet server wants slow typing
                            {
                                SP_writer.Write((char)message5[i]);
                             
                            }  // for loop length of your call sign

                            SP_writer.Write((char)13);
                        
                            SP_writer.Write((char)10);
                       
                           
                            statusBox.ForeColor = Color.Red;
                            console.spotterMenu.ForeColor = Color.Red;

                            statusBox.Text = "Login";
                            console.spotterMenu.Text = "Login";


                            SP_Active = 2; // logging in

                        } // look for login:

                    } // SP_active = 1
                    else if (SP_Active == 2)
                    {
                        SP_Active = 3; // logging in
                   
                        statusBox.ForeColor = Color.Green;
                        console.spotterMenu.ForeColor = Color.Blue;

                        statusBox.Text = "Spotting";
                        console.spotterMenu.Text = "Spotting";

                        textBox1.Text += "Waiting for DX Spots \r\n";


                      //  DX_Index = 0; // start at begining

                    } // SP_active == 2


                    //------------------------------------------------------------------------
                    // ke9nsformat:  
                    // 0     6            23 26          39                  70    76   80818283 (83 with everything or 79 with no Grid)
                    // DX de ke9ns:  7003.5  kc9ffv      up 5                0204Z en52 \a\a\r\n
                    //------------------------------------------------------------------------
                    else if (SP_Active > 2)
                    {
                        sb = new StringBuilder(); // clear sb string over again

                        try // use try since its a socket and can fail
                        {
                           SP_reader.BaseStream.ReadTimeout = 5000; // 5000 cause character Read to break every 5 seconds to check age of DX spots

                      //-------------------------------------------------------------------------------------------------------------------------------------
                      // ke9ns wait for a new message

                            for (;!(sb.ToString().Contains("\r\n"));) //  wait for end of line
                            {
                                sb.Append((char)SP_reader.Read());  // get next char from socket and add it to build the next dx spot string to parse out 
                             

                                if (SP_Active == 0)
                                 {
                                    Trace.WriteLine("break====="); // if user wants to shut down operation 
                                    break;
                                }

                                if (sb.ToString().Length > 90)
                                {
                                    Trace.WriteLine("Leng ====="); // string too long (something happened
                                    sb = new StringBuilder(); // clear sb string over again
                                }


                            }// for (;!(sb.ToString().Contains("\r\n"));) //  wait for end of line
                      //-------------------------------------------------------------------------------------------------------------------------------------


                            statusBox.ForeColor = Color.Green;
                            statusBox.Text = "Spotting";
                            SP_Active = 3;

                          
                            sb.Replace("\a", "");// get rig of bell 
                            sb.Replace("\r", "");// get rig of cr 
                            sb.Replace("\n", "");// get rig of line feed 

                            int qq = sb.Length;
                          //  Trace.WriteLine("message1 length " + qq);

                            if (qq == 75) // if no grid, then add spaces and CR and line feed
                            {
                                sb.Append("     "); // keep all strings the same length
                            }
                           
                            message1 = sb.ToString(); // post message
                            message1.TrimEnd('\0');

                            // ke9ns so at this point all messages are 82 characters long (as though they have a grid#, even if they dont)
                            //   Trace.WriteLine("message2 length " + message1.Length);

                        }
                        catch // read timeout comes here
                        {

                            if (
                               ((chkSUN.Checked == true) || (chkGrayLine.Checked == true)) && 
                               ((Display.CurrentDisplayMode == DisplayMode.PANADAPTER) || (Display.CurrentDisplayMode == DisplayMode.PANAFALL))
                                )

                            {
                              
                                if (chkSUN.Checked == true) SUN = true; // activate display
                                else SUN = false;
                                if (chkGrayLine.Checked == true) GRAYLINE = true; // activate display
                                else GRAYLINE = false;

                              

                                if ((UTCNEW != UTCLAST2)) // do only 1 time per minute
                                {
                                    UTCLAST2 = UTCNEW;

                                    //=================================================================================================
                                    //=================================================================================================
                                    // ke9ns Position of SUN (viewed from SUN) using the ke9ns world skin 
                                    // X  left edge starts 5.6% in, right edge ends 94.7%  (with Display.DIS_X at 100%)
                                    // y  top edge starts 7.8% in, bottom edge ends 90.1%  (with Display.DIS_Y at 100%)
                                    // robinson project map has longitude lines every 15deg (1 per hour) 300deg mark at 0330UTC and sun moves to the left
                                    // left edge is 2359 UTC (right edge is 0 UTC)

                                    //  Trace.WriteLine("Mouse x " + Console.DX_X); // mouse on picDisplay coordinates
                                    //  Trace.WriteLine("mouse y " + Console.DX_Y);

                                    //   Trace.WriteLine("x pos " + Display.DIS_X); // 
                                    //  Trace.WriteLine("y pos " + Display.DIS_Y);

                                    double g = (double)(360 / 365.25 * (DateTime.UtcNow.DayOfYear + (DateTime.UtcNow.Hour / 24))); // convert days to angle
                                  
                                    g = g * Math.PI / 180; // convert angle to rads
                                    
                                    double D = (double)(0.396372 - (22.91327 * Math.Cos(g)) + (4.02543 * Math.Sin(g)) - (0.387205 * Math.Cos(2 * g))
                                        + (0.051967 * Math.Sin(2 * g)) - (0.154527 * Math.Cos(3 * g)) + (0.084798 * Math.Sin(3 * g)));

                                    D = D / 24; // convert to percent of 100

                                    Sun_Top1 = (int)((double)(Display.DIS_Y * 0.078)); // Y pixel location of top of map
                                    Sun_Bot1 = (int)((double)(Display.DIS_Y * 0.901)); // Y pixel locaiton of bottom of map 
                                    int Sun_WidthY1 = Sun_Bot1 - Sun_Top1;             // # of Y pixels from top to bottom of map

                                    int Sun_Top = (int)((double)(Display.DIS_Y * 0.365)); // y pixel location North Hem summer solstice
                                    int Sun_Bot = (int)((double)(Display.DIS_Y * 0.606));  // Y pixel location of North Hem winter solstice

                                    int Sun_WidthY = Sun_Bot - Sun_Top; // # of Y pixes width between solstices
                                    int Sun_WidthHalf = ((Sun_Bot - Sun_Top) / 2) + Sun_Top; // 223

                                    int Sun_Diff = Sun_WidthHalf - Sun_Top; // 54

                                    Sun_Y = (int)(Sun_WidthHalf - (double)(Sun_Diff * D)); // position of SUN on longitude of map (based of time of year)


                                    UTCD = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

                                  //  if (pause == false) test++;
                                  //  if (test > 23) test = 0;
                                   
                                    FD = UTCD.ToString("HH");
                                    UTC100 = Convert.ToInt16(FD);
                                    FD = UTCD.ToString("mm");

                                   
                                   UTC100A = (UTC100 + (Convert.ToInt16(FD) / 60F)); // used by SUNANGLE routine

                                 //   UTC100A = (test + (Convert.ToInt16(FD) / 60F));

                                   UTC100 = (UTC100 + (Convert.ToInt16(FD) / 60F)) / 24F; // used by SUN track routine convert to 100% 2400 = 100%


                                    int Sun_Left = (int)((double)(Display.DIS_X * 0.056)); // .056 ratio of left side of map at equator
                                    int Sun_Right = (int)((double)(Display.DIS_X * 0.940));// .947 ratio of right side of map at equator

                                    Sun_Left1 = Sun_Left; // used by Grayline routine
                                    Sun_Right1 = Sun_Right;


                                    int Sun_Width = Sun_Right - Sun_Left; //used by sun track routine


                                    Sun_X = (int)(Sun_Left + (float)(Sun_Width * (1.0 - UTC100))); // position of SUN on equator based on time of day

                                    if ((GRAYLINE == true)  )
                                    {

                                        //-------------------------------------------------------------------
                                        // ke9ns grayline check 
                                        // horizontal lines top to bottom (North)90 to 0 to (-SOUTH)90
                                        // vertical lines left to right  -West(180) to 0 to +East(180)
                                        //
                                        // Sunset:                SUNANGLE = 90    (horizon = 90deg)
                                        // Civil Twilight:        SUNANGLE = 91 to 95 
                                        // Civil Dusk:            SUNANGLE = 96 
                                        // Nautical Twilight:     SUNANGLE = 97 to 101
                                        // Nautical Dusk:         SUNANGLE = 102
                                        // Astronomical Twilight: SUNANGLE = 103 to 107
                                        // Astronomical Dusk:     SUNANGLE = 108
                                        // Night:                 SUNANGLE > 108


                                        // ftp://ftp.swpc.noaa.gov/pub/latest/wwv.txt
                                   
                                        /*
                                        :Product: Geophysical Alert Message wwv.txt
                                        : Issued: 2016 Mar 31 2110 UTC
                                        # Prepared by the US Dept. of Commerce, NOAA, Space Weather Prediction Center
                                        #
                                        #          Geophysical Alert Message
                                        #
                                        Solar - terrestrial indices for 31 March follow.
                                          Solar flux 82 and estimated planetary A - index 7.
                                          The estimated planetary K - index at 2100 UTC on 31 March was 0.

                                          No space weather storms were observed for the past 24 hours.

                                          No space weather storms are predicted for the next 24 hours

                                     */

                                        int qq = 0;       // index for accumulating lat edges
                                        int ww = 0;       // 0=no edges found, 1=1 edge found, 2=2 edges found

                                        int tt = 0;
                                        bool check_for_light = true; // true = in the dark, so looking for light, false = in the light, so looking for the dark
                                        int tempsun_ang = 0; // temp holder for sun angle


                                        //----------------------------------------------------------------------------------
                                        //----------------------------------------------------------------------------------
                                        // check for Dark (Right side of map)
                                        //----------------------------------------------------------------------------------
                                        //----------------------------------------------------------------------------------

                                        for (int lat = 90; lat >= -90; lat--)  // horizontal lines top to bottom (North)90 to 0 to -90 (South)
                                        {

                                           
                                            if ((SUNANGLE(lat, -180) >= 96) && (SUNANGLE(lat, 180) >= 96)) tt = 1; // dark on edges of screen 
                                            else    tt = 0; // light on at least 1 side

                                            zz = (int)((qq / 180.0 * Sun_WidthY1) + Sun_Top1); // determine the y pixel for this latitude grayline

                                            GrayLine_Pos[zz, 0] = GrayLine_Pos[zz, 1] = 0;


                                            if (SUNANGLE(lat, 0) < 96) check_for_light = false; // your in light so check for dark
                                            else check_for_light = true; // >= 96 your in dark so check for light



                                            for (int lon = 0; lon <= 180; lon++)  
                                            {
                                                tempsun_ang = SUNANGLE(lat, lon); // pos angle from 0 to 120

                                              
                                                if (check_for_light == true) // in dark, looking for light
                                                {

                                                    if (tempsun_ang < 96) // found light
                                                    {
    
                                                        GrayLine_Pos[zz, ww] = (int)(( (lon + 180.0) / 360.0 * Sun_Width) + Sun_Left); // determine x pixel for this longitude grayline

                                                        GrayLine_Pos[zz + 2, ww] = GrayLine_Pos[zz + 1, ww] = GrayLine_Pos[zz, ww]; // make sure to cover unused pixels

                                                        ww++;
                                                        if (ww == 2) break;   // both edges found so done

                                                        lon = lon + 30; // jump a little to save time
                                                        check_for_light = false; // now in light so check for dark

                                                    } // found light

                                                } // your in dark so check for light
                                                else // in light so check for dark
                                                {

                                                    if (tempsun_ang >= 96) // in Dark (found it)
                                                    {

                                                        GrayLine_Pos[zz, ww] = (int)(((lon + 180.0) / 360.0 * Sun_Width) + Sun_Left); // determine x pixel for this longitude grayline

                                                        GrayLine_Pos[zz + 2, ww] = GrayLine_Pos[zz + 1, ww] = GrayLine_Pos[zz, ww]; // make sure to cover unused pixels

                                                        ww++;
                                                        if (ww == 2) break;   // both edges found so done

                                                        lon = lon + 30;         // jump a little to save time
                                                        check_for_light = true; // in dark so now check for light

                                                    } // found dark


                                                }// in light so check for dark


                                            } // for lon (right side of map)

                                            //----------------------------------------------------------------------------------
                                            //----------------------------------------------------------------------------------
                                            // check for Dark (left side of map
                                            //----------------------------------------------------------------------------------
                                            //----------------------------------------------------------------------------------

                                            if (ww < 2) // still need at least 1 edge (maybe 2)
                                            {

                                                if (SUNANGLE(lat, 0) < 96) check_for_light = false; // your in light so check for dark
                                                else check_for_light = true; // >= 90 your in dark so check for light

                                                for (int lon = 0; lon >= -180; lon--)  // vertical lines left to right 0 to -180 (west) (check left side of map)
                                                {
                                                    tempsun_ang = SUNANGLE(lat, lon);
                                                  
                                                    if (check_for_light == true)
                                                    {

                                                        if (tempsun_ang < 96) // found light
                                                        {

                                                            GrayLine_Pos[zz, ww] = (int)(( (180.0 + lon) / 360.0 * Sun_Width) + Sun_Left); // determine x pixel for this longitude grayline
                                                            GrayLine_Pos[zz + 2, ww] = GrayLine_Pos[zz + 1, ww] = GrayLine_Pos[zz, ww];

                                                            ww++;
                                                            if (ww == 2) break;      // if we have 2 edge then done

                                                            lon = lon - 30;           // jump a little to save time
                                                            check_for_light = false; // now in light so check for dark

                                                        } // found light

                                                    } // your in dark so check for light
                                                    else // in light so check for dark
                                                    {

                                                        if (tempsun_ang >= 96) // in Dark (found it)
                                                        {
                                                            GrayLine_Pos[zz, ww] = (int)(((180.0 + lon) / 360.0 * Sun_Width) + Sun_Left); // determine x pixel for this longitude grayline

                                                            GrayLine_Pos[zz + 2, ww] = GrayLine_Pos[zz + 1, ww] = GrayLine_Pos[zz, ww];

                                                            ww++;
                                                            if (ww == 2) break;    // if we have 2 edge then done

                                                            lon = lon - 30;         // jump a little to save time
                                                            check_for_light = true; // in dark so now check for light

                                                        } // found dark


                                                    }// in light so check for dark


                                                } // for lon  (left side of map)


                                            } // ww as < 2 on the right side attempt


                                            if (ww == 0) // if still less than 2 edges then just zero out
                                            {

                                                GrayLine_Pos[zz + 2, 0] = GrayLine_Pos[zz + 1, 0] = GrayLine_Pos[zz, 0] = 0;
                                                GrayLine_Pos[zz + 2, 1] = GrayLine_Pos[zz + 1, 1] = GrayLine_Pos[zz, 1] = 0;

                                            }
                                            else if (ww == 1)
                                            {

                                                GrayLine_Pos[zz + 2, 0] = GrayLine_Pos[zz + 1, 0] = GrayLine_Pos[zz, 0] = GrayLine_Pos[zz, 0] + GrayLine_Pos[zz, 1];
                                                GrayLine_Pos[zz + 2, 1] = GrayLine_Pos[zz + 1, 1] = GrayLine_Pos[zz, 1]= GrayLine_Pos[zz, 0];

                                            }

                                            ww = 0; // start over for next lat

                                            if (tt == 1) // if dark on both edges then figure out which is which and signal display
                                            {

                                                if ((GrayLine_Pos[zz, 0] - GrayLine_Pos[zz, 1]) > 0)
                                                {
                                               
                                                    GrayLine_Pos2[zz + 2] = GrayLine_Pos2[zz + 1] = GrayLine_Pos2[zz] = 1; // ,0 is on right side, ,1 is on left side
                                                }
                                                else if ((GrayLine_Pos[zz, 1] - GrayLine_Pos[zz, 0]) > 0)
                                                {
                                                
                                                    GrayLine_Pos2[zz + 2] = GrayLine_Pos2[zz + 1] = GrayLine_Pos2[zz] = 2; // ,0 is on left side, ,1 is on right side
                                                }
                                                else
                                                {
                                                   GrayLine_Pos2[zz + 2] = GrayLine_Pos2[zz + 1] = GrayLine_Pos2[zz] = 0;             // dark in center of map, (standard)

                                                }

                                            }
                                            else
                                            {
                                            
                                                GrayLine_Pos2[zz + 2] = GrayLine_Pos2[zz + 1] = GrayLine_Pos2[zz] = 0;             // dark in center of map, (standard)
                                            }

                                            qq++; // get next lat

                                        } //  for (int lat = 90;lat >= -90;lat--)   horizontal lines top to bottom (North)90 to 0 to -90 (South)



                                        //-------------------------------------------------------------------
                                        //-------------------------------------------------------------------
                                        // check for dusk (right side first)
                                        //-------------------------------------------------------------------
                                        //-------------------------------------------------------------------
                                        qq = 0;
                                        ww = 0;

                                        for (int lat = 90; lat >= -90; lat--)  // horizontal lines top to bottom (North)90 to 0 to -90 (South)
                                        {


                                            if ((SUNANGLE(lat, -180) >= 90) && (SUNANGLE(lat, 180) >= 90)) tt = 1; // dark on edges of screen 
                                            else tt = 0; // light on at least 1 side

                                            zz = (int)((qq / 180.0 * Sun_WidthY1) + Sun_Top1); // determine the y pixel for this latitude grayline

                                            GrayLine_Pos1[zz, 0] = GrayLine_Pos1[zz, 1] = 0;

                                            if (SUNANGLE(lat, 0) < 90) check_for_light = false; // your in light so check for dark
                                            else check_for_light = true; // >= 96 your in dark so check for light


                                            for (int lon = 0; lon <= 180; lon++)
                                            {
                                                tempsun_ang = SUNANGLE(lat, lon); // pos angle from 0 to 120


                                                if (check_for_light == true) // in dark, looking for light
                                                {

                                                    if (tempsun_ang < 90) // found light
                                                    {

                                                        GrayLine_Pos1[zz, ww] = (int)(((lon + 180.0) / 360.0 * Sun_Width) + Sun_Left); // determine x pixel for this longitude grayline

                                                        GrayLine_Pos1[zz + 2, ww] = GrayLine_Pos1[zz + 1, ww] = GrayLine_Pos1[zz, ww]; // make sure to cover unused pixels

                                                        ww++;
                                                        if (ww == 2) break;   // both edges found so done

                                                         lon = lon + 30; // jump a little to save time
                                                        check_for_light = false; // now in light so check for dark

                                                    } // found light

                                                } // your in dark so check for light
                                                else // in light so check for dark
                                                {

                                                    if (tempsun_ang >= 90) // in Dark (found it)
                                                    {

                                                        GrayLine_Pos1[zz, ww] = (int)(((lon + 180.0) / 360.0 * Sun_Width) + Sun_Left); // determine x pixel for this longitude grayline

                                                        GrayLine_Pos1[zz + 2, ww] = GrayLine_Pos1[zz + 1, ww] = GrayLine_Pos1[zz, ww]; // make sure to cover unused pixels

                                                        ww++;
                                                        if (ww == 2) break;   // both edges found so done

                                                          lon = lon + 30;         // jump a little to save time
                                                        check_for_light = true; // in dark so now check for light

                                                    } // found dark


                                                }// in light so check for dark


                                            } // for lon (right side of map)

                                            //----------------------------------------------------------------------------------
                                            //----------------------------------------------------------------------------------
                                            // check for Dark (left side of map
                                            //----------------------------------------------------------------------------------
                                            //----------------------------------------------------------------------------------

                                            if (ww < 2) // still need at least 1 edge (maybe 2)
                                            {

                                                if (SUNANGLE(lat, 0) < 90) check_for_light = false; // your in light so check for dark
                                                else check_for_light = true; // >= 90 your in dark so check for light

                                                for (int lon = 0; lon >= -180; lon--)  // vertical lines left to right 0 to -180 (west) (check left side of map)
                                                {
                                                    tempsun_ang = SUNANGLE(lat, lon);

                                                    if (check_for_light == true)
                                                    {

                                                        if (tempsun_ang < 90) // found light
                                                        {

                                                            GrayLine_Pos1[zz, ww] = (int)(((180.0 + lon) / 360.0 * Sun_Width) + Sun_Left); // determine x pixel for this longitude grayline
                                                            GrayLine_Pos1[zz + 2, ww] = GrayLine_Pos1[zz + 1, ww] = GrayLine_Pos1[zz, ww];

                                                            ww++;
                                                            if (ww == 2) break;      // if we have 2 edge then done

                                                            //  lon = lon - 60;           // jump a little to save time
                                                            check_for_light = false; // now in light so check for dark

                                                        } // found light

                                                    } // your in dark so check for light
                                                    else // in light so check for dark
                                                    {

                                                        if (tempsun_ang >= 90) // in Dark (found it)
                                                        {
                                                            GrayLine_Pos1[zz, ww] = (int)(((180.0 + lon) / 360.0 * Sun_Width) + Sun_Left); // determine x pixel for this longitude grayline

                                                            GrayLine_Pos1[zz + 2, ww] = GrayLine_Pos1[zz + 1, ww] = GrayLine_Pos1[zz, ww];

                                                            ww++;
                                                            if (ww == 2) break;    // if we have 2 edge then done

                                                              lon = lon - 30;         // jump a little to save time
                                                            check_for_light = true; // in dark so now check for light

                                                        } // found dark


                                                    }// in light so check for dark


                                                } // for lon  (left side of map)


                                            } // ww as < 2 on the right side attempt


                                            if (ww == 0) // if still less than 2 edges then just zero out
                                            {

                                                GrayLine_Pos1[zz + 2, 0] = GrayLine_Pos1[zz + 1, 0] = GrayLine_Pos1[zz, 0] = 0;
                                                GrayLine_Pos1[zz + 2, 1] = GrayLine_Pos1[zz + 1, 1] = GrayLine_Pos1[zz, 1] = 0;

                                            }
                                            else if (ww == 1)
                                            {

                                               
                                                GrayLine_Pos1[zz + 2, 0] = GrayLine_Pos1[zz + 1, 0] = GrayLine_Pos1[zz, 0] = GrayLine_Pos1[zz, 0] + GrayLine_Pos1[zz, 1];
                                                GrayLine_Pos1[zz + 2, 1] = GrayLine_Pos1[zz + 1, 1] = GrayLine_Pos1[zz, 1] = GrayLine_Pos1[zz, 0];

                                            }

                                            ww = 0; // start over for next lat

                                            if (tt == 1) // if dark on both edges then figure out which is which and signal display
                                            {

                                                if ((GrayLine_Pos1[zz, 0] - GrayLine_Pos1[zz, 1]) > 0)
                                                {
                                                   
                                                    GrayLine_Pos3[zz + 2] = GrayLine_Pos3[zz + 1] = GrayLine_Pos3[zz] = 1; // ,0 is on right side, ,1 is on left side
                                                }
                                                else if ((GrayLine_Pos1[zz, 1] - GrayLine_Pos1[zz, 0]) > 0)
                                                {
                                                  
                                                    GrayLine_Pos3[zz + 2] = GrayLine_Pos3[zz + 1] = GrayLine_Pos3[zz] = 2; // ,0 is on left side, ,1 is on right side
                                                }
                                                else
                                                {
                                                     GrayLine_Pos3[zz + 2] = GrayLine_Pos3[zz + 1] = GrayLine_Pos3[zz] = 0;             // dark in center of map, (standard)

                                                }

                                            }
                                            else
                                            {
                                               
                                                GrayLine_Pos3[zz + 2] = GrayLine_Pos3[zz + 1] = GrayLine_Pos3[zz] = 0;             // dark in center of map, (standard)
                                            }

                                            qq++; // get next lat

                                        } //  for (int lat = 90;lat >= -90;lat--)   horizontal lines top to bottom (North)90 to 0 to -90 (South)








                                    } // GRAYLINE = true


                                } // check every 1 minutes

                            } // only check in in panadapter mode since you cant see it in any other mode
                            else
                            {
                                SUN = false;
                                GRAYLINE = false;
                            }



                            //=================================================================================================
                            //=================================================================================================
                            // ke9ns check for TIME EXPIRES  

                            UTCD = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                            FD = UTCD.ToString("HHmm");                                       // get 24hr 4 digit UTC NOW
                            UTCNEW = Convert.ToInt16(FD);                                    // convert 24hr UTC to int

                            if ((UTCLAST == 0) && (UTCNEW != 0))
                            {
                                UTCLAST = UTCNEW;        // for startup only
                                DX_Time[0] = UTCNEW;

                            }

                            if ((DX_Index > 0) && (UTCNEW != UTCLAST))
                            {
                                int xxx = 0;

                                UTCLAST = UTCNEW;
                          
                                Trace.WriteLine("Start=========== " + DX_Index);

                                for (int ii = DX_Index - 1; ii >= 0; ii--) // move from bottom of list up toward top of list
                                {

                                    int UTCDIFF = Math.Abs(UTCNEW - DX_Time[ii]); // time difference 
                                    DX_Age[ii] = UTCDIFF.ToString("00"); // 2 digits


                                    int kk = 0; // look at very bottom of list + 1


                                    if (UTCDIFF > 35) // if its an old SPOT then remove it from the list
                                    {

                                        kk = ii; // 

                                        xxx++; //shorten dx_Index by 1

                                        Trace.WriteLine("time expire, remove=========spot " + DX_Time[ii] + " current time " + UTCLAST + " UTCDIFF " + UTCDIFF + " ii " + ii + " station " + DX_Station[ii]);
                                        Trace.WriteLine("KK " + kk);
                                        Trace.WriteLine("XXX " + xxx);


                                        for (; kk < (DX_Index - xxx); kk++)
                                        {

                                            DX_FULLSTRING[kk] = DX_FULLSTRING[kk + xxx]; // 

                                            DX_Station[kk] = DX_Station[kk + xxx];
                                            DX_Freq[kk] = DX_Freq[kk + xxx];
                                            DX_Spotter[kk] = DX_Spotter[kk + xxx];
                                            DX_Message[kk] = DX_Message[kk + xxx];
                                            DX_Grid[kk] = DX_Grid[kk + xxx];
                                            DX_Time[kk] = DX_Time[kk + xxx];
                                            DX_Age[kk] = DX_Age[kk + xxx];
                                            DX_Mode[kk] = DX_Mode[kk + xxx];
                                            DX_Mode2[kk] = DX_Mode2[kk + xxx];

                                        } // for loop:  push OK Spots from bottom of list up as you delete old spots from list

                                    } // TIMEOUT exceeded remove old spot



                                } // for ii check for dup in list

                                DX_Index = DX_Index - xxx;  // update DX_Index list (shorten if any old spots deleted)

                                Trace.WriteLine("END=========== " + DX_Index);

                                Trace.WriteLine(" ");

                                processTCPMessage(); // update spot window (remove old spots)

                                continue;

                            } // UTC NEW != LAST
                            else
                            {
                                continue; // skip
                            }


                        } // end of catch (read timeout comes here)
                      


                        //-------------------------------------------------------------------------------------------------------------------------------------
                        // ke9ns process received message
                        if ((message1.StartsWith("DX de ") == true) && (message1.Length > 76)) // string can be 77 (with no grid) or 82 (with grid)
                        {
                          
                            DX_Index1 = 250; // use 900 as a temp holding spot. always fill from the top


                            // grab DX_Spotter=======================================================================================
                            try
                            {
                                DX_Spotter[DX_Index1] = message1.Substring(6, 10); // get dx call with : at the end
                                                                                   //  Trace.WriteLine("DX_Call " + DX_Station[DX_Index1]);
                                int pos = DX_Spotter[DX_Index1].IndexOf(':'); // find the :
                                DX_Spotter[DX_Index1] = DX_Spotter[DX_Index1].Substring(0, pos); // reduce the call without the :

                                sb = new StringBuilder(DX_Spotter[DX_Index1]); // clear sb string over again
                                sb.Append('>');
                                sb.Insert(0, '<'); // to differentiate the spotter from the spotted

                                DX_Spotter[DX_Index1] = sb.ToString();

                            }
                            catch (FormatException e)
                            {
                                DX_Spotter[DX_Index1] = "NA";
                           
                            //    textBox1.Text = e.ToString();
                            }
                            catch (ArgumentOutOfRangeException e)
                            {
                           
                            //    textBox1.Text = e.ToString();
                            }
                            //    Trace.WriteLine("DX_Call " + DX_Station[DX_Index1]);

                            // grab DX_Freq========================================================================================
                            try
                            {
                               
                                DX_Freq[DX_Index1] = (int)((double)Convert.ToDouble(message1.Substring(13, 11)) * (double) 1000.0    ); // get dx freq 7016.0  in khz 
                        
                                if ((DX_Freq[DX_Index1] >= 1800000) && (DX_Freq[DX_Index1] <= 1830000))
                                {
                                    DX_Mode[DX_Index1] = 1; // cw mode
                                }
                                else if ((DX_Freq[DX_Index1] >= 3500000) && (DX_Freq[DX_Index1] <= 3600000))
                                {
                                     DX_Mode[DX_Index1] = 1; // cw mode
                                }
                                else if ((DX_Freq[DX_Index1] >= 7000000) && (DX_Freq[DX_Index1] <= 7125000))
                                {
                                    DX_Mode[DX_Index1] = 1; // cw mode
                                }
                                else if ((DX_Freq[DX_Index1] >= 10000000) && (DX_Freq[DX_Index1] <= 11000000))
                                {
                                    DX_Mode[DX_Index1] = 1; // cw mode
                                }
                                else if ((DX_Freq[DX_Index1] >= 14000000) && (DX_Freq[DX_Index1] <= 14150000))
                                {
                                    DX_Mode[DX_Index1] = 1; // cw mode
                                }
                                else if ((DX_Freq[DX_Index1] >= 18000000) && (DX_Freq[DX_Index1] <= 18110000))
                                {
                                    DX_Mode[DX_Index1] = 1; // cw mode
                                }
                                else if ((DX_Freq[DX_Index1] >= 21000000) && (DX_Freq[DX_Index1] <= 21200000))
                                {
                                    DX_Mode[DX_Index1] = 1; // cw mode
                                }
                                else if ((DX_Freq[DX_Index1] >= 24800000) && (DX_Freq[DX_Index1] <= 24930000))
                                {
                                    DX_Mode[DX_Index1] = 1; // cw mode
                                }
                                else if ((DX_Freq[DX_Index1] >= 28000000) && (DX_Freq[DX_Index1] <= 28300000))
                                {
                                    DX_Mode[DX_Index1] = 1; // cw mode
                                }
                                else if ((DX_Freq[DX_Index1] >= 50000000) && (DX_Freq[DX_Index1] <= 50100000))
                                {
                                     DX_Mode[DX_Index1] = 1; // cw mode
                                }
                                else if ((DX_Freq[DX_Index1] >= 144000000) && (DX_Freq[DX_Index1] <= 144100000))
                                {
                                    DX_Mode[DX_Index1] = 1; // cw mode
                                }
                                else if (
                                    (DX_Freq[DX_Index1] == 1885000) || (DX_Freq[DX_Index1] == 1900000) || (DX_Freq[DX_Index1] == 1945000) || (DX_Freq[DX_Index1] == 1985000)
                                    || (DX_Freq[DX_Index1] == 3825000) || (DX_Freq[DX_Index1] == 3870000) || (DX_Freq[DX_Index1] == 3880000) || (DX_Freq[DX_Index1] == 38850000)
                                     || (DX_Freq[DX_Index1] == 7290000) || (DX_Freq[DX_Index1] == 7295000) || (DX_Freq[DX_Index1] == 14286000) || (DX_Freq[DX_Index1] == 18150000)
                                     || (DX_Freq[DX_Index1] == 21285000) || (DX_Freq[DX_Index1] == 21425000) ||( (DX_Freq[DX_Index1] >= 29000000) && (DX_Freq[DX_Index1] < 29200000))
                                     || (DX_Freq[DX_Index1] == 50400000) || (DX_Freq[DX_Index1] == 50250000) || (DX_Freq[DX_Index1] == 144400000) || (DX_Freq[DX_Index1] == 144425000)
                                     || (DX_Freq[DX_Index1] == 144280000) || (DX_Freq[DX_Index1] == 144450000)

                                    )
                                {
                                    DX_Mode[DX_Index1] = 14; // AM mode
                                }
                                else if (
                                         ((DX_Freq[DX_Index1] >= 146000000) && (DX_Freq[DX_Index1] <= 148000000)) || ((DX_Freq[DX_Index1] >= 29200000) && (DX_Freq[DX_Index1] <= 29270000))
                                       || ((DX_Freq[DX_Index1] >= 144500000) && (DX_Freq[DX_Index1] <= 144900000))|| ((DX_Freq[DX_Index1] >= 145100000) && (DX_Freq[DX_Index1] <= 145500000))
                                       )
                                {
                                    DX_Mode[DX_Index1] = 114; // FM mode
                                }

                                else
                                {
                                    DX_Mode[DX_Index1] = 0; // ssb mode
                                }


                            } // try to determine if in the cw portion or ssb portion of each band
                            catch (FormatException e)
                            {
                                DX_Freq[DX_Index1] = 0;
                                DX_Mode[DX_Index1] = 0; // ssb mode
                            }
                            catch (ArgumentOutOfRangeException e)
                            {
                                DX_Freq[DX_Index1] = 0;
                                DX_Mode[DX_Index1] = 0; // ssb mode
                            }
                            //  Trace.WriteLine("DX_Freq " + DX_Freq[DX_Index1]);


                            // grad DX_Station=========================================================================================

                            try
                            {
                                DX_Station[DX_Index1] = message1.Substring(26, 13); // get dx call with : at the end
                                int pos = DX_Station[DX_Index1].IndexOf(' '); // find the
                                DX_Station[DX_Index1] = DX_Station[DX_Index1].Substring(0, pos); // reduce the call without the
                            }
                            catch (FormatException e)
                            {
                                DX_Spotter[DX_Index1] = "NA";
                              }
                            catch (ArgumentOutOfRangeException e)
                            {
                                DX_Spotter[DX_Index1] = "NA";
                            }
                            //  Trace.WriteLine("DX_Spotter " + DX_Spotter[DX_Index1]);

                            // grab comments
                            try
                            {
                                DX_Mode2[DX_Index1] = 0; // reset split hz

                                DX_Message[DX_Index1] = message1.Substring(39, 29).ToLower(); // get dx call with : at the end
                              

                                if (DX_Message[DX_Index1].Contains("cw"))
                                {
                                    DX_Mode[DX_Index1] = 1; // cw mode

                                }
                                else if (DX_Message[DX_Index1].Contains("rty") || DX_Message[DX_Index1].Contains("rtty"))
                                {
                                    DX_Mode[DX_Index1] = 2; // RTTY mode

                                }
                                else if (DX_Message[DX_Index1].Contains("psk"))
                                {
                                    DX_Mode[DX_Index1] = 3; // psk mode

                                }
                                else if (DX_Message[DX_Index1].Contains("oliv"))
                                {
                                    DX_Mode[DX_Index1] = 4; // olivia mode

                                }
                                else if (DX_Message[DX_Index1].Contains("jt65"))
                                {
                                    DX_Mode[DX_Index1] = 5; // jt65 mode

                                }
                                else if (DX_Message[DX_Index1].Contains("contesa"))
                                {
                                    DX_Mode[DX_Index1] = 6; // contesa mode

                                }
                                else if (DX_Message[DX_Index1].Contains("fsk"))
                                {
                                    DX_Mode[DX_Index1] = 7; // fsk mode

                                }
                                else if (DX_Message[DX_Index1].Contains("mt63"))
                                {
                                    DX_Mode[DX_Index1] = 8; // mt63 mode

                                }
                                else if (DX_Message[DX_Index1].Contains("domi"))
                                {
                                    DX_Mode[DX_Index1] = 9; // domino mode

                                }
                                else if (DX_Message[DX_Index1].Contains("packact")|| DX_Message[DX_Index1].Contains("packtor")||DX_Message[DX_Index1].Contains("amtor"))
                                {
                                    DX_Mode[DX_Index1] = 10; // pactor mode

                                }
                                else if (DX_Message[DX_Index1].Contains("fm "))
                                {
                                    DX_Mode[DX_Index1] = 11; // fm mode

                                }
                                else if (DX_Message[DX_Index1].Contains("drm"))
                                {
                                    DX_Mode[DX_Index1] = 12; // DRM mode

                                }
                                else if (DX_Message[DX_Index1].Contains("sstv"))
                                {
                                    DX_Mode[DX_Index1] = 13; // sstv mode

                                }
                                else if (DX_Message[DX_Index1].Contains("easypal"))
                                {
                                    DX_Mode[DX_Index1] = 12; // drm mode

                                }
                                else if (DX_Message[DX_Index1].Contains(" am ")|| DX_Message[DX_Index1].Contains(" sam "))
                                {
                                    DX_Mode[DX_Index1] = 14; // AM mode

                                }


                                DX_Mode2[DX_Index1] = 0;
                                //  resultString = Regex.Match(subjectString, @"\d+").Value;  Int32.Parse(resultString) will then give you the number.

                                if (DX_Message[DX_Index1].Contains("up")) // check for split
                                {
                                  
                                    int ind = DX_Message[DX_Index1].IndexOf("up") + 2;

                                    try // try 1
                                    {
                                        int split_hz = (int)(Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 4))* 1000));
                                        Trace.WriteLine("Found UP split hz" +split_hz);
                                        DX_Mode2[DX_Index1] = split_hz;
                                    }
                                    catch // catch 1
                                    {

                                        try // try 2
                                        {
                                            int split_hz = (int)(Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 3)) * 1000));
                                            Trace.WriteLine("Found UP split hz" + split_hz);
                                            DX_Mode2[DX_Index1] = split_hz;
                                        }
                                        catch // catch 2
                                        {

                                            try // try 3
                                            {
                                                int split_hz = (int)(Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 2)) * 1000));
                                                Trace.WriteLine("Found UP split hz" + split_hz);
                                                DX_Mode2[DX_Index1] = split_hz;
                                            }
                                            catch // catch 3
                                            {

                                                int ind1 = DX_Message[DX_Index1].IndexOf("up") - 4; //

                                                try // try 4
                                                {

                                                    int split_hz = (int)(Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1,4)) * 1000));
                                                    Trace.WriteLine("Found UP split hz" + split_hz);
                                                    DX_Mode2[DX_Index1] = split_hz;
                                                }
                                                catch // catch 4
                                                {
                                                    ind1++; //

                                                    try // try 5
                                                    {

                                                        int split_hz = (int)(Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 3)) * 1000));
                                                        Trace.WriteLine("Found UP split hz" + split_hz);
                                                        DX_Mode2[DX_Index1] = split_hz;
                                                    }
                                                    catch // catch 5
                                                    {
                                                        ind1++; //

                                                        try // try 6
                                                        {

                                                            int split_hz = (int)(Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 2)) * 1000));
                                                            Trace.WriteLine("Found UP split hz" + split_hz);
                                                            DX_Mode2[DX_Index1] = split_hz;
                                                        }
                                                        catch // catch 6
                                                        {

                                                            Trace.WriteLine("failed to find up value================");
                                                            DX_Mode2[DX_Index1] = 1000; // 1khz up

                                                        } // catch6   (2 digits to left side)
                                                    
                                                    } // catch5   (3 digits to left side)
                                                   
                                                } // catch4   (4 digits to left side)
                                        
                                            } // catch3   (2 digits to right side)

                                        } //catch2  (3 digits to right side)

                                    } // catch 1   (4 digits to right side)


                                } // split up
                               
                                else if ( DX_Message[DX_Index1].Contains("dn")) // check for split down
                                {
                                 
                                    int ind = DX_Message[DX_Index1].IndexOf("dn") + 2;

                                    try // try 1
                                    {
                                        int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 4)) * 1000));
                                        Trace.WriteLine("Found dn split hz" + split_hz);
                                        DX_Mode2[DX_Index1] = split_hz;
                                    }
                                    catch // catch 1
                                    {

                                        try // try 2
                                        {
                                            int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 3)) * 1000));
                                            Trace.WriteLine("Found dn split hz" + split_hz);
                                            DX_Mode2[DX_Index1] = split_hz;
                                        }
                                        catch // catch 2
                                        {

                                            try // try 3
                                            {
                                                int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 2)) * 1000));
                                                Trace.WriteLine("Found dn split hz" + split_hz);
                                                DX_Mode2[DX_Index1] = split_hz;
                                            }
                                            catch // catch 3
                                            {

                                                int ind1 = DX_Message[DX_Index1].IndexOf("dn") - 4; //

                                                try // try 4
                                                {
                                                    int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 4)) * 1000));
                                                    Trace.WriteLine("Found dn split hz" + split_hz);
                                                    DX_Mode2[DX_Index1] = split_hz;
                                                }
                                                catch // catch 4
                                                {
                                                    ind++; //

                                                    try // try 5
                                                    {
                                                        int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 3)) * 1000));
                                                        Trace.WriteLine("Found dn split hz" + split_hz);
                                                        DX_Mode2[DX_Index1] = split_hz;
                                                    }
                                                    catch // catch 5
                                                    {
                                                        ind1++; //

                                                        try // try 6
                                                        {
                                                            int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 2)) * 1000));
                                                            Trace.WriteLine("Found dn split hz" + split_hz);
                                                            DX_Mode2[DX_Index1] = split_hz;
                                                        }
                                                        catch // catch 6
                                                        {

                                                            Trace.WriteLine("failed to find dn value================");
                                                            DX_Mode2[DX_Index1] = -1000; // 1khz dn

                                                        } // catch6   (2 digits to left side)

                                                    } // catch5   (3 digits to left side)

                                                } // catch4   (4 digits to left side)

                                            } // catch3   (2 digits to right side)

                                        } //catch2  (3 digits to right side)

                                    } // catch 1   (4 digits to right side)


                                } // split down
                                else if (DX_Message[DX_Index1].Contains("dwn")) // check for split down
                                {

                                    int ind = DX_Message[DX_Index1].IndexOf("dwn") + 3;

                                    try // try 1
                                    {
                                        int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 4)) * 1000));
                                        Trace.WriteLine("Found dn split hz" + split_hz);
                                        DX_Mode2[DX_Index1] = split_hz;
                                    }
                                    catch // catch 1
                                    {

                                        try // try 2
                                        {
                                            int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 3)) * 1000));
                                            Trace.WriteLine("Found dn split hz" + split_hz);
                                            DX_Mode2[DX_Index1] = split_hz;
                                        }
                                        catch // catch 2
                                        {

                                            try // try 3
                                            {
                                                int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 2)) * 1000));
                                                Trace.WriteLine("Found dn split hz" + split_hz);
                                                DX_Mode2[DX_Index1] = split_hz;
                                            }
                                            catch // catch 3
                                            {

                                                int ind1 = DX_Message[DX_Index1].IndexOf("dwn") - 4; //

                                                try // try 4
                                                {
                                                    int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 4)) * 1000));
                                                    Trace.WriteLine("Found dn split hz" + split_hz);
                                                    DX_Mode2[DX_Index1] = split_hz;
                                                }
                                                catch // catch 4
                                                {
                                                    ind1++; //

                                                    try // try 5
                                                    {
                                                        int split_hz = (int)(-Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 3)) * 1000);
                                                        Trace.WriteLine("Found dn split hz" + split_hz);
                                                        DX_Mode2[DX_Index1] = split_hz;
                                                    }
                                                    catch // catch 5
                                                    {
                                                        ind1++; //

                                                        try // try 6
                                                        {
                                                            int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 2)) * 1000));
                                                            Trace.WriteLine("Found dn split hz" + split_hz);
                                                            DX_Mode2[DX_Index1] = split_hz;
                                                        }
                                                        catch // catch 6
                                                        {

                                                            Trace.WriteLine("failed to find dn value================");
                                                            DX_Mode2[DX_Index1] = -1000; // 1khz dn

                                                        } // catch6   (2 digits to left side)

                                                    } // catch5   (3 digits to left side)

                                                } // catch4   (4 digits to left side)

                                            } // catch3   (2 digits to right side)

                                        } //catch2  (3 digits to right side)

                                    } // catch 1   (4 digits to right side)


                                } // split down
                                else if (DX_Message[DX_Index1].Contains("down")) // check for split down
                                {

                                    int ind = DX_Message[DX_Index1].IndexOf("down") + 4;

                                    try // try 1
                                    {
                                        int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 4)) * 1000));
                                        Trace.WriteLine("Found dn split hz" + split_hz);
                                        DX_Mode2[DX_Index1] = split_hz;
                                    }
                                    catch // catch 1
                                    {

                                        try // try 2
                                        {
                                            int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 3)) * 1000));
                                            Trace.WriteLine("Found dn split hz" + split_hz);
                                            DX_Mode2[DX_Index1] = split_hz;
                                        }
                                        catch // catch 2
                                        {

                                            try // try 3
                                            {
                                                int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 2)) * 1000));
                                                Trace.WriteLine("Found dn split hz" + split_hz);
                                                DX_Mode2[DX_Index1] = split_hz;
                                            }
                                            catch // catch 3
                                            {

                                                int ind1 = DX_Message[DX_Index1].IndexOf("down") - 4; //

                                                try // try 4
                                                {
                                                    int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 4)) * 1000));
                                                    Trace.WriteLine("Found dn split hz" + split_hz);
                                                    DX_Mode2[DX_Index1] = split_hz;
                                                }
                                                catch // catch 4
                                                {
                                                    ind1++; //

                                                    try // try 5
                                                    {
                                                        int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 3)) * 1000));
                                                        Trace.WriteLine("Found dn split hz" + split_hz);
                                                        DX_Mode2[DX_Index1] = split_hz;
                                                    }
                                                    catch // catch 5
                                                    {
                                                        ind1++; //

                                                        try // try 6
                                                        {
                                                            int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 2)) * 1000));
                                                            Trace.WriteLine("Found dn split hz" + split_hz);
                                                            DX_Mode2[DX_Index1] = split_hz;
                                                        }
                                                        catch // catch 6
                                                        {

                                                            Trace.WriteLine("failed to find dn value================");
                                                            DX_Mode2[DX_Index1] = -1000; // 1khz dn

                                                        } // catch6   (2 digits to left side)

                                                    } // catch5   (3 digits to left side)

                                                } // catch4   (4 digits to left side)

                                            } // catch3   (2 digits to right side)

                                        } //catch2  (3 digits to right side)

                                    } // catch 1   (4 digits to right side)

                                } // split down
                                else if ((DX_Message[DX_Index1].Contains("s9+")) || (DX_Message[DX_Index1].Contains("59+") )) // check for split
                                {
                                    // ignore + if its part of s9+
                                }

                                else if (DX_Message[DX_Index1].Contains("+")) // check for split
                                {

                                    int ind = DX_Message[DX_Index1].IndexOf("+") + 1;

                                    try // try 1
                                    {
                                        int split_hz = (int)(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 4)) * 1000);
                                        Trace.WriteLine("Found UP split hz" + split_hz);
                                        DX_Mode2[DX_Index1] = split_hz;
                                    }
                                    catch // catch 1
                                    {

                                        try // try 2
                                        {
                                            int split_hz = (int)(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 3)) * 1000);
                                            Trace.WriteLine("Found UP split hz" + split_hz);
                                            DX_Mode2[DX_Index1] = split_hz;
                                        }
                                        catch // catch 2
                                        {

                                            try // try 3
                                            {
                                                int split_hz = (int)(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 2)) * 1000);
                                                Trace.WriteLine("Found UP split hz" + split_hz);
                                                DX_Mode2[DX_Index1] = split_hz;
                                            }
                                            catch // catch 3
                                            {

                                                Trace.WriteLine("failed to find up value================");
                                                DX_Mode2[DX_Index1] = 0; // 


                                            } // catch3   (2 digits to right side)

                                        } //catch2  (3 digits to right side)

                                    } // catch 1   (4 digits to right side)

                                  //  if (DX_Mode2[DX_Index1] > 9000) DX_Mode2[DX_Index1] = 0;

                                } // split up+

                                else if (DX_Message[DX_Index1].Contains(" -")) // check for split
                                {

                                    int ind = DX_Message[DX_Index1].IndexOf("-") + 1;

                                    try // try 1
                                    {
                                        int split_hz = (int)(-Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 4)) * 1000);
                                        Trace.WriteLine("Found dn split hz" + split_hz);
                                        DX_Mode2[DX_Index1] = split_hz;
                                    }
                                    catch // catch 1
                                    {

                                        try // try 2
                                        {
                                            int split_hz = (int)(-Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 3)) * 1000);
                                            Trace.WriteLine("Found dn split hz" + split_hz);
                                            DX_Mode2[DX_Index1] = split_hz;
                                        }
                                        catch // catch 2
                                        {

                                            try // try 3
                                            {
                                                int split_hz = (int)(-Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 2)) * 1000);
                                                Trace.WriteLine("Found dn split hz" + split_hz);
                                                DX_Mode2[DX_Index1] = split_hz;
                                            }
                                            catch // catch 3
                                            {

                                                Trace.WriteLine("failed to find up value================");
                                                DX_Mode2[DX_Index1] = 0; // 


                                            } // catch3   (2 digits to right side)

                                        } //catch2  (3 digits to right side)

                                    } // catch 1   (4 digits to right side)


                                } // split dwn -

                                else if (DX_Message[DX_Index1].Contains("qsx")) // check for split
                                {

                                    int ind = DX_Message[DX_Index1].IndexOf("qsx") + 3;

                                    try // try 1
                                    {
                                        int split_hz = (int)(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 8)) * 1000);
                                        Trace.WriteLine("Found dn split hz" + split_hz);
                                        DX_Mode2[DX_Index1] = split_hz;

                                        DX_Mode2[DX_Index1] =  DX_Mode2[DX_Index1] - DX_Freq[DX_Index1];


                                    }
                                    catch // catch 1
                                    {

                                        try // try 2
                                        {
                                            int split_hz = (int)(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 7)) * 1000);

                                            if (split_hz < 10000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QRX .412  then treat it with the same mhz as DX_Freq
                                            else if (split_hz < 100000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QRX 18.412  then it must be in mhz

                                            Trace.WriteLine("Found qrx split hz" + split_hz);

                                            DX_Mode2[DX_Index1] = split_hz;
                                            DX_Mode2[DX_Index1] = DX_Mode2[DX_Index1] - DX_Freq[DX_Index1];


                                        }
                                        catch // catch 2
                                        {
                                            try // try 3
                                            {
                                                int split_hz = (int)(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 6)) * 1000);

                                                if (split_hz < 10000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QRX .412  then treat it with the same mhz as DX_Freq
                                                else if (split_hz < 100000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QRX 18.412  then it must be in mhz

                                                Trace.WriteLine("Found dn split hz" + split_hz);

                                                DX_Mode2[DX_Index1] = split_hz;
                                                DX_Mode2[DX_Index1] = DX_Mode2[DX_Index1] - DX_Freq[DX_Index1];


                                            }
                                            catch // catch 3
                                            {

                                                Trace.WriteLine("failed to find up value================");
                                                DX_Mode2[DX_Index1] = 0; // 

                                            } // catch 3   (6 digits to right side)
                                          
                                        } // catch 2   (7 digits to right side)
                                       
                                    } // catch 1   (8 digits to right side)


                                } // split qrx

                                else if (DX_Message[DX_Index1].Contains("qrz")) // check for split
                                {

                                    int ind = DX_Message[DX_Index1].IndexOf("qrz") + 3;

                                    try // try 1
                                    {
                                        int split_hz = (int)(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 8)) * 1000);

                                        if (split_hz < 10000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QRX .412  then treat it with the same mhz as DX_Freq
                                        else if (split_hz < 100000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QRX 18.412  then it must be in mhz

                                        Trace.WriteLine("Found qrz split hz" + split_hz);

                                        DX_Mode2[DX_Index1] = split_hz;
                                        DX_Mode2[DX_Index1] = DX_Mode2[DX_Index1] - DX_Freq[DX_Index1];


                                    }
                                    catch // catch 1
                                    {
                                        try // try 2
                                        {
                                            int split_hz = (int)(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 7)) * 1000);

                                            if (split_hz < 10000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QRX .412  then treat it with the same mhz as DX_Freq
                                            else if (split_hz < 100000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QRX 18.412  then it must be in mhz

                                            Trace.WriteLine("Found qrz split hz" + split_hz);

                                            DX_Mode2[DX_Index1] = split_hz;
                                            DX_Mode2[DX_Index1] = DX_Mode2[DX_Index1] - DX_Freq[DX_Index1];

                                        }
                                        catch // catch 2
                                        {
                                            try // try 3
                                            {
                                                int split_hz = (int)(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 6)) * 1000);

                                                if (split_hz < 10000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QRX .412  then treat it with the same mhz as DX_Freq
                                                else if (split_hz < 100000) split_hz = (DX_Freq[DX_Index1] / 1000000) + split_hz; // if its QRX 18.412  then it must be in mhz

                                                Trace.WriteLine("Found dn split hz" + split_hz);
                                                DX_Mode2[DX_Index1] = split_hz;

                                                DX_Mode2[DX_Index1] = DX_Mode2[DX_Index1] - DX_Freq[DX_Index1];


                                            }
                                            catch // catch 3
                                            {

                                                Trace.WriteLine("failed to find up value================");
                                                DX_Mode2[DX_Index1] = 0; // 

                                            } // catch 3   (6 digits to right side)
                                          
                                        } // catch 2   (7 digits to right side)
                                      
                                    } // catch 1   (8 digits to right side)


                                } // split qrz




                            } // try to parse dx spot message above
                            catch (FormatException e)
                            {
                                Trace.WriteLine("mode issue" + e);

                            }
                            catch (ArgumentOutOfRangeException e)
                            {
                                Trace.WriteLine("mode1 issue" + e);

                            }
                            //  Trace.WriteLine("DX_Message " + DX_Message[DX_Index1]);

                            // grab time
                            try
                            {
                                DX_Time[DX_Index1] = Convert.ToInt16(message1.Substring(70, 4)); // get dx freq 7016.0  in khz 
                            }
                            catch (FormatException e)
                            {
                                DX_Time[DX_Index1] = 0;
                          
                            }
                            catch (ArgumentOutOfRangeException e)
                            {
                                DX_Time[DX_Index1] = 0;
                            }
                            //   Trace.WriteLine("DX_Time " + DX_Time[DX_Index1])
                           
                           
                            // grab GRID #
                            DX_Grid[DX_Index1] = message1.Substring(76,4); // get grid
                           
                            sb = new StringBuilder(DX_Grid[DX_Index1]); // clear sb string over again
                            sb.Append(')');
                            sb.Insert(0, '('); // to differentiate the spotter from the spotted

                            DX_Grid[DX_Index1] = sb.ToString();


                            // set age of spot to 0;
                            DX_Age[DX_Index1] = "00"; // reset to start



                            //  Trace.WriteLine("DX_Grid " + DX_Grid[DX_Index1]);

                            //  Trace.WriteLine("============================================");


                            //=================================================================================================
                            //=================================================================================================



                            //=================================================================
                            //=================================================================
                            //=================================================================
                            // ke9ns DX SPOT FILTERS
                            if (checkBoxWorld.Checked) // filter out US calls signs
                            {

                                string us1 = DX_Spotter[DX_Index1].Substring(0, 1);
                                string us2 = DX_Spotter[DX_Index1].Substring(1, 1);


                                Regex r = new Regex("[KNWAX]"); // first char
                                Regex r1 = new Regex("[A-Z0-9]"); // 2nd char
                                Regex r2 = new Regex("[EYO]"); // 2nd char // for V as the first char


                                if (us1 == "V")
                                {
                                    if ((r2.IsMatch(us2)))
                                    {
                                        Trace.WriteLine("bypass4a " + DX_Spotter[DX_Index1]);
                                        continue; // dont show spot if not on the r1 list
                                    }
                                    goto PASS2; // if the 1st letter is not a US letter then GOOD use SPOT

                                }
                                else
                                {
                                    if ((r.IsMatch(us1)))
                                    {
                                        Trace.WriteLine("bypass3 " + DX_Spotter[DX_Index1]);
                                        continue;// dont show spot if not on the r list
                                    }
                                    goto PASS2; // if the 1st letter is not a US letter then GOOD use SPOT

                                }

                                if ((r1.IsMatch(us2)))
                                {
                                     Trace.WriteLine("bypass4 " + DX_Spotter[DX_Index1]);
                                    continue; // dont show spot if not on the r1 list
                                }


                            }
                            else if (checkBoxUSspot.Checked) // filer out call signs outside of US
                            {


                                string us1 = DX_Spotter[DX_Index1].Substring(1, 1);// was 0,1 now 1,1 because I added <>
                                string us2 = DX_Spotter[DX_Index1].Substring(2, 1);// was 1,1


                                Regex r = new Regex("[KNWAVX]"); // first char
                                Regex r1 = new Regex("[A-Z0-9]"); // 2nd char
                                Regex r2 = new Regex("[EYO]"); // 2nd char // for V as the first char

                                if (!(r.IsMatch(us1)))
                                {
                                     Trace.WriteLine("bypass1 " + DX_Spotter[DX_Index1]);
                                    continue;// dont show spot if not on the r list
                                }

                                if (us1 == "V")
                                {
                                    if (!(r2.IsMatch(us2)))
                                    {
                                          Trace.WriteLine("bypass2a " + DX_Spotter[DX_Index1]);
                                        continue; // dont show spot if not on the r1 list
                                    }
                                }
                                else
                                {
                                    if (!(r1.IsMatch(us2)))
                                    {
                                        Trace.WriteLine("bypass2 " + DX_Spotter[DX_Index1]);
                                        continue; // dont show spot if not on the r1 list
                                    }
                                }


                            } // checkBoxUSspot.Checked)

                            SP4_Active = 1; // processing message

                            //=================================================================
                            //=================================================================
                            //=================================================================
                            // ke9ns check for STATION DUPLICATES , there can only be 1 possible duplicate per spot added (but IGNORE if on 2nd band)
                            PASS2: int xx = 0;

                           
                            for (int ii = 0; ii <= DX_Index; ii++)
                            {
                                if ((xx == 0) && (DX_Station[DX_Index1] == DX_Station[ii]))
                                {
                                    if ( (Math.Abs(DX_Freq[DX_Index1] - DX_Freq[ii])) < 1000000 )
                                    {
                                        xx = 1;
                                       Trace.WriteLine("station dup============" + DX_Freq[ii] + " dup "+ DX_Freq[DX_Index1] + " dup " + DX_Station[DX_Index1] + " dup " + DX_Station[ii]);
                                    } // freq too close so its a dup
                                }

                                if (xx == 1)
                                {
                                    DX_FULLSTRING[ii] = DX_FULLSTRING[ii + 1]; // 

                                    DX_Station[ii] = DX_Station[ii + 1];
                                    DX_Freq[ii] = DX_Freq[ii + 1];
                                    DX_Spotter[ii] = DX_Spotter[ii + 1];
                                    DX_Message[ii] = DX_Message[ii + 1];
                                    DX_Grid[ii] = DX_Grid[ii + 1];
                                    DX_Time[ii] = DX_Time[ii + 1];
                                    DX_Age[ii] = DX_Age[ii + 1];
                                    DX_Mode[ii] = DX_Mode[ii + 1];
                                    DX_Mode2[ii] = DX_Mode2[ii + 1];
                                }

                            } // for ii check for dup in list
                            DX_Index = (DX_Index - xx);  // readjust the length of the spot list after taking out the duplicates

                            //=================================================================================================
                            //=================================================================================================
                            // ke9ns check for FREQ DUPLICATES , there can only be 1 possible duplicate per spot added (but IGNORE if on 2nd band)
                            xx = 0;
                            for (int ii = 0; ii <= DX_Index; ii++)
                            {
                                if ((xx == 0) && (DX_Freq[DX_Index1] == DX_Freq[ii])) // if you already have this station in the spot list on the screen remove the old spot
                                {
                                    xx = 1;
                                     Trace.WriteLine("freq dup============");
                                }

                                if (xx == 1)
                                {
                                    DX_FULLSTRING[ii] = DX_FULLSTRING[ii + 1]; // 

                                    DX_Station[ii] = DX_Station[ii + 1];
                                    DX_Freq[ii] = DX_Freq[ii + 1];
                                    DX_Spotter[ii] = DX_Spotter[ii + 1];
                                    DX_Message[ii] = DX_Message[ii + 1];
                                    DX_Grid[ii] = DX_Grid[ii + 1];
                                    DX_Time[ii] = DX_Time[ii + 1];
                                    DX_Age[ii] = DX_Age[ii + 1];
                                    DX_Mode[ii] = DX_Mode[ii + 1];
                                    DX_Mode2[ii] = DX_Mode2[ii + 1];
                                }

                            } // for ii check for dup in list
                            DX_Index = (DX_Index - xx);  // readjust the length of the spot list after taking out the duplicates


                            //=================================================================================================
                            //=================================================================================================
                            // ke9ns  passed the spotter, dx station , freq, and time test
                           
                            DX_Index++; // jump to PASS2 if it passed the valid call spotter test

                            if (DX_Index > 80)
                            {
                                //  Trace.WriteLine("DX SPOT REACH 80 ");
                                DX_Index = 80; // you have reached max spots
                            }

                            //   Trace.WriteLine("index "+ DX_Index);



                            //=================================================================================================
                            //=================================================================================================
                            // ke9ns FILO buffer after taking out duplicate from above
                            for (int ii = DX_Index; ii > 0; ii--)
                            {
                                DX_FULLSTRING[ii] = DX_FULLSTRING[ii - 1]; // move array stack down one (oldest dropped off)

                                DX_Station[ii] = DX_Station[ii - 1];
                                DX_Freq[ii] = DX_Freq[ii - 1];
                                DX_Spotter[ii] = DX_Spotter[ii - 1];
                                DX_Message[ii] = DX_Message[ii -1];
                                DX_Grid[ii] = DX_Grid[ii - 1];
                                DX_Time[ii] = DX_Time[ii - 1];
                                DX_Age[ii] = DX_Age[ii - 1];
                                DX_Mode[ii] = DX_Mode[ii - 1];
                                DX_Mode2[ii] = DX_Mode2[ii - 1];

                            } // for ii

                            DX_FULLSTRING[0] = message1; // add newest message to top

                          
                            DX_Station[0] = DX_Station[DX_Index1];    //insert new spot on top of list now
                            DX_Freq[0] = DX_Freq[DX_Index1];
                            DX_Spotter[0] = DX_Spotter[DX_Index1];
                            DX_Message[0] = DX_Message[DX_Index1];
                            DX_Grid[0] = DX_Grid[DX_Index1];
                            DX_Time[0] = DX_Time[DX_Index1];
                            DX_Age[0] = DX_Age[DX_Index1];
                            DX_Mode[0] = DX_Mode[DX_Index1];
                            DX_Mode2[0] = DX_Mode2[DX_Index1];

                            Trace.WriteLine("INSTALL NEW [0]=========== " + DX_Index);

                            processTCPMessage(); // send to spot window


                            SP4_Active = 0; // done processing message

                          
                            //      Trace.WriteLine("Aindex " + DX_Index);

                        } // (message1.StartsWith("DX de ") valid message
                        else if (message1.Contains(" disconnected"))
                        {
                            textBox1.Text += "Your Socket was disconnected \r\n";

                            statusBox.ForeColor = Color.Red;
                            console.spotterMenu.ForeColor = Color.Red;

                            console.spotterMenu.Text = "Closed";
                            statusBox.Text = "Closed";


                            SP_writer.Close();                  // close down now
                            SP_reader.Close();
                            networkStream.Close();

                            client.Close();
                            //   Trace.WriteLine("END DX SPOT thread");

                            statusBox.ForeColor = Color.Black;
                            console.spotterMenu.ForeColor = Color.White;

                            console.spotterMenu.Text = "Spotter";
                            statusBox.Text = "Off";
                            textBox1.Text += "All closed \r\n";
                            SP_Active = 0;
                            SP2_Active = 0;

                            return;
                        } // if disconnected

     

                    } // SP_active == 3 (getting spots here)


                } // for loop forever for this spotter thread

                // if you reach here, its because your closing down the socket

                //    Trace.WriteLine("END DX SPOT thread");


                statusBox.ForeColor = Color.Red;
                console.spotterMenu.ForeColor = Color.Red;

                console.spotterMenu.Text = "Closing";
                statusBox.Text = "Closing";

                textBox1.Text += "Asked to Close \r\n";


                SP_writer.Close();                  // close down now
                SP_reader.Close();
                networkStream.Close();
              

                client.Close();
                //   Trace.WriteLine("END DX SPOT thread");

                statusBox.ForeColor = Color.Black;
                console.spotterMenu.ForeColor = Color.White;

                console.spotterMenu.Text = "Spotter";
                statusBox.Text = "Off";
                textBox1.Text += "All closed \r\n";
                SP2_Active = 0;
                SP_Active = 0;
                return;


            } // try
            catch (SocketException SE)
            {
                textBox1.Text += "Socket Forced closed \r\n";

                Trace.WriteLine("cannot open socket" + SE);
                statusBox.ForeColor = Color.Red;
                console.spotterMenu.ForeColor = Color.Red;

                statusBox.Text = "Socket";
                console.spotterMenu.Text = "Socket";

                SP_writer.Close();
                SP_reader.Close();
                networkStream.Close();
                client.Close();
                SP_Active = 0;
                SP2_Active = 0;

                //   textBox1.Text += "Socket crash Done \r\n";
                statusBox.Text = "Closed";
                console.spotterMenu.Text = "Spotter";

            //    textBox1.Text = SE.ToString();

                return;

            }
            catch (Exception e1)
            {
                textBox1.Text += "Socket Forced closed \r\n";

                Trace.WriteLine("socket exception issue" + e1);

                 statusBox.ForeColor = Color.Red;
                console.spotterMenu.ForeColor = Color.Red;

                statusBox.Text = "Socket";
                console.spotterMenu.Text = "Socket";

                SP_writer.Close();
                SP_reader.Close();
                networkStream.Close();
                client.Close();

                SP2_Active = 0;

                statusBox.Text = "Closed";
                console.spotterMenu.Text = "Spotter";

                //   textBox1.Text += "Socket crash Done \r\n";
            //    textBox1.Text = e1.ToString();

                return;
            }


        } // SPOTTER Thread


        //===================================================================================
        // ke9ns add process message for spot.cs window by right clicking
        private void processTCPMessage()
        {

            string bigmessage = null;

            for (int ii = 0; ii < DX_Index; ii++)
            {

                if (DX_Age[ii] == null) DX_Age[ii] = "00";
                else if (DX_Age[ii] == "  ") DX_Age[ii] = "00";
                if (DX_Age[ii].Length == 1) DX_Age[ii] = "0" + DX_Age[ii];

                string DXmode = "    "; // 5 spaces

                if (DX_Mode[ii] == 0)       DXmode = " ssb ";
                else if (DX_Mode[ii] == 1)  DXmode = " cw  ";
                else if (DX_Mode[ii] == 2)  DXmode = " rtty";
                else if (DX_Mode[ii] == 3)  DXmode = " psk ";
                else if (DX_Mode[ii] == 4)  DXmode = " oliv";
                else if (DX_Mode[ii] == 5)  DXmode = " jt65";
                else if (DX_Mode[ii] == 6)  DXmode = " cont";
                else if (DX_Mode[ii] == 7)  DXmode = " fsk ";
                else if (DX_Mode[ii] == 8)  DXmode = " mt63";
                else if (DX_Mode[ii] == 9)  DXmode = " domi";
                else if (DX_Mode[ii] == 10) DXmode = " pack";
                else if (DX_Mode[ii] == 11) DXmode = " fm  ";
                else if (DX_Mode[ii] == 12) DXmode = " drm ";
                else if (DX_Mode[ii] == 13) DXmode = " sstv";
                else if (DX_Mode[ii] == 14) DXmode = " am  ";

                else DXmode = "     ";

                bigmessage += (DX_FULLSTRING[ii]+ DXmode+ " :" + DX_Age[ii] + "\r\n" );
            
            } // for loop to update dx spot window

            if (pause == false)  textBox1.Text = bigmessage; // update screen


        } //processTCPMessage

      

        private void nameBox_MouseEnter(object sender, EventArgs e)
        {
           // ToolTip tt = new ToolTip();
          //  tt.Show("Name Name of DX Spider node with a > symbol at the end: Example: HB9DRV-9> or NN1D> ", nameBox, 10, 60, 2000);

        }

        private void callBox_MouseEnter(object sender, EventArgs e)
        {
           // ToolTip tt = new ToolTip();
          //  tt.Show("Your Call Sign to login to DX Spider node. Note: you must have used this call with this node prior to this first time ", callBox, 10, 60, 2000);
        }

        private void nodeBox_MouseEnter(object sender, EventArgs e)
        {
           // ToolTip tt = new ToolTip();
          //  tt.Show("Dx Spider node address ", nodeBox, 10, 60, 2000);
        }

        private void portBox_MouseEnter(object sender, EventArgs e)
        {
          //  ToolTip tt = new ToolTip();
          //  tt.Show("Port # that goes with the node address", portBox, 10, 60, 2000);
        }



        private void callBox_TextChanged(object sender, EventArgs e)
        {
            DX_Index = 0; // start over if change node

        }

        private void nameBox_TextChanged(object sender, EventArgs e)
        {

            DX_Index = 0; // start over if change node


        }

        private void portBox_TextChanged(object sender, EventArgs e)
        {
            DX_Index = 0; // start over if change node

        }

        private void nodeBox_TextChanged(object sender, EventArgs e)
        {

            DX_Index = 0; // start over if change node


        }


        private void SpotControl_Layout(object sender, LayoutEventArgs e)
        {




        }

        private void nameBox_Leave(object sender, EventArgs e)
        {
             callB = callBox.Text;
             nodeB = nodeBox.Text;
             portB = portBox.Text;
             nameB = nameBox.Text;
        }

        private void callBox_Leave(object sender, EventArgs e)
        {

            callB = callBox.Text;
            nodeB = nodeBox.Text;
            portB = portBox.Text;
            nameB = nameBox.Text;
        }

        private void nodeBox_Leave(object sender, EventArgs e)
        {

            callB = callBox.Text;
            nodeB = nodeBox.Text;
            portB = portBox.Text;
            nameB = nameBox.Text;
        }

        private void portBox_Leave(object sender, EventArgs e)
        {

            callB = callBox.Text;
            nodeB = nodeBox.Text;
            portB = portBox.Text;
            nameB = nameBox.Text;
        }


        private void checkBoxUSspot_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBoxUSspot.Checked == true)
            {

                //   Trace.WriteLine("US SPOT CHECKED");
                checkBoxWorld.Checked = false;

            }
            else
            {
                //   Trace.WriteLine("US SPOT UN-CHECKED");
            }

        } //checkBoxUSspot_CheckedChanged(


        private void checkBoxWorld_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxWorld.Checked == true)
            {
                checkBoxUSspot.Checked = false;
                //   Trace.WriteLine("world SPOT CHECKED");
            }
            else
            {
                //   Trace.WriteLine("world SPOT UN-CHECKED");
            }
        } // checkBoxWorld_CheckedChanged

        private void statusBox_Click(object sender, EventArgs e)
        {

            if (SP_Active == 3 ) // if DX cluster active then test it by sending a CR
            {

                statusBox.ForeColor = Color.Red;
              
                statusBox.Text = "Test Sent <CR>";

                SP_writer.Write((char)13);
                SP_writer.Write((char)10);
     
            } // if connection was supposed to be active

        } // statusBox_Click


       
        private void textBox1_MouseUp(object sender, MouseEventArgs e)
        {
            textBox1.ShortcutsEnabled = false;

            if (e.Button == MouseButtons.Left)
            {

                int ii = textBox1.SelectionStart;

                byte iii = (byte)(ii / 91); // get line  /82  or /85 if AGE turned on

                if (DX_Index > iii)
                {
                    int freq1 = DX_Freq[iii];

                    if ((freq1 < 5000000) || ((freq1 > 6000000) && (freq1 < 8000000))) // check for bands using LSB
                    {
                        if (chkDXMode.Checked == true)
                        {
                            if (DX_Mode[iii] == 0)       console.RX1DSPMode = DSPMode.LSB;
                            else if (DX_Mode[iii] == 1)  console.RX1DSPMode = DSPMode.CWL;
                            else if (DX_Mode[iii] == 2)  console.RX1DSPMode = DSPMode.DIGL;
                            else if (DX_Mode[iii] == 3)  console.RX1DSPMode = DSPMode.DIGL;
                            else if (DX_Mode[iii] == 4)  console.RX1DSPMode = DSPMode.DIGL;
                            else if (DX_Mode[iii] == 5)  console.RX1DSPMode = DSPMode.DIGL;
                            else if (DX_Mode[iii] == 6)  console.RX1DSPMode = DSPMode.DIGL;
                            else if (DX_Mode[iii] == 7)  console.RX1DSPMode = DSPMode.DIGL;
                            else if (DX_Mode[iii] == 8)  console.RX1DSPMode = DSPMode.DIGL;
                            else if (DX_Mode[iii] == 9)  console.RX1DSPMode = DSPMode.DIGL;
                            else if (DX_Mode[iii] == 10) console.RX1DSPMode = DSPMode.DIGL;
                            else if (DX_Mode[iii] == 11) console.RX1DSPMode = DSPMode.FM;
                            else if (DX_Mode[iii] == 12) console.RX1DSPMode = DSPMode.LSB;
                            else if (DX_Mode[iii] == 13) console.RX1DSPMode = DSPMode.DIGL;
                            else if (DX_Mode[iii] == 14) console.RX1DSPMode = DSPMode.SAM;
                            else console.RX1DSPMode = DSPMode.LSB;


                        }
                        else
                        {
                            console.RX1DSPMode = DSPMode.LSB;
                        }
                    } // LSB
                    else
                    {
                        if (chkDXMode.Checked == true)
                        {

                            if (DX_Mode[iii] == 0)       console.RX1DSPMode = DSPMode.USB;
                            else if (DX_Mode[iii] == 1)  console.RX1DSPMode = DSPMode.CWU;
                            else if (DX_Mode[iii] == 2)  console.RX1DSPMode = DSPMode.DIGU;
                            else if (DX_Mode[iii] == 3)  console.RX1DSPMode = DSPMode.DIGU;
                            else if (DX_Mode[iii] == 4)  console.RX1DSPMode = DSPMode.DIGU;
                            else if (DX_Mode[iii] == 5)  console.RX1DSPMode = DSPMode.DIGU;
                            else if (DX_Mode[iii] == 6)  console.RX1DSPMode = DSPMode.DIGU;
                            else if (DX_Mode[iii] == 7)  console.RX1DSPMode = DSPMode.DIGU;
                            else if (DX_Mode[iii] == 8)  console.RX1DSPMode = DSPMode.DIGU;
                            else if (DX_Mode[iii] == 9)  console.RX1DSPMode = DSPMode.DIGU;
                            else if (DX_Mode[iii] == 10) console.RX1DSPMode = DSPMode.DIGU;
                            else if (DX_Mode[iii] == 11) console.RX1DSPMode = DSPMode.FM;
                            else if (DX_Mode[iii] == 12) console.RX1DSPMode = DSPMode.USB;
                            else if (DX_Mode[iii] == 13) console.RX1DSPMode = DSPMode.DIGU;
                            else if (DX_Mode[iii] == 14) console.RX1DSPMode = DSPMode.SAM;
                            else console.RX1DSPMode = DSPMode.USB;

                        }
                        else
                        {
                            console.RX1DSPMode = DSPMode.USB;
                        }
                    }


                    console.VFOAFreq = (double)freq1 / 1000000; // convert to MHZ

                    if (chkDXMode.Checked == true)
                    {

                        if (DX_Mode2[iii] != 0)
                        {
                           
                            console.VFOBFreq = (double)(freq1 + DX_Mode2[iii]) / 1000000; // convert to MHZ
                            console.chkVFOSplit.Checked = true; // turn on  split

                            Trace.WriteLine("split here" + (freq1 + DX_Mode2[iii]));



                        }
                        else
                        {
                            console.chkVFOSplit.Checked = false; // turn off split

                        }


                    }



                    button1.Focus();


                } // make sure index you clicked on is within range

            } // left mouse button
            else if (e.Button == MouseButtons.Right)
            {

                if (SP4_Active == 0) // only process lookup if not processing a new spot which might cause issue
                {
                    int ii = textBox1.GetCharIndexFromPosition(e.Location);

                    byte iii = (byte)(ii / 91);  // get line  /82  or /86 if AGE turned on or 91 if mode is also on
                    
                    if (DX_Index > iii)
                    {
                        string DXName = DX_Station[iii];

                      //  Trace.WriteLine("Line " + iii + " Name " + DXName);

                        try
                        {
                            System.Diagnostics.Process.Start("https://www.qrz.com/db/" + DXName);   // System.Diagnostics.Process.Start("http://www.microsoft.com");
                        }
                        catch
                        {
                            Trace.WriteLine("bad station");
                            // if not a URL then ignore
                        }
                    }
                   
                } // not actively processing a new spot

            }

        } // textBox1_MouseUp



        private void chkAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {

            this.TopMost = chkAlwaysOnTop.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pause == true)
            {
                pause = false;
                button1.Text = "Pause";
            }
            else
            {
                pause = true;
                button1.Text = "Paused";
            }

        }

        private void chkSUN_CheckedChanged(object sender, EventArgs e)
        {

            if ((chkSUN.Checked == false) && (chkGrayLine.Checked == false))
            {
                console.picDisplay.Image = null; // put back original image
                console.picDisplay.Invalidate();
            }
            if ((SP_Active !=0) )
            {

             
                if ((chkSUN.Checked == true) || (chkGrayLine.Checked == true))
                {
             
                    console.picDisplay.SizeMode = PictureBoxSizeMode.StretchImage;
                    console.picDisplay.Image = Image.FromStream(Map_image);

                }
            }

        }

        private void chkGrayLine_CheckedChanged(object sender, EventArgs e)
        {
            if ((chkSUN.Checked == false) && (chkGrayLine.Checked == false))
            {
                console.picDisplay.Image = null; // put back original image
                console.picDisplay.Invalidate();
            }
          
            if ( (SP_Active != 0) )
            {
              

                if ((chkSUN.Checked == true) || (chkGrayLine.Checked == true))
                {
                 
                    console.picDisplay.SizeMode = PictureBoxSizeMode.StretchImage;
                    console.picDisplay.Image = Image.FromStream(Map_image);

                }
            }
       }



    } // Spotcontrol


} // powersdr
