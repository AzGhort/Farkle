using System.Diagnostics;

namespace Farkle
{
    /// <summary>
    /// </summary>
    internal class GameExecutor
    {
        private readonly GameRunner runner = new GameRunner();
        
        public void RunGame(string processName1, string processName2)
        {
            var player1Process = StartPlayerProcess(processName1);
            var player2Process = StartPlayerProcess(processName2);
            runner.RunGameExternally(player1Process, player2Process);
        }

        private Process StartPlayerProcess(string processName)
        {
            var p = new Process
            {
                StartInfo =
                {
                    FileName = processName,
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