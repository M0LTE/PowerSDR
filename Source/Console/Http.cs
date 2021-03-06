﻿//=================================================================
// Http.cs
//=================================================================
// Http Server
//
// Николай  RN3KK
// Darrin ke9ns
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

using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Collections;
using System.Data;
using System.ComponentModel;
using System.IO.Ports;
using TDxInput;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using Microsoft.JScript;
using System.Net.Http;
using System.Threading.Tasks;


//using System.Runtime.Remoting.Contexts;

namespace PowerSDR
{
    sealed public class Http
    {
        public static Console console;   // ke9ns mod  to allow console to pass back values to setup screen
        public static Setup setupForm;          // ke9ns communications with setupform  (i.e. allow combometertype.text update from inside console.cs) 

        public static TcpListener m_listener;

        private const String IMAGE_REQUEST = "/image";

        enum RequestType
        {
            GET_IMAGE,
            GET_HTML_INDEX_PAGE,
            UNKNOWN,
            ERROR
        }
        public Http(Console c)
        {
            console = c;

        }




        //=========================================================
        public string Weather()
        {
            var wthr = WeatherAAsync().Result;
            return wthr.ToString();
        }


        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        // ke9ns  ASYNC 
        public async Task<string> WeatherAAsync()
        {

            Debug.WriteLine("GET Real weather data=========");


            string content1 = " ";

            if (console.SpotForm != null)
            {
                if (((int)console.SpotForm.udDisplayLat.Value > 25) && ((int)console.SpotForm.udDisplayLat.Value < 51))
                {
                    if (((int)console.SpotForm.udDisplayLong.Value > -120) && ((int)console.SpotForm.udDisplayLong.Value < -73))
                    {
                        Debug.WriteLine("GOOD LAT AND LONG weather data=========");

                        string latitude = console.SpotForm.udDisplayLat.Value.ToString("##0.00").PadLeft(5);   // -90.00
                        string longitude = console.SpotForm.udDisplayLong.Value.ToString("###0.00").PadLeft(6);  // -180.00 

                        var url = new Uri("http://forecast.weather.gov/MapClick.php?lat=" + latitude + "&lon=" + longitude + "&FcstType=dwml");


                        HttpClient client = new HttpClient();

                        try
                        {
                            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Stackoverflow/1.0");
                        }
                        catch (Exception g)
                        {
                            Debug.WriteLine("http client user agent fail " + g);
                        }

                        Debug.WriteLine("GOOD LAT AND LONG weather data1=========" + url);

                        try
                        {
                            var xml = await client.GetStringAsync(url).ConfigureAwait(false);
                            content1 = xml.ToString();
                            client.Dispose();
                            return content1;

                        }
                        catch (Exception g)
                        {
                            content1 = "Error " + g.ToString();
                            client.Dispose();
                            return content1;
                        }

                    } // SpotForm.udDisplayLong.Value
                    else Debug.WriteLine("LAT not good=========");


                } //   if (((int)SpotForm.udDisplayLong.Value > -120) && ((int)SpotForm.udDisplayLong.Value < -73))
                else Debug.WriteLine("LONG not good=========");


            } // SpotForm.udDisplayLat.Value > 29)  && ((int)SpotForm.udDisplayLat.Value < 49 ))
            else Debug.WriteLine("Spotform not open=========");

            console.LOCALWEATHER = false;
            Console.noaaON = 1;

            return content1;
        } // aync weather data




        //=========================================================================================
        // ke9ns  ASYNC Communicate with ARRL LoTW server to get result if you have made contact with this station before
        //=========================================================
      

