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

        public Player (int health, int moveSpeed, int attackSpeed, int Damage, Texture2D texture, Rectangle position) : Base(int health, int moveSpeed, int attackSpeed, int Damage, Texture2D texture, Rectangle position)
        {

        }
    }
}
