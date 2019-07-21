using System;
using System.Diagnostics;

namespace Farkle
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Expecting name of both players executables in args.");
                return;
            }
            GameExecutor gameExecutor = new GameExecutor(args[0], args[1]);
            gameExecutor.RunGame();
        }
    }
}
