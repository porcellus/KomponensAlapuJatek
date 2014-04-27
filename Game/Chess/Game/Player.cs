using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Chess.Heuristic;

namespace Game
{
    class Player
    {
        public enum PlayerType
        {
            Human,
            Computer
        };
        
        protected PlayerType        type;
        protected bool              white;
        protected bool              inCheck;
        protected Board             board;

        public Player(Board b, bool w, PlayerType ptype)
        {
            type            = ptype;
            white           = w;
            board           = b;
        }

        public ~Player()
        {
	       
        }

        public void Move()
        {
           
        }

	    public int[] GetMove()
        {
            return new int[] {0,0,0,0};
        }

	    public void Reset()
        {
           
        }

	    public void SetColor(bool w)		
        { 
            white = w; 
        }

	    public void SetBoard(Board b)		
        { 
            board = b; 
        }

	    public bool IsWhite()				
        { 
            return white; 
        }

        public bool IsAI()
        {
            return type == PlayerType.Computer;
        }

        public bool IsHuman()
        {
            return type == PlayerType.Human;
        }
    }
}
