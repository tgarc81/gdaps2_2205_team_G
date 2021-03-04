using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Palingenesis
{
    //player the player plays as, it's kinda self explanitory 
    class Player : Character
    {
        private Rectangle attackBox;

        public Player (int health, int moveSpeed, int attackSpeed, int Damage, Texture2D texture, Rectangle position, int windowWidth, int windowHeight) : base (health, moveSpeed, attackSpeed, Damage, texture, position, windowWidth, windowHeight)
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
                position.Y -= 1;
            }

            //down
            if (keyboardState.IsKeyDown(Keys.S))
            {
                if(position.Y < windowHeight)
                position.Y += 1;
            }

            //right
            if (keyboardState.IsKeyDown(Keys.D))
            {
                if(position.X < windowWidth)
                position.X += 1;
            }

            //Left
            if (keyboardState.IsKeyDown(Keys.A))
            {
                //stops player from going off screen
                if(position.X > 0)
                position.X -= 1;
            }
        }

        //when annimating I guess we'll draw something that will fit within the attack box
        public void Attack(Boss target)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            


            if (keyboardState.IsKeyDown(Keys.Up))
            {
                //creates a rectangle 10 pixels above the player, will adjust exact values later
                 attackBox = new Rectangle(position.X, position.Y - 10, 10, 10);
            }
            //use else if so the player can only attack in one direction at a time
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                 attackBox = new Rectangle(position.X, position.Y + 10, 10, 10);
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                attackBox = new Rectangle(position.X + 10, position.Y, 10, 10);
            }
            else if (keyboardState.IsKeyDown(Keys.Left))
            {
                 attackBox = new Rectangle(position.X - 10, position.Y, 10, 10);
            }

            //hit check
            if (attackBox.Intersects(target.Position))
            {
                //each hit does 20 damage
                target.Health -= 20;
            }
        }
    }
}
