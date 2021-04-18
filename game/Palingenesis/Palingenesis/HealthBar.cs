using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Palingenesis
{
    class HealthBar
    {
        int actualHealth;
        int visibleHealth;
        string name;
        int totalHealth;
        Rectangle greenHealthBar;
        Rectangle redHealthBar;
        Rectangle Fullbar;
        Texture2D barCase;
        Texture2D bar;
        SpriteFont font;
        Vector2 location;
        Vector2 BossNameFormat;
        int tempHealth;

        //Constructor that takes all of the info for making the health bar
        public HealthBar(Texture2D barCase, Texture2D bar, Rectangle barSize, Vector2 location, SpriteFont font, string  name, int health)
        {
            this.barCase = barCase;
            this.bar = bar;
            this.location = location;
            this.font = font;
            this.name = name;
            actualHealth=health;
            visibleHealth= health;
            tempHealth = health;
            totalHealth = health;
            greenHealthBar = barSize;
            redHealthBar = barSize;
            Fullbar = barSize;
            BossNameFormat = location;
        }

        //Reset method for health bar
        public void ResetHealth(int health)
        {
            actualHealth = visibleHealth = health;
        }

        //Updates health bar 
        public void Update(int damadge)
        {
            //only updates health bar when damadge is taken  (damadge=current player HP)
            if (tempHealth != damadge)
            {
                //calculates damadge by getting the differnce player's current hp and previous HP(last frame)
                int v = tempHealth - damadge;
                actualHealth = actualHealth - v;
            }

            if (actualHealth < visibleHealth)
            {
                //adds a cool slow effect
                visibleHealth -= 1;
            }
            else if (actualHealth > visibleHealth)
            {
                // Make sure we don't go over
                visibleHealth = actualHealth;
            }

            //int z = (actualHealth / totalHealth) * Fullbar.Width;
            //updates previous health to current health
            tempHealth = damadge;

            //actually updates the health rectangles
            greenHealthBar.Width = actualHealth;
            redHealthBar.Width = visibleHealth;
        }

        //Draws the health bar rectangles
        public void Draw(SpriteBatch sb)
        {
           
            sb.DrawString(
                      font,
                      name,
                      BossNameFormat,
                      Color.White);

           // sb.Draw(barCase, Fullbar, Color.White);
            sb.Draw(bar, redHealthBar, Color.Red);
            sb.Draw(bar, greenHealthBar, Color.Green);
        }
           
      
    }
}
