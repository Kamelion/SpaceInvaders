﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Lab3Invaders
{
    enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    public partial class Form1 : Form
    {
        public bool gameOver = false;
        private Game game;
        private Random random;

        public Form1()
        {
            InitializeComponent();
            random = new Random();
            game = new Game(random, this.ClientRectangle);  // initialize game with boundary size
            game.GameOver += new EventHandler(game_GameOver);
        }

        void game_GameOver(object sender, EventArgs e)
        {
            gameOver = true;
            gameTimer.Stop();
            Invalidate();
        }

        List<Keys> keysPressed = new List<Keys>();
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Q)
                Application.Exit();

            if (gameOver)
                if (e.KeyCode == Keys.S)
                {
                    // code to reset the game and restart the timers
                    game = new Game(random, this.ClientRectangle);
                    gameTimer.Start();
                    gameOver = false;
                    return;
                }
            if (e.KeyCode == Keys.Space)
                game.FireShot();
            if (keysPressed.Contains(e.KeyCode))
                keysPressed.Remove(e.KeyCode);
            keysPressed.Add(e.KeyCode);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (keysPressed.Contains(e.KeyCode))
                keysPressed.Remove(e.KeyCode);
        }
        // timer for frames
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            game.Go();
            foreach (Keys key in keysPressed)
            {
                if (key == Keys.Left)
                {
                    game.MovePlayer(Direction.Left);
                    return;
                }
                else if (key == Keys.Right)
                {
                    game.MovePlayer(Direction.Right);
                    return;
                }
            }
        }

        int frame = 0;
        int cell = 0;
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            game.Draw(e.Graphics, cell, gameOver);
        }

        private void animationTimer_Tick(object sender, EventArgs e)
        {
            frame++;
            if (frame >= 6)
                frame = 0;
            switch (frame)
            {
                case 0: cell = 0; break;
                case 1: cell = 1; break;
                case 2: cell = 2; break;
                case 3: cell = 3; break;
                case 4: cell = 2; break;
                case 5: cell = 1; break;
                default: cell = 0; break;
            }
            game.Twinkle();
            this.Refresh();
        }
    }
}
