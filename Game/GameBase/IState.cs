using System.Collections.Generic;
using Game.GameBase;

namespace GameBase
{
    public interface IState
    {
        IEnumerable<AbstractStep> GetAvailableSteps();
    }
}
