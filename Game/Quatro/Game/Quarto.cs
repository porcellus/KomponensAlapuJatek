using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;
using Client.AIAlgorithmBase;
using Game;
using Game.GameBase;


namespace Game
{
    public class Quarto : AbstractGame
    {
        public Board activeBoard;

        public Quarto()
        {
            activeBoard = new Board();
        }

        static BackgroundWorker gameThread = new BackgroundWorker();
        public void initGame()
        {
            
                activeBoard.CurrentPlayer = PlayerType.PlayerOne;
                  
           

        }

        //A kiválasztott elemet lerakjuk a játéktérre
        public void updateGameState(int x, int y)
        {
            try
            {
                if (activeBoard.checkIsEmpty(x, y))
                {
                    activeBoard.insertPiece(x, y, activeBoard.SelectedPiece);

                    activeBoard.SelectedPiece = null;
                    activeBoard.checkWinningState();
                }

            }
            catch (Exception)
            {
                throw new Exception("A Lépés nem sikerült");
            }
        }
        //Kiválasztjuk a bábút
        public void selectPiece(PlayerType type, Piece select)
        {
            if (activeBoard.SelectedPiece == null)
            {
                //kiválasztjuk a bábút amit az ellenfélnek le kell tenni majd kivesszük a bábúk tömbbjéből
                if (activeBoard.ActivePieces.ToList().Contains(select))
                {
                    if (activeBoard.CurrentPlayer == type)
                    {

                        activeBoard.SelectedPiece = select;
                        activeBoard.UpdateActivePieces(activeBoard.ActivePieces, activeBoard.SelectedPiece);
                        activeBoard.UpdateCurrentPlayer();
                        

                    }
                }
               
            }

        }
        

        private void startGameProcess(object sender, DoWorkEventArgs e)
        {
            //egy külön szálként indítjuk el a játékot ez maga a háttérfolyamat
            initGame();
            //addig mig a nem nyert valaki vagy nem raktak le minden bábút
            while (activeBoard.Winstate == false || !activeBoard.checkIsFull())
            {
                //Thread.Sleep(1000);
            }
        }
        //kiértékeljük a játtékállapotot
        public override double GetHeuristicValue(IState state, PlayerType current )
        {
            if (state is Board)
            {
                Board b = (Board)state;


                int value = b.Heuristic.GetValue(b);
                if (current == PlayerType.PlayerOne)
                {
                    return -value;
                }
               
                return value;

            }
            return 0;




        }

        public  void StartGame()
        {

            try
            { //ezzel indítjuk a háttérszálat 
                gameThread.WorkerSupportsCancellation = true;

                gameThread.DoWork += new DoWorkEventHandler(startGameProcess);
                gameThread.RunWorkerAsync();
            }
            catch (Exception)
            { }


        }
       
        //game info
        public override string GetGameTypeInfo()
        {
            return "Quarto";
        }

          
        //játékosok regisztrációja
        public override void RegisterAsPlayer(ref AbstractGame.StepHandler onStep, PlayerType playerType)

        {           
           
                int index = 0;
                // Index in the players list 
                if (activeBoard.Player[0] != null)
                {
                    index = 1;
                }
                   activeBoard.Player[index] = new Player();
                    activeBoard.Player[index].PlayerType = playerType;
               
                    activeBoard.Player[index].Callback = onStep;
                 if (activeBoard.Player[0] != null && activeBoard.Player[1] != null )
                {
                    StartGame();
                }

        }
        // egy lépés szimulálása
        public override IState SimulateStep(IState current, AbstractStep step)
        {
            if (!(step is QuartoStep))
                throw new Exception("Not proper step type!");
            QuartoStep cStep = (QuartoStep)step;
            Board returnBoard = new Board((Board)current);
                

            if (returnBoard.SelectedPiece == null)
            {
                returnBoard.SelectedPiece= cStep.P;
                returnBoard.UpdateActivePieces(returnBoard.ActivePieces, returnBoard.SelectedPiece);
              
            }
            else
            {
               
                   returnBoard.insertPiece(cStep.X, cStep.Y, returnBoard.SelectedPiece);
                   returnBoard.SelectedPiece = null;
                   returnBoard.checkWinningState();
               
            }
           
            if (returnBoard == (Board)current)
            {
                return null;
            }
            return returnBoard;
        }
        //Segéd a konzolos kiiratáshoz
        private string pieceToString(Piece p)
        {
            if (p == null || p.color == 2) return "----";
            var ret = "";
            if (p.height == 1) ret += "M";
            else ret += "A";
            if(p.full == 1) ret += "T";
            else ret += "U";
            if (p.shape == 1) ret += "O";
            else ret += "Z";
            if (p.color == 1) ret += "Z";
            else ret += "K";
            return ret;
        }
        //Lépés
        public override AbstractStep.Result DoStep(AbstractStep step, PlayerType playerType)
        {
            if (activeBoard.Winstate || activeBoard.checkIsFull())
            {
                return AbstractStep.Result.Failure;
            }

            if (!(step is QuartoStep))
                throw new Exception("Not proper step type!");
            QuartoStep cStep = (QuartoStep)step;
            if (activeBoard.CurrentPlayer == playerType)
            {
                if (activeBoard.SelectedPiece == null)
                {
                    selectPiece(playerType, cStep.P);
                }
                else
                {
                    updateGameState(cStep.X, cStep.Y);
                }
            }
            System.Diagnostics.Debug.WriteLine(playerType + " steps: " + cStep.X + "," + cStep.Y + ":" + pieceToString(cStep.p));
            System.Diagnostics.Debug.WriteLine("==>");
            System.Diagnostics.Debug.WriteLine("Selected piece: " + pieceToString(activeBoard.SelectedPiece));
            for (int i = 0; i < 4; ++i )
            {
                for (int j = 0; j < 4; ++j)
                {
                    System.Diagnostics.Debug.Write(pieceToString(activeBoard.BBoard[i, j]));
                    System.Diagnostics.Debug.Write(" - ");
                }
                System.Diagnostics.Debug.WriteLine("");
            }

            activeBoard.Player[0].Callback(activeBoard);
            activeBoard.Player[1].Callback(activeBoard);
            if (activeBoard.Winstate || activeBoard.checkIsFull())
            {
                activeBoard.CurrentPlayer = PlayerType.NoOne;
            }

            

            return AbstractStep.Result.Success;
        }
        //A lehetséges lépések kiszámítása
        public override IEnumerable<AbstractStep> GetAvailableSteps(IState st)
        {
            try
            {
                Board  state = new Board((Board)st);
                List<QuartoStep> lista = new List<QuartoStep>();

                QuartoStep step;

                if (state.SelectedPiece != null )
                {
                    for (int j = 0; j < 4; j++)
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            if (state.BBoard[j, k].color == 2)
                            {
                                step = new QuartoStep(j, k, state.SelectedPiece);
                                lista.Add(step);
                            }
                        }

                    }
                }
                else
                {
                    for (int i = 0; i < state.ActivePieces.Length; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            for (int k = 0; k < 4; k++)
                            {
                                if (state.BBoard[j, k].color == 2)
                                {
                                    step = new QuartoStep(j, k, state.ActivePieces[i]);
                                    lista.Add(step);
                                }

                            }

                        }

                    }
                }



                return lista;
            }
            catch (Exception)
            {
                return new List<QuartoStep>();
            }
        }

       
    }

}
