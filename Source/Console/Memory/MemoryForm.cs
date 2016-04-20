//=================================================================
// MemoryForm.cs
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace PowerSDR
{
    public partial class MemoryForm : Form
    {
        #region Variable Declaration

        private Console console;

        #endregion

        #region Constructor

        public MemoryForm(Console c)
        {
            InitializeComponent();
            console = c;
            Common.RestoreForm(this, "MemoryForm", true); // ke9ns bring up memory window in place you left it last time

            dataGridView1.RowHeadersVisible = true;

            dataGridView1.DataSource = console.MemoryList.List; // ke9ns get list of memories from memorylist.cs is where the file is opened and saved

            dataGridView1.RowHeadersWidthSizeMode =  DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;

            dataGridView1.ColumnHeadersHeightSizeMode =  DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            dataGridView1.AutoSizeColumnsMode =   DataGridViewAutoSizeColumnsMode.AllCells;

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AutoGenerateColumns = false;

            // Create ComboBox Column
            // DSPMode
            DataGridViewComboBoxColumn comboboxColumnDSPMode = new DataGridViewComboBoxColumn();            
            comboboxColumnDSPMode.DataPropertyName = "DSPMode";
            comboboxColumnDSPMode.Name = "DSPMode";
            comboboxColumnDSPMode.HeaderText = "DSP Mode";
            comboboxColumnDSPMode.ValueType = typeof(DSPMode);

            // Tune Step
            DataGridViewComboBoxColumn comboboxColumnTuneStep = new DataGridViewComboBoxColumn();
            comboboxColumnTuneStep.DataPropertyName = "TuneStep";
            comboboxColumnTuneStep.Name = "TuneStep";
            comboboxColumnTuneStep.HeaderText = "Tune Step";
            comboboxColumnTuneStep.ValueType = typeof(string);
            
            // RPT
            DataGridViewComboBoxColumn comboboxColumnRPTR = new DataGridViewComboBoxColumn();
            comboboxColumnRPTR.DataPropertyName = "RPTR";
            comboboxColumnRPTR.Name = "RPTR";
            comboboxColumnRPTR.HeaderText = "RPTR";
            comboboxColumnRPTR.ValueType = typeof(FMTXMode);

            // CTCSS
            DataGridViewComboBoxColumn comboboxColumnCTCSS = new DataGridViewComboBoxColumn();
            comboboxColumnCTCSS.DataPropertyName = "CTCSSFreq";
            comboboxColumnCTCSS.Name = "CTCSSFreq";
            comboboxColumnCTCSS.HeaderText = "CTCSS Freq";
            comboboxColumnCTCSS.ValueType = typeof(double);            

            // Dev
            DataGridViewComboBoxColumn comboboxColumnDeviation = new DataGridViewComboBoxColumn();
            comboboxColumnDeviation.DataPropertyName = "Deviation";
            comboboxColumnDeviation.Name = "Deviation";
            comboboxColumnDeviation.HeaderText = "Deviation";
            comboboxColumnDeviation.ValueType = typeof(double);

            // Filter
            DataGridViewComboBoxColumn comboboxColumnFilter = new DataGridViewComboBoxColumn();
            comboboxColumnFilter.DataPropertyName = "RXFilter";
            comboboxColumnFilter.Name = "RXFilter";
            comboboxColumnFilter.HeaderText = "RXFilter";
            comboboxColumnFilter.ValueType = typeof(Filter);

            // AGCMode
            DataGridViewComboBoxColumn comboboxColumnAGCMode = new DataGridViewComboBoxColumn();
            comboboxColumnAGCMode.DataPropertyName = "AGCMode";
            comboboxColumnAGCMode.Name = "AGCMode";
            comboboxColumnAGCMode.HeaderText = "AGC Mode";
            comboboxColumnAGCMode.ValueType = typeof(AGCMode);
            

            // populate combobox items -- type is important here!
            comboboxColumnDSPMode.Items.Add(DSPMode.LSB);
            comboboxColumnDSPMode.Items.Add(DSPMode.USB);
            comboboxColumnDSPMode.Items.Add(DSPMode.DSB);
            comboboxColumnDSPMode.Items.Add(DSPMode.CWL);
            comboboxColumnDSPMode.Items.Add(DSPMode.CWU);
            comboboxColumnDSPMode.Items.Add(DSPMode.FM);
            comboboxColumnDSPMode.Items.Add(DSPMode.AM);
            comboboxColumnDSPMode.Items.Add(DSPMode.SAM);
            comboboxColumnDSPMode.Items.Add(DSPMode.SPEC);
            comboboxColumnDSPMode.Items.Add(DSPMode.DIGL);
            comboboxColumnDSPMode.Items.Add(DSPMode.DIGU);
            comboboxColumnDSPMode.Items.Add(DSPMode.DRM);

            for (int i = 0; i < console.TuneStepList.Count; i++)
            {
                comboboxColumnTuneStep.Items.Add(console.TuneStepList[i].Name);
            }

            comboboxColumnRPTR.Items.Add(FMTXMode.High);
            comboboxColumnRPTR.Items.Add(FMTXMode.Simplex);
            comboboxColumnRPTR.Items.Add(FMTXMode.Low);

            for (int i = 0; i < console.CTCSS_array.Length; i++)
                comboboxColumnCTCSS.Items.Add((double)console.CTCSS_array[i]);

            for (int i = 0; i < console.FM_deviation_array.Length; i++)
                comboboxColumnDeviation.Items.Add((int)console.FM_deviation_array[i]);

            for (int i = (int)(Filter.F1); i < (int)Filter.LAST; i++)
                comboboxColumnFilter.Items.Add((Filter)i);

            for (int i = 0; i < (int)AGCMode.LAST; i++)
                comboboxColumnAGCMode.Items.Add((AGCMode)i);  


            // Remove the default DSPMode column (remember index first), and add new combobox column
            int index = dataGridView1.Columns["DSPMode"].Index;
            dataGridView1.Columns.Remove("DSPMode");
            dataGridView1.Columns.Insert(index, comboboxColumnDSPMode);

            index = dataGridView1.Columns["TuneStep"].Index;
            dataGridView1.Columns.Remove("TuneStep");
            dataGridView1.Columns.Insert(index, comboboxColumnTuneStep);

            index = dataGridView1.Columns["RPTR"].Index;
            dataGridView1.Columns.Remove("RPTR");
            dataGridView1.Columns.Insert(index, comboboxColumnRPTR);

            
            index = dataGridView1.Columns["CTCSSFreq"].Index;
            dataGridView1.Columns.Remove("CTCSSFreq");
            dataGridView1.Columns.Insert(index, comboboxColumnCTCSS);

            index = dataGridView1.Columns["Deviation"].Index;
            dataGridView1.Columns.Remove("Deviation");
            dataGridView1.Columns.Insert(index, comboboxColumnDeviation);

            index = dataGridView1.Columns["RXFilter"].Index;
            dataGridView1.Columns.Remove("RXFilter");
            dataGridView1.Columns.Insert(index, comboboxColumnFilter);

            index = dataGridView1.Columns["AGCMode"].Index;
            dataGridView1.Columns.Remove("AGCMode");
            dataGridView1.Columns.Insert(index, comboboxColumnAGCMode);


            // clean up column names for auto-generated fields
            dataGridView1.Columns["RXFreq"].HeaderText = "RX Freq";
            dataGridView1.Columns["RPTROffset"].HeaderText = "RPTR Offset";
            dataGridView1.Columns["CTCSSOn"].HeaderText = "CTCSS";
            dataGridView1.Columns["TXFreq"].HeaderText = "TX Freq";
            dataGridView1.Columns["RXFilterLow"].HeaderText = "RX Filter Low";
            dataGridView1.Columns["RXFilterHigh"].HeaderText = "RX Filter High";
            dataGridView1.Columns["AGCT"].HeaderText = "AGC-T";

            // set the default display for floating point values
            dataGridView1.Columns["RXFreq"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["RXFreq"].DefaultCellStyle.Format = "f6";
            dataGridView1.Columns["TXFreq"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["TXFreq"].DefaultCellStyle.Format = "f6";
            dataGridView1.Columns["RPTROffset"].DefaultCellStyle.Format = "f";

            this.dataGridView1.Columns["Scan"].Visible = false;//

            dataGridView1.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridView1_CellValidating);

      
       
        } //memoryform




        //==============================================================================================
        //ke9ns add AS YOU DRAG YOUR URL ALONG WITH YOUR MOUSE INTO THE MEMORY FORM WINDOW
        public static string URLTEXT; // ke9ns add
        public string droppedUrl;   // ke9ns add
        public static string[] filename; // ke9ns add


        private void dataGridView1_DragEnter(object sender, DragEventArgs e)
        {

          //  Trace.WriteLine("Dragenter");
           
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) // ke9ns check for file dragdrop
            {
              
                e.Effect = DragDropEffects.Copy;
                e.Effect = DragDropEffects.All;

                filename = (string[])e.Data.GetData(DataFormats.FileDrop);
     
                URLTEXT = filename[0]; // grab file name

                return;
            }


            droppedUrl = ReadURL(e.Data); // ke9ns check for URL e.data is the data received during the drag/drop

            if ((droppedUrl != null) && ( droppedUrl.Trim().Length != 0))
            {
               //  Trace.WriteLine("dragdrop URL>" + droppedUrl);
                URLTEXT = droppedUrl;
                e.Effect = DragDropEffects.Link; // must activate the EFFECT before the _dragdrop will work
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }


        } //   Trace.WriteLine("Dragover");


         //==============================================================================================
        //ke9ns add ONCE DRAGENTER VALIDATES YOUR URL, YOU RELEASE YOUR MOUSE OVER THE WINDOW AND COMMENT FIELD IS UPDATED WITH THE URL
        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
        //    Trace.WriteLine("datagrid drag and drop");
       
                dataGridView1["comments",RIndex].Value = URLTEXT;
  
        } // dataGridView1_DragDrop


        //==============================================================================================
        //ke9ns add USED TO OPEN WEB BROWSER if comment field has URL
        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {

            MouseEventArgs me = (MouseEventArgs)e;
         
            if (me.Button == System.Windows.Forms.MouseButtons.Right)
            {
  
                try
                {
                    System.Diagnostics.Process.Start((string)dataGridView1["comments", RIndex].Value);   // System.Diagnostics.Process.Start("http://www.microsoft.com");
                }
                catch
                {
                    // if not a URL then ignore
                }


            } // right mouse button click
            else if (me.Button == System.Windows.Forms.MouseButtons.Left)
            {
                //   Trace.WriteLine("left click ");

            }

        } //dataGridView1_CellMouseDown


        public static int RIndex = 0;
        public static int CIndex = 0;

        //============================================================================================================================================
        // ke9ns add COMES HERE AFTER YOU CLICK ON A FIELD BOX TO DETERMINE WHICH ROW YOU ARE WORKING IN
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        //    Trace.WriteLine("cell click");

       //     Trace.WriteLine("Cell Name " + dataGridView1.Columns[e.ColumnIndex].Name);  // this causes fault if you click in the far left column
       //    Trace.WriteLine("Call Value " + dataGridView1[e.ColumnIndex, e.RowIndex].Value); // 

            RIndex = e.RowIndex; // last row you clicked on 
         //   CIndex = e.ColumnIndex; // last column you clicked on 


        } // dataGridView1_CellClick


        //============================================================================================================================================
         void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
     
            // handle floating point fields
            if (dataGridView1.Columns[e.ColumnIndex].Name == "RXFreq" ||
                dataGridView1.Columns[e.ColumnIndex].Name == "TXFreq" ||
                dataGridView1.Columns[e.ColumnIndex].Name == "RPTROffset")
            {
                double temp; 
                if(!double.TryParse((string)e.FormattedValue, out temp))  dataGridView1[e.ColumnIndex, e.RowIndex].Value = 0.0;
                return;
            }

            // handle int fields
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Power" ||
                dataGridView1.Columns[e.ColumnIndex].Name == "FilterLow" ||
                dataGridView1.Columns[e.ColumnIndex].Name == "FilterHigh" ||
                dataGridView1.Columns[e.ColumnIndex].Name == "AGCT")
            {
                int temp;
                if (!int.TryParse((string)e.FormattedValue, out temp))   dataGridView1[e.ColumnIndex, e.RowIndex].Value = 0;
                return;
            }

      
        } // dataGridView1_CellValidating

        #endregion

        #region Event Handlers


        //============================================================================================================================================
        // ke9ns add YOU DRAG YOUR URL ALONG WITH YOUR MOUSE OVER THE ADD BUTTON
        private void MemoryRecordAdd_DragEnter(object sender, DragEventArgs e)
        {

            //  Trace.WriteLine("add dragenter");

            if (e.Data.GetDataPresent(DataFormats.FileDrop)) // ke9ns check for file dragdrop
            {

                e.Effect = DragDropEffects.Copy;
                e.Effect = DragDropEffects.All;

                filename = (string[])e.Data.GetData(DataFormats.FileDrop);

                URLTEXT = filename[0]; // grab file name

                return;
            }


            droppedUrl = ReadURL(e.Data);

            if (droppedUrl != null && droppedUrl.Trim().Length != 0)
            {
                  URLTEXT = droppedUrl;
             
                e.Effect = DragDropEffects.Link; // got a URL so activate the drop event when mouse button released
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }


        } //MemoryRecordAdd_DragEnter


        //=========================================================================================================================================
        // Ke9ns add YOUR URL (after being VERIFIED) YOU LET GO THE LEFT MOUSE BUTTON TO DROP ONTO THE ADD BUTTON
        private void MemoryRecordAdd_DragDrop(object sender, DragEventArgs e)
        {
                    
                string mem_name = Convert.ToString(console.VFOAFreq);   //W4TME

                // ke9ns Below URLTEXT goes where normally the comment goes with a ""

            console.MemoryList.List.Add(new MemoryRecord("", console.VFOAFreq, mem_name, console.RX1DSPMode, true, console.TuneStepList[console.TuneStepIndex].Name,
                console.CurrentFMTXMode, console.FMTXOffsetMHz, console.dsp.GetDSPTX(0).CTCSSFlag, console.dsp.GetDSPTX(0).CTCSSFreqHz, console.PWR,
                (int)console.dsp.GetDSPTX(0).TXFMDeviation, console.VFOSplit, console.TXFreq, console.RX1Filter, console.RX1FilterLow,
                console.RX1FilterHigh, URLTEXT, console.dsp.GetDSPRX(0, 0).RXAGCMode, console.RF));

            Common.SaveForm(this, "MemoryForm");    // w4tme
            console.MemoryList.Save();              // w4tme 
        


        } // MemoryRecordAdd_DragDrop


        //=========================================================================================================================================
        // Ke9ns  this is the ADD button
        /// <summary>
        /// Add a new Memory entry based on the current console settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MemoryRecordAdd_Click(object sender, EventArgs e)
        {
            string mem_name = Convert.ToString(console.VFOAFreq);   //W4TME

            console.MemoryList.List.Add(new MemoryRecord("", console.VFOAFreq, mem_name, console.RX1DSPMode, true, console.TuneStepList[console.TuneStepIndex].Name,
                console.CurrentFMTXMode, console.FMTXOffsetMHz, console.dsp.GetDSPTX(0).CTCSSFlag, console.dsp.GetDSPTX(0).CTCSSFreqHz, console.PWR,
                (int)console.dsp.GetDSPTX(0).TXFMDeviation, console.VFOSplit, console.TXFreq, console.RX1Filter, console.RX1FilterLow, 
                console.RX1FilterHigh, "", console.dsp.GetDSPRX(0, 0).RXAGCMode, console.RF));

            Common.SaveForm(this, "MemoryForm");    // w4tme
            console.MemoryList.Save();              // w4tme 
        }


        //=========================================================================================================================================

        /// <summary>
        /// Copy an existing row into a new one.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMemoryRecordCopy_Click(object sender, EventArgs e)
        {
            if (console.MemoryList.List.Count == 0) return;
            console.MemoryList.List.Add(new MemoryRecord(console.MemoryList.List[dataGridView1.CurrentCell.RowIndex]));

            Common.SaveForm(this, "MemoryForm");    // w4tme
            console.MemoryList.Save();              // w4tme 
        }


        //=========================================================================================================================================
        /// <summary>
        /// Delete the current row (after confirmation).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMemoryRecordDelete_Click(object sender, EventArgs e)
        {
            if (console.MemoryList.List.Count == 0) return; // nothing in the list to copy, exit
            if (dataGridView1.SelectedRows.Count == 0) // no row selected -- use current cell
            {
                if (dataGridView1.CurrentCell.RowIndex < 0 ||
                    dataGridView1.CurrentCell.RowIndex > console.MemoryList.List.Count - 1)
                    return;
            }

            DialogResult dr = MessageBox.Show("Are you sure you want to remove the selected row(s)?",
                "Remove Row(s)?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dr == DialogResult.No) return;

            if (dataGridView1.SelectedRows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.SelectedRows.Count; ) // no i++ because the selected rows count gets decremented
                    console.MemoryList.List.Remove(console.MemoryList.List[dataGridView1.SelectedRows[i].Index]);
            }
            else // no rows selected, use current cell
            {
                console.MemoryList.List.Remove(console.MemoryList.List[dataGridView1.CurrentCell.RowIndex]);
            }

            Common.SaveForm(this, "MemoryForm");    // w4tme
            console.MemoryList.Save();              // w4tme 
           
        } // delete memory


        //===========================================================================================================================
        // ke9ns  memory  when you select an index line of your memory list

        /// <summary>
        /// Makes the selected row active -- sends it to console
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {

            if (console.MemoryList.List.Count == 0) return; // nothing in the list, exit


            int index = dataGridView1.CurrentCell.RowIndex;


            if (index < 0 || index > console.MemoryList.List.Count - 1) // index out of range
                return;

            console.changeComboFMMemory(index); // ke9ns this will call recallmemory in console 

            Debug.WriteLine("INDEX clicked "+index);
           
          //  Debug.WriteLine("INDEX clicked " + dataGridView1.);

            if (chkMemoryFormClose.Checked) // ke9ns this saves position of memory form window on your screen when you closed it.
            {
                Common.SaveForm(this, "MemoryForm");    // w4tme
                console.MemoryList.Save();              // w4tme 
                this.Close();
            }

            //console.RecallMemory(MemoryList.List[index]);

        } // btnselect_click




        /// <summary>
        /// Don't actually close the form, just hide it and save the position/size.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MemoryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
            Common.SaveForm(this, "MemoryForm");
            console.MemoryList.Save();
        }

        #endregion


    






        //===================================================================================================
        //===================================================================================================
        //KE9NS ADD below is used to determine the URL from a drag and drop onto the memory form
        //===================================================================================================
        //===================================================================================================

        private const string _asciiUrlDataFormatName = "UniformResourceLocator";
        private static readonly Encoding _asciiUrlEncoding = Encoding.ASCII;

        private const string _unicodeUrlDataFormatName = "UniformResourceLocatorW";
        private static readonly Encoding _unicodeUrlEncoding = Encoding.Unicode;


        //==================================================================================================
        // ke9ns look for URL or file
        private string ReadURL(IDataObject data)  // try reading as unicode URL  (data comes from the e.data of the drag/drop operation and is supposed to contain a URL or FILE
        {
           // try unicode first then ascii
            string unicodetest = Readurl(data, _unicodeUrlDataFormatName, _unicodeUrlEncoding); // _unicodeUrlDataFormatName = "UniformResourceLocatorW"; Encoding _unicodeUrlEncoding = Encoding.Unicode;

            if (unicodetest != null) 
            {
                return unicodetest;   // Unicode URL found from the data 
            }
               
            return Readurl(data, _asciiUrlDataFormatName, _asciiUrlEncoding); // ASCII URL found _asciiUrlDataFormatName = "UniformResourceLocator";   Encoding _asciiUrlEncoding = Encoding.ASCII

        } // UNICODE & ASCII testing
        
        //==================================================================================================
        private string Readurl(IDataObject data, string urlDataFormatName, Encoding urlEncoding)    // try reading as ASCII URL
        {
            // Check whether the data contains a URL

            // dataformat is either   UniformResourceLocator     or      UniformResourceLocatorW 
            // encoding is either     Encoding.ASCII           or       Encoding.Unicode;

            if (!DoesDragDropDataContainUrl1(data, urlDataFormatName))
            {
                return null;
            }

            string url;

            using (Stream urlStream = (Stream)data.GetData(urlDataFormatName)) // use drap/drop data
            {
                using (TextReader reader = new StreamReader(urlStream, urlEncoding))
                {
                    url = reader.ReadToEnd();  // Read the URL from the data
                }
            }
           
            return url.TrimEnd('\0');  // URLs in drag/drop data are often padded with null characters so remove these

        } // ASCII URL

      
        //==================================================================================================
        private static bool DoesDragDropDataContainUrl1(IDataObject data, string urlDataFormatName)
        {
            
            return (data != null) && (data.GetDataPresent(urlDataFormatName)); // return true = yes URL or file is in specified format

        }




        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    } // memoryform

} // powerSDR
