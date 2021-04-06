using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

//Name: G-Force
//Date: 3/16/21
//Professor Mesh
//Purpose: To initialize boss traits and behaviors.

namespace Palingenesis
{
    //player the player plays as, it's kinda self explanitory 
    class Player : GameObject
    {
        private List<Bullet> shotList = new List<Bullet>();
        private KeyboardState keyboardState;
        private Texture2D shotTexture;
        private Song takeDamadge;
        //for drawing based on player input
        private enum PlayerState
        {
            FaceForward,
            FaceRight,
            FaceLeft,
            FaceBack,
            WalkForward,
            WalkRight,
            WalkLeft,
            WalkBack
        }

        public List<Bullet> ShotList
        {
            get { return shotList; }
        }

        public Player (int health, int moveSpeed, int attackSpeed, int Damage, Texture2D texture, Rectangle position, Song takeDamadge, int windowHeight, int windowWidth, Texture2D shotTexture) : base (health, moveSpeed, attackSpeed, Damage, texture, position, windowHeight, windowWidth)
        {
            this.shotTexture = shotTexture;
            this.takeDamadge = takeDamadge;
        }

        public override void Update()
        {
            //creates keyboard object
            KeyboardState keyboardState = Keyboard.GetState();

            //WASD movement
            //up
            if (keyboardState.IsKeyDown(Keys.W))
            {
                //stops player from going off screen
                if (position.Y > 0)
                {
                    position.Y -= moveSpeed;
                }
            }

            //down
            if (keyboardState.IsKeyDown(Keys.S))
            {
                if(position.Y < (windowHeight - position.Height))
                {
                    position.Y += moveSpeed;
                }
            }

            //right
            if (keyboardState.IsKeyDown(Keys.D))
            {
                if(position.X < (windowWidth - position.Width))
                {
                    position.X += moveSpeed;
                }
                
            }

            //Left
            if (keyboardState.IsKeyDown(Keys.A))
            {
                //stops player from going off screen
                if(position.X > 0)
                {
                    position.X -= moveSpeed;
                }
                
            }
        }

        //when annimating I guess we'll draw something that will fit within the attack box
        public void Attack(Boss target, KeyboardState prevKeyboardState)
        {
            keyboardState = Keyboard.GetState();



            if (keyboardState.IsKeyDown(Keys.Up) && prevKeyboardState.IsKeyUp(Keys.Up))
            {
                //creates a rectangle 10 pixels above the player, will adjust exact values later

                shotList.Add(new Bullet(shotTexture, new Rectangle(position.X, position.Y, 20, 20), takeDamadge, windowHeight, windowWidth, Direction.up, target, 20));
                
            }
            //use else if so the player can only attack in one direction at a time
            else if (keyboardState.IsKeyDown(Keys.Down) && prevKeyboardState.IsKeyUp(Keys.Down))
            {
                shotList.Add(new Bullet(shotTexture, new Rectangle(position.X, position.Y, 20, 20), takeDamadge, windowHeight, windowWidth, Direction.down, target, 20));
            }
            else if (keyboardState.IsKeyDown(Keys.Right) && prevKeyboardState.IsKeyUp(Keys.Right))
            {
                shotList.Add(new Bullet(shotTexture, new Rectangle(position.X, position.Y, 20, 20), takeDamadge, windowHeight, windowWidth, Direction.right, target, 20));
            }
            else if (keyboardState.IsKeyDown(Keys.Left) && prevKeyboardState.IsKeyUp(Keys.Left))
            {
                shotList.Add(new Bullet(shotTexture, new Rectangle(position.X, position.Y, 20, 20), takeDamadge, windowHeight, windowWidth, Direction.left, target, 20));
            }

            

        }

        

        public void Reset()
        {
            health = 100;
            position.X = 200;
            position.Y = 200;
            shotList.Clear();
        }
    }
}
