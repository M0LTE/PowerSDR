using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Flex.Control;

namespace PowerSDR
{
    public partial class FlexControlBasicForm : Form
    {
        private FlexControlInterface2 fc_interface = null;
        private Console console = null;

        public FlexControlBasicForm(Console c)
        {
            InitializeComponent();
            console = c;

            // setup the actual FlexControl Interface
            fc_interface = new FlexControlInterface2(c);

            // populate knob combobox controls
            foreach (FlexControlKnobFunction function in Enum.GetValues(typeof(FlexControlKnobFunction)))
            {
                string s = KnobFunction2String(function);
                comboButtonLeft.Items.Add(s);
                comboButtonMid.Items.Add(s);
                comboButtonRight.Items.Add(s);
                comboButtonKnob.Items.Add(s);
            }

            // setup defaults
            comboButtonLeft.Text = "Tune RIT";
            comboButtonMid.Text = "Audio Gain";
            comboButtonRight.Text = "Tune VFO B";
            comboButtonKnob.Text = "Tune VFO A";            

            // restore any saved configuration
            Common.RestoreForm(this, "FlexControlBasicForm", false);            
        }

        private FlexControl flexControl;
        public FlexControl FlexControl
        {
            get { return flexControl; }
            set
            {
                flexControl = value;
                
                if (flex_control_mode == FlexControlMode.Basic) 
                    fc_interface.FlexControl = value;
                
                if(flexControl != null)
                {
                    flexControl.Disconnected += new FlexControl.DisconnectHandler(FlexControl_Disconnected);

                    comboButtonLeft.Enabled = true;
                    comboButtonMid.Enabled = true;
                    comboButtonRight.Enabled = true;
                    comboButtonKnob.Enabled = true;
                }
            }
        }

        private void FlexControl_Disconnected(FlexControl fc)
        {
            comboButtonLeft.Enabled = false;
            comboButtonMid.Enabled = false;
            comboButtonRight.Enabled = false;
            comboButtonKnob.Enabled = false;

            //fc_interface.FlexControl = null;
            FlexControl = null;
        }

        public string KnobFunction2String(FlexControlKnobFunction function)
        {
            string ret_val = "";
            switch (function)
            {
                case FlexControlKnobFunction.TuneVFOA: ret_val = "Tune VFO A"; break;
                case FlexControlKnobFunction.TuneVFOB: ret_val = "Tune VFO B"; break;
                case FlexControlKnobFunction.TuneVFOASub: ret_val = "Tune VFO A Sub"; break;
                case FlexControlKnobFunction.TuneRIT: ret_val = "Tune RIT"; break;
                case FlexControlKnobFunction.TuneXIT: ret_val = "Tune XIT"; break;
                case FlexControlKnobFunction.TuneAF: ret_val = "Audio Gain"; break;
                case FlexControlKnobFunction.TuneAGCT: ret_val = "Tune AGC-T"; break;
                case FlexControlKnobFunction.None: ret_val = "None"; break;
            }

            return ret_val;
        }

        public FlexControlKnobFunction String2KnobFunction(string s)
        {
            FlexControlKnobFunction ret_val = FlexControlKnobFunction.TuneVFOA;
            switch (s)
            {
                case "Tune VFO A": ret_val = FlexControlKnobFunction.TuneVFOA; break;
                case "Tune VFO B": ret_val = FlexControlKnobFunction.TuneVFOB; break;
                case "Tune VFO A Sub": ret_val = FlexControlKnobFunction.TuneVFOASub; break;
                case "Tune RIT": ret_val = FlexControlKnobFunction.TuneRIT; break;
                case "Tune XIT": ret_val = FlexControlKnobFunction.TuneXIT; break;
                case "Audio Gain": ret_val = FlexControlKnobFunction.TuneAF; break;
                case "Tune AGC-T": ret_val = FlexControlKnobFunction.TuneAGCT; break;
                case "None": ret_val = FlexControlKnobFunction.None; break;
            }

            return ret_val;
        }

        private void comboButtonLeft_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fc_interface == null) return;
            fc_interface.ButtonLeftFunction = String2KnobFunction(comboButtonLeft.Text);
        }

        private void comboButtonMid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fc_interface == null) return;
            fc_interface.ButtonMidFunction = String2KnobFunction(comboButtonMid.Text);
        }

        private void comboButtonRight_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fc_interface == null) return;
            fc_interface.ButtonRightFunction = String2KnobFunction(comboButtonRight.Text);
        }

        private void comboButtonKnob_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fc_interface == null) return;
            fc_interface.ButtonKnobFunction = String2KnobFunction(comboButtonKnob.Text);
        }

        private void FlexControlSimpleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;

            Common.SaveForm(this, "FlexControlBasicForm");
        }

        private FlexControlMode flex_control_mode = FlexControlMode.Basic;
        public FlexControlMode FlexControlMode
        {
            get { return flex_control_mode; }
            set
            {
                flex_control_mode = value;
                switch (flex_control_mode)
                {
                    case FlexControlMode.Basic:
                        radModeBasic.Checked = true;
                        break;
                    case FlexControlMode.Advanced:
                        radModeAdvanced.Checked = true;
                        break;
                }
            }
        }

        private void radModeBasic_CheckedChanged(object sender, EventArgs e)
        {
            if (radModeBasic.Checked)
            {
                if(fc_interface.FlexControl != flexControl)
                    fc_interface.FlexControl = flexControl;

                object obj = null;
                if (radModeBasic.Focused)
                    obj = this;
                console.SetCurrentFlexControlMode(obj, FlexControlMode.Basic);
            }
        }    

        private void radModeAdvanced_CheckedChanged(object sender, EventArgs e)
        {
            if (radModeAdvanced.Checked)
            {
                if(fc_interface != null)
                fc_interface.FlexControl = null;

                object obj = null;
                if (radModeAdvanced.Focused)
                    obj = this;
                console.SetCurrentFlexControlMode(obj, FlexControlMode.Advanced);

                this.Hide();
            }
        }

        private bool auto_detect = true;
        public bool AutoDetect
        {
            get { return auto_detect; }
            set
            {
                auto_detect = value;
                if (chkAutoDetect != null && chkAutoDetect.Checked != auto_detect)
                    chkAutoDetect.Checked = value;
            }
        }

        private void chkAutoDetect_CheckedChanged(object sender, EventArgs e)
        {
            auto_detect = chkAutoDetect.Checked;
            FlexControl = null;

            if (console == null) return;

            if(console.FlexControlAutoDetect != auto_detect)
                console.FlexControlAutoDetect = chkAutoDetect.Checked;
        }                
    }
}
