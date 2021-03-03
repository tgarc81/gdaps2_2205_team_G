using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Palingenesis
{
    class bullet : Character
    {

        public bullet(int moveSpeed, int Damage, Texture2D texture, Rectangle position, int windowHeight, int windowWidth) : base (moveSpeed, Damage, texture, position, windowHeight, windowWidth)
        {

        }

        public override void Update()
        {
            //TODO, figure out what needs to be updated 
        }
    }
}
