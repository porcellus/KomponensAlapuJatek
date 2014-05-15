using System;
using Client.AIAlgorithmBase;
using Game.GameBase;

namespace Client.AlfaBeta
{
    public class AlphaBetaSearch : IAIAlgorithm
    {
        private AbstractGame game;
        private PlayerType playerType;
        private int depth;

        public AlphaBetaSearch(int depth)
        {
            this.depth = depth;
        }

        public AlphaBetaSearch() : this(2)
        {
        }

        public string GetAIAlgorithmInfo()
        {
            return "AlfaBeta AI Algorithm implementation.";
        }

        public void AlfaBetaStepHandler(IState state)
        {
            Console.WriteLine("AlfaBeta Stephandler has been called.");
            AbstractStep aStep = MakeDecision(state);
            game.DoStep(aStep, playerType);
        }

        public AbstractStep MakeDecision(IState state)
        {
            int level = 0;
            AbstractStep result = default(AbstractStep);
            double resultValue = Double.NegativeInfinity;
            foreach (AbstractStep step in state.GetAvailableSteps())
            {
                Double value = MinValue(level, game.GetNextState(state, step),
                                Double.NegativeInfinity, Double.PositiveInfinity);
                if (value > resultValue)
                {
                    result = step;
                    resultValue = value;
                }
            }
            return result;
        }

        protected double MinValue(int level, IState state, double alpha, double beta)
        {
            level++;
            if (game.IsTerminal(state) || level == depth)
                return -game.GetHeuristicValue(state);
            double value = Double.PositiveInfinity;
            foreach (AbstractStep step in state.GetAvailableSteps())
            {
                value = Math.Min(value, MaxValue(level,
                game.GetNextState(state, step), alpha, beta));
                if (value <= alpha)
                    return value;
                beta = Math.Min(beta, value);
            }
            return value;
        }

        protected double MaxValue(int level, IState state, double alpha, double beta)
        {
            level++;
            if (game.IsTerminal(state) || level == depth)
                return game.GetHeuristicValue(state);
            double value = Double.NegativeInfinity;
            foreach (AbstractStep step in state.GetAvailableSteps())
            {
                value = Math.Max(value, MinValue(level,
                      game.GetNextState(state, step), alpha, beta));
                if (value >= beta)
                    return value;
                alpha = Math.Max(alpha, value);
            }
            return value;
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