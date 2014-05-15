using System.Collections.Generic;
using Game.GameBase;

namespace Game.GameBase
{
    public abstract class AbstractGame
    {
        public abstract string GetGameTypeInfo();
        public delegate void StepHandler(IState state);
        public abstract void RegisterAsPlayer(ref StepHandler onStep, PlayerType playerType);

        // public abstract void SetHeuristic<TBoard>(IHeuristic<TBoard> heuristic);
        // return type lehetne void
        public abstract AbstractStep.Result DoStep(AbstractStep step, PlayerType playerType);

        //public abstract AbstractGame SimulateStep(AbstractStep step);
        // use  state.GetAvailableSteps() instead
        //public abstract bool IsTerminal(IState state);

        // return the heuristic value of the state according to the given playerType
        public abstract double GetHeuristicValue(IState state, PlayerType playerType);

        public abstract IState SimulateStep(IState current, AbstractStep step);
        public abstract IEnumerable<AbstractStep> GetAvailableSteps(IState state);
    }
} 