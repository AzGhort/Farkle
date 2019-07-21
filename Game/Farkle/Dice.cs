using System;

namespace Farkle
{
    /// <summary>
    ///     Dice representation.
    /// </summary>
   public class Dice
    {
        private static readonly Random random = new Random();

        public int Value { get; set; } = random.Next(1, 7);
        public bool Kept { get; set; }

        /// <summary>
        ///     Roll this dice.
        /// </summary>
        public void Roll()
        {
            Kept = false;
            Value = random.Next(1, 7);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Kept ? "Kept " + Value : "Rolled " + Value;
        }
    }
}