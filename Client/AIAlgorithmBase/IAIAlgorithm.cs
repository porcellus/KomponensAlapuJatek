namespace Client.AIAlgorithmBase
{
    public interface IAIAlgorithm<TState, TAction>
    {
        string GetAIAlgorithmInfo();
        void AddToGame(object game, object aiPosition);
    }
}
