using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

//Name: G-Force
//Date: 3/16/21
//Professor Mesh
//Purpose: To initialize bullet traits and behaviors.

namespace Palingenesis
{
    public enum direction
    {
        //at the moment they can only move in straight lines
        up,
        down,
        left,
        right,
        none,
    }
    //projectiles to be fired by bosses
    class Bullet : GameObject
    {
        //use 1,2,3,4 for up down left right
        private direction direction;
        private Texture2D texture;
        private GameObject target;
        private Song takeDamadge;
        private bool hasHit = false;
        private Color color = Color.Red;
       
        public direction Direction
        {
            get { return this.direction; }
            set { direction = value; }
        }

        public bool HasHit
        {
            get { return hasHit; }
            set { hasHit = value; }
        }

        public Color Color
        {
            set { color = value; }
        }

        //contructor
        public Bullet(Texture2D texture, Rectangle position, Song takeDamadge, int windowHeight, int windowWidth, direction direction, GameObject target, int damage) : base(0, 10, 0, damage, texture, position, windowHeight, windowWidth)
        {
            this.texture = texture;
            this.direction = direction;
            this.target = target;
            this.takeDamadge = takeDamadge;
        }

        public override void Update()
        {
            if(Direction == direction.up)
            {
                //moves up the screen by the set amount movespeed
                position.Y -= moveSpeed;
            }
            else if(direction == direction.down)
            {
                position.Y += moveSpeed;
            }
            else if (direction == direction.left)
            {
                position.X -= moveSpeed;
            }
            else if (direction == direction.right)
            {
                position.X += moveSpeed;
            }

            if (position.Intersects(target.Position))
            {
                MediaPlayer.Play(takeDamadge);
                target.Health -= damage;
                hasHit = true;
            }

        }

        public override void Draw(SpriteBatch sb)
        {
            //only drawn while on screen
            if(position.X > (0 - position.Width) && position.X < (windowWidth + 10) && position.Y > (0 - position.Height) && position.Y < (windowHeight + 10))
            sb.Draw(texture, position, color);
        }

    }
}
