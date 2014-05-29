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
                    try
                    {
                        activeBoard.SelectedPiece = select;
                        activeBoard.ActivePieces = activeBoard.ActivePieces.Where(w => w != activeBoard.SelectedPiece).ToArray();
                        activeBoard.ActivePlayerIndex = (activeBoard.ActivePlayerIndex + 1) % 2;
                        activeBoard.CurrentPlayer = activeBoard.Player[activeBoard.ActivePlayerIndex].PlayerType;
                    }
                    catch (Exception)
                    {
                        throw new Exception("Hiba történt");
                    }
                }
                else if (type == PlayerType.PlayerTwo && activeBoard.ActivePlayerIndex == 1)
                {
                    try
                    {
                        activeBoard.SelectedPiece = select;
                        activeBoard.ActivePieces = activeBoard.ActivePieces.Where(w => w != activeBoard.SelectedPiece).ToArray();
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
            Board returnBoard = new Board((Board)current);
            returnBoard.insertPiece(((QuartoStep)step).X, ((QuartoStep)step).Y, ((QuartoStep)step).P);

            return returnBoard;
        }
        
       
        public override AbstractStep.Result DoStep(AbstractStep step, PlayerType playerType)
        {
            if (!(step is QuartoStep))
                throw new Exception("Not proper step type!");
            if (activeBoard.Player[activeBoard.ActivePlayerIndex].PlayerType == playerType)
            {
                QuartoStep cStep = (QuartoStep)step;
                if (activeBoard.SelectedPiece == null)
                {
                    selectPiece(playerType, cStep.P);
                    
                }
                else
                {
                    updateGameState(cStep.X, cStep.Y);
                    

                }


               
            }
            activeBoard.Player[0].Callback(activeBoard);
            activeBoard.Player[1].Callback(activeBoard);

            return AbstractStep.Result.Success;
        }
        public override IEnumerable<AbstractStep> GetAvailableSteps(IState st)
        {
            try
            {
              Board  state = (Board)st;
                List<QuartoStep> lista = new List<QuartoStep>();

                QuartoStep step;

                if (activeBoard.SelectedPiece == null && activeBoard.BBoard != null && activeBoard == (Board)state)
                {

                    for (int i = 0; i < activeBoard.ActivePieces.Length; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            for (int k = 0; k < 4; k++)
                            {
                                if (state.BBoard[j, k].color == 2)
                                {


                                    step = new QuartoStep(j, k, activeBoard.SelectedPiece);
                                    lista.Add(step);
                                }

                            }

                        }

                    }


                }
                else
                {
                    for (int i = 0; i < activeBoard.ActivePieces.Length; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            for (int k = 0; k < 4; k++)
                            {
                                if (state.BBoard[j, k].color == 2)
                                {


                                    step = new QuartoStep(j, k, activeBoard.ActivePieces[i]);
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
