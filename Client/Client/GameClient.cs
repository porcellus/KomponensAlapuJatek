using System.Windows.Controls;
using Client.AIAlgorithmBase;
using Game.GameBase;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Client.Client
{
    public class GameClient : IClient
    {
        private Server.Connector _Connector;
        private List<Int32> lobbyList;
        private IDictionary<string, KeyValuePair<Func<AbstractGame>, Func<AbstractGameGUI>>> GameDict;
        private Dictionary<String, Type> AIAlgDict;

        public IList<string> GetAvailableGameTypes()
        {
            if (GameDict == null)
            {
                GameDict = GameTypeManager.GameTypeManager.GetInstance().GetGameTypeDict();
            }

            return GameDict.Keys.ToList();
        }

        public IAIAlgorithm GetAI(string aiType)
        {
            return Activator.CreateInstance(AIAlgDict[aiType]) as IAIAlgorithm;
        }

        public UserControl getGameGUI(AbstractGame game)
        {
            var gamegui = GameDict[game.GetType().Name].Value();
            var gui = gamegui.GetGameGUI();
            gamegui.AddToGame(game, PlayerType.PlayerOne);
            return gui;
        }

        public AbstractGame CreateLocalGame(string gameName)
        {
            return GameDict[gameName].Key();
        }

        public IList<string> GetAvailableAIAlgorithms()
        {

            if (AIAlgDict == null)
            {
                AIAlgDict = GameTypeManager.GameTypeManager.GetInstance().GetAIAlgDict();
            }
            return AIAlgDict.Keys.ToList();
        }

  

        public bool ConnectToServer(string ip, string port)
        {
            _Connector = new Server.Connector();

            _Connector.connectoServer(ip, Int32.Parse(ip), "name", "Chess");

            return true;
        }

        public IList<int> GetGamesInLobby(string gameType)
        {
            return new[] {1,2,3,4,5};
        }

        public bool JoinGame(string opponentData)
        {
            string[] data = opponentData.Split(',');
            return _Connector.requestGame(data[1], Int32.Parse(data[2]), data[0]);
        }

        public void CreateLocalGame(string gameName, string aiAlgorithm, object playerPosition)
        {/*
            Type gameType;
            gameType = GameDict[gameName];
            AbstractGame game = Activator.CreateInstance(gameType) as AbstractGame;


            Type algType;
            algType = GameDict[aiAlgorithm];
            IAIAlgorithm aiAlg = Activator.CreateInstance(algType) as IAIAlgorithm;
            aiAlg.AddToGame(game, PlayerType.PlayerTwo);*/
        }

        public void StartNetworkGame(string gameType, object startPosition)
        {
            throw new NotImplementedException();
        }
    }
}
