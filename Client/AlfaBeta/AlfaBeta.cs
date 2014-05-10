﻿using System;
using Client.AIAlgorithmBase;
using Game.GameBase;
using GameBase;

namespace Client.AlfaBeta
{
    public class AlphaBetaSearch : IAIAlgorithm
    {
        private AbstractGame game;
        private PlayerType playerType;
        private int depth;
        private int expandedNodes;

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

        private AbstractStep MakeDecision(IState state)
        {
            expandedNodes = 0;
            AbstractStep result = default(AbstractStep);
            double resultValue = Double.NegativeInfinity;
            foreach (AbstractStep step in state.GetAvailableSteps())
            {
                Double value = MinValue(game.GetNextState(state, step),
                                Double.NegativeInfinity, Double.PositiveInfinity);
                if (value > resultValue)
                {
                    result = step;
                    resultValue = value;
                }
            }
            return result;
        }

        public double MinValue(IState state, double alpha, double beta)
        {
            expandedNodes++;
            if (game.IsTerminal(state) || expandedNodes == depth)
                return game.GetHeuristicValue(state);
            double value = Double.PositiveInfinity;
            foreach (AbstractStep step in state.GetAvailableSteps())
            {
                value = Math.Min(value, MaxValue(
                game.GetNextState(state, step), alpha, beta));
                if (value <= alpha)
                    return value;
                beta = Math.Min(beta, value);
            }
            return value;
        }

        public double MaxValue(IState state, double alpha, double beta)
        {
            expandedNodes++;
            if (game.IsTerminal(state) || expandedNodes == depth)
                return game.GetHeuristicValue(state);
            double value = Double.NegativeInfinity;
            foreach (AbstractStep step in state.GetAvailableSteps())
            {
                value = Math.Max(value, MinValue(
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