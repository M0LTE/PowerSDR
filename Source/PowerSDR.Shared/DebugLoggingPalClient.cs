using System;
using System.Collections.Generic;

namespace PowerSDR.Shared
{
    public class DebugLoggingPalClient : IPal
    {
        IPal innerPal;
        Action<string> logDelegate;

        public DebugLoggingPalClient(IPal innerClient, Action<string> logDelegate)
        {
            this.innerPal = innerClient;
            this.logDelegate = logDelegate;
        }

        public void Configure(Dictionary<string, string> settings)
        {
            logDelegate($"{nameof(DebugLoggingPalClient)}.{nameof(Configure)}");

            innerPal.Configure(settings);
        }

        public void Dispose()
        {
            innerPal.Dispose();
        }

        public void Exit()
        {
            logDelegate($"{nameof(DebugLoggingPalClient)}.{nameof(Exit)}");

            innerPal.Exit();
        }

        public bool GetDeviceInfo(uint index, out uint model, out uint sn)
        {
            logDelegate($"{nameof(DebugLoggingPalClient)}.{nameof(GetDeviceInfo)}");

            return innerPal.GetDeviceInfo(index, out model, out sn);
        }

        public int GetNumDevices()
        {
            logDelegate($"{nameof(DebugLoggingPalClient)}.{nameof(GetNumDevices)}");

            return innerPal.GetNumDevices();
        }

        public bool Init()
        {
            logDelegate($"{nameof(DebugLoggingPalClient)}.{nameof(Init)}");

            return innerPal.Init();
        }

        public int ReadOp1(Opcode opcode, uint data1, uint data2, out uint rtn)
        {
            logDelegate($"{nameof(DebugLoggingPalClient)}.{nameof(ReadOp1)}");

            return innerPal.ReadOp1(opcode, data1, data2, out rtn);
        }

        public int ReadOp2(Opcode opcode, uint data1, uint data2, out float rtn)
        {
            logDelegate($"{nameof(DebugLoggingPalClient)}.{nameof(ReadOp2)}");

            return innerPal.ReadOp2(opcode, data1, data2, out rtn);
        }

        public SimpleRadio[] Scan()
        {
            logDelegate($"{nameof(DebugLoggingPalClient)}.{nameof(Scan)}");

            return innerPal.Scan();
        }

        public bool SelectDevice(uint index)
        {
            logDelegate($"{nameof(DebugLoggingPalClient)}.{nameof(SelectDevice)}");

            return innerPal.SelectDevice(index);
        }

        public void SetBufferSize(uint val)
        {
            logDelegate($"{nameof(DebugLoggingPalClient)}.{nameof(SetBufferSize)}");

            innerPal.SetBufferSize(val);
        }

        public void SetCallback(NotificationCallback callback)
        {
            logDelegate($"{nameof(DebugLoggingPalClient)}.{nameof(SetCallback)}");

            innerPal.SetCallback(callback);
        }

        public int WriteOp1(Opcode opcode, uint data1, uint data2)
        {
            logDelegate($"{nameof(DebugLoggingPalClient)}.{nameof(WriteOp1)}");

            return innerPal.WriteOp1(opcode, data1, data2);
        }

        public int WriteOp2(Opcode opcode, float data1, uint data2)
        {
            logDelegate($"{nameof(DebugLoggingPalClient)}.{nameof(WriteOp2)}");

            return innerPal.WriteOp2(opcode, data1, data2);
        }

        public int WriteOp3(Opcode opcode, int data1, int data2)
        {
            logDelegate($"{nameof(DebugLoggingPalClient)}.{nameof(WriteOp3)}");

            return innerPal.WriteOp3(opcode, data1, data2);
        }

        public int WriteOp4(Opcode opcode, uint data1, float data2)
        {
            logDelegate($"{nameof(DebugLoggingPalClient)}.{nameof(WriteOp4)}");

            return innerPal.WriteOp4(opcode, data1, data2);
        }
    }
}
