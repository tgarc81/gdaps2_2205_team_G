using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//Name: G-Force
//Date: 3/16/2
//Professor Mesh
//Purpose: To initialize player traits and movement.

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

        public Rectangle Position 
        {
            get { return position; }

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
            this.position = position;
            this.texture = texture;
        }

        //draws object
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, Color.White);
        }

        //checks if the character is overlaping with a different rectangle, can be used by all inheriting classes
        public virtual bool CheckCollision(Rectangle collider)
        {
            if (position.Intersects(collider))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public abstract void Update();


        //can center either the player or boss on screen
        public void Center()
        {
            position.X = (windowWidth / 2) - (position.Width / 4);
            position.Y = (windowHeight / 2) - (position.Height / 4);
        }
    }
}
