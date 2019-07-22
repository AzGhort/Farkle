using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Farkle
{
    internal class GameRunner
    {
        private Game game;

        public void RunGameExternally(Process player1Process, Process player2Process)
        {
            game = new Game(player1Process.StandardOutput, player1Process.StandardInput,
                player2Process.StandardOutput, player2Process.StandardInput);
            Run();
        }

        public void RunGameLocally()
        {
            var sw = new StreamWriter(Console.OpenStandardOutput())
            {
                AutoFlush = true
            };
            var sr = new StreamReader(Console.OpenStandardInput());
            game = new Game(sr, sw, sr, sw);
            Run();
        }

        /// <summary>
        ///     Runs the game.
        /// </summary>
        private void Run()
        {
            InformStartGame();
            while (!game.IsWon)
            {
                InformStartTurn();
                while (true)
                {
                    var rollResult = Logic.Reroll(game.CurrentPlayer);
                    InformDicesRolled(rollResult, game.CurrentPlayer.State);
                    if (rollResult == GameActionResult.Failure) break;
                    var responseValid = false;
                    var response = new PlayerResponse();
                    while (!responseValid)
                    {
                        response = CheckResponse();
                        if (response.Order == PlayerAction.Score)
                        {
                            var scoreResult = Logic.ScoreCurrentTurn(game.CurrentPlayer);
                            if (scoreResult == GameActionResult.Success)
                            {
                                responseValid = true;
                            }
                            else
                            {
                                InformPlayer(game.CurrentPlayer, CommunicationConstants.FailureInfo);
                                InformPlayer(game.CurrentPlayer, "-- Cant score current kept dices.");
                            }
                        }
                        else if (response.Order == PlayerAction.Keep)
                        {
                            var keepResult = Logic.KeepDices(game.CurrentPlayer.State, response.Dices);
                            if (keepResult == GameActionResult.Success)
                            {
                                responseValid = true;
                            }
                            else
                            {
                                InformPlayer(game.CurrentPlayer, CommunicationConstants.FailureInfo);
                                InformPlayer(game.CurrentPlayer, "-- Cant keep chosen dices.");
                            }
                        }
                    }

                    InformPlayer(game.CurrentPlayer, CommunicationConstants.SuccessInfo);
                    // player scored succesfully, end turn
                    if (response.Order == PlayerAction.Score) break;
                }

                InformEndTurn();
                game.NextPlayer();
            }

            InformEndGame();
        }

        /// <summary>
        ///     Checks response from the player.
        /// </summary>
        /// <returns></returns>
        private PlayerResponse CheckResponse()
        {
            var response = new PlayerResponse();
            while (true)
            {
                var line = ReadLineFromCurrentPlayer();
                while (string.IsNullOrEmpty(line)) line = ReadLineFromCurrentPlayer();
                var orderTokens = line.Split(" ");
                if (orderTokens[0] == CommunicationConstants.ScoreOrder)
                {
                    response.Order = PlayerAction.Score;
                    return response;
                }

                if (orderTokens[0] == CommunicationConstants.KeepOrder)
                {
                    response.Order = PlayerAction.Keep;
                    var dices = new List<Dice>();
                    for (var i = 1; i < orderTokens.Length; i++)
                    {
                        var diceIndex = -1;
                        if (!int.TryParse(orderTokens[i], out diceIndex))
                        {
                            InformPlayer(game.CurrentPlayer, CommunicationConstants.FailureInfo);
                            response.Order = PlayerAction.Invalid;
                            return response;
                        }

                        if (diceIndex < 1 || diceIndex > 6)
                        {
                            InformPlayer(game.CurrentPlayer, CommunicationConstants.FailureInfo);
                            response.Order = PlayerAction.Invalid;
                            return response;
                        }

                        dices.Add(game.CurrentPlayer.State.Dices[diceIndex - 1]);
                    }

                    response.Dices = dices;
                    return response;
                }

                InformPlayer(game.CurrentPlayer, CommunicationConstants.FailureInfo);
                response.Order = PlayerAction.Invalid;
                return response;
            }
        }

        private string ReadLineFromCurrentPlayer()
        {
            return game.CurrentPlayer.PlayerOutput.ReadLine();
        }

        private void InformPlayers(string message)
        {
            foreach (var pl in game.Players) pl.PlayerInput.WriteLine(message);
            Console.WriteLine(message);
        }

        private void InformPlayer(Player pl, string message)
        {
            pl.PlayerInput.WriteLine(message);
            Console.WriteLine(message);
        }

        private void InformDicesRolled(GameActionResult rollResult, TurnState state)
        {
            if (rollResult == GameActionResult.Failure)
            {
                InformPlayer(game.CurrentPlayer, "-- Bad luck! Nothing scorable was rolled!");
            }
            else
            {
                InformPlayer(game.CurrentPlayer, "-- Roll succesful");
                InformPlayer(game.CurrentPlayer, CommunicationConstants.CurrentTurnScoreInfo + " " + state.Score);
            }
            string dices = "";
            foreach (var d in game.CurrentPlayer.State.Dices)
            {
                dices += $"{d.Value} ";
            }
            InformPlayer(game.CurrentPlayer, CommunicationConstants.DicesInfo + " " + dices);
        }

        private void InformStartTurn()
        {
            InformPlayers("-- Turn starts.");
            InformPlayers("-- Current player: " + game.CurrentPlayer);
            InformPlayer(game.CurrentPlayer, CommunicationConstants.StartTurnInfo);
        }

        private void InformEndTurn()
        {
            InformPlayers("-- Current player: " + game.CurrentPlayer);
            InformPlayers("-- Turn ends.");
            InformPlayer(game.CurrentPlayer, CommunicationConstants.EndTurnInfo);
            InformPlayer(game.OtherPlayer, CommunicationConstants.OtherPlayerTurnInfo + " " + game.CurrentPlayer.TotalScore);
        }

        private void InformStartGame()
        {
            InformPlayers(CommunicationConstants.StartGameInfo);
            InformPlayers("-- Game starts!");
        }

        private void InformEndGame()
        {
            InformPlayers(CommunicationConstants.EndGameInfo);
            foreach (var pl in game.Players) InformPlayers(pl.ToString());
            InformPlayers("-- " +
                          $"Winner is player number {game.Winner.TurnOrder}, " +
                          $"scoring {game.Winner.TotalScore} points!");
        }
    }
}