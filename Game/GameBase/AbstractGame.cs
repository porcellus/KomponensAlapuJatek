using System.Collections.Generic;
using Game.GameBase;

namespace Game.GameBase
{
    public abstract class AbstractGame
    {
        public abstract string GetGameTypeInfo();
        public delegate void StepHandler(IState state);
        public abstract void RegisterAsPlayer<TAlgorithm>(ref StepHandler onStep, PlayerType playerType, EntityType entityType, TAlgorithm algorithm);
        public abstract void SetHeuristic<TBoard>(IHeuristic<TBoard> heuristic);
        public abstract AbstractStep.Result DoStep(AbstractStep step, PlayerType playerType);
        public abstract IState SimulateStep(AbstractStep step);
        public abstract int SimulateStep(AbstractStep step, int dummyInt);
        public abstract void StartGame();
        public abstract bool IsTerminal(IState state);
        public abstract double GetHeuristicValue(IState state);
        public abstract IState GetNextState(IState current, AbstractStep step);
    }
} 