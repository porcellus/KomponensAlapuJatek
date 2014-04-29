using System;
using Client.AIAlgorithmBase;
using QuickGraph;

namespace Client.MinMax
{
    public class MinimaxSearch<TState, TAction, TPlayer> : IAIAlgorithm<TState, TAction>
    {
        private IGameExperimental<TState, TAction, TPlayer> _Game;
        private AdjacencyGraph<int, TaggedEdge<int, string>> _Graph;

        public MinimaxSearch(IGameExperimental<TState, TAction, TPlayer> game)
        {
            _Game = game;
        }

        public TAction MakeDecision(TState state)
        {
            TAction result = default(TAction);
            double resultValue = Double.NegativeInfinity;
            TPlayer player = _Game.GetPlayer(state);

            foreach (TAction action in _Game.GetActions(state))
            {
                double value = MinValue(_Game.GetResult(state, action), player);
                if (value > resultValue)
                {
                    result = action;
                    resultValue = value;
                }
            }
            return result;
        }

        private double MaxValue(TState state, TPlayer player)
        {
            if (_Game.IsTerminal(state))
            {
                return _Game.GetUtility(state, player);
            }

            double value = Double.NegativeInfinity;
            foreach (TAction action in _Game.GetActions(state))
            {
                value = Math.Max(value, MinValue(_Game.GetResult(state, action), player));
            }
            return value;
        }

        private double MinValue(TState state, TPlayer player)
        {
            if (_Game.IsTerminal(state))
            {
                return _Game.GetUtility(state, player);
            }

            double value = Double.PositiveInfinity;
            foreach (TAction action in _Game.GetActions(state))
            {
                value = Math.Min(value, MaxValue(_Game.GetResult(state, action), player));
            }
            return value;
        }

        public string GetAIAlgorithmInfo()
        {
            return
                "Minimax is a decision rule used in decision theory, game theory, " +
                "statistics and philosophy for minimizing the possible loss for a worst " +
                "case (maximum loss) scenario.";
        }

        public void AddToGame(object game, object aiPosition)
        {
            throw new NotImplementedException();
        }
    }
}
