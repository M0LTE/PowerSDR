using PowerSDR;
using System;
using System.Collections.Generic;

namespace PowerSDRServer
{
    public class InteropPal : IPal
    {
        public int GetNumDevices()
        {
            return PalInteropMethods.GetNumDevices();
        }

        public bool GetDeviceInfo(uint index, out uint model, out uint sn)
        {
            return PalInteropMethods.GetDeviceInfo(index, out model, out sn);
        }

        public void Exit()
        {
            PalInteropMethods.Exit();
        }

        public bool Init()
        {
            return PalInteropMethods.Init();
        }

        public int ReadOp(Opcode opcode, uint data1, uint data2, out uint rtn)
        {
            return PalInteropMethods.ReadOp(opcode, data1, data2, out rtn);
        }

        public int ReadOp(Opcode opcode, uint data1, uint data2, out float rtn)
        {
            return PalInteropMethods.ReadOp(opcode, data1, data2, out rtn);
        }

        public Radio[] Scan()
        {
            return PalInteropMethods.Scan();
        }

        public bool SelectDevice(uint index)
        {
            return PalInteropMethods.SelectDevice(index);
        }

        public void SetBufferSize(uint val)
        {
            PalInteropMethods.SetBufferSize(val);
        }

        public void SetCallback(NotificationCallback callback)
        {
            PalInteropMethods.SetCallback(callback);
        }

        public int WriteOp(Opcode opcode, uint data1, uint data2)
        {
            return PalInteropMethods.WriteOp(opcode, data1, data2);
        }

        public int WriteOp(Opcode opcode, float data1, uint data2)
        {
            return PalInteropMethods.WriteOp(opcode, data1, data2);
        }

        public int WriteOp(Opcode opcode, int data1, int data2)
        {
            return PalInteropMethods.WriteOp(opcode, data1, data2);
        }

        public int WriteOp(Opcode opcode, uint data1, float data2)
        {
            return PalInteropMethods.WriteOp(opcode, data1, data2);
        }

        public void Configure(Dictionary<string, string> settings)
        {
            if (settings != null && settings.Count > 0)
            {
                throw new InvalidOperationException($"{nameof(InteropPal)} doesn't need settings, did you configure the wrong {nameof(IPal)} implementation?");
            }
        }
    }
}