using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Palingenesis
{
    //parent class for player and boss
    abstract class Character
    {
        int health;
        int moveSpeed;
        int attackSpeed;
        int damage;
        Texture2D texture;
        Rectangle Position;

        public Character(int health, int moveSpeed, int attackSpeed, int Damage, Texture2D texture, Rectangle position)
        {
            this.health = health;
            this.moveSpeed = moveSpeed;
            this.attackSpeed = attackSpeed;
            this.damage = damage;
            this.texture = texture;
        }
    }
}
