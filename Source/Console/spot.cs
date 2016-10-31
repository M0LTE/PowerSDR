﻿//=================================================================
// Spot.cs
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
// Fractional year:
//g = (360/365.25)*(N + hour/24)    //  N=day number 1-365


// declination:
// D = 0.396372 - 22.91327 * cos(g) + 4.02543 * sin(g) - 0.387205 * cos( 2 * g)+
//   + 0.051967 * sin(2 * g) - 0.154527 * cos( 3 * g) + 0.084798 * sin( 3 * g)


//=================================================================

using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;                    // ke9ns add for stringbuilder
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
using System.Net;
//using System.Runtime.Serialization.Json;



using System.Net.Sockets;                // ke9ns add for tcpip internet connections
using System.Threading.Tasks;

namespace PowerSDR
{
   
    //==========================================================
    // ke9ns used by NIST time sync routine to allow update of PC timeclock
    public struct SystemTime
    {
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMilliseconds;

      
        public void FromDateTime(DateTime time)
        {
            wYear = (ushort)time.Year;
            wMonth = (ushort)time.Month;
            wDayOfWeek = (ushort)time.DayOfWeek;
            wDay = (ushort)time.Day;
            wHour = (ushort)time.Hour;
            wMinute = (ushort)time.Minute;
            wSecond = (ushort)time.Second;
            wMilliseconds = (ushort)time.Millisecond;
        }
       
        public DateTime ToDateTime()
        {
            return new DateTime(wYear, wMonth, wDay, wHour, wMinute, wSecond, wMilliseconds);
        }
        
        public static DateTime ToDateTime(SystemTime time)
        {
            return time.ToDateTime();
        }

    } // struct SystemTime


    //=======================================================================================
    public class SpotControl : System.Windows.Forms.Form
    {
        private static System.Reflection.Assembly myAssembly2 = System.Reflection.Assembly.GetExecutingAssembly();
        public static Stream Map_image = myAssembly2.GetManifestResourceStream("PowerSDR.Resources.picD1.png");     // MAP

        public static Stream Map_image2 = myAssembly2.GetManifestResourceStream("PowerSDR.Resources.picD2.png");     // MAP with lat / long on it

        private static System.Reflection.Assembly myAssembly1 = System.Reflection.Assembly.GetExecutingAssembly();
        public static Stream sun_image = myAssembly1.GetManifestResourceStream("PowerSDR.Resources.sun.png");       // SUN


        public static Console console;   // ke9ns mod  to allow console to pass back values to setup screen

        public Setup setupForm;                             // ke9ns communications with setupform  (i.e. allow combometertype.text update from inside console.cs) 
        public StackControl StackForm;                      // ke9ns add  communications with spot.cs and stack
        public SwlControl SwlForm;                          // ke9ns add  communications with spot.cs and swl

        public DXMemList dxmemlist;                         //  ke9ns add comm with dx cluster list

      
        //   public static Display display;


        //   private ArrayList file_list;
        private string wave_folder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\PowerSDR";

        private Button SWLbutton;
        private Button SSBbutton;
        public TextBox textBox1;
        public TextBox nodeBox1;
        private TextBox textBox3;
        public TextBox callBox;
        public TextBox portBox2;
        private TextBox statusBox;
        private Button button1;
        private TextBox statusBoxSWL;
        private Label label1;
        private Label label2;
        private CheckBoxTS chkAlwaysOnTop;
        public CheckBoxTS chkDXMode;
        public CheckBoxTS chkSUN;
        private ToolTip toolTip1;
        public CheckBoxTS chkGrayLine;
        private Button btnTrack;
        public CheckBoxTS chkPanMode;
        public TextBox nameBox;
        public CheckBoxTS chkMapCall;
        public CheckBoxTS chkMapCountry;
        public CheckBoxTS chkMapBand;
        public CheckBoxTS chkBoxCW;
        public CheckBoxTS chkBoxSSB;
        public CheckBoxTS chkBoxDIG;
        public CheckBoxTS chkBoxPan;
        private DataGridView dataGridView1;
        public CheckBoxTS chkBoxMem;
        public DataGridView dataGridView2;
        private Button SWLbutton2;
        public CheckBoxTS chkBoxNA;
        public CheckBoxTS chkBoxWrld;
        private NumericUpDownTS udDisplayLat;
        private NumericUpDownTS udDisplayLong;
        private Label label3;
        private Label label4;
        public CheckBoxTS chkBoxBeam;
        private Label label5;
        private Button btnBeacon;
        public CheckBoxTS BoxBScan;
        public CheckBoxTS BoxBFScan;
        private NumericUpDownTS numericUpDownTS1;
        private Button btnTime;
        public CheckBoxTS checkBoxWWV;
        private NumericUpDownTS udDisplayWWV;
        private IContainer components;

        public DSP dsp;

        #region Constructor and Destructor

