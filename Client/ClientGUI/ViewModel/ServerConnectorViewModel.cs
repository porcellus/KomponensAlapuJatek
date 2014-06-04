using System;
using System.Threading.Tasks;
using Client.Client;

namespace ClientGUI.ViewModel
{
    internal class ServerConnectorViewModel : ViewModelBase
    {
        //private const string DEFAULT_IP = "192.168.1.110";
        private const string DEFAULT_IP = "127.0.0.1";
        private const string DEFAULT_PORT = "8888";
        private readonly IClient _client;
        private bool _isConnected;
        private bool _isConnectionWindowVisible;
        private bool _isLoading;

        public ServerConnectorViewModel(IClient client)
        {
            IsLoading = false;
            IsConnectionWindowVisible = true;
            _client = client;
            IP = DEFAULT_IP;
            Port = DEFAULT_PORT;
            SetupCommands();
        }

        public string IP { get; set; }
        public string Port { get; set; }

        public RelayCommand ConnectCommand { get; private set; }
        public RelayCommand PlayOfflineCommand { get; private set; }
        public RelayCommand StartServerCommand { get; private set; }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged("IsLoading");
                }
            }
        }

        public bool IsConnected
        {
            get { return _isConnected; }
            private set
            {
                _isConnected = value;
                OnPropertyChanged("IsConnected");
            }
        }

        public bool IsConnectionWindowVisible
        {
            get { return _isConnectionWindowVisible; }
            set
            {
                if (_isConnectionWindowVisible != value)
                {
                    _isConnectionWindowVisible = value;
                    OnPropertyChanged("IsConnectionWindowVisible");
                }
            }
        }

        public event EventHandler ConnectionError;


        private void PlayOffline()
        {
            IsConnectionWindowVisible = false;
            IsConnected = false;
        }

        private void ConnectToServer()
        {
            IsLoading = true;
            Task<bool>.Factory.StartNew(() => _client.ConnectToServer(IP, Port))
                .ContinueWith(res =>
                {
                    if (res.Exception == null)
                    {
                        IsLoading = false;
                        if (res.Result)
                        {
                            IsConnectionWindowVisible = false;
                            IsConnected = true;
                        }
                        else
                        {
                            ConnectionError(this, new EventArgs());
                        }
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void SetupCommands()
        {
            ConnectCommand = new RelayCommand(x => ConnectToServer());
            PlayOfflineCommand = new RelayCommand(x => PlayOffline());
            StartServerCommand = new RelayCommand(x => _client.StartServer());
        }
    }
}