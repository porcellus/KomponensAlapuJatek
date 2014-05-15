using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.GameBase;

namespace Game
{
    [Serializable]
    public class ChessHeuristic : IHeuristic<Board>
    {
        // Megadja, hogy mekkora az adott tábla értéke
        public int GetValue(Board board)
        {
            int value           = 0;
            int[] dimension     = board.getDimension();

            for (int i = 1; i <= dimension[1]; ++i)
                for (int j = 1; j <= dimension[0]; ++j)
                {
                    Figure figure = board.getFigureAt(i, j);

                    if (figure == null || figure.getFigureType() == Figure.FigureType.Nothing)
                        continue;

                    int modifier    = figure.isWhite() ? 1 : -1;
                    int fvalue      = GetFigureValue(figure.getFigureType());
                    value           += fvalue * modifier;

                    foreach (int[] vec in Figure.getLegalSteps(board, figure, false))
                    {
                        if (vec[2] == 1)        value += 1 * modifier;
                        else if (vec[2] == 2)   value += GetFigureValue(board.getFigureAt(vec[0], vec[1]).getFigureType()) * modifier * 6 + modifier * 14;
                        else if (vec[2] == 3)   value += 100 * modifier;
                    }
                }

            return value;
        }

        // Megadja, hogy az adott figurának mekkora az értéke
        public int GetFigureValue(Figure.FigureType type)
        {
            switch (type)
            {
                case Figure.FigureType.King:
                    return 0;
                case Figure.FigureType.Queen:
                    return 22;
                case Figure.FigureType.Rook:
                    return 13;
                case Figure.FigureType.Bishop:
                    return 8;
                case Figure.FigureType.Knight:
                    return 7;
                case Figure.FigureType.Pawn:
                    return 4;
            }
            return 0;
        }
    }
}
