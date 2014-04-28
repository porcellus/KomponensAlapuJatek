using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Lobby
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

        public String getClientsName()
        {
            String names = "";
            foreach(ServerClient client in clients)
            {
                names += client.name + "\n";
            }
            return names;



        }

    }
}
