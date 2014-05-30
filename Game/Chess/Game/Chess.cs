using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameBase;
using Client.AIAlgorithmBase;
using GameBase;

namespace Game
{
    public class Chess : AbstractGame
    {
        // Pályaelemek
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
        public Figure[,] GetFigures()
        {
            return board.getData();
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
            board.setContent("5432134566666666000000000000000000000000000000006666666654312345",
                             "2222222222222222000000000000000000000000000000001111111111111111");
            
            waiting             = players[0].IsHuman();

            foreach (Figure figure in board.getData())
                if (figure != null && figure.getFigureType() != Figure.FigureType.Nothing)
                {
                    Console.WriteLine(figure.getFigureType());
                    figure.calculateLegalSteps();
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

        public override IState GetNextState(IState current, AbstractStep step)
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
            players         = null;
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

        public void AddHumanPlayer(GameBase.PlayerType playerType)
        {
            if (players == null)
                players     = new Player[2];

            // Index in the players list 
            int index       = playerType == PlayerType.PlayerOne ? 0 : 1;
            Player player   = new Player(board, index == 0, EntityType.HumanPlayer);
            players[index]  = player;
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

        public override void RegisterAsHumanPlayer(ref StepHandler onStep, GameBase.PlayerType playerType)
        {
            AddHumanPlayer(playerType);
        }

        public override void RegisterAsPlayer<TAlgorithm>(ref StepHandler onStep, GameBase.PlayerType playerType, TAlgorithm algorithm)
        {
            if (!(algorithm is IAIAlgorithm))
                throw new Exception("Not valid algorithm type!");

            if (players == null)
                players = new Player[2];

            // Index in the players list 
            int index       = playerType == PlayerType.PlayerOne ? 0 : 1;
            Player player   = new Player(board, index == 0, EntityType.ComputerPlayer, (IAIAlgorithm)algorithm);

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
