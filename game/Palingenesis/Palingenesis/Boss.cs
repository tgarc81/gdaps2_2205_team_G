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

        public Boss (int health, int moveSpeed, int attackSpeed, int Damage, Texture2D texture, Rectangle position, int windowWidth, int windowHeight, bossName type) : base(health, moveSpeed, attackSpeed, Damage, texture, position, windowWidth, windowHeight)
        {
            this.type = type;
        }

        public override void Update()
        {
            //does nothing for now
        }

        //standard attacks
        public void Line()
        {

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
