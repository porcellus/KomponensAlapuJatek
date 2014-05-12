using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class Piece
    {
        private int Height = 2; //1 magas 0 alacsony
        private int Color = 2;  //1 fehér 0 fekete
        private int Shape = 2;  //1 kerek 0 szögletes
        private int Full = 2;   //1 lyukas 0 sima

        public int height
        {
            get { return Height; }
            set { Height = value; }
        }



        public int color
        {
            get { return Color; }
            set { Color = value; }
        }


        public int shape
        {
            get { return Shape; }
            set { Shape = value; }
        }


        public int full
        {
            get { return Full; }
            set { Full = value; }
        }



        public void setPiece(int vHeight, int vColor, int vShape, int vFull)
        {
            Height = vHeight; //1 magas 0 alacsony
            Color = vColor;  //1 fehér 0 fekete
            Shape = vShape;  //1 kerek 0 szögletes
            Full = vFull;   //1 lyukas 0 sima

        }
    }
}
