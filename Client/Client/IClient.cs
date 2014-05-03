using System.Collections.Generic;

namespace Client.Client
{
    public interface IClient
    {
        IList<string> GetAvailableGameTypes();
        IList<int> GetGamesInLobby(string lobby);
        bool JoinGame(int gameId);
        bool ConnectToServer(string ip, string port);
        void CreateLocalGame(object aiAlgorithm);
        void StartNetworkGame(string gameType, object startPosition);
    }
}
