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
using System.Runtime.InteropServices;
//custom namespaces
using VolumeControl.Library.Constants;
using PowerSDR;

namespace VolumeControl.Library.Structs
{
    /// <summary>
    /// Class file for holding all the custom sructures we need
    /// for controlling the system sound
    /// </summary>
    public static class VolumeStructs
    {
        /// <summary>
        /// struct for holding data for the mixer caps
        /// </summary>
        public struct MixerCaps
        {
            public int wMid;
            public int wPid;
            public int vDriverVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = VolumeConstants.MAXPNAMELEN)] public string szPname;
            public int fdwSupport;
            public int cDestinations;
        }

        /// <summary>
        /// struct to hold data for the mixer control
        /// </summary>
        public struct Mixer
        {
            public int cbStruct;
            public int dwControlID;
            public int dwControlType;
            public int fdwControl;
            public int cMultipleItems;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = VolumeConstants.MIXER_SHORT_NAME_CHARS)] public string szShortName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = VolumeConstants.MIXER_LONG_NAME_CHARS)] public string szName;
            public int lMinimum;
            public int lMaximum;
            [MarshalAs(UnmanagedType.U4, SizeConst = 10)]  public int reserved;
        }

        /// <summary>
        /// struct for holding data about the details of the mixer control
        /// </summary>
        public struct MixerDetails
        {
            public int cbStruct;
            public int dwControlID;
            public int cChannels;
            public int item;
            public int cbDetails;
            public IntPtr paDetails;
        }

        /// <summary>
        /// struct to hold data for an unsigned mixer control details
        /// </summary>
        public struct UnsignedMixerDetails
        {
            public int dwValue;
        }

        /// <summary>
        /// struct to hold data for the mixer line
        /// </summary>
        public struct MixerLine
        {
            public int cbStruct;
            public int dwDestination;
            public int dwSource;
            public int dwLineID;
            public int fdwLine;
            public int dwUser;
            public int dwComponentType;
            public int cChannels;
            public int cConnections;
            public int cControls;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = VolumeConstants.MIXER_SHORT_NAME_CHARS)] public string szShortName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = VolumeConstants.MIXER_LONG_NAME_CHARS)] public string szName;
            public int dwType;
            public int dwDeviceID;
            public int wMid;
            public int wPid;
            public int vDriverVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = VolumeConstants.MAXPNAMELEN)] public string szPname;
        }

        /// <summary>
        /// struct for holding data for the mixer line controls
        /// </summary>
        public struct LineControls
        {
            public int cbStruct;
            public int dwLineID;
            public int dwControl;
            public int cControls;
            public int cbmxctrl;
            public IntPtr pamxctrl;
        }
    }
}
