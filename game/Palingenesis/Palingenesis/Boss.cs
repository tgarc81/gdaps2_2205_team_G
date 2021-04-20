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
        private bool isCharging = false;
        private Vector2 positionVector;
        private Vector2 ChargeUpdateVector;
        
        public List<Bullet> ProjectileList
        {
            get { return projectileList; }
        }
        public List<Bullet> SpecialList
        {
            get { return specialList; }
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
            if(isCharging == true)
            {

            }
        }

        //helper method fills bullet list
        public void CreateProjectiles(int amount, BulletType direction, Rectangle position, int xSpacing, int ySpacing, Player target)
        {
            for(int i = 0; i < amount; i++)
            {
               
                projectileList.Add(new Bullet(bulletTexture, position, takeDamage, this.windowHeight, this.windowWidth, direction, target, this.damage));
                
                    position.X += xSpacing;
                    position.Y += ySpacing;
                
                

            }
        }

        //used to decide what attack the boss will use with a random
        public void AI(int tmp, Player target, double attackTimer, GameTime gameTime)
        {
            
            double initialValue = 0;

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
                    Circle(target);
                    
                }
                else if(tmp == 3)
                {
               
                    SpecialAttack(target); 
                }
                else if(tmp == 4)
                {
                    Charge(target, gameTime);
                }
               
                else
                {
                    Ring(target, attackTimer);
                }

            RemoveBullet(attackTimer, initialValue);
            
            
        }

        //standard attacks

        public void Line(Player target) //currently busted 
        {

            //if the player is to the Right of the boss
            if(target.Position.X > this.position.X)
            {
                //creates between 1 and 5 projectiles that fly to the Right
                //each is set 50 pixels to the Right of the boss
                ///each bullet spawns 75 pixels apart on the y axis
                CreateProjectiles(rng.Next(5, 11), BulletType.Right, new Rectangle((this.Position.X + 100), 0, 25, 25), 0, 100, target);
            }

            else
            {
                CreateProjectiles(rng.Next(5, 11), BulletType.Left, new Rectangle((this.Position.X - 100), 0, 25, 25), 0, 100, target);
            }

          
        }

        //spawns a ring of shots around the boss to make the player retreat
        public void Ring(Player target, double timer)
        {
            //creates a ring of 9 bullets that don't move offset by 100 (in the y x or both directions) which will damage the player if they make contact
            Bullet topRightCorner = new Bullet(bulletTexture, new Rectangle(this.Position.X - 100, this.position.Y - 100, 50, 50), this.takeDamage, windowHeight, windowWidth, BulletType.ring, target, this.damage);
            Bullet RightMiddle = new Bullet(bulletTexture, new Rectangle(this.Position.X - 100, this.position.Y, 50, 50), this.takeDamage, windowHeight, windowWidth, BulletType.ring, target, this.damage);
            Bullet bottomRightCorner = new Bullet(bulletTexture, new Rectangle(this.Position.X - 100, this.position.Y + 100, 50, 50), this.takeDamage, windowHeight, windowWidth, BulletType.ring, target, this.damage);

            Bullet bottom = new Bullet(bulletTexture, new Rectangle(this.Position.X, this.position.Y + 100, 50, 50), this.takeDamage, windowHeight, windowWidth, BulletType.ring, target, this.damage);
            Bullet top = new Bullet(bulletTexture, new Rectangle(this.Position.X, this.position.Y + 100, 50, 50), this.takeDamage, windowHeight, windowWidth, BulletType.ring, target, this.damage);

            Bullet toprightCorner = new Bullet(bulletTexture, new Rectangle(this.Position.X + (position.Width + 100), this.position.Y - 100, 50, 50), this.takeDamage, windowHeight, windowWidth, BulletType.ring, target, this.damage);
            Bullet rightMiddle = new Bullet(bulletTexture, new Rectangle(this.Position.X + (position.Width + 100), this.position.Y, 50, 50), this.takeDamage, windowHeight, windowWidth, BulletType.ring, target, this.damage);
            Bullet bottomrightCorner = new Bullet(bulletTexture, new Rectangle(this.Position.X + (position.Width + 100), this.position.Y + 100, 50, 50), this.takeDamage, windowHeight, windowWidth, BulletType.ring, target, this.damage);

            //creates a shell list so that I can loop through all of the ring bullets
            

            ringList.Add(topRightCorner);
            ringList.Add(RightMiddle);
            ringList.Add(bottomRightCorner);
            ringList.Add(bottom);
            ringList.Add(top);
            ringList.Add(topRightCorner);
            ringList.Add(RightMiddle);
            ringList.Add(bottomRightCorner);

            for(int i = 0; i < ringList.Count; i++)
            {
                //adds each of the ring bullets to the main list
                projectileList.Add(ringList[i]);
            }
            if (timer <= 0)
            {
                projectileList.Clear();
            }
               
            
            
        }

        //fires out shots in a circle around the boss
        public void Circle(Player target)
        {
            //Left
            CreateProjectiles(2, BulletType.Left, new Rectangle(position.X - 25, position.Y - 50, 25, 25), 0, 75, target);

            //bottom
            CreateProjectiles(2, BulletType.Down, new Rectangle(position.X + 50, position.Y + 25, 25, 25), 75, 0, target);

            //Right 
            CreateProjectiles(2, BulletType.Right, new Rectangle(position.X + 25, position.Y + 50, 25, 25), 0, 75, target);

            //top
            CreateProjectiles(2, BulletType.Up, new Rectangle(position.X - 50, position.Y - 25, 25, 25), 75, 0, target);

        }

        //boss charges once at the player  and if it makes contact with the player deals damage
        public void Charge(Player target, GameTime gameTime)
        {
            positionVector = new Vector2(position.X, position.Y);
            //sets color to red to telegraph the attack to the player 
            color = Color.Red;
            isCharging = true;

            
            //finds the vector of difference between the boss and the player's positions
            Vector2 direction = new Vector2(target.Position.X - position.X, target.Position.Y - position.Y);
            direction.Normalize();

            Point tmp = new Point(target.Position.X, target.Position.Y);

            
            positionVector.X += (float)(direction.X * 50 * gameTime.ElapsedGameTime.TotalSeconds);
            positionVector.Y += (float)(direction.Y * 50 * gameTime.ElapsedGameTime.TotalSeconds);
            


            position.X = (int)positionVector.X;
            position.Y = (int)positionVector.Y;


            //after the boss has finished charging resets the color to white
            color = Color.White;
        }
        //fires a single large shot that does 2x damage at the player
        public void MegaShot(Player target)
        {
            if (target.Position.X > this.position.X)
            {
                projectileList.Add(new Bullet(bulletTexture, new Rectangle((this.Position.X + 100), target.Position.Y, 100, 100), this.takeDamage, windowHeight, windowWidth, BulletType.Right, target, this.damage * 2));
            }

            else
            {
                projectileList.Add(new Bullet(bulletTexture, new Rectangle((this.Position.X - 100), target.Position.Y, 100, 100), this.takeDamage, windowHeight, windowWidth, BulletType.Left, target, this.damage * 2));
            }
        }

            public void SpecialAttack(Player target)
            {
                
                    //I want this to create between 10-15 of shots around the screen randomly that after a certain amount of time will start to damage the player
                    //shell list
                    if(type == bossName.RiceGoddess)
                    {
                        for (int i = 0; i < rng.Next(10, 16); i++)
                        {
                            specialList.Add(new Bullet(bulletTexture, new Rectangle(rng.Next(0, windowWidth), rng.Next(0, windowHeight), 30, 30), takeDamage, windowHeight, windowWidth, BulletType.RiceGoddessSpecial, target, damage + 5));

                            //color is set to green so that they player knows that they won't be damage by the projectile yet 
                            specialList[i].Color = Color.Green;

                        }
            }
                    else if(type == bossName.NagaBoss)
                    {
                        if (target.Position.X > this.position.X)
                        {
                            projectileList.Add(new Bullet(bulletTexture, new Rectangle((this.Position.X + 100), target.Position.Y, 100, 100), this.takeDamage, windowHeight, windowWidth, BulletType.SinRight, target, this.damage * 2));
                        }

                        else
                        {
                            projectileList.Add(new Bullet(bulletTexture, new Rectangle((this.Position.X - 100), target.Position.Y, 100, 100), this.takeDamage, windowHeight, windowWidth, BulletType.SinRight, target, this.damage * 2));
                        }
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
            if (!isCharging)
            {
                sb.Draw(texture, Position, color);
            }
            else
            {
                sb.Draw(texture, positionVector, color);
            }
            
        }
            //might be unnecessary
        public void RemoveBullet(double attackTimer, double initialValue)
        {
            
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
