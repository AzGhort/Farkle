using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farkle
{
    class GameLogic
    {
        public static RollResult Reroll(GameState state)
        {
            state.Attempt++;
            var dicesToRoll = new List<Dice>();
            foreach (var dice in state.Dies)
            {
                if (!dice.Kept)
                {
                    dice.Roll();
                    dicesToRoll.Add(dice);
                }
            }

            var diceSet = Scoring.DetermineScore(dicesToRoll);
            if ((diceSet.Score == 0) || (diceSet.Score + state.Score < 350 && state.Attempt == 3))
            {
                return RollResult.FAILURE;
            }

            return RollResult.SUCCESS;
        }
    }

}
