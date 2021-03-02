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
        protected int health;
        protected int moveSpeed;
        protected int attackSpeed;
        protected int damage;
        protected Texture2D texture;
        protected Rectangle position;
        protected int windowWidth;
        protected int windowHeight;

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public Character(int health, int moveSpeed, int attackSpeed, int Damage, Texture2D texture, Rectangle position, int windowHeight, int windowWidth)
        {
            this.health = health;
            this.moveSpeed = moveSpeed;
            this.attackSpeed = attackSpeed;
            this.damage = Damage;
            this.texture = texture;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
        }

        //draws object
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, Color.White);
        }

        public abstract void Update();
    }
}
