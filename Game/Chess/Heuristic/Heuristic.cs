using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Chess;

namespace Game.Chess.Heuristic
{
    public interface IHeuristic<TBoard>
    {
        public int              GetValue(TBoard board);
    }
}
