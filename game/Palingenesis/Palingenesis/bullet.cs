using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//Name: G-Force
//Date: 3/16/2
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
    }
    //projectiles to be fired by bosses
    class Bullet : Character
    {
        //use 1,2,3,4 for up down left right
        private direction direction;
        private Texture2D texture;

        public direction Direction
        {
            get { return this.direction; }
            set { direction = value; }
        }

        //contructor
        public Bullet(Texture2D texture, Rectangle position, int windowHeight, int windowWidth) : base(0, 10, 0, 10, texture, position, windowHeight, windowWidth)
        {

        }

        public override void Update()
        {
            if(Direction == direction.up)
            {
                //moves up the screen by the set amount movespeed
                position.Y -= moveSpeed;
            }
            else if(Direction == direction.down)
            {
                position.Y += moveSpeed;
            }
            else if (Direction == direction.left)
            {
                position.X -= moveSpeed;
            }
            else if (Direction == direction.right)
            {
                position.X += moveSpeed;
            }
        }
    }
}
