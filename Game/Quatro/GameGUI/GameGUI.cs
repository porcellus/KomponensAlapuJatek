using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Game;
using Game.GameBase;

namespace Game.Quatro.GameGUI
{
    public class GameGUI
    {
        QuatroGreenGameGUI.GreenGUI gui;

        public UserControl getGameGui(Game.Quarto _quatro)
        {
            gui = new Game.QuatroGreenGameGUI.GreenGUI(_quatro);
            return gui;
        }

        public void AddToGame(object game, PlayerType pt)
        {
            gui.AddToGame(pt);
        }
    }
}
