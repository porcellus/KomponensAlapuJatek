using System.Collections.Generic;

namespace Client.AIAlgorithmBase
{
    /**
     * @param <TState>
     *            Type which is used for states in the game.
     * @param <TAction>
     *            Type which is used for actions in the game.
     * @param <TPlayer>
     *            Type which is used for players in the game.
     */
    public interface IGameExperimental<TState, TAction, TPlayer>
    {
        TState GetInitialState();

        TPlayer[] GetPlayers();

        TPlayer GetPlayer(TState state);

        List<TAction> GetActions(TState state);

        TState GetResult(TState state, TAction action);

        bool IsTerminal(TState state);

        double GetUtility(TState state, TPlayer player);
    }
}
