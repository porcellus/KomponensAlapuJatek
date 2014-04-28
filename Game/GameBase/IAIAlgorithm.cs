namespace Game.GameBase
{
    public interface IAIAlgorithm
    {
        string GetAIAlgorithmInfo();
        void AddToGame(AbstractGame game, PlayerType playerType);
    }
}