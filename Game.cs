
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Lab3Invaders
{
    // Main class that controlls all aspects of the game
    class Game
    {
        // Initalization
        private int score = 0;
        private int livesLeft = 2;
        private int wave = 0;
        private int framesSkipped = 0;

        private Rectangle boundaries; // Playing field
        private Random random;

        private List<Invader> invaders;

        private PlayerShip playerShip;
        private List<Shot> playerShots;
        private List<Shot> invaderShots;

        private Stars stars;

        public event EventHandler GameOver;

        Direction directionToMove = Direction.Left;

        public Game(Random random, Rectangle boundaries)
        {
            // Create Invader, PlayerShip, List, Stars objects
            this.random = random;
            this.boundaries = boundaries;
            stars = new Stars(random, boundaries);
            playerShip = new PlayerShip(boundaries, true);
            playerShots = new List<Shot>();
            invaders = new List<Invader>();
            invaderShots = new List<Shot>();
            NextWave();
        }

        public void OnGameOver(EventArgs e)
        {
            EventHandler gameOver = GameOver;
            if (gameOver != null)
                gameOver(this, e);
        }

        // Paint the game for each frame
        public void Draw(Graphics g, int animationCell, bool gameOver) 
        {
            g.FillRectangle(Brushes.Black, boundaries);
            stars.Draw(g);
            playerShip.Draw(g);
            foreach (Invader invader in invaders)
                invader.Draw(g, animationCell);
            foreach (Shot shot in playerShots)
                shot.Draw(g);
            foreach (Shot shot in invaderShots)
                shot.Draw(g);
            using (Font font = new Font("Arial", 24, FontStyle.Bold))
                g.DrawString(score.ToString(), font, Brushes.Red, boundaries.X + 20, boundaries.Y + 20);
            g.DrawImageUnscaled(Properties.Resources.player,
                    new Point(boundaries.Right - 110, boundaries.Top + 10));
            using (Font font = new Font("Arial", 20))
                g.DrawString("X " + livesLeft, font, Brushes.Yellow, boundaries.Right - 50, boundaries.Top + 10);
            if (gameOver)
            {
                using (Font font = new Font("Arial", 40))
                    g.DrawString("Game Over", font, Brushes.Purple,
                        (boundaries.Width / 3), boundaries.Height / 3);
                using (Font font = new Font("Arial", 20))
                    g.DrawString("Press 'Q' to quit\nor 'S' to restart", font, Brushes.Purple,
                        boundaries.Width / 3 + 45, boundaries.Height / 3 + 50);
            }

        }
        
        public void Twinkle()
        {
            stars.Twinkle(random, boundaries);
        }

        public void MovePlayer(Direction direction)
        {
            if (playerShip.Alive)
            {
                playerShip.Move(direction);
            }
        }

        public void FireShot()
        {
            if (livesLeft > -1 && playerShip.Alive)
                if (playerShots.Count < 2)
                {
                    playerShots.Add(new Shot(new Point(playerShip.Location.X + 25, playerShip.Location.Y),
                                    Direction.Up, boundaries));
                }
        }
        // play game per frame
        public void Go()
        {
            if (livesLeft < 0)
                OnGameOver(new EventArgs());
            framesSkipped++;
            if (framesSkipped == 6)
            {
                MoveInvaders();
                for (int i = playerShots.Count - 1; i >= 0; i--)
                {
                        if (!playerShots[i].Move())
                        {
                            playerShots.RemoveAt(i);
                        }
                }
                if (playerShip.Alive)
                    ReturnFire();
                CheckForCollisions();
                for (int i = invaderShots.Count - 1; i >= 0; i--)
                {
                    if (!invaderShots[i].Move())
                    {
                        invaderShots.RemoveAt(i);
                    }
                }
                framesSkipped = 0;
            }
            if (invaders.Count == 0)
                NextWave();
        }

        // increase difficulty when each wave is cleared
        private void NextWave()
        {
            wave++;

            for (int i = playerShots.Count - 1; i >= 0; i--)
            {
                playerShots.RemoveAt(i);
            }

            for (int i = invaderShots.Count - 1; i >= 0; i--)
            {
                invaderShots.RemoveAt(i);
            }

            int locX = boundaries.X + 10;
            int locY = boundaries.Y;
            for (int i = 1; i < 7; i++)  // construct waves
            {
                invaders.Add(new Invader(ShipType.Satellite, new Point(
                                           locX, locY), 50));
                invaders.Add(new Invader(ShipType.Bug, new Point(
                                           locX, locY + 55), 40));
                invaders.Add(new Invader(ShipType.Saucer, new Point(
                                           locX, locY + 110), 30));
                invaders.Add(new Invader(ShipType.Spaceship, new Point(
                                           locX, locY + 150), 20));
                invaders.Add(new Invader(ShipType.Star, new Point(
                                           locX, locY + 190), 10));
                locX += 65;
            }
        }
        // check if a shot hit a target
        private void CheckForCollisions()
        {
            List<Shot> deadPlayerShots = new List<Shot>();
            List<Shot> deadInvaderShots = new List<Shot>();

            foreach (Shot shot in invaderShots)
            {
                if (playerShip.Area.Contains(shot.Location))
                {
                    livesLeft--;
                    deadInvaderShots.Add(shot);
                    playerShip.Alive = false;
                }
            }

            foreach (Shot shot in playerShots)
            {
                List<Invader> deadInvaders = new List<Invader>();
                foreach (Invader invader in invaders) 
                {
                    if (invader.Area.Contains(shot.Location))
                    {
                        deadInvaders.Add(invader);
                        deadPlayerShots.Add(shot);
                        score += invader.Score;
                    }
                }
                foreach (Invader invader in deadInvaders)
                    invaders.Remove(invader);
            }
            foreach (Shot shot in deadPlayerShots)
                playerShots.Remove(shot);
            foreach (Shot shot in deadInvaderShots)
                invaderShots.Remove(shot);

            
        }
        // control moving pattern
        private void MoveInvaders()
        {
            if (invaders.Count > 0)
            if (invaders[0].Location.X <= boundaries.X && directionToMove == Direction.Left)
                directionToMove = Direction.Down;
            else if (invaders[0].Location.X <= boundaries.X && directionToMove == Direction.Down)
                directionToMove = Direction.Right;
            else if (invaders[invaders.Count - 1].Location.X + 55 >= boundaries.Width && directionToMove == Direction.Right)
                directionToMove = Direction.Down;
            else if (invaders[invaders.Count - 1].Location.X <= boundaries.Width && directionToMove == Direction.Down)
                directionToMove = Direction.Left;

            foreach (Invader invader in invaders)
            {
                invader.Move(directionToMove);
                if (invader.Location.Y >= boundaries.Bottom - 50)
                    OnGameOver(new EventArgs());
            }
        }
        // query invader list and find an invader on the bottom to designate the shooter
        private void ReturnFire()
        {
            if (invaderShots.Count > wave + 1)
                return;
            if (random.Next(10) < 10 - wave)
                return;

            var results =
                from shooter in invaders
                group shooter by shooter.Location.X
                into shooterGroup
                orderby shooterGroup.Key descending
                select shooterGroup;

            int selectShooterGroup = random.Next(results.Count());
            int count = results.Count();
            if (count > 0)
            {
                var shooterColumn = results.ElementAt(selectShooterGroup);
                Invader invaderShooter = shooterColumn.Last();

                Point shotLocation = new Point(invaderShooter.Location.X + (invaderShooter.Area.Width / 2),
                                               invaderShooter.Location.Y + invaderShooter.Area.Height);
                Shot newShot = new Shot(shotLocation, Direction.Down, boundaries);
                invaderShots.Add(newShot);
            }
        }
    }
}
