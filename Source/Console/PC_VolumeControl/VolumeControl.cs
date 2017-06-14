//*****************************************************************************************
//                           LICENSE INFORMATION
//*****************************************************************************************
//   PC_VolumeControl Version 1.0.0.0
//   A class library for creating a mixer control and controlling the volume on your computer
//
//   Copyright (C) 2007  
//   Richard L. McCutchen 
//   Email: psychocoder@dreamincode.net
//   Created: 04OCT06
//
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with this program.  If not, see <http://www.gnu.org/licenses/>.
//*****************************************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
//custom Namespaces
using VolumeControl.Library.Constants;
using VolumeControl.Library.Structs;
using VolumeControl.Library.Win32;
//using PowerSDR;

namespace PowerSDR
{
    public class VolumeControl
    {

        public Setup setupForm;   // ke9ns communications with setupform  (i.e. allow combometertype.text update from inside console.cs) 

        /// <summary>
        /// method to retrieve a mixer control
        /// </summary>
        /// <param name="i"></param>
        /// <param name="type"></param>
        /// <param name="ctrlType"></param>
        /// <param name="mixerControl"></param>
        /// <param name="currVolume"></param>
        /// <returns></returns>
        private static bool GetMixer(int i, int type, int ctrlType, out VolumeStructs.Mixer mixerControl, out int currVolume)
        {
            //create our method level variables
            int details;
            bool bReturn;
            currVolume = -1;

            //create our struct objects
            VolumeStructs.LineControls lineControls = new VolumeStructs.LineControls();
            VolumeStructs.MixerLine line = new VolumeStructs.MixerLine();
            VolumeStructs.MixerDetails mcDetails = new VolumeStructs.MixerDetails();
            VolumeStructs.UnsignedMixerDetails detailsUnsigned = new VolumeStructs.UnsignedMixerDetails();

            //create a new mixer control
            mixerControl = new VolumeStructs.Mixer();
           
            //set the properties of out mixerline object
            line.cbStruct = Marshal.SizeOf(line);
            line.dwComponentType = type;
            //get the line info and assign it to our details variable
            details = PCWin32.mixerGetLineInfoA(i, ref line, VolumeConstants.MIXER_GETLINEINFOF_COMPONENTTYPE);

            //make sure we didnt receive any errors
            if (VolumeConstants.MMSYSERR_NOERROR == details)
            {
                int mcSize = 152;
                //get the size of the unmanaged type
                int control = Marshal.SizeOf(typeof(VolumeStructs.Mixer));
                //allocate a block of memory
                lineControls.pamxctrl = Marshal.AllocCoTaskMem(mcSize);
                //get the size of the line controls
                lineControls.cbStruct = Marshal.SizeOf(lineControls);

                //set properties for our mixer control
                lineControls.dwLineID = line.dwLineID;
                lineControls.dwControl = ctrlType;
                lineControls.cControls = 1;
                lineControls.cbmxctrl = mcSize;

                // Allocate a buffer for the control
                mixerControl.cbStruct = mcSize;

                // Get the control
                details = PCWin32.mixerGetLineControlsA(i, ref lineControls, VolumeConstants.MIXER_GETLINECONTROLSF_ONEBYTYPE);

                //once again check to see if we received any errors
                if (VolumeConstants.MMSYSERR_NOERROR == details)
                {
                    bReturn = true;
                    //Copy the control into the destination structure
                    mixerControl = (VolumeStructs.Mixer)Marshal.PtrToStructure(lineControls.pamxctrl, typeof(VolumeStructs.Mixer));
                }
                else
                {
                    bReturn = false;
                }

                int mcDetailsSize = Marshal.SizeOf(typeof(VolumeStructs.MixerDetails));
                int mcDetailsUnsigned = Marshal.SizeOf(typeof(VolumeStructs.UnsignedMixerDetails));
                mcDetails.cbStruct = mcDetailsSize;
                mcDetails.dwControlID = mixerControl.dwControlID;
                mcDetails.paDetails = Marshal.AllocCoTaskMem(mcDetailsUnsigned);
                mcDetails.cChannels = 1;
                mcDetails.item = 0;
                mcDetails.cbDetails = mcDetailsUnsigned;
                details = PCWin32.mixerGetControlDetailsA(i, ref mcDetails, VolumeConstants.MIXER_GETCONTROLDETAILSF_VALUE);
                detailsUnsigned = (VolumeStructs.UnsignedMixerDetails)Marshal.PtrToStructure(mcDetails.paDetails, typeof(VolumeStructs.UnsignedMixerDetails));
                currVolume = detailsUnsigned.dwValue;
                return bReturn;
            }

            bReturn = false;
            return bReturn;

        } // get mixer

//======================================================================================================================

