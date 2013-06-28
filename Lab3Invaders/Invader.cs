using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Lab3Invaders
{
    enum ShipType
    {
        Bug,
        Saucer,
        Satellite,
        Spaceship,
        Star,
    }

    class Invader
    {
        private const int HorizontalInterval = 10;
        private const int VerticalInterval = 40;

        private Bitmap image;

        public Point Location { get; private set; }
        public ShipType InvaderType { get; private set; }

        public Rectangle Area
        {
            get
            {
                return new Rectangle(Location, image.Size);
            }
        }

        public int Score { get; private set; }

        public Invader(ShipType invaderType, Point location, int score)
        {
            this.InvaderType = invaderType;
            this.Location = location;
            this.Score = score;
            image = InvaderImage(0);
        }

        public void Draw(Graphics g, int animationCell)
        {
            g.DrawImageUnscaled(InvaderImage(animationCell), Location);
        }

        public void Move(Direction direction)
        {
            Point newLocation;
            if (direction == Direction.Left)
            {
                newLocation = new Point(Location.X - HorizontalInterval, Location.Y);
                Location = newLocation;
            }
            else if (direction == Direction.Right)
            {
                newLocation = new Point(Location.X + HorizontalInterval, Location.Y);
                Location = newLocation;
            }
            else if (direction == Direction.Down)
            {
                newLocation = new Point(Location.X, Location.Y + VerticalInterval);
                Location = newLocation;
            }
        }

        private Bitmap InvaderImage(int animationCell)
        {
            Bitmap newImage;
            switch (animationCell)
            {
                case 0: switch (InvaderType)
                    {
                        case ShipType.Bug: newImage = Properties.Resources.bug1; break;
                        case ShipType.Saucer: newImage = Properties.Resources.flyingsaucer1; break;
                        case ShipType.Satellite: newImage = Properties.Resources.satellite1; break;
                        case ShipType.Spaceship: newImage = Properties.Resources.spaceship1; break;
                        default: newImage = Properties.Resources.star1; break;
                    } break;
                case 1: switch (InvaderType)
                    {
                        case ShipType.Bug: newImage = Properties.Resources.bug2; break;
                        case ShipType.Saucer: newImage = Properties.Resources.flyingsaucer2; break;
                        case ShipType.Satellite: newImage = Properties.Resources.satellite2; break;
                        case ShipType.Spaceship: newImage = Properties.Resources.spaceship2; break;
                        default: newImage = Properties.Resources.star2; break;
                    } break;
                case 2: switch (InvaderType)
                    {
                        case ShipType.Bug: newImage = Properties.Resources.bug3; break;
                        case ShipType.Saucer: newImage = Properties.Resources.flyingsaucer3; break;
                        case ShipType.Satellite: newImage = Properties.Resources.satellite3; break;
                        case ShipType.Spaceship: newImage = Properties.Resources.spaceship3; break;
                        default: newImage = Properties.Resources.star3; break;
                    } break;
                default: switch (InvaderType)
                    {
                        case ShipType.Bug: newImage = Properties.Resources.bug4; break;
                        case ShipType.Saucer: newImage = Properties.Resources.flyingsaucer4; break;
                        case ShipType.Satellite: newImage = Properties.Resources.satellite4; break;
                        case ShipType.Spaceship: newImage = Properties.Resources.spaceship4; break;
                        default: newImage = Properties.Resources.star4; break;
                    } break;
            }
            return newImage;
        }
    }
}
