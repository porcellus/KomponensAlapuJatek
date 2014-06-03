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
    public class Connector
    {

        private TcpClient tcpserver;
        private Stream serverstream;

        private TcpClient tcpopponent;
        private Stream opponentstream;

        private Socket opponentsocket;

        public Int32 playerid;

        private IPAddress ipAddress;
        private TcpListener tcpListener;
        private Int32 port;

        private Thread listenthread;

        private String requestreply;
        private String opponentname;

        private bool firstplayer;

        public Object opponentstep;


        public event ConnectionAcceptEventHandler connectionAcceptEventHandler;
        public event ConnectionRequestEventHandler connectionRequestEventHandler;
        public event StepEventHandler stepEventHandler;

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


        public Connector()
        {
            tcpserver = new TcpClient();
            tcpopponent = new TcpClient();
            opponentsocket = null;
            port = 80;
            requestreply = "Accept";
            opponentname = "";
            firstplayer = false;
        }

        public void connectoServer(String ip, Int32 port, String name, String gametype)
        {
            Console.WriteLine("Connecting...");

            tcpserver.Connect(ip, port);
            Console.WriteLine("Connected...");
            serverstream = tcpserver.GetStream();

            writeStream(serverstream, name);
            writeStream(serverstream, gametype);

            string read = readStream(serverstream, 100);
            string[] data = read.Split(';');

            playerid = int.Parse(data[0]);
            port = int.Parse(data[1]);

            listenthread = new Thread(() =>
            {

                int iplocation = playerid + 1;
                ipAddress = IPAddress.Parse("127.0.0.1");
                tcpListener = new TcpListener(ipAddress, port);
                tcpListener.Start();
                listenGame();
            });
            listenthread.Start();

        }

        public String[] getLobby(String gametype)
        {
            writeStream(serverstream, "getlobby;" + gametype);

            return readStream(serverstream, 100).Split(';');
        }

        public bool requestGame(String opponentip, Int32 opponentport, String name)
        {
            tcpopponent.Connect(opponentip, opponentport);
            opponentstream = tcpopponent.GetStream();

            writeStream(opponentstream, name);

            string received = readStream(opponentstream, 100);

            if (received.Equals("Decline"))
            {
                ConnectionAcceptEventArgs e = new ConnectionAcceptEventArgs("Decline");
                OnConnectionAccept(e);
                return false;
            }
            else
            {
                ConnectionAcceptEventArgs e = new ConnectionAcceptEventArgs(received);
                OnConnectionAccept(e);
                firstplayer = true;
                return true;
            }
        }

        public void setRequestreply(String reply)
        {
            this.requestreply = reply;
        }

        public void step(Object step)
        {
            byte[] sendstring = new ASCIIEncoding().GetBytes("step;");
            byte[] sendobject = serializeObject(step);
            byte[] sendbuffer = (byte[])sendstring.Concat(sendobject);

            if (firstplayer)
            {

                opponentstream.Write(sendbuffer, 0, sendbuffer.Length);
            }
            else
            {
                opponentsocket.Send(sendbuffer);
            }

        }

        public void finishGame()
        {
            if (firstplayer)
            {
                writeStream(opponentstream, "finish");
                opponentstream.Close();
                tcpopponent.Close();
            }
            else
            {
                opponentsocket.Send(new ASCIIEncoding().GetBytes("finish"));
                opponentsocket.Close();
                opponentname = "";
                requestreply = "";
            }

        }

        private void listenGame()
        {
            while (true)
            {
                if (tcpListener.Pending())
                {
                    opponentsocket = tcpListener.AcceptSocket();

                    this.opponentname = readSocket(opponentsocket, 100);

                    Console.WriteLine(Convert.ToString(this.playerid) + " connector received opponentname: " + this.opponentname);

                    ConnectionRequestEventArgs e = new ConnectionRequestEventArgs(this.opponentname);
                    OnConnectionRequest(e);

                    opponentsocket.Send(new ASCIIEncoding().GetBytes(requestreply));
                }

                SocketAsyncEventArgs sArgs = new SocketAsyncEventArgs();
                sArgs.UserToken = opponentsocket;
                sArgs.Completed += receiveCompleted;
                sArgs.SetBuffer(new byte[100], 0, 100);

                if (opponentsocket != null && !opponentsocket.ReceiveAsync(sArgs))
                {
                    receiveCompleted(this, sArgs);
                }
                sArgs.Dispose();

                if (firstplayer)
                {
                    byte[] readbuffer = new byte[100];
                    opponentstream.Read(readbuffer, 0, readbuffer.Length);

                    String output = "";
                    for (int i = 0; i < readbuffer.Length; i++) output += Convert.ToString(readbuffer[i]);

                    if (output.Contains("step"))
                    {
                        String[] datas = output.Split(';');
                        opponentstep = deserializeToObject(datas[1]);
                        StepEventArgs e = new StepEventArgs(opponentstep);
                        OnStep(e);

                    }
                    else if (output.Contains("finish"))
                    {
                        opponentstream.Close();
                        tcpopponent.Close();

                    }


                }

            }
        }

        private void receiveCompleted(object sender, SocketAsyncEventArgs e)
        {
            String output = readSocket(opponentsocket, 100);

            if (output.Contains("step")) //step;object
            {
                String[] datas = output.Split(';');
                opponentstep = datas[1];
                Console.WriteLine("Rec opponentStep: " + datas[1]);

            }
            else if (output.Contains("finish"))
            {
                opponentsocket.Close();
                opponentname = "";
                requestreply = "";
                Console.WriteLine("Rec finish");
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

            Console.WriteLine("Connector sent: " + message);
        }

        private String readStream(Stream stream, int readsize)
        {
            byte[] recivebuffer = new byte[readsize];
            int numberofbytes = stream.Read(recivebuffer, 0, readsize);

            Console.WriteLine("Connector readStr");
            return System.Text.Encoding.UTF8.GetString(recivebuffer).Replace("\0", "");
        }

        private String readSocket(Socket socket, int readsize)
        {
            byte[] recivebuffer = new byte[readsize];
            int bytes = socket.Receive(recivebuffer);

            Console.WriteLine("Connector readSock");
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
                stm.Read(binary, 0, (int)size);
                return binary;
            }
        }

        private object deserializeToObject(String str)
        {
            using (Stream stm = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();

                byte[] bytes = new byte[str.Length * sizeof(char)];
                System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);

                long size = bytes.Length;
                stm.Write(bytes, 0, (int)size);

                return formatter.Deserialize(stm);
            }
        }

    }
}
