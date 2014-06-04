using System;
using Game.GameBase;

namespace ClientGUI.ViewModel
{
    internal class PlayerTypeViewModel : ViewModelBase
    {
        private readonly string _description;
        private readonly PlayerType _playerType;
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
                    _description = "MI";
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