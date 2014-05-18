﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using Client.Client;
using ClientGUI.Model;
using ClientGUI.View;

namespace ClientGUI.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        private readonly IClient _client;
        private string _errorMessage;
        private IList<string> _gamesList;
        private bool _isConnected;
        private bool _isErrorVisible;
        private string _selectedGame;
        private UserControl _selectedGameControl;

        public MainViewModel()
        {
            _client = ClientFactory.CreateClient(ClientFactory.ClientType.MOCK);
            GamesList = _client.GetAvailableGameTypes();
            SetupViewModels();
            SetupCommands();
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

        public SelectorViewModel SelectorViewModel { get; set; }
        public ServerConnectorViewModel ServerConnectorViewModel { get; set; }
        public RelayCommand StartNewGameCommand { get; private set; }
        public RelayCommand CloseErrorCommand { get; private set; }

        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                SelectorViewModel.IsConnectedToServer = IsConnected;
                OnPropertyChanged("IsConnected");
            }
        }

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

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                IsErrorVisible = true;
                OnPropertyChanged("ErrorMessage");
            }
        }

        public bool IsErrorVisible
        {
            get { return _isErrorVisible; }
            set
            {
                if (_isErrorVisible != value)
                {
                    _isErrorVisible = value;
                    OnPropertyChanged("IsErrorVisible");
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

        private void SetupViewModels()
        {
            SelectorViewModel = new SelectorViewModel(_client);
            SelectorViewModel.CreateGame += OnCreateGame;
            SelectorViewModel.ErrorOccured += OnErrorOccuredInSelector;
            ServerConnectorViewModel = new ServerConnectorViewModel(_client);
            ServerConnectorViewModel.ConnectionError += OnConnectionError;
            ServerConnectorViewModel.PropertyChanged += OnServerConnectorPropertyChanged;
        }

        private void OnServerConnectorPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "IsConnected")
            {
                IsConnected = ServerConnectorViewModel.IsConnected;
            }
        }

        private void OnConnectionError(object sender, EventArgs e)
        {
            ErrorMessage = "Connection to server failed";
        }

        private void OnErrorOccuredInSelector(object sender, EventArgs e)
        {
            ErrorMessage = SelectorViewModel.ErrorMessage;
        }

        private void OnCreateGame(object sender, EventArgs e)
        {
            SelectedGameControl = GetControlForSelectedGame();
        }

        private void SetupCommands()
        {
            StartNewGameCommand = new RelayCommand(x => StartNewGame());
            CloseErrorCommand = new RelayCommand(x => CloseErrorWindow());
        }

        private void CloseErrorWindow()
        {
            IsErrorVisible = false;
        }

        private void StartNewGame()
        {
            SelectorViewModel.IsSelectorVisible = true;
        }

        private UserControl GetControlForSelectedGame()
        {
            switch (SelectorViewModel.SelectedGame)
            {
                case "Chess":
                    return new ChessGame();
                case "Quatro":
                    return new QuatroGame();
                default:
                    return null;
            }
        }
    }
}