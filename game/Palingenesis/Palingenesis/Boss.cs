using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Palingenesis
{
    class Boss : Character
    {

        public Boss (int health, int moveSpeed, int attackSpeed, int Damage, Texture2D texture, Rectangle position, int windowWidth, int windowHeight) : base(health, moveSpeed, attackSpeed, Damage, texture, position, windowWidth, windowHeight)
        {

        }

        public override void Update()
        {
            //does nothing for now
        }
    }
}
