//=================================================================
// splash.cs
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
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace PowerSDR
{
	public class Splash : System.Windows.Forms.Form
	{
		#region Variable Declarations

		// Threading
		static Splash ms_frmSplash = null;
		static Thread ms_oThread = null;

		// Fade in and out.
		private double m_dblOpacityIncrement = .07;
		private double m_dblOpacityDecrement = .035;
		private const int TIMER_INTERVAL = 50;

		// Status and progress bar
		private string m_sStatus;
		private double m_dblCompletionFraction = 0;
		private Rectangle m_rProgress;

		// Progress smoothing
		private double m_dblLastCompletionFraction = 0.0;
		private double m_dblPBIncrementPerTimerInterval = .015;

		// Self-calibration support
		private bool m_bFirstLaunch = false;
		private DateTime m_dtStart;
		private bool m_bDTSet = false;
		private int m_iIndex = 1;
		private int m_iActualTicks = 0;
		private ArrayList m_alPreviousCompletionFraction;
		private ArrayList m_alActualTimes = new ArrayList();
		private const string REG_KEY_INITIALIZATION = "Initialization";
		private const string REGVALUE_PB_MILISECOND_INCREMENT = "Increment";
		private const string REGVALUE_PB_PERCENTS = "Percents";
		private System.Windows.Forms.LabelTS lblTimeRemaining;
		private System.Windows.Forms.LabelTS lblStatus;
		private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Timer timer1;
        private TextBox textBox1;
        private System.ComponentModel.IContainer components = null;

		#endregion

		#region Constructor and Destructor

		public Splash()
		{
			InitializeComponent();

           

            this.Opacity = .00;
			timer1.Interval = TIMER_INTERVAL;
			timer1.Start();
			this.ClientSize = this.BackgroundImage.Size;
			this.ShowInTaskbar = false;
		}

		protected override void Dispose( bool disposing )
		{
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
		}

		#endregion

		#region Windows Form Designer generated code

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Splash));
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lblTimeRemaining = new System.Windows.Forms.LabelTS();
            this.lblStatus = new System.Windows.Forms.LabelTS();
            this.SuspendLayout();
            // 
            // pnlStatus
            // 
            this.pnlStatus.BackColor = System.Drawing.Color.Transparent;
            this.pnlStatus.Location = new System.Drawing.Point(50, 247);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(475, 24);
            this.pnlStatus.TabIndex = 2;
            this.pnlStatus.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlStatus_Paint);
            // 
            // timer1
            // 
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.Silver;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(164, 160);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(52, 20);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "RevT9";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // lblTimeRemaining
            // 
            this.lblTimeRemaining.BackColor = System.Drawing.Color.Transparent;
            this.lblTimeRemaining.ForeColor = System.Drawing.Color.White;
            this.lblTimeRemaining.Image = null;
            this.lblTimeRemaining.Location = new System.Drawing.Point(296, 289);
            this.lblTimeRemaining.Name = "lblTimeRemaining";
            this.lblTimeRemaining.Size = new System.Drawing.Size(100, 16);
            this.lblTimeRemaining.TabIndex = 1;
            this.lblTimeRemaining.Text = "Time";
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Image = null;
            this.lblStatus.Location = new System.Drawing.Point(0, 287);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(400, 16);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Status";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Splash
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(600, 384);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.pnlStatus);
            this.Controls.Add(this.lblTimeRemaining);
            this.Controls.Add(this.lblStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Splash";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Splash";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
	
		#region Static Methods
		// ************* Static Methods *************** //

		// A static method to create the thread and 
		// launch the SplashScreen.
		static public void ShowSplashScreen()
		{
			// Make sure it is only launched once.
			if( ms_frmSplash != null )
				return;
			ms_oThread = new Thread( new ThreadStart(ShowForm));
			ms_oThread.IsBackground = true;
			ms_oThread.Name = "Splash Screen Thread";
			ms_oThread.Start();
		}

		// A property returning the splash screen instance
		static public Splash SplashForm 
		{
			get
			{
				return ms_frmSplash;
			} 
		}

		// A private entry point for the thread.
		static private void ShowForm()
		{
			ms_frmSplash = new Splash();
            Control.CheckForIllegalCrossThreadCalls = false;
			Application.Run(ms_frmSplash);
		}

		// A static method to close the SplashScreen
		static public void CloseForm()
		{
			if( ms_frmSplash != null && ms_frmSplash.IsDisposed == false )
			{
				// Make it start going away.
				ms_frmSplash.m_dblOpacityIncrement = - 
					ms_frmSplash.m_dblOpacityDecrement;
			}
			ms_oThread = null;  // we do not need these any more.
			ms_frmSplash = null;
		}

		static public void HideForm()
		{
			if(ms_frmSplash != null && ms_frmSplash.IsDisposed == false)
				ms_frmSplash.Hide();
		}

		static public void UnHideForm()
		{
			if(ms_frmSplash != null && ms_frmSplash.IsDisposed == false)
				ms_frmSplash.Show();
		}

		// A static method to set the status and update the reference.
		static public void SetStatus(string newStatus)
		{
			SetStatus(newStatus, true);
		}
    
		// A static method to set the status and optionally update the reference.
		// This is useful if you are in a section of code that has a variable
		// set of status string updates.  In that case, don't set the reference.
		static public void SetStatus(string newStatus, bool setReference)
		{
			if( ms_frmSplash == null )
				return;
			ms_frmSplash.m_sStatus = newStatus;
			if( setReference )
				ms_frmSplash.SetReferenceInternal();
		}

		// Static method called from the initializing application to 
		// give the splash screen reference points.  Not needed if
		// you are using a lot of status strings.
		static public void SetReferencePoint()
		{
			if( ms_frmSplash == null )
				return;
			ms_frmSplash.SetReferenceInternal();

		}

		// ************ Private methods ************

		// Internal method for setting reference points.
		private void SetReferenceInternal()
		{
			if( m_bDTSet == false )
			{
				m_bDTSet = true;
				m_dtStart = DateTime.Now;
				ReadIncrements();
			}
			double dblMilliseconds = ElapsedMilliSeconds();
			m_alActualTimes.Add(dblMilliseconds);
			m_dblLastCompletionFraction = m_dblCompletionFraction;
			if( m_alPreviousCompletionFraction != null 
				&& m_iIndex < m_alPreviousCompletionFraction.Count )
				m_dblCompletionFraction = 
					(double)m_alPreviousCompletionFraction[m_iIndex++];
			else
				m_dblCompletionFraction = ( m_iIndex > 0 )? 1: 0;
		}

		// Utility function to return elapsed Milliseconds since the 
		// SplashScreen was launched.
		private double ElapsedMilliSeconds()
		{
			TimeSpan ts = DateTime.Now - m_dtStart;
			return ts.TotalMilliseconds;
		}

		// Function to read the checkpoint intervals 
		// from the previous invocation of the
		// splashscreen from the registry.
		private void ReadIncrements()
		{
			string sPBIncrementPerTimerInterval = 
				RegistryAccess.GetStringRegistryValue( 
				REGVALUE_PB_MILISECOND_INCREMENT, "0.0015");
			double dblResult;

			if( Double.TryParse(sPBIncrementPerTimerInterval, 
				System.Globalization.NumberStyles.Float,
				System.Globalization.NumberFormatInfo.InvariantInfo, 
				out dblResult))
				m_dblPBIncrementPerTimerInterval = dblResult;
			else
				m_dblPBIncrementPerTimerInterval = .0015;

			string sPBPreviousPctComplete = RegistryAccess.GetStringRegistryValue(
				REGVALUE_PB_PERCENTS, "" );

			if( sPBPreviousPctComplete != "" )
			{
				string [] aTimes = sPBPreviousPctComplete.Split(null);
				m_alPreviousCompletionFraction = new ArrayList();

				for(int i = 0; i < aTimes.Length; i++ )
				{
					double dblVal;
					if( Double.TryParse(aTimes[i],
						System.Globalization.NumberStyles.Float, 
						System.Globalization.NumberFormatInfo.InvariantInfo, 
						out dblVal) )
						m_alPreviousCompletionFraction.Add(dblVal);
					else
						m_alPreviousCompletionFraction.Add(1.0);
				}
			}
			else
			{
				m_bFirstLaunch = true;
				lblTimeRemaining.Text = "";
			}      
		}

		// Method to store the intervals (in percent complete)
		// from the current invocation of
		// the splash screen to the registry.
		private void StoreIncrements()
		{
			string sPercent = "";
			double dblElapsedMilliseconds = ElapsedMilliSeconds();
			for( int i = 0; i < m_alActualTimes.Count; i++ )
				sPercent += ((double)m_alActualTimes[i]/
					dblElapsedMilliseconds).ToString("0.####",
					System.Globalization.NumberFormatInfo.InvariantInfo) + " ";

			RegistryAccess.SetStringRegistryValue( 
				REGVALUE_PB_PERCENTS, sPercent );

			m_dblPBIncrementPerTimerInterval = 1.0/(double)m_iActualTicks;
			RegistryAccess.SetStringRegistryValue( 
				REGVALUE_PB_MILISECOND_INCREMENT, 
				m_dblPBIncrementPerTimerInterval.ToString("#.000000",
				System.Globalization.NumberFormatInfo.InvariantInfo));
		}

		#endregion

		#region Event Handlers

		//********* Event Handlers ************

		// Tick Event handler for the Timer control.  
		// Handle fade in and fade out.  Also
		// handle the smoothed progress bar.
		private void timer1_Tick(object sender, System.EventArgs e)
		{
			lblStatus.Text = m_sStatus;

			if( m_dblOpacityIncrement > 0 )
			{
				m_iActualTicks++;
				if( this.Opacity < 1 )
					this.Opacity += m_dblOpacityIncrement;
			}
			else
			{
				if( this.Opacity > 0 )
					this.Opacity += m_dblOpacityIncrement;
				else
				{
					StoreIncrements();
					this.Close();
				}
			}
			if( m_bFirstLaunch == false && m_dblLastCompletionFraction 
				< m_dblCompletionFraction )
			{
				m_dblLastCompletionFraction += m_dblPBIncrementPerTimerInterval;
				int width = (int)Math.Floor(
					pnlStatus.ClientRectangle.Width * m_dblLastCompletionFraction);
				int height = pnlStatus.ClientRectangle.Height;
				int x = pnlStatus.ClientRectangle.X;
				int y = pnlStatus.ClientRectangle.Y;
				if( width > 0 && height > 0 )
				{
					m_rProgress = new Rectangle( x, y, width, height);
					pnlStatus.Invalidate(m_rProgress);
					int iSecondsLeft = 1 + (int)(TIMER_INTERVAL * 
						((1.0 - m_dblLastCompletionFraction)/
						m_dblPBIncrementPerTimerInterval)) / 1000;
					if( iSecondsLeft == 1 )
						lblTimeRemaining.Text = string.Format( "1 second");
					else
						lblTimeRemaining.Text = string.Format( "{0} seconds", 
							iSecondsLeft);

				}
			}
		}

		// Paint the portion of the panel invalidated during the tick event.
        SolidBrush status_brush = null;
		private void pnlStatus_Paint(object sender, 
			System.Windows.Forms.PaintEventArgs e)
		{
			if( m_bFirstLaunch == false && e.ClipRectangle.Width > 0 
				&& m_iActualTicks > 1 )
			{
				/*LinearGradientBrush brBackground = 
					new LinearGradientBrush(pnlStatus.ClientRectangle, 
					Color.Black,//Color.FromArgb(100, 100, 100),
					Color.White,//Color.FromArgb(130, 255, 130), 
					LinearGradientMode.Horizontal);*/
                if(status_brush == null)
                    status_brush = new SolidBrush(Color.White);
                e.Graphics.FillRectangle(status_brush, m_rProgress);
			}
		}

		// Close the form if they double click on it.
		private void SplashScreen_DoubleClick(object sender, System.EventArgs e)
		{
			CloseForm();
		}

        #endregion

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }

    #region Registry Access Class

    /// A class for managing registry access.
    public class RegistryAccess
	{
		private const string SOFTWARE_KEY = "Software";
		private const string COMPANY_NAME = "FlexRadio";
		private const string APPLICATION_NAME = "PowerSDR";

		// Method for retrieving a Registry Value.
		static public string GetStringRegistryValue(string key, 
			string defaultValue)
		{
			RegistryKey rkCompany;
			RegistryKey rkApplication;

			rkCompany = Registry.CurrentUser.OpenSubKey(SOFTWARE_KEY, 
				false).OpenSubKey(COMPANY_NAME, false);
			if( rkCompany != null )
			{
				rkApplication = rkCompany.OpenSubKey(APPLICATION_NAME, true);
				if( rkApplication != null )
				{
					foreach(string sKey in rkApplication.GetValueNames())
					{
						if( sKey == key )
						{
							return (string)rkApplication.GetValue(sKey);
						}
					}
				}
			}
			return defaultValue;
		}

		// Method for storing a Registry Value.
		static public void SetStringRegistryValue(string key, 
			string stringValue)
		{
			RegistryKey rkSoftware;
			RegistryKey rkCompany;
			RegistryKey rkApplication;

			rkSoftware = Registry.CurrentUser.OpenSubKey(SOFTWARE_KEY, true);
			rkCompany = rkSoftware.CreateSubKey(COMPANY_NAME);
			if( rkCompany != null )
			{
				rkApplication = rkCompany.CreateSubKey(APPLICATION_NAME);
				if( rkApplication != null )
				{
					rkApplication.SetValue(key, stringValue);
				}
			}
		}
	}

	#endregion
}
