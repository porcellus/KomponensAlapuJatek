using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MinMax;
using Game.Chess.Heuristic;
using Game.Chess.GameGUI;
using Game.GameBase;

namespace Game.Chess.Game
{
    public class Chess : IGameBase
    {
        enum GUIAction
	    {
	        
	    }

        // Pályaelemek
        protected Dictionary<char, Figure>          figures;
        protected Board                             board;

	    // Játékosok
        protected LinkedList<Player>                players;			// A játékosokat tartalmazó vektor
        protected int                               cPlayer;			// Az aktuális játékos indexe

        // Egérkezelés
        protected float                             prevX, prevY;		// Előző egérkattintás pozíciója

        // A FrameUpdate függvény tevékenységének típusa
        protected bool                              waiting;
        protected bool                              clicked;
        protected bool                              figureSelected;
        protected int[]                             figurePos;
        protected int[]                             lastPos;
        protected int                               changeTime;
        protected bool                              alive;
        protected ChessHeuristic                    heuristic;


        public Chess()
        {
            cPlayer         = 0;
            clicked         = false;
            figureSelected  = false;
            figurePos       = new int[] {0, 0};
            lastPos         = new int[] {-1, -1}; 
            changeTime      = 0;
            board           = null;
        }

	    public ~Chess()
        {
        }

        /* GUI által használt függvények */


        public void GUI_Action_()
        {

        }

        /* Belső függvények */
        protected void NewGame(Player.PlayerType ptype1, Player.PlayerType ptype2, ChessHeuristic heur)
        {
            alive               = true;
            int[] dimension     = {8,8};
            
            heuristic           = heur;

            board               = new Board();
            board.setHeuristic(heur);
            board.setDimension(dimension);
            board.setContent("5432134566666666000000000000000000000000000000006666666654312345");
            board.setContentColor("2222222222222222000000000000000000000000000000001111111111111111");

            players             = new LinkedList<Player>();
            players.AddLast(new Player(board, true, ptype1));
            players.AddLast(new Player(board, false, ptype2));

            waiting             = players.ElementAt(0).IsHuman();
            figures             = new Dictionary<char,Figure>();

            for (int i = 0; i<board.getDimension()[1]; ++i)
	        {
		        for (int j = 0; j<board.getDimension()[0]; ++j)
		        {
				    // A mezőhöz tartozó bábú
				    char temp = board.getItemByRC(i+1,j+1);

                    if (temp != Board.BOARD_PIECE_NULL && temp != Board.BOARD_PIECE_EMPTY)
			        {	
					        int type        = int(temp) - 48;
					        Figure figure   = new Figure(type, i+1, j+1, board);
					        --type;
					
					        if (board.getContentColor(i+1, j+1) == '2')
						        figure.setColor(false);
					        else if (board.getContentColor(i+1, j+1) == '1')
						        figure.setColor(true);

					        char fsym       = board.getNextSym();
					        board.setContentByRC(i+1, j+1, fsym);	
                            figures.Add(fsym, figure);
			        }
                }
            }

            foreach (Figure figure in figures.Values)
                figure.calculateLegalSteps();

            while (alive)
            {
                FrameUpdate();
            }
        }

        /* Fő update függvény */ 
        protected void FrameUpdate()
        {
			// Annak vizsgálata, hogy MI-s játékos következik-e
			if (waiting) // Ha emberi játékos, akkor egérkattintás eseményre várunk
			{
				
			} else // Ha gépi játékos, akkor a hozzá tartozó algoritmus fut le
			{

            }

        }

        public void Reset()
        {
	        cPlayer			= 0;
            board           = null;

            players.Clear();
	        figures.Clear();
        }

        public void SetLight() 					// Fények állítása
        {

        }

        public bool OnMouse()					// Egérkattintás kezelése
        {
            return false;
        }

        public void changePlayer()			    // Játékos váltása
        {
            cPlayer = (cPlayer + 1) % 2;
            waiting = players.ElementAt(cPlayer).IsHuman();
        }
        
        public void Win(Player p)
        {
            Player winner = p;

            // TODO: GUI értesítése a végéről

            Reset();
        }
    }
}
