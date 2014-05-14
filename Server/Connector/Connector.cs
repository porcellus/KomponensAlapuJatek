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

        private Int32 playerid;
        
        private IPAddress ipAddress;
        private TcpListener tcpListener;
        private Int32 port;

        private Thread listenthread;

        private String requestreply;
        private String opponentname;

        private bool firstplayer;

        public Object opponentstep;


        public Connector() 
        {
            tcpserver = new TcpClient();
            tcpopponent = new TcpClient();
            opponentsocket = null;
            port = 80;
            requestreply = "";
            opponentname = "";
            firstplayer = false;
        }

        public void connectoServer(String ip, Int32 port, String name, String gametype)
        {
            tcpserver.Connect(ip,port);
            serverstream = tcpserver.GetStream();

            writeStream(serverstream,name);
            writeStream(serverstream,gametype);

            playerid = int.Parse(readStream(serverstream, 100));

            listenthread = new Thread(() => {

                    int iplocation = playerid + 1;
                    ipAddress = IPAddress.Parse("127.0.0." + iplocation.ToString());
                    tcpListener = new TcpListener(ipAddress, this.port);
                    tcpListener.Start();
                    listenGame();
            });
        }

        public String[] getLobby(String gametype)
        {
            writeStream(serverstream, "getlobby;" + gametype);

            return readStream(serverstream, 1024).Split(';');
        }

        public bool requestGame(String opponentip, Int32 opponentport, String name)
        {
            tcpopponent.Connect(opponentip, opponentport);
            opponentstream = tcpopponent.GetStream();

            writeStream(opponentstream, name);
            if (readStream(opponentstream, 100).Equals("Decline"))
            {
                return false;
            }
            else
            {
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

            if(firstplayer)
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

                    opponentsocket.Send(new ASCIIEncoding().GetBytes(requestreply));
                }

                if (!opponentsocket.ReceiveAsync(new SocketAsyncEventArgs()))
                {
                    String output = readSocket(opponentsocket, 1024);

                    if (output.Contains("step")) //step;object
                    {
                        String[] datas = output.Split(';');
                        opponentstep = datas[1];

                    }
                    else if (output.Contains("finish"))
                    {
                        opponentsocket.Close();
                        opponentname = "";
                        requestreply = "";
                    }
                }

                if(firstplayer)
                {
                    byte[] readbuffer = new byte[1024];
                    opponentstream.ReadAsync(readbuffer,0,readbuffer.Length);

                     String output = "";
                     for (int i = 0; i < readbuffer.Length; i++) output += Convert.ToString(readbuffer[i]);

                    if (output.Contains("step"))
                    {
                        String[] datas = output.Split(';');
                        opponentstep = datas[1];

                    }
                    else if (output.Contains("finish"))
                    {
                        opponentstream.Close();
                        tcpopponent.Close();

                    }


                }    

            }
        }

        private void writeStream(Stream stream, String message)
        {
            byte[] sendbuffer = new ASCIIEncoding().GetBytes(message);
            stream.Write(sendbuffer, 0, sendbuffer.Length);
        }

        private String readStream(Stream stream, int readsize)
        {
            byte[] recivebuffer = new byte[readsize];
            int numberofbytes = stream.Read(recivebuffer, 0, readsize);

            String output = "";
            for (int i = 0; i < numberofbytes; i++) output += Convert.ToString(recivebuffer[i]);

            return output;
        }

        private String readSocket(Socket socket, int readsize)
        {
            byte[] recivebuffer = new byte[readsize];
            int bytes = socket.Receive(recivebuffer);
            String output = "";
            for (int i = 0; i < bytes; i++) output += Convert.ToString(recivebuffer[i]);

            return output;
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

    }
}
