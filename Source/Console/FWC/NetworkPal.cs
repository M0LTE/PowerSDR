using System;
using System.Collections.Generic;

namespace PowerSDR
{
    public class NetworkPal : IPal
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Key { get; set; }
        bool configured;

        public void Configure(Dictionary<string, string> settings)
        {
            if (settings.TryGetValue("server", out string server))
            {
                Server = server;
            }
            else
            {
                throw new InvalidOperationException($"{nameof(NetworkPal)} needs configuration setting \"server\" passing in but it was not supplied");
            }

            if (settings.TryGetValue("port", out string port) && int.TryParse(port, out int iport))
            {
                Port = iport;
            }
            else
            {
                throw new InvalidOperationException($"{nameof(NetworkPal)} needs configuration setting \"port\" passing in but it was not supplied or was invalid");
            }

            if (settings.TryGetValue("key", out string key))
            {
                Key = key;
            }
            else
            {
                throw new InvalidOperationException($"{nameof(NetworkPal)} needs configuration setting \"key\" passing in but it was not supplied");
            }

            configured = true;
        }

        void CheckConfig()
        {
            if (!configured)
            {
                throw new InvalidOperationException($"Call {nameof(Configure)} first");
            }
        }

        public void Exit()
        {
            CheckConfig();
            throw new NotImplementedException();
        }

        public bool GetDeviceInfo(uint index, out uint model, out uint sn)
        {
            CheckConfig();
            throw new NotImplementedException();
        }

        public int GetNumDevices()
        {
            CheckConfig();
            throw new NotImplementedException();
        }

        public bool Init()
        {
            CheckConfig();
            throw new NotImplementedException();
        }

        public int ReadOp(Opcode opcode, uint data1, uint data2, out uint rtn)
        {
            CheckConfig();
            throw new NotImplementedException();
        }

        public int ReadOp(Opcode opcode, uint data1, uint data2, out float rtn)
        {
            CheckConfig();
            throw new NotImplementedException();
        }

        public Radio[] Scan()
        {
            CheckConfig();
            throw new NotImplementedException();
        }

        public bool SelectDevice(uint index)
        {
            CheckConfig();
            throw new NotImplementedException();
        }

        public void SetBufferSize(uint val)
        {
            CheckConfig();
            throw new NotImplementedException();
        }

        public void SetCallback(NotificationCallback callback)
        {
            CheckConfig();
            throw new NotImplementedException();
        }

        public int WriteOp(Opcode opcode, uint data1, uint data2)
        {
            CheckConfig();
            throw new NotImplementedException();
        }

        public int WriteOp(Opcode opcode, float data1, uint data2)
        {
            CheckConfig();
            throw new NotImplementedException();
        }

        public int WriteOp(Opcode opcode, int data1, int data2)
        {
            CheckConfig();
            throw new NotImplementedException();
        }

        public int WriteOp(Opcode opcode, uint data1, float data2)
        {
            CheckConfig();
            throw new NotImplementedException();
        }
    }
}
