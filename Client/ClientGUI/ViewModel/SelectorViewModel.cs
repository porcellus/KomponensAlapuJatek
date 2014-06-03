using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using Client.Client;
using Game.GameBase;

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
        private IList<string> _availableLobbies;
        private ObservableCollection<PlayerTypeViewModel> _availablePlayerTypesList;
        private PlayerTypeViewModel _firstPlayerType;
        private IList<string> _gamesList;
        private bool _hasHumanPlayer;
        private IList<string> _heuristicsList;

        private bool _isConnectedToServer;
        private bool _isSelectorVisible;
        private PlayerTypeViewModel _secondPlayerType;
        private string _selectedGame;
        private GameType _selectedGameType;
        private string _selectedHeuristic;
        private string _selectedLobby;


        public SelectorViewModel(IClient client)
        {
            _client = client;
            IsSelectorVisible = false;
            GamesList = _client.GetAvailableGameTypes();
            HeuristicsList = _client.GetAvailableAIAlgorithms();
            SetupCommands();
            ResetAvailablePlayerTypes();
            SelectedGameType = GameType.OFFLINE;
            FirstPlayerType = AvailablePlayerTypesList[0]; //initialize to noone
            SecondPlayerType = AvailablePlayerTypesList[0];
            _client.LoadGame += OnLoadGame;
        }


        public RelayCommand PerformSelectionCommand { get; private set; }
        public RelayCommand CloseSelectorWindowCommand { get; private set; }

        public IList<GameType> GameTypes
        {
            get { return new[] {GameType.OFFLINE, GameType.ONLINE}; }
        }

        public GameType SelectedGameType
        {
            get { return _selectedGameType; }
            set
            {
                if (_selectedGameType != value)
                {
                    _selectedGameType = value;
                    OnPropertyChanged("SelectedGameType");
                    OnPropertyChanged("IsOnlineGame");
                    PerformSelectionCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public bool IsOnlineGame
        {
            get { return IsConnectedToServer && SelectedGameType == GameType.ONLINE; }
        }

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

        public IList<string> HeuristicsList
        {
            get { return _heuristicsList; }
            set
            {
                if (_heuristicsList != value)
                {
                    _heuristicsList = value;
                    OnPropertyChanged("HeuristicsList");
                }
            }
        }

        public ObservableCollection<PlayerTypeViewModel> AvailablePlayerTypesList
        {
            get { return _availablePlayerTypesList; }
            set
            {
                if (_availablePlayerTypesList != value)
                {
                    _availablePlayerTypesList = value;
                    OnPropertyChanged("AvailablePlayerTypesList");
                }
            }
        }

        public string SelectedHeuristic
        {
            get { return _selectedHeuristic; }
            set
            {
                if (_selectedHeuristic != value)
                {
                    _selectedHeuristic = value;
                    OnPropertyChanged("SelectedHeuristic");
                    PerformSelectionCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public PlayerTypeViewModel FirstPlayerType
        {
            get { return _firstPlayerType; }
            set
            {
                if (_firstPlayerType != value)
                {
                    UpdatePlayerTypes(_firstPlayerType, value);
                    _firstPlayerType = value;
                    OnPropertyChanged("FirstPlayerType");
                    OnPropertyChanged("AvailablePlayerTypesList");
                    PerformSelectionCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public PlayerTypeViewModel SecondPlayerType
        {
            get { return _secondPlayerType; }
            set
            {
                if (_secondPlayerType != value)
                {
                    UpdatePlayerTypes(_secondPlayerType, value);
                    _secondPlayerType = value;
                    OnPropertyChanged("SecondPlayerType");
                    OnPropertyChanged("AvailablePlayerTypesList");
                    PerformSelectionCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public IList<string> AvailableLobbies
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
                    Task<IList<string>>.Factory.StartNew(() => _client.GetGamesInLobby(SelectedGame))
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
                    if (value)
                    {
                        SelectedGame = null;
                        SelectedLobby = null;
                        SelectedHeuristic = null;
                    }
                    _isSelectorVisible = value;
                    OnPropertyChanged("IsSelectorVisible");
                }
            }
        }

        public bool HasHumanPlayer
        {
            get { return _hasHumanPlayer; }
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

        private void OnLoadGame(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(
                new Action(() =>
                {
                    CreateNetworkGame(this, new EventArgs());
                    CloseSelector();
                }));
        }

        private void ResetAvailablePlayerTypes()
        {
            if (AvailablePlayerTypesList == null)
            {
                AvailablePlayerTypesList = new ObservableCollection<PlayerTypeViewModel>
                {
                    new PlayerTypeViewModel(PlayerType.NoOne),
                    new PlayerTypeViewModel(PlayerType.Observer),
                    new PlayerTypeViewModel(PlayerType.PlayerOne),
                    new PlayerTypeViewModel(PlayerType.PlayerTwo)
                };
            }
            else
            {
                AvailablePlayerTypesList[2].IsAvailable = true; // player one
                AvailablePlayerTypesList[3].IsAvailable = true; // player two
            }
        }

        private void RemoveActualPlayersFromAvailable()
        {
            AvailablePlayerTypesList[2].IsAvailable = false; // player one
            AvailablePlayerTypesList[3].IsAvailable = false; // player two
        }

        private void UpdatePlayerTypes(PlayerTypeViewModel current, PlayerTypeViewModel selected)
        {
            if (IsHumanPlayer(current) && !IsHumanPlayer(selected))
            {
                _hasHumanPlayer = false;
                ResetAvailablePlayerTypes();
            }
            else if (IsHumanPlayer(selected))
            {
                _hasHumanPlayer = true;
                RemoveActualPlayersFromAvailable();
            }
        }

        private bool IsHumanPlayer(PlayerTypeViewModel current)
        {
            return current != null &&
                   (current.PlayerType == PlayerType.PlayerOne || current.PlayerType == PlayerType.PlayerTwo);
        }

        public event EventHandler CreateGame;
        public event EventHandler CreateNetworkGame;
        public event EventHandler JoinNetworkGame;
        public event EventHandler ErrorOccured;

        private void SetupCommands()
        {
            PerformSelectionCommand = new RelayCommand(x => PerformSelection(), x => CanPerformSelectionExecute());
            CloseSelectorWindowCommand = new RelayCommand(x => CloseSelector());
        }

        private bool CanPerformSelectionExecute()
        {
            return CanStartOnlineGame() || CanStartOfflineGame();
        }

        private bool CanStartOfflineGame()
        {
            return (!IsConnectedToServer && !string.IsNullOrEmpty(SelectedGame) &&
                    !string.IsNullOrEmpty(SelectedHeuristic)) ||
                   SelectedGameType == GameType.OFFLINE && !string.IsNullOrEmpty(SelectedGame) &&
                   !string.IsNullOrEmpty(SelectedHeuristic);
        }

        private bool CanStartOnlineGame()
        {
            return CanSelectLobby && !string.IsNullOrEmpty(SelectedLobby);
        }

        private void PerformSelection()
        {
            if (SelectedGameType == GameType.ONLINE)
            {
                if (_client.JoinGame(SelectedLobby))
                {
                    JoinNetworkGame(this, new EventArgs());
                }
            }

            else
            {
                CreateGame(this, new EventArgs());
            }
            CloseSelector();
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