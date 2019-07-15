using System;
using System.Collections.Generic;
using System.Text;

namespace Farkle
{
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
