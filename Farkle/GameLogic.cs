using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farkle
{
    class GameLogic
    {
        /// <summary>
        /// Treats dies only one-by-one, counts ones-and-fives score.
        /// </summary>
        /// <param name="dies"> Dies to compute score.</param>
        /// <returns>Score of given dies.</returns>
        public static int GetOnesAndFivesScore(List<Dice> dies)
        {
            int ones = dies.FindAll(die => die.Value == 1).Count;
            int fives = dies.FindAll(die => die.Value == 5).Count;
            // only keepable combinations, no other dies allowed
            if (ones + fives != dies.Count) return 0;
            return ones * 100 + fives * 50;
        }

        public static int GetSameDiesScore(List<Dice> dies)
        {
            // only combinations of same dies
            if (dies.Any(die => die.Value != dies[0].Value)) return 0;
            var factor = dies.Count - 2;
            switch (dies[0].Value)
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

        public static int DetermineScore(List<Dice> dies)
        {
            if (dies.Count < 3) return GetOnesAndFivesScore(dies);
            var same = GetSameDiesScore(dies);
            return same > 0 ? same : GetOnesAndFivesScore(dies);
        }

        public static void Reroll(GameState state)
        {
            foreach (var dice in state.Dies)
            {
                if (!dice.Kept)
                {
                    dice.Roll();
                }
            }
        }
    }

}
