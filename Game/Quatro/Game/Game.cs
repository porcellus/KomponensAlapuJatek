using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;


namespace Game.Quatro.Game
{
    public class Game
    {
        static Board activeBoard;
        //actuális pálya lekérdezése
        internal static Board ActiveBoard
        {
            get { return Game.activeBoard; }

        }
        static Player[] player;
        //játékos tömb lekérdezése
        internal static Player[] Player
        {
            get { return Game.player; }

        }
        static Piece[] activePieces;

        //a bábuk tömbjének lekérdése
        internal static Piece[] ActivePieces
        {
            get { return Game.activePieces; }

        }
        static int activePlayer;
        // az aktuális játékos indexe a Player tömbbne
        public static int ActivePlayer
        {
            get { return Game.activePlayer; }

        }
        // az ellenfél számára választott bábú
        static Piece selectedPiece;

        static BackgroundWorker gameThread = new BackgroundWorker();
        internal static void initGame(string playerName)
        {
            // ezzel a függvénnyel inicializáljuk és indítjuk a játékot

            activeBoard = new Board();
            player = new Player[2];
            player[0] = new Player();
            player[0].PlayerName = playerName;
            player[1] = new Player();
            player[1].PlayerName = "AI";

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
            startGame();

        }
        //az elenfél által választott bábut lerakjuk pálya x y korrdinátára 0 tól indexelve
        internal static void updateGameState(int x, int y)
        {
            try
            {
                if (activeBoard.checkIsEmpty(x, y))
                {
                    activeBoard.insertPiece(x, y, selectedPiece);

                    selectedPiece = null;
                    activeBoard.checkWinningState();
                }

            }
            catch (Exception)
            { }
        }
        internal static void selectPiece(int activePiacesIndex)
        {
            //kiválasztjuk a bábút amit az ellenfélnek le kell tenni majd kivesszük a bábúk tömbbjéből
            try
            {
                selectedPiece = activePieces[activePiacesIndex];
                activePieces = activePieces.Where(w => w != selectedPiece).ToArray();
                activePlayer = (activePlayer + 1) % 2;
            }
            catch (Exception)
            { }

        }
        private static void AISelectPiece()
        {
            int hg0 = 0;
            int hg1 = 0;
            int cl0 = 0;
            int cl1 = 0;
            int sp0 = 0;
            int sp1 = 0;
            int fl0 = 0;
            int fl1 = 0;
            
            for (int i = 0; i < activePieces.Length; i++)
            {
                if (Convert.ToInt16(activePieces[i].height) == 0)
                {
                    hg0++;
                }
                else
                {
                    hg1++;
                }
                if (Convert.ToInt16(activePieces[i].color) == 0)
                {
                    cl0++;
                }
                else
                {
                    cl1++;
                }
                if (Convert.ToInt16(activePieces[i].shape) == 0)
                {
                    sp0++;
                }
                else
                {
                    sp1++;
                }
                if (Convert.ToInt16(activePieces[i].full) == 0)
                {
                    fl0++;
                }
                else
                {
                    fl1++;
                }

            }
            Piece[] selectedPiecearray = activePieces;

            if (hg0 > hg1)
            {
                selectedPiecearray = selectedPiecearray.Where(w => w.height == false).ToArray();
            }
            else if (hg0 < hg1)
            {
                selectedPiecearray = selectedPiecearray.Where(w => w.height == true).ToArray();
            
            }
            if (cl0 > cl1)
            {


                if (selectedPiecearray.Where(w => w.color == false).ToArray().Length > 0)
                {
                    selectedPiecearray = selectedPiecearray.Where(w => w.color == false).ToArray();
                }
                else if (selectedPiecearray.Where(w => w.color == true).ToArray().Length > 0)
                {
                    selectedPiecearray = selectedPiecearray.Where(w => w.color == true).ToArray();

                }
            }
            else if (cl0 < cl1)
            {
                if (selectedPiecearray.Where(w => w.color == true).ToArray().Length > 0)
                {
                    selectedPiecearray = selectedPiecearray.Where(w => w.color == true).ToArray();
                }
                else if (selectedPiecearray.Where(w => w.color == false).ToArray().Length > 0)
                {
                    selectedPiecearray = selectedPiecearray.Where(w => w.color == false).ToArray();

                }
            }
            if (sp0 > sp1)
            {
                if (selectedPiecearray.Where(w => w.shape == false).ToArray().Length > 0)
                {
                    selectedPiecearray = selectedPiecearray.Where(w => w.shape == false).ToArray();
                }
                else if (selectedPiecearray.Where(w => w.shape == true).ToArray().Length > 0)
                {
                    selectedPiecearray = selectedPiecearray.Where(w => w.shape == true).ToArray();

                }
            }
            else if(sp0 < sp1)
            {
                if (selectedPiecearray.Where(w => w.shape == true).ToArray().Length > 0)
                {
                    selectedPiecearray = selectedPiecearray.Where(w => w.shape == true).ToArray();
                }
                else if (selectedPiecearray.Where(w => w.shape == false).ToArray().Length > 0)
                {
                    selectedPiecearray = selectedPiecearray.Where(w => w.shape == false).ToArray();

                }
            }
            if (fl0 > fl1)
            {
                if (selectedPiecearray.Where(w => w.full == false).ToArray().Length > 0)
                {
                    selectedPiecearray = selectedPiecearray.Where(w => w.full == false).ToArray();
                }
                else if (selectedPiecearray.Where(w => w.full == true).ToArray().Length > 0)
                {
                    selectedPiecearray = selectedPiecearray.Where(w => w.full == true).ToArray();

                }
            }
            else if(fl0 < fl1)
            {
                if (selectedPiecearray.Where(w => w.full == true).ToArray().Length > 0)
                {
                    selectedPiecearray = selectedPiecearray.Where(w => w.full == true).ToArray();
                }
                else if (selectedPiecearray.Where(w => w.full == false).ToArray().Length > 0)
                {
                    selectedPiecearray = selectedPiecearray.Where(w => w.full == false).ToArray();

                }
            }
           
            for(int i=0; i< activePieces.Length;i++ )
            {
                if (activePieces[i] == selectedPiecearray[0])
                {
                    selectPiece(i);
                    return;
                }
            }



            


        
        }
        private static void startGameProcess(object sender, DoWorkEventArgs e)
        {
            //egy külön szálként indítjuk el a játékot ez maga a háttérfolyamat

            //addig mig a nem nyert valaki vagy nem raktak le minden bábút
            while (activeBoard.Winstate == false || !activeBoard.checkIsFull())
            {


                if (selectedPiece == null)
                {
                    if (activePlayer == 0)
                    {

                        Thread.Sleep(1000);
                        ///megvárjuk míg a játékos lerak egy bábut

                    }
                    else
                    {

                        Thread.Sleep(1000);
                    }
                }
                if (selectedPiece != null)
                {
                    if (activePlayer == 0)
                    {


                        Thread.Sleep(1000);
                        //várunk míg a játékos lerak egy bábut


                    }
                    else
                    {
                        // az AI lerakja a bábút
                        Thread.Sleep(1000);

                    }


                }
            }
        }


        public static void startGame()
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





    }
}
