namespace Farkle
{
    internal class Program
    {
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