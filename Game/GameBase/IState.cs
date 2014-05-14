using System.Collections.Generic;
using Game.GameBase;

namespace Game.GameBase
{
    public interface IState
    {
        IEnumerable<AbstractStep> GetAvailableSteps();
    }
}
