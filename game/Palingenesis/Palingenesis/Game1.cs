using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Palingenesis
{
    //enum used in the FSM
    enum gameState
    {
        Menu,
        Instructions,
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
        private double timer; // Represents the time elapsed in the game
        private Player player; // Represents the actual player
        private Texture2D playerAsset; // Represents player image
        private Boss boss1;
        private Boss riceGoddess; // Represents Rice Goddess boss
        private int numberOfDialougeFrames;
        private List<string> dialougeList = new List<string>();
        private String currentLine;
        private bool isRiceGoddessLoaded; // Bool that represents whether Rice Goddess boss has been loaded in this game
        private Texture2D attackTexture;

        int windowWidth;
        int windowHeight;

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
            isRiceGoddessLoaded = false; // Sets it so Rice Goddess is not loaded yet
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            windowWidth = graphics.GraphicsDevice.Viewport.Width;
            windowHeight = graphics.GraphicsDevice.Viewport.Height;
            font = Content.Load<SpriteFont>("font");

            playerAsset = Content.Load<Texture2D>("playerPlaceHolderTexture");
            attackTexture = Content.Load<Texture2D>("attackPlaceholder");
            player = new Player(100, 10, 10, 20, playerAsset, new Rectangle(200, 200, 50, 50), windowHeight, windowWidth);
            //note: make a placeholder asset for the boss
            boss1= new Boss(1000, 0, 10, 10, playerAsset, new Rectangle(500, 500, 10, 10), windowWidth, windowHeight, bossName.RiceGoddess);
                
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
                        LoadBoss();
                    }
                    break;

                case gameState.Game:
                    player.Update();
                    player.Attack(boss1, prevKbState);

                    //pressing escape during the game pauses
                    if(SingleKeyPress(Keys.P, kbState))
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
                        LoadBoss();
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
                    if(SingleKeyPress(Keys.P, kbState))
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
                    _spriteBatch.DrawString(font, "Press enter to start game", new Vector2(0, 20), Color.White);
                    break;

                case gameState.Game:
                    _spriteBatch.DrawString(font, "PlaceHolder for game", new Vector2(0, 0), Color.White);
                    _spriteBatch.DrawString(font, "Use WASD to move player", new Vector2(0, 20), Color.White);
                    _spriteBatch.DrawString(font, "Use arrow keys to attack", new Vector2(0, 40), Color.White);
                    _spriteBatch.DrawString(font, "Press P to pause", new Vector2(0, 60), Color.White);
                    player.Draw(_spriteBatch);
                    player.attackDraw(_spriteBatch, attackTexture);
                    break;

                case gameState.GameOver:
                    _spriteBatch.DrawString(font, "Press enter to return to Main menu", new Vector2(0, 0), Color.White);
                    break;

                case gameState.Dialouge:
                    //figure out the actual position later
                    _spriteBatch.DrawString(font, string.Format("{0}", currentLine), new Vector2(100, 500), Color.White);
                    break;

                case gameState.Pause:
                    _spriteBatch.DrawString(font, "Pause", new Vector2(0, 0), Color.White);
                    _spriteBatch.DrawString(font, "Press P to return to game", new Vector2(0, 20), Color.White);
                    _spriteBatch.DrawString(font, "Press M to return to main menu", new Vector2(0, 40), Color.White);

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

        private void LoadBoss()
        {
            if(!isRiceGoddessLoaded) // If the Rice Goddess hasn't been loaded in this game yet
            {
                isRiceGoddessLoaded = true; // Changes bool so that Rice Goddess has been loaded in this game
                StreamReader input = null;
                try
                {
                    input = new StreamReader("../../../RiceGoddess.txt"); // Opens a StreamReader specifically to the RiceGoddess file
                    string line = null;
                    string[] data;
                    while((line = input.ReadLine()) != null) // Reads line in Rice Goddess document
                    {
                        data = line.Split(',');
                        int health = int.Parse(data[0]); // Makes health based on first element of data
                        int moveSpeed = int.Parse(data[1]); // Makes moveSpeed based on second element of data
                        int attackSpeed = int.Parse(data[2]); // Makes attackSpeed based on third element of data
                        int damage = int.Parse(data[3]); // Makes damage based on fourth element of data
                        riceGoddess = new Boss(health, moveSpeed, attackSpeed, damage, playerAsset, new Rectangle(500, 500, 10, 10), windowWidth, windowHeight); // Makes Rice Goddess using data gathered from the file
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error loading file: {e.Message}"); // If something goes wrong in loading the board's info from a file, state as such to the user
                }
            }
        }
    }
}
