using System;

namespace ConsoleGameCore.Models
{
    /// <summary>
    /// Base class for all entities in the game (Player, Enemy, etc).
    /// Provides common functionality for position, health, and movement.
    /// </summary>
    public abstract class Entity
    {
        /// <summary>Position on the map.</summary>
        public Position Pos { get; protected set; }

        /// <summary>X coordinate convenience property.</summary>
        public int X => Pos.X;

        /// <summary>Y coordinate convenience property.</summary>
        public int Y => Pos.Y;

        /// <summary>Character used to draw the entity on the map.</summary>
        public char Symbol { get; protected set; }

        /// <summary>Current health points.</summary>
        public int Health { get; protected set; }

        /// <summary>Reference to the game map.</summary>
        protected Map map;

        /// <summary>Initialize entity at position with given health.</summary>
        /// <param name="map">Reference to the game map.</param>
        /// <param name="x">Starting X coordinate.</param>
        /// <param name="y">Starting Y coordinate.</param>
        /// <param name="health">Starting health points.</param>
        /// <param name="symbol">Character representation on the map.</param>
        public Entity(Map map, int x, int y, int health, char symbol)
        {
            this.map = map;
            Pos = new Position(x, y);
            Health = health;
            Symbol = symbol;
        }

        /// <summary>Attempt to move the entity by dx, dy. Returns true when position changed.</summary>
        /// <param name="dx">Change in X coordinate.</param>
        /// <param name="dy">Change in Y coordinate.</param>
        /// <returns>True if the entity successfully moved, false otherwise.</returns>
        public virtual bool Move(int dx, int dy)
        {
            Position newPos = Pos.Offset(dx, dy);

            if (map.InBounds(newPos))
            {
                Pos = newPos;
                return true;
            }
            return false;
        }

        /// <summary>Reduce entity health by damage amount.</summary>
        /// <param name="damage">Amount of damage to take.</param>
        public virtual void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0) Health = 0;
        }

        /// <summary>Check if entity is alive.</summary>
        public bool IsAlive => Health > 0;
    }
}
