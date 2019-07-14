using System;
using System.Collections.Generic;
using System.Text;

namespace Farkle
{
    class DiceCombination
    {
        public List<Dice> Dices = new List<Dice>();
        public int Score = 0;

        public DiceCombination(List<Dice> dies)
        {
            Dices = dies;
            Score = GameLogic.DetermineScore(Dices);
        }

        public override string ToString()
        {
            string ret = "";
            foreach (var dice in Dices)
            {
                ret += dice.Value;
            }

            return ret;
        }
    }

    class Dice
    {
        private static Random random = new Random();

        public int Value { get; set; } = random.Next(1, 7);
        public bool Kept { get; set; } = false;

        public void Roll()
        {
            Kept = false;
            Value = random.Next(1, 7);
        }
    }
}
