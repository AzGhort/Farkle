using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Farkle
{
    /// <summary>
    /// Represents player of the game.
    /// </summary>
    class Player : IComparable<Player>
    {
        public int TurnOrder;
        public TurnState State = new TurnState();
        public int TotalScore = 0;

        /// <summary>
        /// Creates a new player.
        /// </summary>
        /// <param name="turnOrder">Order of the turn, in which the player plays.</param>
        public Player(int turnOrder)
        {
            TurnOrder = turnOrder;
        }

        /// <inheritoc/>
        public override string ToString()
        {
            return $"Player number {TurnOrder}, score {TotalScore}";
        }

        /// <summary>
        /// Compares based on the total score.
        /// </summary>
        /// <param name="other">Player to be compared with.</param>
        /// <returns>The player with better score.</returns>
        public int CompareTo(Player other)
        {
            return TotalScore.CompareTo(other.TotalScore);
        }
    }

    /// <summary>
    /// Represents farkle game.
    /// </summary>
    class Game
    {
        public Player[] Players = new Player[2];
        public int TurnOrder = 0;
        public bool IsWon => (TurnOrder == Players.Length - 1 &&
            Players.Any(player => player.TotalScore >= 10000));
        public Player CurrentPlayer => Players[TurnOrder];

        public Game()
        {
            for (int i = 0; i < 2; i++)
            {
                Players[i] = new Player(i);
            }
        }

        public Player Winner
        {
            get
            {
                if (!IsWon) return null;
                return Players.ToList().Max();
            }
        }

        /// <summary>
        /// Switches to next player (increases turn order).
        /// </summary>
        public void NextPlayer()
        {
            TurnOrder = (TurnOrder + 1) % Players.Length;
        }
    }
}
