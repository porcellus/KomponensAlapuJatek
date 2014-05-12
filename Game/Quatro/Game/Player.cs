using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Client.AIAlgorithmBase;
using Game.GameBase;

namespace Game
{
   public class Player
    {
       
        private PlayerType playerType;
        private IAIAlgorithm algorithm;

        public PlayerType PlayerType
        {
            get { return playerType; }
            set { playerType = value; }
        }
        private EntityType playerEntity;

        public EntityType PlayerEntity
        {
            get { return playerEntity; }
            set { playerEntity = value; }
        }
      

        public IAIAlgorithm Algorithm
        {
            get { return algorithm; }
            set { algorithm = value; }
        }
       
    }
}