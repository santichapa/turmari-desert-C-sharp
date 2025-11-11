using System;
using System.Collections.Generic;
using ConsoleGameCore.Models;

namespace ConsoleGameCore.Models
{
    /// <summary>
    /// Player entity with movement and health management.
    /// Inherits common functionality from Entity base class.
    /// </summary>
    public class Player : Entity
    {
        /// <summary>Create player at starting position with default health of 10.</summary>
        /// <param name="map">Reference to the game map.</param>
        /// <param name="startX">Starting X coordinate.</param>
        /// <param name="startY">Starting Y coordinate.</param>
        public Player(Map map, int startX = 0, int startY = 0)
            : base(map, startX, startY, health: 10, symbol: 'P')
        {
        }

        /// <summary>True if the player attempted an action this turn (move/attack attempt, including failures).</summary>
        public bool ActionAttempted { get; private set; } = false;

        /// <summary>Attempt to move the player by dx, dy. Marks an action attempt regardless of success.</summary>
        /// <param name="dx">Change in X coordinate.</param>
        /// <param name="dy">Change in Y coordinate.</param>
        /// <param name="enemies">List of enemies to check for collision.</param>
        /// <returns>True if the player successfully moved, false otherwise.</returns>
        public bool TryMove(int dx, int dy, List<Enemy>? enemies = null)
        {
            ActionAttempted = true;
            Position newPos = Pos.Offset(dx, dy);

            // Check map bounds
            if (!map.InBounds(newPos))
            {
                return false;
            }

            // Check for enemy collision
            if (enemies != null)
            {
                foreach (var e in enemies)
                {
                    if (e.Pos == newPos && e.Health > 0)
                    {
                        // Enemy is in the way, damage it but don't move
                        e.TakeDamage(1);
                        return false;
                    }
                }
            }

            // No collision, move normally
            Pos = newPos;
            return true;
        }

        /// <summary>Clear the action flag after tick completes.</summary>
        public void ClearActionFlag()
        {
            ActionAttempted = false;
        }
    }
}
