using System;

namespace ConsoleGameCore.Models
{
    /// <summary>
    /// Represents a 2D position on the game map.
    /// An immutable struct for clean coordinate handling.
    /// </summary>
    public struct Position : IEquatable<Position>
    {
        /// <summary>X coordinate (column).</summary>
        public int X { get; }

        /// <summary>Y coordinate (row).</summary>
        public int Y { get; }

        /// <summary>Create a position at the given coordinates.</summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>Calculate the Manhattan distance between this position and another.</summary>
        /// <param name="other">The other position.</param>
        /// <returns>The Manhattan distance.</returns>
        public int ManhattanDistance(Position other)
            => Math.Abs(X - other.X) + Math.Abs(Y - other.Y);

        /// <summary>Get the distance along each axis to another position.</summary>
        /// <param name="other">The other position.</param>
        /// <returns>A tuple with (distX, distY).</returns>
        public (int distX, int distY) DistanceTo(Position other)
            => (Math.Abs(X - other.X), Math.Abs(Y - other.Y));

        /// <summary>Create a new position offset by the given delta.</summary>
        /// <param name="dx">Change in X.</param>
        /// <param name="dy">Change in Y.</param>
        /// <returns>A new position offset by the delta.</returns>
        public Position Offset(int dx, int dy) => new Position(X + dx, Y + dy);

        /// <summary>Check equality with another position.</summary>
        public bool Equals(Position other) => X == other.X && Y == other.Y;

        /// <summary>Check equality with an object.</summary>
        public override bool Equals(object? obj) => obj is Position pos && Equals(pos);

        /// <summary>Get hash code for use in collections.</summary>
        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();

        /// <summary>Get string representation.</summary>
        public override string ToString() => $"({X}, {Y})";

        /// <summary>Compare two positions for equality.</summary>
        public static bool operator ==(Position left, Position right) => left.Equals(right);

        /// <summary>Compare two positions for inequality.</summary>
        public static bool operator !=(Position left, Position right) => !left.Equals(right);

        /// <summary>Add a delta to a position.</summary>
        public static Position operator +(Position pos, (int dx, int dy) delta)
            => new Position(pos.X + delta.dx, pos.Y + delta.dy);

        /// <summary>Subtract a delta from a position.</summary>
        public static Position operator -(Position pos, (int dx, int dy) delta)
            => new Position(pos.X - delta.dx, pos.Y - delta.dy);
    }
}
