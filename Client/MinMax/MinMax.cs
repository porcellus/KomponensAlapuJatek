using System;
using MinMax;

namespace Client.MinMax
{
    public class MinimaxSearch<TState, TAction, TPlayer> : IAIAlgorithm<TState, TAction>
    {
        private IGame<TState, TAction, TPlayer> _Game;
        private int _ExpandedNodes;

        /** Creates a new search object for a given game. */
        public MinimaxSearch(IGame<TState, TAction, TPlayer> game)
        {
            this._Game = game;
        }

        public TAction MakeDecision(TState state)
        {
            _ExpandedNodes = 0;
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
            _ExpandedNodes++;
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
            _ExpandedNodes++;
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
    }
}
