using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//Name: G-Force
//Date: 3/16/2
//Professor Mesh
//Purpose: To initialize boss traits and behaviors.

namespace Palingenesis
{
    //player the player plays as, it's kinda self explanitory 
    class Player : Character
    {
        private Rectangle attackBox;
        private KeyboardState keyboardState;

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

        public Player (int health, int moveSpeed, int attackSpeed, int Damage, Texture2D texture, Rectangle position, int windowHeight, int windowWidth) : base (health, moveSpeed, attackSpeed, Damage, texture, position, windowHeight, windowWidth)
        {
            
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
                    position.Y -= 1;
                }
            }

            //down
            if (keyboardState.IsKeyDown(Keys.S))
            {
                if(position.Y < (windowHeight - position.Height))
                {
                    position.Y += 1;
                }
            }

            //right
            if (keyboardState.IsKeyDown(Keys.D))
            {
                if(position.X < (windowWidth - position.Width))
                {
                    position.X += 1;
                }
                
            }

            //Left
            if (keyboardState.IsKeyDown(Keys.A))
            {
                //stops player from going off screen
                if(position.X > 0)
                {
                    position.X -= 1;
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
                 attackBox = new Rectangle(position.X, position.Y - (100 - position.Height), 25, 50);
            }
            //use else if so the player can only attack in one direction at a time
            else if (keyboardState.IsKeyDown(Keys.Down) && prevKeyboardState.IsKeyUp(Keys.Down))
            {
                 attackBox = new Rectangle(position.X, position.Y + (100 - position.Height), 25, 50);
            }
            else if (keyboardState.IsKeyDown(Keys.Right) && prevKeyboardState.IsKeyUp(Keys.Right))
            {
                attackBox = new Rectangle(position.X + (100 - position.Width), position.Y, 50, 25);
            }
            else if (keyboardState.IsKeyDown(Keys.Left) && prevKeyboardState.IsKeyUp(Keys.Left))
            {
                 attackBox = new Rectangle(position.X - (100 - position.Width), position.Y, 50, 25);
            }

            //hit check
            if (attackBox.Intersects(target.Position))
            {
                //each hit does 20 damage
                target.Health -= 20;
            }

            

        }

        public void attackDraw(SpriteBatch sb, Texture2D texture)
        {
            if(keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.Left))
            sb.Draw(texture, attackBox, Color.White);
        }
    }
}
