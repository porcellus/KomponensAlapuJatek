using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.GameBase
{
    public interface IHeuristic<TBoard>
    {
        int GetValue(TBoard board);
    }
}
