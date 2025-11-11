using System;
using System.Collections.Generic;
using ConsoleGameCore.Models;

namespace ConsoleGameCore
{
    public class Game
    {
        private Map map;
        private Player player;
        private List<Enemy> enemies = new List<Enemy>();
        private Random rng = new Random();
        private int tickCount = 0;
        private int nextSpawnTick = 5;
        private int killCount = 0;

        public Game(int width = 10, int height = 10)
        {
            map = new Map(width, height);
            player = new Player(map, width / 2, height / 2);
        }

        // runs the main game loop, each action (move/attack attempt) by the player is a tick
        public void Run()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                map.Clear();

                // place player on the map
                map.Set(player.X, player.Y, player.Symbol);
                Console.WriteLine("Use arrows or WASD to move. Q to quit.");
                Console.WriteLine($"Health: {player.Health}/10   Kills: {killCount}");

                // place enemies on the map (including dead ones with 'X' symbol)
                foreach (var enemy in enemies)
                    map.Set(enemy.X, enemy.Y, enemy.Symbol);

                map.Render();

                var key = Console.ReadKey(true).Key;
                bool moved = false;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        moved = player.TryMove(0, -1, enemies);
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        moved = player.TryMove(0, 1, enemies);
                        break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        moved = player.TryMove(-1, 0, enemies);
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        moved = player.TryMove(1, 0, enemies);
                        break;
                    case ConsoleKey.Q:
                        running = false;
                        break;
                }

                // advance game state on any player action attempt (move/attack, including failures like hitting a wall)
                // but NOT on quit
                if (key != ConsoleKey.Q && player.ActionAttempted)
                {
                    AdvanceTick();
                    player.ClearActionFlag();
                    RemoveDeadEnemies(); // Only count kills here
                }

                if (player.Health <= 0)
                {
                    Console.Clear();
                    Console.WriteLine("Game Over! You have been defeated. Kills: " + killCount);
                    running = false;
                }
            }
        }

        // advance the game state by one tick, updating enemies and spawning new ones
        private void AdvanceTick()
        {
            tickCount++;

            // spawn new enemy if it's time
            if (tickCount >= nextSpawnTick)
            {
                SpawnEnemy();
                nextSpawnTick += 5; // schedule next spawn
            }

            // update enemies
            foreach (var e in enemies)
            {
                e.Tick();
                e.Act(player, enemies);
            }

            // Set enemies with Health <= 0 to Dead mode
            foreach (var e in enemies)
            {
                if (e.Health <= 0 && e.Mode != EnemyMode.Dead)
                {
                    e.Mode = EnemyMode.Dead;
                    e.UpdateSymbol();
                }
            }
        }

        /// <summary>Remove dead enemies from the game and update kill count.</summary>
        private void RemoveDeadEnemies()
        {
            int before = enemies.Count;
            enemies.RemoveAll(e => e.Mode == EnemyMode.Dead);
            int after = enemies.Count;
            killCount += (before - after);
        }

        private void SpawnEnemy()
        {
            int w = map.Width, h = map.Height;
            Position spawnPos;
            int edge = rng.Next(0, 4);
            switch (edge)
            {
                case 0: spawnPos = new Position(rng.Next(0, w), 0); break;
                case 1: spawnPos = new Position(rng.Next(0, w), h - 1); break;
                case 2: spawnPos = new Position(0, rng.Next(0, h)); break;
                default: spawnPos = new Position(w - 1, rng.Next(0, h)); break;
            }

            // Keep trying until we find a valid spawn location that is not player or another enemy
            for (int i = 0; i < 10; i++)
            {
                bool occupied = spawnPos == player.Pos;
                if (!occupied)
                {
                    foreach (var enemy in enemies)
                    {
                        if (enemy.Pos == spawnPos && enemy.Health > 0)
                        {
                            occupied = true;
                            break;
                        }
                    }
                }
                if (!occupied)
                    break;
                spawnPos = new Position(rng.Next(0, w), rng.Next(0, h));
            }
            enemies.Add(new Enemy(map, "Goblin", health: 3, x: spawnPos.X, y: spawnPos.Y, spawnTick: tickCount));
        }
    }
}
