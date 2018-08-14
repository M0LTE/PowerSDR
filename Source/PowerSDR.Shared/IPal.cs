using System;
using System.Collections.Generic;

namespace PowerSDR.Shared
{
    public interface IPal : IDisposable
    {
        void Configure(Dictionary<string, string> settings);
        int GetNumDevices();
        bool GetDeviceInfo(uint index, out uint model, out uint sn);
        void Exit();
        bool Init();
        int ReadOp1(Opcode opcode, uint data1, uint data2, out uint rtn);
        int ReadOp2(Opcode opcode, uint data1, uint data2, out float rtn);
        SimpleRadio[] Scan();
        bool SelectDevice(uint index);
        void SetBufferSize(uint val);
        void SetCallback(NotificationCallback callback);
        int WriteOp1(Opcode opcode, uint data1, uint data2);
        int WriteOp2(Opcode opcode, float data1, uint data2);
        int WriteOp3(Opcode opcode, int data1, int data2);
        int WriteOp4(Opcode opcode, uint data1, float data2);
    }

    public delegate void NotificationCallback(uint bitmap);
}