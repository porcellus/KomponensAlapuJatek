using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using GameBase;
using Game.GameBase;
using Game;










namespace GameGUI
{
    public partial class QuartoForm : Form
    {   
         
        public QuartoForm()
        {
            InitializeComponent();
          
            
        }

        private void Quarto_Load(object sender, EventArgs e)
        {
            foreach (Control cl in panel2.Controls)
            {
                cl.Click += new EventHandler(PieceClick);
            }
            foreach (Control clboard in panel1.Controls)
            {
                clboard.Click += new EventHandler(PasteClick);
            }
          

        }


        void drawPiece()
        {
            for (int i = 0; i < 16; i++)
            {
                var ctrl = panel2.Controls.Find("Piece" + i.ToString(), true)[0];
                if (i < Quarto.ActivePieces.Length)
                {

                    ctrl.Visible = true;
                    ((PictureBox)ctrl).ImageLocation = @"../../Images/" + Quarto.ActivePieces[i].color.ToString() + Quarto.ActivePieces[i].height.ToString() + Quarto.ActivePieces[i].shape.ToString() + Quarto.ActivePieces[i].full.ToString() + ".png";
                    ((PictureBox)ctrl).Tag = Quarto.ActivePieces[i].color.ToString() + Quarto.ActivePieces[i].height.ToString() + Quarto.ActivePieces[i].shape.ToString() + Quarto.ActivePieces[i].full.ToString();

                    ((PictureBox)ctrl).SizeMode = PictureBoxSizeMode.StretchImage;
                    //((PictureBox)ctrl).Image.Width = ((PictureBox)ctrl).Width;
                    //((PictureBox)ctrl).Image.Size = new Size(((PictureBox)ctrl).Width,((PictureBox)ctrl).Height);
                }
                else
                {
                    ctrl.Visible = false;
                }
            }
        }
        void PieceClick(object sender, EventArgs e)
        {
            if (Quarto.SelectedPiece == null)
            {
                int selectedIndex = Convert.ToInt16(((PictureBox)sender).Name.Replace("Piece", ""));
                Quarto.selectPiece(Quarto.Player[Quarto.ActivePlayer].PlayerType, Quarto.ActivePieces[selectedIndex]);
                SelectedItem.ImageLocation = ((PictureBox)sender).ImageLocation;
                SelectedItem.Tag = ((PictureBox)sender).Tag;
                SelectedItem.SizeMode = PictureBoxSizeMode.StretchImage;
                drawPiece();

                label1.Text = Quarto.Player[Quarto.ActivePlayer].PlayerType.ToString() + " jön (Lerak egy bábút)";
            }



        }
        void PasteClick(object sender, EventArgs e)
        {
            if (Quarto.SelectedPiece != null)
            {
                
               
                int selectedIndex = Convert.ToInt16(((PictureBox)sender).Name.Replace("Board", ""));
               
                Quarto.updateGameState(selectedIndex % 4, selectedIndex / 4);
                
               
                ((PictureBox)sender).ImageLocation = @"../../Images/" + Quarto.ActiveBoard.BBoard[selectedIndex % 4, selectedIndex / 4].color.ToString() + Quarto.ActiveBoard.BBoard[selectedIndex % 4, selectedIndex / 4].height.ToString() + Quarto.ActiveBoard.BBoard[selectedIndex % 4, selectedIndex / 4].shape.ToString() + Quarto.ActiveBoard.BBoard[selectedIndex % 4, selectedIndex / 4].full.ToString() + ".png";
                SelectedItem.ImageLocation = @"../../Images/2222.png";
                ((PictureBox)sender).SizeMode = PictureBoxSizeMode.StretchImage;
                checkWin();

                label1.Text = Quarto.Player[Quarto.ActivePlayer].PlayerType.ToString() + " jön (Választ egy bábút)";
            }



            // Quarto.GetHeuristicValue(Quarto.ActiveBoard, EntityType.HumanPlayer);

        }
        void initBoard()
        {
            for (int i = 0; i < 16; i++)
            {
                var ctrl = panel1.Controls.Find("Board" + i.ToString(), true)[0];

                ((PictureBox)ctrl).ImageLocation = @"../../Images/" + Quarto.ActiveBoard.BBoard[i % 4, i / 4].color.ToString() + Quarto.ActiveBoard.BBoard[i % 4, i / 4].height.ToString() + Quarto.ActiveBoard.BBoard[i % 4, i / 4].shape.ToString() + Quarto.ActiveBoard.BBoard[i % 4, i / 4].full.ToString() + ".png";
                ((PictureBox)ctrl).SizeMode = PictureBoxSizeMode.StretchImage;


            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        void checkWin()
        {
            if (Quarto.ActiveBoard.Winstate)
            {

                MessageBox.Show(" You Win");
            }

        }

        private void újJátékToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Quarto.initGame();
            Quarto.StartGame();

            Quarto.Player[0].PlayerEntity = EntityType.HumanPlayer;
            Quarto.Player[1].PlayerEntity = EntityType.HumanPlayer;
            Quarto.Player[0].PlayerType = PlayerType.PlayerOne;
            Quarto.Player[1].PlayerType = PlayerType.PlayerTwo;
            drawPiece();
            initBoard();

        }
    }
}
