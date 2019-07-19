using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Farkle
{
    /// <summary>
    /// State of the current turn.
    /// </summary>
    class TurnState
    {
        public List<Dice> Dices = new List<Dice>();
        
        public int Score = 0;

        public int Attempt = 0;
        public bool Scorable => Score >= 350;

        /// <summary>
        /// Create a state of a new turn.
        /// </summary>
        public TurnState()
        {
            for (int i = 0; i < 6; i++)
            {
                Dices.Add(new Dice());
            }
        }

        /// <summary>
        /// Reset the dices of the state, but keep the score.
        /// </summary>
        public void ResetDices()
        {
            Dices = new List<Dice>();
            for (int i = 0; i < 6; i++)
            {
                Dices.Add(new Dice());
            }
        }
    }
}
