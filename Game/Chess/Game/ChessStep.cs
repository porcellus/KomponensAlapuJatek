using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.GameBase;

namespace Game
{
    public class ChessStep : AbstractStep
    {
        protected int               fromRow, fromCol;
        protected int               toRow, toCol;
        protected Figure.StepType   type;

        public ChessStep(int fr, int fc, int tr, int tc, Figure.StepType t)
        {
            fromRow     = fr;
            fromCol     = fc;
            toRow       = tr;
            toCol       = tc;
            type        = t;
        }

        public Figure.StepType GetStepType()
        {
            return type;
        }

        public int[] GetFromPosition()
        {
            return new int[] { fromRow, fromCol };
        }

        public int[] GetToPosition()
        {
            return new int[] { toRow, toCol };
        }

    }
}
