using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Chess.Heuristic;

namespace Game
{
    class Board
    {
	    static bool Equals(Board b1, Board b2)
        {
            if (b1 == null || b2 == null)
		        return false;

	        int[] dim1 = b1.getDimension();
	        int[] dim2 = b2.getDimension();

            if (dim1[0] != dim2[0] || dim1[1] != dim2[1])
		        return false;

            for (int i = 1; i <= dim1[1]; ++i)
		        for (int j = 1; j <= dim1[0]; ++j)
			        if (b1.getRealTypeByRC(i, j) != b2.getRealTypeByRC(i, j))
				        return false;
	        return true;  
        }

	    protected String		    content;
        protected String            contentReal;
        protected String            contentColor;
        protected int[]             dimension;

        protected String            symList;
        protected int               symIndex;
        protected ChessHeuristic    heuristic;

        public Board()
        {
            symIndex    = 0;
            symList     = "abcdefghijklmnopqrstuyz123456789,.-+/\'!%=();>*<#&@{}¤ß$Łł€";
        }

        ~Board()
        {
            symIndex    = 0;
        }

        public void setHeuristic(ChessHeuristic heur)
        {
            heuristic   = heur;
        }

        public int getValue()
        {
            if (heuristic == null)
                return 0;
            return heuristic.GetValue(this);
        }

	    public void	setContentByRC(int row, int col, char item)
        {
            StringBuilder sb            = new StringBuilder(content);
            sb[Index(row, col) - 1]     = item;
            content                     = sb.ToString();
        }
            
	    public void	setContent(String c)			
        { 
            content = c; 
            contentReal = c; 
        }

	    public void	setContentColor(String c)		
        {
            contentColor = c; 
        }

	    public void	setDimension(int[] dim)		
        {
            dimension = dim; 
        }

	    public string getContent()						
        { 
            return content; 
        }

	    public char	getContentColor(int row, int col)	
        { 
            return contentColor[Index(row, col) - 1]; 
        }

	    public char getRealTypeByRC(int row, int col)	
        { 
            return contentReal[Index(row, col) - 1]; 
        }

        public int[] getDimension()						
        { 
            return dimension; 
        }

	    public char	getItemByRC(int row, int col)
        {
            if (col < 1 || row < 1 || col > dimension[0] || row > dimension[1])
                return (char)Figure.FigureType.Nothing;
            return getItemByIndex(Index(row, col));
        }

	    public char	getNextSym()
        {
            if (symIndex < symList.Length - 1)
                return (symList[symIndex++]);
            return (char)Figure.FigureType.Nothing;
        }

        public void Step(int fromRow, int fromCol, int toRow, int toCol)
        {
            StringBuilder sb                = new StringBuilder(content);
            sb[Index(toRow, toCol) - 1]     = content[Index(fromRow, fromCol) - 1];
            sb[Index(fromRow, fromCol) - 1] = '0';
            content                         = sb.ToString();

            sb                              = new StringBuilder(contentColor);
            sb[Index(toRow, toCol) - 1]     = contentColor[Index(fromRow, fromCol) - 1];
            sb[Index(fromRow, fromCol) - 1] = '0';
            contentColor                    = sb.ToString();

            sb                              = new StringBuilder(contentReal);
            sb[Index(toRow, toCol) - 1]     = contentReal[Index(fromRow, fromCol) - 1];
            sb[Index(fromRow, fromCol) - 1] = '0';
            contentReal                     = sb.ToString();
        }

        public bool checkTest(bool white)
        {
            for (int i=1; i<=dimension[1]; ++i)
		        for (int j=1; j<=dimension[0]; ++j)
		        {
			        char sym	= getRealTypeByRC(i,j);

                    if (sym != (char)Figure.FigureType.Nothing)
			        {
				        bool enemy;

				        if ((getContentColor(i,j) == '1' && !white) || (getContentColor(i,j) == '2' && white))
					        enemy = true;
				        else
					        enemy = false;

				        if (enemy)
					        foreach (int[] step in Figure.getLegalSteps(this, i, j, true))
						        if (step[2] == 3.0f)
							        return true;
			        }
		        }

            return false;
        }

        public bool checkmateTest(bool white)
        {
            for (int i=1; i<=dimension[1]; ++i)
		        for (int j=1; j<=dimension[0]; ++j)
		        {
			        char sym	= getRealTypeByRC(i,j);

                    if (sym != (char)Figure.FigureType.Nothing)
			        {
				        bool enemy;

				        if ((getContentColor(i,j) == '1' && !white) || (getContentColor(i,j) == '2' && white))
					        enemy = true;
				        else
					        enemy = false;

				        Board tBoard;

				        if (!enemy)
					        foreach (int[] step in Figure.getLegalSteps(this, i, j, false))
					        {
						        tBoard = this.Clone();
						        tBoard.Step(i,j,step[0],step[1]);

						        if (!tBoard.checkTest(white))
							        return false;
					        }
			        }
		        }

	        return true;
        }

	    public Board Clone()
        {
            Board b = new Board();

            b.setDimension(this.dimension);
            b.setContent(this.contentReal);
            b.setContentColor(this.contentColor);
            b.setHeuristic(this.heuristic);

            return b;
        }

	    protected char getItemByIndex(int ind)
        {
            return content[ind - 1];
        }

        protected int Index(int row, int col)
        {
            return (row - 1) * (int)dimension[0] + col;
        }

    }
}
