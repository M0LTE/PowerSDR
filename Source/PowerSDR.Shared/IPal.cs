using System.Collections.Generic;

namespace PowerSDR
{
    public interface IPal
    {
        void Configure(Dictionary<string, string> settings);
        int GetNumDevices();
        bool GetDeviceInfo(uint index, out uint model, out uint sn);
        void Exit();
        bool Init();
        int ReadOp(Opcode opcode, uint data1, uint data2, out uint rtn);
        int ReadOp(Opcode opcode, uint data1, uint data2, out float rtn);
        Radio[] Scan();
        bool SelectDevice(uint index);
        void SetBufferSize(uint val);
        void SetCallback(NotificationCallback callback);
        int WriteOp(Opcode opcode, uint data1, uint data2);
        int WriteOp(Opcode opcode, float data1, uint data2);
        int WriteOp(Opcode opcode, int data1, int data2);
        int WriteOp(Opcode opcode, uint data1, float data2);
    }

    public delegate void NotificationCallback(uint bitmap);
}