        public SpotControl(Console c)
        {
            InitializeComponent();
            console = c;

          

        Display.SpotForm = this;  // allows Display to see public data (not public static data)
            StackControl.SpotForm = this; // allows Stack to see public data from spot
                                          //  SwlControl.SpotForm = this; // allows swl to see public data from spot

            Common.RestoreForm(this, "SpotForm", true);


            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FlexRadio Systems\\";
            string file_name = path + "DXMemory.xml";

            // dataGridView1.Dock = DockStyle.Fill;

            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.DataSource = console.DXMemList.List; // ke9ns get list of memories from memorylist.cs is where the file is opened and saved

            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;

            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.AutoGenerateColumns = false;


            if (!File.Exists(file_name))
            {
                console.DXMemList.List.Add(new DXMemRecord("k1rfi.com:7300"));
                console.DXMemList.List.Add(new DXMemRecord("ve7cc.net:23"));
                console.DXMemList.List.Add(new DXMemRecord("telnet.reversebeacon.net:7000"));
                console.DXMemList.List.Add(new DXMemRecord(""));
                console.DXMemList.List.Add(new DXMemRecord(""));
                console.DXMemList.List.Add(new DXMemRecord(""));
                console.DXMemList.List.Add(new DXMemRecord(""));
                console.DXMemList.List.Add(new DXMemRecord(""));
                console.DXMemList.List.Add(new DXMemRecord(""));
                console.DXMemList.List.Add(new DXMemRecord(""));


                console.DXMemList.Save1();

                Debug.WriteLine("create DXURL File");

            }



        } // spotcontrol

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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.SWLbutton = new System.Windows.Forms.Button();
            this.SSBbutton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.nodeBox1 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.callBox = new System.Windows.Forms.TextBox();
            this.portBox2 = new System.Windows.Forms.TextBox();
            this.statusBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.statusBoxSWL = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnTrack = new System.Windows.Forms.Button();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.SWLbutton2 = new System.Windows.Forms.Button();
            this.btnBeacon = new System.Windows.Forms.Button();
            this.btnTime = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.udDisplayWWV = new System.Windows.Forms.NumericUpDownTS();
            this.checkBoxWWV = new System.Windows.Forms.CheckBoxTS();
            this.numericUpDownTS1 = new System.Windows.Forms.NumericUpDownTS();
            this.BoxBFScan = new System.Windows.Forms.CheckBoxTS();
            this.BoxBScan = new System.Windows.Forms.CheckBoxTS();
            this.chkBoxBeam = new System.Windows.Forms.CheckBoxTS();
            this.udDisplayLong = new System.Windows.Forms.NumericUpDownTS();
            this.udDisplayLat = new System.Windows.Forms.NumericUpDownTS();
            this.chkBoxWrld = new System.Windows.Forms.CheckBoxTS();
            this.chkBoxNA = new System.Windows.Forms.CheckBoxTS();
            this.chkBoxMem = new System.Windows.Forms.CheckBoxTS();
            this.chkBoxPan = new System.Windows.Forms.CheckBoxTS();
            this.chkBoxDIG = new System.Windows.Forms.CheckBoxTS();
            this.chkBoxSSB = new System.Windows.Forms.CheckBoxTS();
            this.chkBoxCW = new System.Windows.Forms.CheckBoxTS();
            this.chkMapBand = new System.Windows.Forms.CheckBoxTS();
            this.chkMapCountry = new System.Windows.Forms.CheckBoxTS();
            this.chkMapCall = new System.Windows.Forms.CheckBoxTS();
            this.chkPanMode = new System.Windows.Forms.CheckBoxTS();
            this.chkGrayLine = new System.Windows.Forms.CheckBoxTS();
            this.chkSUN = new System.Windows.Forms.CheckBoxTS();
            this.chkAlwaysOnTop = new System.Windows.Forms.CheckBoxTS();
            this.chkDXMode = new System.Windows.Forms.CheckBoxTS();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDisplayWWV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDisplayLong)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDisplayLat)).BeginInit();
            this.SuspendLayout();
            // 
            // SWLbutton
            // 
            this.SWLbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SWLbutton.Location = new System.Drawing.Point(615, 444);
            this.SWLbutton.Name = "SWLbutton";
            this.SWLbutton.Size = new System.Drawing.Size(75, 23);
            this.SWLbutton.TabIndex = 2;
            this.SWLbutton.Text = "Spot SWL";
            this.toolTip1.SetToolTip(this.SWLbutton, resources.GetString("SWLbutton.ToolTip"));
            this.SWLbutton.UseVisualStyleBackColor = true;
            this.SWLbutton.Click += new System.EventHandler(this.SWLbutton_Click);
            // 
            // SSBbutton
            // 
            this.SSBbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SSBbutton.Location = new System.Drawing.Point(12, 543);
            this.SSBbutton.Name = "SSBbutton";
            this.SSBbutton.Size = new System.Drawing.Size(75, 23);
            this.SSBbutton.TabIndex = 1;
            this.SSBbutton.Text = "Spot DX";
            this.toolTip1.SetToolTip(this.SSBbutton, "Click to Turn On/Off Dx Cluster Spotting (on both this DX Spotting window and Pan" +
        "adapter)\r\nRequires Internet to work.\r\n");
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
            this.textBox1.Location = new System.Drawing.Point(12, 126);
            this.textBox1.MaximumSize = new System.Drawing.Size(1000, 1000);
            this.textBox1.MaxLength = 10000000;
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(759, 261);
            this.textBox1.TabIndex = 6;
            this.textBox1.TabStop = false;
            this.textBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseDown);
            this.textBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseUp);
            // 
            // nodeBox1
            // 
            this.nodeBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nodeBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nodeBox1.Location = new System.Drawing.Point(658, 442);
            this.nodeBox1.MaxLength = 50;
            this.nodeBox1.Name = "nodeBox1";
            this.nodeBox1.Size = new System.Drawing.Size(84, 22);
            this.nodeBox1.TabIndex = 6;
            this.nodeBox1.Text = "spider.ham-radio-deluxe.com";
            this.toolTip1.SetToolTip(this.nodeBox1, "Enter in a DX Cluster URL address here");
            this.nodeBox1.Visible = false;
            this.nodeBox1.TextChanged += new System.EventHandler(this.nodeBox_TextChanged);
            this.nodeBox1.Leave += new System.EventHandler(this.nodeBox_Leave);
            this.nodeBox1.MouseEnter += new System.EventHandler(this.nodeBox_MouseEnter);
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(287, 7);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(484, 113);
            this.textBox3.TabIndex = 8;
            this.textBox3.TabStop = false;
            this.textBox3.Text = resources.GetString("textBox3.Text");
            // 
            // callBox
            // 
            this.callBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.callBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.callBox.Location = new System.Drawing.Point(682, 546);
            this.callBox.MaxLength = 20;
            this.callBox.Name = "callBox";
            this.callBox.Size = new System.Drawing.Size(87, 22);
            this.callBox.TabIndex = 5;
            this.callBox.Text = "Callsign";
            this.toolTip1.SetToolTip(this.callBox, "Enter Your Call sign to login to the DX Cluster here");
            this.callBox.TextChanged += new System.EventHandler(this.callBox_TextChanged);
            this.callBox.Leave += new System.EventHandler(this.callBox_Leave);
            this.callBox.MouseEnter += new System.EventHandler(this.callBox_MouseEnter);
            // 
            // portBox2
            // 
            this.portBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.portBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.portBox2.Location = new System.Drawing.Point(696, 444);
            this.portBox2.MaxLength = 7;
            this.portBox2.Name = "portBox2";
            this.portBox2.Size = new System.Drawing.Size(56, 22);
            this.portBox2.TabIndex = 7;
            this.portBox2.Text = "0";
            this.toolTip1.SetToolTip(this.portBox2, "Enter in Dx Cluster URL Port# here");
            this.portBox2.Visible = false;
            this.portBox2.TextChanged += new System.EventHandler(this.portBox_TextChanged);
            this.portBox2.Leave += new System.EventHandler(this.portBox_Leave);
            this.portBox2.MouseEnter += new System.EventHandler(this.portBox_MouseEnter);
            // 
            // statusBox
            // 
            this.statusBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.statusBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusBox.Location = new System.Drawing.Point(13, 414);
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
            this.button1.Location = new System.Drawing.Point(100, 543);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(68, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Pause";
            this.toolTip1.SetToolTip(this.button1, "Click to Pause the DX Text window (if spots are coming through too fast)\r\nUpdates" +
        " to the Panadapter will still occur");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // statusBoxSWL
            // 
            this.statusBoxSWL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.statusBoxSWL.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusBoxSWL.Location = new System.Drawing.Point(614, 416);
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
            this.label1.Location = new System.Drawing.Point(13, 397);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Status of DX Cluster";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(612, 400);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Status of SWL Spotter";
            // 
            // btnTrack
            // 
            this.btnTrack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTrack.Location = new System.Drawing.Point(273, 490);
            this.btnTrack.Name = "btnTrack";
            this.btnTrack.Size = new System.Drawing.Size(75, 23);
            this.btnTrack.TabIndex = 62;
            this.btnTrack.Text = "Track";
            this.toolTip1.SetToolTip(this.btnTrack, resources.GetString("btnTrack.ToolTip"));
            this.btnTrack.UseVisualStyleBackColor = true;
            this.btnTrack.Click += new System.EventHandler(this.btnTrack_Click);
            // 
            // nameBox
            // 
            this.nameBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nameBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameBox.Location = new System.Drawing.Point(687, 444);
            this.nameBox.MaxLength = 20;
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(46, 22);
            this.nameBox.TabIndex = 64;
            this.nameBox.Text = "name";
            this.toolTip1.SetToolTip(this.nameBox, "Enter Your Call sign to login to the DX Cluster here");
            this.nameBox.Visible = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowDrop = true;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.OliveDrab;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridView1.Location = new System.Drawing.Point(12, 7);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(269, 113);
            this.dataGridView1.TabIndex = 72;
            this.toolTip1.SetToolTip(this.dataGridView1, "Enter DX address : port#\r\nExample:  k1rfi.com:7300\r\n");
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowDrop = true;
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AllowUserToResizeColumns = false;
            this.dataGridView2.AllowUserToResizeRows = false;
            this.dataGridView2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.OliveDrab;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView2.DefaultCellStyle = dataGridViewCellStyle10;
            this.dataGridView2.Location = new System.Drawing.Point(463, 281);
            this.dataGridView2.MultiSelect = false;
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(254, 94);
            this.dataGridView2.TabIndex = 75;
            this.toolTip1.SetToolTip(this.dataGridView2, "memories");
            this.dataGridView2.Visible = false;
            // 
            // SWLbutton2
            // 
            this.SWLbutton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SWLbutton2.Location = new System.Drawing.Point(696, 444);
            this.SWLbutton2.Name = "SWLbutton2";
            this.SWLbutton2.Size = new System.Drawing.Size(75, 23);
            this.SWLbutton2.TabIndex = 76;
            this.SWLbutton2.Text = "SWL list";
            this.toolTip1.SetToolTip(this.SWLbutton2, resources.GetString("SWLbutton2.ToolTip"));
            this.SWLbutton2.UseVisualStyleBackColor = true;
            this.SWLbutton2.Click += new System.EventHandler(this.SWLbutton2_Click);
            // 
            // btnBeacon
            // 
            this.btnBeacon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBeacon.Location = new System.Drawing.Point(223, 543);
            this.btnBeacon.Name = "btnBeacon";
            this.btnBeacon.Size = new System.Drawing.Size(75, 23);
            this.btnBeacon.TabIndex = 85;
            this.btnBeacon.Text = "Beacon Chk";
            this.toolTip1.SetToolTip(this.btnBeacon, resources.GetString("btnBeacon.ToolTip"));
            this.btnBeacon.UseVisualStyleBackColor = true;
            this.btnBeacon.Click += new System.EventHandler(this.btnBeacon_Click);
            // 
            // btnTime
            // 
            this.btnTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTime.Location = new System.Drawing.Point(511, 402);
            this.btnTime.Name = "btnTime";
            this.btnTime.Size = new System.Drawing.Size(75, 23);
            this.btnTime.TabIndex = 89;
            this.btnTime.Text = "Time Sync";
            this.toolTip1.SetToolTip(this.btnTime, resources.GetString("btnTime.ToolTip"));
            this.btnTime.UseVisualStyleBackColor = true;
            this.btnTime.Click += new System.EventHandler(this.btnTime_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(522, 525);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 13);
            this.label3.TabIndex = 81;
            this.label3.Text = "Your Lat and Long (+/- deg)\r\n";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(693, 525);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 82;
            this.label4.Text = "Your Call sign";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(541, 506);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(227, 13);
            this.label5.TabIndex = 84;
            this.label5.Text = "Setup->CAT Control->DDUtil , for Rotor Control";
            // 
            // udDisplayWWV
            // 
            this.udDisplayWWV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.udDisplayWWV.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udDisplayWWV.Location = new System.Drawing.Point(536, 452);
            this.udDisplayWWV.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.udDisplayWWV.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udDisplayWWV.Name = "udDisplayWWV";
            this.udDisplayWWV.Size = new System.Drawing.Size(39, 20);
            this.udDisplayWWV.TabIndex = 91;
            this.toolTip1.SetToolTip(this.udDisplayWWV, "Check you Check the WWV Sync Box\r\nSelect at WWV station with an S9 signal.\r\nUsual" +
        "ly 10mhz and 15mhz are the cleanest signals\r\n1=2.5mhz \r\n2=5.0mhz\r\n3=10.0mhz\r\n4=1" +
        "5.0mhz\r\n\r\n");
            this.udDisplayWWV.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // checkBoxWWV
            // 
            this.checkBoxWWV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxWWV.Image = null;
            this.checkBoxWWV.Location = new System.Drawing.Point(511, 431);
            this.checkBoxWWV.Name = "checkBoxWWV";
            this.checkBoxWWV.Size = new System.Drawing.Size(98, 24);
            this.checkBoxWWV.TabIndex = 90;
            this.checkBoxWWV.Text = "Use WWV HF";
            this.toolTip1.SetToolTip(this.checkBoxWWV, resources.GetString("checkBoxWWV.ToolTip"));
            this.checkBoxWWV.CheckedChanged += new System.EventHandler(this.checkBoxWWV_CheckedChanged);
            // 
            // numericUpDownTS1
            // 
            this.numericUpDownTS1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDownTS1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTS1.Location = new System.Drawing.Point(382, 549);
            this.numericUpDownTS1.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownTS1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTS1.Name = "numericUpDownTS1";
            this.numericUpDownTS1.Size = new System.Drawing.Size(39, 20);
            this.numericUpDownTS1.TabIndex = 88;
            this.toolTip1.SetToolTip(this.numericUpDownTS1, "Which Band to Start Slow Beacaon Scan on:\r\n1=14.1mhz\r\n2=18.11mhz\r\n3=21.15mhz\r\n4=2" +
        "4.93mhz\r\n5=28.2mhz\r\n");
            this.numericUpDownTS1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTS1.ValueChanged += new System.EventHandler(this.numericUpDownTS1_ValueChanged);
            // 
            // BoxBFScan
            // 
            this.BoxBFScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BoxBFScan.Image = null;
            this.BoxBFScan.Location = new System.Drawing.Point(304, 546);
            this.BoxBFScan.Name = "BoxBFScan";
            this.BoxBFScan.Size = new System.Drawing.Size(87, 24);
            this.BoxBFScan.TabIndex = 87;
            this.BoxBFScan.Text = "Slow Scan";
            this.toolTip1.SetToolTip(this.BoxBFScan, resources.GetString("BoxBFScan.ToolTip"));
            this.BoxBFScan.CheckedChanged += new System.EventHandler(this.BoxBFScan_CheckedChanged);
            // 
            // BoxBScan
            // 
            this.BoxBScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BoxBScan.Image = null;
            this.BoxBScan.Location = new System.Drawing.Point(304, 525);
            this.BoxBScan.Name = "BoxBScan";
            this.BoxBScan.Size = new System.Drawing.Size(107, 24);
            this.BoxBScan.TabIndex = 86;
            this.BoxBScan.Text = "Fast Scan";
            this.toolTip1.SetToolTip(this.BoxBScan, "Check to Scan all 18 Beacon Stations 5 Frequecies at each 10 second Interval\r\nPow" +
        "erSDR will move across all 5 Beacon Frequencies in 1 sec intervals \r\n\r\nTotal bea" +
        "con map is compled in 3 minutes.\r\n\r\n");
            this.BoxBScan.CheckedChanged += new System.EventHandler(this.BoxBScan_CheckedChanged);
            // 
            // chkBoxBeam
            // 
            this.chkBoxBeam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxBeam.Image = null;
            this.chkBoxBeam.Location = new System.Drawing.Point(417, 520);
            this.chkBoxBeam.Name = "chkBoxBeam";
            this.chkBoxBeam.Size = new System.Drawing.Size(88, 24);
            this.chkBoxBeam.TabIndex = 83;
            this.chkBoxBeam.Text = "Map Beam";
            this.toolTip1.SetToolTip(this.chkBoxBeam, "Check To Show Beam heading on map in (deg)\r\n");
            this.chkBoxBeam.CheckedChanged += new System.EventHandler(this.chkBoxBeam_CheckedChanged);
            // 
            // udDisplayLong
            // 
            this.udDisplayLong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.udDisplayLong.DecimalPlaces = 4;
            this.udDisplayLong.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udDisplayLong.Location = new System.Drawing.Point(602, 547);
            this.udDisplayLong.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.udDisplayLong.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.udDisplayLong.Name = "udDisplayLong";
            this.udDisplayLong.Size = new System.Drawing.Size(74, 20);
            this.udDisplayLong.TabIndex = 80;
            this.toolTip1.SetToolTip(this.udDisplayLong, "Enter Longitude in deg (-180 to 180) for Beam Heading\r\n- for West of 0 GMT line\r\n" +
        "+ for East of 0 GMT line\r\n\r\nLeft Click on PowerSDR Display and Hit SHIFT key to " +
        "\r\ntoggle Lat/Long map");
            this.udDisplayLong.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udDisplayLong.ValueChanged += new System.EventHandler(this.udDisplayLong_ValueChanged);
            // 
            // udDisplayLat
            // 
            this.udDisplayLat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.udDisplayLat.DecimalPlaces = 4;
            this.udDisplayLat.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udDisplayLat.Location = new System.Drawing.Point(525, 547);
            this.udDisplayLat.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.udDisplayLat.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            -2147483648});
            this.udDisplayLat.Name = "udDisplayLat";
            this.udDisplayLat.Size = new System.Drawing.Size(71, 20);
            this.udDisplayLat.TabIndex = 79;
            this.toolTip1.SetToolTip(this.udDisplayLat, "Enter Latitude in deg (90 to -90) for Beam Heading\r\n+ for Northern Hemisphere\r\n- " +
        "for Southern Hemisphere\r\n\r\nLeft Click on PowerSDR Display and Hit SHIFT key to \r" +
        "\ntoggle Lat/Long map");
            this.udDisplayLat.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.udDisplayLat.ValueChanged += new System.EventHandler(this.udDisplayLat_ValueChanged);
            // 
            // chkBoxWrld
            // 
            this.chkBoxWrld.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxWrld.Image = null;
            this.chkBoxWrld.Location = new System.Drawing.Point(11, 495);
            this.chkBoxWrld.Name = "chkBoxWrld";
            this.chkBoxWrld.Size = new System.Drawing.Size(194, 24);
            this.chkBoxWrld.TabIndex = 78;
            this.chkBoxWrld.Text = "Exclude North American Spotters";
            this.chkBoxWrld.CheckedChanged += new System.EventHandler(this.chkBoxWrld_CheckedChanged);
            // 
            // chkBoxNA
            // 
            this.chkBoxNA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxNA.Image = null;
            this.chkBoxNA.Location = new System.Drawing.Point(11, 461);
            this.chkBoxNA.Name = "chkBoxNA";
            this.chkBoxNA.Size = new System.Drawing.Size(165, 35);
            this.chkBoxNA.TabIndex = 77;
            this.chkBoxNA.Text = "North American Spotters only";
            this.chkBoxNA.CheckedChanged += new System.EventHandler(this.chkBoxNA_CheckedChanged);
            // 
            // chkBoxMem
            // 
            this.chkBoxMem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxMem.Image = null;
            this.chkBoxMem.Location = new System.Drawing.Point(417, 495);
            this.chkBoxMem.Name = "chkBoxMem";
            this.chkBoxMem.Size = new System.Drawing.Size(123, 24);
            this.chkBoxMem.TabIndex = 74;
            this.chkBoxMem.Text = "MEMORIES to Pan";
            this.toolTip1.SetToolTip(this.chkBoxMem, "Show Memories directly on Panadapter.\r\n\r\nLEFT CLICK on visible Memory + CTRL to s" +
        "et Mode\r\n\r\nLEFT CLICK on PAN + ALT + M keys to save New Memory\r\n");
            this.chkBoxMem.CheckedChanged += new System.EventHandler(this.chkBoxMem_CheckedChanged);
            // 
            // chkBoxPan
            // 
            this.chkBoxPan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxPan.Image = null;
            this.chkBoxPan.Location = new System.Drawing.Point(417, 472);
            this.chkBoxPan.Name = "chkBoxPan";
            this.chkBoxPan.Size = new System.Drawing.Size(100, 24);
            this.chkBoxPan.TabIndex = 71;
            this.chkBoxPan.Text = "Map just Pan";
            this.toolTip1.SetToolTip(this.chkBoxPan, "Show Country or Calls on Map for just the Panadapter freq you are viewing.\r\n");
            this.chkBoxPan.CheckedChanged += new System.EventHandler(this.chkBoxPan_CheckedChanged);
            // 
            // chkBoxDIG
            // 
            this.chkBoxDIG.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxDIG.Checked = true;
            this.chkBoxDIG.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxDIG.Image = null;
            this.chkBoxDIG.Location = new System.Drawing.Point(182, 472);
            this.chkBoxDIG.Name = "chkBoxDIG";
            this.chkBoxDIG.Size = new System.Drawing.Size(85, 24);
            this.chkBoxDIG.TabIndex = 70;
            this.chkBoxDIG.Text = "Spot Digital";
            this.toolTip1.SetToolTip(this.chkBoxDIG, "Show Digital spots when checked (like RTTY, PSK, etc)\r\n");
            // 
            // chkBoxSSB
            // 
            this.chkBoxSSB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxSSB.Checked = true;
            this.chkBoxSSB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxSSB.Image = null;
            this.chkBoxSSB.Location = new System.Drawing.Point(182, 442);
            this.chkBoxSSB.Name = "chkBoxSSB";
            this.chkBoxSSB.Size = new System.Drawing.Size(85, 24);
            this.chkBoxSSB.TabIndex = 69;
            this.chkBoxSSB.Text = "Spot Phone";
            this.toolTip1.SetToolTip(this.chkBoxSSB, "Show SSB spots when checked\r\n");
            // 
            // chkBoxCW
            // 
            this.chkBoxCW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxCW.Checked = true;
            this.chkBoxCW.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxCW.Image = null;
            this.chkBoxCW.Location = new System.Drawing.Point(182, 414);
            this.chkBoxCW.Name = "chkBoxCW";
            this.chkBoxCW.Size = new System.Drawing.Size(85, 24);
            this.chkBoxCW.TabIndex = 68;
            this.chkBoxCW.Text = "Spot CW";
            this.toolTip1.SetToolTip(this.chkBoxCW, "Show CW spots when checked\r\n");
            // 
            // chkMapBand
            // 
            this.chkMapBand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkMapBand.Checked = true;
            this.chkMapBand.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMapBand.Image = null;
            this.chkMapBand.Location = new System.Drawing.Point(417, 449);
            this.chkMapBand.Name = "chkMapBand";
            this.chkMapBand.Size = new System.Drawing.Size(113, 24);
            this.chkMapBand.TabIndex = 67;
            this.chkMapBand.Text = "Map just Band";
            this.toolTip1.SetToolTip(this.chkMapBand, "Show Country or Calls on Map for the Band you are on.\r\n");
            this.chkMapBand.CheckedChanged += new System.EventHandler(this.chkMapBand_CheckedChanged);
            // 
            // chkMapCountry
            // 
            this.chkMapCountry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkMapCountry.Image = null;
            this.chkMapCountry.Location = new System.Drawing.Point(417, 403);
            this.chkMapCountry.Name = "chkMapCountry";
            this.chkMapCountry.Size = new System.Drawing.Size(88, 22);
            this.chkMapCountry.TabIndex = 66;
            this.chkMapCountry.Text = "Map Country";
            this.toolTip1.SetToolTip(this.chkMapCountry, "Show Dx spot Countries on Map\r\n");
            this.chkMapCountry.CheckedChanged += new System.EventHandler(this.chkMapCountry_CheckedChanged);
            // 
            // chkMapCall
            // 
            this.chkMapCall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkMapCall.Checked = true;
            this.chkMapCall.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMapCall.Image = null;
            this.chkMapCall.Location = new System.Drawing.Point(417, 426);
            this.chkMapCall.Name = "chkMapCall";
            this.chkMapCall.Size = new System.Drawing.Size(88, 24);
            this.chkMapCall.TabIndex = 65;
            this.chkMapCall.Text = "Map Calls";
            this.toolTip1.SetToolTip(this.chkMapCall, "Show DX Spot Call signs on Map");
            this.chkMapCall.CheckedChanged += new System.EventHandler(this.chkMapCall_CheckedChanged);
            // 
            // chkPanMode
            // 
            this.chkPanMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkPanMode.Checked = true;
            this.chkPanMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPanMode.Image = null;
            this.chkPanMode.Location = new System.Drawing.Point(273, 461);
            this.chkPanMode.Name = "chkPanMode";
            this.chkPanMode.Size = new System.Drawing.Size(148, 23);
            this.chkPanMode.TabIndex = 63;
            this.chkPanMode.Text = "Special PanaFall Mode\r\n";
            this.toolTip1.SetToolTip(this.chkPanMode, "When Checked, will Display RX1 in Panafall mode, with a small waterfall for bette" +
        "r viewing of the map");
            this.chkPanMode.CheckedChanged += new System.EventHandler(this.chkPanMode_CheckedChanged);
            // 
            // chkGrayLine
            // 
            this.chkGrayLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkGrayLine.Checked = true;
            this.chkGrayLine.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGrayLine.Image = null;
            this.chkGrayLine.Location = new System.Drawing.Point(273, 438);
            this.chkGrayLine.Name = "chkGrayLine";
            this.chkGrayLine.Size = new System.Drawing.Size(105, 17);
            this.chkGrayLine.TabIndex = 61;
            this.chkGrayLine.Text = "GrayLine Track";
            this.toolTip1.SetToolTip(this.chkGrayLine, "GrayLine will show on Panadapter Display\r\nBut only when using KE9SN6_World skin o" +
        "nly\r\nAnd only when RX1 is in Panadapter Mode with RX2 Display OFF");
            this.chkGrayLine.CheckedChanged += new System.EventHandler(this.chkGrayLine_CheckedChanged);
            // 
            // chkSUN
            // 
            this.chkSUN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkSUN.Checked = true;
            this.chkSUN.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSUN.Image = null;
            this.chkSUN.Location = new System.Drawing.Point(273, 410);
            this.chkSUN.Name = "chkSUN";
            this.chkSUN.Size = new System.Drawing.Size(92, 24);
            this.chkSUN.TabIndex = 60;
            this.chkSUN.Text = "SunTracking\r\n";
            this.toolTip1.SetToolTip(this.chkSUN, "Sun will show on Panadapter screen \r\nBut only when using KE9SN6_World 3 only\r\nAnd" +
        " only when RX1 is in Panadapter Mode with RX2 Display OFF");
            this.chkSUN.CheckedChanged += new System.EventHandler(this.chkSUN_CheckedChanged);
            // 
            // chkAlwaysOnTop
            // 
            this.chkAlwaysOnTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAlwaysOnTop.Image = null;
            this.chkAlwaysOnTop.Location = new System.Drawing.Point(668, 473);
            this.chkAlwaysOnTop.Name = "chkAlwaysOnTop";
            this.chkAlwaysOnTop.Size = new System.Drawing.Size(103, 24);
            this.chkAlwaysOnTop.TabIndex = 58;
            this.chkAlwaysOnTop.Text = "Always On Top";
            this.chkAlwaysOnTop.CheckedChanged += new System.EventHandler(this.chkAlwaysOnTop_CheckedChanged);
            // 
            // chkDXMode
            // 
            this.chkDXMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkDXMode.Checked = true;
            this.chkDXMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDXMode.Image = null;
            this.chkDXMode.Location = new System.Drawing.Point(682, 546);
            this.chkDXMode.Name = "chkDXMode";
            this.chkDXMode.Size = new System.Drawing.Size(91, 24);
            this.chkDXMode.TabIndex = 59;
            this.chkDXMode.Text = "Parse \"DX Spot\" Mode";
            this.chkDXMode.UseVisualStyleBackColor = true;
            this.chkDXMode.Visible = false;
            // 
            // SpotControl
            // 
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(783, 578);
            this.Controls.Add(this.udDisplayWWV);
            this.Controls.Add(this.checkBoxWWV);
            this.Controls.Add(this.btnTime);
            this.Controls.Add(this.numericUpDownTS1);
            this.Controls.Add(this.BoxBFScan);
            this.Controls.Add(this.BoxBScan);
            this.Controls.Add(this.btnBeacon);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chkBoxBeam);
            this.Controls.Add(this.SWLbutton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.udDisplayLong);
            this.Controls.Add(this.udDisplayLat);
            this.Controls.Add(this.chkBoxWrld);
            this.Controls.Add(this.chkBoxNA);
            this.Controls.Add(this.SWLbutton2);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.chkBoxMem);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.chkBoxPan);
            this.Controls.Add(this.chkBoxDIG);
            this.Controls.Add(this.chkBoxSSB);
            this.Controls.Add(this.chkBoxCW);
            this.Controls.Add(this.chkMapBand);
            this.Controls.Add(this.chkMapCountry);
            this.Controls.Add(this.chkMapCall);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.chkPanMode);
            this.Controls.Add(this.btnTrack);
            this.Controls.Add(this.chkGrayLine);
            this.Controls.Add(this.chkSUN);
            this.Controls.Add(this.chkAlwaysOnTop);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusBoxSWL);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.statusBox);
            this.Controls.Add(this.portBox2);
            this.Controls.Add(this.callBox);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.nodeBox1);
            this.Controls.Add(this.SSBbutton);
            this.Controls.Add(this.chkDXMode);
            this.Controls.Add(this.textBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1000, 1000);
            this.MinimumSize = new System.Drawing.Size(799, 370);
            this.Name = "SpotControl";
            this.Text = "DX / SWL Spotter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpotControl_FormClosing);
            this.Load += new System.EventHandler(this.SpotControl_Load);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.SpotControl_Layout);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDisplayWWV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDisplayLong)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udDisplayLat)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        } //initializecomponents





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
            nodeBox1.Text = nodeB;
            portBox2.Text = portB;

            try
            {
                if (Convert.ToInt16(portBox2.Text) < 20)
                {
                    dataGridView1.CurrentCell = dataGridView1[0, Convert.ToInt16(portBox2.Text)];
                    Debug.WriteLine("retrieved the index from storage");

                }
            }
            catch (Exception)
            {
                dataGridView1.CurrentCell = dataGridView1[0, 0];

            }
        }

        //=======================================================================================================================
        private void SpotControl_FormClosing(object sender, FormClosingEventArgs e)
        {

            callB = callBox.Text;  // values to save in ke9ns.dat file
            nodeB = nodeBox1.Text;
            portB = portBox2.Text;
            nameB = nameBox.Text;


            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "SpotForm");

            console.DXMemList.Save1(); // save dx spotter list



        }

        public static byte SP_Active = 0;  // 1= DX Spot feature ON, 2=logging in 3=waiting for spots
        public static byte SP2_Active = 0; // DX Spot: 0=closed so ok to open again if you want, 1=in process of shutting down
        public static byte SP4_Active = 0; // 1=processing valid DX spot. 0=not processing new DX spot

        public static byte SP1_Active = 0; // SWL active
        public static byte SP3_Active = 0; // 1=SWL database loaded up, so no need to reload if you turn if OFF

        public static byte SP5_Active = 0; // 1= turn on tracking mode, but you might not have sun or grayline on

        //=======================================================================================
        //=======================================================================================
        // ke9ns SWL spotter // www.eibispace.de to get sked.csv file to read
        private void SWLbutton_Click(object sender, EventArgs e)
        {
            string file_name = " ";


            Debug.WriteLine("LOOK FOR SWL FILE " + console.AppDataPath);
            file_name = console.AppDataPath + "SWL.csv"; //   eibispace.de  sked - b15.csv


            if (!File.Exists(file_name))
            {
                Debug.WriteLine("problem no SWL.CSV file found ");
                statusBoxSWL.ForeColor = Color.Red;

                statusBoxSWL.Text = "No SWL.csv file found";

                return;
            }

            if ((SP1_Active == 0))
            {
                SP1_Active = 1;


                //    Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                //   Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

                Thread t = new Thread(new ThreadStart(SWLSPOTTER));

                t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

                t.Name = "SWL Spotter Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();

                //  while (t.IsAlive)
                //  {
                //      Thread.Sleep(50);
                //      Application.DoEvents();
                //  }


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


        // these are pulled from SWL2.csv file
        public static string[] SWL2_Station = new string[20000];       // Station name
        public static int[] SWL2_Freq = new int[20000];              // in hz
        public static byte[] SWL2_Band = new byte[20000];              // in Mhz

        public static string[] SWL2_Lang = new string[20000];          // language of transmitter
        public static int[] SWL2_TimeN = new int[20000];                // UTC time of operation ON air
        public static int[] SWL2_TimeF = new int[20000];                // UTC time of operation OFF air
        public static string[] SWL2_Mode = new string[20000];          // operating mode
        public static string[] SWL2_Day = new string[20000];          // days of operation
        public static byte[] SWL2_Day1 = new byte[20000];          // days of operation mo,tu,we,th,fr,sa,su = 1,2,4,8,16,32,64


        public static string[] SWL2_Loc = new string[20000];          // location of transmitter
        public static string[] SWL2_Target = new string[20000];          // target area of station
        public static int SWL2_Index1;  // local index that reset back to 0 after reaching max
        public static byte Flag21 = 0; // flag to skip header line in SWL.csv file


        // these are pulled from SWL.csv file
        public static string[] SWL_Station = new string[20000];       // Station name
        public static int[] SWL_Freq = new int[20000];              // in hz
        public static byte[] SWL_Band = new byte[20000];              // in Mhz
        public static int[] SWL_BandL = new int[31];              // index for each start of mhz listed in swl.csv

        public static string[] SWL_Lang = new string[20000];          // language of transmitter
        public static int[] SWL_TimeN = new int[20000];                // UTC time of operation ON air
        public static int[] SWL_TimeF = new int[20000];                // UTC time of operation OFF air
        public static string[] SWL_Mode = new string[20000];          // operating mode
        public static string[] SWL_Day = new string[20000];          // days of operation
        public static byte[] SWL_Day1 = new byte[20000];          // days of operation mo,tu,we,th,fr,sa,su = 1,2,4,8,16,32,64

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

        public static byte UTCDD = (byte)(1 << ((byte)UTCD.DayOfWeek));   // this is the day. SUn = 0, Mon = 1



        public static string FD = UTCD.ToString("HHmm");                                       // get 24hr 4 digit UTC NOW

        public static int UTCNEW1 = Convert.ToInt16(FD);                                       // convert 24hr UTC to int





        //=======================================================================================
        //=======================================================================================
        //ke9ns start SWL spotting THREAD 1 and done
        private void SWLSPOTTER()
        {

            string file_name = " ";
            string file_name1 = " ";

            int FLAG22 = 0;


            file_name = console.AppDataPath + "SWL.csv"; //  sked - b15.csv  
            file_name1 = console.AppDataPath + "SWL2.csv"; // ke9ns extra swl freq that eibispace.de wont add



            //-------------------------------------- SWL2.csv (ke9ns list of extra swl freqs to load)
            if (File.Exists(file_name1))
            {

                try
                {
                    stream2 = new FileStream(file_name1, FileMode.Open); // open file
                    reader2 = new BinaryReader(stream2, Encoding.ASCII);


                }
                catch (Exception)
                {
                    goto SWL1; // no extra SWL2 file, so just use SWL.csv file instead

                }

                var result = new StringBuilder();

                if (SP3_Active == 0) // dont reset if already scanned in  database
                {
                    SWL2_Index1 = 0; // how big is the SWL.CSV data file in lines
                    Flag21 = 0;

                }

                statusBoxSWL.Text = "Reading SWL2 ";

                //------------------------------------------------------------------
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
                    //    statusBoxSWL.Text = "Reading " + SWL_Index1.ToString();


                    if (SP_Active == 0)
                    {
                        console.spotterMenu.ForeColor = Color.Yellow;
                        console.spotterMenu.Text = "Reading";
                    }

                    try
                    {
                        var newChar = (char)reader2.ReadChar();

                        if (newChar == '\r')
                        {
                            newChar = (char)reader2.ReadChar(); // read \n char to finishline

                            if (Flag21 == 1)
                            {

                                string[] values;

                                values = result.ToString().Split(';'); // split line up into segments divided by ;

                                SWL2_Freq[SWL2_Index1] = (int)(Convert.ToDouble(values[0]) * 1000); // get freq and convert to hz
                                SWL2_Band[SWL2_Index1] = (byte)(SWL2_Freq[SWL2_Index1] / 1000000); // get freq and convert to mhz

                                SWL2_TimeN[SWL2_Index1] = Convert.ToInt16(values[1].Substring(0, 4)); // get time ON (24hr 4 digit UTC)
                                SWL2_TimeF[SWL2_Index1] = Convert.ToInt16(values[1].Substring(5, 4)); // get time OFF
                                SWL2_Day[SWL2_Index1] = values[2]; // get days ON

                                SWL2_Day1[SWL2_Index1] = 127; // digital signals are on 7 days

                                SWL2_Loc[SWL2_Index1] = values[3]; // get location of station
                                SWL2_Mode[SWL2_Index1] = "USB"; // get opeating mode
                                SWL2_Station[SWL2_Index1] = values[4]; // get station name
                                SWL2_Lang[SWL2_Index1] = values[5]; // get language
                                SWL2_Target[SWL2_Index1] = values[6]; // get station target area

                                SWL2_Index1++;

                            } // SWL Spots
                            else Flag21 = 1;

                            result = new StringBuilder(); // clean up for next line

                        }
                        else
                        {
                            result.Append(newChar);  // save char
                        }

                    }
                    catch (EndOfStreamException)
                    {
                        SWL2_Index1--;
                        // textBox1.Text = "End of SWL FILE at "+ SWL_Index1.ToString();
                        Debug.WriteLine(" SWL2_Freq[SWL2_Index1] " + SWL2_Freq[SWL2_Index1]);
                        break; // done with file
                    }
                    catch (Exception)
                    {
                        //  Debug.WriteLine("excpt======== " + e);
                        //     textBox1.Text = e.ToString();

                        break; // done with file
                    }


                } // for loop until end of file is reached


                // Debug.WriteLine("reached SWL end of file");


                reader2.Close();    // close  file
                stream2.Close();   // close stream


            } // if file exists SWL2


            // at this point we have SWL2 data read in

            SWL1:;



            //-------------------------------------- SWL.csv
            // SWL.CSV now requires a high freq end of file to allow merging SWL2.csv
            //This needs to be added to the bottom of SWL.csv:  900000; 0000 - 2400; ; USA; ENDOFFILE; E; USA; ; 1;;

            if (File.Exists(file_name))
            {

                try
                {
                    stream2 = new FileStream(file_name, FileMode.Open); // open file
                    reader2 = new BinaryReader(stream2, Encoding.ASCII);
                }
                catch (Exception)
                {
                    SP1_Active = 0; // turn off SWL spotter

                    statusBoxSWL.ForeColor = Color.Red;
                    statusBoxSWL.Text = "Off";


                    if (SP_Active == 0)
                    {
                        console.spotterMenu.ForeColor = Color.Red;
                        console.spotterMenu.Text = "Spotter";
                    }
                    return;
                }

                var result = new StringBuilder();

                if (SP3_Active == 0) // dont reset if already scanned in  database
                {
                    SWL_Index1 = 0; // how big is the SWL.CSV data file in lines
                    SWL_Index = 0; // was 0  start at 1 mhz
                    Flag1 = 0;

                }
                statusBoxSWL.Text = "Reading ";

                //------------------------------------------------------------------
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
                    //    statusBoxSWL.Text = "Reading " + SWL_Index1.ToString();


                    if (SP_Active == 0)
                    {
                        console.spotterMenu.ForeColor = Color.Yellow;
                        console.spotterMenu.Text = "Reading";
                    }


                    try
                    {
                        var newChar = (char)reader2.ReadChar();

                        if (newChar == '\r')
                        {
                            newChar = (char)reader2.ReadChar(); // read \n char to finishline

                            if (Flag1 == 1)
                            {

                                string[] values;

                                values = result.ToString().Split(';'); // split line up into segments divided by ;

                                SWL_Freq[SWL_Index1] = (int)(Convert.ToDouble(values[0]) * 1000); // get freq and convert to hz
                                SWL_Band[SWL_Index1] = (byte)(SWL_Freq[SWL_Index1] / 1000000); // get freq and convert to mhz

                                /*
                                                                    if (SWL_Band[SWL_Index1] > SWL_Index)
                                                                    {
                                                                        //  Debug.WriteLine("INDEX MHZ " + SWL_Index + " index1 " + SWL_Index1);
                                                                        SWL_BandL[SWL_Index] = SWL_Index1;                                   // SWL_BandL[0] = highest index under 1mhz, SWL_BandL[1] = highest index under 2mhz
                                                                        VFOHLast = 0; // refresh pan screen while loading
                                                                        SWL_Index++;
                                                                    }
                                */
                                SWL_TimeN[SWL_Index1] = Convert.ToInt16(values[1].Substring(0, 4)); // get time ON (24hr 4 digit UTC)
                                SWL_TimeF[SWL_Index1] = Convert.ToInt16(values[1].Substring(5, 4)); // get time OFF


                                SWL_Day1[SWL_Index1] = 0;
                                //---------------------------------------------------------------------------------------
                                // ke9ns look at daysofweek on the air sun=0,mon=1,tue=2, etc
                                if (values[2].Contains("-"))
                                {
                                    byte temp3 = 0;
                                    byte temp4 = 0;


                                    string temp1 = values[2].Substring(0, 2); // start day
                                    string temp2 = values[2].Substring(3, 2); // end day


                                    if (temp1 == "Tu") temp3 = 2;             // get your start day position
                                    else if (temp1 == "We") temp3 = 3;
                                    else if (temp1 == "Th") temp3 = 4;
                                    else if (temp1 == "Fr") temp3 = 5;
                                    else if (temp1 == "Sa") temp3 = 6;
                                    else if (temp1 == "Su") temp3 = 0;
                                    else temp3 = 1; // mon

                                    if (temp2 == "Tu") temp4 = 2;           // get your end day position
                                    else if (temp2 == "We") temp4 = 3;
                                    else if (temp2 == "Th") temp4 = 4;
                                    else if (temp2 == "Fr") temp4 = 5;
                                    else if (temp2 == "Sa") temp4 = 6;
                                    else if (temp2 == "Su") temp4 = 0;
                                    else temp4 = 1; // mon

                                    if (temp3 < temp4) // example su thru fr
                                    {
                                        for (int x = temp3; x <= temp4; x++)
                                        {
                                            SWL_Day1[SWL_Index1] |= (byte)(1 << x);
                                        }
                                    } // example su thru sa
                                    else // example fr thru tu
                                    {
                                        for (int x = temp3; x < 7; x++)
                                        {
                                            SWL_Day1[SWL_Index1] |= (byte)(1 << x);
                                        }
                                        for (int x = 0; x <= temp4; x++)
                                        {
                                            SWL_Day1[SWL_Index1] |= (byte)(1 << x);
                                        }


                                    } // example fr thru tu


                                } // contains -
                                else
                                {
                                    if (values[2].Contains("Mo")) SWL_Day1[SWL_Index1] |= 2;
                                    if (values[2].Contains("Tu")) SWL_Day1[SWL_Index1] |= 4;
                                    if (values[2].Contains("We")) SWL_Day1[SWL_Index1] |= 8;
                                    if (values[2].Contains("Th")) SWL_Day1[SWL_Index1] |= 16;
                                    if (values[2].Contains("Fr")) SWL_Day1[SWL_Index1] |= 32;
                                    if (values[2].Contains("Sa")) SWL_Day1[SWL_Index1] |= 64;
                                    if (values[2].Contains("Su")) SWL_Day1[SWL_Index1] |= 1; // 64

                                } // this checks for Mo,Tu,Sa  etc. etc.



                                if (SWL_Day1[SWL_Index1] == 0) SWL_Day1[SWL_Index1] = 127; // if no days then all 7 days
                                SWL_Day[SWL_Index1] = values[2]; // get days ON

                                //--------------------------------------------------------------------
                                //  if (SWL_Freq[SWL_Index1] == 7315000) Debug.WriteLine("station found" + SWL_Freq[SWL_Index1] + " , "+ SWL_Day1[SWL_Index1]);


                                SWL_Loc[SWL_Index1] = values[3]; // get location of station
                                SWL_Mode[SWL_Index1] = "AM"; // get opeating mode
                                SWL_Station[SWL_Index1] = values[4]; // get station name
                                SWL_Lang[SWL_Index1] = values[5]; // get language
                                SWL_Target[SWL_Index1] = values[6]; // get station target area



                                //-------------------------------------------------------------------------------------
                                //-------------------------------------------------------------------------------------
                                //-------------------------------------------------------------------------------------
                                // Ke9ns MERGE SWL and SWL2
                                FLAG22 = 0; // reset for next line

                                if ((SWL2_Index1 > 0) && (SWL_Index1 > 2)) // only try and merge SWL2 into SWL if SWL2 exists
                                {
                                    int lowfreq = SWL_Freq[SWL_Index1 - 1]; // prior freq read in
                                    int highfreq = SWL_Freq[SWL_Index1]; // freq just read in now


                                    // now check to see if any SWL2 freqs can fit in between lines of the SWL file
                                    for (int q = 0; q <= SWL2_Index1; q++)
                                    {

                                        if ((SWL2_Freq[q] < highfreq)) // are you below the current (just read in) swl listing?
                                        {
                                            if ((SWL2_Freq[q] >= lowfreq)) // are you above the last read in swl listing?
                                            {
                                                // move this "just read in" SWL line forward in the index to insert SWL2 entry
                                                SWL_Freq[SWL_Index1 + 1] = SWL_Freq[SWL_Index1];

                                                SWL_Band[SWL_Index1 + 1] = SWL_Band[SWL_Index1];

                                                SWL_TimeN[SWL_Index1 + 1] = SWL_TimeN[SWL_Index1];
                                                SWL_TimeF[SWL_Index1 + 1] = SWL_TimeF[SWL_Index1];
                                                SWL_Day[SWL_Index1 + 1] = SWL_Day[SWL_Index1];
                                                SWL_Day1[SWL_Index1 + 1] = SWL_Day1[SWL_Index1];
                                                SWL_Loc[SWL_Index1 + 1] = SWL_Loc[SWL_Index1];
                                                SWL_Mode[SWL_Index1 + 1] = SWL_Mode[SWL_Index1];
                                                SWL_Station[SWL_Index1 + 1] = SWL_Station[SWL_Index1];
                                                SWL_Lang[SWL_Index1 + 1] = SWL_Lang[SWL_Index1];
                                                SWL_Target[SWL_Index1 + 1] = SWL_Target[SWL_Index1];


                                                // save SWL2 entry into SWL listing 
                                                SWL_Freq[SWL_Index1] = SWL2_Freq[q];

                                                SWL_Band[SWL_Index1] = SWL2_Band[q];

                                                SWL_TimeN[SWL_Index1] = SWL2_TimeN[q];
                                                SWL_TimeF[SWL_Index1] = SWL2_TimeF[q];
                                                SWL_Day[SWL_Index1] = SWL2_Day[q];
                                                SWL_Day1[SWL_Index1] = SWL2_Day1[q];
                                                SWL_Loc[SWL_Index1] = SWL2_Loc[q];
                                                SWL_Mode[SWL_Index1] = SWL2_Mode[q];
                                                SWL_Station[SWL_Index1] = SWL2_Station[q];
                                                SWL_Lang[SWL_Index1] = SWL2_Lang[q];
                                                SWL_Target[SWL_Index1] = SWL2_Target[q];

                                                //  Debug.WriteLine("INSERT 2 HERE= index=" + SWL_Index1 + " Freq=" + SWL_Freq[SWL_Index1] + " station name=" + SWL_Station[SWL_Index1]);

                                                FLAG22 = 1; // flag that you inserted a new SWL2 line into SWL

                                                if (SWL_Band[SWL_Index1] > SWL_Index) // MHZ of the current examined spot > the mhz your looking at?
                                                {
                                                    //  Debug.WriteLine("INDEX MHZ " + SWL_Index + " index1 " + SWL_Index1 + "swl_Band[]" + SWL_Band[SWL_Index1] + " station name: " + SWL_Station[SWL_Index1] + " Freq: " + SWL_Freq[SWL_Index1]);
                                                    SWL_BandL[SWL_Index] = SWL_Index1;                                   // SWL_BandL[0] = highest index under 1mhz, SWL_BandL[1] = highest index under 2mhz
                                                    VFOHLast = 0; // refresh pan screen while loading
                                                    SWL_Index++;
                                                }

                                                SWL_Index1++; // add SWL2 into the SWL list

                                            } // if ( (SWL2_Freq[q] >= lowfreq)) // are you above the last read in swl listing?

                                        } //  if ((SWL2_Freq[q] < highfreq) ) // are you below the current (just read in) swl listing?
                                        else
                                        {
                                            break; // break out of this SWL2 loop
                                        }


                                    } // for loop through SWL2

                                } // if ((SWL2_Index1 > 0) && (SWL_Index1 > 2))

                                // below is to shorten the amount of time the swl routine in display.cs needs to find the swl listings to display in the pan
                                if (SWL_Band[SWL_Index1] > SWL_Index) // MHZ of the current examined spot > the mhz your looking at?
                                {
                                    //  Debug.WriteLine("INDEX MHZ " + SWL_Index + " index1 " + SWL_Index1 +"swl_Band[]" + SWL_Band[SWL_Index1] + " station name: " + SWL_Station[SWL_Index1] + " Freq: " + SWL_Freq[SWL_Index1]);
                                    SWL_BandL[SWL_Index] = SWL_Index1;                                   // SWL_BandL[0] = highest index under 1mhz, SWL_BandL[1] = highest index under 2mhz
                                    VFOHLast = 0; // refresh pan screen while loading
                                    SWL_Index++;
                                }

                                //-------------------------------------------------------------------------------------
                                //-------------------------------------------------------------------------------------
                                //-------------------------------------------------------------------------------------
                                // check for DUPS
                                if (SWL_Index > 0)
                                {
                                    //-------------------------------------------------------------------------------------------------
                                    // ke9ns if the Station Name and Station Freq and Station Days are the same, then check time (below)
                                    if ((SWL_Station[SWL_Index1 - 1] == SWL_Station[SWL_Index1]) &&     // same station NAME
                                        (SWL_Freq[SWL_Index1 - 1] == SWL_Freq[SWL_Index1]) &&           // same Freq
                                        (SWL_Day1[SWL_Index1 - 1] == SWL_Day1[SWL_Index1]))             // same Days on the air
                                    {
                                        //------------------------------------------------------------------------------------
                                        if ((SWL_TimeN[SWL_Index1 - 1] < SWL_TimeN[SWL_Index1]))       // first spot has earlier start time than this spot, then do below
                                        {
                                            if (SWL_TimeF[SWL_Index1 - 1] >= SWL_TimeN[SWL_Index1])    // if the first spot stays on the air past the start of the new spot, then do below
                                            {
                                                if (SWL_TimeF[SWL_Index1 - 1] < SWL_TimeF[SWL_Index1]) // if the first spot leaves the air before the new spot leaves the air then use the new spots finish time
                                                {
                                                    SWL_TimeF[SWL_Index1 - 1] = SWL_TimeF[SWL_Index1];
                                                }

                                                goto BYPASS; // duplicate
                                            }
                                        }

                                        //------------------------------------------------------------------------------------
                                        if ((SWL_TimeN[SWL_Index1 - 1] > SWL_TimeN[SWL_Index1]))       // first spot has later start time than this new spot
                                        {
                                            if (SWL_TimeF[SWL_Index1 - 1] >= SWL_TimeN[SWL_Index1])    // if the first spot stays on the air past the start of the new spot, then do below
                                            {
                                                SWL_TimeN[SWL_Index1 - 1] = SWL_TimeN[SWL_Index1];     // use earlier time from new spot

                                                if (SWL_TimeF[SWL_Index1 - 1] < SWL_TimeF[SWL_Index1])
                                                {
                                                    SWL_TimeF[SWL_Index1 - 1] = SWL_TimeF[SWL_Index1];
                                                }

                                                goto BYPASS; // duplicate
                                            }

                                        }

                                        //------------------------------------------------------------------------------------
                                        if ((SWL_TimeN[SWL_Index1 - 1] == SWL_TimeN[SWL_Index1]))   // if the start time matches do below
                                        {
                                            if (SWL_TimeF[SWL_Index1 - 1] < SWL_TimeF[SWL_Index1]) // if the next in the list stays on the air longer, use its end time and bypass
                                            {
                                                SWL_TimeF[SWL_Index1 - 1] = SWL_TimeF[SWL_Index1];
                                                goto BYPASS; // duplicate
                                            }

                                        } // if time on matches

                                        //------------------------------------------------------------------------------------
                                        if ((SWL_TimeN[SWL_Index1 - 1] == SWL_TimeN[SWL_Index1])             // same ON time
                                            && (SWL_TimeF[SWL_Index1 - 1] == SWL_TimeF[SWL_Index1]))         //smae OFF time
                                        {
                                            goto BYPASS; // duplicate
                                        }


                                    } // name and freq match

                                }  //  if (SWL_Index > 0)

                                SWL_Index1++; // save this

                                BYPASS:;


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

                        break; // done with file
                    }
                    catch (Exception)
                    {
                        //  Debug.WriteLine("excpt======== " + e);
                        //     textBox1.Text = e.ToString();

                        break; // done with file
                    }


                } // for loop until end of file is reached


                // Debug.WriteLine("reached SWL end of file");


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





            //-----------------------------------------
            // ke9ns track ISS



            /*
            try
            {

                // http://api.open-notify.org/iss-now.json

                textBox1.Text += "Attempt to connect to ISS Now http \r\n";

                serverPath = "http://api.open-notify.org/iss-now.json";

            
                HttpWebRequest request = WebRequest.Create(serverPath) as HttpWebRequest;

                textBox1.Text += "Attempt to download ISS position \r\n";

                using(HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));
                    }

                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Response));
                    object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());


                   Response jsonResponse = objResponse as Response;

                }




                //  Stream responseStream = response.GetResponseStream();
                //   StreamReader reader = new StreamReader(responseStream);
                //    noaa = reader.ReadToEnd();

                //  reader.Close();
                //   response.Close();
                //   Debug.WriteLine("noaa=== " + noaa);

                textBox1.Text += "ISS  Download complete \r\n";

            }
            catch (Exception ex)
            {
                //   Debug.WriteLine("ISS fault=== " + ex);
                textBox1.Text += "Failed to get ISS data \r\n";

            }
*/

        } // SWLSPOTTER








        //=======================================================================================
        //=======================================================================================
        //=======================================================================================
        //=======================================================================================
        //ke9ns start DX spotting
        private void spotSSB_Click(object sender, EventArgs e)
        {
            //   Debug.WriteLine("TESt");
            //  Debug.WriteLine("========row " + dataGridView1.CurrentCell.RowIndex);
            //   Debug.WriteLine("========URL " + (string)dataGridView1["dxurl", dataGridView1.CurrentCell.RowIndex].Value);

            if ((SP2_Active == 0) && (SP_Active == 0) && (callBox.Text != "callsign") && (callBox.Text != null) ) // dont allow dx spotting while beacon is on.
            {

                Debug.WriteLine("DX SPOTTER ON start THREAD");

                Thread t = new Thread(new ThreadStart(SPOTTER));

                t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");


                SP_Active = 1;
                t.Name = "Spotter Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal; // normal
                t.Start();

                textBox1.Text = "Clicked to Open DX Spider \r\n";

            }
            else if ((callBox.Text == "callsign") || (callBox.Text == null))
            {

                Debug.WriteLine("callbox " + callBox.Text);

                textBox1.Text += "Must put your CALL Sign in the CALLSIGN box (lower Right of this window)\r\n";
                callBox.ForeColor = Color.Red;
                callBox.BackColor = Color.Yellow;


            }
            else
            {
                Debug.WriteLine("DX SPOTTER OFF, Thread not started " + SP2_Active + " , " + SP_Active + " , " + beacon1);

                SP_Active = 0; // turn off DX Spotter

                statusBox.ForeColor = Color.Red;
                console.spotterMenu.ForeColor = Color.Red;

                console.spotterMenu.Text = "Closing";
                statusBox.Text = "Closing";

                textBox1.Text += "Clicked to Close Socket (click again to Force Closed)\r\n";

                if (SP2_Active != 0)
                {
                    textBox1.Text += "Force closed \r\n";

                    try
                    {
                        SP_writer.Close();
                        SP_reader.Close();
                    }
                    catch (Exception)
                    {
                        Debug.Write("writer/reader was not open to close");
                    }

                    try
                    {

                        networkStream.Close();
                        client.Close();
                    }
                    catch (Exception)
                    {
                        Debug.Write("networkstream was never open to close");
                    }

                    SP_Active = 0; // turn off DX Spotter
                    SP2_Active = 0; // turn off DX Spotter
                }
                else SP2_Active = 1; // in process of shutting down.


            } // turn DX spotting off


        } //  spotSSB_Click




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


        public static string[] DX_FULLSTRING = new string[1000];    // full undecoded message

        public static string[] DX_Station = new string[1000];       // Extracted: dx call sign
        public static int[] DX_Freq = new int[1000];                // Extracted: Freq in hz
        public static string[] DX_Spotter = new string[1000];       // Extracted: spotter call sign
        public static string[] DX_Message = new string[1000];       // Extracted: message
        public static int[] DX_Mode = new int[1000];                // Parse: Mode from message string 0=ssb,1=cw,2=rtty,3=psk,4=olivia,5=jt65,6=contesa,7=fsk,8=mt63,9=domi,10=packtor, 11=fm, 12=drm, 13=sstv, 14=am
        public static int[] DX_Mode2 = new int[1000];               // Parse: split parse from message string 0=normal , +up in hz or -dn in hz
        public static int[] DX_Time = new int[1000];                // Extracted: GMT (Unreliable because its the time submitted by the spotter)
        public static string[] DX_Grid = new string[1000];          // Extracted and Parsed: grid square 
        public static string[] DX_Age = new string[1000];           // Calculated: how old is the spot

        public static int[] DX_Beam = new int[1000];                // Calculated: beam heading from your lat/long

        public static int[] DX_X = new int[1000];                   // Calculated: x pixel location on map (before any scaling) Longitude
        public static int[] DX_Y = new int[1000];                   // Calculated: y pixel location on map (before any scaling) Latitude
        public static string[] DX_country = new string[1000];       // Calculated: country  by matching the callsign pulled from DXLOC.txt file

      


        //-------------------------------------------------------------------------------------

        // holds information from NCDXF/IARU Beacon stations
        // PowerSDR will scan through 14.1, 18.11, 21.15, 24.93, 28.2 mhz
        // looking for 18 stations: 4U1UN, VE8AT, W6WX, KH6RS, ZL6B, VK6RBP, 
        // JA1IGY, RR9O, VR2B, 4S7B, ZS6DN, 5Z4B, 4X6TU, OH2B, CS3B, LU4AA,
        // OA4B, and YV5B in 10 second intervals.Thats 5 frequecies and 18 stations
        // rotating in 10 intervals = 10 * 18 = 180second = 3minutes until a repeat.
        //
        public static string[] BX_FULLSTRING = new string[100];    // full undecoded message

        public static string[] BX_Station = new string[100];       // Extracted: dx call sign
        public static int[] BX_Freq = new int[100];                // Extracted: Freq in hz
        public static string[] BX_Spotter = new string[100];       // Extracted: spotter call sign
        public static string[] BX_Message = new string[100];       // Extracted: message
        public static int[] BX_Mode = new int[100];                // Parse: Mode from message string 0=ssb,1=cw,2=rtty,3=psk,4=olivia,5=jt65,6=contesa,7=fsk,8=mt63,9=domi,10=packtor, 11=fm, 12=drm, 13=sstv, 14=am
        public static int[] BX_Mode2 = new int[100];               // Parse: split parse from message string 0=normal , +up in hz or -dn in hz
        public static int[] BX_Time = new int[100];                // Extracted: GMT (Unreliable because its the time submitted by the spotter)
        public static string[] BX_Grid = new string[100];          // Extracted and Parsed: grid square 
        public static string[] BX_Age = new string[100];           // Calculated: how old is the spot

        public static int[] BX_Beam = new int[100];                // Calculated: beam heading from your lat/long

        public static int[] BX_X = new int[100];                   // Calculated: x pixel location on map (before any scaling) Longitude
        public static int[] BX_Y = new int[100];                   // Calculated: y pixel location on map (before any scaling) Latitude
        public static string[] BX_country = new string[100];       // Calculated: country  by matching the callsign pulled from DXLOC.txt file

        public static int[] BX_dBm = new int[100];                // place to record the signal strength of received stations in dbm
        public static int[] BX_dBm1 = new int[100];               // place to record the noise floor of each freq
        public static int   BX_dBm2 =0;                           // avg value base line db passed back from display.cs
        public static int[] BX_dBm3 = new int[100];               // place to record the background signal strength of received stations as a S#

        public static int[] BX_TSlot = new int[100];              // time slot to hear this particular station of this particular freq (0 to 180 seconds)

        public static int[] BX_TSlot1 = new int[100];              // time slot for each of the 5 freq and 18 stations (0 to 170) but there are 5 of every time, 5 0's, 5 10sec, 5 20sec

        public static int BX_TSlot2 = 0;                         // time slot currently viewed 0 to 170 in 10sec increments

        public static int[] BX_Index = new int[5];                //  keep track of which freq (0-4) you are on for each station 

        public int BX1_Index = 0;                                 // should always be 90 for NCDXF beacons (5 x 18). Index for entire Beacon list (just like DX_Index for Dx spotter) 

        public bool BX_Load = false;   // true = BX_ values above all loaded 1 time, so no need to do it again. this way you can flip beacon on/off and see your last scan data

        //-------------------------------------------------------------------------------------


        public static int DX_Index = 0;                             //  max number of spots in memory currently
        public static int DX_Index1 = 0;                            //  static temp index holder....always 250
        public static int DX_Last = 0;                              //  last # in DX_Index (used for DXLOC_Mapper)spotter(
        public static int Map_Last = 0;                             //  last map checkbox change (used for DXLOC_Mapper) 1=update grayline 2=update spots on map only
        public static int DXK_Last = 0;                             //  last # in console.DXK (used for DXLOC_Mapper)


 
        public static int UTCNEW = Convert.ToInt16(FD);                        // convert 24hr UTC to int
        public static int UTCLAST = 0;                                        // last utc time for determining when to check again
        public static int UTCLASTMIN = 0;                                        // last utc time for determining when to check again

        private bool detectEncodingFromByteOrderMarks = true;

        private bool pause = false; // true = pause dx spot window update.

        private static byte Flag8 = 0; // 1= DX_Index value changed due to spot age cut, 

        //====================================================================================================
        //====================================================================================================
        // ke9ns add Thread routine (get DX spots)

        private void SPOTTER()  // ke9ns Thread opeation (runs in en-us culture) opens internet connection to genearte list of dx spots
        {

           DXLOC_FILE(); // open DXLOC.txt file and put into array of lat/lon values vs prefix

         
            try // opening socket
            {
                textBox1.Text += "Attempt Opening socket \r\n";

                client = new TcpClient(); // for new socket

            
                DXMemRecord nodeBox5 = new DXMemRecord(console.DXMemList.List[dataGridView1.CurrentCell.RowIndex]); // ke9ns 

                nodeBox1.Text = nodeBox5.DXURL; // get string from DXMemory file based on current pointed to index

                Debug.WriteLine("node " + nodeBox1.Text);
              
             
                if (nodeBox1.Text.Contains(":") == true)
                {
                    int ind = nodeBox1.Text.IndexOf(":") + 1;
                    int ind1 = nodeBox1.Text.Length;

                    portBox2.Text = dataGridView1.CurrentCell.RowIndex.ToString(); // to store

                    string PORT1 = nodeBox1.Text.Substring(ind, ind1 - ind);
                    string URL1 = nodeBox1.Text.Substring(0, ind - 1);

                    Debug.WriteLine("url " + URL1);
                    Debug.WriteLine("port " + PORT1);
                    Debug.WriteLine("index " + portBox2.Text);


                    client.Connect(URL1, Convert.ToInt16(PORT1));      // 'EXAMPLE  client.Connect("192.168.0.149", 230) 
                }
                else
                {
                    Debug.WriteLine("NO PORT# detected us 7000 " );

                    client.Connect(nodeBox1.Text, 7000);      // 'EXAMPLE  client.Connect("192.168.0.149", 230) 
                }

               
                networkStream = client.GetStream();

                SP_reader = new StreamReader(networkStream,Encoding.ASCII,detectEncodingFromByteOrderMarks); //Encoding.UTF8  or detectEncodingFromByteOrderMarks
                SP_writer = new BinaryWriter(networkStream,Encoding.UTF7);
   
               
                var sb = new StringBuilder(message2);
                statusBox.ForeColor = Color.Red;
                console.spotterMenu.ForeColor = Color.Red;

                statusBox.Text = "Socket";
                console.spotterMenu.Text = "Socket";

                textBox1.Text += "Got Socket \r\n";


                for (; SP_Active > 0;) // shut down socket and thread if SP_Active = 0  (0=off, 1=turned on, 2=logging , 3=waiting for spots)
                {
   
                    if (SP_Active == 1) // if you shut down dont attempt to read next spot
                    {
                        sb.Append((char)SP_reader.Read(), 1);

                        message3 = sb.ToString();

                        if ((message3.Contains("login: ")) || (message3.Contains("Please enter your call: ")))
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
                         //  SP_reader.BaseStream.ReadTimeout = 3000; // 5000 cause character Read to break every 5 seconds to check age of DX spots

                      //-------------------------------------------------------------------------------------------------------------------------------------
                      // ke9ns wait for a new message

                            for (;!(sb.ToString().Contains("\r\n"));) //  wait for end of line
                            {
                                processDXAGE();

                                Thread.Sleep(50); // slow down the thread here

                                sb.Append((char)SP_reader.Read());  // get next char from socket and add it to build the next dx spot string to parse out 
                             

                                if (SP_Active == 0)
                                 {
                                    Debug.WriteLine("break====="); // if user wants to shut down operation 
                                    break;
                                }

                                if (sb.ToString().Length > 90)
                                {
                                    Debug.WriteLine("Leng ====="); // string too long (something happened
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
                          // Debug.WriteLine("message1 length " + qq);

                            if (qq == 75) // if no grid, then add spaces and CR and line feed
                            {
                                sb.Append("     "); // keep all strings the same length
                            }
                           
                            message1 = sb.ToString(); // post message
                            message1.TrimEnd('\0');

                            // ke9ns so at this point all messages are 82 characters long (as though they have a grid#, even if they dont)
                            //   Debug.WriteLine("message2 length " + message1.Length);

                        }
                        catch // read timeout comes here
                        {
                            //  processDXAGE();
                            //   if (Flag8 == 0) continue; // if DX_Index value changed due to age, then proceess otherwise continue around
                            //  Flag8 = 0;

                            continue;

                        } // end of catch (read timeout comes here)


                        Debug.WriteLine("message " + message1);


                        //-------------------------------------------------------------------------------------------------------------------------------------
                        // ke9ns process received message
                        if ((message1.StartsWith("DX de ") == true) && (message1.Length > 76)) // string can be 77 (with no grid) or 82 (with grid)
                        {
                          
                            DX_Index1 = 250; // use 900 as a temp holding spot. always fill from the top


                            // grab DX_Spotter=======================================================================================
                            try
                            {
                                DX_Spotter[DX_Index1] = message1.Substring(6, 10); // get dx call with : at the end
                                 Debug.WriteLine("DX_Call " + DX_Station[DX_Index1]);

                                int pos = 10;
                                if (DX_Spotter[DX_Index1].Contains(":"))
                                {
                                     pos = DX_Spotter[DX_Index1].IndexOf(':'); // find the :
                                }
                                else
                                {
                                     pos = DX_Spotter[DX_Index1].IndexOf(' '); // find the first space instead of the :
                                }
                               

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
                            //    Debug.WriteLine("DX_Call " + DX_Station[DX_Index1]);

                            // grab DX_Freq========================================================================================
                            try
                            {
                               
                                DX_Freq[DX_Index1] = (int)((double)Convert.ToDouble(message1.Substring(15, 9)) * (double) 1000.0    ); //  get dx freq 7016.0  in khz 
                        
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
                            Debug.WriteLine("DX_Freq " + DX_Freq[DX_Index1]);


                            // grad DX_Station=========================================================================================

                            try
                            {
                                DX_Station[DX_Index1] = message1.Substring(26, 13); // get dx call with : at the end
                                int pos = DX_Station[DX_Index1].IndexOf(' '); // find the
                                DX_Station[DX_Index1] = DX_Station[DX_Index1].Substring(0, pos); // reduce the call without the
                            }
                            catch (FormatException )
                            {
                                DX_Spotter[DX_Index1] = "NA";
                              }
                            catch (ArgumentOutOfRangeException )
                            {
                                DX_Spotter[DX_Index1] = "NA";
                            }
                              Debug.WriteLine("DX_Spotter " + DX_Spotter[DX_Index1]);

                            // grab comments
                            try
                            {
                                DX_Mode2[DX_Index1] = 0; // reset split hz

                                DX_Message[DX_Index1] = message1.Substring(39, 29).ToLower(); // get dx call with : at the end
                              

                                if (DX_Message[DX_Index1].Contains("cw"))
                                {
                                      DX_Mode[DX_Index1] = 1; // cw mode

                                }
                                else if (DX_Message[DX_Index1].Contains(" rty ") || DX_Message[DX_Index1].Contains("rtty"))
                                {
                                    if (chkBoxDIG.Checked != true) continue; // check for a Digitla mode spot
                                    DX_Mode[DX_Index1] = 2; // RTTY mode

                                }
                                else if (DX_Message[DX_Index1].Contains("psk"))
                                {
                                    if (chkBoxDIG.Checked != true) continue; // check for a Digitla mode spot
                                    DX_Mode[DX_Index1] = 3; // psk mode

                                }
                                else if (DX_Message[DX_Index1].Contains("oliv"))
                                {
                                    if (chkBoxDIG.Checked != true) continue; // check for a Digitla mode spot
                                    DX_Mode[DX_Index1] = 4; // olivia mode

                                }
                                else if (DX_Message[DX_Index1].Contains("jt65"))
                                {
                                    if (chkBoxDIG.Checked != true) continue; // check for a Digitla mode spot
                                    DX_Mode[DX_Index1] = 5; // jt65 mode

                                }
                                else if (DX_Message[DX_Index1].Contains("contesa"))
                                {
                                    if (chkBoxDIG.Checked != true) continue; // check for a Digitla mode spot
                                    DX_Mode[DX_Index1] = 6; // contesa mode

                                }
                                else if (DX_Message[DX_Index1].Contains("fsk"))
                                {
                                    if (chkBoxDIG.Checked != true) continue; // check for a Digitla mode spot
                                    DX_Mode[DX_Index1] = 7; // fsk mode

                                }
                                else if (DX_Message[DX_Index1].Contains("mt63"))
                                {
                                    if (chkBoxDIG.Checked != true) continue; // check for a Digitla mode spot
                                    DX_Mode[DX_Index1] = 8; // mt63 mode

                                }
                                else if (DX_Message[DX_Index1].Contains("domi"))
                                {
                                    if (chkBoxDIG.Checked != true) continue; // check for a Digitla mode spot
                                    DX_Mode[DX_Index1] = 9; // domino mode

                                }
                                else if (DX_Message[DX_Index1].Contains("packact")|| DX_Message[DX_Index1].Contains("packtor")||DX_Message[DX_Index1].Contains("amtor"))
                                {
                                    if (chkBoxDIG.Checked != true) continue; // check for a Digitla mode spot
                                    DX_Mode[DX_Index1] = 10; // pactor mode

                                }
                                else if (DX_Message[DX_Index1].Contains("fm "))
                                {
                                    if (chkBoxSSB.Checked != true) continue; // check for a SSB mode spot
                                    DX_Mode[DX_Index1] = 11; // fm mode

                                }
                                else if (DX_Message[DX_Index1].Contains("drm"))
                                {
                                    if (chkBoxDIG.Checked != true) continue; // check for a Digitla mode spot
                                    DX_Mode[DX_Index1] = 12; // DRM mode

                                }
                                else if (DX_Message[DX_Index1].Contains("sstv"))
                                {
                                    if (chkBoxDIG.Checked != true) continue; // check for a Digitla mode spot
                                    DX_Mode[DX_Index1] = 13; // sstv mode

                                }
                                else if (DX_Message[DX_Index1].Contains("easypal"))
                                {
                                    if (chkBoxDIG.Checked != true) continue; // check for a Digitla mode spot
                                    DX_Mode[DX_Index1] = 12; // drm mode

                                }
                                else if (DX_Message[DX_Index1].Contains(" am ")|| DX_Message[DX_Index1].Contains(" sam "))
                                {
                                    if (chkBoxSSB.Checked != true) continue; // check for a SSB mode spot
                                    DX_Mode[DX_Index1] = 14; // AM mode

                                }




                                if (DX_Mode[DX_Index1] == 0)
                                {

                                    if (chkBoxSSB.Checked != true)
                                    {
                                       Debug.WriteLine("bypass ssb because not looking for ssb");
                                        continue; // check for a SSB mode spot
                                    }

                                }

                                if (DX_Mode[DX_Index1] == 1)
                                {

                                    if (chkBoxCW.Checked != true)
                                    {
                                       Debug.WriteLine("bypass CW because not looking for CW");
                                        continue; // check for a CW mode spot
                                    }

                                }

                                //----------------------------------------------------------

                                // grab GRID #
                                DX_Grid[DX_Index1] = message1.Substring(76,4); // get grid

                              sb = new StringBuilder(DX_Grid[DX_Index1]); // clear sb string over again
                              sb.Append(')');
                              sb.Insert(0, '('); // to differentiate the spotter from the spotted

                              DX_Grid[DX_Index1] = sb.ToString();
                      


                          
                                if (DX_Message[DX_Index1].Contains("<") && DX_Message[DX_Index1].Contains(">") ) // check for split
                                {

                                    int ind = DX_Message[DX_Index1].IndexOf(">") + 1;

                                    try // 
                                    {
                                        DX_Grid[DX_Index1] = DX_Message[DX_Index1].Substring(ind, 6);
                                        Debug.WriteLine("FOUND COMMENT GRID " + DX_Grid[DX_Index1]);
                                       
                                    }
                                    catch // 
                                    {

                                    }

                                } // get Grid from comments


                                //----------------------------------------------------------

                                DX_Mode2[DX_Index1] = 0;
                                //  resultString = Regex.Match(subjectString, @"\d+").Value;  Int32.Parse(resultString) will then give you the number.

                                if (DX_Message[DX_Index1].Contains("up")) // check for split
                                {
                                  
                                    int ind = DX_Message[DX_Index1].IndexOf("up") + 2;

                                    try // try 1
                                    {
                                        int split_hz = (int)(Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 4))* 1000));
                                       Debug.WriteLine("Found UP split hz" +split_hz);
                                        DX_Mode2[DX_Index1] = split_hz;
                                    }
                                    catch // catch 1
                                    {

                                        try // try 2
                                        {
                                            int split_hz = (int)(Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 3)) * 1000));
                                          Debug.WriteLine("Found UP split hz" + split_hz);
                                            DX_Mode2[DX_Index1] = split_hz;
                                        }
                                        catch // catch 2
                                        {

                                            try // try 3
                                            {
                                                int split_hz = (int)(Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 2)) * 1000));
                                              Debug.WriteLine("Found UP split hz" + split_hz);
                                                DX_Mode2[DX_Index1] = split_hz;
                                            }
                                            catch // catch 3
                                            {

                                                int ind1 = DX_Message[DX_Index1].IndexOf("up") - 4; //

                                                try // try 4
                                                {

                                                    int split_hz = (int)(Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1,4)) * 1000));
                                                   Debug.WriteLine("Found UP split hz" + split_hz);
                                                    DX_Mode2[DX_Index1] = split_hz;
                                                }
                                                catch // catch 4
                                                {
                                                    ind1++; //

                                                    try // try 5
                                                    {

                                                        int split_hz = (int)(Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 3)) * 1000));
                                                       Debug.WriteLine("Found UP split hz" + split_hz);
                                                        DX_Mode2[DX_Index1] = split_hz;
                                                    }
                                                    catch // catch 5
                                                    {
                                                        ind1++; //

                                                        try // try 6
                                                        {

                                                            int split_hz = (int)(Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 2)) * 1000));
                                                           Debug.WriteLine("Found UP split hz" + split_hz);
                                                            DX_Mode2[DX_Index1] = split_hz;
                                                        }
                                                        catch // catch 6
                                                        {

                                                          Debug.WriteLine("failed to find up value================");
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
                                        Debug.WriteLine("Found dn split hz" + split_hz);
                                        DX_Mode2[DX_Index1] = split_hz;
                                    }
                                    catch // catch 1
                                    {

                                        try // try 2
                                        {
                                            int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 3)) * 1000));
                                          Debug.WriteLine("Found dn split hz" + split_hz);
                                            DX_Mode2[DX_Index1] = split_hz;
                                        }
                                        catch // catch 2
                                        {

                                            try // try 3
                                            {
                                                int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 2)) * 1000));
                                               Debug.WriteLine("Found dn split hz" + split_hz);
                                                DX_Mode2[DX_Index1] = split_hz;
                                            }
                                            catch // catch 3
                                            {

                                                int ind1 = DX_Message[DX_Index1].IndexOf("dn") - 4; //

                                                try // try 4
                                                {
                                                    int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 4)) * 1000));
                                                    Debug.WriteLine("Found dn split hz" + split_hz);
                                                    DX_Mode2[DX_Index1] = split_hz;
                                                }
                                                catch // catch 4
                                                {
                                                    ind++; //

                                                    try // try 5
                                                    {
                                                        int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 3)) * 1000));
                                                        Debug.WriteLine("Found dn split hz" + split_hz);
                                                        DX_Mode2[DX_Index1] = split_hz;
                                                    }
                                                    catch // catch 5
                                                    {
                                                        ind1++; //

                                                        try // try 6
                                                        {
                                                            int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 2)) * 1000));
                                                            Debug.WriteLine("Found dn split hz" + split_hz);
                                                            DX_Mode2[DX_Index1] = split_hz;
                                                        }
                                                        catch // catch 6
                                                        {

                                                          Debug.WriteLine("failed to find dn value================");
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
                                       Debug.WriteLine("Found dn split hz" + split_hz);
                                        DX_Mode2[DX_Index1] = split_hz;
                                    }
                                    catch // catch 1
                                    {

                                        try // try 2
                                        {
                                            int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 3)) * 1000));
                                            Debug.WriteLine("Found dn split hz" + split_hz);
                                            DX_Mode2[DX_Index1] = split_hz;
                                        }
                                        catch // catch 2
                                        {

                                            try // try 3
                                            {
                                                int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 2)) * 1000));
                                                Debug.WriteLine("Found dn split hz" + split_hz);
                                                DX_Mode2[DX_Index1] = split_hz;
                                            }
                                            catch // catch 3
                                            {

                                                int ind1 = DX_Message[DX_Index1].IndexOf("dwn") - 4; //

                                                try // try 4
                                                {
                                                    int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 4)) * 1000));
                                                  Debug.WriteLine("Found dn split hz" + split_hz);
                                                    DX_Mode2[DX_Index1] = split_hz;
                                                }
                                                catch // catch 4
                                                {
                                                    ind1++; //

                                                    try // try 5
                                                    {
                                                        int split_hz = (int)(-Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 3)) * 1000);
                                                      Debug.WriteLine("Found dn split hz" + split_hz);
                                                        DX_Mode2[DX_Index1] = split_hz;
                                                    }
                                                    catch // catch 5
                                                    {
                                                        ind1++; //

                                                        try // try 6
                                                        {
                                                            int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 2)) * 1000));
                                                           Debug.WriteLine("Found dn split hz" + split_hz);
                                                            DX_Mode2[DX_Index1] = split_hz;
                                                        }
                                                        catch // catch 6
                                                        {

                                                           Debug.WriteLine("failed to find dn value================");
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
                                        Debug.WriteLine("Found dn split hz" + split_hz);
                                        DX_Mode2[DX_Index1] = split_hz;
                                    }
                                    catch // catch 1
                                    {

                                        try // try 2
                                        {
                                            int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 3)) * 1000));
                                           Debug.WriteLine("Found dn split hz" + split_hz);
                                            DX_Mode2[DX_Index1] = split_hz;
                                        }
                                        catch // catch 2
                                        {

                                            try // try 3
                                            {
                                                int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 2)) * 1000));
                                               Debug.WriteLine("Found dn split hz" + split_hz);
                                                DX_Mode2[DX_Index1] = split_hz;
                                            }
                                            catch // catch 3
                                            {

                                                int ind1 = DX_Message[DX_Index1].IndexOf("down") - 4; //

                                                try // try 4
                                                {
                                                    int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 4)) * 1000));
                                                    Debug.WriteLine("Found dn split hz" + split_hz);
                                                    DX_Mode2[DX_Index1] = split_hz;
                                                }
                                                catch // catch 4
                                                {
                                                    ind1++; //

                                                    try // try 5
                                                    {
                                                        int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 3)) * 1000));
                                                       Debug.WriteLine("Found dn split hz" + split_hz);
                                                        DX_Mode2[DX_Index1] = split_hz;
                                                    }
                                                    catch // catch 5
                                                    {
                                                        ind1++; //

                                                        try // try 6
                                                        {
                                                            int split_hz = (int)(-Math.Abs(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind1, 2)) * 1000));
                                                           Debug.WriteLine("Found dn split hz" + split_hz);
                                                            DX_Mode2[DX_Index1] = split_hz;
                                                        }
                                                        catch // catch 6
                                                        {

                                                           Debug.WriteLine("failed to find dn value================");
                                                            DX_Mode2[DX_Index1] = -1000; // 1khz dn

                                                        } // catch6   (2 digits to left side)

                                                    } // catch5   (3 digits to left side)

                                                } // catch4   (4 digits to left side)

                                            } // catch3   (2 digits to right side)

                                        } //catch2  (3 digits to right side)

                                    } // catch 1   (4 digits to right side)

                                } // split down
                                else if ((DX_Message[DX_Index1].Contains("9+")) || (DX_Message[DX_Index1].Contains("59+") )) // check for split
                                {
                                    // ignore + if its part of s9+
                                }

                                else if (DX_Message[DX_Index1].Contains("+")) // check for split
                                {

                                    int ind = DX_Message[DX_Index1].IndexOf("+") + 1;

                                    try // try 1
                                    {
                                        int split_hz = (int)(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 4)) * 1000);
                                       Debug.WriteLine("Found UP split hz" + split_hz);
                                        DX_Mode2[DX_Index1] = split_hz;
                                    }
                                    catch // catch 1
                                    {

                                        try // try 2
                                        {
                                            int split_hz = (int)(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 3)) * 1000);
                                            Debug.WriteLine("Found UP split hz" + split_hz);
                                            DX_Mode2[DX_Index1] = split_hz;
                                        }
                                        catch // catch 2
                                        {

                                            try // try 3
                                            {
                                                int split_hz = (int)(Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 2)) * 1000);
                                              Debug.WriteLine("Found UP split hz" + split_hz);
                                                DX_Mode2[DX_Index1] = split_hz;
                                            }
                                            catch // catch 3
                                            {

                                                Debug.WriteLine("failed to find up value================");
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
                                        Debug.WriteLine("Found dn split hz" + split_hz);
                                        DX_Mode2[DX_Index1] = split_hz;
                                    }
                                    catch // catch 1
                                    {

                                        try // try 2
                                        {
                                            int split_hz = (int)(-Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 3)) * 1000);
                                           Debug.WriteLine("Found dn split hz" + split_hz);
                                            DX_Mode2[DX_Index1] = split_hz;
                                        }
                                        catch // catch 2
                                        {

                                            try // try 3
                                            {
                                                int split_hz = (int)(-Convert.ToDouble(DX_Message[DX_Index1].Substring(ind, 2)) * 1000);
                                               Debug.WriteLine("Found dn split hz" + split_hz);
                                                DX_Mode2[DX_Index1] = split_hz;
                                            }
                                            catch // catch 3
                                            {

                                                Debug.WriteLine("failed to find up value================");
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
                                       Debug.WriteLine("Found dn split hz" + split_hz);
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

                                          Debug.WriteLine("Found qrx split hz" + split_hz);

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

                                               Debug.WriteLine("Found dn split hz" + split_hz);

                                                DX_Mode2[DX_Index1] = split_hz;
                                                DX_Mode2[DX_Index1] = DX_Mode2[DX_Index1] - DX_Freq[DX_Index1];


                                            }
                                            catch // catch 3
                                            {

                                               Debug.WriteLine("failed to find up value================");
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

                                        Debug.WriteLine("Found qrz split hz" + split_hz);

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

                                          Debug.WriteLine("Found qrz split hz" + split_hz);

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

                                               Debug.WriteLine("Found dn split hz" + split_hz);
                                                DX_Mode2[DX_Index1] = split_hz;

                                                DX_Mode2[DX_Index1] = DX_Mode2[DX_Index1] - DX_Freq[DX_Index1];


                                            }
                                            catch // catch 3
                                            {

                                                Debug.WriteLine("failed to find up value================");
                                                DX_Mode2[DX_Index1] = 0; // 

                                            } // catch 3   (6 digits to right side)
                                          
                                        } // catch 2   (7 digits to right side)
                                      
                                    } // catch 1   (8 digits to right side)


                                } // split qrz




                            } // try to parse dx spot message above
                            catch (FormatException e)
                            {
                               Debug.WriteLine("mode issue" + e);

                            }
                            catch (ArgumentOutOfRangeException e)
                            {
                                Debug.WriteLine("mode1 issue" + e);

                            }
                          //  Debug.WriteLine("DX_Message " + DX_Message[DX_Index1]);

                            // grab time
                            try
                            {
                                //  DX_Time[DX_Index1] = Convert.ToInt16(message1.Substring(70, 4)); // get time from dx spot

                                DX_Time[DX_Index1] = UTCNEW; // use the my UTC because some spotters have issues and the spot has the wrong time in it.


                            }
                            catch (Exception)
                            {
                                DX_Time[DX_Index1] = UTCNEW;
                          
                            }
                           
                            //   Debug.WriteLine("DX_Time " + DX_Time[DX_Index1])
                           
                           
                      
                            // set age of spot to 0;
                            DX_Age[DX_Index1] = "00"; // reset to start




                            //=================================================================================================
                            //=================================================================================================

                            // CHECK HERE FOR (NA) NORTH AMERICAN,  OR EXCLUDE NORTH AMERICAN SPOTS

                            //=================================================================
                            //=================================================================
                            //=================================================================
                            // ke9ns DX SPOT FILTERS (EXCLUDE NA HERE)
                            if (chkBoxWrld.Checked) // filter out US calls signs
                            {

                                string us1 = DX_Spotter[DX_Index1].Substring(1, 1); // grab first char of Spotter callsign becuase I added a < > around the spotter callsign
                                string us2 = DX_Spotter[DX_Index1].Substring(2, 1); // grab second char of Spotter callsign

                             //   Debug.WriteLine("us1 " + us1 + " us2 " + us2);

                                Regex r = new Regex("[KNWAX]"); // first char (include X for Mexico in the NA spots)
                                Regex r1 = new Regex("[A-Z0-9]"); // 2nd char to select as a NA spot
                                Regex r2 = new Regex("[ABCDEFGYO]"); // 2nd char // for V as the first char for Canada
                                Regex r3 = new Regex("[YGFIJK]"); // 2nd char // for C as the first char


                                if ((us1 == "V") || (us1 == "C")) // check for Canada (NA)
                                {
                                    if ( ((us1 == "V") && (r2.IsMatch(us2)) ) || ((us1 == "C") && (r3.IsMatch(us2))))
                                    {
                                        Debug.WriteLine("bypass4a " + DX_Spotter[DX_Index1]);
                                        continue; // dont show spot if not on the r1 list
                                    }
                                    goto PASS2; // if the 1st letter is not a US letter then GOOD use SPOT

                                }
                                else
                                {
                                    if ((r.IsMatch(us1)))
                                    {
                                        Debug.WriteLine("bypass3 " + DX_Spotter[DX_Index1]);
                                        continue;// dont show spot if not on the r list
                                    }
                                 
                                   // Debug.WriteLine("============CHECK3, fist us1 letter good for not being NA " + DX_Spotter[DX_Index1]);
                                    goto PASS2; // if the 1st letter is not a US letter then GOOD use SPOT

                                }

                                if ((r1.IsMatch(us2)))
                                {
                                     Debug.WriteLine("bypass4 " + DX_Spotter[DX_Index1]);
                                    continue; // dont show spot if not on the r1 list
                                }


                            }
                            else if (chkBoxNA.Checked) // filter out call signs outside of NA
                            {

                                string us1 = DX_Spotter[DX_Index1].Substring(1, 1);// was 0,1 now 1,1 because I added <>
                                string us2 = DX_Spotter[DX_Index1].Substring(2, 1);// was 1,1

                                Debug.WriteLine("us1 " + us1 + " us2 " + us2);


                                Regex r = new Regex("[KNWAVX]"); // first char
                                Regex r1 = new Regex("[A-Z0-9]"); // 2nd char
                                Regex r2 = new Regex("[ABCDEFGYO]"); // 2nd char // for V as the first char
                                Regex r3 = new Regex("[YGFIJK]"); // 2nd char // for C as the first char

                                if (!(r.IsMatch(us1)))
                                {
                                     Debug.WriteLine("bypass1 " + DX_Spotter[DX_Index1]);
                                    continue;// dont show spot if not on the r list
                                }

                                if ((us1 == "V") || (us1 == "C"))
                                {
                                    if ( ((us1 == "V") && !(r2.IsMatch(us2)) ) || ((us1 == "C") && !(r3.IsMatch(us2))))
                                    {
                                        Debug.WriteLine("bypass2a " + DX_Spotter[DX_Index1]);
                                        continue; // dont show spot if not on the r1 list
                                    }
                                }
                                else
                                {
                                    if (!(r1.IsMatch(us2)))
                                    {
                                        Debug.WriteLine("bypass2 " + DX_Spotter[DX_Index1]);
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
                                    Debug.WriteLine("station dup============" + DX_Freq[ii] + " dup "+ DX_Freq[DX_Index1] + " dup " + DX_Station[DX_Index1] + " dup " + DX_Station[ii]);
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

                                    DX_country[ii] = DX_country[ii + 1];

                                    DX_X[ii] = DX_X[ii + 1];
                                    DX_Y[ii] = DX_Y[ii + 1];

                                    DX_Beam[ii] = DX_Beam[ii + 1];

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
                                    Debug.WriteLine("freq dup============");
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

                                    DX_country[ii] = DX_country[ii + 1];
                                    DX_X[ii] = DX_X[ii + 1];
                                    DX_Y[ii] = DX_Y[ii + 1];
                                    DX_Beam[ii] = DX_Beam[ii + 1];
                                }

                            } // for ii check for dup in list
                            DX_Index = (DX_Index - xx);  // readjust the length of the spot list after taking out the duplicates


                            //=================================================================================================
                            //=================================================================================================
                            // ke9ns  passed the spotter, dx station , freq, and time test
                           
                            DX_Index++; // jump to PASS2 if it passed the valid call spotter test

                            if (DX_Index > 90)
                            {
                                 Debug.WriteLine("DX SPOT REACH 90 ");
                                DX_Index = 90; // you have reached max spots
                            }

                            //   Debug.WriteLine("index "+ DX_Index);



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

                                DX_country[ii] = DX_country[ii - 1];
                                DX_X[ii] = DX_X[ii - 1];
                                DX_Y[ii] = DX_Y[ii - 1];
                                DX_Beam[ii] = DX_Beam[ii - 1];

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

                            //------------------------------------------------------------------------------------
                            //------------------------------------------------------------------------------------
                            //------------------------------------------------------------------------------------
                            // Crosscheck Station Call sign Prefix with data from DXLOC.txt (lat and lon) 
                            // and create a list of Country, Callsign, X, Y on unscaled map


                            updatemapspots();


                            //------------------------------------------------------------------------------------
                            //------------------------------------------------------------------------------------
                            //------------------------------------------------------------------------------------

                            Debug.WriteLine("INSTALL NEW [0]=========== " + DX_Index);


                            processTCPMessage(); // send to spot window
                            Debug.WriteLine("INSTALL NEW [1]=========== " + DX_Index);


                            SP4_Active = 0; // done processing message

                          
                            //      Debug.WriteLine("Aindex " + DX_Index);

                        } // (message1.StartsWith("DX de ") valid message


                        else if (message1.Contains(" disconnected"))
                        {
                            textBox1.Text += "Your Socket was disconnected \r\n";

                            statusBox.ForeColor = Color.Red;
                            console.spotterMenu.ForeColor = Color.Red;

                            console.spotterMenu.Text = "Closed12345";
                            statusBox.Text = "Closed";


                            SP_writer.Close();                  // close down now
                            SP_reader.Close();
                            networkStream.Close();

                            client.Close();
                            //   Debug.WriteLine("END DX SPOT thread");

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

                //    Debug.WriteLine("END DX SPOT thread");


                statusBox.ForeColor = Color.Red;
                console.spotterMenu.ForeColor = Color.Red;

                console.spotterMenu.Text = "Closing";
                statusBox.Text = "Closing";

                textBox1.Text += "Asked to Close \r\n";


                SP_writer.Close();                  // close down now
                SP_reader.Close();
                networkStream.Close();
              

                client.Close();
                //   Debug.WriteLine("END DX SPOT thread");

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

                Debug.WriteLine("cannot open socket or socket closed on me" + SE);
                statusBox.ForeColor = Color.Red;
                console.spotterMenu.ForeColor = Color.Red;

                statusBox.Text = "Socket";
                console.spotterMenu.Text = "Socket";

                try
                {
                    SP_writer.Close();
                    SP_reader.Close();
                }
                catch(Exception)
                {
                  

                }

                try
                {
                    networkStream.Close();
                    client.Close();
                }
                catch(Exception)
                {

                }

               
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

                Debug.WriteLine("socket exception issue" + e1);

                 statusBox.ForeColor = Color.Red;
                console.spotterMenu.ForeColor = Color.Red;

                statusBox.Text = "Socket";
                console.spotterMenu.Text = "Socket";

                try
                {
                    SP_writer.Close();
                    SP_reader.Close();
                }
                catch (Exception)
                {


                }

                try
                {
                    networkStream.Close();
                    client.Close();
                }
                catch (Exception)
                {

                }

                SP2_Active = 0;

                statusBox.Text = "Closed";
                console.spotterMenu.Text = "Spotter";

           
                return;
            }


        } // SPOTTER Thread



        //===================================================================================
        // ke9ns add update dx call and country on map
        private void updatemapspots()
        {


            //------------------------------------------------------------------------------------
            //------------------------------------------------------------------------------------
            //------------------------------------------------------------------------------------
            //------------------------------------------------------------------------------------
            // Crosscheck Station Call sign Prefix with data from DXLOC.txt (lat and lon) 
            // and create a list of Country, Callsign, X, Y on unscaled map

            if ((SP8_Active == 1) && (SP_Active > 2)  && (DX_Index > 0 )) // do if dxloc.txt file loaded in        && (SP5_Active == 1 )
            {

                int Sun_WidthY1 = Sun_Bot1 - Sun_Top1;             // # of Y pixels from top to bottom of map

                int Sun_Width = Sun_Right - Sun_Left;              //used by sun track routine

                Debug.WriteLine("MAPPING======");

                DX_Y[0] = 0;
                DX_X[0] = 0;
                DX_country[0] = null;
                DX_Beam[0] = 0; 

                int kk = 0;

                for (; kk < DXLOC_Index1; kk++)  // list of call sign prefixes and there corresponding LAT/LON  DXLOC_Index1 is from when file was read into memory
                {
                    if (DX_Station[0].StartsWith(DXLOC_prefix[kk]) == true) // look for a dx spot callsign prefix to make a match with the dxloc.txt list
                    {
                        if (DXLOC_prefix1[kk] != null)
                        {

                            if (DX_Station[0].Contains(DXLOC_prefix1[kk]) == false) continue; // dont choose if not a match

                        }

                        DX_Y[0] = (int)(((180 - (DXLOC_LAT[kk] + 90)) / 180.0) * Sun_WidthY1) + Sun_Top1;  //latitude 90N to -90S

                        DX_X[0] = (int)(((DXLOC_LON[kk] + 180.0) / 360.0) * Sun_Width) + Sun_Left;  // longitude -180W to +180E

                        DX_country[0] = DXLOC_country[kk]; // save country into dx spotter list (pulled from dxloc list)

                        DX_Beam[0] = BeamHeading(DXLOC_LAT[kk], DXLOC_LON[kk]);

                        Debug.WriteLine("MAPPER " + DX_Station[0] + " " + DX_X[0] + " " + DX_Y[0] + " cntry " + DX_country[0] + " prefix " + DXLOC_prefix[kk] + " lat " + DXLOC_LAT[kk] + " lon " + DXLOC_LON[kk] + " BEAM "+DX_Beam[0]);

                        break; // got a match so break

                    }

                } // for kk loop for DXLOC in memory

                if (kk == DXLOC_Index1) // no match found
                {
                    DX_country[0] = " -- "; // dont have a match so need to add to list

                    DX_Beam[0] = 0;
                    Debug.WriteLine("MAPPER NO MACH FOR Station" + DX_Station[0]);

                }

            } // sp8_active = 1
            else
            {
                DX_country[0] = " -- "; // dont have a match so need to add to list
                DX_Beam[0] = 0;
                //  Debug.WriteLine("mapper OFF");

            }


        } //  updatemapspots()








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

                //----------------------------------------------------------
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

              // old   bigmessage += (DX_FULLSTRING[ii]+ DXmode+ " " + (DX_country[ii].PadRight(8)).Substring(0,8) + " :"+ DX_Age[ii]  + "\r\n" );
                bigmessage += (DX_FULLSTRING[ii] + DXmode + " " + (DX_country[ii].PadRight(8)).Substring(0, 8) + ": "+ DX_Beam[ii].ToString().PadLeft(3) + "° :" + DX_Age[ii] + "\r\n"); // adds 6

            } // for loop to update dx spot window

            if ((pause == false) && (beacon1 == false) && (WTime == false))  textBox1.Text = bigmessage; // update screen


        } //processTCPMessage



        //===================================================================================
        // ke9ns add process message for spot.cs window by right clicking
        private void processTCPMessage1()
        {

            string bigmessage = null;

            for (int ii = 0; ii < BX1_Index; ii++)
            {

                if (BX_Age[ii] == null) BX_Age[ii] = "00";
                else if (DX_Age[ii] == "  ") BX_Age[ii] = "00";

                if (BX_Age[ii].Length == 1) BX_Age[ii] = "0" + BX_Age[ii];

                //----------------------------------------------------------
                string DXmode = "    "; // 5 spaces

                if (BX_Mode[ii] == 0) DXmode = " ssb ";
                else if (BX_Mode[ii] == 1) DXmode = " cw  ";
                else if (BX_Mode[ii] == 2) DXmode = " rtty";
                else if (BX_Mode[ii] == 3) DXmode = " psk ";
                else if (BX_Mode[ii] == 4) DXmode = " oliv";
                else if (BX_Mode[ii] == 5) DXmode = " jt65";
                else if (BX_Mode[ii] == 6) DXmode = " cont";
                else if (BX_Mode[ii] == 7) DXmode = " fsk ";
                else if (BX_Mode[ii] == 8) DXmode = " mt63";
                else if (BX_Mode[ii] == 9) DXmode = " domi";
                else if (BX_Mode[ii] == 10) DXmode = " pack";
                else if (BX_Mode[ii] == 11) DXmode = " fm  ";
                else if (BX_Mode[ii] == 12) DXmode = " drm ";
                else if (BX_Mode[ii] == 13) DXmode = " sstv";
                else if (BX_Mode[ii] == 14) DXmode = " am  ";

                else DXmode = "     ";

                bigmessage += (BX_FULLSTRING[ii] + DXmode + " " + (BX_country[ii].PadRight(8)).Substring(0, 8) + ": " + BX_Beam[ii].ToString().PadLeft(3) + "° :" + BX_Age[ii] + "\r\n"); // adds 6

            } // for loop to update dx spot window

             textBox1.Text = bigmessage; // update screen


        } //processTCPMessage1 beacon

        //===================================================================================
        // ke9ns add process age of dx spot

        private void processDXAGE()
        {

            
            UTCD = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

          
            FD = UTCD.ToString("HHmm");                                       // get 24hr 4 digit UTC NOW
            UTCNEW = Convert.ToInt16(FD);                                    // convert 24hr UTC to int



            int hh = Convert.ToInt16(UTCD.ToString("HH"));
            int mm = Convert.ToInt16(UTCD.ToString("mm"));

            int UTCNEWMIN = mm + (60 * hh);
         //   Debug.WriteLine("time in minutes only " + UTCNEWMIN);



            if ((UTCLAST == 0) && (UTCNEW != 0))  // for startup only (ie. run 1 time)
            {
                UTCLAST = UTCNEW;        
                DX_Time[0] = UTCNEW;
                UTCLASTMIN = UTCNEWMIN; 

            }

            if ((DX_Index > 0) && (UTCNEW != UTCLAST))
            {
                int xxx = 0;

                UTCLAST = UTCNEW;

                int UTCDIFFMIN = UTCNEWMIN - UTCLASTMIN;  // difference in minutes from last time we checked the dx spots for age

                Debug.WriteLine("Time to Check DX Spot Age =========== " + DX_Index + " UTCNEWMIN " + UTCNEWMIN + " UTCLASTMIN " + UTCLASTMIN + " UTCDIFFMIN " + UTCDIFFMIN);

                UTCLASTMIN = UTCNEWMIN; // save for next go around

                if (UTCDIFFMIN < 0) // this indicates you crossed the timeline to the next day
                {
                    UTCDIFFMIN = 1440 - UTCDIFFMIN; // make positive again
                }

                for (int ii = DX_Index - 1; ii >= 0; ii--) // move from bottom of list up toward top of list
                {

                    //  int UTCDIFF = Math.Abs(UTCNEW - DX_Time[ii]); // time difference 
                    //DX_Age[ii] = UTCDIFF.ToString("00"); // 2 digits
                   

                   int  UTCAGE = Convert.ToInt32(DX_Age[ii]) + UTCDIFFMIN; // current age difference for DX spots

                    DX_Age[ii] = UTCAGE.ToString(); // age your DX spot

                                       
                    int kk = 0; // look at very bottom of list + 1

                    
                    if (UTCAGE > 35) // if its an old SPOT then remove it from the list
                    {

                        Flag8 = 1; // signal that the DX_Index will change due to an old spot being removed

                        kk = ii; // 

                        xxx++; //shorten dx_Index by 1

                        Debug.WriteLine("time expire, remove=========spot " + DX_Time[ii] + " current time " + UTCLAST + " UTCDIFFMIN " + UTCDIFFMIN + " ii " + ii + " station " + DX_Station[ii]);
                     //   Debug.WriteLine("KK " + kk);
                     //   Debug.WriteLine("XXX " + xxx);


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

             //   Debug.WriteLine("END=========== " + DX_Index);

             //   Debug.WriteLine(" ");

                processTCPMessage(); // update spot window (remove old spots)

                return;

            } // UTC NEW != LAST
            else
            {
                return; // skip
            }

        } // processDXAGE


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
             nodeB = nodeBox1.Text;
             portB = portBox2.Text;
             nameB = nameBox.Text;
        }

        private void callBox_Leave(object sender, EventArgs e)
        {

            callB = callBox.Text;
            nodeB = nodeBox1.Text;
            portB = portBox2.Text;
            nameB = nameBox.Text;
        }

        private void nodeBox_Leave(object sender, EventArgs e)
        {

            callB = callBox.Text;
            nodeB = nodeBox1.Text;
            portB = portBox2.Text;
            nameB = nameBox.Text;
        }

        private void portBox_Leave(object sender, EventArgs e)
        {

            callB = callBox.Text;
            nodeB = nodeBox1.Text;
            portB = portBox2.Text;
            nameB = nameBox.Text;
        }

   
        private void chkBoxNA_CheckedChanged(object sender, EventArgs e)
        {
      
                if (chkBoxNA.Checked == true)
                {

                    //   Debug.WriteLine("US SPOT CHECKED");
                    chkBoxWrld.Checked = false;

                }
                else
                {
                    //   Debug.WriteLine("US SPOT UN-CHECKED");
                }
           
        }

        private void chkBoxWrld_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxWrld.Checked == true)
            {
                chkBoxNA.Checked = false;
                //   Debug.WriteLine("world SPOT CHECKED");
            }
            else
            {
                //   Debug.WriteLine("world SPOT UN-CHECKED");
            }
        }
       


        private void statusBox_Click(object sender, EventArgs e)
        {

            if ((SP_Active == 3 )) // if DX cluster active then test it by sending a CR
            {

                try
                {
                    statusBox.ForeColor = Color.Red;

                    statusBox.Text = "Test Sent <CR>";

                    SP_writer.Write((char)13);
                    SP_writer.Write((char)10);
                }
                catch(Exception)
                {
                    statusBox.Text = "Failed Test";

                }
     
            } // if connection was supposed to be active

        } // statusBox_Click


        public int LineLength = 105; // was 99

        //===============================================================================
        public bool beam_selected = false; // ke9ns if you clicked on the beam angle

        private void textBox1_MouseUp(object sender, MouseEventArgs e)
        {
            textBox1.ShortcutsEnabled = false;

          
            chkDXMode.Checked = true;

            if (e.Button == MouseButtons.Left)
            {

                int ii = textBox1.SelectionStart; // character position in the text you clicked on 

                byte iii = (byte)(ii / LineLength); // get line  /82  or /86 if AGE turned on or 91 if mode is also on /99 if country added but now /105 with DX_Beam heading


                int gg = ii % LineLength;  // get remainder for checking beam heading

             //   Debug.WriteLine("position in line" + gg);

                if (gg > (LineLength - 10)) beam_selected = true; // did user Left click over the beam heading on the dx spot list ?
                else beam_selected = false;



                 
                if ((DX_Index > iii) && (beacon1 == false))
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

                            Debug.WriteLine("split here" + (freq1 + DX_Mode2[iii]));

                        }
                        else
                        {
                            console.chkVFOSplit.Checked = false; // turn off split

                        }


                    } // dxmode checked

                    if (beam_selected == true)    // ke9ns add send hygain rotor command to DDUtil via the CAT port setup in PowerSDR
                    {
                        Debug.WriteLine("BEAM HEADING TRANSMIT");

                        console.spotDDUtil_Rotor = "AP1" + DX_Beam[iii].ToString().PadLeft(3, '0') + ";";
                        console.spotDDUtil_Rotor = ";";
                        console.spotDDUtil_Rotor = "AM1;";

                   } //  if (chkBoxRotor.Checked == true)
                    button1.Focus();

                    if (beacon1 == false) Map_Last = 2; // redraw map spots
                    else beacon4 = true; // redraw beacon map spots

                } // make sure index you clicked on is within range

                else  if ((BX1_Index > iii) && (beacon1 == true))
                {
                    int freq1 = BX_Freq[iii];

                    if ((freq1 < 5000000) || ((freq1 > 6000000) && (freq1 < 8000000))) // check for bands using LSB
                    {
                        if (chkDXMode.Checked == true)
                        {
                            if (BX_Mode[iii] == 0) console.RX1DSPMode = DSPMode.LSB;
                            else if (BX_Mode[iii] == 1) console.RX1DSPMode = DSPMode.CWL;
                            else if (BX_Mode[iii] == 2) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 3) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 4) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 5) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 6) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 7) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 8) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 9) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 10) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 11) console.RX1DSPMode = DSPMode.FM;
                            else if (BX_Mode[iii] == 12) console.RX1DSPMode = DSPMode.LSB;
                            else if (BX_Mode[iii] == 13) console.RX1DSPMode = DSPMode.DIGL;
                            else if (BX_Mode[iii] == 14) console.RX1DSPMode = DSPMode.SAM;
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

                            if (BX_Mode[iii] == 0) console.RX1DSPMode = DSPMode.USB;
                            else if (BX_Mode[iii] == 1) console.RX1DSPMode = DSPMode.CWU;
                            else if (BX_Mode[iii] == 2) console.RX1DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 3) console.RX1DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 4) console.RX1DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 5) console.RX1DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 6) console.RX1DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 7) console.RX1DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 8) console.RX1DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 9) console.RX1DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 10) console.RX1DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 11) console.RX1DSPMode = DSPMode.FM;
                            else if (BX_Mode[iii] == 12) console.RX1DSPMode = DSPMode.USB;
                            else if (BX_Mode[iii] == 13) console.RX1DSPMode = DSPMode.DIGU;
                            else if (BX_Mode[iii] == 14) console.RX1DSPMode = DSPMode.SAM;
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

                        if (BX_Mode2[iii] != 0)
                        {

                            console.VFOBFreq = (double)(freq1 + BX_Mode2[iii]) / 1000000; // convert to MHZ
                            console.chkVFOSplit.Checked = true; // turn on  split

                            Debug.WriteLine("split here" + (freq1 + BX_Mode2[iii]));

                        }
                        else
                        {
                            console.chkVFOSplit.Checked = false; // turn off split

                        }


                    } // dxmode checked

                    if (beam_selected == true)    // ke9ns add send hygain rotor command to DDUtil via the CAT port setup in PowerSDR
                    {
                        Debug.WriteLine("BEAM HEADING TRANSMIT");

                        console.spotDDUtil_Rotor = "AP1" + BX_Beam[iii].ToString().PadLeft(3, '0') + ";";
                        console.spotDDUtil_Rotor = ";";
                        console.spotDDUtil_Rotor = "AM1;";

                    } //  if (chkBoxRotor.Checked == true)
                    button1.Focus();

                    if (beacon1 == false) Map_Last = 2; // redraw map spots
                    else beacon4 = true; // redraw beacon map spots

                } // make sure index you clicked on is within range (BEACON)






            } // left mouse button
            else if (e.Button == MouseButtons.Right)
            {

                if ((SP4_Active == 0) && (beacon1 == false))// only process lookup if not processing a new spot which might cause issue
                {
                    int ii = textBox1.GetCharIndexFromPosition(e.Location);

                    byte iii = (byte)(ii / LineLength);  // get line  /82  or /86 if AGE turned on or 91 if mode is also on /99 if country added
                    
                    if (DX_Index > iii)
                    {
                        string DXName = DX_Station[iii];

                      //  Debug.WriteLine("Line " + iii + " Name " + DXName);

                        try
                        {
                            System.Diagnostics.Process.Start("https://www.qrz.com/db/" + DXName);   // System.Diagnostics.Process.Start("http://www.microsoft.com");
                        }
                        catch
                        {
                       //     Debug.WriteLine("bad station");
                            // if not a URL then ignore
                        }
                    }
                   
                } // not actively processing a new spot
               else  if ((SP4_Active == 0) && (beacon1 == true))// only process lookup if not processing a new spot which might cause issue
                {
                    int ii = textBox1.GetCharIndexFromPosition(e.Location);

                    byte iii = (byte)(ii / LineLength);  // get line  /82  or /86 if AGE turned on or 91 if mode is also on /99 if country added

                    if (BX1_Index > iii)
                    {
                        string DXName = BX_Station[iii];

                        //  Debug.WriteLine("Line " + iii + " Name " + DXName);

                        try
                        {
                            System.Diagnostics.Process.Start("https://www.qrz.com/db/" + DXName);   // System.Diagnostics.Process.Start("http://www.microsoft.com");
                        }
                        catch
                        {
                            //     Debug.WriteLine("bad station");
                            // if not a URL then ignore
                        }
                    }

                } // not actively processing a new spot






            }

        } // textBox1_MouseUp



        //=========================================================================================
        private void chkAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {

            this.TopMost = chkAlwaysOnTop.Checked;
        }


        //=========================================================================================
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




        public static Image Skin1 = null; // temp holder for orignal skin image in picdisplay

        //=========================================================================================
        private void chkSUN_CheckedChanged(object sender, EventArgs e)
        {

            if ((chkSUN.Checked == false) && (chkGrayLine.Checked == false))
            {
                if (Skin1 != null) console.picDisplay.BackgroundImage = Skin1; // put back original image
            }
            if (SP_Active != 0 ) 
            {
             
                if ((chkSUN.Checked == true) || (chkGrayLine.Checked == true))
                {

                    if (Skin1 == null) Skin1 = console.picDisplay.BackgroundImage;
                    console.picDisplay.SizeMode = PictureBoxSizeMode.StretchImage;

                    if (MAP == null)
                    {
                        if (Console.DXR == 0)
                        console.picDisplay.BackgroundImage = Image.FromStream(Map_image);
                        else console.picDisplay.BackgroundImage = Image.FromStream(Map_image2);

                    }
                    else console.picDisplay.BackgroundImage = MAP;
 
                } // SUN or GRAY LINE checked

            } // dx spot on

            Map_Last = 1;

        } // chkSUN_CheckedChanged



        //=========================================================================================
        private void chkGrayLine_CheckedChanged(object sender, EventArgs e)
        {
            if ((chkSUN.Checked == false) && (chkGrayLine.Checked == false))
            {
                if (Skin1 != null) console.picDisplay.BackgroundImage = Skin1; // put back original image
            }

            if ( SP_Active != 0) 
            {
              
                if ((chkSUN.Checked == true) || (chkGrayLine.Checked == true))
                {
                    if (Skin1 == null) Skin1 = console.picDisplay.BackgroundImage;
                    console.picDisplay.SizeMode = PictureBoxSizeMode.StretchImage;

                    if (MAP == null)
                    {
                        if (Console.DXR == 0)  console.picDisplay.BackgroundImage = Image.FromStream(Map_image);
                        else console.picDisplay.BackgroundImage = Image.FromStream(Map_image2);

                      //  console.picDisplay.BackgroundImage = Image.FromStream(Map_image);
                    }
                    else console.picDisplay.BackgroundImage = MAP;
      
                } // only do if SUN or GRAY LINE checked

            } // dx spot active

            Map_Last = 1;

        } // chkGrayLine_CheckedChanged





        private static DisplayMode LastDisplayMode = 0;

        //===============================================================================================================================
        //===============================================================================================================================
        //===============================================================================================================================
        //===============================================================================================================================
        // turn on/off tracking (sun and/or grayline)
        private void btnTrack_Click(object sender, EventArgs e)
        {

            if (SP5_Active == 0)  // if OFF then turn ON
            {

                if (chkPanMode.Checked == true) Display.map = 1;
                else Display.map = 0;

                btnTrack.Text = "Track ON";

                LastDisplayMode = Display.CurrentDisplayMode; // save the display mode that you were in before you turned on special panafall mode

                if (chkPanMode.Checked == true)  Display.CurrentDisplayMode = DisplayMode.PANAFALL;


                Display.GridOff = 1; // Force Gridlines off but dont change setupform setting
                
                if ((chkSUN.Checked == true) || (chkGrayLine.Checked == true))
                {

                    if (Skin1 == null) Skin1 = console.picDisplay.BackgroundImage;
                    console.picDisplay.SizeMode = PictureBoxSizeMode.StretchImage;

                    if (MAP == null)
                    {
                        if (Console.DXR == 0)   console.picDisplay.BackgroundImage = Image.FromStream(Map_image);
                        else console.picDisplay.BackgroundImage = Image.FromStream(Map_image2);
                        //  console.picDisplay.BackgroundImage = Image.FromStream(Map_image);
                    }
                    else console.picDisplay.BackgroundImage = MAP;

                }


                Thread t = new Thread(new ThreadStart(TrackSun));

                t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");


                SP5_Active = 1;
                t.Name = "Track Thread";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();

                textBox1.Text = "Clicked to Turn on GrayLine Sun Tracker\r\n";

            }
            else // map was already on so turn off
            {

                SP5_Active = 0;                     // turn off tracking

                Display.map = 0;                    // tell display program to got back to standard panafall mode

                if (chkPanMode.Checked == true) Display.CurrentDisplayMode = LastDisplayMode;
               
                if (console.setupForm.gridBoxTS.Checked == true) Display.GridOff = 1; // put gridlines back the way they were
                else Display.GridOff = 0; // gridlines ON
            
                btnTrack.Text = "Track";
     
                textBox1.Text += "Click to turn off GrayLine Sun Tracking\r\n";
             
                if (Skin1 != null) console.picDisplay.BackgroundImage = Skin1; // put back original image

            } // turn Tracking  off


        } //  btnTrack_Click





        public static int UTCLAST2 = 0;                                        // for Sun Tracker

        public static double UTC100A = 0;                                       // UTC time in hours.100th of min for grayline

        public static double UTC100 = 0;                                       // UTC time as a factor of 1 (1=2400 UTC, 0= 0000UTC) 
        public static double UTCDAY = 0;                                       // UTC day as a factor of 1 (0=1day, 1=365)
        public static int[] Gray_X = new int[400];                             // for mapping the gray line x and y 
        public static int[] Gray_Y = new int[400];

        public static int Sun_Right = 0;
        public static int Sun_Left = 0;

        public static int[,] GrayLine_Pos = new int[1000, 3];                      // [0,]=is lat 180 to 0 to -180 (top to bottom) dark
        public static int[,] GrayLine_Pos1 = new int[1000, 3];                      // [0,]=is lat 180 to 0 to -180 (top to bottom) dusk
        public static int[] GrayLine_Pos2 = new int[1000];
        public static int[] GrayLine_Pos3 = new int[1000];

        public static int zz = 0; // determine the y pixel for this latitude grayline


        public static int Sun_X = 0;  // position of SUN in picDisplay window (based on ke9ns world map skin)
        public static int Sun_Y = 0;  // position of SUN in picDisplay window (based on ke9ns world map skin) 
        public static int Sun_Top1 = 0;  // position of SUN in picDisplay window (based on ke9ns world map skin)
        public static int Sun_Bot1 = 0;  // position of SUN in picDisplay window (based on ke9ns world map skin) 

        public static bool SUN = false; // true = on
        public static bool GRAYLINE = false; // true = on

        public static int suncounter = 0; // for space weather
        public static int SFI = 0;       // for Space weather
        public static int SN = 0;        // for Space weather
        public static int Aindex = 0;    // for Space weather
        public static int Kindex = 0;    // for Space weather
        public static string RadioBlackout = " ";
        public static string GeoBlackout = " ";
        private string serverPath;       // for Space weather



        //====================================================================================================
        //====================================================================================================
        // ke9ns add compute angle to sun (for dusk grayline) // equations in this section below provided by Roland Leigh
        private int SUNANGLE(double lat, double lon)  // ke9ns Thread opeation (runs in en-us culture) opens internet connection to genearte list of dx spots
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



        public static Color GrayLine_Last = Color.FromArgb(70, Color.Black);                       // used to check if setup.cs changed the color
        public static Band RX1Band_Last = 0;                                                      // to track a change in RX1 band

        private static Font font1 = new Font("Ariel", 10.5f, FontStyle.Regular, GraphicsUnit.Pixel);  // ke9ns add dx spot call sign font style
        private static Font font2 = new Font("Ariel", 9.0f, FontStyle.Regular, GraphicsUnit.Pixel);  // ke9ns add dx spot call sign font style

        private static Color grid_text_color = Color.Yellow;
        SolidBrush grid_text_brush = new SolidBrush(grid_text_color);

        private static Color Beacon_color = Color.Violet;
        SolidBrush Beacon_brush = new SolidBrush(Beacon_color);       // color when scanning a beacon station

        SolidBrush greenbrush = new SolidBrush(Color.Green);     // beacon signal strength color STRONG
        SolidBrush orangebrush = new SolidBrush(Color.Orange);        // beacon signal strength color not checked yet
        SolidBrush yellowbrush = new SolidBrush(Color.Yellow);   // beacon signal strength color LIGHT
        SolidBrush redbrush = new SolidBrush(Color.Red);              // normal red dot color on map  (beacon signal strength color no signal)

        SolidBrush graybrush = new SolidBrush(Color.DarkGray);        // beacon signal strength color not checked yet
        SolidBrush bluebrush = new SolidBrush(Color.LightBlue);       // beacon signal strength color not checked yet



        public static Image MAP = null; // holds bitmap image for SUN and GRAY LINE

        private static int[] spots = new int[100];  // holder for all the spots current on your entire band.

        public static int VFOLOW = 0;   // set in console rx1band for use in the mapper
        public static int VFOHIGH = 0;

        string[] country = new string[200];
        string[] call = new string[200];

        int[] yy = new int[200];  // increments the y axis down to allow multiple station names under a red dot



        //===============================================================================================================================
        //===============================================================================================================================
        //===============================================================================================================================
        //===============================================================================================================================
        private void TrackSun()  // ke9ns Thread opeation (runs in en-us culture) To create and draw Sun and/or Grayline Track
        {
           

            //-------------------------------------------------------------------
            // ke9ns grayline check 
            // horizontal lines top to bottom (North)90 to 0 to (-SOUTH)90
            // vertical lines left to right  -West(180) to 0 to +East(180)
            // Solstice June and Dec  (reach northern or southern extreme (about +/-24deg)
            // Equinox: March and Sept  (pass the equator)
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

            console.setupForm.clrbtnGrayLine_Changed(this, EventArgs.Empty);               // get color from setup at startup

          
            if (Console.noaaON == 0) // only do this if space weather is OFF on the main console window
            {
                textBox1.Text += "Attempt login to:  NOAA Space Weather Prediction Center \r\n";

                NOAA(); // get noaa space data

            }

         //   textBox1.Text += "NOAA Download complete \r\n";


            //--------------------------------------------------------------------------------------------
            // stay in this thread loop until you turn off tracking
            for (; SP5_Active == 1;)
            {

                Thread.Sleep(50);

                if (SP5_Active == 0) continue;

                if (  ((beacon4 == true)) ||((chkSUN.Checked == true) || (chkGrayLine.Checked == true)) &&
                       ((Display.CurrentDisplayMode == DisplayMode.PANADAPTER) || (Display.CurrentDisplayMode == DisplayMode.PANAFALL))
                   )
                {

                    UTCD = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                    FD = UTCD.ToString("HHmm");                                       // get 24hr 4 digit UTC NOW
                    UTCNEW = Convert.ToInt16(FD);                                    // convert 24hr UTC to int


                    if (chkSUN.Checked == true) SUN = true; // activate display
                    else SUN = false;

                    if (chkGrayLine.Checked == true) GRAYLINE = true; // activate display
                    else GRAYLINE = false;


                    // Do a SUN, GRAYLINE, or DX country/callsign update

                    // Check for TIME CHANGE
                    // Check for GrayLine COLOR change
                    // Check for DX spot list change
                    // Check in any checkboxes changed state
                    // Check if the number of spots on map changed (DXK is the # of spots on the current panadapter)
                    // Check for Transmitting (dont update if transmitting)

                    if ((!console.MOX) && ((UTCNEW != UTCLAST2) || (Setup.DisplayGrayLineColor != GrayLine_Last) || (Map_Last > 0) ||
                        (((DX_Index != DX_Last) || (Console.DXK != DXK_Last) || (console.RX1Band != RX1Band_Last)) && (SP8_Active == 1)))
                        || ((beacon4 == true))      )  // Beacon scanner
                    {

                      
                        Debug.WriteLine("Update DX Spots on Screen=================");

                        GrayLine_Last = Setup.DisplayGrayLineColor; // store last color for compare next time

                        DXK_Last = Console.DXK;

                        DX_Last = DX_Index;                    // if the DX spot list changed
                      
                        RX1Band_Last = console.RX1Band;

                        if ( (UTCNEW != UTCLAST2) || (Setup.DisplayGrayLineColor != GrayLine_Last) || (Map_Last == 1))
                        {
                            Debug.WriteLine("Update GrayLine=================");

                            UTCLAST2 = UTCNEW;                            // store time for compare next time

                            //=================================================================================================
                            //=================================================================================================
                            // ke9ns Position of SUN (viewed from SUN) using the ke9ns world skin 
                            // X  left edge starts 5.6% in, right edge ends 94.7%  (with Display.DIS_X at 100%)
                            // y  top edge starts 7.8% in, bottom edge ends 90.1%  (with Display.DIS_Y at 100%)
                            // equirectangle project map has longitude lines every 15deg (1 per hour) center is 0 UTC and sun moves to the left
                            // left edge is 2359 UTC (right edge is 0 UTC)

                            //  Debug.WriteLine("Mouse x " + Console.DX_X); // mouse on picDisplay coordinates
                            //  Debug.WriteLine("mouse y " + Console.DX_Y);

                            //   Debug.WriteLine("x pos " + Display.DIS_X); // 
                            //  Debug.WriteLine("y pos " + Display.DIS_Y);

                            double g1 = (double)(360 / 365.25 * (DateTime.UtcNow.DayOfYear + (DateTime.UtcNow.Hour / 24))); // convert days to angle

                            g1 = g1 * Math.PI / 180;                             // convert angle to rads

                            double D = (double)(0.396372 - (22.91327 * Math.Cos(g1)) + (4.02543 * Math.Sin(g1)) - (0.387205 * Math.Cos(2 * g1))
                                + (0.051967 * Math.Sin(2 * g1)) - (0.154527 * Math.Cos(3 * g1)) + (0.084798 * Math.Sin(3 * g1)));

                            D = D / 24;                                        // convert to percent of 100

                            Sun_Top1 = 26;                                     // 26  45 Y pixel location of top of map
                            Sun_Bot1 = 465;                                    // 465  485 Y pixel locaiton of bottom of map 

                            int Sun_WidthY1 = Sun_Bot1 - Sun_Top1;             // # of Y pixels from top to bottom of map


                            int Sun_Top = 187;                                 // 207 y pixel location North Hem summer solstice
                            int Sun_Bot = 303;                                 // 324 Y pixel location of North Hem winter solstice

                            int Sun_WidthY = Sun_Bot - Sun_Top;                // # of Y pixes width between solstices
                            int Sun_WidthHalf = (Sun_WidthY / 2) + Sun_Top;    // y= 265 put you at equator

                            int Sun_Diff = Sun_WidthHalf - Sun_Top;            // 

                            Sun_Y = (int)(Sun_WidthHalf - (double)(Sun_Diff * D)); // position of SUN on longitude of map (based of time of year)

                            UTCD = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                            FD = UTCD.ToString("HH");
                            UTC100 = Convert.ToInt16(FD);
                            FD = UTCD.ToString("mm");

                            UTC100A = (UTC100 + (Convert.ToInt16(FD) / 60F));  // used by SUNANGLE routine

                            UTC100 = (UTC100 + (Convert.ToInt16(FD) / 60F)) / 24F; // used by SUN track routine convert to 100% 2400 = 100%

                            Sun_Left = 57;                                       // Left side at equator used by Grayline routine
                            Sun_Right = 939;

                            int Sun_Width = Sun_Right - Sun_Left; //used by sun track routine

                            Sun_X = (int)(Sun_Left + (float)(Sun_Width * (1.0 - UTC100))); // position of SUN on equator based on time of day

                            if ((GRAYLINE == true))
                            {

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

                                for (double lat = 90.0; lat >= -90.0; lat = lat - 0.5)  // horizontal lines top to bottom (North)90 to 0 to -90 (South)
                                {

                                    if ((SUNANGLE(lat, -180.0) >= 96) && (SUNANGLE(lat, 180.0) >= 96)) tt = 1; // dark on edges of screen 
                                    else tt = 0; // light on at least 1 side

                                    zz = (int)((qq / 360.0 * Sun_WidthY1) + Sun_Top1); // 360 = number of latitude points, determine the y pixel for this latitude grayline

                                    GrayLine_Pos[zz, 0] = GrayLine_Pos[zz, 1] = 0;

                                    if (SUNANGLE(lat, 0.0) < 96) check_for_light = false; // your in light so check for dark
                                    else check_for_light = true; // >= 96 your in dark so check for light


                                    for (double lon = 0.0; lon <= 180.0; lon = lon + 0.5)
                                    {
                                        tempsun_ang = SUNANGLE(lat, lon); // pos angle from 0 to 120


                                        if (check_for_light == true) // in dark, looking for light
                                        {

                                            if (tempsun_ang < 96) // found light
                                            {

                                                GrayLine_Pos[zz, ww] = (int)(((lon + 180.0) / 360.0 * Sun_Width) + Sun_Left); // determine x pixel for this longitude grayline

                                                GrayLine_Pos[zz + 2, ww] = GrayLine_Pos[zz + 1, ww] = GrayLine_Pos[zz, ww]; // make sure to cover unused pixels

                                                ww++;
                                                if (ww == 2) break;   // both edges found so done

                                                lon = lon + 40.0; // 40.0 jump a little to save time
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

                                                lon = lon + 40.0;         // 40.0 jump a little to save time
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

                                        if (SUNANGLE(lat, 0.0) < 96) check_for_light = false; // your in light so check for dark
                                        else check_for_light = true; // >= 90 your in dark so check for light

                                        for (double lon = 0.0; lon >= -180.0; lon = lon - 0.5)  // vertical lines left to right 0 to -180 (west) (check left side of map)
                                        {
                                            tempsun_ang = SUNANGLE(lat, lon);

                                            if (check_for_light == true)
                                            {

                                                if (tempsun_ang < 96) // found light
                                                {

                                                    GrayLine_Pos[zz, ww] = (int)(((180.0 + lon) / 360.0 * Sun_Width) + Sun_Left); // determine x pixel for this longitude grayline

                                                    GrayLine_Pos[zz + 2, ww] = GrayLine_Pos[zz + 1, ww] = GrayLine_Pos[zz, ww];

                                                    ww++;
                                                    if (ww == 2) break;      // if we have 2 edge then done

                                                    lon = lon - 40.0;           // 40.0 jump a little to save time
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

                                                    lon = lon - 40;         // 40.0 jump a little to save time
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
                                        GrayLine_Pos[zz + 2, 1] = GrayLine_Pos[zz + 1, 1] = GrayLine_Pos[zz, 1] = GrayLine_Pos[zz, 0];
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



                                //-------------------------------------------------------------------------------------------------------------------------------------
                                //-------------------------------------------------------------------------------------------------------------------------------------
                                //-------------------------------------------------------------------------------------------------------------------------------------
                                //-------------------------------------------------------------------
                                //-------------------------------------------------------------------
                                // check for dusk (right side first)
                                //-------------------------------------------------------------------
                                //-------------------------------------------------------------------
                                qq = 0;
                                ww = 0;

                                for (double lat = 90.0; lat >= -90.0; lat = lat - 0.5)  // 0.5 horizontal lines top to bottom (North)90 to 0 to -90 (South)
                                {
                                  
                                    if ((SUNANGLE(lat, -180.0) >= 90) && (SUNANGLE(lat, 180.0) >= 90)) tt = 1; // dark on edges of screen 
                                    else tt = 0; // light on at least 1 side

                                    zz = (int)((qq / 360.0 * Sun_WidthY1) + Sun_Top1); // 360 = number of latitude points, determine the y pixel for this latitude grayline

                                 
                                    GrayLine_Pos1[zz, 0] = GrayLine_Pos1[zz, 1] = 0;
                                     
                                    if (SUNANGLE(lat, 0.0) < 90) check_for_light = false; // your in light so check for dark
                                    else check_for_light = true; // >= 96 your in dark so check for light

                                
                                    for (double lon = 0.0; lon <= 180.0; lon = lon + 0.5)   // 0.5
                                    {
                                        tempsun_ang = SUNANGLE(lat, lon); // pos angle from 0 to 120

                                     
                                        if (check_for_light == true) // in dark, looking for light
                                        {

                                            if ((tempsun_ang < 90) ) // found light
                                            {

                                                GrayLine_Pos1[zz, ww] = (int)(((lon + 180.0) / 360.0 * Sun_Width) + Sun_Left); // determine x pixel for this longitude grayline

                                                GrayLine_Pos1[zz + 2, ww] = GrayLine_Pos1[zz + 1, ww] = GrayLine_Pos1[zz, ww]; // make sure to cover unused pixels

                                             
                                                ww++;
                                                if (ww == 2) break;   // both edges found so done

                                                lon = lon + 1.0; // 40.0  jump a little to save time

                                                check_for_light = false; // now in light so check for dark

                                            } // found light

                                        } // your in dark so check for light
                                        else // in light so check for dark
                                        {

                                            if ((tempsun_ang >= 90))  // in Dark (found it)
                                            {

                                                GrayLine_Pos1[zz, ww] = (int)(((lon + 180.0) / 360.0 * Sun_Width) + Sun_Left); // determine x pixel for this longitude grayline

                                                GrayLine_Pos1[zz + 2, ww] = GrayLine_Pos1[zz + 1, ww] = GrayLine_Pos1[zz, ww]; // make sure to cover unused pixels
                                             
                                                ww++;
                                                if (ww == 2) break;   // both edges found so done

                                                lon = lon + 1.0;         // 40.0 jump a little to save time
                                                check_for_light = true; // in dark so now check for light

                                            } // found dark


                                        }// in light so check for dark


                                    } // for lon (right side of map)

                                    //----------------------------------------------------------------------------------
                                    //----------------------------------------------------------------------------------
                                    // check for gray (left side of map
                                    //----------------------------------------------------------------------------------
                                    //----------------------------------------------------------------------------------

                                    if (ww < 2) // still need at least 1 edge (maybe 2)
                                    {

                                        if (SUNANGLE(lat, 0.0) < 90) check_for_light = false; // your in light so check for dark
                                        else check_for_light = true; // >= 90 your in dark so check for light

                                        for (double lon = 0.0; lon >= -180.0; lon = lon - 0.5)  // vertical lines left to right 0 to -180 (west) (check left side of map)
                                        {
                                            tempsun_ang = SUNANGLE(lat, lon);

                                            if (check_for_light == true)
                                            {

                                                if ((tempsun_ang < 90)) // found light
                                                {

                                                    GrayLine_Pos1[zz, ww] = (int)(((180.0 + lon) / 360.0 * Sun_Width) + Sun_Left); // determine x pixel for this longitude grayline
                                                    GrayLine_Pos1[zz + 2, ww] = GrayLine_Pos1[zz + 1, ww] = GrayLine_Pos1[zz, ww];
                                                 
                                                    ww++;
                                                    if (ww == 2) break;      // if we have 2 edge then done

                                                    lon = lon - 1.0;           // 40.0 jump a little to save time
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

                                                    lon = lon - 1.0;         //40.0  jump a little to save time
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

                                    if (tt == 1) // if gray on both edges then figure out which is which and signal display
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
                                            GrayLine_Pos3[zz + 2] = GrayLine_Pos3[zz + 1] = GrayLine_Pos3[zz] = 0;             // gray in center of map, (standard)
                                        }

                                    }
                                    else
                                    {
                                        GrayLine_Pos3[zz + 2] = GrayLine_Pos3[zz + 1] = GrayLine_Pos3[zz] = 0;             // gray in center of map, (standard)
                                    }

                                    qq++; // get next lat

                                } //  for (int lat = 90;lat >= -90;lat--)   horizontal lines top to bottom (North)90 to 0 to -90 (South)


                            } // GRAYLINE = true

                        } //  if ( (UTCNEW != UTCLAST2) || (Setup.DisplayGrayLineColor != GrayLine_Last) )  time changeed or color change


                        if ((Map_Last == 2) && (SP4_Active == 0)) updatemapspots(); // if you need a map update make sure your not in the middle of updating your dx spot list

                        Map_Last = 0;

                        if (SP5_Active == 0) continue;


                        //-------------------------------------------------------------------------------------------------
                        //-------------------------------------------------------------------------------------------------
                        // draw sun tracker and gray line
                        //-------------------------------------------------------------------------------------------------
                        //-------------------------------------------------------------------------------------------------

                        if ((Console.DXR == 0))  MAP = new Bitmap(Map_image); // load up Map image
                        else MAP = new Bitmap(Map_image2); // load up Map image

                        Graphics g = Graphics.FromImage(MAP);

                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        //  g.SmoothingMode = SmoothingMode.AntiAlias;


                        if (SUN == true)
                        {

                            Image src = new Bitmap(sun_image); // load up SUN image ( use PNG to allow transparent background)

                            g.DrawImage(src, Sun_X - 10, Sun_Y - 10, 23, 27); // draw SUN 20 x 20 pixel

                            if (Console.noaaON == 0) // do below if console space weather OFF
                            {
                                g.DrawString("SFI " + SFI.ToString("D"), font1, grid_text_brush, Sun_X + 15, Sun_Y - 10);
                                g.DrawString("A " + Aindex.ToString("D") + ", " + RadioBlackout, font1, grid_text_brush, Sun_X + 15, Sun_Y);

                                if (suncounter > 40) // check every 40 minutes
                                {
                                    NOAA();
                                    suncounter = 0;
                                    Debug.WriteLine("NOAA GET=============");

                                }
                                else
                                {
                                    suncounter++;
                                }
                            }
                            else // console space weather ON
                            {

                                g.DrawString("SFI " + Console.SFI.ToString("D"), font1, grid_text_brush, Sun_X + 15, Sun_Y - 10);
                                g.DrawString("A " + Console.Aindex.ToString("D") + ", " + Console.RadioBlackout, font1, grid_text_brush, Sun_X + 15, Sun_Y);

                            }

                        } // sun tracker enabled


                        //---------------------------------------------------------------------------
                        if (GRAYLINE == true)
                        {

                            Pen p3 = new Pen(GrayLine_Last, 1.0f); // dark
                            Pen p4 = new Pen(GrayLine_Last, 1.0f); // dusk


                            for (int ee = Sun_Top1; ee < Sun_Bot1; ee++)
                            {

                                //-----------------------------------------------------------------
                                // ke9ns dusk
                                if (GrayLine_Pos3[ee] == 0) // not dusk on edges on screen
                                {
                                    if ((GrayLine_Pos1[ee, 0]) == 0 && (GrayLine_Pos1[ee, 1]== 0)) g.DrawLine(p4, Sun_Left, ee, Sun_Right, ee);
                                    else  g.DrawLine(p4, GrayLine_Pos1[ee, 0], ee, GrayLine_Pos1[ee, 1], ee);
                                
                                }
                                else if (GrayLine_Pos3[ee] == 1)
                                {
                                    g.DrawLine(p4, GrayLine_Pos1[ee, 0], ee, Sun_Right, ee);
                                    g.DrawLine(p4, GrayLine_Pos1[ee, 1], ee, Sun_Left, ee);
                                  
                                }
                                else
                                {
                                    g.DrawLine(p4, GrayLine_Pos1[ee, 1], ee, Sun_Right, ee);
                                    g.DrawLine(p4, GrayLine_Pos1[ee, 0], ee, Sun_Left, ee);
                                 
                                }


                                //-----------------------------------------------------------------
                                // ke9ns dark
                                if (GrayLine_Pos2[ee] == 0)  // not dark on edges on screen
                                {
                                   if ((GrayLine_Pos[ee, 0]) == 0 && (GrayLine_Pos[ee, 1] == 0)) g.DrawLine(p3, Sun_Left, ee, Sun_Right, ee);
                                   else g.DrawLine(p3, GrayLine_Pos[ee, 0], ee, GrayLine_Pos[ee, 1], ee);
                                 
                                }
                                else if (GrayLine_Pos2[ee] == 1)
                                {
                                    g.DrawLine(p3, GrayLine_Pos[ee, 0], ee, Sun_Right, ee);
                                    g.DrawLine(p3, GrayLine_Pos[ee, 1], ee, Sun_Left, ee);
                                  
                                }
                                else
                                {
                                    g.DrawLine(p3, GrayLine_Pos[ee, 1], ee, Sun_Right, ee);
                                    g.DrawLine(p3, GrayLine_Pos[ee, 0], ee, Sun_Left, ee);
                                 
                                }


                            }  // for loop  

                        } // if GRAYLINE enabled




                        if (SP8_Active == 1) // parse map display just by band, but red dots are for all bands
                        {
                            //-------------------------------------------------------------------------------------
                            //-------------------------------------------------------------------------------------
                            //-------------------------------------------------------------------------------------
                            //-------------------------------------------------------------------------------------
                            // draw country or call sign on map

                      
                            int Flag11 = 0;

                            int kk = 0;
                            int rr = 0;

                            int zz = 0;

                            //  Debug.WriteLine("Band " + console.RX1Band);
                            //  Debug.WriteLine("BandLOW " + VFOLOW);
                            //  Debug.WriteLine("BandHIGH " + VFOHIGH);

                            //   Debug.WriteLine(">>>>>>>>BEACON: check red dot");

                          //  Array.Clear(yy,0,200);

                            if(beacon1 == true) // show time slot for beacons
                            {
                               

                                for (int x = 0; x < 18; x++)
                                {
                                    g.DrawString(Beacon_Call[x].ToString() + " " + Beacon_Country[x].ToString(), font2, grid_text_brush, 55, 20+(x*10)); // use Pandapdater holder[] data
                                }
                                for (int x = 0; x < 5; x++)
                                {
                                    int y = BX_Index[x] / 5;
                                    int y1 = BX_Index[x] % 5; // get remainder

                                    g.DrawString((Beacon_Freq[y1]/1e6).ToString("f2"), font2, grid_text_brush, 27, 20 + (y * 10)); // use Pandapdater holder[] data

                                }
      

                            } // if beacon scan turned on show list of staations and which ones are on right now



                            if (beacon1 == false)
                            {
                                for (int ii = 0; ii < DX_Index; ii++) // red dot always all bands
                                {

                                   
                                    if ((DX_X[ii] != 0) && (DX_Y[ii] != 0))
                                    {

                                       
                                        g.FillRectangle(redbrush, DX_X[ii], DX_Y[ii], 3, 3);  // place red dot on map (all bands)
                                       

                                        if ((chkMapBand.Checked == true)) // map just the band, 
                                        {

                                            if ((DX_Freq[ii] >= VFOLOW) && (DX_Freq[ii] <= VFOHIGH))  //find band your on and its low and upper limits
                                            {
                                                spots[zz++] = ii;                    // ii is the actual DX_INdex pos the the KK holds
                                                                                     // in the display routine this is Display.holder[kk] = ii
                                            }

                                        } // map band only


                                    } // have a lat/long for the spot

                                } // for ii DX_Index  (full dx spot list)
                            }
                            else // beacon below
                            {

                                for (int ii = 0; ii < BX1_Index; ii++) // red dot always all bands
                                {

                                    int dBaboveNoiseFloor = 0;

                                    if ((beacon1 == true))
                                    {
                                        dBaboveNoiseFloor = BX_dBm[ii] - BX_dBm1[ii];
                                    }


                                    if ((BX_X[ii] != 0) && (BX_Y[ii] != 0))
                                    {

                                        if ((beacon1 == true) && (BX_dBm[ii] == -150)) // not checked yet (gray
                                        {
                                            g.FillRectangle(graybrush, BX_X[ii], BX_Y[ii], 3, 3);  // place blue dot on map (all bands)

                                        }
                                        else if ((beacon1 == false)) // Standard DX Spotter RED DOT
                                        {
                                            g.FillRectangle(redbrush, BX_X[ii], BX_Y[ii], 3, 3);  // place red dot on map (all bands)
                                        }
                                        else if ((dBaboveNoiseFloor > Grn_dBm) && (BX_dBm3[ii] > Grn_S)) // strong green  BX_dBm2
                                        {
                                            g.FillRectangle(greenbrush, BX_X[ii], BX_Y[ii], 3, 3);  // place green dot on map (all bands)

                                        }
                                        else if ((dBaboveNoiseFloor > Yel_dBm) && (BX_dBm3[ii] > Yel_S)) // med yellow
                                        {
                                            g.FillRectangle(orangebrush, BX_X[ii], BX_Y[ii], 3, 3);  // place green dot on map (all bands)

                                        }
                                        else if ((dBaboveNoiseFloor >= Org_dBm) && (BX_dBm3[ii] > Org_S)) // week orange
                                        {
                                            g.FillRectangle(yellowbrush, BX_X[ii], BX_Y[ii], 3, 3);  // place yellow dot on map (all bands)

                                        }
                                        else      //if ((beacon1 == false) || ((BX_dBm[ii] - BX_dBm1[ii]) < 10)) // cannot hear signal RED if rx dbm is less then 10db above noise floor
                                        {
                                            g.FillRectangle(redbrush, BX_X[ii], BX_Y[ii], 3, 3);  // place red dot on map (all bands)
                                        }

                                        if ((chkMapBand.Checked == true)) // map just the band, 
                                        {

                                            if ((BX_Freq[ii] >= VFOLOW) && (BX_Freq[ii] <= VFOHIGH))  //find band your on and its low and upper limits
                                            {
                                                spots[zz++] = ii;                    // ii is the actual BX_INdex pos the the KK holds
                                                                                     // in the display routine this is Display.holder[kk] = ii
                                            }

                                        } // map band only


                                    } // have a lat/long for the spot

                                } // for ii BX1_Index  (full dx spot list)


                            } // beacon here above




                            if ((chkBoxPan.Checked == true) && (beacon1 == false)) // show spots on map for just your panadapter
                            {

                                for (int ii = 0; ii < Console.DXK; ii++) // dx call sign or country name on map is just for the band your on
                                {

                                    if ((DX_X[Display.holder[ii]] != 0) && (DX_Y[Display.holder[ii]] != 0))  // dont even bother with the spot if X and Y = 0 since that means no data to plot
                                    {

                                        if (chkMapCountry.Checked == true) // spot country on map
                                        {


                                            if (chkBoxBeam.Checked == true) g.DrawString(DX_country[Display.holder[ii]] + "(" + DX_Beam[Display.holder[ii]] + "°)", font2, grid_text_brush, DX_X[Display.holder[ii]], DX_Y[Display.holder[ii]]); // use Pandapdater holder[] data
                                            else g.DrawString(DX_country[Display.holder[ii]], font2, grid_text_brush, DX_X[Display.holder[ii]], DX_Y[Display.holder[ii]]); // use Pandapdater holder[] data


                                        } // chkMapCountry true = draw country name on map

                                        else if (chkMapCall.Checked == true)  // else show call signs on map
                                        {

                                            for (rr = 0; rr < kk; rr++)  // check all accumulated countrys from the current DX_index list
                                            {
                                                if (country[rr] == DX_country[Display.holder[ii]])  // use Pandapdater holder[] data
                                                {
                                                    yy[rr] = yy[rr] + 10; // multiple calls for same country stack downward
                                                    Flag11 = 1;
                                                    break;
                                                }


                                            } // for rr loop


                                            if (Flag11 == 0)
                                            {
                                                country[kk] = DX_country[Display.holder[ii]]; // add to list
                                                yy[kk] = 0;
                                            }

                                            kk++; // increment for next country

                                            Flag11 = 0; // reset flag

                                            if (chkBoxBeam.Checked == true) g.DrawString(DX_Station[Display.holder[ii]] + "(" + DX_Beam[Display.holder[ii]] + "°)", font2, grid_text_brush, DX_X[Display.holder[ii]], DX_Y[Display.holder[ii]] + yy[rr]); // Station  name
                                            else g.DrawString(DX_Station[Display.holder[ii]], font2, grid_text_brush, DX_X[Display.holder[ii]], DX_Y[Display.holder[ii]] + yy[rr]); // Station  name

                                        } // chkMapCall true = draw all sign on map


                                    } //  if ((DX_X[ii] != 0) && (DX_Y[ii] != 0))


                                } // for ii index loop

                            } // chkboxpan

                            else if ((chkMapBand.Checked == true) && (beacon1 == false)) //  show spots on map for your entire band
                            {

                                for (int ii = 0; ii < zz; ii++) // dx call sign or country name on map is just for the band your on
                                {

                                    if ((DX_X[spots[ii]] != 0) && (DX_Y[spots[ii]] != 0))  // dont even bother with the spot if X and Y = 0 since that means no data to plot
                                    {

                                        if (chkMapCountry.Checked == true) // spot country on map
                                        {
                                            if (chkBoxBeam.Checked == true) g.DrawString(DX_country[spots[ii]] + "(" + DX_Beam[spots[ii]] + "°)", font2, grid_text_brush, DX_X[spots[ii]], DX_Y[spots[ii]]); // use Pandapdater holder[] data
                                            else g.DrawString(DX_country[spots[ii]], font2, grid_text_brush, DX_X[spots[ii]], DX_Y[spots[ii]]); // use Pandapdater holder[] data

                                        } // chkMapCountry true = draw country name on map

                                        else if (chkMapCall.Checked == true)  // else show call signs on map
                                        {

                                            for (rr = 0; rr < kk; rr++)  // check all accumulated countrys from the current DX_index list
                                            {
                                                if (country[rr] == DX_country[spots[ii]])  // use Pandapdater holder[] data
                                                {
                                                    yy[rr] = yy[rr] + 10; // multiple calls for same country stack downward
                                                    Flag11 = 1;
                                                    break;
                                                }


                                            } // for rr loop


                                            if (Flag11 == 0)
                                            {
                                                country[kk] = DX_country[spots[ii]]; // add to list
                                                yy[kk] = 0;
                                            }

                                            kk++; // increment for next country

                                            Flag11 = 0; // reset flag

                                            if (chkBoxBeam.Checked == true) g.DrawString(DX_Station[spots[ii]] + "(" + DX_Beam[spots[ii]] + "°)", font2, grid_text_brush, DX_X[spots[ii]], DX_Y[spots[ii]] + yy[rr]); // Station  name
                                            else g.DrawString(DX_Station[spots[ii]], font2, grid_text_brush, DX_X[spots[ii]], DX_Y[spots[ii]] + yy[rr]); // Station  name


                                        } // chkMapCall true = draw all sign on map


                                    } //  if ((DX_X[ii] != 0) && (DX_Y[ii] != 0))


                                } // for ii index loop
                            } // chkMapBand true = just show spots on map for the band you can see

                            //---------------------------------------------------
                            // display data on red dots for all HF below  and for Beacon Scanning
                            else
                            {


                            if (beacon1 == false)
                            { 
                                for (int ii = 0; ii < DX_Index; ii++) // dx call sign or country name on map is for all HF
                                {

                              
                                    if ((DX_X[ii] != 0) && (DX_Y[ii] != 0))
                                    {

                                        if (chkMapCountry.Checked == true) // spot country on map
                                        {

                                            
                                            if (chkBoxBeam.Checked == true) g.DrawString(DX_country[ii] + "(" + DX_Beam[ii] + "°)", font2, grid_text_brush, DX_X[ii], DX_Y[ii]); // country name
                                            else g.DrawString(DX_country[ii], font2, grid_text_brush, DX_X[ii], DX_Y[ii]); // country name
                                   

                                        } // chkMapCountry true = draw country name on map

                                        else if (chkMapCall.Checked == true)  // show call signs on map
                                        {

                                            for (rr = 0; rr < kk; rr++)  // check all accumulated countrys from the current DX_index list
                                            {
                                                if (country[rr] == DX_country[ii])
                                                {
                                                    yy[rr] = yy[rr] + 10; // multiple calls for same country stack downward
                                                    Flag11 = 1;
                                                    break;
                                                }


                                            } // for rr loop


                                            if (Flag11 == 0)
                                            {
                                                country[kk] = DX_country[ii]; // add to list
                                                yy[kk] = 0;
                                            }

                                            kk++; // increment for next country

                                            Flag11 = 0; // reset flag

                                           
                                            if (chkBoxBeam.Checked == true) g.DrawString(DX_Station[ii] + "(" + DX_Beam[ii] + "°)", font2, grid_text_brush, DX_X[ii], DX_Y[ii] + yy[rr]); // Station  name
                                            else g.DrawString(DX_Station[ii], font2, grid_text_brush, DX_X[ii], DX_Y[ii] + yy[rr]); // Station  name

                                  

                                        } // chkMapCall true = draw all sign on map


                                    } //  if ((DX_X[ii] != 0) && (DX_Y[ii] != 0))


                                } // for ii index loop
                            } // beacon1 == false
                                //=========================================================================================

                            else
                             {
                                    for (int ii = 0; ii < BX1_Index; ii++) // dx call sign or country name on map is for all HF
                                    {

                                        int dBaboveNoiseFloor = 0;

                                        if ((beacon1 == true))
                                        {
                                            dBaboveNoiseFloor = BX_dBm[ii] - BX_dBm1[ii];
                                        }

                                        if ((BX_X[ii] != 0) && (BX_Y[ii] != 0))
                                        {

                                            if (chkMapCountry.Checked == true) // spot country on map
                                            {
                                                
                                                    if (
                                                        ((beacon11 > 0) && (ii >= ((BX_Index[beacon11 - 1] / 5) * 5)) && (ii <= ((BX_Index[beacon11 - 1] / 5) * 5) + 4)) ||  // for slow beacon scanning

                                                        ((beacon11 == 0) &&
                                                        (ii >= ((BX_Index[0] / 5) * 5)) && (ii <= ((BX_Index[0] / 5) * 5) + 4) ||        // for fast beacon scanning
                                                        (ii >= ((BX_Index[1] / 5) * 5)) && (ii <= ((BX_Index[1] / 5) * 5) + 4) ||
                                                        (ii >= ((BX_Index[2] / 5) * 5)) && (ii <= ((BX_Index[2] / 5) * 5) + 4) ||
                                                        (ii >= ((BX_Index[3] / 5) * 5)) && (ii <= ((BX_Index[3] / 5) * 5) + 4) ||
                                                        (ii >= ((BX_Index[4] / 5) * 5)) && (ii <= ((BX_Index[4] / 5) * 5) + 4))
                                                        )

                                                    {
                                                        if (chkBoxBeam.Checked == true) g.DrawString(BX_country[ii] + "(" + BX_Beam[ii] + "°)", font2, Beacon_brush, BX_X[ii], BX_Y[ii]); // country name (violet)
                                                        else g.DrawString(BX_country[ii], font2, Beacon_brush, BX_X[ii], BX_Y[ii]); // country name

                                                    }
                                                    else
                                                    {


                                                        if (BX_TSlot[ii] == 0)        // ((BX_dBm[ii] == -150)) // not checked yet gray
                                                        {
                                                            if (chkBoxBeam.Checked == true) g.DrawString(BX_country[ii] + "(" + BX_Beam[ii] + "°)", font2, graybrush, BX_X[ii], BX_Y[ii]); // country name
                                                            else g.DrawString(BX_country[ii], font2, graybrush, BX_X[ii], BX_Y[ii]); // country name

                                                        }
                                                        else if ((dBaboveNoiseFloor > Grn_dBm) && (BX_dBm3[ii] > Grn_S)) // strong green
                                                        {
                                                            if (chkBoxBeam.Checked == true) g.DrawString(BX_country[ii] + "(" + BX_Beam[ii] + "°)", font2, greenbrush, BX_X[ii], BX_Y[ii]); // country name
                                                            else g.DrawString(BX_country[ii], font2, greenbrush, BX_X[ii], BX_Y[ii]); // country name

                                                        }
                                                        else if ((dBaboveNoiseFloor > Yel_dBm) && (BX_dBm3[ii] > Yel_S)) // med yellow
                                                        {
                                                            if (chkBoxBeam.Checked == true) g.DrawString(BX_country[ii] + "(" + BX_Beam[ii] + "°)", font2, orangebrush, BX_X[ii], BX_Y[ii]); // country name
                                                            else g.DrawString(BX_country[ii], font2, orangebrush, BX_X[ii], BX_Y[ii]); // country name

                                                        }
                                                        else if ((dBaboveNoiseFloor >= Org_dBm) && (BX_dBm3[ii] > Org_S)) // week orange
                                                        {
                                                            if (chkBoxBeam.Checked == true) g.DrawString(BX_country[ii] + "(" + BX_Beam[ii] + "°)", font2, grid_text_brush, BX_X[ii], BX_Y[ii]); // country name
                                                            else g.DrawString(BX_country[ii], font2, grid_text_brush, BX_X[ii], BX_Y[ii]); // country name

                                                        }
                                                        else            // if ((dBaboveNoiseFloor < 10)) // cannot hear signal red
                                                        {
                                                            if (chkBoxBeam.Checked == true) g.DrawString(BX_country[ii] + "(" + BX_Beam[ii] + "°)", font2, redbrush, BX_X[ii], BX_Y[ii]); // country name
                                                            else g.DrawString(BX_country[ii], font2, redbrush, BX_X[ii], BX_Y[ii]); // country name

                                                        }
                                                    }

                                               
                                               


                                            } // chkMapCountry true = draw country name on map

                                            else if (chkMapCall.Checked == true)  // show call signs on map
                                            {

                                                for (rr = 0; rr < kk; rr++)  // check all accumulated countrys from the current BX1_index list
                                                {
                                                    if (country[rr] == BX_country[ii])
                                                    {
                                                        yy[rr] = yy[rr] + 10; // multiple calls for same country stack downward
                                                        Flag11 = 1;
                                                        break;
                                                    }


                                                } // for rr loop


                                                if (Flag11 == 0)
                                                {
                                                    country[kk] = BX_country[ii]; // add to list
                                                    yy[kk] = 0;
                                                }

                                                kk++; // increment for next country

                                                Flag11 = 0; // reset flag

                                               

                                                    // violet when its scanning this spot
                                                    if (
                                                          ((beacon11 > 0) && (BX_Index[beacon11 - 1] == ii)) ||    // for slow beacon scanning

                                                          (beacon11 == 0) && ((BX_Index[0] == ii) || (BX_Index[1] == ii) || (BX_Index[2] == ii) || (BX_Index[3] == ii) || (BX_Index[4] == ii))  // for fast beacon scanning
                                                        )
                                                    {
                                                        if (chkBoxBeam.Checked == true) g.DrawString(BX_Station[ii] + "(" + BX_Beam[ii] + "°)", font2, Beacon_brush, BX_X[ii], BX_Y[ii] + yy[rr]); // VIOLET  Station name
                                                        else g.DrawString(BX_Station[ii], font2, Beacon_brush, BX_X[ii], BX_Y[ii] + yy[rr]); // Station  name

                                                    }
                                                    else
                                                    {

                                                        if (BX_TSlot[ii] == 0)        // ((BX_dBm[ii] == -150)) // not checked yet GRAY
                                                        {
                                                            if (chkBoxBeam.Checked == true) g.DrawString(BX_Station[ii] + "(" + BX_Beam[ii] + "°)", font2, graybrush, BX_X[ii], BX_Y[ii] + yy[rr]); // Station  name
                                                            else g.DrawString(BX_Station[ii], font2, graybrush, BX_X[ii], BX_Y[ii] + yy[rr]); // Station  name

                                                        }
                                                        else if ((dBaboveNoiseFloor > Grn_dBm) && (BX_dBm3[ii] > Grn_S)) // strong GREEN
                                                        {
                                                            if (chkBoxBeam.Checked == true) g.DrawString(BX_Station[ii] + "(" + BX_Beam[ii] + "°)", font2, greenbrush, BX_X[ii], BX_Y[ii] + yy[rr]); // Station  name
                                                            else g.DrawString(BX_Station[ii], font2, greenbrush, BX_X[ii], BX_Y[ii] + yy[rr]); // Station  name

                                                        }
                                                        else if ((dBaboveNoiseFloor > Yel_dBm) && (BX_dBm3[ii] > Yel_S)) //med Yel
                                                        {
                                                            if (chkBoxBeam.Checked == true) g.DrawString(BX_Station[ii] + "(" + BX_Beam[ii] + "°)", font2, orangebrush, BX_X[ii], BX_Y[ii] + yy[rr]); // Station  name
                                                            else g.DrawString(BX_Station[ii], font2, orangebrush, BX_X[ii], BX_Y[ii] + yy[rr]); // Station  name

                                                        }
                                                        else if ((dBaboveNoiseFloor >= Org_dBm) && (BX_dBm3[ii] > Org_S)) // week Org
                                                        {
                                                            if (chkBoxBeam.Checked == true) g.DrawString(BX_Station[ii] + "(" + BX_Beam[ii] + "°)", font2, yellowbrush, BX_X[ii], BX_Y[ii] + yy[rr]); // Station  name
                                                            else g.DrawString(BX_Station[ii], font2, yellowbrush, BX_X[ii], BX_Y[ii] + yy[rr]); // Station  name

                                                        }
                                                        else           //if ((dBaboveNoiseFloor < 10)) // cannot hear signal RED
                                                        {
                                                            if (chkBoxBeam.Checked == true) g.DrawString(BX_Station[ii] + "(" + BX_Beam[ii] + "°)", font2, redbrush, BX_X[ii], BX_Y[ii] + yy[rr]); // Station  name
                                                            else g.DrawString(BX_Station[ii], font2, redbrush, BX_X[ii], BX_Y[ii] + yy[rr]); // Station  name

                                                        }
                                                    }
                                               
                                               

                                            } // chkMapCall true = draw all sign on map


                                        } //  if ((BX_X[ii] != 0) && (BX_Y[ii] != 0))


                                    } // for ii index loop

                                } // beacon here above



                            } // chkMapBand false = show all spots on map

                        } // SP8_Active = 1


                        //----------------------------------------------------------------------------------------------------
                        // update MAP background

                        console.picDisplay.SizeMode = PictureBoxSizeMode.StretchImage;           // put image back onto picDisplay background image
                        console.picDisplay.BackgroundImage = MAP;                                  // MAP.Save("test.bmp");  save modified map_image to actual file on hard drive


                        beacon4 = false; // reset the beacon scanner flag (you just updated the map)


                    } // check every 1 minutes or unless spots change

                } // only check in in panadapter mode since you cant see it in any other mode
                else
                {
                    SUN = false;
                    GRAYLINE = false;
                }

            } // for loop (SP5_Active == 1)

        } // TrackSun


       
   



        public void chkPanMode_CheckedChanged(object sender, EventArgs e)
        {

            if (SP5_Active == 1) // only check if tracking is already on
            {
                if (chkPanMode.Checked == true)
                {

                    Display.map = 1;
                    Display.CurrentDisplayMode = DisplayMode.PANAFALL;


                }
                else if (chkPanMode.Checked == false) // turn off special pan and put back original display mode
                {
                    Display.map = 0;
                    Display.CurrentDisplayMode = LastDisplayMode;
                }
            }
        } // chkPanMode_CheckedChanged



        //=========================================================================================


        private static int DXLOC_Index1 = 0;
        private static int SP8_Active = 0;    // 1=DX LOC scanned into memory

        // data obtained from DXLOC.txt file
        public static string[] DXLOC_prefix = new string[2000];       // prefix (must start with)
        public static string[] DXLOC_prefix1 = new string[2000];      // prefix (must also contain) /
        public static string[] DXLOC_prefix2 = new string[2000];      // prefix (must exclude) \

        public static string[] DXLOC_lat = new string[2000];          // text of lat
        public static string[] DXLOC_lon = new string[2000];          // text of  lon
        public static double[] DXLOC_LAT = new double[2000];          // latitude  
        public static double[] DXLOC_LON = new double[2000];          //  longitude
        public static string[] DXLOC_country = new string[2000];      // country
        public static string[] DXLOC_continent = new string[2000];    // continent

     
        //=======================================================================================
        //=======================================================================================
        //ke9ns Open and read DXLOC.txt file here (put into array of prefix vs lat/lon value)
        //   Fields:
        //
        //0	    DXCC number
        //1*	ARRL DXCC prefix,
        //2*    DXCC Entity name,
        //3*	Continent,
        //4*	Latitude,
        //5*	Longitude,
        //6	    CQ Zone,
        //7	    ITU Zone,
        //8	Active (A) or Deleted (D),
        //9	Date from becoming a valid Entity,
        //10	Possible prefixes from ITU Assigned Blocks to Sovereign UN Territory(s)
        public void DXLOC_FILE()
        {

            string file_name = console.AppDataPath + "DXLOC.txt"; // //  sked - b15.csv

            textBox1.Text += "Attempting to open DX Location list\r\n";

            if (File.Exists(file_name))
            {

                textBox1.Text += "Reading DX Location list\r\n";

                try
                {
                    stream2 = new FileStream(file_name, FileMode.Open); // open  file
                    reader2 = new BinaryReader(stream2, Encoding.ASCII);

                }
                catch(Exception)
                {
                    SP8_Active = 0;
                    Debug.WriteLine("NO DX LOC FILE============================");
                    return;


                }
                var result = new StringBuilder();

                if (SP8_Active == 0) // dont reset if already scanned in  database
                {
                    DXLOC_Index1 = 0; // how big is the DXLOC data file in lines

                }

                for (;;) // read file and extract data from it and close it and set sp8_active = 1 when done
                {

                    if (SP8_Active == 1) // aleady scanned database
                    {
                        break; // dont rescan database over 
                    }

                    if (SP_Active == 0)
                    {
                        reader2.Close();    // close  file
                        stream2.Close();   // close stream

                        return;
                    }



                    try
                    {
                        var newChar = (char)reader2.ReadChar();

                        if ((newChar == '\r'))
                        {
                            newChar = (char)reader2.ReadChar(); // read \n char to finishline

                            string[] values = result.ToString().Split(','); // split line up into segments divided by ,


                            //   Debug.Write(DXLOC_Index1.ToString());

                            DXLOC_prefix[DXLOC_Index1] = values[1].Substring(1, values[1].Length - 2);                       // call sign prefix
                                                                                                                             //    Debug.Write(" prefix>" + DXLOC_prefix[DXLOC_Index1]);


                            if (DXLOC_prefix[DXLOC_Index1].Contains("/")) // indicating an extra character the call sign must contain
                            {
                                DXLOC_prefix1[DXLOC_Index1] = DXLOC_prefix[DXLOC_Index1].Substring(DXLOC_prefix[DXLOC_Index1].Length - 1, 1);
                                DXLOC_prefix[DXLOC_Index1] = DXLOC_prefix[DXLOC_Index1].Substring(0, DXLOC_prefix[DXLOC_Index1].Length - 2);

                            }
                            else DXLOC_prefix1[DXLOC_Index1] = null;

                            if (DXLOC_prefix[DXLOC_Index1].Contains("\\")) // indicating an extra character the call sign must not contain
                            {
                                DXLOC_prefix2[DXLOC_Index1] = DXLOC_prefix[DXLOC_Index1].Substring(DXLOC_prefix[DXLOC_Index1].Length - 1, 1);
                                DXLOC_prefix[DXLOC_Index1] = DXLOC_prefix[DXLOC_Index1].Substring(0, DXLOC_prefix[DXLOC_Index1].Length - 2);


                            }
                            else DXLOC_prefix2[DXLOC_Index1] = null;

                            //    Debug.Write(" prefix>" + DXLOC_prefix[DXLOC_Index1]);

                            //    Debug.Write(" pre/ " + DXLOC_prefix1[DXLOC_Index1]);
                            //   Debug.Write(" pre\\ " + DXLOC_prefix2[DXLOC_Index1]);



                            DXLOC_country[DXLOC_Index1] = values[2].Substring(1, values[2].Length - 2);                       // call sign country
                                                                                                                              // Debug.Write(" country>" + DXLOC_country[DXLOC_Index1]);


                            DXLOC_continent[DXLOC_Index1] = values[3].Substring(1, values[3].Length - 2);                     // call sign continent
                                                                                                                              //  Debug.Write(" continent>" + DXLOC_continent[DXLOC_Index1]);

                            DXLOC_lat[DXLOC_Index1] = values[4];                          // call sign lat
                                                                                          //  Debug.Write(" lat>" + DXLOC_lat[DXLOC_Index1]);

                            DXLOC_lon[DXLOC_Index1] = values[5];                          // call sign lon
                                                                                          //  Debug.Write(" lon>" + DXLOC_lon[DXLOC_Index1]);

                            // horizontal lines top to bottom (North)90 to 0 to (-SOUTH)90
                            // vertical lines left to right  -West(180) to 0 to +East(180)


                            if (DXLOC_lat[DXLOC_Index1].Contains("N")) // pos 90 North
                            {
                                int ff = DXLOC_lat[DXLOC_Index1].IndexOf('N') - 1;

                                try
                                {
                                    DXLOC_LAT[DXLOC_Index1] = Convert.ToDouble(DXLOC_lat[DXLOC_Index1].Substring(1, ff));
                                    //     Debug.Write(" LAT>" + DXLOC_LAT[DXLOC_Index1]);

                                }
                                catch (Exception)
                                {

                                       Debug.WriteLine(" NORTH " + DXLOC_lon[DXLOC_Index1].Substring(1, ff));
                                    DXLOC_LAT[DXLOC_Index1] = 0;


                                }

                            }  // pos 90 North
                            else if (DXLOC_lat[DXLOC_Index1].Contains("S")) // neg 90 North
                            {

                                int ff = DXLOC_lat[DXLOC_Index1].IndexOf('S') - 1;

                                try
                                {
                                    DXLOC_LAT[DXLOC_Index1] = -Convert.ToDouble(DXLOC_lat[DXLOC_Index1].Substring(1, ff));
                                    //    Debug.Write(" LAT>" + DXLOC_LAT[DXLOC_Index1]);

                                }
                                catch (Exception)
                                {
                                    DXLOC_LAT[DXLOC_Index1] = 0;
                                       Debug.WriteLine(" SOUTH " + DXLOC_lon[DXLOC_Index1].Substring(1, ff));
                                        Debug.Write(" prefix>" + DXLOC_prefix[DXLOC_Index1]);

                                }

                            }  // neg 90 North


                            if (DXLOC_lon[DXLOC_Index1].Contains("W")) // neg 180 west
                            {
                                int ff = DXLOC_lon[DXLOC_Index1].IndexOf('W') - 1;

                                try
                                {

                                    DXLOC_LON[DXLOC_Index1] = -Convert.ToDouble(DXLOC_lon[DXLOC_Index1].Substring(1, ff));
                                    //  Debug.WriteLine(" LON>" + DXLOC_LON[DXLOC_Index1]);

                                }
                                catch (Exception)
                                {
                                        Debug.WriteLine(" WEST " + DXLOC_lon[DXLOC_Index1].Substring(1, ff));
                                    DXLOC_LON[DXLOC_Index1] = 0;
                                }

                            }  // neg 180 West
                            else if (DXLOC_lon[DXLOC_Index1].Contains("E")) // pos 180 East
                            {
                                int ff = DXLOC_lon[DXLOC_Index1].IndexOf('E') - 1;

                                try
                                {

                                    DXLOC_LON[DXLOC_Index1] = Convert.ToDouble(DXLOC_lon[DXLOC_Index1].Substring(1, ff));
                                    //  Debug.WriteLine(" LON>" + DXLOC_LON[DXLOC_Index1]);

                                }
                                catch (Exception)
                                {
                                       Debug.WriteLine(" EAST " + DXLOC_lon[DXLOC_Index1].Substring(1, ff));
                                    DXLOC_LON[DXLOC_Index1] = 0;
                                }

                            }  // pos 180 East



                            DXLOC_Index1++;

                            if (DXLOC_Index1 > 2000) break;

                            result = new StringBuilder(); // clean up for next line

                        } // \r
                        else
                        {
                            result.Append(newChar);  // save char
                        }

                    }
                    catch (EndOfStreamException)
                    {
                        DXLOC_Index1--;

                        if (DXLOC_Index1 < 10) textBox1.Text += "No DXLOC.txt list file found in database folder\r\n";
                        else   textBox1.Text += "End of DX LOC FILE at " + DXLOC_Index1.ToString() + "\r\n";

                        break; // done with file
                    }
                    catch (Exception)
                    {
                        //  Debug.WriteLine("excpt======== " + e);
                        //     textBox1.Text = e.ToString();

                        if (DXLOC_Index1 < 10) textBox1.Text += "No DXLOC.txt list file found in database folder\r\n";

                        break; // done with file
                    }


                } // for loop until end of file is reached


             //   Debug.WriteLine("reached DXLOC end of file" + DXLOC_Index1.ToString());
                textBox1.Text += "Reached End of DXLOC.txt FILE with # " + DXLOC_Index1.ToString() + "\r\n";


                reader2.Close();    // close  file
                stream2.Close();   // close stream


                SP8_Active = 1; // done loading DXLOC database (Good)



            } // if file exists
            else
            {
                SP8_Active = 0;
                Debug.WriteLine("NO DX LOC FILE============================");
            }



        } // DXMAP()

        private void chkMapCountry_CheckedChanged(object sender, EventArgs e)
        {

            if (chkMapCountry.Checked == true) chkMapCall.Checked = false;
            Map_Last = 1;

        }

        private void chkMapCall_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMapCall.Checked == true) chkMapCountry.Checked = false;
            Map_Last = 1;

        }

        private void chkMapBand_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMapBand.Checked == true) chkBoxPan.Checked = false;
            Map_Last = 1;

        }

        private void chkBoxPan_CheckedChanged(object sender, EventArgs e)
        {

            if (chkBoxPan.Checked == true) chkMapBand.Checked = false;
            Map_Last = 1;
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {

            textBox1.ShortcutsEnabled = false; // added to eliminate the contextmenu from popping up

        }

      

       

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (console.DXMemList.List.Count == 0) return; // nothing in the list, exit

            int index = dataGridView1.CurrentCell.RowIndex;


            if ((index < 0) || (index > (console.DXMemList.List.Count - 1))) return;// index out of range

            //   DXMemRecord recordToRestore = new DXMemRecord((DXMemRecord)DXURL.SelectedItem);

            Debug.WriteLine("Double CLick=" + index);
        }


        public static int RIndex1 = 0;

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            RIndex1 = e.RowIndex; // last row you clicked on 

        }

      
      


        public static byte SP6_Active = 0; // 1= turn on MEMORY in panadapter

        private void chkBoxMem_CheckedChanged(object sender, EventArgs e)
        {

            if (chkBoxMem.Checked == true)
            {
              
                  dataGridView2.DataSource = console.MemoryList.List;   // ke9ns get list of memories from memorylist.cs is where the file is opened and saved

                SP6_Active = 1;

                //  comboFMMemory.DataSource = MemoryList.List;
                //  comboFMMemory.DisplayMember = "Name";
                //  comboFMMemory.ValueMember = "Name";


                //  Debug.WriteLine("comboFM " + (string)dataGridView1["Name", dataGridView1.CurrentCell.RowIndex].Value);

                //  Debug.WriteLine("comboFM " + (string)dataGridView2[2, 0].Value);
                //  Debug.WriteLine("comboFM1 " + dataGridView2[1, 3].ToString());
                  Debug.WriteLine("Rows Count " + dataGridView2.Rows.Count);

            }
            else
            {
                SP6_Active = 0;
            }

        //  MemoryList X = console.MemoryList.List;
         //   MemoryRecord recordToRestore = new MemoryRecord((MemoryRecord)comboFMMemory.SelectedItem);
           
              //  console.RecallMemory(recordToRestore);

        } //chkboxmem_checked



        //=====================================================
        private void SWLbutton2_Click(object sender, EventArgs e)
        {
            console.SWLFORM = true; // open up SWL search window


        }


        //=====================================================
        // Your station Lat and Long used in Beam heading for Spots
        private void udDisplayLat_ValueChanged(object sender, EventArgs e)
        {
            Map_Last = 1;
        }

        private void udDisplayLong_ValueChanged(object sender, EventArgs e)
        {
            Map_Last = 1;
        }

        // ke9ns put beam heading on map
        private void chkBoxBeam_CheckedChanged(object sender, EventArgs e)
        {
            Map_Last = 1;

        }

        //========================================================================
        //========================================================================

        public void NOAA()
        {

            RadioBlackout = " ";
            GeoBlackout = " ";

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
             //   Debug.WriteLine("noaa=== " + noaa);

             //   textBox1.Text += "NOAA Download complete \r\n";

           

            //--------------------------------------------------------------------
            if (noaa.Contains("Solar flux ")) // 
            {

                int ind = noaa.IndexOf("Solar flux ") + 11;

                try
                {
                    SFI = (int)(Convert.ToDouble(noaa.Substring(ind, 3)));
                    Debug.WriteLine("SFI " + SFI);
                }
                catch (Exception)
                {
                    SFI = 0;
                }


            } // SFI

            if (noaa.Contains("A-index ")) // 
            {

                int ind = noaa.IndexOf("A-index ") + 8;

                try
                {
                    Aindex = (int)(Convert.ToDouble(noaa.Substring(ind, 2)));
                  Debug.WriteLine("Aindex " + Aindex);
                }
                catch (Exception)
                {
                    Aindex = 0;
                }


            } // Aindex

            if (noaa.Contains("Radio blackouts reaching the ")) // 
            {

                int ind = noaa.IndexOf("Radio blackouts reaching the ") + 29;

                try
                {
                    RadioBlackout = noaa.Substring(ind, 2);
                      Debug.WriteLine("Radio Blackout " + RadioBlackout);
                }
                catch (Exception)
                {
                    RadioBlackout = " ";
                }


            } // radio blackouts
           
          
            if (!noaa.Contains("No space weather storms ") && noaa.Contains("Geomagnetic storms reaching the ")) // 
            {

                int ind = noaa.IndexOf("Geomagnetic storms reaching the ") + 32;

                try
                {
                    GeoBlackout = noaa.Substring(ind, 2);
                    Debug.WriteLine("Geomagnetic storms" + GeoBlackout);
                }
                catch (Exception)
                {
                    GeoBlackout = " ";
                }


            } // radio blackouts

                if (RadioBlackout != " ")
                {
                    RadioBlackout = RadioBlackout + GeoBlackout;
                    Debug.WriteLine("radio-geo " + RadioBlackout);


                }
                else
                {
                    RadioBlackout = GeoBlackout;
                    Debug.WriteLine("geo " + RadioBlackout);
                }

            }
            catch (Exception ex)
            {
                //   Debug.WriteLine("noaa fault=== " + ex);
                textBox1.Text += "Failed to download Space Weather \r\n";

            }


    } // NOAA


        //=====================================================
        //ke9ns formulat to calculate beam heading
        // thanks to: http://www.movable-type.co.uk/scripts/latlong.html 
        //Formula: θ = atan2( sin Δλ ⋅ cos φ2 , cos φ1 ⋅ sin φ2 − sin φ1 ⋅ cos φ2 ⋅ cos Δλ ) 
        //  where φ1, λ1 is the start point (lat, long), φ2,λ2 (lat, long) the end point       (Δλ is the difference in longitude)
        //For final bearing, simply take the initial bearing from the end point to the start point and reverse it (using θ = (θ+180) % 360).

        double LatDest = 0; // radians.  but equals -90 to 90 deg
        double LongDest = 0;  // radians. but equals -180 to 180 deg

        double LatStart = 0; // radians.  but equals -90 to 90 deg
        double LongStart = 0;  // radians. but equals -180 to 180 deg

        public int BeamHeading(double SpotLat, double SpotLong)
        {

            LatStart = (Math.PI / 180.0) * (double)udDisplayLat.Value; // convert degree's to radians
            LongStart = (Math.PI / 180.0) * (double)udDisplayLong.Value;

            LatDest = (Math.PI / 180.0) * SpotLat;  // convert degree's to rads
            LongDest = (Math.PI / 180.0) * SpotLong;

            double y = Math.Sin(LongDest - LongStart) * Math.Cos(LatDest);
            double x = Math.Cos(LatStart) * Math.Sin(LatDest) - Math.Sin(LatStart) * Math.Cos(LatDest) * Math.Cos(LongDest - LongStart);

            int Bearing = (int)(Math.Atan2(y, x) * (180.0 / Math.PI));
         //   Debug.WriteLine("Init Bearing=" + Bearing);

            int FBearing = (int)(((Math.Atan2(y, x) * (180.0 / Math.PI)) + 360) % 360.0 );

         //   Debug.WriteLine("Final Bearing=" + FBearing);

            return FBearing;


        } // Beamheading



        //==============================================================================
        //==============================================================================
        //==============================================================================
        // BEACON CHECK ON/OFF
        //==============================================================================
        //==============================================================================
        //==============================================================================
        // PowerSDR will scan through 14.1, 18.11, 21.15, 24.93, 28.2 mhz
        // looking for 18 stations: 4U1UN, VE8AT, W6WX, KH6RS, ZL6B, VK6RBP, 
        // JA1IGY, RR9O, VR2B, 4S7B, ZS6DN, 5Z4B, 4X6TU, OH2B, CS3B, LU4AA,
        // OA4B, and YV5B in 10 second intervals.Thats 5 frequecies and 18 stations
        // rotating in 10 intervals = 10 * 18 = 180second = 3minutes until a repeat.
        // must have 18 stations so 4U1UN repeats on 14100 20 times/ hour exactly
   
        public int Grn_dBm = 35;   // green indicator when this dBm above noise floor
        public int Grn_S = 3;      // green indicator when this S units or above

        public int Yel_dBm = 25;   // Yellow indicator when this dBm above noise floor
        public int Yel_S = 2;      // Yellow indicator when this S units or above

        public int Org_dBm = 15;   // Orange indicator when this dBm above noise floor
        public int Org_S = 1;      // Orange indicator when this S units or above
      
        public int Red_dBm = 5;    // Red indicator when this dBm above noise floor
        public int Red_S = 0;      // Red indicator when this S units or above

        public int[] Beacon_Freq = new int[] { 14100000, 18110000, 21150000, 24930000, 28200000 }; // ke9ns NCDXF/IARU beacon channels 0-4

        public string[] Beacon_Call = new string[]               
          { "4U1UN", "VE8AT", "W6WX", "KH6RS", "ZL6B", "VK6RBP",   //  "4U1UN", "VE8AT", "W6WX", "KH6RS", "ZL6B", "VK6RBP",
           "JA2IGY", "RR9O", "VR2B", "-4S7B", "ZS6DN", "5Z4B", //  "JA2IGY", "RR9O", "VR2B", "4S7B", "ZS6DN", "5Z4B",
              "4X6TU", "OH2B", "-CS3B", "-LU4AA", "0A4B", "YV5B" };    //  "4X6TU", "OH2B", "CS3B", "LU4AA", "0A4B", "YV5B"               // BEACON CALL SIGN

        public string[] Beacon_Country = new string[]
        { "USA1", "CANADA", "USA6", "HAWAII", "NEW ZEALAND", "AUSTRALIA", //  "USA1", "CANADA", "USA6", "HAWAII", "NEW ZEALAND", "Australia",
           "JAPAN", "RUSSIA", "HONG KONG", "SRI LANKA", "S.AFRICA", "KENYA", //  "Japan", "RUSSIA", "HONG KONG", "SRI LANKA", "S.AFRICA", "KENYA",
            "ISRAEL", "FINLAND",  "MADEIRA", "ARGENTINA", "PERU", "VENEZUELA" };   // "ISRAEL", "FINLAND",  "MADEIRA", "ARGENTINA", "PERU", "VENEZUELA" // BEACON COUNTRY

        public string[] Beacon_Grid = new string[] 
        { "FN30", "EQ78", "CM97", "BL10", "RE78", "OF87",
            "PM84", "NO14", "OL72", "MJ96", "KG44", "KI88",
            "KM72", "KP20", "IM12", "GF05", "FH17", "FJ69" };             // BEACON GRID LOCATION

        public double[] Beacon_Lat = new double[] 
        { 40.75, 79.978, 37.145, 20.77, -41.06, -32.105,
            34.436, 54.978, 22.27, 6.895, -25.896, -1.23,
            32.06, 62.989, 32.728, -34.645, -12.063, 9.103 };                       // always 18 stations

        public double[] Beacon_Lon = new double[]
        { -73.96, -85.96, -121.876, -156.376, 175.623, 116.04,
            136.79, 82.873, 114.123, 79.873, 28.29, 36.873,
            34.79, 25.75, -16.793, -58.413, -76.96, -67.793 };

        //S9+10dB 160.0 -63 44 
        //S9 50.2 -73 34 
        //S8 25.1 -79 28 
        //S7 12.6 -85 22 
        //S6 6.3 -91 16 
        //S5 3.2 -97 10 
        //S4 1.6 -103 4 
        //S3 0.8 -109 -2 
        //S2 0.4 -115 -8 
        //S1 0.2 -121 -14 

        private bool beacon = false; // true = pause DX spotting while doing a beacon test (3min max)
        public bool beacon1 = false; // flag storage for beacon loaded up or not true=beacon test now running
        public bool beacon2 = false; // flag storage for sp8_active
        public bool beacon3 = false; // flag storage for was map already on or not
        public bool beacon4 = false; // true = need map update now.
        public int beacon5 = 0;       // 1-5 indicates which fast scan freq your on 1 to 5, 6=done with 1 slot (18 slots total)
        public int beacon6 = 0;       // counter to try and ignore the pulse in the signal that happens when you change bands

        public int beacon11 = 0;       // 1-5 indicates which slow scan freq your on 1 to 5, 6=done with 1 slot (18 slots total)
        public int beacon12 = 0;       // 1-18 indicates which beacon slot (station) your looking at currently
        public int[] beacon13 = new int[100]; // slow beacon scan place holder

        public int beacon14 = 0;     // prior time slot position in slot stack (for updating the spotter information)
        public int beacon15 = 0;     // cause a 1 cycle delay in startup on SLOW scan routine

        public bool beacon16 = false; // true=PTT was diable prior to running a beacon chk, false = PPT was not disabled prior to running beacon chk

        public DSPMode beacon7;       // to store prior operating mode before running beacon scan
        public int beacon8 = 0;       // to store prior high filter before running beacon scan
        public int beacon9= 0;        //to store prior low filter before running beacon scan
        public Filter beacon89;       // to store filter name before running beacon scan
        public int beacon77 = 600;    // to store cw pitch before running beacon scan

        public int oldSR = 48000;     // to store original SR

        public bool beacon10 = false; //true indicates op mode has changed (ie scan was run)

        //=====================================================================
        private void btnBeacon_Click(object sender, EventArgs e)
        {
            if (beacon == true)
            {
                beacon = false;
                btnBeacon.Text = "Beacon Chk";
                btnBeacon.ForeColor = Color.Black;

                stopWatch.Stop();
                stopWatch1.Stop();

                Debug.WriteLine(">>>>>>>>BEACON: TURN BACK OFF");


            }
            else // if the beacon chk was OFF,then turn it on
            {
            
                beacon = true;
                btnBeacon.Text = "Beacon Run";
                btnBeacon.ForeColor = Color.Red;

                beacon11 = 0; // reset freq for slow scan since you may have changed the freq
                beacon12 = 0; // reset the station your looking at


                Debug.WriteLine(">>>>>>>>BEACON: START");

                //--------------------------------------
                // turn on mapping if it wasnt on when you click on the beacon button

                if (SP5_Active == 0)  // if OFF then turn ON
                {
                    Debug.WriteLine(">>>>>>>>BEACON: turn on mapping");

                    if (chkPanMode.Checked == true) Display.map = 1;
                    else Display.map = 0;

                //    btnTrack.Text = "Track ON";

                    LastDisplayMode = Display.CurrentDisplayMode; // save the display mode that you were in before you turned on special panafall mode

                    if (chkPanMode.Checked == true) Display.CurrentDisplayMode = DisplayMode.PANAFALL;


                    Display.GridOff = 1; // Force Gridlines off but dont change setupform setting

                    if ((chkSUN.Checked == true) || (chkGrayLine.Checked == true))
                    {

                        if (Skin1 == null) Skin1 = console.picDisplay.BackgroundImage;
                        console.picDisplay.SizeMode = PictureBoxSizeMode.StretchImage;

                        if (MAP == null)
                        {
                            if (Console.DXR == 0) console.picDisplay.BackgroundImage = Image.FromStream(Map_image);
                            else console.picDisplay.BackgroundImage = Image.FromStream(Map_image2);
                            //  console.picDisplay.BackgroundImage = Image.FromStream(Map_image);
                        }
                        else console.picDisplay.BackgroundImage = MAP;

                    }

                    Thread t = new Thread(new ThreadStart(TrackSun));

                    t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                    t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");


                    SP5_Active = 1;
                    t.Name = "Track Thread";
                    t.IsBackground = true;
                    t.Priority = ThreadPriority.Normal;
                    t.Start();

                    textBox1.Text = "Clicked to Turn on GrayLine Sun Tracker\r\n";

                    Debug.WriteLine(">>>>>>>>BEACON:  mapping turned on");

                    btnTrack.Text = "Track-ON";


                }//  if (SP5_Active == 0) map was off (above) so turn it on 
                else
                {
                    beacon3 = true; // map is already on
                }


             
            } //  if (beacon != true) from here and above is all part of the Beacon ON button clicking

            //------------------------------------------------
            //------------------------------------------------
            //------------------------------------------------
            //------------------------------------------------

            if (beacon == true) // you just licked to turn on beacon scan
            {

                Sun_Top1 = 26;                                     // 45 Y pixel location of top of map
                Sun_Bot1 = 465;                                    // 485 Y pixel locaiton of bottom of map 
                Sun_Left = 57;                                       // Left side at equator used by Grayline routine
                Sun_Right = 939;

                int Sun_WidthY1 = Sun_Bot1 - Sun_Top1;             // # of Y pixels from top to bottom of map
                int Sun_Width = Sun_Right - Sun_Left;              //used by sun track routine

                Debug.WriteLine(">>>>>>>>BEACON: start loading BX");

                int x1 = 0;

                for (int x = 0; x < 18; x++)
                {

                    if (BX_Load == false)
                    {
                        BX_Time[x * 5 + 4] = BX_Time[x * 5 + 3] = BX_Time[x * 5 + 2] = BX_Time[x * 5 + 1] = BX_Time[x * 5] = UTCNEW;

                        BX_TSlot[x * 5] = 0;        // #=how many times the station was checked  (0=never checked yet, gray) (1=checked color = signal)
                        BX_TSlot[x * 5 + 1] = 0;   // 
                        BX_TSlot[x * 5 + 2] = 0;   // 
                        BX_TSlot[x * 5 + 3] = 0;   // 
                        BX_TSlot[x * 5 + 4] = 0;   // 

                        BX_Freq[x * 5] = Beacon_Freq[0];                          // Beacon freq in hz
                        BX_Freq[x * 5 + 1] = Beacon_Freq[1];                      // Beacon freq in hz
                        BX_Freq[x * 5 + 2] = Beacon_Freq[2];                      // Beacon freq in hz
                        BX_Freq[x * 5 + 3] = Beacon_Freq[3];                      // Beacon freq in hz
                        BX_Freq[x * 5 + 4] = Beacon_Freq[4];                      // Beacon freq in hz

                        BX_Station[x * 5 + 4] = Beacon_Call[x] + " 10m";          // Beacon station name
                        BX_Station[x * 5 + 3] = Beacon_Call[x] + " 12m";          // Beacon station name
                        BX_Station[x * 5 + 2] = Beacon_Call[x] + " 15m";          // Beacon station name
                        BX_Station[x * 5 + 1] = Beacon_Call[x] + " 17m";          // Beacon station name
                        BX_Station[x * 5] = Beacon_Call[x] + " 20m";              // Beacon station name

                        BX_Spotter[x * 5 + 4] = BX_Spotter[x * 5 + 3] = BX_Spotter[x * 5 + 2] = BX_Spotter[x * 5 + 1] = BX_Spotter[x * 5] = callBox.Text;               // PowerSDR callsign station (spotter)
                        BX_Message[x * 5 + 4] = BX_Message[x * 5 + 3] = BX_Message[x * 5 + 2] = BX_Message[x * 5 + 1] = BX_Message[x * 5] = "NCDXF/IARU Beacon";        // message field
                        BX_Mode[x * 5 + 4] = BX_Mode[x * 5 + 3] = BX_Mode[x * 5 + 2] = BX_Mode[x * 5 + 1] = BX_Mode[x * 5] = 1;                                    // operating mode (cw), 
                        BX_Mode2[x * 5 + 4] = BX_Mode2[x * 5 + 3] = BX_Mode2[x * 5 + 2] = BX_Mode2[x * 5 + 1] = BX_Mode2[x * 5] = 0;                                // operating mode2  (split) no

                        BX_Grid[x * 5 + 4] = BX_Grid[x * 5 + 3] = BX_Grid[x * 5 + 2] = BX_Grid[x * 5 + 1] = BX_Grid[x * 5] = Beacon_Grid[x];                       // Beacon Grid location 
                        BX_country[x * 5 + 4] = BX_country[x * 5 + 3] = BX_country[x * 5 + 2] = BX_country[x * 5 + 1] = BX_country[x * 5] = Beacon_Country[x];        // Beacon Country
                        BX_Beam[x * 5 + 4] = BX_Beam[x * 5 + 3] = BX_Beam[x * 5 + 2] = BX_Beam[x * 5 + 1] = BX_Beam[x * 5] = BeamHeading(Beacon_Lat[x], Beacon_Lon[x]); // Beam heading to Beacon from spotter station

                        BX_Y[x * 5 + 4] = BX_Y[x * 5 + 3] = BX_Y[x * 5 + 2] = BX_Y[x * 5 + 1] = BX_Y[x * 5] = (int)(((180 - (Beacon_Lat[x] + 90)) / 180.0) * Sun_WidthY1) + Sun_Top1;   //latitude 90N to -90S

                        BX_X[x * 5 + 4] = BX_X[x * 5 + 3] = BX_X[x * 5 + 2] = BX_X[x * 5 + 1] = BX_X[x * 5] = (int)(((Beacon_Lon[x] + 180.0) / 360.0) * Sun_Width) + Sun_Left;         // longitude -180W to +180E

                        //-------------------------------------------
                        // this below does not need to be swapped in/out of DX_

                        BX_dBm[x * 5] = -150; // signal strength reading for station & freq
                        BX_dBm[x * 5 + 1] = -150; // signal strength reading for station & freq
                        BX_dBm[x * 5 + 2] = -150; // signal strength reading for station & freq
                        BX_dBm[x * 5 + 3] = -150; // signal strength reading for station & freq
                        BX_dBm[x * 5 + 4] = -150; // signal strength reading for station & freq


                        BX_dBm1[x * 5] = -150; // signal strength reading for station & freq
                        BX_dBm1[x * 5 + 1] = -150; // signal strength reading for station & freq
                        BX_dBm1[x * 5 + 2] = -150; // signal strength reading for station & freq
                        BX_dBm1[x * 5 + 3] = -150; // signal strength reading for station & freq
                        BX_dBm1[x * 5 + 4] = -150; // signal strength reading for station & freq

                        x1 = x * 10;
                        BX_TSlot1[x * 5] = x1;     // time slot for this station on 14mhz

                        x1 += 10;
                        if (x1 > 170) x1 = 0;
                        BX_TSlot1[x * 5 + 1] = x1;     // time slot for this station on 18mhz

                        x1 += 10
                            ;
                        if (x1 > 170) x1 = 0;
                        BX_TSlot1[x * 5 + 2] = x1;     // time slot for this station on 21mhz

                        x1 += 10;
                        if (x1 > 170) x1 = 0;
                        BX_TSlot1[x * 5 + 3] = x1;     // time slot for this station on 24mhz

                        x1 += 10;
                        if (x1 > 170) x1 = 0;
                        BX_TSlot1[x * 5 + 4] = x1;     // time slot for this station on 28mhz

                        BX_FULLSTRING[x * 5] = "DX de " + (callBox.Text + ": ").PadRight(11) + (((float)Beacon_Freq[0] / 1e3).ToString("f1")).PadRight(9) + Beacon_Call[x].PadRight(13) + "NCDXF/IARU Beacon     " + "- NA" + " dBm " + BX_Time[x * 5].ToString("D4") + "z " + Beacon_Grid[x];
                        BX_FULLSTRING[x * 5 + 1] = "DX de " + (callBox.Text + ": ").PadRight(11) + (((float)Beacon_Freq[1] / 1e3).ToString("f1")).PadRight(9) + Beacon_Call[x].PadRight(13) + "NCDXF/IARU Beacon     " + "- NA" + " dBm " + BX_Time[x * 5 + 1].ToString("D4") + "z " + Beacon_Grid[x];
                        BX_FULLSTRING[x * 5 + 2] = "DX de " + (callBox.Text + ": ").PadRight(11) + (((float)Beacon_Freq[2] / 1e3).ToString("f1")).PadRight(9) + Beacon_Call[x].PadRight(13) + "NCDXF/IARU Beacon     " + "- NA" + " dBm " + BX_Time[x * 5 + 2].ToString("D4") + "z " + Beacon_Grid[x];
                        BX_FULLSTRING[x * 5 + 3] = "DX de " + (callBox.Text + ": ").PadRight(11) + (((float)Beacon_Freq[3] / 1e3).ToString("f1")).PadRight(9) + Beacon_Call[x].PadRight(13) + "NCDXF/IARU Beacon     " + "- NA" + " dBm " + BX_Time[x * 5 + 3].ToString("D4") + "z " + Beacon_Grid[x];
                        BX_FULLSTRING[x * 5 + 4] = "DX de " + (callBox.Text + ": ").PadRight(11) + (((float)Beacon_Freq[4] / 1e3).ToString("f1")).PadRight(9) + Beacon_Call[x].PadRight(13) + "NCDXF/IARU Beacon     " + "- NA" + " dBm " + BX_Time[x * 5 + 4].ToString("D4") + "z " + Beacon_Grid[x];

                    } // BX_Load = false
           
                } // for loop. load up data on all 18 Beacon stations

                Debug.WriteLine(">>>>>>>>BEACON: loaded up BX");

                BX_Load = true; // flag to not reload again

                if (SP8_Active == 0) // check if dxloc.txt loaded into memory already
                {
                    beacon2 = false;
                    SP8_Active = 1; // fake it for the red dots
                }
                else beacon2 = true; // dxloc was already loaded so SP8_active is 1
                 
                BX1_Index = 90; // this is always 90 unless they change the number of beacons

                beacon1 = true; // flag it so we know we ran a beacon check

                Debug.WriteLine(">>>>>>>>BEACON: ALL LOADED UP");

                //-----------------------------------------------------
                // THREAD START UP FOR CHECKING TIME SLOT

                Thread t = new Thread(new ThreadStart(BeaconSlot));

                t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

                SP5_Active = 1;
                t.Name = "Beacon slot tracking";
                t.IsBackground = true;
                t.Priority = ThreadPriority.Normal;
                t.Start();

                textBox1.Text = "Clicked on Beacon Tracking\r\n";

                processTCPMessage1();

            } //  if (beacon == true)

            //---------------------------------------------------------------
            //---------------------------------------------------------------
            //---------------------------------------------------------------
            //---------------------------------------------------------------
            //---------------------------------------------------------------
            // beacon scanner turned OFF
            else
            {
                if (beacon2 == false)
                {
                    SP8_Active = 0; // sp8_active as originally 0, so return it to 0
                    Debug.WriteLine(">>>>>>>>BEACON: SP8 was originaly 0 so return back to 0");
                }

                Debug.WriteLine(">>>>>>>>BEACON: TURN BACK OFF2");

                beacon4 = true; // do map update

                beacon1 = false; // turn beacon back off

                //------------------------------------------------------

                if (beacon3 == false) // map was off so turn back off
                {

                    SP5_Active = 0;                     // turn off tracking

                    Display.map = 0;                    // tell display program to got back to standard panafall mode

                    if (chkPanMode.Checked == true) Display.CurrentDisplayMode = LastDisplayMode;

                    if (console.setupForm.gridBoxTS.Checked == true) Display.GridOff = 1; // put gridlines back the way they were
                    else Display.GridOff = 0; // gridlines ON

                    btnTrack.Text = "Track";

                    textBox1.Text += "Click to turn off GrayLine Sun Tracking\r\n";

                    if (Skin1 != null) console.picDisplay.BackgroundImage = Skin1; // put back original image

                } // map was off so turn back off after doen with beacon

                beacon4 = true; // do map update

                processTCPMessage();


            }//  beacon button clicked OFF


        } // btnBeacon_Click()  (Beacon Check on/off)




        //==============================================================================
        //==============================================================================
        //==============================================================================
        // BEACON Scanner ON/OFF  (Fast or Slow)
        //==============================================================================
        //==============================================================================
        //==============================================================================
        // ke9ns THREAD

        // 18 Stations scan 5 Frequencies on 10sec intervals (starting with 4U1UN and 14.1 mhz)
        // So it takes 4U1UN 50 seconds to complete its transmission and then waits silent for the remaining 130 seconds

        // The listener can sit on 14.1 and here 18 stations (in succession) for a total of 3 minutes
        // That would take a total of 20minutes to hear all 18 stations on all 5 frequencies

        // The listener can pick 1 station and jump to all 5 frequencies when its that stations time (starting at 14.1 mhz)
        // That would take a total of 3 minutes to hear but may require up to 3min to start, for a total of 6minutes worst case

        Stopwatch stopWatch = new Stopwatch();
        TimeSpan ts;

        Stopwatch stopWatch1 = new Stopwatch();
        TimeSpan ts1;

        double tsTime = 0;
        double LasttsTime = 0;

        double tsTime1 = 0;                                             // replaced beacon6 for ant switch glitch which causes S meter spike
        double BandSwitchDelay = 0.24;                                   // replaced beacon6 amount of delay to get past the ant switch glitch

        public static string SEC1;                                       // get 24hr 4 digit UTC NOW
      
        public static int SECNEW1;                                       // convert 24hr UTC to int
        public static int seconds;
        public static int minutes;                                       // get 24hr 4 digit UTC NOW
        public static int Last_seconds;
        public static int Totseconds;
        public static int SlotSeconds;  // conversion of mmss down to 3min intervals
        public static int TSlot; // 10 second intervals
        public static int Last_TSlot; // 10 second intervals
        public static int Last_TSlot1; // 10 second intervals for slow scan

        //========================================================================================== 
        private void BeaconSlot()
        {
            Debug.WriteLine(">>>>>>>>BEACON:  thread started");

            UTCD = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            SEC1= UTCD.ToString("mm:ss");
            minutes = Convert.ToInt16( SEC1.Substring(0, 2));
            seconds = Convert.ToInt16(SEC1.Substring(3, 2));
   
            // seconds = TimeSpan.Parse(SEC1).TotalSeconds; // get total seconds of the day
            // example: 23:19 = 23min 19sec = min/3 = modulo of 2minutes into a 3 min block + 19sec = 139 seconds into a 180second block

            SlotSeconds = ((minutes % 3)*60)+seconds;
            TSlot = (SlotSeconds / 10) * 10;

            Debug.WriteLine(">>>TIME1: THREAD START TIME " + SEC1);
            Debug.WriteLine(">>>TIME1: THREAD START TIME in minutes " + minutes);
            Debug.WriteLine(">>>TIME1: THREAD START TIME in seconds " + seconds);
            Debug.WriteLine(">>>TIME1: THREAD START TIME in total seconds per 3 minute intervals: " + SlotSeconds);
            Debug.WriteLine(">>>TIME1: THREAD START TIME in total seconds per 3 minute intervals/10: " + TSlot);


            //---------------------------------------------------------------------
            for (int x = 0; x < 18;x++) // find starting station then BX_Index will keep track
            {
             
                if ((BX_TSlot1[x*5] >= TSlot) && (BX_TSlot1[x * 5] < TSlot+10))
                {
                    BX_Index[0] = x * 5; // this is the start index
                    Debug.WriteLine(">>>TIME1: Current BX_Index[0] 14mhz station# " +  BX_Index[0] + " , "+BX_Station[BX_Index[0]]);
                }
                else if ((BX_TSlot1[x * 5 + 1] >= TSlot) && (BX_TSlot1[x * 5 + 1] < TSlot + 10))
                {
                    BX_Index[1] = x * 5+1; // this is the start index
                    Debug.WriteLine(">>>TIME1: Current BX_Index[1] 18mhz station# " + BX_Index[1] + " , " + BX_Station[BX_Index[1]]);
                }
                else if ((BX_TSlot1[x * 5 + 2] >= TSlot) && (BX_TSlot1[x * 5 + 2] < TSlot + 10))
                {
                    BX_Index[2] = x * 5+2; // this is the start index
                    Debug.WriteLine(">>>TIME1: Current BX_Index[2] 21mhz station# " + BX_Index[2] + " , " + BX_Station[BX_Index[2]]);
                }
                else if ((BX_TSlot1[x * 5 + 3] >= TSlot) && (BX_TSlot1[x * 5 + 3] < TSlot + 10))
                {
                    BX_Index[3] = x * 5+3; // this is the start index
                    Debug.WriteLine(">>>TIME1: Current BX_Index[3] 24mhz station#  " + BX_Index[3] + " , " + BX_Station[BX_Index[3]]);
                }
                else if ((BX_TSlot1[x * 5 + 4] >= TSlot) && (BX_TSlot1[x * 5 + 4] < TSlot + 10))
                {
                    BX_Index[4] = x * 5+4; // this is the start index
                    Debug.WriteLine(">>>TIME1: Current BX_Index[4] 28mhz station# " + BX_Index[4] + " , " + BX_Station[BX_Index[4]]);
                }

            } // for loop of 90

            Last_TSlot1 = BX_TSlot2 = Last_TSlot = TSlot;

          
            beacon4 = true;
            beacon5 = 0;


            beacon7 = console.RX1DSPMode;           // get mode so you can restore it when you turn off the beacon check
            beacon8 = console.RX1FilterHigh;        // get high filter so you can restore it when you turn off the beacon check
            beacon9 = console.RX1FilterLow;         // get low filter so you can restore it when you turn off the beacon check
            beacon89 = console.RX1Filter;           // get filter name so you can restore
            beacon77 = (int)console.udCWPitch.Value;     // get filter name so you can restore

          
            //-----------------------------------------------------------------------
            //-----------------------------------------------------------------------
            while ( (beacon1 == true) ) // only do while the beacon testing is goin on
            {
                Thread.Sleep(10); // slow down the thread here

                UTCD = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                SEC1 = UTCD.ToString("mm:ss");
                minutes = Convert.ToInt16(SEC1.Substring(0, 2));
                seconds = Convert.ToInt16(SEC1.Substring(3, 2));


                 SlotSeconds = ((minutes % 3) * 60) + seconds;
                 TSlot = (SlotSeconds / 10) * 10;


                if (Last_TSlot != TSlot)
                {
                    Last_TSlot = TSlot; // update 1 time per 10seconds

                 //  Debug.WriteLine(">>>TIME2: THREAD START TIME in total seconds per 3 minute intervals/10: " + TSlot);

                    for (int x = 0; x < 18; x++) // find starting station then BX_Index will keep track
                    {

                        if ((BX_TSlot1[x * 5] >= TSlot) && (BX_TSlot1[x * 5] < TSlot + 10))
                        {
                            BX_Index[0] = x * 5; // this is the start index
                           Debug.WriteLine(">>>TIME2: Current BX_Index[0] 14mhz station# " + BX_Index[0] + " , " + BX_Station[BX_Index[0]]);

                        }
                        else if ((BX_TSlot1[x * 5 + 1] >= TSlot) && (BX_TSlot1[x * 5 + 1] < TSlot + 10))
                        {
                            BX_Index[1] = x * 5 + 1; // this is the start index
                            Debug.WriteLine(">>>TIME2: Current BX_Index[1] 18mhz station# " + BX_Index[1] + " , " + BX_Station[BX_Index[1]]);

                        }
                        else if ((BX_TSlot1[x *5 + 2] >= TSlot) && (BX_TSlot1[x * 5 + 2] < TSlot + 10))
                        {
                            BX_Index[2] = x * 5 + 2; // this is the start index
                           Debug.WriteLine(">>>TIME2: Current BX_Index[2] 21mhz station# " + BX_Index[2] + " , " + BX_Station[BX_Index[2]]);

                        }
                        else if ((BX_TSlot1[x * 5 + 3] >= TSlot) && (BX_TSlot1[x * 5 + 3] < TSlot + 10))
                        {
                            BX_Index[3] = x * 5 + 3; // this is the start index
                          Debug.WriteLine(">>>TIME2: Current BX_Index[3] 24mhz station#  " + BX_Index[3] + " , " + BX_Station[BX_Index[3]]);

                        }
                        else if ((BX_TSlot1[x * 5 + 4] >= TSlot) && (BX_TSlot1[x * 5 + 4] < TSlot + 10))
                        {
                            BX_Index[4] = x * 5 + 4; // this is the start index
                           Debug.WriteLine(">>>TIME2: Current BX_Index[4] 28mhz station# " + BX_Index[4] + " , " + BX_Station[BX_Index[4]]);

                        }

                    } // for loop of 90 

                    //   Debug.WriteLine(">>>TIME: New SLot: " + UTCD);
                    BX_TSlot2 = TSlot;

                    beacon4 = true; // force map update

                    if (BoxBScan.Checked == true) // fast 3 minute complete scan (5 freq over 18 periods)
                    {
                        LasttsTime = 0;   // time period for fast scanning each of 5 frequencies
                        stopWatch.Restart(); // reset every time Slot (10 seconds)

                      
                        if (beacon5 == 0) // only do one time
                        {
                    
                            
                            beacon5 = 1; // scan all 5 freq fast fast
                            beacon10 = true; // you will put back original op mode when done with scan
                            Debug.WriteLine(">>>BEACON5 RESET..................... ");

                            beacon11 = 0;
                            beacon12 = 0;
                            beacon14 = 0;
                        }
                        else if (beacon5 == 7) beacon5 = 1; // scan next Tslot as long as the checkbox is still checked

                    }
                    else if (BoxBFScan.Checked == true) // Long slow 15 minute complete scan (1 freq over 18 periods, 5 times)
                    {
                        if (beacon11 == 0) // only do one time
                        {
                            beacon11 = (int)numericUpDownTS1.Value; // 1-5  scan all 5 freq fast fast (6=done)  (normally set to 1

                            stopWatch.Stop();
                            stopWatch1.Stop();

                            beacon12 = 0; // 1-18 scan all 18 slots (stations) for 5 times
                            beacon5 = 0; // turn off fast scanner
                            beacon10 = true; // you will put back original op mode when done with scan
                            beacon14 = 0; // slot position holder (0-85 = start of each station in stack )
                            beacon15 = 0; // reset
                        }
                    }
                    else // if both check boxes are off
                    {
                        beacon5 = 0;  // clear all scanner functions since your not scanning
                        beacon11 = 0; // 1-5 freq
                        beacon12 = 0; // 1-18 slots
                        beacon14 = 0;

                        stopWatch.Stop();
                        stopWatch1.Stop();

                        if (beacon10 == true) // put back original op mode, now that the beacon scanner was turned from ON to OFF
                        {
                            console.udCWPitch.Value = beacon77;     // restore cw pitch value
                            console.RX1DSPMode = beacon7;           //  restore  mode  when you turn off the beacon check
                            console.RX1Filter = beacon89;           // restore filter name
                            console.RX1FilterHigh = beacon8;        // restore high filter  when you turn off the beacon check
                            console.RX1FilterLow = beacon9;         // restore low filter  when you turn off the beacon check
                            
                            beacon10 = false;
                        }
                    }

                    Last_seconds = 1000; // reset so the first time through the scanner it selects a freq

                } // new TSLOT came up 



                //----------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------
                // ke9ns  add SLOW beacon scan
                // BX_Index[0] = is the slot (0 to 89) up now on 14mhz
                // BX_Index[1] = is the slot (0 to 89) up now on 18mhz, etc., etc.
                // beacon13[] = 0 to 89 slots (0=14m, 1=18m, 2=21m, 3=24m, 4=28mhz repeating)(with every 5th slot is next station)

                if (beacon11 > 0) // do a slow scan
                {
                  
                    if (Last_TSlot1 != TSlot)
                    {
                        Last_TSlot1 = TSlot; // update 1 time per 10seconds

                       
                        if (beacon11 < 6) // scan through all 5 beacon freq
                        {

                            // set mode and freq
                            if (console.RX1DSPMode != DSPMode.CWU) console.RX1DSPMode = DSPMode.CWU;
                            if (console.udCWPitch.Value != 600)   console.udCWPitch.Value = 600;
                            if (console.RX1Filter != Filter.VAR1) console.RX1Filter = Filter.VAR1;
                            if (console.RX1FilterHigh != 650) console.RX1FilterHigh = 650;   // set filter for narrow area around the target freq
                            if (console.RX1FilterLow != 550) console.RX1FilterLow = 550;

                            console.VFOAFreq = (double)Beacon_Freq[beacon11 - 1] / 1e6; // shift 0hz down 600 for cw mode and convert to MHZ

                            Debug.WriteLine(">>>freq:beacon11, BX_Index[beacon11 - 1] , beacon14: " + beacon11+" , "+ BX_Index[beacon11 - 1] + " , " + beacon14);

                            //------------------------------------------

                            if (beacon15 > 0) // start processing after the 1st 10 seconds of each freq change
                            {
                                if (BX_dBm[beacon14] >= -73)      BX_dBm3[beacon14] = 9;
                                else if (BX_dBm[beacon14] >= -79) BX_dBm3[beacon14] = 8;
                                else if (BX_dBm[beacon14] >= -85) BX_dBm3[beacon14] = 7;
                                else if (BX_dBm[beacon14] >= -91) BX_dBm3[beacon14] = 6;
                                else if (BX_dBm[beacon14] >= -97) BX_dBm3[beacon14] = 5;
                                else if (BX_dBm[beacon14] >= -103) BX_dBm3[beacon14] = 4;
                                else if (BX_dBm[beacon14] >= -109) BX_dBm3[beacon14] = 3;
                                else if (BX_dBm[beacon14] >= -115) BX_dBm3[beacon14] = 2;
                                else if (BX_dBm[beacon14] >= -121) BX_dBm3[beacon14] = 1;
                                else BX_dBm3[beacon14] = 0;

                              
                                if ((BX_dBm1[beacon14] - BX_dBm[beacon14]) > -1)
                                {
                                  //  Debug.WriteLine("dbm " + BX_dBm1[beacon14] + " , " + BX_dBm[beacon14]);

                                    BX_dBm1[beacon14] = -151; // noise floor
                                    BX_dBm[beacon14] = -150; // signal 
                                }

                                //  Debug.WriteLine("Slow: Call, freq, dbm, S, noise floor: " + BX_Station[ beacon14] + " , " + BX_Freq[ beacon14] + " , " + BX_dBm[ beacon14] + " , " + BX_dBm3[ beacon14] + " , " + BX_dBm1[ beacon14]); // 20m

                                BX_TSlot[beacon14] = 1; // set indicator for panadapter display

                                BX_FULLSTRING[ beacon14] = "DX de " + (callBox.Text + ": ").PadRight(11) + (((float)BX_Freq[ beacon14] / 1e3).ToString("f1")).PadRight(9) + BX_Station[ beacon14].PadRight(13) + "NCDXF/IARU Beacon  " + "S" + BX_dBm3[ beacon14] + " " + (BX_dBm1[beacon14] - BX_dBm[beacon14]).ToString("D3") + " dBm " + BX_Time[ beacon14].ToString("D4") + "z " + BX_Grid[beacon14];

                                processTCPMessage1(); // update dx spotter window
                           

                            } // if beacon15 > 0

                            beacon15 = 1;       // skip first time through after a freq change

                            beacon14 = BX_Index[beacon11 - 1];

                            beacon12++;         // increment station counter every 10 sec (make sure we scan all 18 stations)

                            if (beacon12 == 18) // when we have all 18 stations then go to next freq
                            {
                               
                                beacon12 = 0;   // reset counter
                                beacon11++;     // go to next freq the the slot

                                beacon6 = 0;   // reset noise pulse ignore
                                stopWatch1.Stop();
                                stopWatch1.Reset();

                                              
                                Debug.WriteLine(">>>RESET BEACON11: "+beacon11);

                                if (beacon11 == 6)
                                {
                                    Debug.WriteLine(">>>SLOW BEACON DONE<<<<<<<<<<<<<<<<<<<<<<");

                                    BoxBFScan.Checked = false; // turn off slow scan
                                    BoxBScan.Checked = false; // turn off slow scan

                                    beacon12 = 0;
                                    beacon11 = 0;
                                    beacon14 = 0;
                                    beacon15 = 0;

                                }

                            } // if beacon12  18 stations (slots)


                        } // if beacon11 < 6  5 freq
                        else // when done scanning all 5 freq
                        {
                            Debug.WriteLine(">>>SLOW BEACON DONE<<<<<<<<<<<<<<<<<<<<<<");

                            BoxBFScan.Checked = false; // turn off slow scan
                            BoxBScan.Checked = false; // turn off slow scan

                            beacon12 = 0;
                            beacon11 = 0;
                        }

                    } // if (last TSLOT != TSLOT)  10 second intervals
                    else // search for a signal
                    {

                        if (BoxBFScan.Checked == true)
                        {
                            stopWatch1.Start();

                            ts1 = stopWatch1.Elapsed;
                            tsTime1 = (double)ts1.Seconds + ((double)ts1.Milliseconds / 1000.0);

                            if (tsTime1 >= BandSwitchDelay)    // (beacon6 > 25) // wait for band switching pulse to disapate
                            {

                                int tempDB = console.ReadAvgStrength(0);
                               
                                if (tempDB > BX_dBm[beacon14])
                                {
                                    BX_dBm[beacon14] = tempDB; // get signal strengh avg reading to match avg floor reading
                                }
                                if (BX_dBm2 > BX_dBm1[beacon14] )
                                    BX_dBm1[beacon14] = BX_dBm2; // value passed back from display.cs noise floor (avg value)

                              //  WWVThreshold = BX_dBm1[beacon14]; // display.cs the floor
                            }
                            else   beacon6++;

                        }

                    } // in between TSLOT changes (in between 10 second slots)



                } // if (beacon11 > 0) slow scan


                //----------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------
                // ke9ns  add FAST beacon scan

                if (beacon5 > 0)
                {
                 
                    ts = stopWatch.Elapsed;
                    tsTime = (double)ts.Seconds + ((double)ts.Milliseconds / 1000.0);

                    if (tsTime >= LasttsTime )
                    {

                      //   Debug.WriteLine("RunTime1 " + tsTime);

                        LasttsTime = LasttsTime + 1.15;  // FAST SCAN time delay of 1.4 seconds

                        if (beacon5 < 6)
                        {
                            // set mode and freq
                            if (console.RX1DSPMode != DSPMode.CWU) console.RX1DSPMode = DSPMode.CWU;
                            if (console.udCWPitch.Value != 600) console.udCWPitch.Value = 600;
                            if (console.RX1Filter != Filter.VAR1) console.RX1Filter = Filter.VAR1;
                            if (console.RX1FilterHigh != 650) console.RX1FilterHigh = 650;   // set filter for narrow area around the target freq
                            if (console.RX1FilterLow != 550) console.RX1FilterLow = 550;


                            console.VFOAFreq = (double)Beacon_Freq[beacon5 - 1] / 1e6; //  convert to MHZ
                            beacon6 = 0; // reset noise pulse ignore
                            stopWatch1.Stop();
                            stopWatch1.Reset();

         
                        }

                        if (beacon5 == 6)
                        {


                            //S9+10dB 160.0 -63 44 
                            //S9 50.2 -73 34 
                            //S8 25.1 -79 28 
                            //S7 12.6 -85 22 
                            //S6 6.3 -91 16 
                            //S5 3.2 -97 10 
                            //S4 1.6 -103 4 
                            //S3 0.8 -109 -2 
                            //S2 0.4 -115 -8 
                            //S1 0.2 -121 -14 

                            for (int u = 0; u < 5; u++)
                            {
                                if (BX_dBm[BX_Index[u]] >= -73) BX_dBm3[BX_Index[u]] = 9;
                                else if (BX_dBm[BX_Index[u]] >= -79) BX_dBm3[BX_Index[u]] = 8;
                                else if (BX_dBm[BX_Index[u]] >= -85) BX_dBm3[BX_Index[u]] = 7;
                                else if (BX_dBm[BX_Index[u]] >= -91) BX_dBm3[BX_Index[u]] = 6;
                                else if (BX_dBm[BX_Index[u]] >= -97) BX_dBm3[BX_Index[u]] = 5;
                                else if (BX_dBm[BX_Index[u]] >= -103) BX_dBm3[BX_Index[u]] = 4;
                                else if (BX_dBm[BX_Index[u]] >= -109) BX_dBm3[BX_Index[u]] = 3;
                                else if (BX_dBm[BX_Index[u]] >= -115) BX_dBm3[BX_Index[u]] = 2;
                                else if (BX_dBm[BX_Index[u]] >= -121) BX_dBm3[BX_Index[u]] = 1;
                                else BX_dBm3[BX_Index[u]] = 0;


                                if ((BX_dBm1[BX_Index[u]] - BX_dBm[BX_Index[u]]) > -1)
                                {
                                     Debug.WriteLine("dbm " + BX_dBm1[BX_Index[u]] + " , " + BX_dBm[BX_Index[u]]);

                                    BX_dBm1[BX_Index[u]] = -151; // noise floor
                                    BX_dBm[BX_Index[u]] = -150; // signal 
                                }
                            } // for loop all 5 freq

                            //   Debug.WriteLine(">>>beacon12: " + beacon12);

                            //    Debug.WriteLine("Call, freq, dbm, S, noise floor: " + BX_Station[BX_Index[0]] + " , " + BX_Freq[BX_Index[0]] + " , " + BX_dBm[BX_Index[0]] + " , " + BX_dBm3[BX_Index[0]] + " , " + BX_dBm1[BX_Index[0]]); // 20m
                            //    Debug.WriteLine("Call, freq, dbm, S, noise floor: " + BX_Station[BX_Index[1]] + " , " + BX_Freq[BX_Index[1]] + " , " + BX_dBm[BX_Index[1]] + " , " + BX_dBm3[BX_Index[1]] + " , " + BX_dBm1[BX_Index[1]]); // 17m
                            //   Debug.WriteLine("Call, freq, dbm, S, noise floor: " + BX_Station[BX_Index[2]] + " , " + BX_Freq[BX_Index[2]] + " , " + BX_dBm[BX_Index[2]] + " , " + BX_dBm3[BX_Index[2]] + " , " + BX_dBm1[BX_Index[2]]); // 15m
                            //    Debug.WriteLine("Call, freq, dbm, S, noise floor: " + BX_Station[BX_Index[3]] + " , " + BX_Freq[BX_Index[3]] + " , " + BX_dBm[BX_Index[3]] + " , " + BX_dBm3[BX_Index[3]] + " , " + BX_dBm1[BX_Index[3]]); //12m
                            //   Debug.WriteLine("Call, freq, dbm, S, noise floor: " + BX_Station[BX_Index[4]] + " , " + BX_Freq[BX_Index[4]] + " , " + BX_dBm[BX_Index[4]] + " , " + BX_dBm3[BX_Index[4]] + " , " + BX_dBm1[BX_Index[4]]); //10m


                            BX_TSlot[BX_Index[4]] = BX_TSlot[BX_Index[3]] = BX_TSlot[BX_Index[2]] = BX_TSlot[BX_Index[1]] = BX_TSlot[BX_Index[0]] = 1; //  // set indicator for panadapter display

                            BX_FULLSTRING[BX_Index[0]] = "DX de " + (callBox.Text + ": ").PadRight(11) + (((float)BX_Freq[BX_Index[0]] / 1e3).ToString("f1")).PadRight(9) + BX_Station[BX_Index[0]].PadRight(13) + "NCDXF/IARU Beacon  " + "S" + BX_dBm3[BX_Index[0]] + " " + (BX_dBm1[BX_Index[0]] - BX_dBm[BX_Index[0]]).ToString("D3") + " dBm " + BX_Time[BX_Index[0]].ToString("D4") + "z " + BX_Grid[BX_Index[0]];
                            BX_FULLSTRING[BX_Index[1]] = "DX de " + (callBox.Text + ": ").PadRight(11) + (((float)BX_Freq[BX_Index[1]] / 1e3).ToString("f1")).PadRight(9) + BX_Station[BX_Index[1]].PadRight(13) + "NCDXF/IARU Beacon  " + "S" + BX_dBm3[BX_Index[1]] + " " + (BX_dBm1[BX_Index[1]] - BX_dBm[BX_Index[1]]).ToString("D3") + " dBm " + BX_Time[BX_Index[1]].ToString("D4") + "z " + BX_Grid[BX_Index[1]];
                            BX_FULLSTRING[BX_Index[2]] = "DX de " + (callBox.Text + ": ").PadRight(11) + (((float)BX_Freq[BX_Index[2]] / 1e3).ToString("f1")).PadRight(9) + BX_Station[BX_Index[2]].PadRight(13) + "NCDXF/IARU Beacon  " + "S" + BX_dBm3[BX_Index[2]] + " " + (BX_dBm1[BX_Index[2]] - BX_dBm[BX_Index[2]]).ToString("D3") + " dBm " + BX_Time[BX_Index[2]].ToString("D4") + "z " + BX_Grid[BX_Index[2]];
                            BX_FULLSTRING[BX_Index[3]] = "DX de " + (callBox.Text + ": ").PadRight(11) + (((float)BX_Freq[BX_Index[3]] / 1e3).ToString("f1")).PadRight(9) + BX_Station[BX_Index[3]].PadRight(13) + "NCDXF/IARU Beacon  " + "S" + BX_dBm3[BX_Index[3]] + " " + (BX_dBm1[BX_Index[3]] - BX_dBm[BX_Index[3]]).ToString("D3") + " dBm " + BX_Time[BX_Index[3]].ToString("D4") + "z " + BX_Grid[BX_Index[3]];
                            BX_FULLSTRING[BX_Index[4]] = "DX de " + (callBox.Text + ": ").PadRight(11) + (((float)BX_Freq[BX_Index[4]] / 1e3).ToString("f1")).PadRight(9) + BX_Station[BX_Index[4]].PadRight(13) + "NCDXF/IARU Beacon  " + "S" + BX_dBm3[BX_Index[4]] + " " + (BX_dBm1[BX_Index[4]] - BX_dBm[BX_Index[4]]).ToString("D3") + " dBm " + BX_Time[BX_Index[4]].ToString("D4") + "z " + BX_Grid[BX_Index[4]];

                            processTCPMessage1(); // update dx spotter window

                             stopWatch.Stop(); // stop to reset
                             stopWatch1.Stop(); // stop to reset


                            beacon12++;
                            if (beacon12 == 18)
                            {
                                Debug.WriteLine(">>>TURN OFF beason fast");

                                BoxBScan.Checked = false; // turn off slow scan
                                BoxBFScan.Checked = false;
                                beacon12 = 0;
                                beacon11 = 0;
                                beacon5 = 0;
                            }
                            else
                            {
                                beacon5 = 7;
                                console.VFOAFreq = (double)Beacon_Freq[0] / 1e6; // reset to start beacon freq


                            }



                        } // if beacon5 == 6
                        else
                        {
                            if (beacon5 < 6)  beacon5++; // if not == 6 

                        }

                    } // seconds != lastseconds
                    else // search for a signal
                    {

                        stopWatch1.Start();
                        ts1 = stopWatch1.Elapsed;
                        tsTime1 = (double)ts1.Seconds + ((double)ts1.Milliseconds / 1000.0);

                        if ((beacon5 < 7) &&  (tsTime1 >= BandSwitchDelay))          //(beacon6 > 25)) // wait for band switching pulse to disapate
                        {
    
                            int tempDB = console.ReadAvgStrength(0);

                            if (tempDB > BX_dBm[beacon14])
                            {
                                BX_dBm[BX_Index[beacon5 - 2]] = tempDB; // get signal strengh avg reading to match avg floor reading
                            }

                            if (BX_dBm2 > BX_dBm1[BX_Index[beacon5 - 2]] )
                               BX_dBm1[BX_Index[beacon5 - 2]] = BX_dBm2;                           // this is the noise Floor value passed back from Display to spot.cs

                         //   WWVThreshold = BX_dBm1[BX_Index[beacon5 - 2]]; // display.cs floor

                        } //   wait for band switching pulse to disapate
                        
                        beacon6++;


                    } // in between each 1 second interval

                } // beacon5 > 0


            } //  while ( beacon1 == true)

            Debug.WriteLine(">>>>>>>>BEACON:  thread STOPPED");
            beacon5 = 0;

            stopWatch.Stop();
            stopWatch1.Stop();

            if (beacon10 == true)
            {
                console.udCWPitch.Value = beacon77;     // restore cw pitch
                console.RX1DSPMode = beacon7;           //  restore  mode  when you turn off the beacon check
                console.RX1Filter = beacon89;
                console.RX1FilterHigh = beacon8;        // restore high filter  when you turn off the beacon check
                console.RX1FilterLow = beacon9;         // restore low filter  when you turn off the beacon check
                
            }

        } //  private void BeaconSlot()

        private void BoxBFScan_CheckedChanged(object sender, EventArgs e)
        {
            BoxBScan.Checked = false;
            beacon11 = 0; // reset freq for slow scan since you may have changed the freq
            beacon12 = 0; // reset the station your looking at

            if (BoxBFScan.Checked == true)
            {
                for (int x = 0; x < 18; x++)  // reset time and reset back to GRAY since your now running a new test
                {
                    BX_Time[x * 5 + 4] = BX_Time[x * 5 + 3] = BX_Time[x * 5 + 2] = BX_Time[x * 5 + 1] = BX_Time[x * 5] = UTCNEW;

                    BX_TSlot[x * 5] = 0;        // #=how many times the station was checked  (0=never checked yet, gray) (1=checked color = signal)
                    BX_TSlot[x * 5 + 1] = 0;   // 
                    BX_TSlot[x * 5 + 2] = 0;   // 
                    BX_TSlot[x * 5 + 3] = 0;   // 
                    BX_TSlot[x * 5 + 4] = 0;   // 
                }
            }

        } // BoxBFScan_CheckedChanged

        private void BoxBScan_CheckedChanged(object sender, EventArgs e)
        {
            BoxBFScan.Checked = false;

            if (BoxBScan.Checked == true)
            {
                for (int x = 0; x < 18; x++)  // reset time and reset back to GRAY since your now running a new test
                {
                    BX_Time[x * 5 + 4] = BX_Time[x * 5 + 3] = BX_Time[x * 5 + 2] = BX_Time[x * 5 + 1] = BX_Time[x * 5] = UTCNEW;

                    BX_TSlot[x * 5] = 0;        // #=how many times the station was checked  (0=never checked yet, gray) (1=checked color = signal)
                    BX_TSlot[x * 5 + 1] = 0;   // 
                    BX_TSlot[x * 5 + 2] = 0;   // 
                    BX_TSlot[x * 5 + 3] = 0;   // 
                    BX_TSlot[x * 5 + 4] = 0;   // 
                }
            }
        } // BoxBScan_CheckedChanged

        //====================================================================================================================
        //====================================================================================================================
        //====================================================================================================================
        // ke9ns Grab NIST Internet Time
        //====================================================================================================================
        //====================================================================================================================
        //====================================================================================================================
        public void SetInternetTime()
        {
           
           DateTime startDT = DateTime.Now; 
           
            //Create a IPAddress object and port, create an IPEndPoint node:  
            int port = 13;
            string[] whost = { "time-nw.nist.gov", "time.windows.com", "time.nist.gov",  "time-a.nist.gov", "time-b.nist.gov", "tick.mit.edu",  "clock.sgi.com" };

            IPHostEntry iphostinfo;
            IPAddress ip;
            IPEndPoint ipe;
            Socket c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//Create Socket  

            c.ReceiveTimeout = 100 * 1000;    //Setting the timeout  

            byte[] RecvBuffer = new byte[1024];

            int nBytes = 0;
            int nTotalBytes = 0;

            StringBuilder sb1 = new StringBuilder();
            StringBuilder sb = new StringBuilder();

            System.Text.Encoding myE = Encoding.UTF8;

            string EX1 = " ";
            TimeSpan k = new TimeSpan();
            TimeSpan k1 = new TimeSpan();

            SystemTime st = new SystemTime();

            DateTime SetDT = DateTime.Now;

            startDT = DateTime.Now; // record time you opened a connection to NIST

            foreach (string strHost in whost)   // try all the time servers until you get a response
            {
                try
                {
                    iphostinfo = Dns.GetHostEntry(strHost);
                    ip = iphostinfo.AddressList[0];
                    ipe = new IPEndPoint(ip, port);

                    c.Connect(ipe);     // Connect to server which starts clock (NIST will now send back the correct Time)

                    if (c.Connected)
                    {
                      
                        Debug.WriteLine("got connection to " + strHost);
                        break;// If the connection to the server is out of 
                       
                    } 
                }
                catch (Exception ex)
                {
                    EX1 = ex.Message;
                    Debug.WriteLine("SOCKET ERROR: "+strHost + " , " + ex);

                }
            } // for loop through time server addresses

            if (!c.Connected)
            {
                MessageBox.Show("Time server connection failed! /r error: " + EX1, " the system prompts", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

           //----------------------------------------------------------------
           // get NIST formated time
            while ((nBytes = c.Receive(RecvBuffer, 0, 1024, SocketFlags.None)) > 0)
            {
                nTotalBytes += nBytes;
                sb.Append(myE.GetString(RecvBuffer, 0, nBytes));
            }

            c.Close(); // close the socket

            // example of downloaded time sync from NIST
            // <cr>57682 16-10-21 14:42:46 17 0 0 159.1 UTC(NIST) * 
            // o[0] = <cr>57682   MJD day since 4713 BC
            // o[1] = 16-10-21    date yy-mm-dd UTC
            // o[2] = 14:42:46    time hh:mm:ss UTC
            // o[3] = 17          0=ST 50=DT (in between indicates the number of days remaining in DT)
            // o[4] = 0           Leap second 0=no 1=leap second added at end of month (61 seconds), 2=leap second subtracted at end of month (59 seconds)
            // o[5] = 0           UT1
            // o[6] = 159.1       MSadv  milliseconds advance (if you send back the * to NIST, NIST will return the OTM # with the correct MSAdv value)
            // o[7] = *           OTM on time marker  #=ACTS successfully calibrated the path


            try
            {

                string[] o = sb.ToString().Split(' '); // Cut the string  

                string temp2 = o[6].Substring(0, 3); // get millisecond delay

                string temp1 = o[1] + " " + o[2] + ".000";

                SetDT = DateTime.ParseExact(temp1, "yy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
              
                k1 = TimeSpan.FromMilliseconds(Convert.ToInt32(temp2)); // convert NIST delay to k1 timespan
                k = (TimeSpan)(DateTime.Now - startDT);   // get the delay since we read the time from NIST (milliseconds)

                k1 = k1 - k; // Set your PC time based on time reported by NIST - the time it took to receive that time. 


                SetDT = Convert.ToDateTime(SetDT).Subtract(k1); // subtract this k1 value since NIST always reports a future time
              
                SetDT = SetDT.ToLocalTime(); // adjust UTC back to my time .NOW

                st.FromDateTime(SetDT); //Convert System.DateTime to SystemTime 
                Win32API.SetLocalTime(ref st);  //Call Win32 API to set the system time  

                textBox1.Text = "IMPORTANT: Your PC Time will NOT update unless PowerSDR is launched in ADMIN mode!!!!" + "\r\n" + 
                                "PC TIME when Request sent to NIST: " + startDT.ToString("yy-MM-dd HH:mm:ss.fff") + "\r\n" +
                                "UTC TIME reported back from NIST : " + temp1 + "\r\n" +
                                "NIST reported this time: " + temp2 + " milliseconds Early" + "\r\n" +
                                "Time Delay: From request to Update is:" + k + " milliseconds" + "\r\n" +
                                "DONE: PC new time: " + SetDT.ToString("yy-MM-dd HH:mm:ss.fff");


                Debug.WriteLine("DONE...Time delay from request to update:" + k + " milliseconds");
            }
            catch (Exception e4)
            {
                MessageBox.Show("Time server connection failed! Try Again.", " the system prompts", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
           
        //    MessageBox.Show("Time synchronization, ", " the system prompts", MessageBoxButtons.OK, MessageBoxIcon.Information);

        } // SetInternetTime()


      
        //====================================================================================================================
        //====================================================================================================================
        //====================================================================================================================

       
        // ke9ns request to update this PC's time clock
        private void btnTime_Click(object sender, EventArgs e)
        {

            if (checkBoxWWV.Checked == true)
            {
                Thread t = new Thread(new ThreadStart(WWVTime));

                t.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                t.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

                WTime = true;   // enabled (let display know to get a Floor dbm

                SP5_Active = 1;
                t.Name = "WWV Time Sync";
                t.IsBackground = true;
                t.Priority = ThreadPriority.AboveNormal;
                t.Start();

                textBox1.Text = "Will Attempt to read Radio Station WWV !\r\n";
            }
            else
            {
                SetInternetTime();
                WTime = false;
            }

        } // btnTime_Click

    
        double[] WWV_Freq = { 2.500100, 5.000100, 10.000100, 15.000100 };  // listen to 100hz tone
        double[] WWV_Freq1 = { 2.5, 5.0, 10.0, 15.0 };                     // listen to 1000khz tone

        public bool WTime = false;
     
        Stopwatch tickON = new Stopwatch();        // WWV 100hz tick ON elapsed time to find if PCM BCD data stream pluse is 1 or 0
        Stopwatch tickOFF = new Stopwatch();       // WWV 100hz tick OFF elapsed time to find start of minute (HOLE)

        public Stopwatch WWVNewTime = new Stopwatch();    // This timer starts when a P frame is detected. If the HOLE is detected immediately after, then this time + .3sec is used as the marker

        DateTime WWVNT = DateTime.Now;

        public int WWVThreshold = -150; // the trip point where the PCM BCD data stream from WWV determines a 1 or 0

        public int indexP = 0;  // P frame index (with 10 seconds inside it)
        public int indexS = 0; // seconds index inside a P frame

        //====================================================================================================================
        //====================================================================================================================
        //====================================================================================================================
        //====================================================================================================================
        // Thread
        // ke9ns Read WWV for Time Sync
        unsafe private void WWVTime()
        {
         
            beacon7 = console.RX1DSPMode;           // get mode so you can restore it when you turn off the beacon check
            beacon8 = console.RX1FilterHigh;        // get high filter so you can restore it when you turn off the beacon check
            beacon9 = console.RX1FilterLow;         // get low filter so you can restore it when you turn off the beacon check
            beacon89 = console.RX1Filter;           // get filter name so you can restore
            beacon77 = (int)console.udCWPitch.Value; // get filter name so you can restore

            oldSR = console.SampleRate1;            // get SR

           

            if (oldSR == 192000)  // 192kSR will not work so reduce to 96k
            {
                Debug.WriteLine("orig SR: " + oldSR);

               console.setupForm.comboAudioSampleRate1.Text = "96000"; // select 96000

                Debug.WriteLine("orig SR Changed to 96k ");


            }

       
            console.RX1Filter = Filter.VAR1;
            console.RX1DSPMode = DSPMode.DIGU;

            console.RX1FilterHigh = 30;          // set filter around the 100hz seconds pulse
            console.RX1FilterLow = -30;


            console.VFOAFreq = WWV_Freq[(int)udDisplayWWV.Value-1];         // WWV in CWU mode will center on 5000.1 khz

            console.chkEnableMultiRX.Checked = true;  // enable sub receiver
            console.VFOBFreq = WWV_Freq1[(int)udDisplayWWV.Value - 1];       // WWV in CWU mode will center on 5000 khz

            textBox1.Text += "Waiting for Start of Minute!\r\n";

            Debug.WriteLine("WWV Time Thread running ");

           
            int BCDSignal= 0;              // measured BCD data stream dBm signal
            int CarrierSignal = 0;         // measured Carrier dBm signal

          
          
            bool BCD1timeFlag = false;

            bool BCDONTrig = false;
            bool BCDOFFTrig = false;

            int tickTimeON = 0;
           
            int tickTimeOFF = 0;
          
            tickON.Reset();
            tickOFF.Reset();

          
          
            int CarrierSignalINIT = 0;         // measured signal floor around the WWV signal

            int BCDSignalON = -150;           // BCD data steam high dbm signal found initially
            int BCDSignalOFF = 0;             // BCD data steam low dbm signal found initially

            int BCDSignalON1 = -150;         // BCD data steam high dbm signal found while running
            int BCDSignalOFF1 = 0;           // BCD data steam low dbm signal found while running

            int WWVCF = 0;               // fault counter for low signal strength fault

          
            int[,] P = new int[7, 20]; // 6 Position Identifiers with 10 seconds inside each Position frame 

            int newMinutes = 0;
            int newHours = 0;

         
            int BCD1 = 0; // false BCD value detected as (0), true BCD value detected as (1)   [for this last second]

            bool WWVStart = false; // true = got start of minute frame
            bool WWVStop = false; // true = got entire 1 minute frame
            bool[] WWVFault = { false, false, false, false, false, false }; // true = bad data bit somewhere in WWV frames
            bool WWVPos = false;  // true = indicates you got a Position indicator frame at least 1 time before you got the HOLE (i.e. before WWVStart == true)

            TimeSpan k1 = new TimeSpan();

            SystemTime st = new SystemTime();

            WWVCF = 0;

            Stopwatch ST = new Stopwatch();
            ST.Reset();
            ST.Start();

            BCDSignalOFF = 0;

            CarrierSignalINIT = 0;
            Debug.WriteLine("WWV>>1");
            while (ST.ElapsedMilliseconds < 2000) // get floor for bcd stream
            {
                CarrierSignalINIT = 0;
                BCDSignalOFF = 0;

            }
            ST.Restart();

            Debug.WriteLine("WWV>>2");
            while ( ST.ElapsedMilliseconds < 1300)                          // get floor for bcd stream
            {
                BCDSignal = console.ReadStrength(0);
                CarrierSignal = console.ReadStrength(1);

                if ((BCDSignal < BCDSignalOFF) && (BCDSignal > -120))       // check low dBm value
                {
                    BCDSignalOFF = BCDSignal;                               // finding the OFF state of this bcd stream area if the WWV signal
                    CarrierSignalINIT = CarrierSignal;                      // find the carrier level at the same time
                }
                if ((BCDSignal > BCDSignalON) && (BCDSignal > -120))        // check high dBm value
                {
                    BCDSignalON = BCDSignal;                                // finding the ON state of this bcd stream area if the WWV signal
                }

            } // for loop 1.3 seconds to test levels

            ST.Stop();
            Debug.WriteLine("WWV>>3");

            Debug.WriteLine("WWV>> Lowest BCD ,  Carrier " + BCDSignalOFF + " , " + CarrierSignalINIT);

            textBox1.Text += "WWV BCD Data stream: (0)= " + BCDSignalOFF + "dBm, (1)= " + BCDSignalON + "dBm @ Carrier Level: " + CarrierSignalINIT + "dBm\r\n";

            ST.Restart();

            //---------------------------------------------------------------
            //---------------------------------------------------------------
            //---------------------------------------------------------------
            //---------------------------------------------------------------
            while (WTime == true)
            {

                //  tempDB3 = BX_dBm2;                  // get floor

                BCDSignal = console.ReadStrength(0);            // read wwv 100 hz OFF of carrier point (BCD data stream)            
                CarrierSignal = console.ReadStrength(1);        // read WWV 0hz carrier point
       

                //------------------------------------------------------------------
                // keep adjusting signal based on signal strength you are seeing

                if ((BCDSignal < BCDSignalOFF1))   // check low dBm value
                {
                    BCDSignalOFF1 = BCDSignal;                           // finding the OFF state of this bcd stream area if the WWV signal
                   
                }
                if ((BCDSignal > BCDSignalON1))    // check high dBm value
                {
                    BCDSignalON1 = BCDSignal;                            // finding the ON state of this bcd stream area if the WWV signal
                }

                if (ST.ElapsedMilliseconds > 1300)
                {
                    CarrierSignalINIT = CarrierSignal;
                    BCDSignalOFF = BCDSignalOFF1;
                    BCDSignalON = BCDSignalON1;
                   
                    BCDSignalON1 = -150;         // RESET BCD data steam high dbm signal found while running
                    BCDSignalOFF1 = 0;           // RESET BCD data steam low dbm signal found while running
                    ST.Restart();

                 //   textBox1.Text += "WWV BCD Data stream: (0)= " + BCDSignalOFF + "dBm, (1)= " + BCDSignalON + "dBm @ Carrier Level: " + CarrierSignal + "dBm\r\n";

                }

                //-------------------------------------------------
                // check the carrier signal strength 

                //   WWVFloor = tempDB3;


                  
                    if ((uint)(BCDSignalON - BCDSignalOFF) < 6) // if you loose the carrier, then NO GOOD
                    {
                        if (WWVCF > 30) // FAIL if carrier stays LOW for too long
                        {
                            textBox1.Text += "\r\n";
                            textBox1.Text += "Radio Station WWV: Carrier signal too low, choose different Frequency\r\n";

                            indexP = 0;     // reset data to catch next minute data stream
                            indexS = 0;
                            WWVPos = false;
                            WWVStart = false;
                            WWVStop = false;
                            WWVFault[0] = WWVFault[1] = WWVFault[2] = WWVFault[3] = WWVFault[4] = WWVFault[5] = false;
                            WTime = false;
                        }
                        else WWVCF++;

                    }
                    else
                    {
                        WWVCF = 0;
                    }
    

              
                WWVThreshold =   BCDSignalOFF + (3*(BCDSignalON - BCDSignalOFF) / 7); // adjust the threshold based on the last seconds ON/OFF dBm values
            
                WWVThreshold = WWVThreshold + ((CarrierSignal - CarrierSignalINIT)/3); // adjust the threshold based on the last seconds Carrier dBm values


                //------------------------------------------------------
                //------------------------------------------------------
                //------------------------------------------------------
                //------------------------------------------------------
                // WWVStart == TRUE when Receive the HOLE signal, but WWVNewTime timer started at end of P0 BCD Tone.
                // SO WWVNewTime timer started 230 milliseconds early AND now receiving the second #1 (always a short BCD Tone), but wont come until second # 2

                if (WWVStart == true)   // do below if got HOLE (start of new minute) WWVNewTime timer is running from 0 second
                {
                   
                    // the extra 230 is for the extra time starting WWVNewTime at the end of P0 to the start of the new Minute Second#0

                    if (WWVNewTime.ElapsedMilliseconds >= ( 230 + (indexS + (indexP*10))*1000 )   )   // 1000,2000,3000,4000,5000 milliseconds, etc
                    {
                        if (ST.IsRunning) ST.Stop();   // turn off init threshold timer
                      
                        CarrierSignalINIT = CarrierSignal;  // set new ON/OFF and Carrier levels at the start of every second
                        BCDSignalOFF = BCDSignalOFF1;
                        BCDSignalON = BCDSignalON1;

                        BCDSignalON1 = -150;         // RESET BCD data steam high dbm signal found while running
                        BCDSignalOFF1 = 0;           // RESET BCD data steam low dbm signal found while running
                       

                        if (indexS == 10) // 9, 19,29,39, etc
                        {
                            BCD1 = 0;  // reset value

                            indexP++;
                            textBox1.Text += " P"+(indexP + 1)+">";
                            indexS = 1;
                        }
                        else // first 9 seconds of every P frame
                        {

                            if (BCD1 > 1)   // BCD tone was med ( 440 msec)
                            {
                                BCD1 = 0;  // reset value
     
                                P[indexP, indexS-1] = 1;
                                textBox1.Text += "1";
                                
                            }
                            else // BCD tone was short ( 170 msec)
                            {
                                BCD1 = 0;  // reset value

                                P[indexP, indexS-1] = 0;
                                textBox1.Text += "0";
                            }

                            indexS++; // index the second marker

                        }  // first 9 seconds of every P frame

                        tickON.Restart();  // we no we will get a BCD tone, but for how long: 170msec= 0, 440msec = 1, or 770msec if it a P frame

                    } //if (WWVNewTime.ElapsedMilliseconds >= (indexS * 1000))

                    //--------------------------------------------------------------
                    if (indexP == 3)
                    {

                        if ((WWVFault[1] == true) || (WWVFault[2] == true))
                        {
                            textBox1.Text += "\r\n";
                            textBox1.Text += "Radio Station WWV: Data No Good, will Try again.\r\n";

                            indexP = 0;     // reset data to catch next minute data stream
                            indexS = 0;
                            WWVCF = 0;
                            WWVPos = false;
                            WWVStart = false;
                            WWVStop = false;
                            WWVFault[0] = WWVFault[1] = WWVFault[2] = WWVFault[3] = WWVFault[4] = WWVFault[5] = false;
                            WWVNewTime.Stop();

                            // DO OVER AGAIN UNTIL YOU GET GOOD DATA
                        } // fault detected in PCM BCD data stream

                        else // no faults detected in PCM BCD data stream
                        {
                            newMinutes = (P[1, 0] * 1) + (P[1, 1] * 2) + (P[1, 2] * 4) + (P[1, 3] * 8) + (P[1, 5] * 10) + (P[1, 6] * 20) + (P[1, 7] * 40);
                            newHours = (P[2, 0] * 1) + (P[2, 1] * 2) + (P[2, 2] * 4) + (P[2, 3] * 8) + (P[2, 5] * 10) + (P[2, 6] * 20);

                            WWVStop = true;
                            WTime = false;  // DONE
                            WWVPos = false;

                            string ww1 = newHours.ToString("D2") + ":" + newMinutes.ToString("D2") + ":00.000";
                            DateTime WWVUTC = new DateTime();

                            try
                            {
                                WWVUTC = DateTime.ParseExact(ww1, "HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                            }
                            catch(Exception)
                            {
                                textBox1.Text += "WWV time not read correctly. Will Try again.\r\n";
                                WWVNewTime.Stop();
                                indexP = 0;     // reset data to catch next minute data stream
                                indexS = 0;
                                WWVCF = 0;
                                WWVPos = false;
                                WWVStart = false;
                                WWVStop = false;
                                WWVFault[0] = WWVFault[1] = WWVFault[2] = WWVFault[3] = WWVFault[4] = WWVFault[5] = false;
                                goto EXITOUT;
                            }

                            textBox1.Text += "\r\n";
                            textBox1.Text += "DONE: Radio Station WWV UTC Time: " + ww1 + "\r\n";

                            //-------------------------------------
                            // now computer real time and save it.
                            textBox1.Text += "IMPORTANT: Your PC Time will NOT update unless PowerSDR is launched in ADMIN mode!!!!" + "\r\n";


                            DialogResult temp0 = MessageBox.Show("You must be running in ADMIN mode to set your PC Clock.\r\nDo You Want to Update Your PC Clock?\r\n" +
                                "This is the reported WWV UTC Time > " + ww1 + "\r\n An additional correction factor will be added if you select YES",
                                "WWV PC TIME UPDATE", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);

                            if (temp0 == DialogResult.Yes)
                            {
                                // update PC time here

                                textBox1.Text += "Elapsed time since WWV Sync Pulse> " + WWVNewTime.Elapsed + "\r\n";

                                k1 = TimeSpan.FromMilliseconds(Convert.ToInt32(300)); // convert o k1 timespan

                                WWVUTC = WWVUTC.Subtract(k1); // subtract the 300 millseconds from the HOLE
                                WWVUTC = WWVUTC + WWVNewTime.Elapsed;

                                textBox1.Text += "New Time updated to your PC clock> " + WWVUTC + "\r\n";

                                WWVUTC = WWVUTC.ToLocalTime(); // adjust UTC back to my time .NOW

                                st.FromDateTime(WWVUTC); //Convert System.DateTime to SystemTime 
                                Win32API.SetLocalTime(ref st);  //Call Win32 API to set the system time  

                                textBox1.Text += "New Time updated to your PC clock1> " + WWVUTC + "\r\n";
                                WWVNewTime.Stop();

                            } // user wants you to update PC time based on WWV radio results
                            else
                            {
                                // do not update PC time
                                textBox1.Text += "OK. PC Time Clock NOT Updated.\r\n";
                                WWVNewTime.Stop();

                            }


                        } // NO faults detected in PCM BCD data stream

                    } // indexP == 3 (got the hours and minutes)



                } //if (WWVStart == true) 

                EXITOUT:


                //---------------------------------------------------------------
                //---------------------------------------------------------------
                // use Threshold to check 100hz subcarrier for BCD data stream
                //---------------------------------------------------------------
                //---------------------------------------------------------------

                if (WWVStart == true)
                {
                    if (BCDSignal >= WWVThreshold)             // this should be a 1 in the BCD data stream (or a P Frame signal)
                    {
                        // we already know the BCD tone is already ON
                    }
                    else
                    {
                     
                        if ((int)tickON.ElapsedMilliseconds < 60)
                        {
                           // keep going because the min BCD length is 170msec, we must have the threshold too high or lost signal in the noise

                        }
                        else
                        {
                            BCDONTrig = true; // do just 1 time per tick
                            tickON.Stop();
                            tickTimeON = (int)tickON.ElapsedMilliseconds;

                        }

                    }

                    if (BCDONTrig == true)
                    {
                        BCDONTrig = false; // do just 1 time per tick


                        if ((tickTimeON > 280) && (tickTimeON < 660))  // .440 seconds duration: weighted code digit (1)
                        {
                            WWVPos = false;

                            if (WWVStart == true)
                            {
                                BCD1 = 2;
                            }

                        }
                        else if ((tickTimeON > 60) && (tickTimeON <= 280))  // .170 seconds duration: (0) unweighted code digit, index marker, and unweighted control element
                        {
                            WWVPos = false;
                            if (WWVStart == true)
                            {
                                BCD1 = 1;
                            }
                        }
                        else
                        {
                            WWVPos = false;
                            if (WWVStart == true)
                            {
                                WWVFault[indexP] = false;
                                //  textBox1.Text += "F";
                            }
                        }


                    } // BCDONTrig == true


                } //  if (WWVStart == true)
                else   // do below only to find HOLE(SYNC) to start minute
                {
                    if (BCDSignal >= WWVThreshold)             // this should be a 1 in the BCD data stream (or a P Frame signal)
                    {
                        tickOFF.Stop();

                        if (BCD1timeFlag == false)
                        {
                            tickTimeOFF = (int)tickOFF.ElapsedMilliseconds; // 

                            BCDOFFTrig = true; // got an TICK TIMEOFF
                            BCDONTrig = false; // reset ON timer

                            BCD1timeFlag = true; // 1 time flag

                            tickON.Start();   // first time here, start timer to see how long the BCD tone lasts

                            tickOFF.Reset();  // we are no longer looking at an off signal

                        }
                    }
                    else  // no TONE  this should be a 0 in the BCD data stream
                    {

                        tickON.Stop();

                        if (BCD1timeFlag == true)
                        {
                            tickTimeON = (int)tickON.ElapsedMilliseconds;
                            BCD1timeFlag = false;

                            BCDOFFTrig = false;
                            BCDONTrig = true;     // GOT A TICK TIMEON 

                            tickON.Reset();
                            tickOFF.Start();

                        }

                    } //if (BCDSignal >= WWVThreshold) NO


                    if (BCDONTrig == true)
                    {
                        BCDONTrig = false; // do just 1 time per tick

                        if ((tickTimeON >= 660) && (tickTimeON < 900))       // .770 seconds duration: position identifier (P0-P5)  (i.e. every 10 seconds)
                        {
                            if (WWVStart == false)
                            {
                                WWVPos = true; // allow detected of HOLE
                                WWVNewTime.Restart(); // zero and Start the stopwatch just in case this is actually the sync pulse (i.e. the HOLE is next)
                                textBox1.Text += "Pframe,";
                            }

                        } // P Frame above

                    } //  if (BCDONTrig == true)

                    //-----------------------------------------------------------------------
                    // LOOK FOR HOLE TO START WWV MINUTE
                    if (BCDOFFTrig == true)
                    {
                        BCDOFFTrig = false;

                        if ((WWVPos == true)  && (tickTimeOFF > 1010) && (tickTimeOFF < 1700))  // Long HOLE indicates start of new minute
                        {

                            Debug.WriteLine("WWV TIME>> Position HOLE: Start of new MINUTE============");

                            textBox1.Text += "\r\n";
                            textBox1.Text += "WWV>> Got Start of new Minute Sync Pulse\r\n";
                            textBox1.Text += "Frame (1-6)#: P1>";

                            WWVFault[0] = WWVFault[1] = WWVFault[2] = WWVFault[3] = WWVFault[4] = WWVFault[5] = false;

                            WWVStart = true;
                            indexP = 0;
                            indexS = 2; // this next tick will be second # 1 of the P0 Frame
                            BCD1 = 0;
                            P[indexP, 0] = 0;
                            textBox1.Text += "H";
                        }
                        else
                        {
                            if ((WWVPos == true) )  // Long HOLE indicates start of new minute
                            {
                                textBox1.Text += " >> Position SYNC HOLE > 1230 ?: " + tickTimeOFF + "\r\n";
                            }
                            //  Debug.WriteLine("WWV TIME>> Position HOLE?: " + tickTimeOFF);
                            WWVPos = false;
                        }

                    } // BCDONTrig == false

                }  // WWVStart == false

                Thread.Sleep(1);

            } // while(WTime == true)


            //---------------------------------------------------------------
            //---------------------------------------------------------------
            // DONE WITH WWV THREAD HERE
            //---------------------------------------------------------------
            //---------------------------------------------------------------

            Debug.WriteLine("WWV Time Thread Ended ");

            checkBoxWWV.Checked = false; // turn off WWV checking

            if (oldSR == 192000)  // 192kSR will not work so reduce to 96k
            {
                console.setupForm.comboAudioSampleRate1.Text = "192000"; // select 192000 again when done

            }

            console.RX1DSPMode = beacon7;           //  restore  mode  when you turn off the beacon check
            console.RX1Filter = beacon89;           // restore filter name
            console.RX1FilterHigh = beacon8;        // restore high filter  when you turn off the beacon check
            console.RX1FilterLow = beacon9;         // restore low filter  when you turn off the beacon check
            WWVNewTime.Stop();

        } // WWVTime()

        private void checkBoxWWV_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxWWV.Checked == false)
            {
                WTime = false; // if you turn off checkbox, then shut down thread
                WWVNewTime.Stop();
                indexP = 0;     // reset data to catch next minute data stream
                indexS = 0;
               
            }

        }

        private void numericUpDownTS1_ValueChanged(object sender, EventArgs e)
        {
         //   beacon11 = 0; // reset when changing

        }
    } // Spotcontrol


    public class Win32API
    {
        [DllImport("Kernel32.dll")]
        public static extern bool SetLocalTime(ref SystemTime Time);
        [DllImport("Kernel32.dll")]
        public static extern void GetLocalTime(ref SystemTime Time);
    }
/*
    //--------------------------------------------------------------------------
    // ke9ns ?? get OFFSET2 by PLL, adds to the OFFSET1 (found above)
    public void PLL()
    {
        float a, b;
             DttSP.GetSAMPLLvals(0, 0, &a, &b);     // ke9ns save original a and b values to put back after you getfreq


            Debug.WriteLine("CalibrateFreq a, b " + a + " , " + b);
            
            float a1 = a * 0.1f;
             float b1 = 0.25f * a1 * a1;

                 DttSP.SetSAMPLLvals(0, 0, a1, b1);     // ke9ns a1 = 10% of original value, b1= 25% of a1^2

            Thread.Sleep(200);

          //  int counter = 0;
         int samples = 200;
         float sum1 = 0.0f;

            for (int i = 0; i<samples; i++)                     // ke9ns loop 200 times
            {
                float temp;

              DttSP.GetSAMFreq(0, 0, &temp);                   // ke9ns ?? returns a freq

                Debug.WriteLine("CalibrateFreq temp " + temp);

                sum1 += temp;
                Thread.Sleep(50);
              //  progress.SetPercent((float)((float)++counter / samples));
            }



            Debug.WriteLine("CalibrateFreq sum1 " + sum1 );

            diff = -(float)((sum1 / samples) * sample_rate1 / (2 * Math.PI));

            DttSP.SetSAMPLLvals(0, 0, a, b);                    // reset PLL values

            // Calculate the DDS offset
            offset = 0;
            switch (current_model)
            {
                case Model.FLEX5000:
                    offset = (int)(500.0 / freq* diff);
                    break;
                case Model.FLEX3000:
                    offset = (int)(499.201 / freq* diff);
                    break;
                case Model.FLEX1500:
                    if (flex_1500_xref)
                        offset = (int)(400.0 / freq* diff);
                    else offset = (int)(384.0 / freq* diff);
                    break;
                default:
                    offset = (int)(200.0 / freq* diff);
                    break;
            }

            Debug.WriteLine("  offset2: " + offset);

    }

    */
    /*
    private double GoertzelFilter(float[] samples, double freq, int start, int end)
    {
        double sPrev = 0.0;
        double sPrev2 = 0.0;
        int i;
        double normalizedfreq = freq / SIGNAL_SAMPLE_RATE;
        double coeff = 2 * Math.Cos(2 * Math.PI * normalizedfreq);
        for (i = start; i < end; i++)
        {
            double s = samples[i] + coeff * sPrev - sPrev2;
            sPrev2 = sPrev;
            sPrev = s;
        }
        double power = sPrev2 * sPrev2 + sPrev * sPrev - coeff * sPrev * sPrev2;
        return power;
    }

    */
} // powersdr
