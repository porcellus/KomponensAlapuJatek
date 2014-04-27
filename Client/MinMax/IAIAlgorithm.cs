namespace MinMax
{
    /**
     * Variant of the search interface. Since players can only control the next
     * move, method <code>makeDecision</code> returns only one action, not a
     * sequence of actions.
     */
    public interface IAIAlgorithm<TState, TAction>
    {
        string GetAIAlgorithmInfo();
        void AddToGame(object game, object aiPosition);
    }
}
