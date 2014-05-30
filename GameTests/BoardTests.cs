using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Game.Tests
{
    [TestClass()]
    public class BoardTests
    {
        [TestMethod()]
        public void insertPieceTest()
        {
            Board board = new Board();
            Piece piece = new Piece();
            piece.setPiece(1,1,1,1);
                
            for (int i = 0; i < board.BHeight; i++)
            {
                for (int j = 0; j < board.BWidth; j++)
                {
                    board.BBoard[i, j] = new Piece();
                    board.BBoard[i, j].color = 2;
                    board.BBoard[i, j].height = 2;
                    board.BBoard[i, j].shape = 2;
                    board.BBoard[i, j].full = 2;

                }
            }
            board.insertPiece(0, 0, piece);

            if (board.BBoard[0, 0] == piece)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void checkIsEmptyTest()
        {
            Board board = new Board();
            Piece piece = new Piece();
            piece.setPiece(1, 1, 1, 1);

            for (int i = 0; i < board.BHeight; i++)
            {
                for (int j = 0; j < board.BWidth; j++)
                {
                    board.BBoard[i, j] = new Piece();
                    board.BBoard[i, j].color = 2;
                    board.BBoard[i, j].height = 2;
                    board.BBoard[i, j].shape = 2;
                    board.BBoard[i, j].full = 2;

                }
            }


            board.insertPiece(0, 0, piece);

            if (!board.checkIsEmpty(0,0))
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void checkIsFullTest()
        {
            Board board = new Board();
            Piece piece = new Piece();
            piece.setPiece(1, 1, 1, 1);

            for (int i = 0; i < board.BHeight; i++)
            {
                for (int j = 0; j < board.BWidth; j++)
                {
                    board.BBoard[i, j] = new Piece();
                    board.BBoard[i, j].color = 2;
                    board.BBoard[i, j].height = 2;
                    board.BBoard[i, j].shape = 2;
                    board.BBoard[i, j].full = 2;
                    board.insertPiece(i, j, piece);

                }
            }


           

            if (board.checkIsFull())
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void checkWinningStateTest()
        {
            Board board = new Board();
            Piece piece = new Piece();
            piece.setPiece(1, 1, 1, 1);

            for (int i = 0; i < board.BHeight; i++)
            {
                for (int j = 0; j < board.BWidth; j++)
                {
                    board.BBoard[i, j] = new Piece();
                    board.BBoard[i, j].color = 2;
                    board.BBoard[i, j].height = 2;
                    board.BBoard[i, j].shape = 2;
                    board.BBoard[i, j].full = 2;
                    if (i == j)
                    {
                        board.insertPiece(i, j, piece);
                    }

                }
            }



            board.checkWinningState();
            if (board.Winstate)
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
