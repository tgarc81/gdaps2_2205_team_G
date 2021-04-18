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

        public void ResetHealth(int health)
        {
            actualHealth = visibleHealth = health;
        }

        public void Update(int damadge)
        {
           
            if (tempHealth != damadge)
            {
                int v = tempHealth - damadge;
                actualHealth = actualHealth - v;
            }

            if (actualHealth < visibleHealth)
            {
                visibleHealth -= 1;
            }
            else if (actualHealth > visibleHealth)
            {
                // Make sure we don't go over
                visibleHealth = actualHealth;
            }

            //int z = (actualHealth / totalHealth) * Fullbar.Width;

            tempHealth = damadge;

            greenHealthBar.Width = actualHealth;
            redHealthBar.Width = visibleHealth;
        }

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
