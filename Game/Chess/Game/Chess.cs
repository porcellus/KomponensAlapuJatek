using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Chess.Heuristic;
using Game.Chess.GameGUI;
using Game.GameBase;

namespace Game.Chess.Game
{
    class Chess : AbstractGame
    {
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

	    ~Chess()
        {
        }

        /* GUI által használt függvények */
        public LinkedList< int[] > GetFigures()
        {
            LinkedList<int[]> result = new LinkedList<int[]>();

            // int[0] - Figura típusa
            // int[1] - Figura színe
            // int[2,3] - X,Y koordinátája
            for (int r = 1; r < board.getDimension()[0]; ++r)
                for (int c = 1; c < board.getDimension()[1]; ++c)
                    if (board.getItemByRC(r, c) != (char)Figure.FigureType.Nothing && board.getItemByRC(r, c) != (char)Figure.FigureType.Nothing)
                    {
                        int[] figure = new int[] { 0, 0, r, c };
                        result.AddLast(figure);
                    }

            return result;
        }

        public int GUI_Action_Step(int fromRow, int fromCol, int toRow, int toCol)
        {
            return 0;   
        }

        /* Belső függvények */
        public void StartGame(Player.PlayerType ptype1, Player.PlayerType ptype2, ChessHeuristic heur)
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

                    if (temp != (char)Figure.FigureType.Nothing)
			        {	
					        int type        = (int)(temp) - 48;
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

        /* Interface methods */
        public override string GetGameTypeInfo()
        {
            return "";
        }

        public override void RegisterAsPlayer(ref StepHandler onStep, GameBase.PlayerType playerType)
        {
            if (players.Count >= 2)
                return;
            

        }

        public override void DoStep(AbstractStep step, GameBase.PlayerType playerType)
        {
            if (!(step is ChessStep))
                throw new Exception("Not proper step type!");

            ChessStep cStep = (ChessStep)step;
            int[] from      = cStep.GetFromPosition();
            int[] to        = cStep.GetToPosition();

            board.Step(from[0], from[1], to[0], to[1]);
        }

        public override AbstractGame SimulateStep(AbstractStep step)
        {
            if (!(step is ChessStep))
                throw new Exception("Not proper step type!");

            ChessStep cStep     = (ChessStep)step;
            Board clone         = board.Clone();
            
            int[] from          = cStep.GetFromPosition();
            int[] to            = cStep.GetToPosition();

            clone.Step(from[0], from[1], to[0], to[1]);


            return this;
        }
    }
}
