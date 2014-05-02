namespace Game.GameBase
{
    public abstract class AbstractGame
    {
        public abstract string GetGameTypeInfo();
        public delegate void StepHandler();
        public abstract void RegisterAsPlayer<TAlgorithm>(ref StepHandler onStep, PlayerType playerType, EntityType entityType, TAlgorithm algorithm);
        public abstract void SetHeuristic<TBoard>(IHeuristic<TBoard> heuristic);
        public abstract AbstractStep.Result DoStep(AbstractStep step, PlayerType playerType);
        public abstract int SimulateStep(AbstractStep step);
        public abstract void StartGame();
    }
}