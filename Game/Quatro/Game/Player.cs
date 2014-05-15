using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;
using Client.AIAlgorithmBase;
using Game.GameBase;

namespace Game
{
   public class Player
    {
       
        private PlayerType playerType;
        private IAIAlgorithm algorithm;
        private EntityType playerEntity;
        private GameBase.AbstractGame.StepHandler callback;

        public GameBase.AbstractGame.StepHandler Callback
        {
            get { return callback; }
            set { callback = value; }
        }
        public PlayerType PlayerType
        {
            get { return playerType; }
            set { playerType = value; }
        }
      

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