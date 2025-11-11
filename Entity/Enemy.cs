using System;
using System.Collections.Generic;
using ConsoleGameCore.Models;

namespace ConsoleGameCore
{
    /// <summary>Behavior mode for an enemy: stays put, warns, then attacks/moves.</summary>
    public enum EnemyMode { Stay, Warning, Active, Dead }

    /// <summary>
    /// Enemy entity with mode cycling, AI behavior, and spawn tick tracking.
    /// Inherits position and health management from Entity base class.
    /// </summary>
    public class Enemy : Entity
    {
        /// <summary>Enemy type name.</summary>
        public string Type { get; set; }

        /// <summary>Current mode in the cycle (Stay -> Warning -> Active -> Stay ...).</summary>
        public EnemyMode Mode { get; set; } = EnemyMode.Stay;

        /// <summary>Game tick when this enemy was spawned (used for collision priority).</summary>
        public int SpawnTick { get; set; }

        /// <summary>Create enemy at position with given type and health.</summary>
        /// <param name="map">Reference to the game map.</param>
        /// <param name="type">Enemy type name.</param>
        /// <param name="health">Starting health points.</param>
        /// <param name="x">Starting X coordinate.</param>
        /// <param name="y">Starting Y coordinate.</param>
        /// <param name="spawnTick">Game tick when this enemy spawned.</param>
        public Enemy(Map map, string type, int health, int x, int y, int spawnTick = 0)
            : base(map, x, y, health, symbol: 'E')
        {
            Type = type;
            SpawnTick = spawnTick;
            UpdateSymbol();
        }

        /// <summary>Advance the enemy's mode by one tick in the cycle: Stay -> Warning -> Active -> Stay.</summary>
        public void Tick()
        {
            if (Mode == EnemyMode.Dead)
            {
                UpdateSymbol();
                return;
            }
            Mode = Mode switch
            {
                EnemyMode.Stay => EnemyMode.Warning,
                EnemyMode.Warning => EnemyMode.Active,
                EnemyMode.Active => EnemyMode.Stay,
                _ => EnemyMode.Stay
            };
            UpdateSymbol();
        }

        /// <summary>Update the symbol based on current mode and health status.</summary>
        public void UpdateSymbol()
        {
            if (Mode == EnemyMode.Dead || Health <= 0)
                Symbol = 'X';
            else if (Mode == EnemyMode.Warning)
                Symbol = '!';
            else
                Symbol = 'E';
        }

        /// <summary>Perform the enemy action for this tick. Only acts during Active mode.</summary>
        /// <param name="player">Reference to the player.</param>
        /// <param name="enemies">List of all enemies for collision detection.</param>
        public bool Act(Player player, List<Enemy> enemies)
        {
            if (Mode != EnemyMode.Active || Health <= 0)
                return false;

            var (distX, distY) = Pos.DistanceTo(player.Pos);

            // Prioritize axis with greater distance
            if (distX > distY)
            {
                int dx = Math.Sign(player.Pos.X - Pos.X);
                if (!TryMoveAndCheckAttack(dx, 0, player, enemies))
                {
                    int dy = Math.Sign(player.Pos.Y - Pos.Y);
                    TryMoveAndCheckAttack(0, dy, player, enemies);
                }
            }
            else
            {
                int dy = Math.Sign(player.Pos.Y - Pos.Y);
                if (!TryMoveAndCheckAttack(0, dy, player, enemies))
                {
                    int dx = Math.Sign(player.Pos.X - Pos.X);
                    TryMoveAndCheckAttack(dx, 0, player, enemies);
                }
            }

            return false;
        }

        /// <summary>Attempt to move in direction (dx, dy) and check for attack or collision.</summary>
        /// <param name="dx">Change in X coordinate.</param>
        /// <param name="dy">Change in Y coordinate.</param>
        /// <param name="player">Reference to the player.</param>
        /// <param name="enemies">List of all enemies for collision detection.</param>
        /// <returns>True if move was successful, false otherwise.</returns>
        private bool TryMoveAndCheckAttack(int dx, int dy, Player player, List<Enemy> enemies)
        {
            Position newPos = Pos.Offset(dx, dy);

            // Check map bounds
            if (!map.InBounds(newPos))
                return false;

            // Check if moving into player
            if (newPos == player.Pos)
            {
                player.TakeDamage(1);
                return true;
            }

            // Check collision with other enemies using spawn tick priority
            foreach (var e in enemies)
            {
                if (e != this && e.Pos == newPos && e.Health > 0)
                {
                    // Only allow move if this enemy is older (has lower spawn tick)
                    if (SpawnTick < e.SpawnTick)
                    {
                        Pos = newPos;
                        return true;
                    }
                    // Newer enemy blocked by older enemy
                    return false;
                }
            }

            // No collision, move freely
            Pos = newPos;
            return true;
        }

        /// <summary>Take damage and update the symbol.</summary>
        public override void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Health = 0;
                Mode = EnemyMode.Dead;
            }
            UpdateSymbol();
        }
    }
}
