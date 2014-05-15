// Author: Zsolt Turi (ITUPBD)
// Email: tuzraai at inf.elte.hu
// Date: 2014.05.15.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Game;
using Game.GameBase;

namespace Game.Quatro.GameGUI
{
    //GameGUI - the main reason of this class is to give back a new game gui user control
    public class GameGUI
    {
        QuatroGreenGameGUI.GreenGUI gui;

        //returns a new GameGui
        //_quatro: a quatro object
        public UserControl getGameGui(Game.Quarto _quatro)
        {
            gui = new Game.QuatroGreenGameGUI.GreenGUI(_quatro);
            return gui;
        }

        // Sets the UserControl to player one or two
        // game: nothing to do
        // pt: player one or two
        public void AddToGame(object game, PlayerType pt)
        {
            gui.AddToGame(pt);
        }
    }
}
