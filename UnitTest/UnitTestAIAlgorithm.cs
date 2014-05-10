﻿using System;
using System.Collections.Generic;
using Client.AlfaBeta;
using Game.GameBase;
using GameBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestAIAlgorithm
{
    class TestGame : AbstractGame
    {
        public override string GetGameTypeInfo()
        {
            throw new NotImplementedException();
        }

        public override void RegisterAsPlayer<TAlgorithm>(ref StepHandler onStep, PlayerType playerType, EntityType entityType,
            TAlgorithm algorithm)
        {
        }

        public override void SetHeuristic<TBoard>(IHeuristic<TBoard> heuristic)
        {
            throw new NotImplementedException();
        }

        public override AbstractStep.Result DoStep(AbstractStep step, PlayerType playerType)
        {
            throw new NotImplementedException();
        }

        public override IState SimulateStep(AbstractStep step)
        {
            throw new NotImplementedException();
        }

        public override int SimulateStep(AbstractStep step, int dummyInt)
        {
            throw new NotImplementedException();
        }

        public override void StartGame()
        {
            throw new NotImplementedException();
        }

        public override bool IsTerminal(IState state)
        {
            if (state is TestState)
            {
                return ((TestState)state).GetChildCount() == 0;
            }
            throw new NotImplementedException();
        }

        public override double GetHeuristicValue(IState state)
        {
            if (state is TestState)
            {
                return ((TestState)state).GetHValue();
            }
            throw new NotImplementedException();
        }

        public override IState GetNextState(IState current, AbstractStep step)
        {
            if (current is TestState && step is TestStep)
            {
                return ((TestState)current).getChildAt(((TestStep)step).GetStep());
            }
            throw new NotImplementedException();
        }
    }
    class TestStep : AbstractStep
    {
        private IState state;
        private int step;
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
    class TestState : IState
    {
        List<IState> list = new List<IState>();
        private double hValue;

        public TestState(double value)
        {
            hValue = value;
        }
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
        public IEnumerable<AbstractStep> GetAvailableSteps()
        {
            List<AbstractStep> steps = new List<AbstractStep>();
            for (int i = 0; i < list.Count; i++)
            {
                steps.Add(new TestStep(list[i], i));
            }
            return steps;
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
            AlphaBetaSearch abs = new AlphaBetaSearch();
            IState state = new TestState(0);
            AbstractStep step = abs.MakeDecision(state);
            Assert.AreEqual(null, step);
        }

        [TestMethod]
        public void TestMethodLevel1()
        {
            AlphaBetaSearch abs = new AlphaBetaSearch(2);
            TestState state = new TestState(1);
            TestState left = new TestState(4);
            TestState right = new TestState(3);

            state.AddChilds(left, right);

            abs.AddToGame(new TestGame(), PlayerType.PlayerOne);
            AbstractStep step = abs.MakeDecision(state);
            Assert.AreEqual(1, ((TestStep)step).GetStep());
            Assert.AreEqual(right, ((TestStep)step).GetState());

        }

        [TestMethod]
        public void TestMethodLevel2()
        {
            AlphaBetaSearch abs = new AlphaBetaSearch(4);
            TestState state = new TestState(0);
            TestState level1_0 = new TestState(0);
            TestState level1_1 = new TestState(0);
            TestState level2_0 = new TestState(2);
            TestState level2_1 = new TestState(7);
            TestState level2_2 = new TestState(1);
            TestState level2_3 = new TestState(8);
            state.AddChilds(level1_0, level1_1);
            level1_0.AddChilds(level2_0, level2_1);
            level1_1.AddChilds(level2_2, level2_3);

            abs.AddToGame(new TestGame(), PlayerType.PlayerOne);
            AbstractStep step = abs.MakeDecision(state);
            Assert.AreEqual(level1_0, ((TestStep)step).GetState());
        }
    }
}
