using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Palingenesis
{
    public enum bossName
    {
        RiceGoddess,

    }
    class Boss : Character
    {
        private bossName type;
        private Point playerPosition;
        private List<Bullet> projectileList = new List<Bullet>();
        private Random rng = new Random();
        private Texture2D bulletTexture;
        public Boss (int health, int moveSpeed, int attackSpeed, int Damage, Texture2D texture, Rectangle position, int windowWidth, int windowHeight, bossName type, Texture2D bulletTexture) : base(health, moveSpeed, attackSpeed, Damage, texture, position, windowWidth, windowHeight)
        {
            this.type = type;
            this.bulletTexture = bulletTexture;
        }

        public override void Update()
        {
            //does nothing for now
        }

        //helper method fills bullet list
        public void CreateProjectiles(int amount, direction direction, Rectangle position, int xSpacing, int ySpacing)
        {
            for(int i = 0; i < amount; i++)
            {
               
                projectileList.Add(new Bullet(bulletTexture, position, this.windowHeight, this.windowWidth));
                position.X += xSpacing;
                position.Y += ySpacing;

            }
        }

        //used to decide what attack the boss will use with a random
        public void AI()
        {

        }

        //standard attacks
        public void Line(Player target)
        {

            //if the player is to the left of the boss
            if(target.Position.X < this.position.X)
            {
                //creates between 1 and 5 projectiles that fly to the left
                //first bullet is created at Y = 100 and each bullet after that is 25 pixels further down the screen
                CreateProjectiles(rng.Next(1, 6), direction.left, new Rectangle((this.Position.X - 50), 100, 25, 25), 0, 25);
            }

        }
        public void Circle()
        {

        }

        public void specialAttack()
        {
            if(type == bossName.RiceGoddess)
            {

            }
        }
    }
}
