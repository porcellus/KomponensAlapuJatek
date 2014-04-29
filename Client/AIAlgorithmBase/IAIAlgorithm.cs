using Game.GameBase;

namespace Client.AIAlgorithmBase
{
    public interface IAIAlgorithm
    {
        string GetAIAlgorithmInfo();
        void AddToGame(AbstractGame game, PlayerType playerType);
    }
}
