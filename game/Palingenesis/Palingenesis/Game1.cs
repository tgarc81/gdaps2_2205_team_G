﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Palingenesis
{
    //enum used in the FSM
    enum gameState
    {
        Menu,
        Game,
        GameOver,
        Dialouge,
        Pause
    }
    

    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardState kbState;
        private KeyboardState prevKbState;
        private SpriteFont font;
        private double timer;
        private Player player;
        private Texture2D playerAsset;
        private Boss boss1;
        private int numberOfDialougeFrames;

        int windowWidth;
        int windowHieght;

        //game starts in the menu state
        private gameState currentState = gameState.Menu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            windowWidth = graphics.GraphicsDevice.Viewport.Width;
            windowHieght = graphics.GraphicsDevice.Viewport.Height;
            font = Content.Load<SpriteFont>("font");

            playerAsset = Content.Load<Texture2D>("playerPlaceHolderTexture");

            player = new Player(100, 10, 10, 20, playerAsset, new Rectangle(200, 200, 50, 50), windowHieght, windowWidth);
            //note: make a placeholder asset for the boss
            boss1= new Boss(1000, 0, 10, 10, playerAsset, new Rectangle(500, 500, 10, 10), windowWidth, windowHieght);
                
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            kbState = Keyboard.GetState();

            switch (currentState)
            {
                case gameState.Menu:
                    //pressing enter on the main menu starts the game
                    if(SingleKeyPress(Keys.Enter, kbState))
                    {
                        currentState = gameState.Game;
                    }
                    break;

                case gameState.Game:
                    player.Update();

                    //pressing escape during the game pauses
                    if(SingleKeyPress(Keys.Escape, kbState))
                    {
                        currentState = gameState.Pause;
                    }

                    //if the players health reaches zero game over
                    if(player.Health <= 0)
                    {
                        currentState = gameState.GameOver;
                    }

                    //when the bosses health hit's zero, starts the next dialouge section
                    if (boss1.Health <= 0)
                    {
                        currentState = gameState.Dialouge;
                    }

                    
                    break;

                case gameState.GameOver:
                    //pressing neter on the game over screen sends you to the menu
                    if(SingleKeyPress(Keys.Enter, kbState))
                    {
                        currentState = gameState.Menu;
                    }

                    break;

                case gameState.Dialouge:

                    //each time the user presses enter the dialouge advances by one line
                   if(SingleKeyPress((Keys.Enter), kbState))
                   {
                        numberOfDialougeFrames--;
                   }

                   //when there is no remaining dialouge starts the next section of the game
                   if(numberOfDialougeFrames == 0)
                   {
                        currentState = gameState.Game;
                   }

                    break;

                case gameState.Pause:

                    //pressing escape on pause restarts the game
                    if(SingleKeyPress(Keys.Escape, kbState))
                    {
                        currentState = gameState.Game;
                    }

                    break;
            }


            prevKbState = kbState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            // TODO: Add your drawing code here
            switch (currentState)
            {
                case gameState.Menu:
                    _spriteBatch.DrawString(font, "PlaceHolder for menu", new Vector2(0, 0), Color.White);
                    break;

                case gameState.Game:
                    _spriteBatch.DrawString(font, "PlaceHolder for game", new Vector2(0, 0), Color.White);

                    player.Draw(_spriteBatch);
                    break;

                case gameState.GameOver:
                    _spriteBatch.DrawString(font, "PlaceHolder for gameover", new Vector2(0, 0), Color.White);
                    break;

                case gameState.Dialouge:
                    _spriteBatch.DrawString(font, "PlaceHolder for visual novel sections", new Vector2(0, 0), Color.White);
                    break;

                case gameState.Pause:
                    _spriteBatch.DrawString(font, "PlaceHolder for pause", new Vector2(0, 0), Color.White);
                    break;
            }


            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private bool SingleKeyPress(Keys key, KeyboardState kbState)
        {
            //if the specified key is currently down and was previously up, returns as true
            if (kbState.IsKeyDown(key) == true && prevKbState.IsKeyUp(key) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
