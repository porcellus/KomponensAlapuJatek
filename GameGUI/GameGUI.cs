using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Game;
using Game.GameBase;

namespace GameGUI
{
    //GameGUI - the main reason of this class is to give back a new game gui user control
    public class GameGUI : AbstractGameGUI, GameGUI<Chess>
    {
        View.MainWindow gui;
        //returns a new GameGui
        //_chess: a chess object

        // Sets the UserControl to player one or two
        // game: nothing to do
        // pt: player one or two

        public override void AddToGame(AbstractGame game, PlayerType pt)
        {
            if (game is Chess)
                gui.AddToGame((Chess)game, pt);
            else throw new Exception("Bad game type");
        }

        public override Control GetGameGUI()
        {
            gui = new View.MainWindow();
            return gui;
        }

    }
}
