/*   Server Program    */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;



namespace Server
{
    public class Server
    {
        IPAddress ipAddress;
        TcpListener tcpListener;
        Thread serverthread;
        List<Socket> clientsockets;
        List<Game> gamelist;
        Lobby lobby;

        public event EventHandler<ServerInfoEventArgs> needlog;

        public Server()
        {
            ipAddress = null;
            tcpListener = null;
            serverthread = null;
            clientsockets = null;
            gamelist = null;
            lobby = null;
        }

        public void stratserver(String _ip, Int32 _port)
        {
            try
            {
                serverthread = new Thread(() =>
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

                    clientsockets = new List<Socket>();
                    gamelist = new List<Game>();
                    lobby = new Lobby();

                    while (true)
                    {
                        if (tcpListener.Pending())
                        {
                            clientsockets.Add(tcpListener.AcceptSocket());
                            acceptConnection(100);
                        }
                    }

                });

                serverthread.Start();
                   
            }
            catch (Exception e) 
            { 
                Console.WriteLine("Error..... " + e.Message); 
            }

        }

        public void stopserver()
        {
            if (tcpListener != null && serverthread != null)
            {
                foreach(Socket socket in clientsockets)
                {
                   socket.Close();
                }
                tcpListener.Stop();

                serverthread.Abort();
            }

            ServerInfoEventArgs args = new ServerInfoEventArgs();
            args.consoleinfo = "Server Closed Successfully!\n";
            OnNeedLog(args);
        }

        public bool sendGameRequest(Int32 player1id, Int32 player2id)
        {
            ServerClient senderclinet = new ServerClient(lobby.getbyId(player1id));

            Socket socketsendto = clientsockets[player2id - 1];
            socketsendto.Send(new ASCIIEncoding().GetBytes(senderclinet.name + " szeretne " + senderclinet.gametype + " jatekot játszani!"));
            return recieveReaction(socketsendto,100);
        }

        public bool initializeGame(Int32 starterplayerid, Int32 secondplayerid, Object gameField)
        {
            Game currgame = new Game(new ServerClient(lobby.getbyId(starterplayerid)), new ServerClient(lobby.getbyId(secondplayerid)),gameField);
            gamelist.Add(currgame);

            byte[] data = serializeObject(gameField);
            Socket socketsendto = clientsockets[secondplayerid - 1];
            socketsendto.Send(data);
            byte[] id = serializeObject(starterplayerid);
            socketsendto.Send(id);
            currgame.gamestarted = true;

            return recieveReaction(socketsendto, 100);
           
        }

        public void step(Int32 currplayerid, Int32 otherplayerid, Object gameField/*steppet kapok*/)
        {
            Game currgame = gamelist.Find(x => x.currplayer.id == currplayerid);

            ServerClient tmp = currgame.currplayer;
            currgame.currplayer = currgame.secondplayer;
            currgame.secondplayer = tmp;
            currgame.gamefiled = gameField;

            gamelist.Insert(gamelist.IndexOf(currgame), currgame);

            byte[] data = serializeObject(gameField);
            Socket socketsendto = clientsockets[otherplayerid - 1];
            socketsendto.Send(data);
        }

        public void finish(Int32 currplayerid, Int32 otherplayerid)
        {
            Game currgame = gamelist.Find(x => x.currplayer.id == currplayerid);
            currgame.gamefinished = true;

            gamelist.Insert(gamelist.IndexOf(currgame), currgame);

        }

        private void acceptConnection(Int32 _inputsize)
        {
            int clientid = clientsockets.Count;
            Socket clinetsocket = clientsockets[clientid-1];

                ServerInfoEventArgs args = new ServerInfoEventArgs();
                args.consoleinfo = "Connection accepted from " + clinetsocket.RemoteEndPoint + "\n";
                OnNeedLog(args);
                 
                ServerClient client = new ServerClient();
                client.id = clientid;
                client.port = ((IPEndPoint)clinetsocket.RemoteEndPoint).Port.ToString();
                client.ip = ((IPEndPoint)clinetsocket.RemoteEndPoint).Address.ToString();
                client.name = recieveData(clinetsocket,_inputsize);   // itt várakozik az inputra
                client.gametype = "";

                lobby.add(client);

                clinetsocket.Send(new ASCIIEncoding().GetBytes("Users in the lobby.\n" + lobby.getClientsName()));

        }

        private String recieveData(Socket socket,Int32 inputsize)
        {

            byte[] buffer = new byte[inputsize];
            int bytes = socket.Receive(buffer);

            String output="";
            for (int i = 0; i < bytes; i++)
            {
                output += Convert.ToString(buffer[i]);
            }

            return output;

        }

        private bool recieveReaction(Socket socket,Int32 inputsize)
        {
            String reaction = recieveData(socket, inputsize);

            if (reaction == "Decline") return false;
            return true;
        }

        private  byte[] serializeObject(Object obj)
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