        // this gets ALL your LoTW XML contact Data EVER made
        public string Lotw1()
        {
            if (console.SpotForm != null)
            {
                var lotw = LotwAAsync().Result;
                return lotw.ToString(); // return XML data
            }
            else
            {
                Debug.WriteLine("LoTW NOT READY");
                return "NOT READY";

            }
        } //Lotw1()



        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        // ke9ns  ASYNC Communicate with ARRL LoTW server to get ALL YOUR uploaded CONTACTS EVER MADE
        public async Task<string> LotwAAsync()
        {

            Debug.WriteLine("LoTW TOTAL ASYNC NOW=========");

            string content1 = "NOT READY";


            // This grabs ALL your LoTW QSL that you have uploaded (including those that the other person has not confirmed)

            // this only gets confirmed QSL's
            //  var url = new Uri("https://lotw.arrl.org/lotwuser/lotwreport.adi?login=" + console.SpotForm.callBox.Text + "&password=" + console.SpotForm.LoTWPASS + "&qso_qsldetail=yes" + "&qso_query=1");

            // this gets all your QSO's (confirmed and unconfirmed), but they dont always have DXCC entity numbers, etc., etc.
            var url = new Uri("https://lotw.arrl.org/lotwuser/lotwreport.adi?login=" + console.SpotForm.callBox.Text + "&password=" + console.SpotForm.LoTWPASS + "&qso_qsldetail=yes" + "&qso_qsl=no" + "&qso_query=1");

            HttpClient client = new HttpClient();

            try
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Stackoverflow/1.0");
            }
            catch (Exception g)
            {
                Debug.WriteLine("https client user agent fail " + g);
                console.SpotForm.LoTWResult = 3;
                console.SpotForm.LoTWDone = true;
                return content1;
            }

            Debug.WriteLine("GOOD LoTW data1=========" + url);

            try
            {
                var xml = await client.GetStringAsync(url).ConfigureAwait(false);
                content1 = xml.ToString();
                client.Dispose();

                string file_name2 = console.AppDataPath + "LoTW_LOG.adi"; // save your LoTW_LOG file

                FileStream stream2 = new FileStream(file_name2, FileMode.Create); // open  file
             
                BinaryWriter writer2 = new BinaryWriter(stream2);

                writer2.Write(content1);


                writer2.Close();    // close  file
                stream2.Close();   // close stream


                console.SpotForm.LoTWResult = 2; // good
                console.SpotForm.LoTWDone = true;

                return content1;

            }
            catch (Exception g)
            {
                content1 = "Error " + g.ToString();
                client.Dispose();
                console.SpotForm.LoTWResult = 3;
                console.SpotForm.LoTWDone = true;
                return content1;
            }

        } //   public async Task<string> LotwAAsync1()




        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        public void HttpServer1()
        {

            try
            {
                m_listener = new TcpListener(IPAddress.Any, console.HTTP_PORT);

            }
            catch (Exception e)
            {

                Debug.WriteLine("7exception" + e);
                return;

            }


            Console.m_terminated = false;

            Thread t = new Thread(new ThreadStart(TCPSERVER));
            t.Name = "TCP SERVER THREAD";
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            t.Start();

        } // httpserver()


        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //========================================================================================
        // ke9ns add  THREAD
        private void TCPSERVER()
        {
            try
            {
                m_listener.Start();


            }
            catch (Exception e)
            {
                Debug.WriteLine("Cannot start thread " + e);

                terminate();
            }

            Debug.WriteLine("LISTENER STARTED");


            while (!Console.m_terminated)
            {

                try
                {
                    Thread.Sleep(50);


                    TcpClient tempClient = getHandler(m_listener.AcceptTcpClient());

                    //   TcpClient client = m_listener.AcceptTcpClient();
                    //   string ip = ((IPEndPoint)m_listener.Server.LocalEndPoint).Address.ToString();
                    //    TcpClient tempClient = getHandler(client);

                    if (TcpType != 0)
                    {
                        if (TcpType == 1)
                        {

                            ImageRequest(tempClient);
                          //  AudioRequest(tempClient);
                        }
                        else if (TcpType == 2)
                        {
                            WebPageRequest(tempClient);
                        }
                        else
                        {
                            UnknownRequest(tempClient);
                        }
                    }

                }
                catch (Exception e)
                {
                    Debug.WriteLine("get TCP RECEIVE fault " + e);

                    try
                    {
                        m_listener.Stop(); // try and close the getcontext thread
                        m_listener.Start();
                    }
                    catch (Exception e1)
                    {
                        Debug.WriteLine("close THREAD " + e1);
                        break;
                    }


                }

                Thread.Sleep(50);

            } //while (!m_terminated)


            console.URLPRESENT = false;

        } // TCPSERVER() THREAD


        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        public static void terminate()
        {
            Console.m_terminated = true;
            console.URLPRESENT = false;
            try
            {
                m_listener.Stop(); // try and close the getcontext thread

            }
            catch (Exception e)
            {
                Debug.WriteLine("close THREAD " + e);
            }
        }

        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================

