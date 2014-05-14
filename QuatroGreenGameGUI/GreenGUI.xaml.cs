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
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class GreenGUI : UserControl
    {
        Int32 selection = -1;
        Int32 turns = 0;
        String[] players;
        Quarto quatro;
        PlayerType iam = PlayerType.PlayerOne;

        public GreenGUI(Quarto _quatro)
        {
            InitializeComponent();

            quatro = _quatro;
            AbstractGame.StepHandler ost = new AbstractGame.StepHandler(OnStep);
            quatro.RegisterAsPlayer(ref ost);

            players = new String[2] { "You", "Opponent" };
            ShowNextEvent();
        }

        private void OnStep(IState state)
        {
            //state.GetAvailableSteps();
            Piece[] pa = quatro.getAviablePiece();
            Piece[,] paa = quatro.getAviableStep();
            Piece p = quatro.getSelectedPiece();
            bool w = quatro.getWinningState();
            bool m = quatro.getIsMyMove();

            //TODO everything
        }

        private void ShowNextEvent()
        {
            LbNextEvent.Content = players[turns] + " " + (selection == -1?"Choose":"Put");
        }

        private void BtChoose_Click(object sender, RoutedEventArgs e)
        {
            if (selection != -1) return;

            Button btn = (sender as Button);
            Int32 id = Convert.ToInt32(btn.Tag);
            selection = id;
            ImNext.Source = (btn.Content as Image).Source;
            btn.Visibility = System.Windows.Visibility.Hidden;

            //quatro.DoStep();

            turns = 1 - turns;
            ShowNextEvent();
        }

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

            QuartoStep qs = new QuartoStep(0, 4, new Piece(id));
            quatro.DoStep(qs, iam);

            ShowNextEvent();
        }

        public void AddToGame(PlayerType pt)
        {
            iam = pt;
        }
    }
}
