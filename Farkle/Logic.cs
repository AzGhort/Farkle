using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farkle
{
    /// <summary>
    /// Provides method to execute actions according to the game logic.
    /// </summary>
    class Logic
    {
        /// <summary>
        /// Rerolls unkept dices of the given player.
        /// </summary>
        /// <param name="player">Player who rerolls the dices.</param>
        /// <returns>Whether the roll succeeded.</returns>
        public static GameActionResult Reroll(Player player)
        {
            player.State.Attempt++;
            var dicesRolled = new List<Dice>();
            foreach (var dice in player.State.Dices)
            {
                if (!dice.Kept)
                {
                    dice.Roll();
                    dicesRolled.Add(dice);
                }
            }
            var diceSet = Scoring.DetermineScore(dicesRolled);
            if ((diceSet.Score == 0) || (diceSet.Score + player.State.Score < 350 && player.State.Attempt == 3))
            {
                player.State = new TurnState();
                return GameActionResult.FAILURE;
            }

            return GameActionResult.SUCCESS;
        }

        /// <summary>
        /// Tries to keep given dices with respect to the turn's state.
        /// </summary>
        /// <param name="state">Current state of the turn.</param>
        /// <param name="dices">Dices to be kept.</param>
        /// <returns>Whether the keep succeeded.</returns>
        public static GameActionResult KeepDices(TurnState state, List<Dice> dices)
        {
            if (dices.Count == 0) return GameActionResult.FAILURE;
            foreach (Dice d in dices)
            {
                if (d.Kept || !state.Dices.Contains(d)) return GameActionResult.FAILURE;
            }
            var score = Scoring.DetermineScore(dices);
            if (!score.Keepable) return GameActionResult.FAILURE;

            // all dices chosen can be kept
            state.Score += score.Score;
            foreach (Dice d in dices) d.Kept = true;
            if (state.Dices.TrueForAll(dice => dice.Kept)) state.ResetDices();
            return GameActionResult.SUCCESS;
        }

        /// <summary>
        /// Tries to score current turn.
        /// </summary>
        /// <param name="player">Player to score the turn.</param>
        /// <returns>Whether the scoring succeeded.</returns>
        public static GameActionResult ScoreCurrentTurn(Player player)
        {
            if (!player.State.Scorable) return GameActionResult.FAILURE;
            player.TotalScore += player.State.Score;
            player.State = new TurnState();
            return GameActionResult.SUCCESS;
        }
    }

    /// <summary>
    /// Represents results of the game actions.
    /// </summary>
    enum GameActionResult
    {
        FAILURE, SUCCESS
    }
}
