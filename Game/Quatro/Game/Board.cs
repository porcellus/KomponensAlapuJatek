﻿using System;
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

        public Board(Board oBoard)
        {
               bBoard = new Piece[4, 4];
                for (int i = 0; i < BHeight; i++)
                {
                    for (int j = 0; j < BWidth; j++)
                    {
                        bBoard[i, j] = new Piece(oBoard.BBoard[i, j].color,
                                                 oBoard.BBoard[i, j].height,
                                                 oBoard.BBoard[i, j].shape,
                                                 oBoard.BBoard[i, j].full);
                    }
                }
                ActivePieces = new Piece[oBoard.activePieces.Length];
                for (int i = 0; i < oBoard.activePieces.Length; ++i)
                {
                    ActivePieces[i] = new Piece(oBoard.ActivePieces[i].height, oBoard.ActivePieces[i].color, oBoard.ActivePieces[i].shape, oBoard.ActivePieces[i].full);
                }
                SelectedPiece = oBoard.SelectedPiece;
           
        }

        public Board()
        {
            bBoard = new Piece[bWidth,bHeight];
            for (int i = 0; i < BHeight; i++)
            {
                for (int j = 0; j < BWidth; j++)
                {
                    bBoard[i, j] = new Piece();
                }
            }
            ActivePieces = new Piece[16];
            ActivePieces[0] = new Piece(1, 1, 1, 1);
            ActivePieces[1] = new Piece(1, 1, 1, 0);
            ActivePieces[2] = new Piece(1, 1, 0, 1);
            ActivePieces[3] = new Piece(1, 1, 0, 0);
            ActivePieces[4] = new Piece(1, 0, 1, 1);
            ActivePieces[5] = new Piece(1, 0, 1, 0);
            ActivePieces[6] = new Piece(1, 0, 0, 1);
            ActivePieces[7] = new Piece(1, 0, 0, 0);
            ActivePieces[8] = new Piece(0, 1, 1, 1);
            ActivePieces[9] = new Piece(0, 1, 1, 0);
            ActivePieces[10] = new Piece(0, 1, 0, 1);
            ActivePieces[11] = new Piece(0, 1, 0, 0);
            ActivePieces[12] = new Piece(0, 0, 1, 1);
            ActivePieces[13] = new Piece(0, 0, 1, 0);
            ActivePieces[14] = new Piece(0, 0, 0, 1);
            ActivePieces[15] = new Piece(0, 0, 0, 0);
        }
        // Az elérhető bábúk módosítása, kivesz egy elemet
        public void UpdateActivePieces(Piece[] list, Piece p)
        {
            var nList = list.ToList();
            nList.RemoveAll(a => a.Equals(p));
            activePieces = nList.ToArray();
        }

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
        public void UpdateCurrentPlayer()
        {
            if (Player[0].PlayerType == CurrentPlayer)
            {
                CurrentPlayer = Player[1].PlayerType;
            }
            else
            {
                CurrentPlayer = player[0].PlayerType;
            }
        
        }
        public bool Winstate
        {
            get { return winstate; }
            set { winstate = value; }
        }

        private Piece[,] bBoard; //4*4 tábla létrehozása

        private ReadOnlyBoard rBoard;
        public ReadOnlyBoard BBoard
        {
            get { return rBoard ?? (rBoard = new ReadOnlyBoard(bBoard)); }
        }

        public class ReadOnlyBoard
        {
            
            private Piece[,] _board;
            public ReadOnlyBoard(Piece[,] board)
            {
                _board = board;
            }

            public Piece this[int i, int j]
            {
                get { return _board[i, j]; }
            }
        }
      

        //bábú lerakása
        public void insertPiece(int x, int y, Piece iPiece)
        {
            bBoard[x, y] = iPiece;
            
        }
        //az adott mező üres-e
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
        //Minden mezőre került-e bábú
        public bool checkIsFull()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (bBoard[i, j].color == 2)
                    {
                        return false;
                    }

                }
            }
            return true;
        }
        //Nyerő állapot keresésé a bBordon
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
                    else
                    {
                        hg = cl = sp = fl = -1;
                        break;
                    }

                }
                winstate = (win(hg) || win(cl) || win(sp) || win(fl));
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
                    else
                    {
                        hg = cl = sp = fl = -1;
                        break;
                    }

                }
                winstate = (win(hg) || win(cl) || win(sp) || win(fl));
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
                else
                {
                    hg = cl = sp = fl = -1;
                    break;
                }
                column++;

            }
            winstate = (win(hg) || win(cl) || win(sp) || win(fl));
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
                else
                {
                    hg = cl = sp = fl = -1;
                    break;
                }
                column--;

            }
            winstate = (win(hg) || win(cl) || win(sp) || win(fl));
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
                        fl += Convert.ToInt16(bBoard[row, col].full) + Convert.ToInt16(bBoard[row + 1, col].full) + Convert.ToInt16(bBoard[row, col + 1].full) + Convert.ToInt16(bBoard[row + 1, col + 1].full);

                    }
                    else
                    {
                        hg = cl = sp = fl = -1;
                        break;
                    }
                   
                    winstate = (win(hg) || win(cl) || win(sp) || win(fl));
                    hg = 0;
                    cl = 0;
                    sp = 0;
                    fl = 0;
                    if (winstate) return;
                }


            }

        }


        private Boolean win(int valami)
        {
            return valami == 0 || valami == 4;
        }
    }
}
