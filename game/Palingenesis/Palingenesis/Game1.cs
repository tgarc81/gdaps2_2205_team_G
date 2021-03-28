﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

//Name: G-Force
//Date: 3/16/21
//Professor Mesh
//Purpose: To intialize the base states of our game.

namespace Palingenesis
{
    //enum used in the FSM
    enum gameState
    {
        Menu,
        ScoreBoard,
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
        private double timer; // Represents the time elapsed in the game to be used for boss attacks
        private double time; // Represents total time elapsed in the game
        private Player player; // Represents the actual player
        private Texture2D playerAsset; // Represents player image
        private Boss boss1;
        private Boss riceGoddess; // Represents Rice Goddess boss
        private int numberOfDialougeFrames;
        // private List<string> dialougeList = new List<string>();
        private List<Dialogue> dialougeList;
        private String currentLine;
        private bool isRiceGoddessLoaded; // Bool that represents whether Rice Goddess boss has been loaded in this game
        private Texture2D attackTexture;
        private Texture2D bossTexture;
        int windowWidth;
        int windowHeight;
        private int dialogueNum=0;
        private Random rng = new Random();
        private List<string> names; // Represents all the names to go in leaderboard
        private List<double> times; // Represents all the times to go in leaderboard
        //game starts in the menu state
        private gameState currentState = gameState.Menu;
        private GameTime gametime = new GameTime();
        private Texture2D bossVN;
        private Texture2D playerVN;
        private Texture2D backgroundVN;
        private Texture2D textboxVN;
        private Texture2D textboxNameVN;
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
            names = new List<string>();
            times = new List<double>();
            dialougeList = new List<Dialogue>();
            

            graphics.PreferredBackBufferWidth = 1920;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 1080;   // set this value to the desired height of your window

            graphics.ApplyChanges();
            base.Initialize();

            
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            windowWidth = graphics.GraphicsDevice.Viewport.Width;
            windowHeight = graphics.GraphicsDevice.Viewport.Height;
            font = Content.Load<SpriteFont>("font");

            bossTexture = Content.Load<Texture2D>("bossPlaceHolder");
            playerAsset = Content.Load<Texture2D>("playerPlaceHolderTexture");
            attackTexture = Content.Load<Texture2D>("attackPlaceholder");

            textboxVN= Content.Load<Texture2D>("textbox");
            textboxNameVN = Content.Load<Texture2D>("rustyName");
            backgroundVN = Content.Load<Texture2D>("bayo2");
            playerVN = Content.Load<Texture2D>("theo");
            bossVN = Content.Load<Texture2D>("bingus");
            

            player = new Player(100, 10, 10, 20, playerAsset, new Rectangle(200, 200, 50, 50), windowHeight, windowWidth);
            //note: make a placeholder asset for the boss
            boss1= new Boss(1000, 0, 10, 10, bossTexture, new Rectangle(500, 500, 75, 75), windowWidth, windowHeight, bossName.RiceGoddess, attackTexture);

            DialogueListAdd();

            boss1.Center();
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
                        currentState = gameState.Dialouge;
                        //LoadBoss();
                        //player.Reset();
                    }
                    break;

                case gameState.Game:
                    player.Update();
                    player.Attack(boss1, prevKbState);

                    //AI method runs every 2 seconds
                    if (timer > 2)
                    {
                        boss1.AI(rng, player);
                        timer = 0;
                    }

                    //runs update on each bullet after the pattern is spawned by AI
                    for (int i = 0; i < boss1.ProjectileList.Count; i++)
                    {
                        boss1.ProjectileList[i].Update();
                    }

                    for (int i = 0; i < player.ShotList.Count; i++)
                    {
                        player.ShotList[i].Update();
                    }

                    timer += gameTime.ElapsedGameTime.TotalSeconds;
                    

                    Console.WriteLine(gameTime.ElapsedGameTime.TotalSeconds);
                    //pressing escape during the game pauses
                    if (SingleKeyPress(Keys.P, kbState))
                    {
                        currentState = gameState.Pause;
                    }

                    //if the players health reaches zero game over
                    if(player.Health <= 0)
                    {
                        currentState = gameState.GameOver;
                        LoadScoreboard();
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
                        currentState = gameState.ScoreBoard;
                        SaveScoreboard();
                    }

                    break;

                case gameState.Dialouge:

                    //each time the user presses enter the dialouge advances by one line
                   if(SingleKeyPress((Keys.Enter), kbState))
                   { 
                        dialogueNum++;
                   }

