//=================================================================
// fwc.cs
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

//#define TIMING

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;

using FlexCW;

namespace PowerSDR
{
	unsafe public class FWC
	{
		#region Public Functions

		public static int SetRegister(uint addr, uint val)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_REG_SET, addr, val);
		}

		public static int GetRegister(uint addr, out uint val)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_REG_GET, addr, 0, out val);
		}

		public static int GPIORead(out uint val)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_READ_GPIO, 0, 0, out val);
		}
		
		public static int GPIOWrite(uint val)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_WRITE_GPIO, val, 0);
		}

		public static int GPIODDRRead(out uint val)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_READ_GPIO_DDR, 0, 0, out val);
		}

		public static int GPIODDRWrite(uint val)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_WRITE_GPIO_DDR, val, 0);
		}

		public static int I2C_WriteValue(ushort addr, byte val)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_I2C_WRITE_VALUE, (uint)addr, (uint)val);
		}

		public static int I2C_Write2Value(ushort addr, byte v1, byte v2)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_I2C_WRITE_2_VALUE, (uint)addr, (uint)((v1<<8)+v2));
		}

		public static int I2C_ReadValue(ushort addr, out uint val)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_I2C_READ_VALUE, (uint)addr, 0, out val);
		}

		public static int GetSerialNum(out uint num)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_GET_SERIAL_NUM, 0, 0, out num);			
		}

		public static int GetModel(out uint model)
		{
            return PalManager.Instance.ReadOp(Opcode.RDAL_OP_GET_MODEL, 0, 0, out model);
		}

        public static int GetModelMidi(out uint model)
        {
            model = FWCMidi.SendGetMessage(Opcode.RDAL_OP_GET_MODEL, 0, 0);
            return 0;
        }


