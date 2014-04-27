using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Chess.Heuristic;

namespace Game
{
    class ChessHeuristic : IHeuristic<Board>
    {
        // Megadja, hogy mekkora az adott tábla értéke
        public int GetValue(Board board)
        {
            int value           = 0;
            int[] dimension     = board.getDimension();

            for (int i = 1; i <= dimension[1]; ++i)
                for (int j = 1; j <= dimension[0]; ++j)
                {
                    char figure = board.getRealTypeByRC(i, j);

                    if (figure != Board.BOARD_PIECE_NULL && figure != Board.BOARD_PIECE_EMPTY)
                    {
                        int modifier    = board.getContentColor(i, j) == '1' ? 1 : -1;
                        int ftype       = (int)figure - 48;
                        int fvalue      = GetFigureValue(ftype);
                        value           += fvalue * modifier;

                        foreach (int[] vec in Figure.getLegalSteps(board, i, j, false))
                        {
                            if (vec[2] == 1)        value += 1 * modifier;
                            else if (vec[2] == 2)   value += GetFigureValue(board.getRealTypeByRC(vec[0], vec[1]) - 48) * modifier * 6 + modifier * 14;
                            else if (vec[2] == 3)   value += 100 * modifier;
                        }
                    }
                }

            return value;
        }

        // Megadja, hogy az adott figurának mekkora az értéke
        public int GetFigureValue(int type)
        {
            switch (type)
            {
                case Figure.TYPE_KING:
                    return 0;
                case Figure.TYPE_QUEEN:
                    return 22;
                case Figure.TYPE_ROOK:
                    return 13;
                case Figure.TYPE_BISHOP:
                    return 8;
                case Figure.TYPE_KNIGHT:
                    return 7;
                case Figure.TYPE_PAWN:
                    return 4;
            }
            return 0;
        }
    }
}
