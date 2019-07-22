using System.Diagnostics;

namespace Farkle
{
    /// <summary>
    ///     Executes matches of the game.
    /// </summary>
    internal class GameExecutor
    {
        private const string dotnet = "dotnet.exe";
        private readonly GameRunner runner = new GameRunner();

        /// <summary>
        ///     Runs game with the bots as external dlls.
        /// </summary>
        /// <param name="botName1">Filename of the first bot's dll.</param>
        /// <param name="botName2">Filename of the second bot's dll.</param>
        public void RunGame(string botName1, string botName2)
        {
            var player1Process = StartPlayerProcess(botName1);
            var player2Process = StartPlayerProcess(botName2);
            runner.RunGameExternally(player1Process, player2Process);
        }

        /// <summary>
        ///     Starts a new player process.
        /// </summary>
        /// <param name="processName">Filename of the player dll.</param>
        /// <returns>Process of the running dll.</returns>
        private Process StartPlayerProcess(string processName)
        {
            var p = new Process
            {
                StartInfo =
                {
                    FileName = dotnet,
                    Arguments = processName,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = false
                }
            };
            // debugging
            p.Start();
            return p;
        }
    }
}