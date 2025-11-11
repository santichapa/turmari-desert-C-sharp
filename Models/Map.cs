using System;

namespace ConsoleGameCore.Models
{
    public class Map
    {
        public int Width { get; }
        public int Height { get; }
        private char[,] cells;

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            cells = new char[Height, Width];
            Clear();
        }

        public void Clear()
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    cells[y, x] = '·';
        }

        public void Set(int x, int y, char c)
        {
            if (InBounds(x, y)) cells[y, x] = c;
        }

        public void Set(Position pos, char c)
        {
            if (InBounds(pos)) cells[pos.Y, pos.X] = c;
        }

        public bool InBounds(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;

        public bool InBounds(Position pos) => pos.X >= 0 && pos.Y >= 0 && pos.X < Width && pos.Y < Height;

        public void Render()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                    Console.Write(cells[y, x]);
                Console.WriteLine();
            }
        }
    }
}