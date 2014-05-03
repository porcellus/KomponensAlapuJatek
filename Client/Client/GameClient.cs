using System;
using System.Collections.Generic;

namespace Client.Client
{
    public class GameClient : IClient
    {
        public IList<string> GetAvailableGameTypes()
        {
            throw new NotImplementedException();
        }

        public IList<int> GetGamesInLobby(string lobby)
        {
            throw new NotImplementedException();
        }

        public bool JoinGame(int gameId)
        {
            throw new NotImplementedException();
        }

        public bool ConnectToServer(string ip, string port)
        {
            throw new NotImplementedException();
        }

        public void CreateLocalGame(object aiAlgorithm)
        {
            throw new NotImplementedException();
        }

        public void StartNetworkGame(string gameType, object startPosition)
        {
            throw new NotImplementedException();
        }
    }
}
