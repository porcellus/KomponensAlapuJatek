﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.GameBase;

namespace Game
{
    public class QuartoStep : GameBase.AbstractStep
    {
        private int x;

        private int y;

        public Piece p;



        public QuartoStep(int a, int b, Piece pie)
        {
            x = a;
            y = b;
            p = pie;
        }
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        public Piece P
        {
            get { return p; }
            set { p = value; }
        }
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
    }
}
