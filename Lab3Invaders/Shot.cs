using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Lab3Invaders
{
    class Shot
    {
        private const int moveInterval = 20;
        private const int width = 5;
        private const int height = 15;

        public Point Location { get; private set; }

        private Direction direction;
        private Rectangle boundaries;

        public Shot(Point location, Direction direction, Rectangle boundaries)
        {
            this.Location = location;
            this.direction = direction;
            this.boundaries = boundaries;
        }

        public void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.Yellow, Location.X, Location.Y - height, width, height);
        }

        public bool Move()
        {
            Point newLocation = new Point();
            if (direction == Direction.Up)
            {
                newLocation.Y = Location.Y - moveInterval;
                newLocation.X = Location.X;
                Location = newLocation;
                if (Location.Y <= boundaries.Top - height)
                    return false;
                else
                    return true;
            }
            else //direction == Direction.Down
            {
                newLocation.Y = Location.Y + moveInterval;
                newLocation.X = Location.X;
                Location = newLocation;
                if (Location.Y >= boundaries.Bottom)
                    return false;
                else
                    return true;
            }
        }
    }
}
