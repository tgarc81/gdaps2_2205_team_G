using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Palingenesis
{
    class Player : Character
    {
       

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
    }
}
