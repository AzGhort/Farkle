using System;
using System.IO;
using System.Linq;

namespace Farkle
{
    /// <summary>
    ///     Represents player of the game.
    /// </summary>
    internal class Player : IComparable<Player>
    {
        public StreamWriter PlayerInput;
        public StreamReader PlayerOutput;
        public TurnState State = new TurnState();
        public int TotalScore = 0;
        public int TurnOrder;

        /// <summary>
        ///     Creates a new player.
        /// </summary>
        /// <param name="turnOrder">Order of the turn, in which the player plays.</param>
        public Player(int turnOrder, StreamReader o, StreamWriter i)
        {
            TurnOrder = turnOrder;
            PlayerInput = i;
            PlayerOutput = o;
        }

        /// <summary>
        ///     Compares based on the total score.
        /// </summary>
        /// <param name="other">Player to be compared with.</param>
        /// <returns>The player with better score.</returns>
        public int CompareTo(Player other)
        {
            return TotalScore.CompareTo(other.TotalScore);
        }

        /// <inheritoc />
        public override string ToString()
        {
            return $"Player number {TurnOrder}, score {TotalScore}";
        }
    }

    /// <summary>
    ///     Represents farkle game.
    /// </summary>
    internal class Game
    {
        public Player[] Players = new Player[2];
        public int TurnOrder;

        public Game(StreamReader out1, StreamWriter in1, StreamReader out2, StreamWriter in2)
        {
            Players[0] = new Player(0, out1, in1);
            Players[1] = new Player(1, out2, in2);
        }

        public bool IsWon => TurnOrder == Players.Length - 1 &&
                             Players.Any(player => player.TotalScore >= 10000);

        public Player CurrentPlayer => Players[TurnOrder];
        public Player OtherPlayer => Players[(TurnOrder + 1) % 2];

        public Player Winner
        {
            get
            {
                if (!IsWon) return null;
                return Players.ToList().Max();
            }
        }

        /// <summary>
        ///     Switches to next player (increases turn order).
        /// </summary>
        public void NextPlayer()
        {
            CurrentPlayer.State = new TurnState();
            TurnOrder = (TurnOrder + 1) % Players.Length;
        }
    }
}