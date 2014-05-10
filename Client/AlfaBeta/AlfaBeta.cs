using System;
using Client.AIAlgorithmBase;
using Game.GameBase;
using GameBase;

namespace Client.AlfaBeta
{
    public class AlphaBetaSearch : IAIAlgorithm
    {
        private AbstractGame game;
        private PlayerType playerType;
//        private readonly IGameExperimental<TState, TAction, TPlayer> _game;
//        private int _expandedNodes;
//
//
//        public AlphaBetaSearch(IGameExperimental<TState, TAction, TPlayer> game)
//        {
//            this._game = game;
//        }
//
//        public TAction MakeDecision(TState state)
//        {
//            _expandedNodes = 0;
//            TAction result = default(TAction);
//            double resultValue = Double.NegativeInfinity;
//            TPlayer player = _game.GetPlayer(state);
//            foreach (TAction action in _game.GetActions(state))
//            {
//                double value = MinValue(_game.GetResult(state, action), player,
//                    Double.NegativeInfinity, Double.PositiveInfinity);
//                if (value > resultValue)
//                {
//                    result = action;
//                    resultValue = value;
//                }
//            }
//            return result;
//        }
//
//        public double MaxValue(TState state, TPlayer player, double alpha, double beta)
//        {
//            _expandedNodes++;
//            if (_game.IsTerminal(state))
//                return _game.GetUtility(state, player);
//            double value = Double.NegativeInfinity;
//            foreach (TAction action in _game.GetActions(state))
//            {
//                value = Math.Max(value, MinValue(
//                    _game.GetResult(state, action), player, alpha, beta));
//                if (value >= beta)
//                    return value;
//                alpha = Math.Max(alpha, value);
//            }
//            return value;
//        }
//
//        public double MinValue(TState state, TPlayer player, double alpha, double beta)
//        {
//            _expandedNodes++;
//            if (_game.IsTerminal(state))
//                return _game.GetUtility(state, player);
//            double value = Double.PositiveInfinity;
//            foreach (TAction action in _game.GetActions(state))
//            {
//                value = Math.Min(value, MaxValue(
//                    _game.GetResult(state, action), player, alpha, beta));
//                if (value <= alpha)
//                    return value;
//                beta = Math.Min(beta, value);
//            }
//            return value;
//        }

        public string GetAIAlgorithmInfo()
        {
            return "AlfaBeta AI Algorithm implementation.";
        }

        public void AlfaBetaStepHandler(AbstractStep step, IState state)
        {
            Console.WriteLine("AlfaBeta Stephandler has been called.");
            // 1. Get the current gamestate.(current node on the gametree)
            // 2. execute alfabeta search in a given depth
            // 3. select the next step.
            AbstractStep aStep = null;
            game.DoStep(aStep, playerType);
        }

        public void AddToGame(AbstractGame game, PlayerType playerType)
        {
            this.game = game;
            this.playerType = playerType;
            AbstractGame.StepHandler stepHandler = AlfaBetaStepHandler;
            game.RegisterAsPlayer<AlphaBetaSearch>(ref stepHandler, playerType, EntityType.ComputerPlayer, this);
        }
    }
}