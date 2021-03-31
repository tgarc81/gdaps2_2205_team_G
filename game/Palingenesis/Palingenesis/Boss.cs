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
        private Song takeDamage;

        public List<Bullet> ProjectileList
        {
            get { return projectileList; }
        }

        public Boss (int health, int moveSpeed, int attackSpeed, int Damage, Texture2D texture, Rectangle position, Song takeDamage, int windowWidth, int windowHeight, bossName type, Texture2D bulletTexture) : base(health, moveSpeed, attackSpeed, Damage, texture, position, windowWidth, windowHeight)
        {
            this.type = type;
            this.bulletTexture = bulletTexture;
            this.texture = texture;
            this.takeDamage = takeDamage;
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
               
                projectileList.Add(new Bullet(bulletTexture, position, takeDamage, this.windowHeight, this.windowWidth, direction, target, this.damage));
                position.X += xSpacing;
                position.Y += ySpacing;

            }
        }

        //used to decide what attack the boss will use with a random
        public void AI(Random rng, Player target)
        {
           
                int tmp = rng.Next(0, 3);
                if (tmp == 0)
                {
                    Line(target);
                }
                else if (tmp == 1)
                {
                    MegaShot(target);
                }
                else
                {
                    //Ring(target,);
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
                CreateProjectiles(rng.Next(5, 11), direction.right, new Rectangle((this.Position.X + 100), 0, 25, 25), 0, 100, target);
            }

            else
            {
                CreateProjectiles(rng.Next(5, 11), direction.left, new Rectangle((this.Position.X - 100), 0, 25, 25), 0, 100, target);
            }

          
        }

        //spawns a ring of shots around the boss to make the player retreat
        public void Ring(Player target, double timer)
        {
            //creates a ring of 9 bullets that don't move offset by 100 (in the y x or both directions) which will damage the player if they make contact
            Bullet topLeftCorner = new Bullet(bulletTexture, new Rectangle(this.Position.X - 100, this.position.Y - 100, 50, 50), this.takeDamage, windowHeight, windowWidth, direction.none, target, this.damage);
            Bullet leftMiddle = new Bullet(bulletTexture, new Rectangle(this.Position.X - 100, this.position.Y, 50, 50), this.takeDamage, windowHeight, windowWidth, direction.none, target, this.damage);
            Bullet bottomLeftCorner = new Bullet(bulletTexture, new Rectangle(this.Position.X - 100, this.position.Y + 100, 50, 50), this.takeDamage, windowHeight, windowWidth, direction.none, target, this.damage);

            Bullet bottom = new Bullet(bulletTexture, new Rectangle(this.Position.X, this.position.Y + 100, 50, 50), this.takeDamage, windowHeight, windowWidth, direction.none, target, this.damage);
            Bullet top = new Bullet(bulletTexture, new Rectangle(this.Position.X, this.position.Y + 100, 50, 50), this.takeDamage, windowHeight, windowWidth, direction.none, target, this.damage);

            Bullet topRightCorner = new Bullet(bulletTexture, new Rectangle(this.Position.X + (position.Width + 100), this.position.Y - 100, 50, 50), this.takeDamage, windowHeight, windowWidth, direction.none, target, this.damage);
            Bullet rightMiddle = new Bullet(bulletTexture, new Rectangle(this.Position.X + (position.Width + 100), this.position.Y, 50, 50), this.takeDamage, windowHeight, windowWidth, direction.none, target, this.damage);
            Bullet bottomRightCorner = new Bullet(bulletTexture, new Rectangle(this.Position.X + (position.Width + 100), this.position.Y + 100, 50, 50), this.takeDamage, windowHeight, windowWidth, direction.none, target, this.damage);

            //creates a shell list so that I can loop through all of the ring bullets
            List<Bullet> ringList = new List<Bullet>();

            ringList.Add(topLeftCorner);
            ringList.Add(leftMiddle);
            ringList.Add(bottomLeftCorner);
            ringList.Add(bottom);
            ringList.Add(top);
            ringList.Add(topRightCorner);
            ringList.Add(rightMiddle);
            ringList.Add(bottomRightCorner);

            for(int i = 0; i < ringList.Count; i++)
            {
                //adds each of the ring bullets to the main list
                projectileList.Add(ringList[i]);
            }

            //after 2 seconds have passed
            if(timer >= 2)
            {
                for(int i = 0; i < ringList.Count; i++)
                {
                    projectileList.Remove(ringList[i]);
                }
            }

        }

        //fires out shots in a circle around the boss
        public void Circle()
        {

        }

        //boss moves 3 times and if it makes contact with the player deals damage
        public void Charge()
        {
            {

            }
        }
        //fires a single large shot that does 2x damage at the player
        public void MegaShot(Player target)
        {
            if (target.Position.X > this.position.X)
            {
                projectileList.Add(new Bullet(bulletTexture, new Rectangle((this.Position.X + 100), target.Position.Y, 100, 100), this.takeDamage, windowHeight, windowWidth, direction.right, target, this.damage * 2));
            }

            else
            {
                projectileList.Add(new Bullet(bulletTexture, new Rectangle((this.Position.X - 100), target.Position.Y, 100, 100), this.takeDamage, windowHeight, windowWidth, direction.left, target, this.damage * 2));
            }
        }

            public void SpecialAttack()
            {
                if(type == bossName.RiceGoddess)
                {

                }
            }

        public void Reset()
        {
            health = 1000;
            position.X = 960;
            position.Y = 540;
            projectileList.Clear();
        }
    }
}
