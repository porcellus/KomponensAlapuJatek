using System;
using System.ComponentModel;
using Game.GameBase;

namespace ClientGUI.ViewModel
{
    class PlayerTypeViewModel : ViewModelBase
    {
        private readonly PlayerType _playerType;
        private readonly string _description;
        private bool _isAvailable;

        public PlayerTypeViewModel(PlayerType playerType)
        {
            _playerType = playerType;
            IsAvailable = true;
            switch (playerType)
            {
                case PlayerType.PlayerOne:
                    _description = "Első játékos";
                    break;
                case PlayerType.PlayerTwo:
                    _description = "Második játékos";
                    break;
                case PlayerType.Observer:
                    _description = "Néző";
                    break;
                case PlayerType.NoOne:
                    _description = "Üres";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("playerType");
            }
        }

        public bool IsAvailable
        {
            get { return _isAvailable; }
            set
            {
                _isAvailable = value;
                OnPropertyChanged("IsAvailable");
            }
        }

        public string Description
        {
            get { return _description; }
        }

        public PlayerType PlayerType
        {
            get { return _playerType; }
        }
    }
}
