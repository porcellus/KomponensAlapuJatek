using System;
using MinMax;

namespace Client.AlfaBeta
{
    public class AlphaBetaSearch<TState, TAction, TPlayer> : IAIAlgorithm<TState, TAction>
    {
        private readonly IGame<TState, TAction, TPlayer> game;
        private int expandedNodes;


        public AlphaBetaSearch(IGame<TState, TAction, TPlayer> game)
        {
            this.game = game;
        }

        public TAction MakeDecision(TState state)
        {
            expandedNodes = 0;
            TAction result = default(TAction);
            double resultValue = Double.NegativeInfinity;
            TPlayer player = game.GetPlayer(state);
            foreach (TAction action in game.GetActions(state))
            {
                double value = minValue(game.GetResult(state, action), player,
                    Double.NegativeInfinity, Double.PositiveInfinity);
                if (value > resultValue)
                {
                    result = action;
                    resultValue = value;
                }
            }
            return result;
        }

        public double maxValue(TState state, TPlayer player, double alpha, double beta)
        {
            expandedNodes++;
            if (game.IsTerminal(state))
                return game.GetUtility(state, player);
            double value = Double.NegativeInfinity;
            foreach (TAction action in game.GetActions(state))
            {
                value = Math.Max(value, minValue(
                    game.GetResult(state, action), player, alpha, beta));
                if (value >= beta)
                    return value;
                alpha = Math.Max(alpha, value);
            }
            return value;
        }

        public double minValue(TState state, TPlayer player, double alpha, double beta)
        {
            expandedNodes++;
            if (game.IsTerminal(state))
                return game.GetUtility(state, player);
            double value = Double.PositiveInfinity;
            foreach (TAction action in game.GetActions(state))
            {
                value = Math.Min(value, maxValue(
                    game.GetResult(state, action), player, alpha, beta));
                if (value <= alpha)
                    return value;
                beta = Math.Min(beta, value);
            }
            return value;
        }
    }
}