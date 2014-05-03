using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Client;

namespace ClientGUI.ViewModel
{
    internal class SelectorViewModel : ViewModelBase
    {
        public enum GameType
        {
            ONLINE,
            OFFLINE
        }

        private readonly IClient _client;
        private IList<int> _availableLobbies;
        private IList<string> _gamesList;
        private bool _isConnectedToServer;
        private bool _isSelectorVisible;
        private string _selectedGame;
        private string _selectedLobby;

        public SelectorViewModel(IClient client)
        {
            _client = client;
            IsSelectorVisible = false;
            GamesList = _client.GetAvailableGameTypes();
            SetupCommands();
        }

        public RelayCommand PerformSelectionCommand { get; private set; }
        public RelayCommand CloseSelectorWindowCommand { get; private set; }

        public IList<GameType> GameTypes
        {
            get { return new[] {GameType.OFFLINE, GameType.ONLINE}; }
        }

        public GameType SelectedGameType { get; set; }

        public IList<string> GamesList
        {
            get { return _gamesList; }
            set
            {
                if (!Equals(_gamesList, value))
                {
                    _gamesList = value;
                    OnPropertyChanged("GamesList");
                }
            }
        }

        public IList<int> AvailableLobbies
        {
            get { return _availableLobbies; }
            set
            {
                if (!Equals(_availableLobbies, value))
                {
                    _availableLobbies = value;
                    OnPropertyChanged("AvailableLobbies");
                }
            }
        }

        public bool CanSelectLobby
        {
            get { return !string.IsNullOrEmpty(SelectedGame); }
        }

        public string ErrorMessage { get; set; }

        public string SelectedLobby
        {
            get { return _selectedLobby; }
            set
            {
                if (_selectedLobby != value)
                {
                    _selectedLobby = value;
                    PerformSelectionCommand.RaiseCanExecuteChanged();
                    OnPropertyChanged("SelectedLobby");
                }
            }
        }

        public string SelectedGame
        {
            get { return _selectedGame; }
            set
            {
                if (_selectedGame != value)
                {
                    _selectedGame = value;
                    Task<IList<int>>.Factory.StartNew(() => _client.GetGamesInLobby(SelectedGame))
                        .ContinueWith(res =>
                        {
                            if (res.Exception == null)
                            {
                                AvailableLobbies = res.Result;
                            }
                            else
                            {
                                ErrorMessage = res.Exception.Flatten().ToString();
                                RaiseErrorOccured();
                            }
                        });
                    PerformSelectionCommand.RaiseCanExecuteChanged();
                    OnPropertyChanged("CanSelectLobby");
                    OnPropertyChanged("SelectedGame");
                }
            }
        }

        public bool IsSelectorVisible
        {
            get { return _isSelectorVisible; }
            set
            {
                if (_isSelectorVisible != value)
                {
                    _isSelectorVisible = value;
                    OnPropertyChanged("IsSelectorVisible");
                }
            }
        }

        public bool IsConnectedToServer
        {
            get { return _isConnectedToServer; }
            set
            {
                if (_isConnectedToServer != value)
                {
                    _isConnectedToServer = value;
                    OnPropertyChanged("IsConnectedToServer");
                }
            }
        }

        public event EventHandler CreateGame;
        public event EventHandler ErrorOccured;

        private void SetupCommands()
        {
            PerformSelectionCommand = new RelayCommand(x => PerformSelection(), x => CanPerformSelectionExecute());
            CloseSelectorWindowCommand = new RelayCommand(x => CloseSelector());
        }

        private bool CanPerformSelectionExecute()
        {
            return (CanSelectLobby && !string.IsNullOrEmpty(SelectedLobby)) ||
                   (!IsConnectedToServer && !string.IsNullOrEmpty(SelectedGame));
        }

        private void PerformSelection()
        {
            CreateGame(this, new EventArgs());
            IsSelectorVisible = false;
        }

        private void RaiseErrorOccured()
        {
            ErrorOccured(this, new EventArgs());
        }

        private void CloseSelector()
        {
            IsSelectorVisible = false;
        }
    }
}