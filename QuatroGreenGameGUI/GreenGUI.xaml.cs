// Author: Zsolt Turi (ITUPBD)
// Email: tuzraai at inf.elte.hu
// Date: 2014.05.15.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Game;
using Game.GameBase;

namespace Game.QuatroGreenGameGUI
{
    // Defines the GreenGUI class
    // which is a UserControl
    // and specifies one type of a Quatro GameGUI.
    public partial class GreenGUI : UserControl
    {
        Int32 selection = -1; // which piece is selected 0..15, -1 is none
        Int32 turns = 0;      // 0 if it is my turn, 1 if the opponent's
        String[] players;     // name of the two player
        Quarto quatro;        // this object holds all of the game data the can be queried
        PlayerType iam = PlayerType.PlayerOne;  // am i player one or two?

        Button[,] area = new Button[4, 4];      // buttons on the left side, the game area
        Button[,] available = new Button[4, 4]; // buttons on the right side, the available pieces
        Image[] images = new Image[16];         // images of the pieces
        Image imageEmpty;                       // image for the case there is no piece
        Image imageUnknown;                     // image for the case there is no piece choosed

        // Constructs the UserControl GreenGUI
        // _quatro: this object holds all of the game data the can be queried
        public GreenGUI()
        {
            InitializeComponent();
            
            // Load images
            for (int i = 0; i < 16; i++)
            {
                Image im = new Image();
                im.Source = new BitmapImage(new Uri(@"Images/piece-" + i + ".png", UriKind.Relative));
                images[i] = im;
            }
            imageEmpty = new Image();
            imageEmpty.Source = new BitmapImage(new Uri(@"Images/empty.png", UriKind.Relative));
            imageUnknown = new Image();
            imageUnknown.Source = new BitmapImage(new Uri(@"Images/unknown.png", UriKind.Relative));

            // Create buttons (area)
            for (int i = 0; i < 16; i++)
            {
                Button b = new Button();
                b.Click += BtPut_Click;
                b.Tag = i;
                gameArea.Children.Add(b);
                Image im = new Image();
                im.Source = imageEmpty.Source;
                b.Content = im;
                b.SetValue(Grid.RowProperty, i/4);
                b.SetValue(Grid.ColumnProperty, i%4);
                area[i/4,i%4] = b;
            }

            // Create buttons (available)
            for (int i = 0; i < 16; i++)
            {
                Button b = new Button();
                b.Click += BtChoose_Click;
                b.Tag = i; 
                gameArea.Children.Add(b);
                Image im = new Image();
                im.Source = images[i].Source;
                b.Content = im;
                b.SetValue(Grid.RowProperty, i / 4);
                b.SetValue(Grid.ColumnProperty, i % 4);
                available[i/4,i%4] = b;
            }

            // Set the two player string
            players = new String[2] { "You", "Opponent" };

            // Displays who turns next
            ShowNextEvent();
        }

        // OnStep is a callback called every time a step happened
        // state: there is no mean of this parameter
        private void OnStep(IState state)
        {
            // Gets all of the data from the quatro object
            if(!(state is Board)) return;
            var board = (Board) state;
            Piece[] pa = board.ActivePieces;
            var paa = board.BBoard; // pálya, ahol 2222 a szabad
            Piece p = board.SelectedPiece; // null, ha semmi
            //bool w = paa.Length == 0;
            bool w = board.Winstate;
            bool m = board.CurrentPlayer == iam;
            bool c = (p != null);

            // Sets the game area according to the data given
            for (int i = 0; i < 16; i++)
            {
                var btn = (((System.Windows.Controls.Panel)(gameArea.Children[0])).Children[i]) as Button;
                btn.IsEnabled = false;
                (btn.Content as Image).Source = imageEmpty.Source;
                
            }

            for (int i = 0; i < 16; i++)
            {
                var btn = (((System.Windows.Controls.Panel)(gameArea.Children[0])).Children[i]) as Button;
                int id = paa[i/4, i%4].getNumber();
                if (m && c) btn.IsEnabled = true;
                (btn.Content as Image).Source = id < 16 ? images[id].Source : imageEmpty.Source;

            }

            // Sets the available pieces according to the data given
            for (int i = 0; i < 16; i++)
            {
                var btn = (((System.Windows.Controls.Panel)(availablePieces)).Children[i]) as Button;
                btn.IsEnabled = false;
                (btn.Content as Image).Source = imageEmpty.Source;
            }
            foreach (Piece x in pa)
            {
                int id = x.getNumber();
                var btn = (((System.Windows.Controls.Panel)(availablePieces)).Children[id]) as Button;
                if (m && !c) btn.IsEnabled = true;
                (btn.Content as Image).Source = id < 16 ? images[id].Source : imageEmpty.Source;
            }

            // Sets the selected piece
            if (p == null) ImNext.Source = imageUnknown.Source;
            else ImNext.Source = images[p.getNumber()].Source;
            
            // Tells if someone wins the game
            if (w) MessageBox.Show(!m ? "You lose.":"You win!");
            
            // Changes if the other player comes
            turns = m ? 0 : 1;

            // Displays who turns next
            ShowNextEvent();
        }

        // Displays who turns next and what to do
        private void ShowNextEvent()
        {
            LbNextEvent.Content = players[turns] + " " + (selection == -1?"Choose":"Put");
        }

        // Handles the clicks from the available pieces
        private void BtChoose_Click(object sender, RoutedEventArgs e)
        {
            if (selection != -1) return;

            Button btn = (sender as Button);
            Int32 id = Convert.ToInt32(btn.Tag);
            selection = id;
            ImNext.Source = (btn.Content as Image).Source;
            btn.Visibility = System.Windows.Visibility.Hidden;

            // Sends the step to the quatro object
            QuartoStep qs = new QuartoStep(0, 0, new Piece(id));
            quatro.DoStep(qs, iam);

            turns = 1 - turns;
            ShowNextEvent();

            foreach (Button b in area) { b.IsEnabled = false; }
            foreach (Button b in available) { b.IsEnabled = false; }
        }

        // Handles the clicks on the game area
        private void BtPut_Click(object sender, RoutedEventArgs e)
        {
            if (selection == -1) return;
            
            Button btn = (sender as Button);
            Image img = (btn.Content as Image);
            Int32 id = Convert.ToInt32(btn.Tag);
            img.Source = ImNext.Source;
            ImNext.Source = ImUnknown.Source;
            selection = -1;
            btn.IsEnabled = false;

            // Sends the step to the quatro object
            QuartoStep qs = new QuartoStep(id / 4, id % 4, null);
            quatro.DoStep(qs, iam);

            ShowNextEvent();

            foreach (Button b in area) { b.IsEnabled = false; }
            foreach (Button b in available) { b.IsEnabled = (b.Content as Image).Source != imageEmpty.Source; }
        }

        // Defines am i a player one or player two
        // pt:PlayerType -II-
        public void AddToGame(Quarto game, PlayerType pt)
        {
            iam = pt;

            // Sets the quatro object
            quatro = game;

            // Sets a callback for every step happened
            AbstractGame.StepHandler ost = new AbstractGame.StepHandler(OnStep);
            quatro.RegisterAsPlayer(ref ost, iam);
        }
    }
}