        public static int TcpType = 0;

        public static TcpClient getHandler(TcpClient tcpClient)
        {
            switch (getType(tcpClient))
            {
                case RequestType.GET_IMAGE:   //  ImageRequest(tempClient);
                    TcpType = 1;
                    return tcpClient;
                case RequestType.GET_HTML_INDEX_PAGE: //  WebPageRequest(tempClient);
                    TcpType = 2;
                    return tcpClient;
                case RequestType.UNKNOWN: //  UnknownRequest(tempClient);
                    TcpType = 3;
                    return tcpClient;
            }

            TcpType = 0;
            return tcpClient;

        } // TcpClient getHandler


        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================

        private static RequestType getType(TcpClient tcpClient)
        {
            string Request = "";
            byte[] Buffer = new byte[1024];
            int Count;

            while ((Count = tcpClient.GetStream().Read(Buffer, 0, Buffer.Length)) > 0)
            {
                Request += Encoding.ASCII.GetString(Buffer, 0, Count);
                if (Request.IndexOf("\r\n\r\n") >= 0 || Request.Length > 4096)
                {
                    break;
                }
            }

            Match ReqMatch = Regex.Match(Request, @"^\w+\s+([^\s\?]+)[^\s]*\s+HTTP/.*|");

            if (ReqMatch == Match.Empty)
            {
                SendError(tcpClient, 400);
                return RequestType.ERROR;
            }

            string RequestUri = ReqMatch.Groups[1].Value;

            Debug.WriteLine("URI " + RequestUri);

            RequestUri = Uri.UnescapeDataString(RequestUri);

            if (RequestUri.IndexOf("..") >= 0)
            {
                SendError(tcpClient, 400);
                return RequestType.ERROR;
            }

            else if (RequestUri.CompareTo(IMAGE_REQUEST) == 0)  // /image
            {
                return RequestType.GET_IMAGE;
            }

            else if (RequestUri.CompareTo("/") == 0)
            {
                // return RequestType.GET_IMAGE;

                return RequestType.GET_HTML_INDEX_PAGE;
            }

            return RequestType.UNKNOWN;

        } // private static RequestType getType(TcpClient tcpClient)


        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================


        public void ImageRequest(TcpClient m_tcpClient)
        {

            if (m_tcpClient == null) return;

            Debug.WriteLine("IMAGEREQUEST1");

            if (console.URLPRESENT == false) console.URLPRESENT = true; // ke9ns let the setup HTTP server know that someone is requesting an image


            byte[] imageArray = console.getImage(); // ke9ns this gets either the Spectral Display or the entire Console widow and puts it into a jpeg byte array

         

            if (imageArray == null) // if we dont have an image, let the requestor know we dont have an image to send.
            {
                string CodeStr = "500 " + ((System.Net.HttpStatusCode)500).ToString();

                string Html = "<html><body><h1>" + CodeStr + "</h1></body></html>";

                string Str = "HTTP/1.1 " + CodeStr + "\nContent-type: text/html\nContent-Length:" + Html.Length.ToString() + "\n\n" + Html;

                byte[] Buffer = Encoding.ASCII.GetBytes(Str);

                m_tcpClient.GetStream().Write(Buffer, 0, Buffer.Length);
                m_tcpClient.Close();
                return;
            }

            //  "<meta http-equiv= \"refresh\" content= \"500\" > \r\n" +

            string responseHeaders = "HTTP/1.1 200 The file is coming right up!\r\n" +
                                       "Server: MyOwnServer\r\n" +
                                       "Content-Length: " + imageArray.Length + "\r\n" +
                                       "Content-Type: image/jpeg\r\n" +
                                       "Content-Disposition: inline;filename=\"picDisplay.jpg;\"\r\n" +
                                       "\r\n";



            byte[] headerArray = Encoding.ASCII.GetBytes(responseHeaders); // convert responseheader into byte array


            NetworkStream stream1 = m_tcpClient.GetStream(); // create a stream to send/receive data over the TCP/IP connection

            stream1.Write(headerArray, 0, headerArray.Length); // send header
            stream1.Write(imageArray, 0, imageArray.Length);   // send image


            stream1.Close();

            m_tcpClient.Close();


        } // ImageRequest()


