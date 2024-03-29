﻿using System.Collections.Generic;

namespace Farkle
{
    /// <summary>
    ///     Represents set of rolled dices.
    /// </summary>
    public class DiceSet
    {
        public List<Dice> Dices;
        public bool Keepable = true;
        public int Score;
        public bool Scorable => Score >= 350;
    }

    /// <summary>
    ///     Provides methods to compute score of given dices.
    /// </summary>
    public class Scoring
    {
        /// <summary>
        ///     Determines score of given dices.
        /// </summary>
        /// <param name="dices">Dices to be scored.</param>
        /// <returns>Dices and their score.</returns>
        public static DiceSet DetermineScore(List<Dice> dices)
        {
            var retval = new DiceSet {Dices = dices};
            var dicesCounts = SplitDicesByValue(dices);
            foreach (var diceValue in dicesCounts.Keys)
            {
                var curVal = ComputeSameDicesScore(diceValue, dicesCounts[diceValue]);
                if (curVal == 0) retval.Keepable = false;

                retval.Score += curVal;
            }

            return retval;
        }

        /// <summary>
        ///     Splits dices by value to dictionary.
        /// </summary>
        /// <param name="dices">Dices to be split.</param>
        /// <returns>Mapping from values to counts of the dices.</returns>
        public static Dictionary<int, int> SplitDicesByValue(IEnumerable<Dice> dices)
        {
            var ret = new Dictionary<int, int>();
            foreach (var dice in dices)
                if (ret.ContainsKey(dice.Value)) ret[dice.Value]++;
                else ret[dice.Value] = 1;

            return ret;
        }

        /// <summary>
        ///     Computes score of dices with the same value.
        /// </summary>
        /// <param name="diceValue">Value of the dices.</param>
        /// <param name="diceCount">Count of the dices.</param>
        /// <returns>Score of given dice set.</returns>
        public static int ComputeSameDicesScore(int diceValue, int diceCount)
        {
            if (diceCount < 3)
                switch (diceValue)
                {
                    case 1:
                        return diceCount * 100;
                    case 5:
                        return diceCount * 50;
                    default:
                        return 0;
                }

            var factor = diceCount - 2;
            switch (diceValue)
            {
                case 1:
                    return 1000 * factor;
                case 2:
                    return 200 * factor;
                case 3:
                    return 300 * factor;
                case 4:
                    return 400 * factor;
                case 5:
                    return 500 * factor;
                case 6:
                    return 600 * factor;
                default:
                    return 0;
            }
        }
    }
}