//*****************************************************************************************
		public static int GetFirmwareRev(out uint rev)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_GET_FIRMWARE_REV, 0, 0, out rev);
		}

		public static int Initialize()
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_INITIALIZE, 0, 0);
		}

		public static int GetTRXOK(out bool b)
		{
			uint val;
			int rtn = PalManager.Instance.ReadOp(Opcode.RDAL_OP_GET_TRX_OK, 0, 0, out val);
			b = Convert.ToBoolean(val);
			return rtn;
		}

		public static int GetTRXRev(out uint rev)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_GET_TRX_REV, 0, 0, out rev);
		}

		public static int GetTRXSN(out uint sn)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_GET_TRX_SN, 0, 0, out sn);
		}

		public static int GetPAOK(out bool b)
		{
			uint val;
			int rtn = PalManager.Instance.ReadOp(Opcode.RDAL_OP_GET_PA_OK, 0, 0, out val);
			b = Convert.ToBoolean(val);
			return rtn;
		}

		public static int GetPARev(out uint rev)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_GET_PA_REV, 0, 0, out rev);
		}

		public static int GetPASN(out uint sn)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_GET_PA_SN, 0, 0, out sn);
		}

		public static int GetRFIOOK(out bool b)
		{
			uint val;
			int rtn = PalManager.Instance.ReadOp(Opcode.RDAL_OP_GET_RFIO_OK, 0, 0, out val);
			b = Convert.ToBoolean(val);
			return rtn;
		}

		public static int GetRFIORev(out uint rev)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_GET_RFIO_REV, 0, 0, out rev);
		}

		public static int GetRFIOSN(out uint sn)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_GET_RFIO_SN, 0, 0, out sn);
		}

		public static int GetATUOK(out bool b)
		{
			uint val;
			int rtn = PalManager.Instance.ReadOp(Opcode.RDAL_OP_GET_ATU_OK, 0, 0, out val);
			b = Convert.ToBoolean(val);
			return rtn;
		}

		public static int GetATURev(out uint rev)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_GET_ATU_REV, 0, 0, out rev);
		}

		public static int GetATUSN(out uint sn)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_GET_ATU_SN, 0, 0, out sn);
		}

		public static int GetRX2OK(out bool b)
		{
			uint val;
			int rtn = PalManager.Instance.ReadOp(Opcode.RDAL_OP_GET_RX2_OK, 0, 0, out val);
			b = Convert.ToBoolean(val);
			return rtn;
		}

		public static int GetRX2Rev(out uint rev)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_GET_RX2_REV, 0, 0, out rev);
		}

		public static int GetRX2SN(out uint sn)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_GET_RX2_SN, 0, 0, out sn);
		}

		public static int GetVUOK(out bool b)
		{
			uint val;
			int rtn = PalManager.Instance.ReadOp(Opcode.RDAL_OP_GET_VU_OK, 0, 0, out val);
			b = Convert.ToBoolean(val);
			return rtn;
		}

		public static int GetVURev(out uint rev)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_GET_VU_REV, 0, 0, out rev);
		}

		public static int GetVUSN(out uint sn)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_GET_VU_SN, 0, 0, out sn);
		}

		public static int ReadClockReg(int index, out int val)
		{
			uint data;
			int rtn = PalManager.Instance.ReadOp(Opcode.RDAL_OP_READ_CLOCK_REG, (uint)index, 0, out data);
			val = (int)data;
			return rtn;
		}

		public static int WriteClockReg(int index, int val)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_WRITE_CLOCK_REG, (uint)index, (uint)val);
		}

		public static int ReadTRXDDSReg(int chan, int index, out uint val)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_READ_TRX_DDS_REG, (uint)chan, (uint)index, out val);
		}

		public static int WriteTRXDDSReg(int chan, int index, uint val)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_WRITE_TRX_DDS_REG, (uint)((chan<<8)+index), val);
		}

		public static int ReadRX2DDSReg(int chan, int index, out uint val)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_READ_RX2_DDS_REG, (uint)chan, (uint)index, out val);
		}

		public static int WriteRX2DDSReg(int chan, int index, uint val)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_WRITE_RX2_DDS_REG, (uint)((chan<<8)+index), val);
		}

		public static int WritePIOReg(int index, int reg, int val)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_WRITE_PIO_REG, (uint)((index<<8)+reg), (uint)val);
		}

		public static int ReadPIOReg(int index, int reg, out uint val)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_READ_PIO_REG, (uint)index, (uint)reg, out val);
		}

		public static int ReadTRXEEPROMByte(uint offset, out byte buf)
		{
			uint data;
			int rtn = PalManager.Instance.ReadOp(Opcode.RDAL_OP_READ_TRX_EEPROM_UINT8, offset, 0, out data);
			buf = (byte)data;
			return rtn;
		}

		public static int ReadTRXEEPROMUshort(uint offset, out ushort buf)
		{
			uint data;
			int rtn = PalManager.Instance.ReadOp(Opcode.RDAL_OP_READ_TRX_EEPROM_UINT16, offset, 0, out data);
            buf = (ushort)data;
			return rtn;
		}

		public static int ReadTRXEEPROMUint(uint offset, out uint buf)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_READ_TRX_EEPROM_UINT32, offset, 0, out buf);
		}

		public static int ReadTRXEEPROMFloat(uint offset, out float buf)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_READ_TRX_EEPROM_UINT32, offset, 0, out buf);
		}

		public static int WriteTRXEEPROMByte(uint offset, byte buf)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_WRITE_TRX_EEPROM_UINT8, offset, (uint)buf);
		}

		public static int WriteTRXEEPROMUshort(uint offset, ushort buf)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_WRITE_TRX_EEPROM_UINT16, offset, (uint)buf);
		}

		public static int WriteTRXEEPROMUint(uint offset, uint buf)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_WRITE_TRX_EEPROM_UINT32, offset, buf);
		}

		public static int WriteTRXEEPROMFloat(uint offset, float val)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_WRITE_TRX_EEPROM_UINT32, offset, val);
		}

		public static int ReadRX2EEPROMByte(uint offset, out byte buf)
		{
			uint data;
			int rtn = PalManager.Instance.ReadOp(Opcode.RDAL_OP_READ_RX2_EEPROM_UINT8, offset, 0, out data);
			buf = (byte)data;
			return rtn;
		}

		public static int ReadRX2EEPROMUshort(uint offset, out ushort buf)
		{
			uint data;
			int rtn = PalManager.Instance.ReadOp(Opcode.RDAL_OP_READ_RX2_EEPROM_UINT16, offset, 0, out data);
			buf = (ushort)data;
			return rtn;
		}

		public static int ReadRX2EEPROMUint(uint offset, out uint buf)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_READ_RX2_EEPROM_UINT32, offset, 0, out buf);
		}

		public static int ReadRX2EEPROMFloat(uint offset, out float buf)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_READ_RX2_EEPROM_UINT32, offset, 0, out buf);
		}

		public static int WriteRX2EEPROMByte(uint offset, byte buf)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_WRITE_RX2_EEPROM_UINT8, offset, (uint)buf);
		}

		public static int WriteRX2EEPROMUshort(uint offset, ushort buf)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_WRITE_RX2_EEPROM_UINT16, offset, (uint)buf);
		}

		public static int WriteRX2EEPROMUint(uint offset, uint buf)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_WRITE_RX2_EEPROM_UINT32, offset, buf);
		}

		public static int WriteRX2EEPROMFloat(uint offset, float val)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_WRITE_RX2_EEPROM_UINT32, offset, val);
		}

		public static int SetTRXPot(uint val)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_TRX_POT, val, 0);
		}

		public static int TRXPotSetRDAC(int index, int val)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_TRX_POT_SET_RDAC, (uint)index, (uint)val);
		}

		public static int TRXPotGetRDAC(out uint val)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_TRX_POT_GET_RDAC, 0, 0, out val);
		}

		public static int PAPotSetRDAC(int index, int val)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_PA_POT_SET_RDAC, (uint)index, (uint)val);
		}

		public static int PAPotGetRDAC(out uint val)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_PA_POT_GET_RDAC, 0, 0, out val);
		}

		public static int SetMux(int chan)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_MUX, (uint)chan, 0);
		}

		public static int WriteCodecReg(int reg, byte val)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_WRITE_CODEC_REG, (uint)reg, (uint)val);
		}

		public static int ReadCodecReg(byte reg, out byte val)
		{
			uint data;
			int rtn = PalManager.Instance.ReadOp(Opcode.RDAL_OP_READ_CODEC_REG, (uint)reg, 0, out data);
			val = (byte)data;
			return rtn;
		}


		public static int SetRX1Freq(float freq)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_RX1_FREQ, freq, 0);
		}

		public static int SetRX1FreqTW(uint tw, float freq)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_RX1_FREQ_TW, tw, freq);
		}

		public static int SetRX2Freq(float freq)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_RX2_FREQ, freq, 0);
		}

		public static int SetRX2FreqTW(uint tw, float freq)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_RX2_FREQ_TW, tw, freq);
		}

		public static int SetTXFreq(float freq)                                        // ke9ns set transmit freq
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_TX_FREQ, freq, 0);
		}

		public static int SetTXFreqTW(uint tw, float freq)                            // ke9ns set TX DDS freq ???
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_TX_FREQ_TW, tw, freq);
		}

		public static int SetTRXPreamp(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_TRX_PREAMP, Convert.ToUInt32(b), 0);
		}

		public static int SetTest(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_TEST, Convert.ToUInt32(b), 0);
		}

		public static int SetGen(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_GEN, Convert.ToUInt32(b), 0);
		}

		public static int SetSig(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_SIG, Convert.ToUInt32(b), 0);
		}

		public static int SetImpulse(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_IMPULSE, Convert.ToUInt32(b), 0);
		}

		public static int SetXVEN(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_XVEN, Convert.ToUInt32(b), 0);
		}

		public static int SetXVTXEN(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_XVTXEN, Convert.ToUInt32(b), 0);
		}		

		public static int SetQSD(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_QSD, Convert.ToUInt32(b), 0);
		}

		public static int SetQSE(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_QSE, Convert.ToUInt32(b), 0);
		}

		public static int SetXREF(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_XREF, Convert.ToUInt32(b), 0);
		}

		public static int SetRX1Filter(float freq)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_RX1_FILTER, freq, 0);
		}

		public static int SetRX2Filter(float freq)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_RX2_FILTER, freq, 0);
		}

		public static int SetTXFilter(float freq)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_TX_FILTER, freq, 0);
		}

		public static int SetPAFilter(float freq)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_PA_FILTER, freq, 0);
		}

		public static int SetIntSpkr(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_INT_SPKR, Convert.ToUInt32(b), 0);
		}

		public static int SetRX1Tap(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_RX1_TAP, Convert.ToUInt32(b), 0);
		}

		public static int BypassRX1Filter(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_BYPASS_RX1_FILTER, Convert.ToUInt32(b), 0);
		}

		public static int BypassRX2Filter(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_BYPASS_RX2_FILTER, Convert.ToUInt32(b), 0);
		}

		public static int BypassTXFilter(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_BYPASS_TX_FILTER, Convert.ToUInt32(b), 0);
		}

		public static int BypassPAFilter(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_BYPASS_PA_FILTER, Convert.ToUInt32(b), 0);
		}

		public static int ReadPTT(out bool dot, out bool dash, out bool rca_ptt, out bool mic_ptt)
		{
			uint data;
			int rtn = PalManager.Instance.ReadOp(Opcode.RDAL_OP_READ_PTT, 0, 0, out data);
			dot = ((data & 0x01) == 1);
			dash = ((data & 0x02) == 2);
			rca_ptt = ((data & 0x04) == 4);
			mic_ptt = ((data & 0x08) == 8);
			//Debug.WriteLine("dot: "+dot+" dash: "+dash+" rca: "+rca_ptt+" mic: "+mic_ptt);
			return rtn;
		}

		public static int ReadDot(out bool dot)
		{
			uint data;
			int rtn = PalManager.Instance.ReadOp(Opcode.RDAL_OP_READ_PTT, 0, 0, out data);
			dot = ((data & 0x01) == 1);
			return rtn;
		}

		public static int ReadDash(out bool dash)
		{
			uint data;
			int rtn = PalManager.Instance.ReadOp(Opcode.RDAL_OP_READ_PTT, 0, 0, out data);
			dash = ((data & 0x02) == 2);
			return rtn;
		}

		public static int ReadRCAPTT(out bool rca_ptt)
		{
			uint data;
			int rtn = PalManager.Instance.ReadOp(Opcode.RDAL_OP_READ_PTT, 0, 0, out data);
			rca_ptt = ((data & 0x04) == 4);
			return rtn;
		}

		public static int ReadMicPTT(out bool mic_ptt)
		{
			uint data;
			int rtn = PalManager.Instance.ReadOp(Opcode.RDAL_OP_READ_PTT, 0, 0, out data);
			mic_ptt = ((data & 0x08) == 8);
			return rtn;
		}

		public static int ReadMicDown(out bool mic_down)
		{
			uint data;
			int rtn = PalManager.Instance.ReadOp(Opcode.RDAL_OP_READ_PTT, 0, 0, out data);
			mic_down = ((data & 0x10) == 0x10);
			return rtn;
		}

		public static int ReadMicUp(out bool mic_up)
		{
			uint data;
			int rtn = PalManager.Instance.ReadOp(Opcode.RDAL_OP_READ_PTT, 0, 0, out data);
			mic_up = ((data & 0x20) == 0x20);
			return rtn;
		}

		public static int ReadMicFast(out bool mic_fast)
		{
			uint data;
			int rtn = PalManager.Instance.ReadOp(Opcode.RDAL_OP_READ_PTT, 0, 0, out data);
			mic_fast = ((data & 0x40) == 0x40);
			return rtn;
		}

		public static int ReadPTT(out byte dot, out byte dash, out bool rca_ptt, out bool mic_ptt)
		{
			uint data;
			int rtn = PalManager.Instance.ReadOp(Opcode.RDAL_OP_READ_PTT, 0, 0, out data);
			if((data & 0x01) == 1) dot = 1; else dot = 0;
			if((data & 0x02) == 2) dash = 1; else dash = 0;
			rca_ptt = ((data & 0x04) == 4);
			mic_ptt = ((data & 0x08) == 8);
			//Debug.WriteLine("dot: "+dot+" dash: "+dash+" rca: "+rca_ptt+" mic: "+mic_ptt);
			return rtn;
		}

		public static int SetHeadphone(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_HEADPHONE, Convert.ToUInt32(b), 0);
		}

		public static int SetPLL(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_PLL, Convert.ToUInt32(b), 0);
		}

		public static int SetRCATX1(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_RCA_TX1, Convert.ToUInt32(b), 0);
		}

		public static int SetRCATX2(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_RCA_TX2, Convert.ToUInt32(b), 0);
		}

		public static int SetRCATX3(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_RCA_TX3, Convert.ToUInt32(b), 0);
		}

		public static int SetFan(bool b)
		{
			//FWCMidi.SendSetMessage(Opcode.RDAL_OP_SET_FAN, Convert.ToUInt32(b), 0);
			return 0;
		}

		public static int GetPLLStatus2(out bool b)
		{
			uint data;
			int rtn = PalManager.Instance.ReadOp(Opcode.RDAL_OP_GET_PLL_STATUS2, 0, 0, out data);
			b = (data != 0);

			return rtn;
		}

		public static int ReadPAADC(int chan, out int val)
		{
			uint data;
			int rtn = PalManager.Instance.ReadOp(Opcode.RDAL_OP_READ_PA_ADC, (uint)chan, 0, out data);
			val = (int)data;
			return rtn;
		}

		public static int SetRX1Loop(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_RX1_LOOP, Convert.ToUInt32(b), 0);
		}

		public static int SetTR(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_TR, Convert.ToUInt32(b), 0);
		}

		public static int SetAnt(int ant)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_ANT, (uint)ant, 0);
		}

		public static int SetRX1Ant(int ant)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_RX1_ANT, Convert.ToUInt32(ant), 0);
		}

		public static int SetRX2Ant(int ant)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_RX2_ANT, Convert.ToUInt32(ant), 0);
		}

		public static int SetTXAnt(int ant)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_TX_ANT, Convert.ToUInt32(ant), 0);
		}

		public static int SetTXMon(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_TXMON, Convert.ToUInt32(b), 0);
		}

		public static int SetXVCOM(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_XVCOM, Convert.ToUInt32(b), 0);
		}

		public static int SetXVINT(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_XVINT, Convert.ToUInt32(b), 0);
		}

		public static int SetKey2M(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_KEY_2M, Convert.ToUInt32(b), 0);
		}

		public static int SetXVTR(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_XVTR, Convert.ToUInt32(b), 0);
		}

		public static int SetPABias(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_PA_BIAS, Convert.ToUInt32(b), 0);
		}

		public static int SetPowerOff()
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_POWER_OFF, 0, 0);
		}

		public static int SetFPLED(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_FPLED, Convert.ToUInt32(b), 0);
		}

		public static int SetCTS(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_CTS, Convert.ToUInt32(b), 0);
		}

		public static int SetRTS(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_RTS, Convert.ToUInt32(b), 0);
		}

		public static int SetPCReset(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_PC_RESET, Convert.ToUInt32(b), 0);
		}

		public static int SetPCPWRBT(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_PC_PWRBT, Convert.ToUInt32(b), 0);
		}

		public static int SetMOX(bool b)
		{
			//Debug.WriteLine("FWC.SetMOX("+b+")");
            /*if (FWCMidi.Init)
            {
                FWCMidi.SetMOX(b);
                return 0;
            }
            else*/
            {
                return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_MOX, Convert.ToUInt32(b), 0);
            }
		}

		public static int SetIntLED(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_INT_LED, Convert.ToUInt32(b), 0);
		}

		public static int ATUSendCmd(byte b1, byte b2, byte b3)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_ATU_SEND_CMD, (uint)((b1<<8)+b2), b3);
		}

		public static int ATUGetResult(out byte cmd, out byte b2, out byte b3, out byte b4, uint timeout_ms)
		{
			uint data;
			int rtn = PalManager.Instance.ReadOp(Opcode.RDAL_OP_ATU_GET_RESULT, timeout_ms, 0, out data);
			cmd = (byte)data;
			b2 = (byte)(data>>8);
			b3 = (byte)(data>>16);
			b4 = (byte)(data>>24);
            Debug.WriteLine("cmd: " + cmd);
            Debug.WriteLine("b2: " + b2);
            Debug.WriteLine("b3: " + b3);
            Debug.WriteLine("b4: " + b4);

			return rtn;
		}

		public static int SetFullDuplex(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_FULL_DUPLEX, Convert.ToUInt32(b), 0);
		}

		public static int SetTXDAC(bool b)                                                // ke9ns not used
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_TX_DAC, Convert.ToUInt32(b), 0);
		}

		public static int SetAmpTX1(bool b)                                              // ke9ns determine which RCA plug will be keyed up 
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_AMP_TX1, Convert.ToUInt32(b), 0);
		}

		public static int SetAmpTX2(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_AMP_TX2, Convert.ToUInt32(b), 0);
		}

		public static int SetAmpTX3(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_AMP_TX3, Convert.ToUInt32(b), 0);
		}

        //public static int SetXVTRActive(bool b)
        //{
        //    return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_XVTR_ACTIVE, Convert.ToUInt32(b), 0);
        //}

        public static int SetXVTRRXOn(bool b)                                                // ke9ns transverter ON/OFF
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_XVTR_RX_ON, Convert.ToUInt32(b), 0);
        }

        public static int SetXVTRTXOn(bool b)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_XVTR_TX_ON, Convert.ToUInt32(b), 0);
        }

		public static int SetXVTRSplit(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_XVTR_SPLIT, Convert.ToUInt32(b), 0);
		}

		public static int SetRX1Out(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_RX1OUT, Convert.ToUInt32(b), 0);
		}

		public static int FlexWire_WriteValue(byte addr, byte val)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_FLEXWIRE_WRITE_VALUE, addr, val);
		}

		public static int FlexWire_Write2Value(byte addr, byte v1, byte v2)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_FLEXWIRE_WRITE_2_VALUE, addr, (uint)((v1<<8)+v2));
		}

		public static int FlexWire_ReadValue(ushort addr, out uint val)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_FLEXWIRE_READ_VALUE, (uint)addr, 0, out val);
		}

		public static int FlexWire_Read2Value(ushort addr, out uint val)
		{
			return PalManager.Instance.ReadOp(Opcode.RDAL_OP_FLEXWIRE_READ_2_VALUE, (uint)addr, 0, out val);
		}

		public static int SetRX1DSPMode(DSPMode mode)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_RX1_DSP_MODE, (uint)mode, 0);
		}

		public static int SetRX2DSPMode(DSPMode mode)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_RX2_DSP_MODE, (uint)mode, 0);
		}

		public static int SetRX2On(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_RX2_ON, Convert.ToUInt32(b), 0);
		}

		public static int SetStandby(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_STANDBY, Convert.ToUInt32(b), 0);
		}

		public static int SetAmpTX1DelayEnable(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_AMP_TX1_DELAY_ENABLE, Convert.ToUInt32(b), 0);
		}

		public static int SetAmpTX2DelayEnable(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_AMP_TX2_DELAY_ENABLE, Convert.ToUInt32(b), 0);
		}

		public static int SetAmpTX3DelayEnable(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_AMP_TX3_DELAY_ENABLE, Convert.ToUInt32(b), 0);
		}

		public static int SetAmpTX1Delay(uint val)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_AMP_TX1_DELAY, val, 0);
		}

		public static int SetAmpTX2Delay(uint val)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_AMP_TX2_DELAY, val, 0);
		}

		public static int SetAmpTX3Delay(uint val)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_AMP_TX3_DELAY, val, 0);
		}

		public static int ResetRX2DDS()
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_RESET_RX2_DDS, 0, 0);
		}

		public static int SetRX2Preamp(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_RX2_PREAMP, Convert.ToUInt32(b), 0);
		}

		public static int SetIambic(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_IAMBIC, Convert.ToUInt32(b), 0);
		}

		public static int SetBreakIn(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_BREAK_IN, Convert.ToUInt32(b), 0);
		}

		public static int SetManualRX1Filter(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_MANUAL_RX1_FILTER, Convert.ToUInt32(b), 0);
		}

		public static int SetManualRX2Filter(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_MANUAL_RX2_FILTER, Convert.ToUInt32(b), 0);
		}

		public static int SetHiZ(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_HIZ, Convert.ToUInt32(b), 0);
		}		

		public static int SetPDrvMon(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_PDRVMON, Convert.ToUInt32(b), 0);
		}

		public static int SetATUEnable(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_ENABLE_ATU, Convert.ToUInt32(b), 0);
		}

		public static int SetATUATTN(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_ATU_ATTN, Convert.ToUInt32(b), 0);
		}

		public static int SetRXAttn(bool b)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_RX_ATTN, Convert.ToUInt32(b), 0);
		}

		public static int SetFanPWM(int on, int off)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_FAN_PWM, (uint)on, (uint)off);
		}

		public static int SetFanSpeed(float val)
		{
			return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_FAN_SPEED, val, 0);
		}

		public static int GetRegion(out FRSRegion region)                            // ke9ns find region programmed into your flex radio itself
		{
			uint data;
			int rtn = PalManager.Instance.ReadOp(Opcode.RDAL_OP_GET_REGION, 0, 0, out data);
			region = (FRSRegion)data;
			return rtn;
		}

        public static int SetTXDSPFilter(int low, int high)                           // ke9ns to set the upper and lower bounds to the Transmit passband (up to 90khz in either direction)
        {
            return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_TX_DSP_FILTER, low, high);
        }

        public static int SetTXOffset(int offset)                                    // ke9ns offset = (int)dsp.GetDSPTX(0).TXOsc;
        {
            return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_TX_OFFSET, offset, 0);
        }

        public static int SetTXDSPMode(DSPMode mode)                                  // ke9ns select your mode (USB,CW,FM, etc)
        {
            return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_TX_DSP_MODE, (uint)mode, 0);
        }

        public static int SetCWPitch(uint pitch)
        {
            return PalManager.Instance.WriteOp(Opcode.RDAL_OP_SET_CW_PITCH, pitch, 0);
        }

		public static void SetPalCallback()
		{
			PalManager.Instance.SetCallback(callback);
		}

        public static bool GetStatus()
        {
            uint status;
            int rtn = PalManager.Instance.ReadOp(Opcode.RDAL_OP_GET_STATUS, 0, 0, out status);

         //  Debug.WriteLine("1EXTENDED " + rtn + " , "+status);
            return (status != 0);
        }




        //=================================================================================
        //=================================================================================
        //=================================================================================
        //=================================================================================
        // ke9ns VHF/UHF module

        public static int SetVU_FanHigh(bool b)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_FAN_HIGH, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_KeyV(bool b)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_KEY_V, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_KeyU(bool b)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_KEY_U, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_K14(bool b)  //K15 TO K 14
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_K14, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_K15(bool b)  //K13 TO K15
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_K15, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_K16(bool b)  //K18 TO K16
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_K16, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_K17(bool b)  //K16 TO K17
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_K17, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_K18(bool b)  //K12 TO K18
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_K18, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_KeyVU(bool b)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_KEYVU, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_DrvU(bool b)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_DRVU, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_DrvV(bool b)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_DRVV, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_LPwrU(bool b)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_LPWRU, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_LPwrV(bool b)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_LPWRV, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_RX2V(bool b)  //RX2U TO RX2V
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_RX2V, Convert.ToUInt32(b), 0);
        }
        
        public static int SetVU_TXIFU(bool b)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_TXIFU, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_RXIFV(bool b)  //RXIFU to RXIFV (typo)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_RXIFV, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_UIFHG1(bool b)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_UIFHG1, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_VIFHG1(bool b)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_VIFHG1, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_UIFHG2(bool b)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_UIFHG2, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_RXURX2(bool b)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_RXURX2, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_VIFHG2(bool b)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_VIFHG2, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_RXV(bool b)  //RX2V TO RXV
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_RXV, Convert.ToUInt32(b), 0);
        }
       
        public static int SetVU_TXU(bool b)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_TXU, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_TXV(bool b)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_TXV, Convert.ToUInt32(b), 0);
        }

        //User modes
        public static int SetVU_modeVHGRX1(bool b)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_MODE_VHGRX1, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_modeVLG1(bool b)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_MODE_VLG1, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_modeTXVLP(bool b)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_MODE_TXVLP, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_modeTXV60W(bool b)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_MODE_TXV60W, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_VIFGain_PAEnable_00(bool b)
        {
            //return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_VIFGain_PAEnable_00, Convert.ToUInt32(b), 0);
            return 0;
        }

        public static int SetVU_VIFGain_PAEnable_01(bool b)
        {
            //return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_VIFGain_PAEnable_01, Convert.ToUInt32(b), 0);
            return 0;
        }

        public static int SetVU_VIFGain_PAEnable_10(bool b)
        {
           // return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_VIFGain_PAEnable_10, Convert.ToUInt32(b), 0);
            return 0;
        }

        public static int SetVU_VIFGain_PAEnable_11(bool b)
        {
            //return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_VIFGain_PAEnable_11, Convert.ToUInt32(b), 0);
            return 0;
        }

        //UHF
        public static int SetVU_UIFGain_PAEnable_00(bool b)
        {
            //return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_UIFGain_PAEnable_00, Convert.ToUInt32(b), 0);
            return 0;
        }

        public static int SetVU_UIFGain_PAEnable_01(bool b)
        {
            //return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_UIFGain_PAEnable_01, Convert.ToUInt32(b), 0);
            return 0;
        }

        public static int SetVU_UIFGain_PAEnable_10(bool b)
        {
            //return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_UIFGain_PAEnable_10, Convert.ToUInt32(b), 0);
            return 0;
        }

        public static int SetVU_UIFGain_PAEnable_11(bool b)
        {
            //return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_UIFGain_PAEnable_11, Convert.ToUInt32(b), 0);
            return 0;
        }

        public static int SetVU_VRX2Enable(bool b)
        {
            //return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_VRX2Enable, Convert.ToUInt32(b), 0);
            return 0;
        }

        public static int SetVU_URX2Enable(bool b)
        {
            //return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_URX2Enable, Convert.ToUInt32(b), 0);
            return 0;
        }

        //New
        public static int SetVU_RXPath(int b)
        {
           // Debug.WriteLine("*****RXPath(" + b + ")");
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_RXPATH, Convert.ToUInt32(b), 0);
        }         

        public static int SetVU_VIF(bool b)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_VIF, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_UIF(bool b)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_UIF, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_VPA(bool b)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_VPA, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_UPA(bool b)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_UPA, Convert.ToUInt32(b), 0);
        }

        public static int SetVU_TXBand(uint b)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_VU_TXBAND, b, 0);
        }

        // ke9ns VHF/UHF module above
        //=================================================================================
        //=================================================================================
        //=================================================================================
        //=================================================================================


        public static int DDSIOUpdate()
        {
            return PalManager.Instance.WriteOp(Opcode.RDAL_OP_UPDATE_DDS, 0, 0);
        }

        public static int SetManualIOUpdate(bool b)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_MANUAL_IO_UPDATE, Convert.ToUInt32(b), 0);
        }

        public static int SetRX2DisconnectOnTX(bool b)
        {
            return PalManager.Instance.WriteOp(Opcode.FWC_OP_SET_RX2_DISCONNECT, Convert.ToUInt32(b), 0);
        }

		private static NotificationCallback callback = new NotificationCallback(Notify);

        private static bool reverse_paddles = false;
        public static bool ReversePaddles
        {
            set { reverse_paddles = value; }
        }

        public static bool old_atu = false;


        public static bool ignore_dash = false;        
		public static Console console;
        public static uint last_bmp = 0;

		public static void Notify(uint bmp)
		{
#if(TIMING)
            //GPIOWrite(bmp & 0x01);
            if ((bmp & 0x01) != 0)
            {
                GPIOWrite(1);
                GPIOWrite(0);
            }
#else
            if (console == null) return;

			//Debug.WriteLine("FWCNotify: "+bmp.ToString("X"));
			bool temp = (bmp & 0x01) != 0; // dot
            /*if (temp)
            {
                console.sp.RtsEnable = true;
                //Thread.Sleep(10);
                //console.sp.RtsEnable = false;
            }*/
            
			//if(temp != console.Keyer.FWCDot) console.Keyer.FWCDot = temp;
            if((bmp & 0x01) != (last_bmp & 0x01))
            {
                CWSensorItem.InputType type = CWSensorItem.InputType.Dot;
                if (reverse_paddles) type = CWSensorItem.InputType.Dash;

                CWSensorItem item = new CWSensorItem(type, temp);
                CWKeyer.SensorEnqueue(item);
            }
            /*if (temp && !console.MOX && console.PowerOn)
            {
                if(console.CheckForTXCW())
                    Audio.MOX = true;
            }*/
 
			temp = (bmp & 0x02) != 0; // dash
            /*if (temp)
            {
                console.sp.RtsEnable = true;
                //Thread.Sleep(10);
                //console.sp.RtsEnable = false;
            }*/
            //if (!ignore_dash && temp != console.Keyer.FWCDash) console.Keyer.FWCDash = temp;
            if (!ignore_dash && ((bmp & 0x02) != (last_bmp & 0x02)))
            {
                CWSensorItem.InputType type = CWSensorItem.InputType.Dash;
                if (reverse_paddles) type = CWSensorItem.InputType.Dot;

                CWSensorItem item = new CWSensorItem(type, temp);
                CWKeyer.SensorEnqueue(item);
            }
            /*if (!ignore_dash && temp && !console.MOX && console.PowerOn)
            {
                if (console.CheckForTXCW())
                Audio.MOX = true;
            }*/

			temp = (bmp & 0x04) != 0; // RCA PTT
			if(temp != console.FWCRCAPTT) console.FWCRCAPTT = temp;
 
			temp = (bmp & 0x08) != 0;
			if(temp != console.FWCMicPTT) console.FWCMicPTT = temp;
 
			temp = (bmp & 0x10) != 0;
			if(temp != console.MicDown) console.MicDown = temp;

			temp = (bmp & 0x20) != 0;
			if(temp != console.MicUp) console.MicUp = temp;

			if((bmp & 0x40) != 0) console.MicFast = !console.MicFast;

            last_bmp = bmp;
#endif
        }

		#endregion
	}
}
