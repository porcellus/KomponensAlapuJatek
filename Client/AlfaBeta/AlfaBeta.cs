using System;
using System.Linq;
using Client.AIAlgorithmBase;
using Game.GameBase;

namespace Client.AlfaBeta
{
    public class AlphaBetaSearch : IAIAlgorithm
    {
        private readonly int _depth;
        private AbstractGame _game;
        private PlayerType _playerType;

        public AlphaBetaSearch(int depth)
        {
            _depth = depth;
        }

        public AlphaBetaSearch() : this(2)
        {
        }

        public string GetAIAlgorithmInfo()
        {
            return "AlfaBeta AI Algorithm implementation.";
        }

        public void AddToGame(AbstractGame game, PlayerType playerType)
        {
            _game = game;
            _playerType = playerType;
            AbstractGame.StepHandler stepHandler = AlfaBetaStepHandler;
            game.RegisterAsPlayer(ref stepHandler, playerType);
        }

        public void AlfaBetaStepHandler(IState state)
        {
            Console.WriteLine("AlfaBeta Stephandler has been called.");
            if (_playerType != state.CurrentPlayer) return;
            AbstractStep aStep = MakeDecision(state);
            _game.DoStep(aStep, _playerType);
        }

        public AbstractStep MakeDecision(IState state)
        {
            const int level = 0;
            AbstractStep result = default(AbstractStep);
            double resultValue = Double.NegativeInfinity;
            foreach (AbstractStep step in _game.GetAvailableSteps(state))
            {
                Double value = MinValue(level, _game.SimulateStep(state, step),
                    Double.NegativeInfinity, Double.PositiveInfinity);
                if (value > resultValue)
                {
                    result = step;
                    resultValue = value;
                }
            }
            return result;
        }

        private double MinValue(int level, IState state, double alpha, double beta)
        {
            level++;
            if (level == _depth || _game.GetAvailableSteps(state).ToList().Count == 0)
                return _game.GetHeuristicValue(state, _playerType);
            double value = Double.PositiveInfinity;
            foreach (AbstractStep step in _game.GetAvailableSteps(state))
            {
                value = Math.Min(value, MaxValue(level,
                    _game.SimulateStep(state, step), alpha, beta));
                if (value <= alpha)
                    return value;
                beta = Math.Min(beta, value);
            }
            return value;
        }


        private double MaxValue(int level, IState state, double alpha, double beta)
        {
            level++;
            if (level == _depth || _game.GetAvailableSteps(state).ToList().Count == 0)
                return _game.GetHeuristicValue(state, _playerType);
            double value = Double.NegativeInfinity;
            foreach (AbstractStep step in _game.GetAvailableSteps(state))
            {
                value = Math.Max(value, MinValue(level,
                    _game.SimulateStep(state, step), alpha, beta));
                if (value >= beta)
                    return value;
                alpha = Math.Max(alpha, value);
            }
            return value;
        }
    }
}