        /// <summary>
        /// method for setting the value for a volume control
        /// </summary>
        /// <param name="i"></param>
        /// <param name="mixerControl"></param>
        /// <param name="volumeLevel"></param>
        /// <returns>true/false</returns>
        private static bool SetMixer(int i, VolumeStructs.Mixer mixerControl, int volumeLevel)
        {
            //method level variables
            bool bReturn;
            int details;

            //create our struct object for controlling the system sound
            VolumeStructs.MixerDetails mixerDetails = new VolumeStructs.MixerDetails();
            VolumeStructs.UnsignedMixerDetails volume = new VolumeStructs.UnsignedMixerDetails();

            //set out mixer control properties
            mixerDetails.item = 0;
            //set the id of the mixer control
            mixerDetails.dwControlID = mixerControl.dwControlID;
            //return the size of the mixer details struct
            mixerDetails.cbStruct = Marshal.SizeOf(mixerDetails);
            //return the volume
            mixerDetails.cbDetails = Marshal.SizeOf(volume);

            //Allocate a buffer for the mixer control value buffer
            mixerDetails.cChannels = 1;
            volume.dwValue = volumeLevel;

            //Copy the data into the mixer control value buffer
            mixerDetails.paDetails = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(VolumeStructs.UnsignedMixerDetails)));
            Marshal.StructureToPtr(volume, mixerDetails.paDetails, false);

            //Set the control value
            details = PCWin32.mixerSetControlDetails(i, ref mixerDetails, VolumeConstants.MIXER_SETCONTROLDETAILSF_VALUE);

            //Check to see if any errors were returned
            if (VolumeConstants.MMSYSERR_NOERROR == details)
            {
                bReturn = true;
            }
            else
            {
                bReturn = false;
            }
            return bReturn;

        } // set mixer

        //======================================================================================================================

        /// <summary>
        /// method for retrieving the current volume from the system
        /// </summary>
        /// <returns>int value</returns>
        public static int GetVolume()
        {
            //method level variables
            int currVolume;
            int mixerControl;

            //create a new volume control
            VolumeStructs.Mixer mixer = new VolumeStructs.Mixer();
            
            //open the mixer
            PCWin32.mixerOpen(out mixerControl, 0, 0, 0, 0);

            //set the type to volume control type
            int type = VolumeConstants.MIXERCONTROL_CONTROLTYPE_VOLUME;

            //get the mixer control and get the current volume level
            GetMixer(mixerControl, VolumeConstants.MIXERLINE_COMPONENTTYPE_DST_SPEAKERS, type, out mixer, out currVolume);

            //close the mixer control since we are now done with it
            PCWin32.mixerClose(mixerControl);

            //return the current volume to the calling method
            return currVolume;
        }

        //======================================================================================================================

        /// <summary>
        /// method for setting the volume to a specific level
        /// </summary>
        /// <param name="volumeLevel">volume level we wish to set volume to</param>
        public static void SetVolume(int volumeLevel)
        {
            try
            {
                //method level variables
                int currVolume;
                int mixerControl;

                //create a new volume control
                VolumeStructs.Mixer volumeControl = new VolumeStructs.Mixer();

                //open the mixer control
                PCWin32.mixerOpen(out mixerControl, 0, 0, 0, 0);

                //set the type to volume control type
                int controlType = VolumeConstants.MIXERCONTROL_CONTROLTYPE_VOLUME;

                //get the current mixer control and get the current volume
                GetMixer(mixerControl, VolumeConstants.MIXERLINE_COMPONENTTYPE_DST_SPEAKERS, controlType, out volumeControl, out currVolume);

                //now check the volume level. If the volume level
                //is greater than the max then set the volume to
                //the max level. If it's less than the minimum level
                //then set it to the minimun level
                if (volumeLevel > volumeControl.lMaximum)
                {
                    volumeLevel = volumeControl.lMaximum;
                }
                else if (volumeLevel < volumeControl.lMinimum)
                {
                    volumeLevel = volumeControl.lMinimum;
                }

                //set the volume
                SetMixer(mixerControl, volumeControl, volumeLevel);

                //now re-get the mixer control
                GetMixer(mixerControl, VolumeConstants.MIXERLINE_COMPONENTTYPE_DST_SPEAKERS, controlType, out volumeControl, out currVolume);

                //make sure the volume level is equal to the current volume
                if (volumeLevel != currVolume)
                {
                    throw new Exception("Cannot Set Volume");
                }

                //close the mixer control as we are finished with it
                PCWin32.mixerClose(mixerControl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }  
                      
        } // setvolume


    } // class volumecontrol
} // PC_Volumecontrol