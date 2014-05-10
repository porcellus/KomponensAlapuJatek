using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClientGUI.View
{
    /// <summary>
    /// Interaction logic for QuatroGame.xaml
    /// </summary>
    public partial class QuatroGame : UserControl
    {
        Int32 selection = -1;
        Int32 turns = 0;
        String[] players;

        public QuatroGame()
        {
            InitializeComponent();

            players = new String[2] { "Peter", "Paul" };
            ShowNextEvent();
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

            ShowNextEvent();
        }
    }
}
