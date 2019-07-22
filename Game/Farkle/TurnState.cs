using System.Collections.Generic;

namespace Farkle
{
    /// <summary>
    ///     State of the current turn.
    /// </summary>
    public class TurnState
    {
        public int Attempt = 0;
        public List<Dice> Dices = new List<Dice>();

        public int Score = 0;

        /// <summary>
        ///     Create a state of a new turn.
        /// </summary>
        public TurnState()
        {
            for (var i = 0; i < 6; i++) Dices.Add(new Dice());
        }

        public bool Scorable => Score >= 350;
        public bool RerolledAll;

        /// <summary>
        ///     Reset the dices of the state, but keep the score.
        /// </summary>
        public void ResetDices()
        {
            Dices = new List<Dice>();
            RerolledAll = true;
            for (var i = 0; i < 6; i++) Dices.Add(new Dice());
        }
    }
}