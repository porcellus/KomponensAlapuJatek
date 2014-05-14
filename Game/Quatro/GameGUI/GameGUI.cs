using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Game.Quatro.GameGUI
{
    public class GameGUI
    {
        public UserControl getGameGui(Game.Quarto _quatro)
        {
            return new Game.QuatroGreenGameGUI.GreenGUI(_quatro);
        }
    }
}
