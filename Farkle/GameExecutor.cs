using System;
using System.Collections.Generic;
using System.Text;

namespace Farkle
{
    /// <summary>
    /// 
    /// </summary>
    class GameExecutor
    {
        private Game game = new Game();

        /// <summary>
        /// Runs the game itself.
        /// </summary>
        public void RunGame()
        {
            Console.WriteLine("-- Game starts!");
            while (!game.IsWon)
            {
                InformStartTurn();
                while (true)
                {
                    var rollResult = Logic.Reroll(game.CurrentPlayer);
                    InformDicesRolled(rollResult, game.CurrentPlayer.State);
                    if (rollResult == GameActionResult.FAILURE) break;
                    bool responseValid = false;
                    var response = new PlayerResponse();
                    while (!responseValid)
                    {
                        response = CheckResponse();
                        if (response.Order == PlayerAction.SCORE)
                        {
                            var scoreResult = Logic.ScoreCurrentTurn(game.CurrentPlayer);
                            if (scoreResult == GameActionResult.SUCCESS) responseValid = true;
                            else
                            {
                                Console.WriteLine(CommunicationConstants.FailureInfo);
                                Console.WriteLine("-- Cant score current kept dices.");
                            }
                        }
                        else if (response.Order == PlayerAction.KEEP)
                        {
                            var keepResult = Logic.KeepDices(game.CurrentPlayer.State, response.Dices);
                            if (keepResult == GameActionResult.SUCCESS) responseValid = true;
                            else
                            {
                                Console.WriteLine(CommunicationConstants.FailureInfo);
                                Console.WriteLine("-- Cant keep chosen dices.");
                            }
                        }
                    }
                    Console.WriteLine(CommunicationConstants.SuccessInfo);
                    // player scored succesfully, end turn
                    if (response.Order == PlayerAction.SCORE) break;                   
                }                                
                InformEndTurn();
                game.NextPlayer();
            }
            InformEndGame();
        }

        /// <summary>
        /// Checks response from the player.
        /// </summary>
        /// <returns></returns>
        private PlayerResponse CheckResponse()
        {
            PlayerResponse response = new PlayerResponse();
            while (true)
            {
                var line = Console.ReadLine();
                while (line == "")
                {
                    line = Console.ReadLine();
                }
                var orderTokens = line.Split(" ");
                if (orderTokens[0] == CommunicationConstants.ScoreOrder)
                {
                    response.Order = PlayerAction.SCORE;
                    return response;
                }
                else if (orderTokens[0] == CommunicationConstants.KeepOrder)
                {
                    response.Order = PlayerAction.KEEP;
                    var dices = new List<Dice>();
                    for (int i = 1; i < orderTokens.Length; i++)
                    {
                        int diceIndex = -1;
                        if (!int.TryParse(orderTokens[i], out diceIndex))
                        {
                            Console.WriteLine(CommunicationConstants.BadOrderInfo);
                            response.Order = PlayerAction.INVALID;
                            return response;
                        }
                        else
                        {
                            if (diceIndex < 1 || diceIndex > 6)
                            {
                                Console.WriteLine(CommunicationConstants.BadOrderInfo);
                                response.Order = PlayerAction.INVALID;
                                return response;
                            }
                            dices.Add(game.CurrentPlayer.State.Dices[diceIndex - 1]);
                        }
                    }
                    response.Dices = dices;
                    return response;
                }
                else
                {
                    Console.WriteLine(CommunicationConstants.BadOrderInfo);
                    response.Order = PlayerAction.INVALID;
                    return response;
                }
            }
        }

        private void InformDicesRolled(GameActionResult rollResult, TurnState state)
        { 
            if (rollResult == GameActionResult.FAILURE)
            {
                Console.WriteLine("-- Bad luck! Nothing scorable was rolled!");
            }
            else
            {
                Console.WriteLine("-- Roll succesful");
                Console.WriteLine(CommunicationConstants.CurrentTurnScoreInfo + " " + state.Score);
            }
            foreach (Dice d in game.CurrentPlayer.State.Dices)
            {                
                Console.Write($"{d.Value} ");  
            }
            Console.WriteLine();
        }

        private void InformStartTurn()
        {
            Console.WriteLine($"-- Turn starts.");
            Console.WriteLine("-- Current player: " + game.CurrentPlayer.ToString());
        }

        private void InformEndTurn()
        {
            Console.WriteLine("-- Current player: " + game.CurrentPlayer.ToString());
            Console.WriteLine($"-- Turn ends.");
        }

        private void InformEndGame()
        {
            Console.WriteLine(CommunicationConstants.EndGameInfo);
            foreach (Player pl in game.Players)
            {
                Console.WriteLine(pl.ToString());
            }
            Console.WriteLine($"-- " +
                $"Winner is player number {game.Winner.TurnOrder}, " +
                $"scoring {game.Winner.TotalScore} points!");
        }
    }
}
