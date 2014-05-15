using System;
using System.Linq;
using Client.AIAlgorithmBase;
using Game.GameBase;
using QuickGraph;

namespace Client.MinMax
{
    public class MinimaxSearch : IAIAlgorithm
    {
        private AbstractGame _Game;
        private PlayerType _PlayerType;
        private AdjacencyGraph<int, TaggedEdge<int, string>> _Graph;
        private int _ExpandedNodes;
        private int _Depth;

        public MinimaxSearch(int depth)
        {
            _Depth = depth;
        }

        public MinimaxSearch()
        {
            _Depth = 2;
        }

        public string GetAIAlgorithmInfo()
        {
            return @"Minimax is a decision rule used in decision theory, game theory, 
                     statistics and philosophy for minimizing the possible loss for a worst 
                     case (maximum loss) scenario.";
        }

        public void AddToGame(AbstractGame game, PlayerType playerType)
        {
            _Game = game;
            _PlayerType = playerType;

            AbstractGame.StepHandler stepHandler = StepHandler;
            _Game.RegisterAsPlayer(ref stepHandler, playerType, EntityType.ComputerPlayer, this);
        }

        public void StepHandler(IState state)
        {
            //IEnumerable<AbstractStep> availableSteps = state.GetAvailableSteps();
            //BuildGraph(availableSteps);
            AbstractStep optimalStep = MakeDecision(state);
            _Game.DoStep(optimalStep, _PlayerType);
        }

        public AbstractStep MakeDecision(IState state)
        {
            _ExpandedNodes = 0;
            AbstractStep result = null;
            double resultValue = Double.NegativeInfinity;

            foreach (AbstractStep step in state.GetAvailableSteps())
            {
                double value = MinValue(_Game.GetNextState(state, step));
                if (value > resultValue)
                {
                    result = step;
                    resultValue = value;
                }
            }
            return result;
        }

        private double MaxValue(IState state)
        {
            _ExpandedNodes++;
            if (_Game.IsTerminal(state) || _Depth == _ExpandedNodes)
            {
                return _Game.GetHeuristicValue(state);
            }

            return state.GetAvailableSteps().Select(step =>
                MinValue(_Game.GetNextState(state, step))).Concat(new[] { Double.NegativeInfinity }).Max();
        }

        private double MinValue(IState state)
        {
            _ExpandedNodes++;
            if (_Game.IsTerminal(state) || _Depth == _ExpandedNodes)
            {
                return -_Game.GetHeuristicValue(state);
            }

            return state.GetAvailableSteps().Select(step =>
                MaxValue(_Game.GetNextState(state, step))).Concat(new[] { Double.PositiveInfinity }).Min();
        }

        /*      private AbstractStep MakeDecision(TState state)
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

        private void BuildGraph(IEnumerable<AbstractStep> stpes)
        {
            foreach (AbstractStep step in stpes)
            {
                IState simulateStep = _Game.SimulateStep(step);
            }
        }*/
    }
}
