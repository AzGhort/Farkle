using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Farkle
{
    /// <summary>
    /// 
    /// </summary>
    class GameExecutor
    {
        private Game game;

        public GameExecutor(string processName1, string processName2)
        {
            Process player1Process = StartPlayerProcess(processName1);
            Process player2Process = StartPlayerProcess(processName2);
            game = new Game(player1Process.StandardOutput, player1Process.StandardInput,
                player2Process.StandardOutput, player2Process.StandardInput);
        }

        private Process StartPlayerProcess(string processName)
        {
            Process p = new Process();
            p.StartInfo.FileName = processName;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.CreateNoWindow = false; // debugging
            p.Start();
            return p;
        }

        /// <summary>
        /// Runs the game.
        /// </summary>
        public void RunGame()
        {
            InformStartGame();
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
                                InformPlayer(game.CurrentPlayer, CommunicationConstants.FailureInfo);
                                InformPlayer(game.CurrentPlayer, "-- Cant score current kept dices.");
                            }
                        }
                        else if (response.Order == PlayerAction.KEEP)
                        {
                            var keepResult = Logic.KeepDices(game.CurrentPlayer.State, response.Dices);
                            if (keepResult == GameActionResult.SUCCESS) responseValid = true;
                            else
                            {
                                InformPlayer(game.CurrentPlayer, CommunicationConstants.FailureInfo);
                                InformPlayer(game.CurrentPlayer, "-- Cant keep chosen dices.");
                            }
                        }
                    }
                    InformPlayer(game.CurrentPlayer, CommunicationConstants.SuccessInfo);
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
                var line = ReadLineFromCurrentPlayer();
                while (line == "")
                {
                    line = ReadLineFromCurrentPlayer();
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
                            InformPlayer(game.CurrentPlayer, CommunicationConstants.BadOrderInfo);
                            response.Order = PlayerAction.INVALID;
                            return response;
                        }
                        else
                        {
                            if (diceIndex < 1 || diceIndex > 6)
                            {
                                InformPlayer(game.CurrentPlayer, CommunicationConstants.BadOrderInfo);
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
                    InformPlayer(game.CurrentPlayer, CommunicationConstants.BadOrderInfo);
                    response.Order = PlayerAction.INVALID;
                    return response;
                }
            }
        }

        private string ReadLineFromCurrentPlayer()
        {
            return game.CurrentPlayer.PlayerOutput.ReadLine();
        }

        private void InformPlayers(string message)
        {
            foreach (Player pl in game.Players)
            {
                pl.PlayerInput.WriteLine(message);
            }
            Console.WriteLine(message);
        }

        private void InformPlayer(Player pl, string message)
        {
            pl.PlayerInput.WriteLine(message);
            Console.WriteLine(message);
        }

        private void InformDicesRolled(GameActionResult rollResult, TurnState state)
        { 
            if (rollResult == GameActionResult.FAILURE)
            {
                InformPlayer(game.CurrentPlayer, "-- Bad luck! Nothing scorable was rolled!");
            }
            else
            {
                InformPlayer(game.CurrentPlayer, "-- Roll succesful");
                InformPlayer(game.CurrentPlayer, CommunicationConstants.CurrentTurnScoreInfo + " " + state.Score);
            }
            foreach (Dice d in game.CurrentPlayer.State.Dices)
            {                
                game.CurrentPlayer.PlayerInput.Write($"{d.Value} ");
                Console.Write($"{d.Value} ");
            }
            InformPlayer(game.CurrentPlayer, "");
        }

        private void InformStartTurn()
        {
            InformPlayers("-- Turn starts.");
            InformPlayers("-- Current player: " + game.CurrentPlayer.ToString());
            InformPlayer(game.CurrentPlayer, CommunicationConstants.StartTurnInfo);
        }

        private void InformEndTurn()
        {
            InformPlayers("-- Current player: " + game.CurrentPlayer.ToString());
            InformPlayers("-- Turn ends.");
            InformPlayer(game.CurrentPlayer, CommunicationConstants.EndTurnInfo);
        }

        private void InformStartGame()
        {
            InformPlayers(CommunicationConstants.StartGameInfo);
            InformPlayers("-- Game starts!");
        }

        private void InformEndGame()
        {
            InformPlayers(CommunicationConstants.EndGameInfo);
            foreach (Player pl in game.Players)
            {
                InformPlayers(pl.ToString());
            }
            InformPlayers($"-- " +
                $"Winner is player number {game.Winner.TurnOrder}, " +
                $"scoring {game.Winner.TotalScore} points!");
        }
    }
}
