using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int _boardX = 10, _boardY = 10;
        int _boardWidth = 240, _boardHeight = 240;//must be multiple of 8
        Brush _borderColor = Brushes.Black;
        Brush[] _boardColor = { Brushes.Gold, Brushes.Red };
        void DrawChessBoard(Graphics g)
        {
            //draw squares
            int spacingX = _boardWidth / 8;
            int spacingY = _boardHeight / 8;
            for (int col = 0; col < 8; col++)
            {
                for (int row = 0; row < 8; row++)
                {
                    g.FillRectangle(_boardColor[(col + row) % 2], _boardX + col * spacingX, _boardY + row * spacingY, spacingX, spacingY);
                }
            }
            for (int i = 0; i < 8; i++)
            {
                //draw lines
                g.DrawLine(new Pen(_borderColor), _boardX, _boardY + i * spacingY, _boardX + _boardWidth, _boardY + i * spacingY);
                g.DrawLine(new Pen(_borderColor), _boardX + i * spacingX, _boardY, _boardX + i * spacingX, _boardY + _boardHeight);
            }
            //draw border
            g.DrawRectangle(new Pen(_borderColor, 1), new Rectangle(_boardX, _boardY, _boardWidth, _boardHeight));
            //next you would draw pieces with an array of where they are
            //assuming you have a ChessPiece class you could do something like this
            /*
            foreach(ChessPiece piece in pieces)
            {
                g.DrawImage(piece.img, piece.x, piece.y)
            }
             */
        }

        //Form1 Paint method calls DrawChessBoard
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawChessBoard(e.Graphics);
        }
    }
}
