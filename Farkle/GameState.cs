using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Farkle
{
    class GameState
    {
        public Dice[] Dies = new Dice[6];

        public List<DiceCombination> KeptDiceCombinations = new List<DiceCombination>();

        public int Score => KeptDiceCombinations.Sum(combination => combination.Score);

        public int Attempt = 1;
        public bool Keepable => Score >= 350;

        public GameState()
        {
            for (int i = 0; i < 6; i++)
            {
                Dies[i] = new Dice();
            }
        }

        public List<DiceCombination> GetAllDiceCombinations()
        {
            var diceCombinations = new List<DiceCombination>();
            for (int i = 0; i < 64; i++)
            {
                var combination = new List<Dice>();
                for (int j = 0; j < 6; j++)
                {
                    if ((i & (1 << (6 - j - 1))) != 0)
                    {
                        combination.Add(Dies[j]);
                    }
                }
                diceCombinations.Add(new DiceCombination(combination));
            }

            return diceCombinations;
        }
    }
}
