using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ServerClient
    {
        public Int32 id;
        public String port;
        public String ip;
        public String name;
        public String gametype;
        public Socket clientsocket;
        public bool startedrecive;

        public ServerClient(Int32 id, String port, String ip, String name, String gametype, Socket socket, bool startedrevice)
        {
            this.id = id;
            this.port = port;
            this.ip = ip;
            this.name = name;
            this.gametype = gametype;
            this.clientsocket = socket;
            this.startedrecive = startedrevice;
        }
    }
}
