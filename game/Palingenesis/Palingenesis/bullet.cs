using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Palingenesis
{
    //projectiles to be fired by bosses
    class bullet : Character
    {

        //contructor
        public bullet(int moveSpeed, int Damage, Texture2D texture, Rectangle position, int windowHeight, int windowWidth) : base(0, moveSpeed, 0, Damage, texture, position, windowHeight, windowWidth)
        {

        }

        public override void Update()
        {
            //TODO, figure out what needs to be updated 
        }
    }
}
