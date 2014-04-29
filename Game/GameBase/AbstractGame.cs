namespace Game.GameBase
{
    public abstract class AbstractGame
    {
        public abstract string GetGameTypeInfo();
        public delegate void StepHandler();
        public abstract void RegisterAsPlayer(ref StepHandler onStep, PlayerType playerType);
        public abstract void DoStep(AbstractStep step, PlayerType playerType);
        public abstract AbstractGame SimulateStep(AbstractStep step);
    }
}