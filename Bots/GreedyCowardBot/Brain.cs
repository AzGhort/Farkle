using System;
using System.Collections.Generic;
using Farkle;
using GreedyCowardAI;

namespace GreedyCowardBot
{
    /// <summary>
    ///     Represents bot's logic.
    /// </summary>
    internal class Brain
    {
        private PlayerAction currentAction;
        private TurnState currentTurnState = new TurnState();
        private Message lastDicesMessage;
        private List<Dice> lastKeptDices;
        private int totalScore;

        /// <summary>
        ///     Runs the main bot's AI loop.
        /// </summary>
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
                        lastKeptDices = new List<Dice>();
                        lastDicesMessage = null;
                        break;
                    case MessageType.SuccessfulAction:
                        AccountLastAction();
                        break;
                    case MessageType.FailedAction:
                        var dices = ConsiderDices(lastDicesMessage.MessageParams);
                        Message.SendActionOrder(currentAction, dices);
                        break;
                    case MessageType.DicesRolled:
                        lastDicesMessage = message;
                        var dicez = ConsiderDices(message.MessageParams);
                        Message.SendActionOrder(currentAction, dicez);
                        break;
                }
            }
        }

        /// <summary>
        ///     After receiving confirmation from the game, account the last action.
        /// </summary>
        private void AccountLastAction()
        {
            var diceSet = Scoring.DetermineScore(lastKeptDices);
            switch (currentAction)
            {
                case PlayerAction.Score:
                    diceSet = Scoring.DetermineScore(lastKeptDices);
                    currentTurnState.Score += diceSet.Score;
                    totalScore += currentTurnState.Score;
                    break;
                case PlayerAction.Keep:
                    diceSet = Scoring.DetermineScore(lastKeptDices);
                    currentTurnState.Score += diceSet.Score;
                    foreach (var dice in lastKeptDices) dice.Kept = true;
                    if (currentTurnState.Dices.TrueForAll(dice => dice.Kept)) currentTurnState.ResetDices();
                    break;
            }
        }

        /// <summary>
        ///     Consider new dices rolled.
        /// </summary>
        /// <param name="dices">List of dices values.</param>
        /// <returns>Indices of dices to keep, empty if intending to score.</returns>
        private List<int> ConsiderDices(List<int> dices)
        {
            var diceIndices = new List<int>();
            var lastRolledDices = new List<Dice>();
            currentTurnState.Attempt++;
            for (var i = 0; i < dices.Count; i++)
            {
                currentTurnState.Dices[i].Value = dices[i];
                // this dice is not kept yet
                if (!currentTurnState.Dices[i].Kept) lastRolledDices.Add(currentTurnState.Dices[i]);
            }

            var currentTotalScore = currentTurnState.Score;
            var split = Scoring.SplitDicesByValue(lastRolledDices);
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

            lastKeptDices = new List<Dice>();
            foreach (var diceIndex in diceIndices) lastKeptDices.Add(currentTurnState.Dices[diceIndex - 1]);
            currentAction =
                currentTotalScore < 350 ||
                lastKeptDices.Count + currentTurnState.Dices.FindAll(dice => dice.Kept).Count == 6
                    ? PlayerAction.Keep
                    : PlayerAction.Score;

            return diceIndices;
        }

        /// <summary>
        ///     Finds all dices with given value, and returns their indices.
        /// </summary>
        /// <param name="diceValue">Which dices to look for.</param>
        /// <returns>List of dice indices (+1)</returns>
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