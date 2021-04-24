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
    public enum BulletType
    {
        //at the moment they can only move in straight lines
        Up,
        Down,
        Left,
        Right,
        None,
        SinLeft,
        SinRight,
        RiceGoddessSpecial,
        ring
    }

    //projectiles to be fired by bosses
    class Bullet : GameObject
    {
        //use 1,2,3,4 for up down left right
        private BulletType direction;
        private Texture2D texture;
        private GameObject target;
        private Song takeDamadge;
        private Color color = Color.White;
        private bool hasHit = false;
        private double timer = 0;

        //properties
        public BulletType Direction
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

        /// <summary>
        /// Bullet class constructor
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="takeDamadge"></param>
        /// <param name="windowHeight"></param>
        /// <param name="windowWidth"></param>
        /// <param name="direction"></param>
        /// <param name="target"></param>
        /// <param name="damage"></param>
        public Bullet(Texture2D texture, Rectangle position, Song takeDamadge, int windowHeight, int windowWidth, BulletType direction, GameObject target, int damage) : base(0, 10, 0, damage, texture, position, windowHeight, windowWidth)
        {
            this.texture = texture;
            this.direction = direction;
            this.target = target;
            this.takeDamadge = takeDamadge;
        }

        public override void Update()
        {
            if(Direction == BulletType.Up)
            {
                //moves up the screen by the set amount movespeed
                position.Y -= moveSpeed;
            }
            else if(direction == BulletType.Down)
            {
                position.Y += moveSpeed;
            }
            else if (direction == BulletType.Left)
            {
                position.X -= moveSpeed;
            }
            else if (direction == BulletType.Right)
            {
                position.X += moveSpeed;
            }
            else if(direction == BulletType.SinLeft)
            {
                position.X -= moveSpeed;
                
                position.Y += (int)Math.Sin(timer);
                timer += 0.001;
            }

            //for player intersection
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
