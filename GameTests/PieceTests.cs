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
    public class PieceTests
    {
       
        [TestMethod()]
        public void setPieceTest()
        {
            Piece a = new Piece();
            Piece b = new Piece();
            b.color = 0;
            b.full = 1;
            b.height = 1;
            b.shape = 1;
            a.setPiece(0, 1, 1, 1);

            if (a == b)
            {
                Assert.IsTrue(true);

            }
            else
            {
                Assert.IsFalse(false);
            }
        }
    }
}
