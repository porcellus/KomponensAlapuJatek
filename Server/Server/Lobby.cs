using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Lobby
    {
        private List<ServerClient> clients;

        public Lobby() { }

        public void add(ServerClient client)
        {
            clients.Add(client);
        }

        public void remove(ServerClient client)
        {
            clients.Remove(client);
        }

        public ServerClient get(Int32 index)
        {
            return clients[index];
        }

        public ServerClient getbyId(Int32 id)
        {

            return clients.Find(x => x.id == id);

        }

        public List<ServerClient> getClientlist()
        {
            return clients;
        }

        public String getClientsInfo(String gametype)
        {
            String infos = "";
            foreach (ServerClient client in clients)
            {
                if (client.gametype.Equals(gametype))
                {
                    infos += client.name + "," + client.ip + "," + client.port + ";";
                }
            }
            return infos;

        }

    }
}
