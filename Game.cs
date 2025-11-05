using System;
using ConsoleGameCore.Models;

namespace ConsoleGameCore
{
    public class Game
    {
        private Map map;
        private Player player;

        public Game(int width = 10, int height = 10)
        {
            map = new Map(width, height);
            player = new Player(map, width / 2, height / 2);
        }

        public void Run()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                map.Clear();
                map.Set(player.X, player.Y, player.Symbol);

                Console.WriteLine("Use arrows or WASD to move. Q to quit.");
                Console.WriteLine($"Health: {player.Health}/10");
                map.Render();

                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        player.Move(0, -1);
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        player.Move(0, 1);
                        break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        player.Move(-1, 0);
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        player.Move(1, 0);
                        break;
                    case ConsoleKey.Q:
                        running = false;
                        break;
                }
                if (player.Health <= 0)
                {
                    Console.Clear();
                    Console.WriteLine("Game Over! You have been defeated.");
                    running = false;
                }
            }
        }
    }
}