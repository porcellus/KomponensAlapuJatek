using System.Windows.Controls;
using Client.AIAlgorithmBase;
using Game.GameBase;
using System;
using System.Collections.Generic;
using System.Linq;
using GameTypeManager;
using System.Threading;
using System.Windows;

namespace Client.Client
{
    public class GameClient : IClient
    {
        private Server.Connector clientConnector;
        private Server.Server _Server;
        private IDictionary<string, GamePair> GameDict;
        private Dictionary<String, Type> AIAlgDict;

        public event EventHandler LoadGame;

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
            var gamegui = GameDict[game.GetType().Name].GameGuiFunc();
            var gui = gamegui.GetGameGUI();
            gamegui.AddToGame(game, PlayerType.PlayerOne);
            return gui;
        }

        public AbstractGame CreateLocalGame(string gameName)
        {
            return GameDict[gameName].GameFunc();
        }

        public IList<string> GetAvailableAIAlgorithms()
        {

            if (AIAlgDict == null)
            {
                AIAlgDict = GameTypeManager.GameTypeManager.GetInstance().GetAIAlgDict();
            }
            return AIAlgDict.Keys.ToList();
        }

        public void StartServer()
        {
            _Server = new Server.Server();
            _Server.stratServer("127.0.0.1", 8888);
        }

        public bool ConnectToServer(string ip, string port)
        {
            clientConnector = new Server.Connector();
            clientConnector.connectionAcceptEventHandler += new Connector.ConnectionAcceptEventHandler(ConnectionAccept);
            clientConnector.connectionRequestEventHandler += new Connector.ConnectionRequestEventHandler(ConnectionRequest);
            clientConnector.stepEventHandler += new Connector.StepEventHandler(OnStep);


            Thread connectorThread = new Thread(() =>
            {
                clientConnector.connectoServer(ip, Int32.Parse(port), "name", "Quarto");
            });

            connectorThread.Start();

            return true;
        }

        public bool ConnectToServer(string ip, string port, string playerName, string gameType)
        {
            clientConnector = new Server.Connector();
            clientConnector.connectionAcceptEventHandler += new Connector.ConnectionAcceptEventHandler(ConnectionAccept);
            clientConnector.connectionRequestEventHandler += new Connector.ConnectionRequestEventHandler(ConnectionRequest);
            clientConnector.stepEventHandler += new Connector.StepEventHandler(OnStep);


            Thread connectorThread = new Thread(() =>
            {
                clientConnector.connectoServer(ip, Int32.Parse(port), playerName, gameType);
            });

            connectorThread.Start();

            return true;
        }

        private void ConnectionRequest(object sender, Connector.ConnectionRequestEventArgs e)
        {
            if (MessageBox.Show(Convert.ToString(clientConnector.playerip) + ": Elfogadja a kapcsolódást " + e.PlayerName + " játékostól?", "Hálózati játék?", MessageBoxButton.YesNo)
                == MessageBoxResult.Yes)
            {
                clientConnector.sendRequestreply("Accept");

                LoadGame(this, new EventArgs());
            }
            else
            {
                clientConnector.sendRequestreply("Decline");
            }

        }

        private void ConnectionAccept(object sender, Connector.ConnectionAcceptEventArgs e)
        {
            MessageBox.Show(Convert.ToString(clientConnector.playerip) + ": A kapott válasz: " + e.Result);
        }

        public IList<string> GetGamesInLobby(string gameType)
        {
            try
            {
                return clientConnector.getLobby(gameType).ToList();
            }
            catch
            {
                return new List<string>();
            }
        }

        public bool JoinGame(String opponentData)
        {
            string[] data = opponentData.Split(',');
            clientConnector.requestGame(data[1], Int32.Parse(data[2]), data[0]);

            return true;

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

        private void OnStep(object sender, Connector.StepEventArgs e)
        {
            currentGame.DoStep((AbstractStep)e.Step, otherPlayerType);
        }

        public void StepHandler(IState state)
        {
            Console.WriteLine("Stephandler");
        }

        public void SendStep(AbstractStep step)
        {
            clientConnector.step(step);
        }

        public AbstractGame currentGame;
        public UserControl currentGameGUI;
        public PlayerType currentPlayerType;
        public PlayerType otherPlayerType;

        public AbstractGame StartNetworkGame(string gameType, PlayerType playerType)
        {
            currentGame = GameDict[gameType].GameFunc();
            var gamegui = GameDict[currentGame.GetType().Name].GameGuiFunc();
            currentGameGUI = gamegui.GetGameGUI();
            gamegui.AddToGame(currentGame, playerType);

            currentPlayerType = playerType;
            otherPlayerType = (playerType == PlayerType.PlayerOne) ? PlayerType.PlayerTwo : PlayerType.PlayerOne;

            AbstractGame.StepHandler stepHandler = StepHandler;
            currentGame.RegisterAsPlayer(ref stepHandler, otherPlayerType);

            return currentGame;
        }
    }
}
