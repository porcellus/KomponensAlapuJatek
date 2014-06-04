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
    public class Server
    {
        IPAddress ipAddress;
        TcpListener tcpListener;

        Thread serverThread;

        Int32 idCounter;

        Lobby lobby;

        List<ServerClient> clList;

        public event EventHandler<ServerInfoEventArgs> needlog;

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
                                socket
                            ));

                            idCounter++;
                            string sendString = idCounter.ToString();
                            sendString += ";" + ((IPEndPoint)socket.RemoteEndPoint).Port.ToString();

                            socket.Send(new ASCIIEncoding().GetBytes(sendString));
                        }

                        clList = lobby.getClientlist();


                        foreach (ServerClient client in clList)
                        {
                            SocketAsyncEventArgs sArgs = new SocketAsyncEventArgs();
                            sArgs.UserToken = client;
                            sArgs.Completed += receiveCompleted;
                            sArgs.SetBuffer(new byte[100], 0, 100);
                            if (!client.clientsocket.ReceiveAsync(sArgs))
                            {
                                receiveCompleted(this, sArgs);
                            }
                        }

                    }

                });

                serverThread.Start();
                Console.WriteLine("Server started ");

            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.Message);
            }

        }

        private void receiveCompleted(object sender, SocketAsyncEventArgs e)
        {
            ServerClient client = (e.UserToken as ServerClient);

            String recieveddata = System.Text.Encoding.UTF8.GetString(e.Buffer).Replace("\0", "");

            String[] datas;

            if (recieveddata.Contains("getlobby"))//getlobby;chess
            {
                datas = recieveddata.Split(';');
                String gametype = datas[1];
                client.clientsocket.Send(new ASCIIEncoding().GetBytes(lobby.getClientsInfo(gametype)));
            }
        }

        public void stopServer()
        {
            if (tcpListener != null && serverThread != null && lobby != null)
            {
                foreach (ServerClient client in lobby.getClientlist()) lobby.remove(client);
                idCounter = 0;
                tcpListener.Stop();
                serverThread.Abort();
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
            Console.WriteLine("Server receive: " + output);

            return output;

        }

        protected virtual void OnNeedLog(ServerInfoEventArgs e)
        {
            Console.WriteLine("Server: " + e.consoleinfo);
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