        public void AudioRequest(TcpClient m_tcpClient)
        {

            if (m_tcpClient == null) return;

            Debug.WriteLine("AudioREQUEST1");

            if (console.URLPRESENT == false) console.URLPRESENT = true; // ke9ns let the setup HTTP server know that someone is requesting an image

            byte[] audioArray = console.getAudio(); // ke9ns gets audio stream
                                                   

            if (audioArray == null) // if we dont have an image, let the requestor know we dont have an image to send.
            {
                string CodeStr = "500 " + ((System.Net.HttpStatusCode)500).ToString();

                string Html = "<html><body><h1>" + CodeStr + "</h1></body></html>";

                string Str = "HTTP/1.1 " + CodeStr + "\nContent-type: text/html\nContent-Length:" + Html.Length.ToString() + "\n\n" + Html;

                byte[] Buffer = Encoding.ASCII.GetBytes(Str);

                m_tcpClient.GetStream().Write(Buffer, 0, Buffer.Length);
                m_tcpClient.Close();
                return;
            }

            //  "<meta http-equiv= \"refresh\" content= \"500\" > \r\n" +

            string responseHeaders = "HTTP/1.1 200 The file is coming right up!\r\n" +
                                       "Server: MyOwnServer\r\n" +
                                       "Content-Length: " + audioArray.Length + "\r\n" +
                                       "Content-Type: audio/mpeg\r\n" +
                                       //  "Content-Disposition: inline;filename=\"picDisplay.jpg;\"\r\n" +
                                       "\r\n";



            byte[] headerArray = Encoding.ASCII.GetBytes(responseHeaders); // convert responseheader into byte array


            NetworkStream stream1 = m_tcpClient.GetStream(); // create a stream to send/receive data over the TCP/IP connection

            stream1.Write(headerArray, 0, headerArray.Length); // send header
            stream1.Write(audioArray, 0, audioArray.Length);   // send audio

            stream1.Close();

            m_tcpClient.Close();


        } // AudioRequest()

        //===============================================================================


        public void PlayAudio() //  public void PlayAudio(int id)
        {
            byte[] bytes = new byte[0];

            // using (The_FactoryDBContext db = new The_FactoryDBContext())
            //  {
            //    if (db.Words.FirstOrDefault(word => word.wordID == id).engAudio != null)
            //  {
            //        bytes = db.Words.FirstOrDefault(word => word.wordID == id).engAudio;

            //  }
            // }

            //  Context.Response.Clear();
            //  Context.Response.ClearHeaders();
            //  Context.Response.ContentType = "audio/wav"; //  "audio/mpeg";
            // Context.Response.AddHeader("Content-Length", bytes.Length.ToString());
            // Context.Response.OutputStream.Write(bytes, 0, bytes.Length);
            // Context.Response.End();
        }

        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================

