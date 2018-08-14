using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ZeroMQ;

namespace PowerSDR.Shared
{
    public class NetworkPalClient : CommonPal, IPal
    {
        public string Server { get; set; }
        public int Port { get; set; }
        bool configured;

        public NetworkPalClient()
        {
        }

        ZContext context;
        ZSocket requester;

        public void Configure(Dictionary<string, string> settings)
        {
            if (settings.TryGetValue("server", out string server))
            {
                Server = server;
            }
            else
            {
                throw new InvalidOperationException($"{nameof(NetworkPalClient)} needs configuration setting \"server\" passing in but it was not supplied");
            }

            if (settings.TryGetValue("port", out string port) && int.TryParse(port, out int iport))
            {
                Port = iport;
            }
            else
            {
                throw new InvalidOperationException($"{nameof(NetworkPalClient)} needs configuration setting \"port\" passing in but it was not supplied or was invalid");
            }

            context = new ZContext();
            requester = new ZSocket(context, ZSocketType.REQ);
            requester.Connect($"tcp://{server}:{port}");

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

            var exitMessage = new ExitMessage();
            Send(exitMessage);
        }

        static readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        static object lockObj = new object();
        private PalResponse Send(PalMessage message)
        {
            lock (lockObj)
            {
                requester.Send(new ZFrame(JsonConvert.SerializeObject(message, jsonSettings)));

                /*if (message is SetCallbackMessage)
                {
                    return null;
                }
                else*/
                {
                    using (ZFrame reply = requester.ReceiveFrame())
                    {
                        string replyJson = reply.ReadString();

                        PalResponse deserialised = JsonConvert.DeserializeObject<PalResponse>(replyJson, jsonSettings);

                        return deserialised;
                    }
                }
            }
        }

        public bool GetDeviceInfo(uint index, out uint model, out uint sn)
        {
            CheckConfig();
            var msg = new GetDeviceInfoMessage(index);
            var response = (GetDeviceInfoResponse)Send(msg);
            model = response.Model;
            sn = response.Sn;
            return response.Result;
        }

        public int GetNumDevices()
        {
            CheckConfig();
            var msg = new GetNumDevicesMessage();
            var response = (GetNumDevicesResponse)Send(msg);
            return response.Result;
        }

        public bool Init()
        {
            CheckConfig();
            var msg = new InitMessage();
            var response = (InitResponse)Send(msg);
            return response.Result;
        }

        public int ReadOp1(Opcode opcode, uint data1, uint data2, out uint rtn)
        {
            CheckConfig();
            var msg = new ReadOp1Message(opcode, data1, data2);
            var response = (ReadOp1Response)Send(msg);
            rtn = response.Rtn;
            return response.Result;
        }

        public int ReadOp2(Opcode opcode, uint data1, uint data2, out float rtn)
        {
            CheckConfig();
            var msg = new ReadOp2Message(opcode, data1, data2);
            var response = (ReadOp2Response)Send(msg);
            rtn = response.Rtn;
            return response.Result;
        }

        public bool SelectDevice(uint index)
        {
            CheckConfig();
            var msg = new SelectDeviceMessage(index);
            var response = (SelectDeviceResponse)Send(msg);
            return response.Result;
        }

        public void SetBufferSize(uint val)
        {
            CheckConfig();
            var msg = new SetBufferSizeMessage(val);
            Send(msg);
        }

        NotificationCallback notificationCallback;
        public void SetCallback(NotificationCallback callback)
        {
            if (notificationCallback != null)
            {
                throw new NotImplementedException("Safeguard against the application being designed in a more difficult way than easiest case");
            }

            CheckConfig();

            Task.Factory.StartNew(() =>
            {
                // wait for callback

                using (var ctx = new ZContext())
                using (var subscriber = new ZSocket(ctx, ZSocketType.SUB))
                {
                    subscriber.Connect("tcp://127.0.0.1:5556");
                    subscriber.Subscribe("notificationCallback");
                    while (true)
                    {
                        using (var replyFrame = subscriber.ReceiveFrame())
                        {
                            string json = replyFrame.ReadString();

                            PalResponse deserialised = JsonConvert.DeserializeObject<PalResponse>(json, jsonSettings);

                            if (deserialised is NotificationResponse msg1)
                            {
                                notificationCallback(msg1.Bitmap);
                            }
                            else
                            {
                                throw new NotImplementedException("Bad notificationCallback");
                            }
                        }
                    }
                }
            });

            // tell the far side that a callback delegate has been set. Don't actually pass the delegate, since that would be meaningless.
            var msg = new SetCallbackMessage();
            Send(msg);

            // keep a reference to the delegate, such that when we receive callbacks, by whatever means, this is the method we call.
            notificationCallback = callback;

#warning Only supports a single caller to this method. This might not be adequate, depending on application design. Test.
        }

        public int WriteOp1(Opcode opcode, uint data1, uint data2)
        {
            CheckConfig();
            var msg = new WriteOp1Message(opcode, data1, data2);
            var response = (WriteOp1Response)Send(msg);
            return response.Result;
        }

        public int WriteOp2(Opcode opcode, float data1, uint data2)
        {
            CheckConfig();
            var msg = new WriteOp2Message(opcode, data1, data2);
            var response = (WriteOp2Response)Send(msg);
            return response.Result;
        }

        public int WriteOp3(Opcode opcode, int data1, int data2)
        {
            CheckConfig();
            var msg = new WriteOp3Message(opcode, data1, data2);
            var response = (WriteOp3Response)Send(msg);
            return response.Result;
        }

        public int WriteOp4(Opcode opcode, uint data1, float data2)
        {
            CheckConfig();
            var msg = new WriteOp4Message(opcode, data1, data2);
            var response = (WriteOp4Response)Send(msg);
            return response.Result;
        }

        public void Dispose()
        {
            requester.Dispose();
            context.Dispose();
        }
    }
}
