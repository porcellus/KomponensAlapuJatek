using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{

    public class StateObject
    {
        public Socket workSocket = null;

        public const int BufferSize = 100;

        public byte[] buffer = new byte[BufferSize];
    }

    public class Server
    {
        public event EventHandler<ServerInfoEventArgs> needlog;

        private IPAddress ipAddress;
        private TcpListener tcpListener;

        private Thread serverThread;

        private Int32 idCounter;

        private Lobby lobby;

        private static ManualResetEvent recieveDone = new ManualResetEvent(false);

        public Server()
        {
            ipAddress = null;
            tcpListener = null;
            serverThread = null;
            idCounter = 0;
            lobby = new Lobby();
        }

        public void stratServer(String _ip, Int32 _port)
        {
            try
            {
                serverThread = new Thread(() =>
                {
                    ipAddress = IPAddress.Parse(_ip);
                    tcpListener = new TcpListener(ipAddress, _port);
                    tcpListener.Start();

                    ServerInfoEventArgs args = new ServerInfoEventArgs();
                    args.consoleinfo = "The server is running at port " + _port + " ...\n";
                    OnNeedLog(args);
                    args.consoleinfo = "The local End point is: " + tcpListener.LocalEndpoint + "\n";
                    OnNeedLog(args);
                    args.consoleinfo = "Waiting for a connection.....\n";
                    OnNeedLog(args);

                    while (true)
                    {
                        if (tcpListener.Pending())
                        {
                            Console.WriteLine("Adding ID: " + Convert.ToString(idCounter));
                            Socket socket = tcpListener.AcceptSocket();
                            lobby.add(new ServerClient(
                                idCounter,
                                ((IPEndPoint)socket.RemoteEndPoint).Port.ToString(),
                                ((IPEndPoint)socket.RemoteEndPoint).Address.ToString(),
                                recieveData(socket, 100),
                                recieveData(socket, 100),
                                socket,
                                false
                            ));

                            idCounter++;
                            string sendString = ((IPEndPoint)socket.RemoteEndPoint).Address.ToString();
                            sendString += ";" + ((IPEndPoint)socket.RemoteEndPoint).Port.ToString();

                            socket.Send(new ASCIIEncoding().GetBytes(sendString));
                        }

                        List<ServerClient>clList = lobby.getClientlist();


                        foreach (ServerClient client in clList)
                        {
                            if (!client.startedrecive)
                            {
                                client.startedrecive = true;

                                Thread recieve = new Thread(() => 
                                {
                                    while (true)
                                    {
                                        recieveDone.Reset();
                                        StateObject state = new StateObject();
                                        state.workSocket = client.clientsocket;

                                        client.clientsocket.BeginReceive(state.buffer, 0, StateObject.BufferSize,
                                                                         0, new AsyncCallback(receiveCompleted), state);
                                        recieveDone.WaitOne();
                                    }

                                });

                                recieve.Start();
                            }

                        }

                    }

                });

                serverThread.Start();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.Message);
            }

        }

        private void receiveCompleted(IAsyncResult ar)
        {

            recieveDone.Set();

            StateObject state = (StateObject)ar.AsyncState;

            Socket handler = state.workSocket;

            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                String recieveddata = System.Text.Encoding.UTF8.GetString(state.buffer).Replace("\0", "");

                String[] datas;

                if (recieveddata.Contains("getlobby"))
                {
                    datas = recieveddata.Split(';');
                    String gametype = datas[1];
                    handler.Send(new ASCIIEncoding().GetBytes(lobby.getClientsInfo(gametype)));
                }
            }
        }

        public void stopServer()
        {
            if (tcpListener != null && serverThread != null && lobby != null)
            {
                serverThread.Abort();

                foreach (ServerClient client in lobby.getClientlist()) lobby.remove(client);
     
                idCounter = 0;
                tcpListener.Stop();
            }

            ServerInfoEventArgs args = new ServerInfoEventArgs();
            args.consoleinfo = "Server Closed Successfully!\n";
            OnNeedLog(args);
        }

        private String recieveData(Socket socket, Int32 inputsize)
        {

            byte[] buffer = new byte[inputsize];
            int bytes = socket.Receive(buffer);
            String output = System.Text.Encoding.UTF8.GetString(buffer).Replace("\0", "");

            return output;
        }

        protected virtual void OnNeedLog(ServerInfoEventArgs e)
        {
            EventHandler<ServerInfoEventArgs> handler = needlog;
            if (handler != null)
            {
                handler(this, e);
            }
        }

    }

    public class ServerInfoEventArgs : EventArgs
    {
        public String consoleinfo { get; set; }
    }
}