        public void UnknownRequest(TcpClient m_tcpClient)
        {
            Debug.WriteLine("Unknown_REQUEST");


            if (m_tcpClient == null) return;

            string CodeStr = "404 " + ((HttpStatusCode)404).ToString();
            string Html = "<html><body><h1>" + CodeStr + "</h1></body></html>";
            string Str = "HTTP/1.1 " + CodeStr + "\nContent-type: text/html\nContent-Length:" + Html.Length.ToString() + "\n\n" + Html;
            byte[] Buffer = Encoding.ASCII.GetBytes(Str);

            m_tcpClient.GetStream().Write(Buffer, 0, Buffer.Length);

            m_tcpClient.Close();

        } // UnknownRequest()

        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================

        public void WebPageRequest(TcpClient m_tcpClient)
        {
            Debug.WriteLine("Web_REQUEST");


            if (m_tcpClient == null) return;

            Debug.WriteLine("Web_REQUEST2");


            string timeRefresh_in_ms = getTimeRefresh();

            string CodeStr = "200 " + ((HttpStatusCode)200).ToString();

            string Html = "<!DOCTYPE html>\n" +
                          "<html>\n" +
                          "<head>\n" +
                          "<title></title>\n" +
                          "</head>\n" +
                          "<body>\n" +
                          "<div><img id = 'img' src = \"\"></div>\n" +
                          "<script type = \"text/javascript\" src = \"https://code.jquery.com/jquery-3.1.1.min.js\"></script>\n" +
                          "<script type = \"text/javascript\">\n" +
                          "var link = \"http://\"+window.location.host;\n" +
                          "console.log(link);\n" +
                          "setInterval(function(){\n" +
                          "var now = new Date();\n" +
                          "$('#img').prop(\"src\",link+\"/image\" + '?_=' + now.getTime());\n" +
                          "}, " + timeRefresh_in_ms + ");\n" +
                          "</script>\n" +
                          "</body>\n" +
                          "</html>\n";


            string Str = "HTTP/1.1 " + CodeStr + "\nContent-Type: text/html\nContent-Length:" + Html.Length.ToString() + "\n\n" + Html;

            Debug.WriteLine("STRING TO SEND: " + Str);


            byte[] Buffer = Encoding.ASCII.GetBytes(Str);
            m_tcpClient.GetStream().Write(Buffer, 0, Buffer.Length);
            m_tcpClient.Close();

        } // webrequest

        private string getTimeRefresh()
        {
            //  return "200"; // ************** Darrin,need add property "Refreh time in ms" and get data from his
            return console.HTTP_REFRESH.ToString();

        }



        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        Bitmap bitmap;
        byte[] picDisplayOutput;
        MemoryStream memstream;


        private byte[] getImage()
        {

            bitmap = new Bitmap(console.picDisplay.Width, console.picDisplay.Height); // ke9ns set bitmap size to size of picDisplay since it gets resized with your screen
            console.picDisplay.DrawToBitmap(bitmap, console.picDisplay.ClientRectangle); // ke9ns grab picDisplay and convert to bitmap

            using (memstream = new MemoryStream())
            {
                bitmap.Save(memstream, ImageFormat.Jpeg);
                picDisplayOutput = memstream.ToArray();
            }


            return picDisplayOutput;

        } // getImage()





