﻿using System;
using System.Collections.Generic;
using Client.AlfaBeta;
using Client.MinMax;
using Game.GameBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestAIAlgorithm
{
    internal class TestGame : AbstractGame
    {
        public override string GetGameTypeInfo()
        {
            throw new NotImplementedException();
        }

        public override void RegisterAsPlayer(ref StepHandler onStep, PlayerType playerType)
        {
        }

        public override AbstractStep.Result DoStep(AbstractStep step, PlayerType playerType)
        {
            throw new NotImplementedException();
        }

        public override double GetHeuristicValue(IState state, PlayerType playerType)
        {
            if (state is TestState)
            {
                return ((TestState) state).GetHValue();
            }
            throw new NotImplementedException();
        }

        public override IState SimulateStep(IState current, AbstractStep step)
        {
            if (current is TestState && step is TestStep)
            {
                return ((TestState) current).getChildAt(((TestStep) step).GetStep());
            }
            throw new NotImplementedException();
        }

        public override IEnumerable<AbstractStep> GetAvailableSteps(IState state)
        {
            var steps = new List<AbstractStep>();
            if (state is TestState)
            {
                var ts = (TestState) state;
                for (int i = 0; i < ts.GetChildCount(); i++)
                {
                    IState child = ts.getChildAt(i);
                    if (child is TestState)
                    {
                        var ch = (TestState) child;
                        steps.Add(new TestStep(child, i));
                    }
                }
            }
            return steps;
        }
    }

    internal class TestStep : AbstractStep
    {
        private readonly IState state;
        private readonly int step;

        public TestStep(IState st, int i)
        {
            step = i;
            state = st;
        }


        public int GetStep()
        {
            return step;
        }

        public IState GetState()
        {
            return state;
        }
    }

    internal class TestState : IState
    {
        private readonly List<IState> list = new List<IState>();
        private double hValue;

        public TestState(double value)
        {
            hValue = value;
        }

        public PlayerType CurrentPlayer { get; set; }

        public void AddChild(IState state)
        {
            list.Add(state);
        }

        public void AddChilds(params IState[] state)
        {
            list.AddRange(state);
        }

        public void setStateValue(double value)
        {
            hValue = value;
        }

        public IState getChildAt(int getStep)
        {
            return list[getStep];
        }

        public double GetHValue()
        {
            return hValue;
        }

        public int GetChildCount()
        {
            return list.Count;
        }
    }

    [TestClass]
    public class UnitTestAB
    {
        [TestMethod]
        public void TestMethodLevel0()
        {
            var abs = new AlphaBetaSearch();
            IState state = new TestState(0);
            abs.AddToGame(new TestGame(), PlayerType.PlayerOne);
            AbstractStep step = abs.MakeDecision(state);
            Assert.AreEqual(null, step);
        }

        [TestMethod]
        public void TestMethodLevel1()
        {
            var abs = new AlphaBetaSearch(2);
            var state = new TestState(1);
            var left = new TestState(4);
            var right = new TestState(3);
            state.AddChilds(left, right);

            abs.AddToGame(new TestGame(), PlayerType.PlayerOne);
            AbstractStep step = abs.MakeDecision(state);
            var result = (TestState) ((TestStep) step).GetState();
            Assert.AreEqual(4, result.GetHValue());
            Assert.AreEqual(left, result);
        }

        [TestMethod]
        public void TestMethodLevel2()
        {
            var abs = new AlphaBetaSearch(4);

            var state = new TestState(0);
            var level1_0 = new TestState(0);
            var level1_1 = new TestState(0);
            var level2_0 = new TestState(2);
            var level2_1 = new TestState(7);
            var level2_2 = new TestState(1);
            var level2_3 = new TestState(8);
            state.AddChilds(level1_0, level1_1);
            level1_0.AddChilds(level2_0, level2_1);
            level1_1.AddChilds(level2_2, level2_3);

            abs.AddToGame(new TestGame(), PlayerType.PlayerOne);
            AbstractStep step = abs.MakeDecision(state);
            var result = (TestState) ((TestStep) step).GetState();
            Assert.AreEqual(level1_0, result);
        }

        [TestMethod]
        public void MinimaxSearchLevel0()
        {
            var minmax = new MinimaxSearch();
            IState state = new TestState(0);
            AbstractStep step = minmax.MakeDecision(state);
            Assert.AreEqual(null, step);
        }

        [TestMethod]
        public void MinimaxSearchLevel1()
        {
            var minmax = new MinimaxSearch(2);
            var state = new TestState(1);
            var left = new TestState(4);
            var right = new TestState(3);

            state.AddChilds(left, right);

            minmax.AddToGame(new TestGame(), PlayerType.PlayerOne);
            AbstractStep step = minmax.MakeDecision(state);
            Assert.AreEqual(1, ((TestStep) step).GetStep());
            Assert.AreEqual(right, ((TestStep) step).GetState());
        }

        [TestMethod]
        public void MinimaxSearchLevel2()
        {
            var minmax = new MinimaxSearch(4);
            var state = new TestState(0);
            var level1_0 = new TestState(0);
            var level1_1 = new TestState(0);
            var level2_0 = new TestState(2);
            var level2_1 = new TestState(7);
            var level2_2 = new TestState(1);
            var level2_3 = new TestState(8);
            state.AddChilds(level1_0, level1_1);
            level1_0.AddChilds(level2_0, level2_1);
            level1_1.AddChilds(level2_2, level2_3);

            minmax.AddToGame(new TestGame(), PlayerType.PlayerOne);
            AbstractStep step = minmax.MakeDecision(state);
            Assert.AreEqual(level1_0, ((TestStep) step).GetState());
        }
    }
}