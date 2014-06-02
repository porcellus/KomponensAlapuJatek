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
    public class BoardTests:Board
    {
        [TestMethod()]
        public void BoardTest()
        {
            BoardTests bboard = new BoardTests();

            Assert.AreEqual(null, bboard);
        }

        [TestMethod()]
        public void BoardTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void setActivePiecesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void insertPieceTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void checkIsEmptyTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void checkIsFullTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void checkWinningStateTest()
        {
            Assert.Fail();
        }
    }
}
