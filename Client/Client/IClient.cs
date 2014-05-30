using System.Collections.Generic;

namespace Client.Client
{
    public interface IClient
    {
        IList<string> GetAvailableGameTypes();
        IList<int> GetGamesInLobby(string lobby);
        IList<string> GetAvailableAIAlgorithms();
        bool JoinGame(string opponentData);
        bool ConnectToServer(string ip, string port);
        void CreateLocalGame(string gameName, string aiAlgorithm, object playerPosition);
        void StartNetworkGame(string gameType, object startPosition);
    }
}
