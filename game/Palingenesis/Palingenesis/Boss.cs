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
        NagaBoss

    }
    class Boss : GameObject
    {
        private bossName type;
        private Point playerPosition;
        private List<Bullet> projectileList = new List<Bullet>();
        private Random rng = new Random();
        private Texture2D bulletTexture;
        private Song takeDamage;
        private Color color = Color.White;
        private List<Bullet> specialList = new List<Bullet>();
        List<Bullet> ringList = new List<Bullet>();
        public List<Bullet> ProjectileList
        {
            get { return projectileList; }
        }

        public Boss (int health, int moveSpeed, int attackSpeed, int Damage, Texture2D texture, Rectangle position, Song takeDamage, int windowHeight, int windowWidth, bossName type, Texture2D bulletTexture) : base(health, moveSpeed, attackSpeed, Damage, texture, position, windowHeight, windowWidth)
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
        public void CreateProjectiles(int amount, Direction direction, Rectangle position, int xSpacing, int ySpacing, Player target)
        {
            for(int i = 0; i < amount; i++)
            {
               
                projectileList.Add(new Bullet(bulletTexture, position, takeDamage, this.windowHeight, this.windowWidth, direction, target, this.damage));
                
                    position.X += xSpacing;
                    position.Y += ySpacing;
                
                

            }
        }

        //used to decide what attack the boss will use with a random
        public void AI(Random rng, Player target, double attackTimer)
        {
           
                int tmp = rng.Next(0, 4);
                if (tmp == 0)
                {
                    Line(target);
                }
                else if (tmp == 1)
                {
                    MegaShot(target);
                }
                else if(tmp == 2)
                {
                    Ring(target, attackTimer);
                }
                else
                {
                    Circle(target);
                }

            RemoveBullet(attackTimer);

            
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
                CreateProjectiles(rng.Next(5, 11), Direction.right, new Rectangle((this.Position.X + 100), 0, 25, 25), 0, 100, target);
            }

            else
            {
                CreateProjectiles(rng.Next(5, 11), Direction.left, new Rectangle((this.Position.X - 100), 0, 25, 25), 0, 100, target);
            }

          
        }

        //spawns a ring of shots around the boss to make the player retreat
        public void Ring(Player target, double timer)
        {
            //creates a ring of 9 bullets that don't move offset by 100 (in the y x or both directions) which will damage the player if they make contact
            Bullet topLeftCorner = new Bullet(bulletTexture, new Rectangle(this.Position.X - 100, this.position.Y - 100, 50, 50), this.takeDamage, windowHeight, windowWidth, Direction.none, target, this.damage);
            Bullet leftMiddle = new Bullet(bulletTexture, new Rectangle(this.Position.X - 100, this.position.Y, 50, 50), this.takeDamage, windowHeight, windowWidth, Direction.none, target, this.damage);
            Bullet bottomLeftCorner = new Bullet(bulletTexture, new Rectangle(this.Position.X - 100, this.position.Y + 100, 50, 50), this.takeDamage, windowHeight, windowWidth, Direction.none, target, this.damage);

            Bullet bottom = new Bullet(bulletTexture, new Rectangle(this.Position.X, this.position.Y + 100, 50, 50), this.takeDamage, windowHeight, windowWidth, Direction.none, target, this.damage);
            Bullet top = new Bullet(bulletTexture, new Rectangle(this.Position.X, this.position.Y + 100, 50, 50), this.takeDamage, windowHeight, windowWidth, Direction.none, target, this.damage);

            Bullet topRightCorner = new Bullet(bulletTexture, new Rectangle(this.Position.X + (position.Width + 100), this.position.Y - 100, 50, 50), this.takeDamage, windowHeight, windowWidth, Direction.none, target, this.damage);
            Bullet rightMiddle = new Bullet(bulletTexture, new Rectangle(this.Position.X + (position.Width + 100), this.position.Y, 50, 50), this.takeDamage, windowHeight, windowWidth, Direction.none, target, this.damage);
            Bullet bottomRightCorner = new Bullet(bulletTexture, new Rectangle(this.Position.X + (position.Width + 100), this.position.Y + 100, 50, 50), this.takeDamage, windowHeight, windowWidth, Direction.none, target, this.damage);

            //creates a shell list so that I can loop through all of the ring bullets
            

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
            timer = 0;
        }

        //fires out shots in a circle around the boss
        public void Circle(Player target)
        {
            //left
            CreateProjectiles(2, Direction.left, new Rectangle(position.X - 25, position.Y - 50, 25, 25), 0, 75, target);

            //bottom
            CreateProjectiles(2, Direction.down, new Rectangle(position.X + 50, position.Y + 25, 25, 25), 75, 0, target);

            //right 
            CreateProjectiles(2, Direction.right, new Rectangle(position.X + 25, position.Y + 50, 25, 25), 0, 75, target);

            //top
            CreateProjectiles(2, Direction.up, new Rectangle(position.X - 50, position.Y - 25, 25, 25), 75, 0, target);

        }

        //boss charges once at the player  and if it makes contact with the player deals damage
        public void Charge(Player target)
        {
            //sets color to red to telegraph the attack to the player 
           color = Color.Red;
            
            //player is too the right of the boss
                if(target.Position.X > position.X)
                {
                    //player is below the boss
                    if(target.Position.Y > position.Y)
                    {
                        position.X += 3;
                        position.Y += 3;
                    }
                    //player is above the boss
                    else
                    {
                        position.X += 3;
                        position.Y -= 3;
                    }
                }
                //player is to the left of the boss
                else
                {
                    //player is below the boss
                    if (target.Position.Y > position.Y)
                    {
                        position.X -= 3;
                        position.Y += 3;
                    }
                    //player is above the boss
                    else
                    {
                        position.X -= 3;
                        position.Y -= 3;
                    }
                }
            

            //after the boss has finished charging resets the color to white
            color = Color.White;
        }
        //fires a single large shot that does 2x damage at the player
        public void MegaShot(Player target)
        {
            if (target.Position.X > this.position.X)
            {
                projectileList.Add(new Bullet(bulletTexture, new Rectangle((this.Position.X + 100), target.Position.Y, 100, 100), this.takeDamage, windowHeight, windowWidth, Direction.right, target, this.damage * 2));
            }

            else
            {
                projectileList.Add(new Bullet(bulletTexture, new Rectangle((this.Position.X - 100), target.Position.Y, 100, 100), this.takeDamage, windowHeight, windowWidth, Direction.left, target, this.damage * 2));
            }
        }

            public void SpecialAttack(Player target)
            {
                if(type == bossName.RiceGoddess)
                {
                    //I want this to create between 10-15 of shots around the screen randomly that after a certain amount of time will start to damage the player
                    //shell list
                    
                    for(int i = 0; i < rng.Next(10, 16); i++)
                    {
                        specialList.Add(new Bullet(bulletTexture, new Rectangle(rng.Next(0, windowWidth), rng.Next(0, windowHeight), 30, 30), takeDamage, windowHeight, windowWidth, Direction.none, target, damage + 5));

                        //color is set to green so that they player knows that they won't be damage by the projectile yet 
                        specialList[i].Color = Color.Green;

                    }   

                    //gonna go to office hours on tuesday to try and figure out to get timer to work which will set it so the projectiles do damage
                }
            }

        public void Reset()
        {
            health = maxHealth;
            position.X = 960;
            position.Y = 540;
            projectileList.Clear();
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, Position, color);
        }

        public void RemoveBullet(double attackTimer)
        {
            double initialValue = attackTimer;
            //after 2 seconds
            if(attackTimer >= (initialValue + 2))
            {
                for (int i = 0; i < specialList.Count; i++)
                {
                    specialList[i].Color = Color.Red;
                    projectileList.Add(specialList[i]);
                }
                for (int i = 0; i < ringList.Count; i++)
                {
                    projectileList.Remove(ringList[i]);
                }

            }

            if(attackTimer >= initialValue + 4)
            {
                for (int i = 0; i < specialList.Count; i++)
                {
                    projectileList.Remove(specialList[i]);
                }
            }

        }
    }
}
