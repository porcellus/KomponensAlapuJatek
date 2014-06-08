using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Game.GameBase;

namespace Game
{
    public class Board : IState
    {
	    static bool Equals(Board b1, Board b2)
        {
            if (b1 == null || b2 == null)
		        return false;

	        int[] dim1 = b1.getDimension();
	        int[] dim2 = b2.getDimension();

            if (dim1[0] != dim2[0] || dim1[1] != dim2[1])
		        return false;

            for (int i = 0; i < dim1[1]; ++i)
		        for (int j = 0; j < dim1[0]; ++j)
                    if (b1.getFigureAt(i, j).getFigureType() != b2.getFigureAt(i, j).getFigureType() ||
                        b1.getFigureAt(i, j).isWhite() != b2.getFigureAt(i, j).isWhite())
				        return false;
	        return true;  
        }

        protected int[]             dimension;

        protected String            symList;
        protected int               symIndex;
        protected ChessHeuristic    heuristic;

        protected Figure[,]         data;
        private PlayerType          currentPlayer;

        public Board()
        {
            symIndex    = 0;
            symList     = "abcdefghijklmnopqrstuyz123456789,.-+/\'!%=();>*<#&@{}¤ß$Łł€";
        }

        ~Board()
        {
            symIndex    = 0;
        }

        public void setCurrentPlayer(PlayerType cPlayer)
        {
            currentPlayer = cPlayer;
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

        public Figure[,] getData()
        {
            return data;
        }
            
	    public void	setContent(String fig, String col)			
        {
            if (data == null)
                return;

            if (fig.Length != col.Length)
                return;

            for (int i = 0; i < fig.Length; ++i)
            {
                int row        = i / dimension[0];
                int column     = i % dimension[1];

                Figure.FigureType ftype = (Figure.FigureType)(int.Parse(fig[i] + ""));

                if (ftype == Figure.FigureType.Nothing)
                    continue;

                bool white      = col[i] == '1';

                Figure figure   = new Figure(ftype, row, column, this);
                figure.setColor(white);
                data[row, column]  = figure;
            }
        }

	    public void	setDimension(int[] dim)		
        {
            dimension   = dim;
            data        = new Figure[dim[0], dim[1]];
        }

	    public Figure getFigureAt(int row, int col)	
        { 
            return data[row, col]; 
        }

        public int[] getDimension()						
        { 
            return dimension; 
        }

        public Figure getItemByRC(int row, int col)
        {
            if (col < 0 || row < 0 || col >= dimension[0] || row >= dimension[1])
                return null;
            return data[row,col];
        }

	    public char	getNextSym()
        {
            if (symIndex < symList.Length - 1)
                return (symList[symIndex++]);
            return (char)Figure.FigureType.Nothing;
        }

        public void Step(int fromRow, int fromCol, int toRow, int toCol)
        {
            data[fromRow, fromCol].setRC(toRow, toCol);

            data[toRow, toCol]          = data[fromRow, fromCol];
            data[fromRow, fromCol]      = null;
        }

        public bool checkTest(bool white)
        {
            for (int i=0; i<dimension[1]; ++i)
		        for (int j=0; j<dimension[0]; ++j)
		        {
                    Figure figure = getFigureAt(i, j);

                    if (figure == null || figure.getFigureType() == Figure.FigureType.Nothing)
                        continue;

				    bool enemy;

                    if (figure.isWhite() != white)
					    enemy = true;
				    else
					    enemy = false;

				    if (enemy)
					    foreach (int[] step in Figure.getLegalSteps(this, figure, true))
						    if (step[2] == 3.0f)
							    return true;
		        }

            return false;
        }

        public bool checkmateTest(bool white)
        {
            for (int i=0; i<dimension[1]; ++i)
		        for (int j=0; j<dimension[0]; ++j)
                {
                    Figure figure = getFigureAt(i, j);

                    if (figure == null || figure.getFigureType() == Figure.FigureType.Nothing)
                        continue;

                    bool enemy;

                    if (figure.isWhite() != white)
                        enemy = true;
                    else
                        enemy = false;

				    Board tBoard;

				    if (!enemy)
					    foreach (int[] step in Figure.getLegalSteps(this, figure, false))
					    {
						    tBoard = this.Clone();
						    tBoard.Step(i, j, step[0], step[1]);

						    if (!tBoard.checkTest(white))
							    return false;
					    }
			        
		        }

	        return true;
        }

	    public Board Clone()
        {
            Board b     = new Board();

            b.setDimension(this.dimension);
            b.setHeuristic(this.heuristic);
            b.setContent("0000000000000000000000000000000000000000000000000000000000000000", 
                         "0000000000000000000000000000000000000000000000000000000000000000");

            foreach (Figure fig in data)
            {
                if (fig == null || fig.getFigureType() == Figure.FigureType.Nothing)
                    continue;

                Figure clone = fig.Clone();
                fig.setBoard(b);
                b.addFigure(fig);
            }

            return b;
        }

        public void addFigure(Figure fig)
        {
            data[fig.getRow(), fig.getCol()] = fig;
        }
        
        protected int Index(int row, int col)
        {
            return (row - 1) * (int)dimension[0] + col;
        }


        public PlayerType CurrentPlayer
        {
            get
            {
                return currentPlayer;
            }
            set { setCurrentPlayer(value); }
        }
    }
}
