using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class Figure
    {
        public enum StepType
        {
            Capture         = 0,
            CaptureKing     = 1,
            Failure         = 2,
            Success         = 3
        };

        public enum FigureType
        {
            Nothing = 0,
            King    = 1,
            Queen   = 2,
            Rook    = 3,  
            Knight  = 4,
            Bishop  = 5,
            Pawn    = 6
        };

	    // Megnézi, hogy adott figurával legális lépés-e
	    public static StepType isLegalStep(Board board, int fromRow, int fromCol, int toRow, int toCol)
        {
            if (toRow < 0 || toRow >= board.getDimension()[1] || toCol < 0 || toCol >= board.getDimension()[0])
		        return StepType.Failure;

	        if (fromRow == toRow && fromCol == toCol)
		        return StepType.Failure;

            Figure figure = board.getFigureAt(toRow, toCol);

            if (figure == null || figure.getFigureType() == FigureType.Nothing)
		        return StepType.Success;

	        if (Figure.isEnemy(board, fromRow, fromCol, toRow, toCol))
	        {
                if (figure.getFigureType() == Figure.FigureType.King)
			        return StepType.CaptureKing;
		        return StepType.Capture;
	        }

	        return StepType.Failure;	
        }

	    // Megadja a legális lépéseket
	    public static LinkedList< int[] > getLegalSteps(Board board, Figure figure, bool fromCheckTest)
        {
            if (figure == null || figure.getFigureType() == FigureType.Nothing)
                return null;

            LinkedList<int[]>  LegalSteps       = new LinkedList<int[]>();
            FigureType figureType               = figure.getFigureType();

	        bool					white		= figure.isWhite();
            int                     j           = -1;
            LinkedList<int[]>       steps       = new LinkedList<int[]>();

            int fromRow                         = figure.getRow();
            int fromCol                         = figure.getCol();

	        switch (figureType)
	        {
	        case FigureType.King:

                StepType ans;

                steps.AddLast(new int[] { fromRow - 1, fromCol - 1, 0 });
                steps.AddLast(new int[] { fromRow - 1, fromCol, 0 });
                steps.AddLast(new int[] { fromRow - 1, fromCol + 1, 0 });
                steps.AddLast(new int[] { fromRow, fromCol - 1, 0 });
                steps.AddLast(new int[] { fromRow, fromCol + 1, 0 });
                steps.AddLast(new int[] { fromRow + 1, fromCol - 1, 0 });
                steps.AddLast(new int[] { fromRow + 1, fromCol, 0 });
                steps.AddLast(new int[] { fromRow + 1, fromCol + 1, 0 });
                    
                foreach (int[] step in steps)
                {
                    ans = Figure.isLegalStep(board, fromRow, fromCol, step[0], step[1]);
			        if (ans == StepType.Success)
			        {
				        step[2] = 1;
                        LegalSteps.AddLast(step);
			        } else if (ans == StepType.Capture)
			        {
				        step[2] = 2;
                        LegalSteps.AddLast(step);
			        } else if (ans == StepType.CaptureKing)
			        {
				        step[2] = 3;
                        LegalSteps.AddLast(step);
			        }
                }

		        break;
			
	        case FigureType.Queen:

                steps.AddLast( new int[] {1,	0,	0 } );
                steps.AddLast( new int[] {-1,	0,	0 } );
                steps.AddLast( new int[] {0,	1,	0 } );
                steps.AddLast( new int[] {0,	-1,	0 } );
                steps.AddLast( new int[] {1,	1,	0 } );
                steps.AddLast( new int[] {1,	-1,	0 } );
                steps.AddLast( new int[] {-1,	1,	0 } );
                steps.AddLast( new int[] {-1,	-1,	0 } );
                    
                foreach (int[] step in steps)
                {
                    j   = 0;
                    while (j >= 0)
                    {
                        ++j;
                        ans = Figure.isLegalStep(board, fromRow, fromCol, fromRow + step[0] * j, fromCol + step[1] * j);
				        if (ans == StepType.Success)				LegalSteps.AddLast( new int[] {fromRow + step[0] * j, fromCol + step[1] * j, 1} );
				        else if (ans == StepType.Capture){		    LegalSteps.AddLast( new int[] {fromRow + step[0] * j, fromCol + step[1] * j, 2} ); j = -1; }
				        else if (ans == StepType.CaptureKing){	    LegalSteps.AddLast( new int[] {fromRow + step[0] * j, fromCol + step[1] * j, 3} ); j = -1; }
				        else if (ans == StepType.Failure)		    j = -1;
                    }
                }

		        break;
				
	        case FigureType.Rook:		
                steps.AddLast( new int[] {1,	0,	0 } );
                steps.AddLast( new int[] {-1,	0,	0 } );
                steps.AddLast( new int[] {0,	1,	0 } );
                steps.AddLast( new int[] {0,	-1,	0 } );
                    
                foreach (int[] step in steps)
                {
                    j   = 0;
                    while (j >= 0)
                    {
                        ++j;
                        ans = Figure.isLegalStep(board, fromRow, fromCol, fromRow + step[0] * j, fromCol + step[1] * j);
				        if (ans == StepType.Success)				LegalSteps.AddLast( new int[] {fromRow + step[0] * j, fromCol + step[1] * j, 1} );
				        else if (ans == StepType.Capture){		    LegalSteps.AddLast( new int[] {fromRow + step[0] * j, fromCol + step[1] * j, 2} ); j = -1; }
				        else if (ans == StepType.CaptureKing){	    LegalSteps.AddLast( new int[] {fromRow + step[0] * j, fromCol + step[1] * j, 3} ); j = -1; }
				        else if (ans == StepType.Failure)		    j = -1;
                    }
                }
		        break;
				
	        case FigureType.Bishop:
                steps.AddLast( new int[] {1,	1,	0 } );
                steps.AddLast( new int[] {1,	-1,	0 } );
                steps.AddLast( new int[] {-1,	1,	0 } );
                steps.AddLast( new int[] {-1,	-1,	0 } );
                    
                foreach (int[] step in steps)
                {
                    j = 0;
                    while (j >= 0)
                    {
                        ++j;
                        ans = Figure.isLegalStep(board, fromRow, fromCol, fromRow + step[0] * j, fromCol + step[1] * j);
				        if (ans == StepType.Success)				LegalSteps.AddLast( new int[] {fromRow + step[0] * j, fromCol + step[1] * j, 1} );
				        else if (ans == StepType.Capture){		    LegalSteps.AddLast( new int[] {fromRow + step[0] * j, fromCol + step[1] * j, 2} ); j = -1; }
				        else if (ans == StepType.CaptureKing){	    LegalSteps.AddLast( new int[] {fromRow + step[0] * j, fromCol + step[1] * j, 3} ); j = -1; }
				        else if (ans == StepType.Failure)		    j = -1;
                    }
                }
		        break;

            case FigureType.Knight:

                steps.AddLast( new int[] { fromRow - 2, fromCol - 1, 0 } );
                steps.AddLast( new int[] { fromRow - 2, fromCol + 1, 0 } );
                steps.AddLast( new int[] { fromRow + 2, fromCol - 1, 0 } );
                steps.AddLast( new int[] { fromRow + 2, fromCol + 1, 0 } );
                steps.AddLast( new int[] { fromRow - 1, fromCol - 2, 0 } );
                steps.AddLast( new int[] { fromRow + 1, fromCol - 2, 0 } );
                steps.AddLast( new int[] { fromRow - 1, fromCol + 2, 0 } );
                steps.AddLast( new int[] { fromRow + 1, fromCol + 2, 0 } );

                foreach (int[] step in steps)
                {
                    ans = Figure.isLegalStep(board, fromRow, fromCol, step[0], step[1]);
			        if (ans == StepType.Success)
			        {
				        step[2] = 1;
				        LegalSteps.AddLast(step);
			        } else if (ans == StepType.Capture)
			        {
				        step[2] = 2;
				        LegalSteps.AddLast(step);
			        } if (ans == StepType.CaptureKing)
			        {
				        step[2] = 3;
				        LegalSteps.AddLast(step);
			        }
                }
		        break;
			
	        case FigureType.Pawn:
		        int modifier = white ? -1 : 1;
		        if (Figure.isLegalStep(board, fromRow, fromCol, fromRow + modifier, fromCol) == StepType.Success)
			        LegalSteps.AddLast(new int[] { fromRow + modifier, fromCol, 1 });

		        ans = Figure.isLegalStep(board, fromRow, fromCol, fromRow + modifier, fromCol - 1);
		        if (ans == StepType.Capture)
			        LegalSteps.AddLast(new int[] { fromRow + modifier, fromCol - 1, 2 });
		        if (ans == StepType.CaptureKing)
			        LegalSteps.AddLast(new int[] { fromRow + modifier, fromCol - 1, 3 });

		        ans = Figure.isLegalStep(board, fromRow, fromCol, fromRow + modifier, fromCol + 1);
		        if (ans == StepType.Capture)
			        LegalSteps.AddLast(new int[] { fromRow + modifier, fromCol + 1, 2 });
		        if (ans == StepType.CaptureKing)
			        LegalSteps.AddLast(new int[] { fromRow + modifier, fromCol + 1, 3 });
		        break;
	        }

            Console.WriteLine(figure.getFigureType() + " " + figure.getRow() + " " + fromCheckTest);
	        if (!fromCheckTest)
            {
                Board tBoard = board.Clone();
		        for (int i = 0; i < LegalSteps.Count(); ++i)
                {
                    Console.WriteLine(figure.getFigureType() + " " + figure.getRow() + " " + fromCheckTest);
                    Console.WriteLine(i + " " + fromRow + " " + fromCol);
                    Console.WriteLine(1 + " " + tBoard.getFigureAt(fromRow, fromCol).getFigureType());
                    tBoard.Step(fromRow, fromCol, LegalSteps.ElementAt(i)[0], LegalSteps.ElementAt(i)[1]);

			        if (tBoard.checkTest(white))
                    {
                        tBoard.Step(LegalSteps.ElementAt(i)[0], LegalSteps.ElementAt(i)[1], fromRow, fromCol);
                        LegalSteps.Remove(LegalSteps.ElementAt(i));
				        --i;
			        } else 
                        tBoard.Step(LegalSteps.ElementAt(i)[0], LegalSteps.ElementAt(i)[1], fromRow, fromCol);

		        }
	        }

	        return LegalSteps;
        }

        // Megadja, hogy adott figurához a másik figura ellenség-e
        protected static bool isEnemy(Board board, int fr, int fc, int r, int c)
        {
            return board.getFigureAt(r, c).isWhite() != board.getFigureAt(fr, fc).isWhite();
        }

	    // A figura típusa
	    protected FigureType	            _figureType;
	    // A figura táblabeli pizíciója
	    protected int			            _row, _col;
	    // A figura színe
	    protected bool						_white;
	    // A figurához zartozó tábla
	    protected Board						_board;
	    // A legális lépéseket tartalmazó tömb
        protected LinkedList<int[]>         _legalSteps;

        public Figure(FigureType type, int r, int c, Board b)
        {
            _figureType = type;
            _row        = r;
            _col        = c;
            _board      = b;
        }

        ~Figure()
        {

        }

	    // Megnézi, hogy adott figurával legális lépés-e
	    public StepType isLegalStep(int r, int c)
        {
            LinkedListNode<int[]> node = _legalSteps.Find(new int[] { r, c, 1 });

            if (node != null)
		        return StepType.Success;

            node = _legalSteps.Find(new int[] { r, c, 2 });
            
            if (node != null)
		        return StepType.Capture;

            node = _legalSteps.Find(new int[] { r, c, 3 });
            
            if (node != null)
		        return StepType.CaptureKing;

	        return StepType.Failure;
        }
	
	    // Megadja a legális lépéseket
	    public LinkedList< int[] > getLegalSteps()
        {
            calculateLegalSteps();
            return _legalSteps;
        }
	
	    // Megadja az aktuális figura típusát
	    public FigureType getFigureType()							
        { 
            return _figureType; 
        }

	    public void calculateLegalSteps()
        {
            if (_legalSteps == null)
                _legalSteps = new LinkedList<int[]>();

            _legalSteps.Clear();
	        _legalSteps = Figure.getLegalSteps(_board, this, false);
        }

	    // Beállítja a figura színét
	    public void	setColor(bool w)						
        { 
            _white = w; 
        }

	    // Igaz, ha a figura fehér, ellenkező esetben fekete
	    public bool	isWhite() 
        { 
            return _white; 
        }

	    // Átállítja a figura pozícióját a táblán
	    public void	setRC(int r, int c)	                    
        { 
            _row    = r; 
            _col    = c; 
        }

        public int getRow()
        {
            return _row;
        }

        public int getCol()
        {
            return _col;
        }

        // Megadja, hogy adott figurához a másik figura ellenség-e
        protected bool isEnemy(int r, int c)
        {
            return _board.getFigureAt(r, c).isWhite() != _white;
        }

        public Figure Clone()
        {
            Figure clone = new Figure(_figureType, _row, _col, _board);
            clone.setColor(_white);
            return clone;
        }

        public void setBoard(Board board)
        {
            _board = board;
        }
    }
}
