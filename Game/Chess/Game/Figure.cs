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

        /*public static const int (int)StepType.Capture         = 0;	// Ütéshelyzet
        public static const int     = 1;	// Sakk helyzet
        public static const int (int)StepType.Failure         = 2;	// Hibás lépés
        public static const int SUCCESS         = 3;	// Helyes lépés

	    // A lehetséges figura típusok
	    public static const int	FigureType.King		= 1;	// Király
        public static const int FigureType.Queen      = 2;	// Királynő
        public static const int FigureType.Rook       = 3;	// Bástya
        public static const int FigureType.Knight     = 4;	// Ló
        public static const int FigureType.Bishop     = 5;	// Futó
        public static const int FigureType.Pawn       = 6;	// Gyalog*/

	    // Megnézi, hogy adott figurával legális lépés-e
	    public static StepType isLegalStep(Board board, int fromRow, int fromCol, int toRow, int toCol)
        {
            if (toRow < 1 || toRow > board.getDimension()[1] || toCol < 1 || toCol > board.getDimension()[0] || board.getItemByRC(toRow,toCol) == 'x')
		        return StepType.Failure;

	        if (fromRow == toRow && fromCol == toCol)
		        return StepType.Failure;

	        char sym = board.getRealTypeByRC(toRow,toCol);

	        if (sym == '0')
		        return StepType.Success;

	        if (Figure.isEnemy(board, fromRow, fromCol, toRow, toCol))
	        {
		        if ((int)sym - 48 == (int)Figure.FigureType.King)
			        return StepType.CaptureKing;
		        return StepType.Capture;
	        }

	        return StepType.Failure;	
        }

	    // Megadja a legális lépéseket
	    public static LinkedList< int[] > getLegalSteps(Board board, int fromRow, int fromCol, bool fromCheckTest)
        {
            LinkedList<int[]>       LegalSteps      = new LinkedList<int[]>(); 
	        FigureType			    figureType	    = (FigureType)(board.getRealTypeByRC(fromRow, fromCol) - 48);

	        if (figureType == FigureType.Nothing)
		        return LegalSteps;

	        bool					white		= (board.getContentColor(fromRow,fromCol) == '1') ? true : false;
            int                     j           = -1;
            LinkedList<int[]>       steps       = new LinkedList<int[]>();

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
                    while (++j >= 0)
                    {
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
                    while (++j >= 0)
                    {
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
                    j   = 0;
                    while (++j >= 0)
                    {
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

	        if (!fromCheckTest)
	        {
		        Board tBoard = null;
		        for (int i = 0; i < LegalSteps.Count(); ++i)
		        {
			        tBoard  = board.Clone();
                    tBoard.Step(fromRow, fromCol, LegalSteps.ElementAt(0)[0], LegalSteps.ElementAt(0)[1]);

			        if (tBoard.checkTest(white))
			        {
                        LegalSteps.Remove(LegalSteps.ElementAt(i));
				        --i;
			        }
		        }
	        }

	        return LegalSteps;
        }

        // Megadja, hogy adott figurához a másik figura ellenség-e
        protected static bool isEnemy(Board board, int fr, int fc, int r, int c)
        {
            bool white  = (board.getContentColor(fr, fc) == '1') ? true : false;
            return (board.getContentColor(r, c) == '1' && !white) || (board.getContentColor(r, c) == '2' && white);
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

        public Figure(int type, int r, int c, Board b)
        {
            _figureType = (FigureType)type;
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
	    public int getFigureType()							
        { 
            return (int)_figureType; 
        }

	    public void calculateLegalSteps()
        {
            _legalSteps.Clear();
	        _legalSteps = Figure.getLegalSteps(_board, _row, _col, false);
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
            _row = r; 
            _col = c; 
        }

        // Megadja, hogy adott figurához a másik figura ellenség-e
        protected bool isEnemy(int r, int c)
        {
            return (_board.getContentColor(r, c) == '1' && !_white) || (_board.getContentColor(r, c) == '2' && _white);
        }
    }
}
