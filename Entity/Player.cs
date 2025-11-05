namespace ConsoleGameCore.Models
{
    public class Player
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Health { get; set; } = 10;
        public char Symbol { get; } = 'P';
        private Map map;

        public Player(Map map, int startX = 0, int startY = 0)
        {
            this.map = map;
            X = startX;
            Y = startY;
        }

        public void Move(int dx, int dy)
        {
            int nx = X + dx, ny = Y + dy;
            if (map.InBounds(nx, ny))
            {
                X = nx;
                Y = ny;
            }
            else
            {
                Health--;
            }
        }
    }
}