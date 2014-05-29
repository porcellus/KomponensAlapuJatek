using Client.AIAlgorithmBase;
using Game;
using Game.GameBase;
using System;
using System.Collections.Generic;
using Client;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Reflection;
using System.IO;

namespace Client.Client
{
    public class GameClient : IClient
    {
        private Server.Connector _Connector;
        private List<Int32> lobbyList;

        private Dictionary<String, Type> GameDict;
        private Dictionary<String, Type> AIAlgDict;

        public IList<string> GetAvailableGameTypes()
        {

            if (GameDict == null)
            {
                BuildGameTypeDict();
            }
            return GameDict.Keys.ToList();
        }

        private void BuildGameTypeDict()
        {
            GameDict = new Dictionary<String, Type>();
            String path = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);

            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] files = dir.GetFiles("*.dll");
            foreach (FileInfo fi in files)
            {
                Assembly ass = Assembly.LoadFrom(fi.Name);
                foreach (Type t in ass.GetTypes())
                {
                    if (t.IsSubclassOf(typeof(AbstractGame)) && t.GetConstructor(Type.EmptyTypes) != null)
                    {
                        GameDict.Add(t.Name, t);
                    }
                }
            }
        }

        public IList<string> GetAvailableAIAlgorithms()
        {

            if (AIAlgDict == null)
            {
                BuildAIAlgDict();
            }
            return AIAlgDict.Keys.ToList();
        }

        private void BuildAIAlgDict()
        {
            AIAlgDict = new Dictionary<String, Type>();
            String path = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);

            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] dlls = dir.GetFiles("*.dll");
            foreach (FileInfo fi in dlls)
            {
                Assembly ass = Assembly.LoadFrom(fi.Name);
                foreach (Type t in ass.GetTypes())
                {
                    if (t.GetInterfaces().Contains(typeof(IAIAlgorithm)))
                    {
                        AIAlgDict.Add(t.Name, t);
                    }
                }
            }
        }

        public bool ConnectToServer(string ip, string port)
        {
            _Connector = new Server.Connector();

            _Connector.connectoServer(ip, Int32.Parse(port), "name", "Chess");

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
        {
            Type gameType;
            gameType = GameDict[gameName];
            AbstractGame game = Activator.CreateInstance(gameType) as AbstractGame;


            Type algType;
            algType = GameDict[aiAlgorithm];
            IAIAlgorithm aiAlg = Activator.CreateInstance(algType) as IAIAlgorithm;
            aiAlg.AddToGame(game, PlayerType.PlayerTwo);
        }

        public void StartNetworkGame(string gameType, object startPosition)
        {
            throw new NotImplementedException();
        }
    }
}
