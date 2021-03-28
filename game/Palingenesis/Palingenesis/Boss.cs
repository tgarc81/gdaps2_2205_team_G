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
//Purpose: To initialize boss traits and behaviors.

namespace Palingenesis
{
    public enum bossName
    {
        RiceGoddess,

    }
    class Boss : GameObject
    {
        private bossName type;
        private Point playerPosition;
        private List<Bullet> projectileList = new List<Bullet>();
        private Random rng = new Random();
        private Texture2D bulletTexture;
        private Song takeDamadge;

        public List<Bullet> ProjectileList
        {
            get { return projectileList; }
        }

        public Boss (int health, int moveSpeed, int attackSpeed, int Damage, Texture2D texture, Rectangle position, Song takeDamadge, int windowWidth, int windowHeight, bossName type, Texture2D bulletTexture) : base(health, moveSpeed, attackSpeed, Damage, texture, position, windowWidth, windowHeight)
        {
            this.type = type;
            this.bulletTexture = bulletTexture;
            this.texture = texture;
            this.takeDamadge = takeDamadge;
        }



        public override void Update()
        {
            //does nothing for now
        }

        //helper method fills bullet list
        public void CreateProjectiles(int amount, direction direction, Rectangle position, int xSpacing, int ySpacing, Player target)
        {
            for(int i = 0; i < amount; i++)
            {
               
                projectileList.Add(new Bullet(bulletTexture, position, takeDamadge, this.windowHeight, this.windowWidth, direction, target, 10));
                position.X += xSpacing;
                position.Y += ySpacing;

            }
        }

        //used to decide what attack the boss will use with a random
        public void AI(Random rng, Player target)
        {
            if(rng.NextDouble() < 1)
            {
                Line(target);
            }

            
        }

        //standard attacks

        public void Line(Player target) //currently busted 
        {

            //if the player is to the left of the boss
            if(target.Position.X > this.position.X)
            {
                //creates between 1 and 5 projectiles that fly to the left
                //each is set 50 pixels to the left of the boss
                ///each bullet spawns 75 pixels apart on the y axis
                CreateProjectiles(rng.Next(5, 10), direction.right, new Rectangle((this.Position.X + 100), 0, 25, 25), 0, 100, target);
            }

            else
            {
                CreateProjectiles(rng.Next(5, 10), direction.left, new Rectangle((this.Position.X - 100), 0, 25, 25), 0, 100, target);
            }

          
        }

        //spawns a ring of shots around the boss to make the player retreat
        public void Ring()
        {

        }

        //fires out shots in a circle around the boss
        public void Circle()
        {

        }

        //boss moves 3 times and if it makes contact with the player deals damage
        public void Charge()
        {

        }

        //fires a single large shot that does 2x damage at the player
        public void MegaShot()
        {

        }

        public void SpecialAttack()
        {
            if(type == bossName.RiceGoddess)
            {

            }
        }
    }
}
