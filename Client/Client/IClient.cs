using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Game.GameBase;

namespace Client.Client
{
    public interface IClient
    {
        IList<string> GetAvailableGameTypes();
        IList<string> GetAvailableAIAlgorithms();

        AIAlgorithmBase.IAIAlgorithm GetAI(string aiType);
        UserControl getGameGUI(AbstractGame game);
        Game.GameBase.AbstractGame CreateLocalGame(string gameName);

        event EventHandler LoadGame;

        void StartServer();
        IList<string> GetGamesInLobby(string lobby);
        bool JoinGame(string opponentData);
        bool ConnectToServer(string ip, string port);
        bool ConnectToServer(string ip, string port, string playerName, string gameType);

        AbstractGame StartNetworkGame(string gameType, PlayerType playerType);
    }
}
