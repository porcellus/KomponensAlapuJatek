using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;
using GameBase;
using Client.AIAlgorithmBase;
using Game.GameBase;


namespace Game
{
    public class Quarto
    {
        private static Board activeBoard;

        public static Board ActiveBoard
        {
            get { return Quarto.activeBoard; }
            
        }
        static QuartoHeuristic heuristic;

      
             
        static Player[] player;
        //játékos tömb lekérdezése
        public static bool call = true;

        static Piece[] activePieces;

        public static Piece[] ActivePieces
        {
            get { return Quarto.activePieces; }
            
        }

     
       
        static int activePlayer;
        // az aktuális játékos indexe a Player tömbbne
       
        // az ellenfél számára választott bábú
        static Piece selectedPiece;

        public static Piece SelectedPiece
        {
            get { return Quarto.selectedPiece; }
           
        }

        

        static BackgroundWorker gameThread = new BackgroundWorker();
        public static void initGame()
        {
            // ezzel a függvénnyel inicializáljuk és indítjuk a játékot
            try
            {
               activeBoard = new Board();
                for (int i = 0; i < activeBoard.BHeight; i++)
                {
                    for (int j = 0; j < activeBoard.BWidth; j++)
                    {
                        activeBoard.BBoard[i, j] = new Piece();
                        activeBoard.BBoard[i, j].color = 2;
                        activeBoard.BBoard[i, j].height = 2;
                        activeBoard.BBoard[i, j].shape = 2;
                        activeBoard.BBoard[i, j].full = 2;

                    }
                }
                player = new Player[2];
               



                //a bábuk tömbje 1 0 kommentezve a Pieaces osztálynál
                activePieces = new Piece[16];
                activePieces[0] = new Piece();
                activePieces[0].setPiece(1, 1, 1, 1);
                activePieces[1] = new Piece();
                activePieces[1].setPiece(1, 1, 1, 0);
                activePieces[2] = new Piece();
                activePieces[2].setPiece(1, 1, 0, 1);
                activePieces[3] = new Piece();
                activePieces[3].setPiece(1, 1, 0, 0);
                activePieces[4] = new Piece();
                activePieces[4].setPiece(1, 0, 1, 1);
                activePieces[5] = new Piece();
                activePieces[5].setPiece(1, 0, 1, 0);
                activePieces[6] = new Piece();
                activePieces[6].setPiece(1, 0, 0, 1);
                activePieces[7] = new Piece();
                activePieces[7].setPiece(1, 0, 0, 0);
                activePieces[8] = new Piece();
                activePieces[8].setPiece(0, 1, 1, 1);
                activePieces[9] = new Piece();
                activePieces[9].setPiece(0, 1, 1, 0);
                activePieces[10] = new Piece();
                activePieces[10].setPiece(0, 1, 0, 1);
                activePieces[11] = new Piece();
                activePieces[11].setPiece(0, 1, 0, 0);
                activePieces[12] = new Piece();
                activePieces[12].setPiece(0, 0, 1, 1);
                activePieces[13] = new Piece();
                activePieces[13].setPiece(0, 0, 1, 0);
                activePieces[14] = new Piece();
                activePieces[14].setPiece(0, 0, 0, 1);
                activePieces[15] = new Piece();
                activePieces[15].setPiece(0, 0, 0, 0);

                //eldöntjük hogy ki kezd
                Random random = new Random();
                activePlayer = random.Next(0, 1000) % 2;
            }
            catch (Exception)
            {
                throw new Exception("A játék inicilaizálása sikertelen");
            }

        }


