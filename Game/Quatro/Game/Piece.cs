using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Piece
    {
        private bool Height = false; //1 magas 0 alacsony
        private bool Color = false;  //1 fehér 0 fekete
        private bool Shape = false;  //1 kerek 0 szögletes
        private bool Full = false;   //1 lyukas 0 sima

        public bool height
        {
            get { return Height; }
            set { Height = value; }
        }


        public bool color
        {
            get { return Color; }
            set { Color = value; }
        }


        public bool shape
        {
            get { return Shape; }
            set { Shape = value; }
        }


        public bool full
        {
            get { return Full; }
            set { Full = value; }
        }


        public void setPiece(bool vHeight, bool vColor, bool vShape, bool vFull)
        {
            Height = vHeight; //1 magas 0 alacsony
            Color = vColor;  //1 fehér 0 fekete
            Shape = vShape;  //1 kerek 0 szögletes
            Full = vFull;   //1 lyukas 0 sima

        }
        public void setPiece(int vHeight, int vColor, int vShape, int vFull)
        {
            Height = Convert.ToBoolean(vHeight); //1 magas 0 alacsony
            Color = Convert.ToBoolean(vColor);  //1 fehér 0 fekete
            Shape = Convert.ToBoolean(vShape);  //1 kerek 0 szögletes
            Full = Convert.ToBoolean(vFull);   //1 lyukas 0 sima

        }
    }
}
