using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.GameBase;
using GameBase;

namespace Game
{
    class QuartoHeuristic : IHeuristic<Board>
    {

        public int GetValue(Board board)
        {
            int ret = 0;
            

            board.checkWinningState();

            if (!board.Winstate)
            {


                ret = calculate( board);


                return ret;
            }
            else
            {
                return 100;

            }

        }

        public int calculate( Board b)
        {
            int sum = 0;
            /*

            //megnézzük a sorokat

            for (int i = 0; i < b.BWidth; i++)
            {
                if (b.BBoard[i, y] != null)
                {
                    if (b.BBoard[i, y].color == p.color)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[i, y].shape == p.shape)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[i, y].full == p.full)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[i, y].height == p.height)
                    {
                        sum += 1;
                    }
                }

            }
            //megnézzük az oszlopokat
            for (int i = 0; i < b.BHeight; i++)
            {
                if (b.BBoard[x, i] != null)
                {
                    if (b.BBoard[x, i].color == p.color)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[x, i].shape == p.shape)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[x, i].full == p.full)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[x, i].height == p.height)
                    {
                        sum += 1;
                    }
                }

            }
            // négyzetek ellenőrzése
            if (x - 1 > 0)
            {
                if (b.BBoard[x - 1, y] != null)
                {
                    if (b.BBoard[x - 1, y].color == p.color)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[x - 1, y].shape == p.shape)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[x - 1, y].full == p.full)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[x - 1, y].height == p.height)
                    {
                        sum += 1;
                    }
                }
            }
            if (x + 1 < 4)
            {
                if (b.BBoard[x + 1, y] != null)
                {
                    if (b.BBoard[x + 1, y].color == p.color)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[x + 1, y].shape == p.shape)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[x + 1, y].full == p.full)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[x + 1, y].height == p.height)
                    {
                        sum += 1;
                    }
                }
            }
            if (y - 1 > 0)
            {
                if (b.BBoard[x, y - 1] != null)
                {
                    if (b.BBoard[x, y - 1].color == p.color)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[x, y - 1].shape == p.shape)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[x, y - 1].full == p.full)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[x, y - 1].height == p.height)
                    {
                        sum += 1;
                    }
                }
            }
            if (y + 1 < 4)
            {
                if (b.BBoard[x, y + 1] != null)
                {
                    if (b.BBoard[x, y + 1].color == p.color)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[x, y + 1].shape == p.shape)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[x, y + 1].full == p.full)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[x, y + 1].height == p.height)
                    {
                        sum += 1;
                    }
                }
            }
            if (y + 1 < 4 && x + 1 < 4)
            {
                if (b.BBoard[x + 1, y + 1] != null)
                {
                    if (b.BBoard[x + 1, y + 1].color == p.color)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[x + 1, y + 1].shape == p.shape)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[x + 1, y + 1].full == p.full)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[x + 1, y + 1].height == p.height)
                    {
                        sum += 1;
                    }
                }
            }
            if (y + 1 < 4 && x - 1 > 0)
            {
                if (b.BBoard[x - 1, y + 1] != null)
                {
                    if (b.BBoard[x - 1, y + 1].color == p.color)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[x - 1, y + 1].shape == p.shape)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[x - 1, y + 1].full == p.full)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[x - 1, y + 1].height == p.height)
                    {
                        sum += 1;
                    }
                }
            }
            if (y - 1 > 0 && x - 1 > 0)
            {
                if (b.BBoard[x - 1, y - 1] != null)
                {
                    if (b.BBoard[x - 1, y - 1].color == p.color)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[x - 1, y - 1].shape == p.shape)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[x - 1, y - 1].full == p.full)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[x - 1, y - 1].height == p.height)
                    {
                        sum += 1;
                    }
                }
            }
            if (y - 1 > 0 && x + 1 < 4)
            {
                if (b.BBoard[x + 1, y - 1] != null)
                {
                    if (b.BBoard[x + 1, y - 1].color == p.color)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[x + 1, y - 1].shape == p.shape)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[x + 1, y - 1].full == p.full)
                    {
                        sum += 1;
                    }
                    if (b.BBoard[x + 1, y - 1].height == p.height)
                    {
                        sum += 1;
                    }
                }
            }


            */

            return sum;
        }
    }
}
