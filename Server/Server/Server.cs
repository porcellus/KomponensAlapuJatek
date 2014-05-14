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
                            Socket socket = tcpListener.AcceptSocket();
                            lobby.add(new ServerClient(
                                idCounter,
                                ((IPEndPoint)socket.RemoteEndPoint).Port.ToString(),
                                ((IPEndPoint)socket.RemoteEndPoint).Address.ToString(),
                                recieveData(socket,100),
                                recieveData(socket,100),
                                socket
                            ));

                            socket.Send(new ASCIIEncoding().GetBytes(idCounter.ToString()));
                            idCounter++;
                        }

                        foreach (ServerClient client in lobby.getClientlist())
                        {
                            if(!client.clientsocket.ReceiveAsync(new SocketAsyncEventArgs()))
                            {
                                String recieveddata = recieveData(client.clientsocket,100);
                                String[] datas;

                                if(recieveddata.Contains("getlobby"))//getlobby;chess
                                {
                                    datas = recieveddata.Split(';');
                                    String gametype = datas[1];
                                    client.clientsocket.Send(new ASCIIEncoding().GetBytes(lobby.getClientsInfo(gametype)));
                                }

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

            String output = "";
            for (int i = 0; i < bytes; i++)
            {
                output += Convert.ToString(buffer[i]);
            }

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
