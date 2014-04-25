using System;
using MinMax;

namespace Client.AlfaBeta
{
    public class AlphaBetaSearch<TState, TAction, TPlayer> : IAIAlgorithm<TState, TAction>
    {
        private readonly IGame<TState, TAction, TPlayer> _game;
        private int _expandedNodes;


        public AlphaBetaSearch(IGame<TState, TAction, TPlayer> game)
        {
            this._game = game;
        }

        public TAction MakeDecision(TState state)
        {
            _expandedNodes = 0;
            TAction result = default(TAction);
            double resultValue = Double.NegativeInfinity;
            TPlayer player = _game.GetPlayer(state);
            foreach (TAction action in _game.GetActions(state))
            {
                double value = MinValue(_game.GetResult(state, action), player,
                    Double.NegativeInfinity, Double.PositiveInfinity);
                if (value > resultValue)
                {
                    result = action;
                    resultValue = value;
                }
            }
            return result;
        }

        public double MaxValue(TState state, TPlayer player, double alpha, double beta)
        {
            _expandedNodes++;
            if (_game.IsTerminal(state))
                return _game.GetUtility(state, player);
            double value = Double.NegativeInfinity;
            foreach (TAction action in _game.GetActions(state))
            {
                value = Math.Max(value, MinValue(
                    _game.GetResult(state, action), player, alpha, beta));
                if (value >= beta)
                    return value;
                alpha = Math.Max(alpha, value);
            }
            return value;
        }

        public double MinValue(TState state, TPlayer player, double alpha, double beta)
        {
            _expandedNodes++;
            if (_game.IsTerminal(state))
                return _game.GetUtility(state, player);
            double value = Double.PositiveInfinity;
            foreach (TAction action in _game.GetActions(state))
            {
                value = Math.Min(value, MaxValue(
                    _game.GetResult(state, action), player, alpha, beta));
                if (value <= alpha)
                    return value;
                beta = Math.Min(beta, value);
            }
            return value;
        }
    }
}