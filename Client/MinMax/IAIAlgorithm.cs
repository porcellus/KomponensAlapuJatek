namespace MinMax
{
    /**
     * Variant of the search interface. Since players can only control the next
     * move, method <code>makeDecision</code> returns only one action, not a
     * sequence of actions.
     */
    public interface IAIAlgorithm<TState, TAction>
    {
        /** Returns the action which appears to be the best at the given state. */
        TAction MakeDecision(TState state);
    }
}
