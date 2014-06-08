using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Threading;
using Game.GameBase;
using Client.AIAlgorithmBase;

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
        private bool                              gameEnded;

        static BackgroundWorker                   gameThread        = new BackgroundWorker();

        public Chess()
        {
            cPlayer                 = 0;
            clicked                 = false;
            figureSelected          = false;
            figurePos               = new int[] {0, 0};
            lastPos                 = new int[] {-1, -1}; 
            changeTime              = 0;
            board                   = null;
        }

	    ~Chess()
        {
        }

        private void startGameProcess(object sender, DoWorkEventArgs e)
        {
            while (alive)
            {
            }
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

        public override IEnumerable<AbstractStep> GetAvailableSteps(IState st)
        {
            try
            {
                Board state             = ((Board)st).Clone();
                List<ChessStep> lista   = new List<ChessStep>();

                ChessStep step;
                
                for (int i = 0; i < board.getDimension()[1]; ++i)
                    for (int j = 0; j < board.getDimension()[0]; ++j)
                    {
                        Figure figure = board.getFigureAt(i,j);
                        if (figure != null && (figure.isWhite() && cPlayer == 0 || !figure.isWhite() && cPlayer == 1))
                        {
                            LinkedList< int[] > legalSteps  = figure.getLegalSteps();

                            for (int k = 0; k < legalSteps.Count; ++k)
                            {
                                Figure.StepType type;

                                if (legalSteps.ElementAt(k)[2] == 1)
                                    type = Figure.StepType.Success;
                                else if (legalSteps.ElementAt(k)[2] == 2)
                                    type = Figure.StepType.Capture;
                                else
                                    type = Figure.StepType.CaptureKing;

                                step = new ChessStep(figure.getRow(), figure.getCol(), legalSteps.ElementAt(k)[0], legalSteps.ElementAt(k)[1], type);
                            }
                        }
                    }

                return lista;
            }
            catch (Exception)
            {
                return new List<ChessStep>();
            }
        }

        /* Belső függvények */
        public void StartGame()
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
            board.setCurrentPlayer(PlayerType.PlayerOne);
            board.setContent("5432134566666666000000000000000000000000000000006666666654312345",
                             "2222222222222222000000000000000000000000000000001111111111111111");
            
            waiting             = players[0].IsHuman();

            foreach (Figure figure in board.getData())
                if (figure != null && figure.getFigureType() != Figure.FigureType.Nothing)
                {
                    Console.WriteLine(figure.getFigureType());
                    figure.calculateLegalSteps();
                }

            try
            {
                gameThread.WorkerSupportsCancellation = true;

                gameThread.DoWork += new DoWorkEventHandler(startGameProcess);
                gameThread.RunWorkerAsync();
            }
            catch (Exception)
            {
            }
        }

        public bool IsTerminal(IState state)
        {
            throw new NotImplementedException();
        }

        public override double GetHeuristicValue(IState state, PlayerType current)
        {
            if (state is Board)
            {
                Board b     = (Board)state;
                int value   = b.getValue();

                if (current != PlayerType.PlayerTwo)
                    return -value;
                return value;

            }
            return 0;

        }

        public IState GetNextState(IState current, AbstractStep step)
        {
            throw new NotImplementedException();
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

            alive = false;

            Reset();
        }

        public void AddHumanPlayer(ref AbstractGame.StepHandler onStep, GameBase.PlayerType playerType)
        {
            if (players == null)
                players     = new Player[2];

            // Index in the players list 
            int index       = playerType == PlayerType.PlayerOne ? 0 : 1;
            Player player   = new Player(board, index == 0, EntityType.HumanPlayer, ref onStep);
            players[index]  = player;
        }

        /* Interface methods */
        public void SetHeuristic<Board>(IHeuristic<Board> heuristic)
        {
            if (!(heuristic is ChessHeuristic))
                throw new ArgumentException("Not valid heuristic type for this game!");

            this.heuristic = (ChessHeuristic)heuristic;
        }

        public override string GetGameTypeInfo()
        {
            return "Chess";
        }

        public void RegisterAsHumanPlayer(ref StepHandler onStep, GameBase.PlayerType playerType)
        {
            AddHumanPlayer(ref onStep, playerType);
        }

        public override void RegisterAsPlayer(ref AbstractGame.StepHandler onStep, GameBase.PlayerType playerType)
        {
            if (players == null)
                players = new Player[2];

            // Index in the players list 
            int index       = playerType == PlayerType.PlayerOne ? 0 : 1;
            Player player   = new Player(board, index == 0, EntityType.ComputerPlayer, ref onStep);
            players[index]  = player;

            if (players[0] != null && players[1] != null)
                StartGame();
        }

        public override AbstractStep.Result DoStep(AbstractStep step, GameBase.PlayerType playerType)
        {
            if (!(step is ChessStep))
                throw new Exception("Not proper step type!");

            ChessStep cStep = (ChessStep)step;
            int[] from      = cStep.GetFromPosition();
            int[] to        = cStep.GetToPosition();

            board.Step(from[0], from[1], to[0], to[1]);

            players[0].Callback(board);
            players[1].Callback(board);

            if (board.checkmateTest(cPlayer != 0))
            {
                Win(players[cPlayer]);
                return AbstractStep.Result.Success;
            }

            cPlayer         = (cPlayer + 1) % 2;
            board.setCurrentPlayer(cPlayer == 0 ? PlayerType.PlayerOne : PlayerType.PlayerTwo);
            
            return AbstractStep.Result.Success;
        }

        public override IState SimulateStep(IState current, AbstractStep step)
        {
            if (!(step is ChessStep))
                throw new Exception("Not proper step type!");

            ChessStep cStep     = (ChessStep)step;
            Board clone         = board.Clone();
            
            int[] from          = cStep.GetFromPosition();
            int[] to            = cStep.GetToPosition();

            clone.Step(from[0], from[1], to[0], to[1]);

            if (clone == (Board)current)
            {
                return null;
            }

            return clone;
        }
    }
}
