using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ServerClient
    {
        public Int32 id;
        public String port;
        public String ip;
        public String name;
        public String gametype;

        public ServerClient(ServerClient client)
        {
            this.id = client.id;
            this.port = client.port;
            this.ip = client.ip;
            this.name = client.name;
            this.gametype = client.gametype;
        }

        public ServerClient() { }
    }
}