                   //when there is no remaining dialouge starts the next section of the game
                   if(dialougeList[dialogueNum]== null)
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
                    if(SingleKeyPress(Keys.M, kbState))
                    {
                        currentState = gameState.Menu;
                    }
                    break;

                case gameState.ScoreBoard:
                    if (SingleKeyPress(Keys.Enter, kbState))
                    {
                        currentState = gameState.Menu;
                    }
                    break;
            }


            prevKbState = kbState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CadetBlue);
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
                    _spriteBatch.DrawString(font, string.Format("Player Health: {0}", player.Health), new Vector2(0, 80), Color.White);
                    _spriteBatch.DrawString(font, string.Format("Boss Health: {0}", boss1.Health), new Vector2(0, 100), Color.White);

                    player.Draw(_spriteBatch);
                    
                    boss1.Draw(_spriteBatch);
                    for(int i =0; i < boss1.ProjectileList.Count; i++)
                    {
                        boss1.ProjectileList[i].Draw(_spriteBatch);
                    }

                    for (int i = 0; i < boss1.ProjectileList.Count; i++)
                    {
                        if(boss1.ProjectileList[i].HasHit == true)
                        {
                            boss1.ProjectileList.RemoveAt(i);
                        }
                    }

                    for (int i = 0; i < player.ShotList.Count; i++)
                    {
                        player.ShotList[i].Draw(_spriteBatch);

                    }

                    for (int i = 0; i < player.ShotList.Count; i++)
                    {
                        if(player.ShotList[i].HasHit == true)
                        {
                            player.ShotList.RemoveAt(i);
                        }
                    }

                    break;

                case gameState.GameOver:
                    _spriteBatch.DrawString(font, "Press enter to return to Main menu", new Vector2(0, 0), Color.White);
                    break;

                case gameState.Dialouge:

                    dialougeList[dialogueNum].Draw(_spriteBatch);
                    //figure out the actual position later

                   // _spriteBatch.DrawString(font, string.Format("{0}", currentLine), new Vector2(100, 500), Color.White);
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
            Boss boss = null;
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
                        boss = new Boss(health, moveSpeed, attackSpeed, damage, bossTexture, new Rectangle(500, 500, 75, 75), windowWidth, windowHeight, bossName.RiceGoddess, bossTexture); // Makes Rice Goddess using data gathered from the file
                    }
                }
                catch (Exception e)
                {
                    // A way to output to user
                }
            }
            boss.Center();
        }

        /// <summary>
        /// Saves the current scoreboard information to a file so that it may be saved and loaded in later on
        /// </summary>
        private void SaveScoreboard()
        {
            // Create the variable here since we need it after the try
            StreamWriter output = null;
            try
            {
                // When we open for writing, create the file if it doesn't exist yet
                output = new StreamWriter("../../../Scoreboard.txt");
                // We would get user input for name
                string name = "Default"; // We would need a way to get user input for name
                names.Add(name);
                times.Add(time);
                for (int i = 0; i < names.Count; i++) // For each element in both the names and times List
                {
                    output.WriteLine($"{names[i]},{times[i]}"); // Add the corresponding name with their time in the file
                }
                // Confirmation message
            }
            catch(Exception e)
            {
                //Output error message
            }
            // Ensure that we can close the file, as long as it was actually opened in the first place
            if (output != null)
            {
                output.Close();
            }
        }

        /// <summary>
        /// Loads the scoreboard data in from a file so it may be used later on to display
        /// </summary>
        private void LoadScoreboard()
        {
            // Create the variable here since we need it after the try
            StreamReader input = null;
            try
            {
                // Creating the streamreader opens the file
                input = new StreamReader("../../../Scoreboard.txt");
                string line = "DEFAULT";
                String[] data = line.Split(",");
                // Loops through the file one line at a time
                while ((line = input.ReadLine()) != null)
                {
                    names.Add(data[0]); // Adds the name in file to the list of names for scoreboard
                    times.Add(int.Parse(data[1])); // Adds the corresponding times to match the name to the list of times for scoreboard
                }
                // Confirmation message
            }
            catch(Exception e)
            {
                // Error message
            }
            // Ensure that we can close the file, as long as it was actually opened in the first place
            if (input != null)
            {
                input.Close();
            }
        }

        private void DialogueListAdd()
        {
            dialougeList.Add(new Dialogue(playerVN, bossVN, backgroundVN,textboxVN,textboxNameVN, font, "wus good", true));
            dialougeList.Add(new Dialogue(playerVN, bossVN, backgroundVN, textboxVN, textboxNameVN, font, "nm hbu", false));
        }
    }
}
