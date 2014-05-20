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
        public static Board activeBoard = new Board();

             
      
        

        static BackgroundWorker gameThread = new BackgroundWorker();
        public static void initGame()
        {
            // ezzel a függvénnyel inicializáljuk és indítjuk a játékot
            try
            {
               
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
              


           

                //a bábuk tömbje 1 0 kommentezve a Pieaces osztálynál
               
                activeBoard.ActivePieces[0] = new Piece();
                activeBoard.ActivePieces[0].setPiece(1, 1, 1, 1);
                activeBoard.ActivePieces[1] = new Piece();
                activeBoard.ActivePieces[1].setPiece(1, 1, 1, 0);
                activeBoard.ActivePieces[2] = new Piece();
                activeBoard.ActivePieces[2].setPiece(1, 1, 0, 1);
                activeBoard.ActivePieces[3] = new Piece();
                activeBoard.ActivePieces[3].setPiece(1, 1, 0, 0);
                activeBoard.ActivePieces[4] = new Piece();
                activeBoard.ActivePieces[4].setPiece(1, 0, 1, 1);
                activeBoard.ActivePieces[5] = new Piece();
                activeBoard.ActivePieces[5].setPiece(1, 0, 1, 0);
                activeBoard.ActivePieces[6] = new Piece();
                activeBoard.ActivePieces[6].setPiece(1, 0, 0, 1);
                activeBoard.ActivePieces[7] = new Piece();
                activeBoard.ActivePieces[7].setPiece(1, 0, 0, 0);
                activeBoard.ActivePieces[8] = new Piece();
                activeBoard.ActivePieces[8].setPiece(0, 1, 1, 1);
                activeBoard.ActivePieces[9] = new Piece();
                activeBoard.ActivePieces[9].setPiece(0, 1, 1, 0);
                activeBoard.ActivePieces[10] = new Piece();
                activeBoard.ActivePieces[10].setPiece(0, 1, 0, 1);
                activeBoard.ActivePieces[11] = new Piece();
                activeBoard.ActivePieces[11].setPiece(0, 1, 0, 0);
                activeBoard.ActivePieces[12] = new Piece();
                activeBoard.ActivePieces[12].setPiece(0, 0, 1, 1);
                activeBoard.ActivePieces[13] = new Piece();
                activeBoard.ActivePieces[13].setPiece(0, 0, 1, 0);
                activeBoard.ActivePieces[14] = new Piece();
                activeBoard.ActivePieces[14].setPiece(0, 0, 0, 1);
                activeBoard.ActivePieces[15] = new Piece();
                activeBoard.ActivePieces[15].setPiece(0, 0, 0, 0);

                //eldöntjük hogy ki kezd
                Random random = new Random();
                activeBoard.ActivePlayerIndex = random.Next(0, 1000) % 2;
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
        public static void selectPiece(PlayerType type, Piece select)
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
                    }
                    catch (Exception)
                    {
                        throw new Exception("Hiba történt");
                    }
                }
            }

        }
        

        private static void startGameProcess(object sender, DoWorkEventArgs e)
        {
            //egy külön szálként indítjuk el a játékot ez maga a háttérfolyamat
            initGame();
            //addig mig a nem nyert valaki vagy nem raktak le minden bábút
            while (activeBoard.Winstate == false || !activeBoard.checkIsFull())
            {
                if ((activeBoard.Player[0].PlayerType == PlayerType.PlayerOne || activeBoard.Player[0].PlayerType == PlayerType.PlayerTwo) && (activeBoard.Player[1].PlayerType == PlayerType.PlayerOne || activeBoard.Player[1].PlayerType == PlayerType.PlayerTwo))
                {
                   
                        //GameBase.AbstractGame.StepHandler step = new AbstractGame.StepHandler(activeBoard.Player[activeBoard.ActivePlayerIndex].Callback);
                        activeBoard.Player[activeBoard.ActivePlayerIndex].Callback((IState)activeBoard);
                       
                   
                    //várunk a játékosokra
                            Thread.Sleep(1000);
                        
                 
                }
                // várunk a játékosok regisztrációjára
                Thread.Sleep(1000);

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
                if (activeBoard.Player[index].PlayerType != PlayerType.PlayerOne && activeBoard.Player[index].PlayerType != PlayerType.PlayerTwo)
                {
                    
                    activeBoard.Player[index].PlayerType = playerType;
               
                    activeBoard.Player[index].Callback = onStep;
                }

        }
        
        public override IState SimulateStep(IState current, AbstractStep step)
        {
            Board returnBoard = ((Board)current);
            returnBoard.insertPiece(((QuartoStep)step).X, ((QuartoStep)step).Y, ((QuartoStep)step).P);

            return (IState)returnBoard;
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



                return AbstractStep.Result.Success;
            }
            else
            {
                return AbstractStep.Result.Failure;
            }
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
                return null;
            }
        }

       
    }

}
