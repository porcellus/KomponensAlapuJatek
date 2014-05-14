using System.Collections.Generic;
using Game.GameBase;

namespace Game.GameBase
{
    public interface IState
    {
        public IEnumerable<AbstractStep> GetAvailableSteps();
    }
}
