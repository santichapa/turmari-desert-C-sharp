using System;

namespace ConsoleGameCore
{
    class Program
    {
        /// <summary>
        /// Application main method. Creates a Game instance and starts the game loop.
        /// </summary>
        /// <param name="args">Command-line arguments (ignored).</param>
        static void Main(string[] args)
        {
            var game = new Game(width: 20, height: 20);
            game.Run();
        }
    }
}