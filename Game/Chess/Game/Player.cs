using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.GameBase;
using Client.AIAlgorithmBase;

namespace Game
{
    public class Player
    {        
        protected EntityType        type;
        protected bool              white;
        protected bool              inCheck;
        protected Board             board;
        protected IAIAlgorithm      algorithm;
        AbstractGame.StepHandler    callback;

        public Player(Board b, bool w, EntityType ptype, ref AbstractGame.StepHandler onStep)
        {
            type = ptype;
            white = w;
            board = b;
            callback    = onStep;
        }

        ~Player()
        {
	       
        }

        public GameBase.AbstractGame.StepHandler Callback
        {
            get { return callback; }
            set { callback = value; }
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
            return type == EntityType.ComputerPlayer;
        }

        public bool IsHuman()
        {
            return type == EntityType.HumanPlayer;
        }
    }
}
