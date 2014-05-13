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



                ret += calculateRow(board);
                ret += calculateCol(board);
                ret += calculateCub(board);
                ret += calculateDiag(board);




                return ret;
            }
            else
            {
                return 100;

            }

        }


        public int calculateRow(Board b)
        {
            int sum = 0;
            int[] rowsColor = new int[4] { 2, 2, 2, 2 };
            int[] rowsHeigh = new int[4] { 2, 2, 2, 2 };
            int[] rowsShape = new int[4] { 2, 2, 2, 2 };
            int[] rowsFull = new int[4] { 2, 2, 2, 2 };

            for (int i = 0; i < b.BHeight; ++i)
            {
                for (int j = 0; j < b.BWidth; i++)
                {


                    rowsColor[i] = b.BBoard[i, j].color;
                    rowsHeigh[i] = b.BBoard[i, j].height;
                    rowsShape[i] = b.BBoard[i, j].shape;
                    rowsFull[i] = b.BBoard[i, j].full;


                }
                int countElement = (rowsColor).Count(s => s != 2);
                if (rowsColor.Count(s => s == 1) == countElement)
                {
                    if (countElement == 1)
                    {
                        sum += 1;
                    }
                    else if (countElement == 2)
                    {
                        sum += 4;
                    }
                    else if (countElement == 3)
                    {
                        sum += 10;
                    }
                    else if (countElement == 4 )
                    {
                        sum += 100;
                    }
                }
                countElement = (rowsHeigh).Count(s => s != 2);
                if (rowsHeigh.Count(s => s == 1) == countElement)
                {
                    if (countElement == 1)
                    {
                        sum += 1;
                    }
                    else if (countElement == 2)
                    {
                        sum += 4;
                    }
                    else if (countElement == 3)
                    {
                        sum += 10;
                    }
                    else if (countElement == 4 )
                    {
                        sum += 100;
                    }
                }
                countElement = (rowsShape).Count(s => s != 2);
                if (rowsShape.Count(s => s == 1) == countElement)
                {
                    if (countElement == 1)
                    {
                        sum += 1;
                    }
                    else if (countElement == 2)
                    {
                        sum += 4;
                    }
                    else if (countElement == 3)
                    {
                        sum += 10;
                    }
                    else if (countElement == 4 )
                    {
                        sum += 100;
                    }
                }
                countElement = (rowsFull).Count(s => s != 2);
                if (rowsFull.Count(s => s == 1) == countElement)
                {
                    if (countElement == 1)
                    {
                        sum += 1;
                    }
                    else if (countElement == 2)
                    {
                        sum += 4;
                    }
                    else if (countElement == 3)
                    {
                        sum += 10;
                    }
                    else if (countElement == 4 )
                    {
                        sum += 100;
                    }
                }
            }




            return sum;
        }
        public int calculateCol(Board b)
        {
            int sum = 0;

            int[] rowsColor = new int[4] { 2, 2, 2, 2 };
            int[] rowsHeigh = new int[4] { 2, 2, 2, 2 };
            int[] rowsShape = new int[4] { 2, 2, 2, 2 };
            int[] rowsFull = new int[4] { 2, 2, 2, 2 };

            for (int i = 0; i < b.BHeight; i++)
            {
                for (int j = 0; j < b.BWidth; i++)
                {
                    if (b.BBoard[j, i] != null)
                    {
                        rowsColor[i] = b.BBoard[j, i].color;
                        rowsHeigh[i] = b.BBoard[j, i].height;
                        rowsShape[i] = b.BBoard[j, i].shape;
                        rowsFull[i] = b.BBoard[j, i].full;
                    }
                }

                int countElement = (rowsColor).Count(s => s != 2);
                if (rowsColor.Count(s => s == 1) == countElement)
                {
                    if (countElement == 1)
                    {
                        sum += 1;
                    }
                    else if (countElement == 2)
                    {
                        sum += 4;
                    }
                    else if (countElement == 3)
                    {
                        sum += 10;
                    }
                    else if (countElement == 4 )
                    {
                        sum += 100;
                    }
                }
                countElement = (rowsHeigh).Count(s => s != 2);
                if (rowsHeigh.Count(s => s == 1) == countElement)
                {
                    if (countElement == 1)
                    {
                        sum += 1;
                    }
                    else if (countElement == 2)
                    {
                        sum += 4;
                    }
                    else if (countElement == 3)
                    {
                        sum += 10;
                    }
                    else if (countElement == 4 )
                    {
                        sum += 100;
                    }
                }
                countElement = (rowsShape).Count(s => s != 2);
                if (rowsShape.Count(s => s == 1) == countElement)
                {
                    if (countElement == 1)
                    {
                        sum += 1;
                    }
                    else if (countElement == 2)
                    {
                        sum += 4;
                    }
                    else if (countElement == 3)
                    {
                        sum += 10;
                    }
                    else if (countElement == 4 )
                    {
                        sum += 100;
                    }
                }
                countElement = (rowsFull).Count(s => s != 2);
                if (rowsFull.Count(s => s == 1) == countElement)
                {
                    if (countElement == 1)
                    {
                        sum += 1;
                    }
                    else if (countElement == 2)
                    {
                        sum += 4;
                    }
                    else if (countElement == 3)
                    {
                        sum += 10;
                    }
                    else if (countElement == 4 )
                    {
                        sum += 100;
                    }
                }

            }




            return sum;


        }
        public int calculateCub(Board b)
        {
            int sum = 0;
            int[] rowsColor = new int[4] { 2, 2, 2, 2 };
            int[] rowsHeigh = new int[4] { 2, 2, 2, 2 };
            int[] rowsShape = new int[4] { 2, 2, 2, 2 };
            int[] rowsFull = new int[4] { 2, 2, 2, 2 };

            for (int i = 0; i < b.BHeight - 1; i++)
            {
                if (b.BBoard[i, i] != null)
                {
                    rowsColor[0] = b.BBoard[i, i].color;
                    rowsHeigh[0] = b.BBoard[i, i].height;
                    rowsShape[0] = b.BBoard[i, i].shape;
                    rowsFull[0] = b.BBoard[i, i].full;

                }
                if (b.BBoard[i, i + 1] != null)
                {
                    rowsColor[1] = b.BBoard[i, i + 1].color;
                    rowsHeigh[1] = b.BBoard[i, i + 1].height;
                    rowsShape[1] = b.BBoard[i, i + 1].shape;
                    rowsFull[1] = b.BBoard[i, i + 1].full;
                }
                if (b.BBoard[i + 1, i + 1] != null)
                {
                    rowsColor[2] = b.BBoard[i + 1, i + 1].color;
                    rowsHeigh[2] = b.BBoard[i + 1, i + 1].height;
                    rowsShape[2] = b.BBoard[i + 1, i + 1].shape;
                    rowsFull[2] = b.BBoard[i + 1, i + 1].full;
                }
                if (b.BBoard[i + 1, i] != null)
                {
                    rowsColor[3] = b.BBoard[i + 1, i].color;
                    rowsHeigh[3] = b.BBoard[i + 1, i].height;
                    rowsShape[3] = b.BBoard[i + 1, i].shape;
                    rowsFull[3] = b.BBoard[i + 1, i].full;
                }

                int countElement = (rowsColor).Count(s => s != 2);
                if (rowsColor.Count(s => s == 1) == countElement)
                {
                    if (countElement == 1)
                    {
                        sum += 1;
                    }
                    else if (countElement == 2)
                    {
                        sum += 4;
                    }
                    else if (countElement == 3)
                    {
                        sum += 6;
                    }
                    else if (countElement == 4 )
                    {
                        sum += 100;
                    }
                }
                countElement = (rowsHeigh).Count(s => s != 2);
                if (rowsHeigh.Count(s => s == 1) == countElement)
                {
                    if (countElement == 1)
                    {
                        sum += 1;
                    }
                    else if (countElement == 2)
                    {
                        sum += 4;
                    }
                    else if (countElement == 3)
                    {
                        sum += 6;
                    }
                    else if (countElement == 4 )
                    {
                        sum += 100;
                    }
                }
                countElement = (rowsShape).Count(s => s != 2);
                if (rowsShape.Count(s => s == 1) == countElement)
                {
                    if (countElement == 1)
                    {
                        sum += 1;
                    }
                    else if (countElement == 2)
                    {
                        sum += 4;
                    }
                    else if (countElement == 3)
                    {
                        sum += 6;
                    }
                    else if (countElement == 4 )
                    {
                        sum += 100;
                    }
                }
                countElement = (rowsFull).Count(s => s != 2);
                if (rowsFull.Count(s => s == 1) == countElement)
                {
                    if (countElement == 1)
                    {
                        sum += 1;
                    }
                    else if (countElement == 2)
                    {
                        sum += 4;
                    }
                    else if (countElement == 3)
                    {
                        sum += 6;
                    }
                    else if (countElement == 4 )
                    {
                        sum += 100;
                    }
                }

            }

            return sum;
        }
        public int calculateDiag(Board b)
        {

            int sum = 0;
            int[] rowsColor = new int[4] { 2, 2, 2, 2 };
            int[] rowsHeigh = new int[4] { 2, 2, 2, 2 };
            int[] rowsShape = new int[4] { 2, 2, 2, 2 };
            int[] rowsFull = new int[4] { 2, 2, 2, 2 };

            for (int i = 0; i < b.BHeight; i++)
            {
                for (int j = 0; j < b.BWidth; i++)
                {
                    if (i == j)
                    {
                        if (b.BBoard[i, j] != null)
                        {
                            rowsColor[i] = b.BBoard[i, j].color;
                            rowsHeigh[i] = b.BBoard[i, j].height;
                            rowsShape[i] = b.BBoard[i, j].shape;
                            rowsFull[i] = b.BBoard[i, j].full;
                        }
                    }
                }
                int countElement = (rowsColor).Count(s => s != 2);
                if (rowsColor.Count(s => s == 1) == countElement)
                {
                    if (countElement == 1)
                    {
                        sum += 1;
                    }
                    else if (countElement == 2)
                    {
                        sum += 4;
                    }
                    else if (countElement == 3)
                    {
                        sum += 10;
                    }
                    else if (countElement == 4 )
                    {
                        sum += 100;
                    }
                }
                countElement = (rowsHeigh).Count(s => s != 2);
                if (rowsHeigh.Count(s => s == 1) == countElement)
                {
                    if (countElement == 1)
                    {
                        sum += 1;
                    }
                    else if (countElement == 2)
                    {
                        sum += 4;
                    }
                    else if (countElement == 3)
                    {
                        sum += 10;
                    }
                    else if (countElement == 4 )
                    {
                        sum += 100;
                    }
                }
                countElement = (rowsShape).Count(s => s != 2);
                if (rowsShape.Count(s => s == 1) == countElement)
                {
                    if (countElement == 1)
                    {
                        sum += 1;
                    }
                    else if (countElement == 2)
                    {
                        sum += 4;
                    }
                    else if (countElement == 3)
                    {
                        sum += 10;
                    }
                    else if (countElement == 4 )
                    {
                        sum += 100;
                    }
                }
                countElement = (rowsFull).Count(s => s != 2);
                if (rowsFull.Count(s => s == 1) == countElement)
                {
                    if (countElement == 1)
                    {
                        sum += 1;
                    }
                    else if (countElement == 2)
                    {
                        sum += 4;
                    }
                    else if (countElement == 3)
                    {
                        sum += 10;
                    }
                    else if (countElement == 4 )
                    {
                        sum += 100;
                    }
                }
            }

            return sum;
        }

    }
}
