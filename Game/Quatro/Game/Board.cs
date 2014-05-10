﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameBase;
using Game.GameBase;



namespace Game
{
    class Board : IState
    {
        private const int bWidth = 4;

        public int BWidth
        {
            get { return bWidth; }
        }

        private const int bHeight = 4;

        public int BHeight
        {
            get { return bHeight; }
        } 


        bool winstate = false;

        public bool Winstate
        {
            get { return winstate; }
            set { winstate = value; }
        }

        Piece[,] bBoard = new Piece[bWidth, bHeight]; //4*4 tábla létrehozása

        internal Piece[,] BBoard
        {
            get { return bBoard; }
           
        }

        //bábú lerakása
        public void insertPiece(int x, int y, Piece iPiece)
        {
            bBoard[x , y] = iPiece;

        }
        public bool checkIsEmpty(int x, int y)
        {
            if (bBoard[x - 1, y - 1] == null)
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
                    if (bBoard[row, col] != null)
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
                    if (bBoard[row, col] != null)
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
                if (bBoard[row, column] != null)
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
                if (bBoard[row, column] != null)
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
                    if ((bBoard[row, col] != null) && (bBoard[row + 1, col] != null) && (bBoard[row, col + 1] != null) && (bBoard[row + 1, col + 1] != null))
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
        public IState GetNextState(IState current, AbstractStep step)
        {
            Board returnBoard = ((Board)current);
            returnBoard.insertPiece(((QuartoStep)step).X, ((QuartoStep)step).Y, ((QuartoStep)step).P);

            return (IState)returnBoard;
        }
       
        public override IEnumerable<AbstractStep> GetAvailableSteps()
        {
           
          List<QuartoStep> lista = new List<QuartoStep>();

          QuartoStep step;
         
          if (Game.Quatro.Game.Quarto.SelectedPiece == null && bBoard == Game.Quatro.Game.Quarto.ActiveBoard.BBoard)
          {
              for (int i = 0; i < Game.Quatro.Game.Quarto.ActivePieces.Length; i++)
              {
                  for (int j = 0; j < 4; j++)
                  {
                      for (int k = 0; k < 4; k++)
                      {
                          if (bBoard[j, k] == null)
                          {


                              step = new QuartoStep(j, k, Game.Quatro.Game.Quarto.SelectedPiece);
                              lista.Add(step);
                          }

                      }

                  }

              }
          
          
          }
          else
              {
                  for (int i = 0; i < Game.Quatro.Game.Quarto.ActivePieces.Length; i++)
                  {
                      for (int j = 0; j < 4; j++)
                      {
                          for (int k = 0; k < 4; k++)
                          {
                              if (bBoard[j, k] == null)
                              {


                                  step = new QuartoStep(j, k, Game.Quatro.Game.Quarto.ActivePieces[i]);
                                  lista.Add(step);
                              }

                          }

                      }

                  }
              }



          return lista;
      }
    }
}
