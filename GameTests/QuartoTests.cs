using Game.GameBase;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Game;
using System.Collections.Generic;

namespace Game.Tests
{
    [TestClass()]
    public class QuartoTests : Quarto
    {
        

        [TestMethod()]
        

        private void onStep(IState st)
        {
            IState st1 = st;
            
        }

        private void Register()
        {
            AbstractGame.StepHandler ost = new AbstractGame.StepHandler(onStep);
            StartGame();
            RegisterAsPlayer(ref ost, PlayerType.PlayerOne);
            RegisterAsPlayer(ref ost, PlayerType.PlayerTwo);
        }
        [TestMethod()]
        //Olyan bábú lerakása ami nincs a lehetséges elemek között
        public void InstertPieceTest()
        {

            Register();

            Piece piece = new Piece(1, 1, 1, 1);
            activeBoard.UpdateActivePieces(activeBoard.ActivePieces, piece);
            QuartoStep step = new QuartoStep(0, 0, piece);
            
           
           
            DoStep(step, PlayerType.PlayerOne);

             Assert.IsTrue(activeBoard.SelectedPiece == null && activeBoard.CurrentPlayer==PlayerType.PlayerOne);
           

           
        }
        [TestMethod()]
        // nem az aktív játékos jön, de megpróbál lerakni egy bábút
        public void ActivePlayerTest()
        {

            Register();
            Piece piece = new Piece(1, 1, 1, 1);
            QuartoStep step = new QuartoStep(0, 0, piece);
            DoStep(step, PlayerType.PlayerOne);
            QuartoStep step2 = new QuartoStep(1, 1, piece);
            DoStep(step, PlayerType.PlayerOne);
            Piece emptyPiece = new Piece();
            Assert.IsTrue((activeBoard.SelectedPiece).Equals(piece)  && activeBoard.CurrentPlayer == PlayerType.PlayerTwo && activeBoard.BBoard[1,1].Equals(emptyPiece));
            
        }
        [TestMethod()]
        //Nem üres mezőre próbálunk lerakni bábút
        public void InsertPieceNotEmptyFieldTest()
        {
            Register();
            
            Piece piece = new Piece(1, 1, 1, 1);
            Piece piece2 = new Piece(0, 0, 0, 0);

            QuartoStep step = new QuartoStep(0, 0, piece);
            QuartoStep step2 = new QuartoStep(1, 1, piece);
            QuartoStep step3 = new QuartoStep(1, 1, piece2);

            DoStep(step, PlayerType.PlayerOne);
            DoStep(step2, PlayerType.PlayerTwo);
            DoStep(step3, PlayerType.PlayerTwo);
            DoStep(step3, PlayerType.PlayerOne);

                  
             Assert.IsTrue((activeBoard.SelectedPiece).Equals(piece2) && activeBoard.CurrentPlayer == PlayerType.PlayerOne && activeBoard.BBoard[1, 1].Equals(piece));
           
        
        }

        [TestMethod()]
        public void AfterWinStateInsertPieceTest()
        {
            Register();
                       
            activeBoard.BBoard[0, 0].setPiece(1, 0, 1, 1);
            activeBoard.BBoard[0, 1].setPiece(1, 1, 0, 1);
            activeBoard.BBoard[1, 0].setPiece(1, 1, 1, 0);

            Piece piece = new Piece(1, 0, 0, 0);
            Piece piece2 = new Piece(0, 0, 0, 0);
            Piece emptyPiece = new Piece();
           
            QuartoStep step = new QuartoStep(1, 1, piece);
            QuartoStep step2 = new QuartoStep(2, 2, piece2);

            DoStep(step, PlayerType.PlayerOne);
            DoStep(step, PlayerType.PlayerTwo);
            DoStep(step2, PlayerType.PlayerTwo);
            DoStep(step2, PlayerType.PlayerOne);

            Assert.IsTrue(activeBoard.CurrentPlayer == PlayerType.NoOne && activeBoard.BBoard[2, 2].Equals(emptyPiece) && activeBoard.Winstate);
           
        
        }
        [TestMethod()]
        // a lehetséges lépések lekérdezése, amikor minden elem le van rakva
        public void GetAviableStepEmptyTest()
        {
            Register();

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {      Piece p=new Piece(i + j);
                    activeBoard.BBoard[i, j].setPiece(p.color,p.height,p.shape,p.full );
                }

            }
            IEnumerable <AbstractStep> lista = GetAvailableSteps(activeBoard);
            Assert.IsTrue(lista.ToList().Count == 0);
           
        }
        [TestMethod()]
        //a lehetséges lépések lekérdezése, nincs elem lerakva, de van kiválasztva 
        public void GetAviableStepFull()
        {
            Register();
            
            Piece piece = new Piece(1, 0, 0, 0);

            QuartoStep step = new QuartoStep(1, 1, piece);

            DoStep(step, PlayerType.PlayerOne);
           
            IEnumerable<AbstractStep> lista = GetAvailableSteps(activeBoard);
          
            Assert.IsTrue(lista.ToList().Count == 16);
            
        }
        [TestMethod()]
        //a lehetséges lépések lekérdezése, nincs elem lerakva és kiválasztva sem 
        public void GetAviableStepFullTest2()
        {
            Register();

            
            IEnumerable<AbstractStep> lista = GetAvailableSteps(activeBoard);
            Assert.IsTrue(lista.ToList().Count == 256);
            
        }
        [TestMethod()]
        //lépés szimulálása
        public void SimulateStepTest()
        {
            Register();

            Piece piece = new Piece(1, 0, 0, 0);
            QuartoStep step = new QuartoStep(1, 1, piece);

            IState st = SimulateStep(activeBoard, step);

            Assert.IsTrue(activeBoard.SelectedPiece == null && ((Board)st).SelectedPiece.Equals(piece));
            

            
        
        }

        
        

        
    }
}
