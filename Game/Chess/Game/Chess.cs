using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameBase;
using Client.AIAlgorithmBase;
using GameBase;

namespace Game.Chess.Game
{
    public class Chess : AbstractGame
    {
        // Pályaelemek
        private Dictionary<char, Figure>          figures;
        private Board                             board;

	    // Játékosok
        private Player[]                          players;			// A játékosokat tartalmazó tömb
        private int                               cPlayer;			// Az aktuális játékos indexe

        // Egérkezelés
        private float                             prevX, prevY;		// Előző egérkattintás pozíciója

        // A FrameUpdate függvény tevékenységének típusa
        private bool                              waiting;
        private bool                              clicked;
        private bool                              figureSelected;
        private int[]                             figurePos;
        private int[]                             lastPos;
        private int                               changeTime;
        private bool                              alive;
        private ChessHeuristic                    heuristic;


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
        public override void StartGame()
        {
            if (heuristic == null)
                throw new Exception("Heuristic hasn't set yet.");

            if (players == null || players[0] == null || players[1] == null)
                throw new Exception("Players hasn't initialized proper yet.");

            alive               = true;
            int[] dimension     = {8,8};

            board               = new Board();
            board.setHeuristic(heuristic);
            board.setDimension(dimension);
            board.setContent("5432134566666666000000000000000000000000000000006666666654312345");
            board.setContentColor("2222222222222222000000000000000000000000000000001111111111111111");
            
            waiting             = players[0].IsHuman();
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

        public override bool IsTerminal(IState state)
        {
            throw new NotImplementedException();
        }

        public override double GetHeuristicValue(IState state)
        {
            throw new NotImplementedException();
        }

        /* Fő update függvény */ 
        private void FrameUpdate()
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

	        figures.Clear();

            players         = null;
            figures         = null;
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
        public override void SetHeuristic<Board>(IHeuristic<Board> heuristic)
        {
            if (!(heuristic is ChessHeuristic))
                throw new ArgumentException("Not valid heuristic type for this game!");

            this.heuristic = (ChessHeuristic)heuristic;
        }

        public override string GetGameTypeInfo()
        {
            return "";
        }

        public override void RegisterAsPlayer<TAlgorithm>(ref StepHandler onStep, GameBase.PlayerType playerType, GameBase.EntityType controller, TAlgorithm algorithm)
        {
            if (!(algorithm is IAIAlgorithm))
                throw new Exception("Not valid algorithm type!");

            if (players == null)
                players = new Player[2];

            // Index in the players list 
            int index       = playerType == PlayerType.PlayerOne ? 0 : 1;
            Player player   = new Player(board, index == 0, controller, (IAIAlgorithm)algorithm);

            players[index]  = player;
        }

        public override AbstractStep.Result DoStep(AbstractStep step, GameBase.PlayerType playerType)
        {
            if (!(step is ChessStep))
                throw new Exception("Not proper step type!");

            ChessStep cStep = (ChessStep)step;
            int[] from      = cStep.GetFromPosition();
            int[] to        = cStep.GetToPosition();

            board.Step(from[0], from[1], to[0], to[1]);

            return AbstractStep.Result.Success;
        }

        public override IState SimulateStep(AbstractStep step)
        {
            throw new NotImplementedException();
        }

        public override int SimulateStep(AbstractStep step, int dummyInt)
        {
            if (!(step is ChessStep))
                throw new Exception("Not proper step type!");

            ChessStep cStep     = (ChessStep)step;
            Board clone         = board.Clone();
            
            int[] from          = cStep.GetFromPosition();
            int[] to            = cStep.GetToPosition();

            clone.Step(from[0], from[1], to[0], to[1]);
            return clone.getValue();
        }
    }
}
