using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using Game.GameBase;

using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Game.Tests
{
    [TestClass()]
    public class QuartoTests : Quarto
    {
        

      
        
       

      
        [TestMethod()]
        public void updateGameStateTest()
        {
            
            Piece piece = new Piece();
            piece.setPiece(1, 1, 1, 1);
            
           initGame();
            SelectedPiece = piece;
            
            updateGameState(0, 0);
            if (ActiveBoard.BBoard[0, 0] == piece)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail();
            }
           
        }

        [TestMethod()]
        public void selectPieceTest()
        {
            initGame();
            activePlayer = 0;
            PlayerType type =  PlayerType.PlayerOne;
            Piece piece = new Piece();
            piece.setPiece(1, 1, 1, 1);
            selectPiece(type, piece);

            if (SelectedPiece == piece)
            {
                Assert.IsTrue(true);
            }
            else
            {

                Assert.Fail();
            }
        }

        [TestMethod()]
        public void IsTerminalTest()
        {
            initGame();
            if (!IsTerminal((IState)ActiveBoard))
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail();
            
            }

        }

        [TestMethod()]
        public void GetHeuristicValueTest()
        {
            initGame();
            Piece piece = new Piece();
            piece.setPiece(1, 1, 1, 1);
            ActiveBoard.BBoard[0, 0] = piece;
            IState State = (IState)ActiveBoard;

            double ert = GetHeuristicValue(State);
            

            if (ert == 5.0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail();
            }

        }

       

         

        [TestMethod()]
        public void GetGameTypeInfoTest()
        {
            string str = GetGameTypeInfo();
            if (str == "Quarto")
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void SimulateStepTest()
        {
            Piece piece = new Piece();
            piece.setPiece(1, 1, 1, 1);
            QuartoStep step = new QuartoStep(0, 0, piece);
            initGame();
            IState State = (IState)ActiveBoard;
            Game.GameBase.AbstractStep sStep = (Game.GameBase.AbstractStep)step;
            IState ret = GetNextState(State, sStep);
            if (((Board)ret).BBoard[0, 0] == piece)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail();
            }
        }

        
        

        
        

        [TestMethod()]
        public void GetNextStateTest()
        {
            Piece piece = new Piece();
            piece.setPiece(1, 1, 1, 1);
            QuartoStep step = new  QuartoStep(0,0,piece);
            initGame();
          IState  State =(IState)ActiveBoard;
           Game.GameBase.AbstractStep sStep =(Game.GameBase.AbstractStep) step;
         IState ret =  GetNextState(State, sStep);
         if (((Board)ret).BBoard[0, 0] == piece)
         {
             Assert.IsTrue(true);
         }
         else
         {
             Assert.Fail();
         }
            
        }

        [TestMethod()]
        public void DoStepTest()
        {
            initGame();
            Piece piece = new Piece();
            piece.setPiece(1, 1, 1, 1);
            QuartoStep step = new QuartoStep(0, 0, piece);
            activePlayer = 0;
            player[0] = new Player();
            player[0].PlayerType = PlayerType.PlayerOne;
            AbstractStep sStep = (AbstractStep)step;
            DoStep(sStep, PlayerType.PlayerOne);

            if (SelectedPiece != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail();
            }

           
        }

        
    }
}
