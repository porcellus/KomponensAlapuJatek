using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ClientGUI.View;

namespace ClientGUI.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        private string _selectedGame;
        private UserControl _selectedGameControl;
        public IList<string> GamesList { get; set; }

        public UserControl SelectedGameControl
        {
            get { return _selectedGameControl; }
            set
            {
                if (!Equals(value, _selectedGameControl))
                {
                    _selectedGameControl = value;
                    OnPropertyChanged("SelectedGameControl");
                }
            }
        }

        public string SelectedGame
        {
            get { return _selectedGame; }
            set
            {
                if (value != _selectedGame)
                {
                    _selectedGame = value;
                    SelectedGameControl = GetControlForSelectedGame();
                    OnPropertyChanged("SelectedGame");
                }
            }
        }

        private UserControl GetControlForSelectedGame()
        {
            switch (_selectedGame)
            {
                case "Chess":
                    return new ChessGame();
                case "Quatro":
                    return new QuatroGame();
                default:
                    return null;
            }
        }

        public MainViewModel()
        {
            GamesList = new List<string> {"Chess", "Quatro"};
        }
    }
}
