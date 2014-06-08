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


namespace GameGUI.View
{
    //Defines the MainWindow class
    //which is a UserControl
    //and specifies one type of a Chess GameGUI.
    public partial class MainWindow : UserControl
    {
        Int32 selection = -1; // which piece is selected 0..64, -1 is none
        Int32 turns = 0;      // 0 if it is my turn, 1 if the opponent's
        String[] players;     // name of the two player
        Chess chess;      // this object holds all of the game data the can be queried
        PlayerType iam = PlayerType.PlayerOne;  // am i player one or two?

        Button[,] area = new Button[8, 8];      // buttons on the side, the game area
        Image[] images = new Image[64];         // images of the pieces
        Image imageEmpty;                       // image for the case there is no piece
        
        // Constructs the UserControl MainWindow
        // _chess: this object holds all of the game data the can be queried
        
        public MainWindow()
        {
            InitializeComponent();
        
            
        }

        // OnStep is a callback called every time a step happened
        // state: there is no mean of this parameter
        private void OnStep(IState state)
        {


         }

        private void BtPut_Click(object sender, RoutedEventArgs e)
        {

        }

        // Defines am i a player one or player two
        // pt:PlayerType -II-
        public void AddToGame(Chess game, PlayerType pt)
        {
            iam = pt;

            // Sets the chess object
            chess = game;

            // Sets a callback for every step happened
            AbstractGame.StepHandler ost = new AbstractGame.StepHandler(OnStep);
            chess.RegisterAsPlayer(ref ost, iam);
        }
    }
}
