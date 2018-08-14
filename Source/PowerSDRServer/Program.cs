using Newtonsoft.Json;
using PowerSDR;
using PowerSDR.Shared;
using System;
using ZeroMQ;

namespace PowerSDRServer
{
    class Program
    {
        private static readonly JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        private static readonly NotificationCallback notificationCallback = (uint bitmap) => {
            Console.WriteLine($"--> {nameof(NotificationResponse)}");

            Send(publisher, new NotificationResponse { Bitmap = bitmap });
        };

        static ZSocket responder;
        static ZSocket publisher;

        static void Main(string[] args)
        {
            //TODO: Implement a (presumably) high performance server which somehow exposes the operations on IPal over the network, executes those operations against InteropPal, and returns results when relevant.

            InteropPal ip = new InteropPal();

            using (var context = new ZContext())
            using (responder = new ZSocket(context, ZSocketType.REP))
            {
                // Bind
                responder.Bind("tcp://*:5555");
                while (true)
                {
                    // Receive
                    using (ZFrame request = responder.ReceiveFrame())
                    {
                        string requestJson = request.ReadString();

                        var palMessage = JsonConvert.DeserializeObject<PalMessage>(requestJson, settings);

                        if (palMessage is InitMessage)
                        {
                            Console.WriteLine($"<-- {nameof(InitMessage)}");

                            Send(responder, new InitResponse { Result = ip.Init() });
                        }
                        else if (palMessage is GetNumDevicesMessage)
                        {
                            Console.WriteLine($"<-- {nameof(GetNumDevicesMessage)}");

                            Send(responder, new GetNumDevicesResponse { Result = ip.GetNumDevices() });
                        }
                        else if (palMessage is GetDeviceInfoMessage msg1)
                        {
                            Console.WriteLine($"<-- {nameof(GetDeviceInfoMessage)}");

                            bool result = ip.GetDeviceInfo(msg1.Index, out uint model, out uint sn);

                            Send(responder, new GetDeviceInfoResponse { Result = result, Model = model, Sn = sn });
                        }
                        else if (palMessage is SelectDeviceMessage msg2)
                        {
                            Console.WriteLine($"<-- {nameof(SelectDeviceMessage)}");

                            Send(responder, new SelectDeviceResponse { Result = ip.SelectDevice(msg2.Index) });
                        }
                        else if (palMessage is SetCallbackMessage msg10)
                        {
                            Console.WriteLine($"<-- {nameof(SetCallbackMessage)}");

                            ip.SetCallback(notificationCallback);

                            var ctx = new ZContext();
                            publisher = new ZSocket(ctx, ZSocketType.PUB);
                            publisher.Bind("tcp://*:5556");

                            Send(responder, new PalResponse());
                        }
                        else if (palMessage is ReadOp1Message msg3)
                        {
                            Console.WriteLine($"<-- {nameof(ReadOp1Message)}");
                            int result = ip.ReadOp1(msg3.Opcode, msg3.Data1, msg3.Data2, out uint rtn);

                            Send(responder, new ReadOp1Response { Result = result, Rtn = rtn });
                        }
                        else if (palMessage is ReadOp2Message msg4)
                        {
                            Console.WriteLine($"<-- {nameof(ReadOp2Message)}");

                            int result = ip.ReadOp2(msg4.Opcode, msg4.Data1, msg4.Data2, out float rtn);

                            Send(responder, new ReadOp2Response { Result = result, Rtn = rtn });
                        }
                        else if (palMessage is WriteOp1Message msg5)
                        {
                            Console.WriteLine($"<-- {nameof(WriteOp1Message)}");

                            int result = ip.WriteOp1(msg5.Opcode, msg5.Data1, msg5.Data2);

                            Send(responder, new WriteOp1Response { Result = result });
                        }
                        else if (palMessage is WriteOp2Message msg6)
                        {
                            Console.WriteLine($"<-- {nameof(WriteOp2Message)}");

                            int result = ip.WriteOp2(msg6.Opcode, msg6.Data1, msg6.Data2);

                            Send(responder, new WriteOp2Response { Result = result });
                        }
                        else if (palMessage is WriteOp3Message msg7)
                        {
                            Console.WriteLine($"<-- {nameof(WriteOp3Message)}");

                            int result = ip.WriteOp3(msg7.Opcode, msg7.Data1, msg7.Data2);

                            Send(responder, new WriteOp3Response { Result = result });
                        }
                        else if (palMessage is WriteOp4Message msg8)
                        {
                            Console.WriteLine($"<-- {nameof(WriteOp4Message)}");

                            int result = ip.WriteOp4(msg8.Opcode, msg8.Data1, msg8.Data2);

                            Send(responder, new WriteOp4Response { Result = result });
                        }
                        else if (palMessage is SetBufferSizeMessage msg9)
                        {
                            Console.WriteLine($"<-- {nameof(SetBufferSizeMessage)}");

                            ip.SetBufferSize(msg9.Val);

                            Send(responder, new PalResponse());
                        }
                        else if (palMessage is ExitMessage)
                        {
                            Console.WriteLine($"<-- {nameof(ExitMessage)}");

                            ip.Exit();

                            Send(responder, new PalResponse());
                        }
                        else
                        {
                            Console.WriteLine("Not implemented: " + palMessage.GetType().Name);
                        }
                    }
                }
            }
        }

        private static void Send(ZSocket responder, PalResponse msg)
        {
            Console.WriteLine($"--> {msg.GetType().Name}");

            string responseJson = JsonConvert.SerializeObject(msg, settings);

            responder.Send(new ZFrame(responseJson));
        }
    }
}
