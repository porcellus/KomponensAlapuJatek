using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using Game.GameBase;



namespace Game
{
    public class Board : IState
    {
        private const int bWidth = 4;
        private const int bHeight = 4;
        private PlayerType currentPlayer;
        private  QuartoHeuristic heuristic = new QuartoHeuristic();

        public  QuartoHeuristic Heuristic
        {
            get { return heuristic; }
            set { heuristic = value; }
        }
        private Player[] player = new Player[2];

        public  Player[] Player
        {
            get { return player; }
            set { player = value; }
        }
        private  int activePlayerIndex = 0;

        public  int ActivePlayerIndex
        {
            get { return activePlayerIndex; }
            set { activePlayerIndex = value; }
        }
        private  Piece[] activePieces;

        public  Piece[] ActivePieces
        {
            get { return activePieces; }
            set { activePieces = value; }
        }
        private Piece selectedPiece;

        public Piece SelectedPiece
        {
            get { return selectedPiece; }
            set { selectedPiece = value; }
        }

        public int BWidth
        {
            get { return bWidth; }
        }

        public int BHeight
        {
            get { return bHeight; }
        }
        public PlayerType CurrentPlayer
        {
            get { return currentPlayer; }
            set {  currentPlayer = value;}
        }
       

        bool winstate = false;

        public bool Winstate
        {
            get { return winstate; }
            set { winstate = value; }
        }

        private Piece[,] bBoard= new Piece[bWidth,bHeight] ; //4*4 tábla létrehozása

        public Piece[,] BBoard
        {
            get { return bBoard; }
            set { bBoard = value; }
        }

      

        //bábú lerakása
        public void insertPiece(int x, int y, Piece iPiece)
        {
            bBoard[x, y] = iPiece;

        }
        public bool checkIsEmpty(int x, int y)
        {
            if (bBoard[x, y].color == 2)
            {
                return true;

            }
            else
            {
                return false;
            }
        }
        public bool checkIsFull()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (bBoard[i, j] == null)
                    {
                        return false;
                    }

                }
            }
            return true;
        }
        public void checkWinningState()
        {
            int hg = 0;
            int cl = 0;
            int sp = 0;
            int fl = 0;

            //megnézzük a sorokat

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (bBoard[row, col].color != 2)
                    {
                        hg += Convert.ToInt16(bBoard[row, col].height);
                        cl += Convert.ToInt16(bBoard[row, col].color);
                        sp += Convert.ToInt16(bBoard[row, col].shape);
                        fl += Convert.ToInt16(bBoard[row, col].full);

                    }

                }
                winstate = ((hg == 4) || (cl == 4) || (sp == 4) || (fl == 4));
                hg = 0;
                cl = 0;
                sp = 0;
                fl = 0;
                if (winstate) return;

            }
            //megnézzük az oszlopokat

            for (int col = 0; col < 4; col++)
            {
                for (int row = 0; row < 4; row++)
                {
                    if (bBoard[row, col].color != 2)
                    {
                        hg += Convert.ToInt16(bBoard[row, col].height);
                        cl += Convert.ToInt16(bBoard[row, col].color);
                        sp += Convert.ToInt16(bBoard[row, col].shape);
                        fl += Convert.ToInt16(bBoard[row, col].full);

                    }

                }
                winstate = ((hg == 4) || (cl == 4) || (sp == 4) || (fl == 4));
                hg = 0;
                cl = 0;
                sp = 0;
                fl = 0;
                if (winstate) return;

            }
            //egyik átló
            int column = 0;
            for (int row = 0; row < 4; row++)
            {
                if (bBoard[row, column].color != 2)
                {
                    hg += Convert.ToInt16(bBoard[row, column].height);
                    cl += Convert.ToInt16(bBoard[row, column].color);
                    sp += Convert.ToInt16(bBoard[row, column].shape);
                    fl += Convert.ToInt16(bBoard[row, column].full);

                }
                column++;

            }
            winstate = ((hg == 4) || (cl == 4) || (sp == 4) || (fl == 4));
            hg = 0;
            cl = 0;
            sp = 0;
            fl = 0;
            if (winstate) return;

            //másik átló
            column = 3;
            for (int row = 0; row < 4; row++)
            {
                if (bBoard[row, column].color != 2)
                {
                    hg += Convert.ToInt16(bBoard[row, column].height);
                    cl += Convert.ToInt16(bBoard[row, column].color);
                    sp += Convert.ToInt16(bBoard[row, column].shape);
                    fl += Convert.ToInt16(bBoard[row, column].full);

                }
                column--;

            }
            winstate = ((hg == 4) || (cl == 4) || (sp == 4) || (fl == 4));
            hg = 0;
            cl = 0;
            sp = 0;
            fl = 0;
            if (winstate) return;

            //2x2 es négyzetek ellenőrzése

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if ((bBoard[row, col].color != 2) && (bBoard[row + 1, col].color != 2) && (bBoard[row, col + 1].color != 2) && (bBoard[row + 1, col + 1].color != 2))
                    {
                        hg += Convert.ToInt16(bBoard[row, col].height) + Convert.ToInt16(bBoard[row + 1, col].height) + Convert.ToInt16(bBoard[row, col + 1].height) + Convert.ToInt16(bBoard[row + 1, col + 1].height);
                        cl += Convert.ToInt16(bBoard[row, col].color) + Convert.ToInt16(bBoard[row + 1, col].color) + Convert.ToInt16(bBoard[row, col + 1].color) + Convert.ToInt16(bBoard[row + 1, col + 1].color);
                        sp += Convert.ToInt16(bBoard[row, col].shape) + Convert.ToInt16(bBoard[row + 1, col].shape) + Convert.ToInt16(bBoard[row, col + 1].shape) + Convert.ToInt16(bBoard[row + 1, col + 1].shape);
                        cl += Convert.ToInt16(bBoard[row, col].full) + Convert.ToInt16(bBoard[row + 1, col].full) + Convert.ToInt16(bBoard[row, col + 1].full) + Convert.ToInt16(bBoard[row + 1, col + 1].full);

                    }
                    winstate = ((hg == 4) || (cl == 4) || (sp == 4) || (fl == 4));
                    hg = 0;
                    cl = 0;
                    sp = 0;
                    fl = 0;
                    if (winstate) return;
                }


            }

        }


       
    }
}
