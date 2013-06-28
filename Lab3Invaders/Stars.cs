using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Lab3Invaders
{
    class Stars
    {
        private struct Star
        {
            public Point point;
            public Pen pen;

            public Star(Point point, Pen pen)
            {
                this.point = point;
                this.pen = pen;
            }
        }

        private List<Star> starList;

        public Stars(Random random, Rectangle boundaries)
        {
            starList = new List<Star>();
            for (int i = 0; i < 300; i++)
            {
                starList.Add(AddStar(random, boundaries));
            }
        }

        public void Draw(Graphics g)
        {
            foreach (Star star in starList)
            {
                g.DrawLine(star.pen, star.point, new Point(star.point.X + 1, star.point.Y + 1));
            }
        }

        public void Twinkle(Random random, Rectangle boundaries)
        {
            int index;
            for (int i = 0; i < 5; i++)
            {
                index = random.Next(0, starList.Count);
                starList.RemoveAt(index);
                starList.Add(AddStar(random, boundaries));
            }
        }

        private Star AddStar(Random random, Rectangle boundaries)
        {
            Star starToAdd = new Star();
            starToAdd.point = new Point(random.Next(0, boundaries.Width),
                                            random.Next(0, boundaries.Height));
            starToAdd.pen = RandomPen(random);
            return starToAdd;
        }

        private Pen RandomPen(Random random)
        {
            int colorNumber = random.Next(0, 4);
            Color colorType = new Color();
            switch (colorNumber) 
            {
                case 0: colorType = Color.White; break;
                case 1: colorType = Color.Yellow; break;
                case 2: colorType = Color.MistyRose; break;
                default: colorType = Color.LightCyan; break;
            }
            Pen randomPen = new Pen(colorType, 1.0F);
            return randomPen;
        }
    }
}
