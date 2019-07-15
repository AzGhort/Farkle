using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Farkle
{
    class GameState
    {
        public Dice[] Dies = new Dice[6];
        
        public int Score = 0;

        public int Attempt = 0;
        public bool Scorable => Score >= 350;

        public GameState()
        {
            for (int i = 0; i < 6; i++)
            {
                Dies[i] = new Dice();
            }
        }
    }
}
