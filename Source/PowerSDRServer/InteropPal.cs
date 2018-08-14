using PowerSDR;
using PowerSDR.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PowerSDRServer
{
    public class InteropPal : CommonPal, IPal
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

        public int ReadOp1(Opcode opcode, uint data1, uint data2, out uint rtn)
        {
            return PalInteropMethods.ReadOp(opcode, data1, data2, out rtn);
        }

        public int ReadOp2(Opcode opcode, uint data1, uint data2, out float rtn)
        {
            return PalInteropMethods.ReadOp(opcode, data1, data2, out rtn);
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

        public int WriteOp1(Opcode opcode, uint data1, uint data2)
        {
            return PalInteropMethods.WriteOp(opcode, data1, data2);
        }

        public int WriteOp2(Opcode opcode, float data1, uint data2)
        {
            return PalInteropMethods.WriteOp(opcode, data1, data2);
        }

        public int WriteOp3(Opcode opcode, int data1, int data2)
        {
            return PalInteropMethods.WriteOp(opcode, data1, data2);
        }

        public int WriteOp4(Opcode opcode, uint data1, float data2)
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

        public void Dispose()
        {
            // NOOP
        }
    }
}