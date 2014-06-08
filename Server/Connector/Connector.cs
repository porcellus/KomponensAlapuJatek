using Connector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{

    public class StateObject
    {
        public Socket workSocket = null;

        public const int BufferSize = 1024;

        public byte[] buffer = new byte[BufferSize];

        public StringBuilder sb = new StringBuilder();
    }

    public class Connector
    {

        private TcpClient tcpserver;
        private Stream serverstream;

        private static Socket listener;
        private static Socket opponentsocket;
        private static Socket clientsocket;

        private Thread acceptconnectionthread;
        private Thread listenconnectionthread;
        private Thread requestthread;

        private static ManualResetEvent connectionDone = new ManualResetEvent(false);
        private static ManualResetEvent recieveDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent connectDone = new ManualResetEvent(false);

        private bool firstplayer;

        public String playerip;
        public Int32 clientport;

        public event ConnectionAcceptEventHandler connectionAcceptEventHandler;
        public event ConnectionRequestEventHandler connectionRequestEventHandler;
        public event StepEventHandler stepEventHandler;

        public Connector()
        {
            tcpserver = new TcpClient();
            listener = new Socket(AddressFamily.InterNetwork,
                                  SocketType.Stream, ProtocolType.Tcp);

            opponentsocket = new Socket(AddressFamily.InterNetwork,
                                        SocketType.Stream, ProtocolType.Tcp);

            clientsocket = new Socket(AddressFamily.InterNetwork,
                                SocketType.Stream, ProtocolType.Tcp);

            firstplayer = false;
        }

        public void connectoServer(String ip, Int32 port, String name, String gametype)
        {

            tcpserver.Connect(ip, port);

            serverstream = tcpserver.GetStream();

            writeStream(serverstream, name);
            writeStream(serverstream, gametype);

            string read = readStream(serverstream, 100);

            string[] data = read.Split(';');

            playerip = data[0];
            clientport = int.Parse(data[1]);

            acceptconnectionthread = new Thread(() =>
            {
                IPAddress ipAddress = IPAddress.Parse(playerip);
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, clientport);
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    connectionDone.Reset();

                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    connectionDone.WaitOne();

                }
            });

            listenconnectionthread = new Thread(() =>
            {
                while(true)
                {
                    if (opponentsocket.Connected)
                    {
                        recieveDone.Reset();

                        StateObject state = new StateObject();
                        state.workSocket = opponentsocket;
                        opponentsocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                                        new AsyncCallback(ReadCallback), state);

                        recieveDone.WaitOne();
                    }
                }

            });


            acceptconnectionthread.Start();
            listenconnectionthread.Start();


        }

        public String[] getLobby(String gametype)
        {
            writeStream(serverstream, "getlobby;" + gametype);

            return readStream(serverstream, 100).Split(';');
        }

        public void requestGame(String opponentip, Int32 opponentport, String myname)
        {
            IPHostEntry ipHostInfo = Dns.Resolve(opponentip);
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, opponentport);

            clientsocket.BeginConnect(remoteEP,
                    new AsyncCallback(ConnectCallback), clientsocket);
            connectDone.WaitOne();

            requestthread = new Thread(() => 
            { 
            
              while(true)
              {
                  if (clientsocket.Connected)
                  {
                      recieveDone.Reset();

                      StateObject state = new StateObject();
                      state.workSocket = clientsocket;
                      clientsocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                                      new AsyncCallback(ReadCallback), state);

                      recieveDone.WaitOne();
                  }
              }

            });

            firstplayer = true;

            requestthread.Start();

            sendDone.Reset();
            Send(clientsocket,"startgame;" + myname);
            sendDone.WaitOne();
        }

        public void sendRequestreply(String reply)
        {
            if(!firstplayer && opponentsocket.Connected)
            {
                sendDone.Reset();
                Send(opponentsocket, "reply;" + reply);
                sendDone.WaitOne();
            }
        }

        public void step(Object step)
        {
            byte[] sendstring = new ASCIIEncoding().GetBytes("step;");
            byte[] sendobject = serializeObject(step);
            byte[] sendbuffer = Combine(sendstring, sendobject);

            if (firstplayer)
            {
                sendDone.Reset();
                Send(clientsocket, sendbuffer);
                sendDone.WaitOne();
            }
            else
            {
                sendDone.Reset();
                Send(opponentsocket, sendbuffer);
                sendDone.WaitOne();
            }

        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                client.EndConnect(ar);

                connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
       
        private void AcceptCallback(IAsyncResult ar)
        {
            connectionDone.Set();

            Socket listener = (Socket)ar.AsyncState;
            opponentsocket = listener.EndAccept(ar);

        }

        private void ReadCallback(IAsyncResult ar)
        {
            recieveDone.Set();

            String content = String.Empty;

            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                state.sb.Append(Encoding.ASCII.GetString(state.buffer,
                                0, bytesRead));
                content = state.sb.ToString();

                String[] data = content.Split(';');

                if(data[0].Equals("reply"))
                {
                    ConnectionAcceptEventArgs e = new ConnectionAcceptEventArgs(data[1]);
                    OnConnectionAccept(e);
                }
                else if(data[0].Equals("startgame"))
                {
                    ConnectionRequestEventArgs e = new ConnectionRequestEventArgs(data[1]);
                    OnConnectionRequest(e);
                }
                else if(data[0].Equals("step"))
                {
                    StepEventArgs e = new StepEventArgs(deserializeToObject(state.buffer,5));
                    OnStep(e);
                }

            }

        }

        private void Send(Socket client, String data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        private void Send(Socket client, byte[] data)
        {
            client.BeginSend(data, 0, data.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                int bytesSent = client.EndSend(ar);

                sendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void writeStream(Stream stream, String message)
        {
            byte[] sendbuffer = new ASCIIEncoding().GetBytes(message);
            if (sendbuffer.Length < 100)
            {
                Array.Resize<byte>(ref sendbuffer, 100);
            }
            stream.Write(sendbuffer, 0, 100);
        }

        private String readStream(Stream stream, int readsize)
        {
            byte[] recivebuffer = new byte[readsize];
            int numberofbytes = stream.Read(recivebuffer, 0, readsize);

            return System.Text.Encoding.UTF8.GetString(recivebuffer).Replace("\0", "");
        }

        private byte[] serializeObject(Object obj)
        {
            using (Stream stm = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stm, obj);

                long size = stm.Length;
                byte[] binary = new byte[size];

                stm.Seek(0, SeekOrigin.Begin);
                stm.Read(binary, 0, (int)size);

                return binary;
            }
        }

        private object deserializeToObject(byte[] bytes, int offset)
        {
                BinaryFormatter formatter = new BinaryFormatter();
            
                int size = bytes.Length;

                Stream stm = new MemoryStream(size);

                stm.Seek(0, SeekOrigin.Begin);
                stm.Write(bytes, offset, size-offset);
                stm.Seek(0, SeekOrigin.Begin);

                return formatter.Deserialize(stm);

        }

        private byte[] Combine(byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }

        protected virtual void OnConnectionAccept(ConnectionAcceptEventArgs e)
        {
            ConnectionAcceptEventHandler handler = connectionAcceptEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnConnectionRequest(ConnectionRequestEventArgs e)
        {
            ConnectionRequestEventHandler handler = connectionRequestEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnStep(StepEventArgs e)
        {
            StepEventHandler handler = stepEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
