using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//Name: G-Force
//Date: 3/16/21
//Professor Mesh
//Purpose: To provide an amazing visual novel experience.

namespace Palingenesis
{
    class Dialogue
    {
        private Texture2D player;
        private Texture2D boss;
        private Texture2D background;
        private Texture2D textBoxColor;
        private Texture2D textBoxName;
        private SpriteFont font;
        private string convo;
        private bool isPlayer;
        private string playername = "Theophania";
        private string bossname = "Rice Godess";

        private Rectangle backgroundBox = new Rectangle(0, 0, 1920, 1080);
        private Rectangle textBox = new Rectangle(0, 800, 1920, 280);
        private Rectangle playerBox = new Rectangle(0, 100, 600, 1080);
        private Rectangle bossBox = new Rectangle(1400, 100, 600, 1080);
        private Rectangle playerName = new Rectangle(50, 700, 300, 150);
        private Rectangle bossName = new Rectangle(1500, 700, 300, 150);

       

        public Dialogue(Texture2D player, Texture2D boss, Texture2D background, Texture2D textBoxColor, Texture2D textBoxName, SpriteFont font, string convo, bool isPlayer)
        { 
            this.player = player;
            this.boss = boss;
            this.background = background;
            this.font = font;
            this.convo = convo;
            this.isPlayer = isPlayer;
            this.textBoxColor = textBoxColor;
            this.textBoxName = textBoxName;
        }

        public void Draw(SpriteBatch sb)
        {
            if (isPlayer == true)
            {
                sb.Draw(background, backgroundBox, Color.White);
                sb.Draw(boss, bossBox, Color.Black);
                sb.Draw(player, playerBox, Color.White);
                sb.Draw(textBoxColor, textBox, Color.White);
                sb.Draw(textBoxName, playerName, Color.Red);
                sb.DrawString(font, playername, new Vector2(65, 750), Color.White);
                sb.DrawString(font, convo, new Vector2(60, 900), Color.White);
            }
            else
            {
                sb.Draw(background, backgroundBox, Color.White);
                sb.Draw(player, playerBox, Color.Black);
                sb.Draw(boss, bossBox, Color.White);
                sb.Draw(textBoxColor, textBox, Color.White);
                sb.Draw(textBoxName, bossName, Color.Green);
                sb.DrawString(font, bossname, new Vector2(1515, 750), Color.White);
                sb.DrawString(font, convo, new Vector2(60, 900), Color.White);
            }
        }
    }
}
