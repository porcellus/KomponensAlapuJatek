﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Client.AIAlgorithmBase;
using Game.GameBase;

namespace GameTypeManager
{
    public class GamePair
    {
        public Func<AbstractGame> GameFunc { get; set; }
        public Func<AbstractGameGUI> GameGuiFunc { get; set; }
        public GamePair(Func<AbstractGame> gameFunc, Func<AbstractGameGUI> gameGuiFunc)
        {
            GameFunc = gameFunc;
            GameGuiFunc = gameGuiFunc;
        }
    }
    public class GameTypeManager
    {
        private GameTypeManager() {}
        private static GameTypeManager instance;
        public static GameTypeManager GetInstance()
        {
            if (instance == null)
            {
                instance = new GameTypeManager();
            }
            return instance;
        }
        public Dictionary<String, Type> GetAIAlgDict()
        {
            Dictionary<String, Type> aiAlgDict = new Dictionary<String, Type>();
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
                        aiAlgDict.Add(t.Name, t);
                    }
                }
            }
            return aiAlgDict;
        }

        public IDictionary<string, GamePair> GetGameTypeDict()
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
            // iterate over files in actual directory
            foreach (FileInfo fi in files)
            {
                Assembly ass = Assembly.LoadFrom(fi.Name);

                //System.Diagnostics.Debug.WriteLine("Loaded "+ fi.DirectoryName + "\\" + fi.Name);

                //System.Diagnostics.Debug.Indent();
                // for each class
                foreach (Type t in ass.GetTypes())
                {
                    //System.Diagnostics.Debug.WriteLine("Checking " + t.FullName);
                    //System.Diagnostics.Debug.Indent();

                    if (t.BaseType == null)
                    {
                        //System.Diagnostics.Debug.Unindent();
                        continue;
                    }
                    if (t.IsSubclassOf(typeof(AbstractGame)) && t.GetConstructor(Type.EmptyTypes) != null)
                    {
                        gameDict.Add(t.Name, t);
                        //System.Diagnostics.Debug.WriteLine("Added as game");
                    }
                    
                    foreach (var iface in t.GetInterfaces())
                    {
                        //System.Diagnostics.Debug.WriteLine("Interface: " + iface.Name);
                        //System.Diagnostics.Debug.WriteLine(iface.Name + " IsGeneric: " + iface.IsGenericType);
                        if (iface.IsGenericType)
                        {
                            //System.Diagnostics.Debug.WriteLine(iface.Name + " GenericTypeDefinition: " + iface.GetGenericTypeDefinition().Name);
                            /*foreach (var arg in iface.GetGenericArguments())
                            {
                                System.Diagnostics.Debug.WriteLine(iface.Name + " GenericArgument: " + arg.Name);
                            }*/
                            if (iface.GetGenericTypeDefinition() == typeof(GameGUI<>) && t.GetConstructor(Type.EmptyTypes) != null)
                            {
                                var gameType = iface.GetGenericArguments()[0];
                                guiDict.Add(gameType.Name, t);
                                System.Diagnostics.Debug.WriteLine("Added as gui");
                                break;
                            }
                        }
                    }
                    
                    //System.Diagnostics.Debug.Unindent();
                }
                //System.Diagnostics.Debug.Unindent();
            }
            var result = new Dictionary<string, GamePair>();
            
            foreach (var game in gameDict)
            {
                System.Diagnostics.Debug.Write("Trying to add " + game.Key);
                if (guiDict.ContainsKey(game.Key))
                {
                    result[game.Key] = new GamePair(
                        () => Activator.CreateInstance(game.Value) as AbstractGame,
                        () =>
                            {
                                //System.Diagnostics.Debug.WriteLine(game.Key + "->" + guiDict[game.Key].Name);
                                return Activator.CreateInstance(guiDict[game.Key]) as AbstractGameGUI;
                            }
                    );
                    //System.Diagnostics.Debug.WriteLine(", and found gui, added.");
                } //else System.Diagnostics.Debug.WriteLine(", but no gui, not added.");
            }
            return result;
        }

    }
}
