﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Palingenesis
{
    class HealthBar
    {
        string name;

        SpriteFont font;

        Texture2D barCase;
        Texture2D bar;

        Vector2 location;
        Vector2 BossNameFormat;

        Rectangle greenHealthBar;
        Rectangle redHealthBar;
        Rectangle Fullbar;

        int actualHealth;
        int visibleHealth;
        int totalHealth;
        int tempHealth;

        /// <summary>
        /// Constructor that takes all of the info for making the health bar
        /// </summary>
        /// <param name="barCase"></param>
        /// <param name="bar"></param>
        /// <param name="barSize"></param>
        /// <param name="location"></param>
        /// <param name="font"></param>
        /// <param name="name"></param>
        /// <param name="health"></param>
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

        /// <summary>
        /// Reset method for health bar
        /// </summary>
        /// <param name="health"></param>
        public void ResetHealth(int health)
        {
            actualHealth = visibleHealth = health;
        }

        /// <summary>
        /// Updates health bar 
        /// </summary>
        /// <param name="damadge"></param>
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

        /// <summary>
        /// Draws the health bar rectangles
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
           
            sb.DrawString(
                      font,
                      name,
                      BossNameFormat,
                      Color.White);

            sb.Draw(barCase, Fullbar, Color.White);
            sb.Draw(bar, redHealthBar, Color.Red);
            sb.Draw(bar, greenHealthBar, Color.Green);
        }
           
      
    }
}
