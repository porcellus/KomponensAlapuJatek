using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameBase;

namespace GameTypeManager
{
    public class GameTypeManager
    {
        IEnumerable<AbstractGame> GetGameTypes()
        {
            var instances = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from t in assembly.GetTypes()
                where t.GetInterfaces().Contains(typeof (AbstractGame)) && t.GetConstructor(Type.EmptyTypes) != null
                select Activator.CreateInstance(t) as AbstractGame;
            return instances;
        }
    }
}
