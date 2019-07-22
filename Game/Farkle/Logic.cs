using System.Collections.Generic;

namespace Farkle
{
    /// <summary>
    ///     Provides method to execute actions according to the game logic.
    /// </summary>
    internal class Logic
    {
        /// <summary>
        ///     Rerolls unkept dices of the given player.
        /// </summary>
        /// <param name="player">Player who rerolls the dices.</param>
        /// <returns>Whether the roll succeeded.</returns>
        public static GameActionResult Reroll(Player player)
        {
            player.State.Attempt++;
            var dicesRolled = new List<Dice>();
            foreach (var dice in player.State.Dices)
                if (!dice.Kept)
                {
                    dice.Roll();
                    dicesRolled.Add(dice);
                }

            var diceSet = Scoring.DetermineScore(dicesRolled);
            if (diceSet.Score == 0 || diceSet.Score + player.State.Score < 350 && player.State.Attempt == 3)
            {
                return GameActionResult.Failure;
            }

            return GameActionResult.Success;
        }

        /// <summary>
        ///     Tries to keep given dices with respect to the turn's state.
        /// </summary>
        /// <param name="state">Current state of the turn.</param>
        /// <param name="dices">Dices to be kept.</param>
        /// <returns>Whether the keep succeeded.</returns>
        public static GameActionResult KeepDices(TurnState state, List<Dice> dices)
        {
            if (dices.Count == 0) return GameActionResult.Failure;
            foreach (var d in dices)
                if (d.Kept || !state.Dices.Contains(d))
                    return GameActionResult.Failure;
            var score = Scoring.DetermineScore(dices);
            if (!score.Keepable) return GameActionResult.Failure;

            // all dices chosen can be kept
            state.Score += score.Score;
            foreach (var d in dices) d.Kept = true;
            if (state.Dices.TrueForAll(dice => dice.Kept)) state.ResetDices();
            return GameActionResult.Success;
        }

        /// <summary>
        ///     Tries to score current turn.
        /// </summary>
        /// <param name="player">Player to score the turn.</param>
        /// <returns>Whether the scoring succeeded.</returns>
        public static GameActionResult ScoreCurrentTurn(Player player)
        {
            if (!player.State.Scorable)
            {
                // try to score the last rolled dices
                var dices = player.State.Dices.FindAll(dice => !dice.Kept);
                var diceSet = Scoring.DetermineScore(dices);
                if (diceSet.Score + player.State.Score < 350) return GameActionResult.Failure;
                player.TotalScore += diceSet.Score + player.State.Score;
                return GameActionResult.Success;
            }

            player.TotalScore += player.State.Score;
            return GameActionResult.Success;
        }
    }

    /// <summary>
    ///     Represents results of the game actions.
    /// </summary>
    internal enum GameActionResult
    {
        Failure,
        Success
    }
}