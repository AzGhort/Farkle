namespace Farkle
{
    internal class Program
    {
        /// <summary>
        /// Expects 2 arguments, paths to the players' dlls. If they are not specified, runs the game locally.
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                var runner = new GameRunner();
                runner.RunGameLocally();
                return;
            }

            var gameExecutor = new GameExecutor();
            gameExecutor.RunGame(args[0], args[1]);
        }
    }
}