        /*     // ke9ns if you want to save image as a file and then read file
                private byte[] getImage()
                {

                    bitmap = new Bitmap(console.picDisplay.Width, console.picDisplay.Height); // ke9ns set bitmap size to size of picDisplay since it gets resized with your screen
                    console.picDisplay.DrawToBitmap(bitmap, console.picDisplay.ClientRectangle); // ke9ns grab picDisplay and convert to bitmap
                    bitmap.Save(console.AppDataPath + "picDisplay.jpg", ImageFormat.Jpeg); // ke9ns save image into database folder

                    FileInfo picDisplayFile = new FileInfo(console.AppDataPath + "picDisplay.jpg");
                    FileStream picDisplayStream = new FileStream(console.AppDataPath + "picDisplay.jpg", FileMode.Open, FileAccess.Read); // open file  stream 
                    BinaryReader picDisplayReader = new BinaryReader(picDisplayStream); // open stream for binary reading

                    picDisplayOutput = picDisplayReader.ReadBytes((int)picDisplayFile.Length); // create array of bytes to transmit

                    picDisplayReader.Close();
                    picDisplayStream.Close();

                    return picDisplayOutput;


                } // getImage()

            */
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        //=========================================================================================
        private static void SendError(TcpClient Client, int Code)
        {
            string CodeStr = Code.ToString() + " " + ((HttpStatusCode)Code).ToString();
            string Html = "<html><body><h1>" + CodeStr + "</h1></body></html>";
            string Str = "HTTP/1.1 " + CodeStr + "\nContent-type: text/html\nContent-Length:" + Html.Length.ToString() + "\n\n" + Html;
            byte[] Buffer = Encoding.ASCII.GetBytes(Str);
            Client.GetStream().Write(Buffer, 0, Buffer.Length);
            Client.Close();
        }


      

    } // class http

    sealed public class Server
    {


        private Socket sock;

        private int port = 8080;

        private IPAddress addr = IPAddress.Any;

        private int backlog;


        //------------------------------------------------
        public void Start() //  // This is the method that starts the server listening.
        {
            this.sock = new Socket(addr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            sock.Bind(new IPEndPoint(this.addr, this.port));

            this.sock.Listen(this.backlog); // places socket in listen state

            this.sock.BeginAccept(this.OnConnectRequest, sock); // accepts incoming connection attemp


        } // Start()


        //------------------------------------------------
        public void OnConnectRequest(IAsyncResult result)  // This is the method that is called when the socket receives a request for a new connection.
        {

            Socket sock = (Socket)result.AsyncState;  // get socket 

            Connection newConn = new Connection(sock.EndAccept(result)); // create new connection

            sock.BeginAccept(this.OnConnectRequest, sock); // tell listener socket to start listening again


        } //  public void OnConnectRequest(IAsyncResult result)





    } //  public class Server
    



    public class Connection
    {

        private Socket sock;
        byte[] dataRcvBuf;

        private Encoding encoding = Encoding.UTF8;

        //--------------------------------------
        public Connection(Socket s)
        {
            this.sock = s;
            this.BeginReceive(); // start listening for incoming data (this could be in a thread

        } //AsyncResult result)


        //--------------------------------------
        private void BeginReceive()  //  // Call this method to set this connection's socket up to receive data.
        {
            this.sock.BeginReceive(this.dataRcvBuf, 0, this.dataRcvBuf.Length, SocketFlags.None, new AsyncCallback(this.OnBytesReceived), this);


        } // private void BeginReceive()


        //--------------------------------------
         protected void OnBytesReceived(IAsyncResult result)   // This is the method that is called whenever the socket receives incoming bytes.
        {

            int nBytesRec = this.sock.EndReceive(result);   // End the data receiving that the socket has done and get the number of bytes read.

            if (nBytesRec <= 0)                            // If no bytes were received, the connection is closed (at least as far as we're concerned).
            {
                this.sock.Close();
                return;
            }

            string strReceived = this.encoding.GetString(this.dataRcvBuf, 0, nBytesRec);   // Convert the data we have to a string.

            Debug.WriteLine("!!!!!!GOT BYTES" + strReceived);


            // ...Now, do whatever works best with the string data.
            // You could, for example, look at each character in the string
            // one-at-a-time and check for characters like the "end of text"
            // character ('\u0003') from a client indicating that they've finished
            // sending the current message.  It's totally up to you how you want
            // the protocol to work.

            // Whenever you decide the connection should be closed, call 
            // sock.Close() and don't call sock.BeginReceive() again.  But as long 
            // as you want to keep processing incoming data...


            this.sock.BeginReceive(this.dataRcvBuf, 0, this.dataRcvBuf.Length, SocketFlags.None, new AsyncCallback(this.OnBytesReceived), this);  // Set up again to get the next chunk of data.

        } //  protected void OnBytesReceived(IAsyncResult result)


    } //  public class Connection

} // namespace powersdr