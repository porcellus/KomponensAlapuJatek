using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApplication
{
    class Game
    {
        public ServerClient currplayer;
        public ServerClient secondplayer;
        public Object gamefiled;
        public bool gamestarted = false;
        public bool gamefinished = false;

        public Game(ServerClient currplayer, ServerClient secondplayer, Object gamefiled)
        {
            this.currplayer = currplayer;
            this.secondplayer = secondplayer;
            this.gamefiled = gamefiled;
        }

    }
}
