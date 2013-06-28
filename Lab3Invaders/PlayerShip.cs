using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Lab3Invaders
{
    class PlayerShip
    {
        public Point Location { get; private set; }
        private Bitmap image;

        private bool alive;
        public bool Alive
        {
            get { return alive; }
            set
            {
                alive = value;
                if (!value)
                    wait = DateTime.Now;
            }
        }
        private Rectangle boundaries;
        private DateTime wait;
        
        public Rectangle Area
        {
            get
            {
                return new Rectangle(Location, image.Size);
            }
        }

        public PlayerShip(Rectangle boundaries, bool Alive)
        {
            this.boundaries = boundaries;
            this.Alive = Alive;
            image = Properties.Resources.player;
            Location = new Point((boundaries.Width / 2) - image.Width / 2, boundaries.Height - 35);
        }

        public void Draw(Graphics g)
        {
            if (Alive)
            {
                g.DrawImageUnscaled(image, Location);
            }
            else
            {
                if (DateTime.Now - wait < TimeSpan.FromSeconds(1))
                    g.DrawImage(image, Location.X, Location.Y, image.Width, image.Height / 2);
                else if (DateTime.Now - wait > TimeSpan.FromSeconds(1) &&
                    DateTime.Now - wait < TimeSpan.FromSeconds(2))
                    g.DrawImage(image, Location.X, Location.Y, image.Width, image.Height / 4);
                else if (DateTime.Now - wait > TimeSpan.FromSeconds(2) &&
                    DateTime.Now - wait < TimeSpan.FromSeconds(3))
                    g.DrawImage(image, Location.X, Location.Y, image.Width, image.Height / 8);
                else
                    Alive = true;
            }
        }

        public void Move(Direction direction)
        {
            Point newLocation;
            if (direction == Direction.Left && Location.X >= boundaries.X + 5)
            {
                newLocation = new Point(Location.X - 5, Location.Y);
                Location = newLocation;
            }
            else if (direction == Direction.Right && Location.X <= boundaries.Width - 60)
            {
                newLocation = new Point(Location.X + 5, Location.Y);
                Location = newLocation;
            }
        }
    }
}
