using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class Piece : IEquatable<Piece>
    {
        private int Height = 2; 
        private int Color = 2;  
        private int Shape = 2;  
        private int Full = 2;   

        public int height
        {
            get { return Height; }
         
        }

        public int color
        {
            get { return Color; }
           
        }


        public int shape
        {
            get { return Shape; }
            
        }


        public int full
        {
            get { return Full; }
          
        }

        public int getNumber(){
            return 8 * color + 4 * height + 2 * shape + full;
            
        }

        public Piece()
        {
            setPiece(2,2,2,2);
        }

        public Piece(int vSum)
        {
            Full = vSum % 2;
            vSum = (vSum - Full) / 2;
            Shape = vSum % 2;
            vSum = (vSum - Shape) / 2;
            Height = vSum % 2;
            vSum = (vSum - Height) / 2;
            Color = vSum;
        }

        public Piece(int vHeight, int vColor, int vShape, int vFull)
        {
            setPiece(vHeight, vColor, vShape, vFull);
        }
        
        public void setPiece(int vHeight, int vColor, int vShape, int vFull)
        {
            Height = vHeight; 
            Color = vColor;  
            Shape = vShape;  
            Full = vFull;   

        }
        public bool Equals(Piece p1)
        {
            if (p1 == null)
            {
                return false;
            }
            return (Height == p1.Height && Color == p1.Color && Shape == p1.Shape && Full == p1.Full);
         
        }

    }
}
