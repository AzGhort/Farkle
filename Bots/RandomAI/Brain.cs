using System;
using System.Collections.Generic;
using Farkle;
using GreedyCowardAI;

namespace GreedyCowardBot
{
    internal class Brain
    {
        private PlayerAction currentAction;
        private TurnState currentTurnState;
        private List<Dice> lastRolledDices;
        private int totalScore;

        public void Start()
        {
            while (true)
            {
                var input = Console.ReadLine();
                var message = new Message(input);
                switch (message.Type)
                {
                    case MessageType.EndGame:
                        // end this brain
                        return;
                    case MessageType.StartTurn:
                        currentTurnState = new TurnState();
                        currentAction = PlayerAction.Invalid;
                        lastRolledDices = new List<Dice>();
                        break;
                    case MessageType.SuccessfulAction:
                        AccountLastAction();
                        break;
                    case MessageType.DicesRolled:
                        var dices = ConsiderDices(message.MessageParams);
                        Message.SendActionOrder(currentAction, dices);
                        break;
                }
            }
        }

        private void AccountLastAction()
        {
            switch (currentAction)
            {
                case PlayerAction.Score:
                    totalScore += currentTurnState.Score;
                    break;
                case PlayerAction.Keep:
                    var diceSet = Scoring.DetermineScore(lastRolledDices);
                    currentTurnState.Score += diceSet.Score;
                    foreach (var dice in lastRolledDices) dice.Kept = true;
                    if (currentTurnState.Dices.TrueForAll(dice => dice.Kept)) currentTurnState.ResetDices();
                    break;
            }
        }

        private List<int> ConsiderDices(List<int> dices)
        {
            var diceIndices = new List<int>();
            var lastDices = new List<Dice>();
            currentTurnState.Attempt++;
            for (var i = 0; i < dices.Count; i++)
            {
                currentTurnState.Dices[i].Value = dices[i];
                // this dice is not kept yet
                if (!currentTurnState.Dices[i].Kept) lastDices.Add(currentTurnState.Dices[i]);
            }

            lastRolledDices = lastDices;
            var currentTotalScore = currentTurnState.Score;
            var split = Scoring.SplitDicesByValue(lastDices);
            foreach (var diceValue in split.Keys)
            {
                // keep all ones and fives
                if (diceValue == 1 || diceValue == 5)
                {
                    diceIndices.AddRange(FindDiceByValueSendableIndices(diceValue));
                }
                else
                {
                    if (Scoring.ComputeSameDicesScore(diceValue, split[diceValue]) > 0)
                        diceIndices.AddRange(FindDiceByValueSendableIndices(diceValue));
                }

                currentTotalScore += Scoring.ComputeSameDicesScore(diceValue, split[diceValue]);
            }

            currentAction = currentTotalScore < 350 ? PlayerAction.Keep : PlayerAction.Score;

            return diceIndices;
        }

        private List<int> FindDiceByValueSendableIndices(int diceValue)
        {
            var ret = new List<int>();

            for (var i = 0; i < currentTurnState.Dices.Count; i++)
                if (!currentTurnState.Dices[i].Kept && currentTurnState.Dices[i].Value == diceValue)
                    ret.Add(i + 1);

            return ret;
        }
    }
}