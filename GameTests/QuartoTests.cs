using Game.GameBase;

using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Game.Tests
{
    [TestClass()]
    public class QuartoTests : Quarto
    {
        

      
        
       

     
        

       

         

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
        public void DoStepTest()
        {
            initGame();
            Piece piece = new Piece();
            piece.setPiece(1, 1, 1, 1);
            QuartoStep step = new QuartoStep(0, 0, piece);
            activeBoard.ActivePlayerIndex = 0;
            activeBoard.Player[0] = new Player();
            activeBoard.Player[0].PlayerType = PlayerType.PlayerOne;
            AbstractStep sStep = (AbstractStep)step;
            DoStep(sStep, PlayerType.PlayerOne);

            if (activeBoard.SelectedPiece != null)
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