        //az elenfél által választott bábut lerakjuk pálya x y korrdinátára 0 tól indexelve
        public  Piece[,] getAviableStep()
        {
            try
            {
                return activeBoard.BBoard;
            }
            catch (Exception)
            {
                return null;
            }
        
        }
        public Piece[] getAviablePiece()
        {
            try
            {
                return activePieces;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public bool getWinningState()
        {
            return activeBoard.Winstate;
        }
        public Piece getSelectedPiece()
        {
            try
            {
                return selectedPiece;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public void updateGameState(int x, int y)
        {
            try
            {
                if (activeBoard.checkIsEmpty(x, y))
                {
                    activeBoard.insertPiece(x, y, selectedPiece);

                    selectedPiece = null;
                   //activePlayer = (activePlayer + 1) % 2;
                    activeBoard.checkWinningState();
                }

            }
            catch (Exception)
            {
                throw new Exception("A Lépés nem sikerült");
            }
        }
        public static void selectPiece(PlayerType type, Piece select)
        {
            if (selectedPiece == null)
            {
                //kiválasztjuk a bábút amit az ellenfélnek le kell tenni majd kivesszük a bábúk tömbbjéből
                if (type == PlayerType.PlayerOne && activePlayer == 0)
                {
                    try
                    {
                        selectedPiece = select;
                        activePieces = activePieces.Where(w => w != selectedPiece).ToArray();
                        activePlayer = (activePlayer + 1) % 2;
                    }
                    catch (Exception)
                    {
                        throw new Exception("Hiba történt");
                    }
                }
                else if (type == PlayerType.PlayerTwo && activePlayer == 1)
                {
                    try
                    {
                        selectedPiece = select;
                        activePieces = activePieces.Where(w => w != selectedPiece).ToArray();
                        activePlayer = (activePlayer + 1) % 2;
                    }
                    catch (Exception)
                    {
                        throw new Exception("Hiba történt");
                    }
                }
            }

        }
        public bool isTreminate(IState state)
        {
            try
            {
                Board sState = (Board)state;
                sState.checkWinningState();

                return sState.Winstate;
            }
            catch(Exception )
            {
                return false;
            }

        }

        private static void startGameProcess(object sender, DoWorkEventArgs e)
        {
            //egy külön szálként indítjuk el a játékot ez maga a háttérfolyamat
            initGame();
            //addig mig a nem nyert valaki vagy nem raktak le minden bábút
            while (activeBoard.Winstate == false || !activeBoard.checkIsFull())
            {
                if ((player[0].PlayerType == PlayerType.PlayerOne || player[0].PlayerType == PlayerType.PlayerTwo) && (player[1].PlayerType == PlayerType.PlayerOne || player[1].PlayerType == PlayerType.PlayerTwo))
                {
                    if (call)
                    {
                        GameBase.AbstractGame.StepHandler step = new AbstractGame.StepHandler(player[activePlayer].Callback);
                        step(activeBoard);
                        call = false;
                    }
                    //várunk a játékosokra
                            Thread.Sleep(1000);
                        
                 
                }
                // várunk a játékosok regisztrációjára
                Thread.Sleep(1000);

            }
        }
        public  double GetHeuristicValue(IState state)
        {
            if (state is Board)
            {
                Board b = (Board)state;
                int value = heuristic.GetValue(b);

               
                return value;

            }
            return 0;




        }

        public  void StartGame()
        {

            try
            { //ezzel indítjuk a háttérszálat automatikusan meghívásra kerül az initGame-ben
                gameThread.WorkerSupportsCancellation = true;

                gameThread.DoWork += new DoWorkEventHandler(startGameProcess);
                gameThread.RunWorkerAsync();
            }
            catch (Exception)
            { }


        }
        public  void  SetHeuristic<Board>(IHeuristic<Board> heur)
        {
            if (!(heur is QuartoHeuristic))
                throw new ArgumentException("Not valid heuristic type for this game!");

            heuristic = (QuartoHeuristic)heur;
        }

        public  string GetGameTypeInfo()
        {
            return "Quarto";
        }

        public  void RegisterAsPlayer<TAlgorithm>(ref AbstractGame.StepHandler onStep, PlayerType playerType, EntityType controller, TAlgorithm algorithm)
        {
            if (!(algorithm is IAIAlgorithm))
                throw new Exception("Not valid algorithm type!");

           
                int index = 1;
                // Index in the players list 
                if (playerType == PlayerType.PlayerOne)
                {
                    index = 0;
                }
                if (player[index].PlayerType != PlayerType.PlayerOne && player[index].PlayerType != PlayerType.PlayerTwo)
                {
                    player[index].PlayerEntity = controller;
                    player[index].PlayerType = playerType;
                    player[index].Algorithm = (IAIAlgorithm)algorithm;
                    player[index].Callback = onStep;
                }

        }
         public  IState SimulateStep(AbstractStep step )
        {
            if (!(step is QuartoStep))
                throw new Exception("Not proper step type!");

            QuartoStep cStep = (QuartoStep)step;
            Board clone = activeBoard;

            clone.insertPiece(((QuartoStep)step).X, ((QuartoStep)step).Y, ((QuartoStep)step).P);

            return (IState)clone;
            
        }
        public override IState GetNextState(IState current, AbstractStep step)
        {
            Board returnBoard = ((Board)current);
            returnBoard.insertPiece(((QuartoStep)step).X, ((QuartoStep)step).Y, ((QuartoStep)step).P);

            return (IState)returnBoard;
        }
        
       
        public override AbstractStep.Result DoStep(AbstractStep step, PlayerType playerType)
        {
            if (!(step is QuartoStep))
                throw new Exception("Not proper step type!");
            if (player[activePlayer].PlayerType == playerType)
            {
                QuartoStep cStep = (QuartoStep)step;
                if (selectedPiece == null)
                {
                    selectPiece(playerType, cStep.P);
                    call = true;
                }
                else
                {
                    updateGameState(cStep.X, cStep.Y);
                    call = true;

                }



                return AbstractStep.Result.Success;
            }
            else
            {
                return AbstractStep.Result.Failure;
            }
        }
    }

}
