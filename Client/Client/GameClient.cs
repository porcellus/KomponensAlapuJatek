using System.Collections;
using System.Windows.Controls;
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

        private IDictionary<string, KeyValuePair<Func<AbstractGame>, Func<AbstractGameGUI>>> GameDict;
        private Dictionary<String, Type> AIAlgDict;
        
        public IList<string> GetAvailableGameTypes()
        {
            if (GameDict == null)
            {
                BuildGameTypeDict();
            }

            return GameDict.Keys.ToList();
        }
        
        public IAIAlgorithm GetAI(string aiType)
        {
            return Activator.CreateInstance(AIAlgDict[aiType]) as IAIAlgorithm;
        }

        public UserControl getGameGUI(AbstractGame game)
        {
            var gamegui =  GameDict[game.GetType().Name].Value();
            var gui = gamegui.GetGameGUI();
            gamegui.AddToGame(game, PlayerType.PlayerOne);
            return gui;
        }

        public AbstractGame CreateLocalGame(string gameName)
        {
            return GameDict[gameName].Key();
        }

        private void BuildGameTypeDict()
        {
            var gameDict = new Dictionary<string, Type>();
            var guiDict = new Dictionary<string, Type>();
            String path = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);

            DirectoryInfo dir = new DirectoryInfo(path);
            var files = dir.GetFiles("*.dll").ToList();
            files.RemoveAll(
                a =>
                a.Name == "MahApps.Metro.dll" || a.Name == "Rhino.Mocks.dll" ||
                a.Name == "System.Windows.Interactivity.dll");
            foreach (FileInfo fi in files)
            {
                Assembly ass = Assembly.LoadFrom(fi.Name);

                System.Diagnostics.Debug.WriteLine("Loaded "+ fi.DirectoryName + "\\" + fi.Name);

                System.Diagnostics.Debug.Indent();

                foreach (Type t in ass.GetTypes())
                {
                    System.Diagnostics.Debug.WriteLine("Checking " + t.FullName);
                    System.Diagnostics.Debug.Indent();

                    if (t.BaseType == null)
                    {
                        System.Diagnostics.Debug.Unindent();
                        continue;
                    }
                    if (t.IsSubclassOf(typeof(AbstractGame)) && t.GetConstructor(Type.EmptyTypes) != null)
                    {
                        gameDict.Add(t.Name, t);
                        System.Diagnostics.Debug.WriteLine("Added as game");
                    }
                    foreach (var iface in t.GetInterfaces())
                    {
                        System.Diagnostics.Debug.WriteLine("Interface: " + iface.Name);
                        System.Diagnostics.Debug.WriteLine(iface.Name + " IsGeneric: " + iface.IsGenericType);
                        if (iface.IsGenericType)
                        {
                            System.Diagnostics.Debug.WriteLine(iface.Name + " GenericTypeDefinition: " + iface.GetGenericTypeDefinition().Name);
                            foreach (var arg in iface.GetGenericArguments())
                            {
                                System.Diagnostics.Debug.WriteLine(iface.Name + " GenericArgument: " + arg.Name);
                            }
                            if (iface.GetGenericTypeDefinition() == typeof(GameGUI<>) && t.GetConstructor(Type.EmptyTypes) != null)
                            {
                                var gameType = iface.GetGenericArguments()[0];
                                guiDict.Add(gameType.Name, t);
                                System.Diagnostics.Debug.WriteLine("Added as gui");
                                break;
                            }
                        }
                    }
                    
                    System.Diagnostics.Debug.Unindent();
                }
                System.Diagnostics.Debug.Unindent();
            }
            GameDict = new Dictionary<string, KeyValuePair<Func<AbstractGame>, Func<AbstractGameGUI>>>();
            
            foreach (var game in gameDict)
            {
                System.Diagnostics.Debug.Write("Trying to add " + game.Key);
                if (guiDict.ContainsKey(game.Key))
                {
                    GameDict[game.Key] = new KeyValuePair<Func<AbstractGame>, Func<AbstractGameGUI>>(
                        () => Activator.CreateInstance(game.Value) as AbstractGame,
                        () =>
                        {
                            System.Diagnostics.Debug.WriteLine(game.Key + "->" + guiDict[game.Key].Name);
                            return Activator.CreateInstance(guiDict[game.Key]) as AbstractGameGUI;
                        });
                    System.Diagnostics.Debug.WriteLine(", and found gui, added.");
                } else System.Diagnostics.Debug.WriteLine(", but no gui, not added.");
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
