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
            // ezzel a függvénnyel inicializáljuk és indítjuk a játékot
            try
            {
                //eldöntjük hogy ki kezd
                Random random = new Random();
                //activeBoard.ActivePlayerIndex = random.Next(0, 1000) % 2;
                activeBoard.ActivePlayerIndex = 0;
            }
            catch (Exception)
            {
                throw new Exception("A játék inicilaizálása sikertelen");
            }

        }


        //az elenfél által választott bábut lerakjuk pálya x y korrdinátára 0 tól indexelve
       
        public void updateGameState(int x, int y)
        {
            try
            {
                if (activeBoard.checkIsEmpty(x, y))
                {
                    activeBoard.insertPiece(x, y, activeBoard.SelectedPiece);

                    activeBoard.SelectedPiece = null;
                   //activePlayer = (activePlayer + 1) % 2;
                    activeBoard.checkWinningState();
                }

            }
            catch (Exception)
            {
                throw new Exception("A Lépés nem sikerült");
            }
        }

        public void selectPiece(PlayerType type, Piece select)
        {
            if (activeBoard.SelectedPiece == null)
            {
                //kiválasztjuk a bábút amit az ellenfélnek le kell tenni majd kivesszük a bábúk tömbbjéből
                if (type == PlayerType.PlayerOne && activeBoard.ActivePlayerIndex == 0)
                {
                   
                        activeBoard.SelectedPiece = select;
                       
                        activeBoard.setActivePieces( activeBoard.ActivePieces, activeBoard.SelectedPiece);
                        activeBoard.ActivePlayerIndex = (activeBoard.ActivePlayerIndex + 1) % 2;
                        activeBoard.CurrentPlayer = activeBoard.Player[activeBoard.ActivePlayerIndex].PlayerType;
                   
                }
                else if (type == PlayerType.PlayerTwo && activeBoard.ActivePlayerIndex == 1)
                {
                    try
                    {
                        activeBoard.SelectedPiece = select;
                        activeBoard.setActivePieces(activeBoard.ActivePieces, activeBoard.SelectedPiece);
                        activeBoard.ActivePlayerIndex = (activeBoard.ActivePlayerIndex + 1) % 2;
                        activeBoard.CurrentPlayer = activeBoard.Player[activeBoard.ActivePlayerIndex].PlayerType;
                    }
                    catch (Exception)
                    {
                        throw new Exception("Hiba történt");
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
            { //ezzel indítjuk a háttérszálat automatikusan meghívásra kerül az initGame-ben
                gameThread.WorkerSupportsCancellation = true;

                gameThread.DoWork += new DoWorkEventHandler(startGameProcess);
                gameThread.RunWorkerAsync();
            }
            catch (Exception)
            { }


        }
       

        public override string GetGameTypeInfo()
        {
            return "Quarto";
        }

       

       

        public override void RegisterAsPlayer(ref AbstractGame.StepHandler onStep, PlayerType playerType)

        {
           
           
                int index = 1;
                // Index in the players list 
                if (playerType == PlayerType.PlayerOne)
                {
                    index = 0;
                }
                //if (activeBoard.Player[index].PlayerType != PlayerType.PlayerOne && activeBoard.Player[index].PlayerType != PlayerType.PlayerTwo)
                //{
                   activeBoard.Player[index] = new Player();
                    activeBoard.Player[index].PlayerType = playerType;
               
                    activeBoard.Player[index].Callback = onStep;
                //}
                if (activeBoard.Player[0] != null && activeBoard.Player[1] != null && activeBoard.Player[0].PlayerType != null && activeBoard.Player[0].PlayerType != null)
                {
                    StartGame();
                }

        }
        
        public override IState SimulateStep(IState current, AbstractStep step)
        {
            if (!(step is QuartoStep))
                throw new Exception("Not proper step type!");
            QuartoStep cStep = (QuartoStep)step;
            Board returnBoard = new Board((Board)current);
                

            if (returnBoard.SelectedPiece == null)
            {
                returnBoard.SelectedPiece= cStep.P;
                returnBoard.setActivePieces(returnBoard.ActivePieces, returnBoard.SelectedPiece);
               // returnBoard.ActivePieces = returnBoard.ActivePieces.Where(w => w != returnBoard.SelectedPiece).ToArray();
            }
            else
            {
               
                    returnBoard.insertPiece(cStep.X, cStep.Y, returnBoard.SelectedPiece);
                    //returnBoard.setActivePieces(returnBoard.ActivePieces, returnBoard.SelectedPiece);
                    returnBoard.SelectedPiece = null;
                    //activePlayer = (activePlayer + 1) % 2;
                    returnBoard.checkWinningState();
               
            }
            //returnBoard.insertPiece(((QuartoStep)step).X, ((QuartoStep)step).Y, ((QuartoStep)step).P);
            if (returnBoard == (Board)current)
            {
                return null;
            }
            return returnBoard;
        }
        
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
       
        public override AbstractStep.Result DoStep(AbstractStep step, PlayerType playerType)
        {
            if (activeBoard.Winstate || activeBoard.checkIsFull())
            {
                return AbstractStep.Result.Failure;
            }

            if (!(step is QuartoStep))
                throw new Exception("Not proper step type!");
            QuartoStep cStep = (QuartoStep)step;
            if (activeBoard.Player[activeBoard.ActivePlayerIndex].PlayerType == playerType)
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

            if (activeBoard.Winstate || activeBoard.checkIsFull())
            {
                activeBoard.CurrentPlayer = PlayerType.NoOne;
            }

            activeBoard.Player[0].Callback(activeBoard);
            activeBoard.Player[1].Callback(activeBoard);

            return AbstractStep.Result.Success;
        }
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
