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

        IList<int> GetGamesInLobby(string lobby);
        IList<string> GetAvailableAIAlgorithms();
        bool JoinGame(string opponentData);
        bool ConnectToServer(string ip, string port);
        
        void StartNetworkGame(string gameType, object startPosition);
    }
}
