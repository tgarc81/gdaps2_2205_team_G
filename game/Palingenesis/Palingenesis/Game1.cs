using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Media;

//Name: G-Force
//Date: 3/16/21
//Updated: 4/17/21
//Professor Mesh
//Purpose: To intialize the base states of our game.
//check
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
        private SpriteFont fontVN;
        private double timer; // Represents the time elapsed in the game to be used for boss attacks
        private double time; // Represents total time elapsed in the game
        private Player player; // Represents the actual player
        private Texture2D playerAsset; // Represents player image
        private Boss boss;
        private int numberOfDialougeFrames;
        // private List<string> dialougeList = new List<string>();
        private List<Dialogue> dialougeList;
        private String currentLine;
        private bool isRiceGoddessLoaded; // Bool that represents whether Rice Goddess boss has been loaded in this game
        private bool isNagaBossLoaded; // Bool that represents whether Naga boss has been loaded in this game
        private int bossesDefeated;
        private const int numberOfBosses = 2;
        private Texture2D attackTextureRG;
        private Texture2D attackTextureNA;
        private Texture2D attackTexturePlayer;
        private Texture2D riceGoddessTexture;
        private Texture2D RiceGoddessBackground;
        private Texture2D NagaBackground;
        private Texture2D titleScreen;
        private Texture2D scoreBoard;
        private Texture2D gameOver;
        private Texture2D bar;
        private bool BossBeaten;
        Rectangle fullScreen = new Rectangle(0, 0, 1920, 1080);
        private int dialogueNum=0;
        private Random rng = new Random();
        private List<string> names; // Represents all the names to go in leaderboard
        private List<double> times; // Represents all the times to go in leaderboard
        //game starts in the menu state
        private gameState currentState = gameState.Menu;
        private GameTime gametime = new GameTime();
        //VN variables are for visual novel.
        private Texture2D RGVNDefault;
        private Texture2D RGVNAlternate;
        private Texture2D RGVNWise;
        private Texture2D playerVNDefault;
        private Texture2D playerVNAlternate;
        private Texture2D RGbackgroundVN;
        private Texture2D textboxVN;
        private Texture2D textboxNameVN;
        private Texture2D NAVNDefault;
        private Texture2D NAbackgroundVN;
        private Texture2D pauseScreen;
        double scoreTimer = 0;
        //Load sounds
        public Song takeDamadge;
        private Song deathSound;
        private Song forwardVN;
        private Song hit;
        private Song error;
        private Song RiceGoddessOST;
        private const int WindowWidth = 1920;
        private const int WindowHeight = 1080;
        private double attackTimer;
        int randomChoice;
        private double elapsed = 2;
        private const double Elapsed = 2;


        HealthBar BossHealth;
        HealthBar PlayerHealth;

        private Texture2D nagaBossTexture;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        //the
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            isRiceGoddessLoaded = false; // Sets it so Rice Goddess is not loaded yet
            isNagaBossLoaded = false; // Sets it so Naga boss is not loaded yet
            bossesDefeated = 0;
            names = new List<string>();
            times = new List<double>();
            dialougeList = new List<Dialogue>();
            

            graphics.PreferredBackBufferWidth = WindowWidth;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = WindowHeight;   // set this value to the desired height of your window

            graphics.ApplyChanges();
            base.Initialize();

            
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            
            font = Content.Load<SpriteFont>("font");
            fontVN = Content.Load<SpriteFont>("bigfont");
            

            //Load title screen textures
            titleScreen = Content.Load<Texture2D>("titlescreen");
            pauseScreen = Content.Load<Texture2D>("PauseScreen");
            scoreBoard= Content.Load<Texture2D>("ScoreBoard");

            //Load gameplay textures
            riceGoddessTexture = Content.Load<Texture2D>("RiceGoddessSprite");
            nagaBossTexture = Content.Load<Texture2D>("naga fire worm");
            playerAsset = Content.Load<Texture2D>("theophaniaInGame");
            attackTextureRG = Content.Load<Texture2D>("rice");
            attackTextureNA = Content.Load<Texture2D>("Fireball");
            attackTexturePlayer = Content.Load<Texture2D>("playerProjectileCorrected");
            RiceGoddessBackground = Content.Load<Texture2D>("RiceGodessBackground");
            NagaBackground = Content.Load<Texture2D>("NagaBackgroundFight");
            bar = Content.Load<Texture2D>("bar");
            takeDamadge = Content.Load<Song>("takeDamadge");
            deathSound= Content.Load<Song>("deathSound");
            hit = Content.Load<Song>("Punch_Hit_Sound_Effect");
            RiceGoddessOST = Content.Load<Song>("Sad but True - The HU");
            //Load GameOver Textures
            gameOver= Content.Load<Texture2D>("GameOver");

            //Load VN textures
            textboxVN = Content.Load<Texture2D>("textbox");
            textboxNameVN = Content.Load<Texture2D>("rustyName");
            RGbackgroundVN = Content.Load<Texture2D>("RGbackgroundVN");
            playerVNDefault = Content.Load<Texture2D>("theophaniaExpression1");
            playerVNAlternate = Content.Load<Texture2D>("TheoVisualNovelExpression2");
            RGVNDefault = Content.Load<Texture2D>("riceGoddessExpression1");
            RGVNAlternate = Content.Load<Texture2D>("RiceGoddessVisualNovelExpression2");
            RGVNWise = Content.Load<Texture2D>("ricegoddessWise");
            NAbackgroundVN = Content.Load<Texture2D>("NagaBackground");
            NAVNDefault= Content.Load<Texture2D>("bingus");
            forwardVN = Content.Load<Song>("forward_sound");

            
            player = new Player(100, 10, 10, 20, playerAsset, new Rectangle(200, 200, 50, 50),hit, WindowHeight, WindowWidth, attackTexturePlayer);
            //note: make a placeholder asset for the boss
            boss = null;
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
                    if (SingleKeyPress(Keys.Enter, kbState))
                    {
                        BossBeaten = false;
                        LoadBoss();
                        player.Reset();
                        boss.Reset();
                        scoreTimer = 0;
                        DialogueListAdd();
                        currentState = gameState.Dialouge;
                        MediaPlayer.Play(forwardVN);
                        
                        //Makes the rectangles for player and boss HP
                        Rectangle BossHPBar = new Rectangle(WindowWidth / 2 - WindowWidth / 4, WindowHeight / 10, boss.Health, 50);
                        Rectangle PlayerHPBar = new Rectangle(WindowWidth / 10 , WindowHeight - WindowHeight/20, player.Health, 50);

                        //Loads Health bar for Theophania
                        PlayerHealth = new HealthBar(textboxVN, bar, PlayerHPBar, new Vector2((WindowWidth / 10 - PlayerHPBar.Width / 4), WindowHeight - WindowHeight / 10), fontVN, "Theophania", player.Health);
                        switch (randomChoice)
                        {
                            case 1:
                                //Loads bigger rice goddess health bar
                                BossHealth = new HealthBar(textboxVN, bar, BossHPBar, new Vector2(WindowWidth / 2 - BossHPBar.Width / 4, (WindowHeight / 10) - WindowHeight / 20), fontVN, "Rice Goddess", boss.Health);
                                break;

                            case 2:
                                //Loads smaller Naga health bar
                                BossHealth = new HealthBar(textboxVN, bar, BossHPBar, new Vector2(WindowWidth / 2 - BossHPBar.Width / 4, (WindowHeight / 10) - WindowHeight / 20), fontVN, "Naga", boss.Health);
                                break;
                        }

                    }
                    break;
                case gameState.Game:
                    //updates player and bosses
                    player.Update();
                    player.Attack(boss, prevKbState);
                    BossHealth.Update(boss.Health);
                    PlayerHealth.Update(player.Health);

                    //this is the timer component needed for the special attack method
                    float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    elapsed -= time;

                    //logic for drawing each order based on elapsed seconds
                    if (elapsed < 0)
                    {
                        //I moved the random variable generation oustide the AI method to save on memory
                        int tmp = rng.Next(0, 5);
                       
                        boss.AI(4, player, elapsed, gameTime);

                        //updates timer
                        elapsed = Elapsed;
                    }

                    //AI method runs every 2 seconds
                    /*if (timer > 2)e
                    */
                    //runs update on each bullet after the pattern is spawned by AI
                    for (int i = 0; i < boss.ProjectileList.Count; i++)
                    {
                        boss.ProjectileList[i].Update();
                    }

                    for (int i = 0; i < player.ShotList.Count; i++)
                    {
                        player.ShotList[i].Update();
                    }

                    timer += gameTime.ElapsedGameTime.TotalSeconds;
                    scoreTimer += gameTime.ElapsedGameTime.TotalSeconds;
                    attackTimer += gameTime.TotalGameTime.TotalSeconds;
                    //Console.WriteLine(gameTime.ElapsedGameTime.TotalSeconds);
                    //pressing escape during the game pauses
                    if (SingleKeyPress(Keys.P, kbState))
                    {
                        currentState = gameState.Pause;
                    }

                    //if the players health reaches zero game over
                    if(player.Health <= 0)
                    {
                        MediaPlayer.Play(deathSound);
                        currentState = gameState.GameOver;
                    }

                    //when the bosses health hit's zero, starts the next dialouge section
                    if (boss.Health <= 0)
                    {
                        BossBeaten = true;
                        currentState = gameState.Dialouge;
                        LoadBoss();
                    }

                    
                    break;

                case gameState.GameOver:
                    //pressing enter on the game over screen sends you to the menu
                    if(SingleKeyPress(Keys.Enter, kbState))
                    {
                        currentState = gameState.Menu;
                    }

                    break;

                case gameState.Dialouge:

                    //each time the user presses enter the dialouge advances by one line
                   if(SingleKeyPress((Keys.Enter), kbState))
                   { 
                        dialogueNum++;
                        MediaPlayer.Play(forwardVN);
                   }

                   //checks for dialogue after game
                    if ((dialougeList[dialogueNum] == null) && (BossBeaten == true))
                    {
                        currentState = gameState.ScoreBoard;
                        dialogueNum = 0;
                        LoadScoreboard();
                    }

                    //when there is no remaining dialouge starts the next section of the game
                    if (dialougeList[dialogueNum]== null)
                    {
                        currentState = gameState.Game;
                        dialogueNum++;
                    }
                   

                    break;

                case gameState.Pause:

                    //pressing escape on pause restarts the game
                    if(SingleKeyPress(Keys.R, kbState))
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
                        SaveScoreboard();
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
                    _spriteBatch.Draw(titleScreen, fullScreen, Color.White);
                    _spriteBatch.DrawString(font, "PlaceHolder for menu", new Vector2(0, 0), Color.White);
                    _spriteBatch.DrawString(font, "Press enter to start game", new Vector2(0, 20), Color.White);
                    break;

                case gameState.Game:
                    //draws backgrounds for bosses
                    switch (randomChoice)
                    {
                        case 1:
                        _spriteBatch.Draw(RiceGoddessBackground, fullScreen, Color.White);
                            break;

                        case 2:
                            _spriteBatch.Draw(NagaBackground, fullScreen, Color.White);
                            break;

                    }

                    _spriteBatch.DrawString(font, "PlaceHolder for game", new Vector2(0, 0), Color.White);
                    _spriteBatch.DrawString(font, "Use WASD to move player", new Vector2(0, 20), Color.White);
                    _spriteBatch.DrawString(font, "Use arrow keys to attack", new Vector2(0, 40), Color.White);
                     
                    _spriteBatch.DrawString(font, "Press P to pause", new Vector2(0, 60), Color.White);
                    _spriteBatch.DrawString(font, string.Format("Player Health: {0}", player.Health), new Vector2(0, 80), Color.White);
                    _spriteBatch.DrawString(font, string.Format("Boss Health: {0}", boss.Health), new Vector2(0, 100), Color.White);

                   

                    player.Draw(_spriteBatch);
                    BossHealth.Draw(_spriteBatch);
                    PlayerHealth.Draw(_spriteBatch);

                    boss.Draw(_spriteBatch);
                    for(int i =0; i < boss.ProjectileList.Count; i++)
                    {
                        boss.ProjectileList[i].Draw(_spriteBatch);
                        
                    }
                    for(int i = 0; i < boss.SpecialList.Count; i++)
                    {
                        boss.SpecialList[i].Draw(_spriteBatch);
                    }

                    for (int i = 0; i < boss.ProjectileList.Count; i++)
                    {
                        if(boss.ProjectileList[i].HasHit == true)
                        {
                            boss.ProjectileList.RemoveAt(i);
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
                    _spriteBatch.Draw(gameOver, fullScreen, Color.White);
                    _spriteBatch.DrawString(font, "Press enter to return to Main menu", new Vector2(0, 0), Color.White);
                    
                    break;

                case gameState.Dialouge:
                    //draws object from dialgoe list using that object's dialogue method.
                    dialougeList[dialogueNum].Draw(_spriteBatch); // possible bug
                    //figure out the actual position later

                   // _spriteBatch.DrawString(font, string.Format("{0}", currentLine), new Vector2(100, 500), Color.White);
                    break;

                case gameState.Pause:
                    _spriteBatch.Draw(pauseScreen, fullScreen, Color.White);
                    
                    /*
                    _spriteBatch.DrawString(font, "Pause", new Vector2(0, 0), Color.White);
                    _spriteBatch.DrawString(font, "Press P to return to game", new Vector2(0, 20), Color.White);
                    _spriteBatch.DrawString(font, "Press M to return to main menu", new Vector2(0, 40), Color.White);
                    */

                    break;

                case gameState.ScoreBoard:
                    _spriteBatch.Draw(scoreBoard, fullScreen, Color.White);
                    _spriteBatch.DrawString(fontVN, String.Format("final time: {0:F} seconds", scoreTimer), new Vector2(1150, 200), Color.White);
                    dialogueNum = 0;
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
            Random rng = new Random();
            randomChoice = 1; //temporarily set so only the rice goddess will appear
            if (randomChoice == 1) // If it randomly chooses to load the Rice Goddess
            {
                if (!isRiceGoddessLoaded) // If the Rice Goddess hasn't been loaded in this game yet
                {
                    isRiceGoddessLoaded = true; // Changes bool so that Rice Goddess has been loaded in this game
                    StreamReader input = null;
                    try
                    {
                        input = new StreamReader("../../../RiceGoddess.txt"); // Opens a StreamReader specifically to the RiceGoddess file
                        string line = null;
                        string[] data;
                        while ((line = input.ReadLine()) != null) // Reads line in Rice Goddess document
                        {
                            data = line.Split(',');
                            int health = int.Parse(data[0]); // Makes health based on first element of data
                            int moveSpeed = int.Parse(data[1]); // Makes moveSpeed based on second element of data
                            int attackSpeed = int.Parse(data[2]); // Makes attackSpeed based on third element of data
                            int damage = int.Parse(data[3]); // Makes damage based on fourth element of data
                            boss = new Boss(health, moveSpeed, attackSpeed, damage, riceGoddessTexture, new Rectangle(500, 500, 75, 75), takeDamadge, WindowHeight, WindowWidth, bossName.RiceGoddess, attackTextureRG); // Makes Rice Goddess using data gathered from the file
                        }
                    }
                    catch (Exception e)
                    {
                        // A way to output to user
                    }
                }
                //I commented this out because it would always end up loading the opposite boss dialogue
                /*
                while(randomChoice == 1)
                {
                    randomChoice = rng.Next(1, 3);
                }
                */
            }
            else if (randomChoice == 2) // If it randomly chooses to load the Naga boss
            {
                if (!isNagaBossLoaded) // If the Naga boss hasn't been loaded in this game yet
                {
                    isNagaBossLoaded = true; // Changes bool so that Naga boss has been loaded in this game
                    StreamReader input = null;
                    try
                    {
                        input = new StreamReader("../../../NagaBoss.txt"); // Opens a StreamReader specifically to the Naga boss file
                        string line = null;
                        string[] data;
                        while ((line = input.ReadLine()) != null) // Reads line in Naga boss document
                        {
                            data = line.Split(',');
                            int health = int.Parse(data[0]); // Makes health based on first element of data
                            int moveSpeed = int.Parse(data[1]); // Makes moveSpeed based on second element of data
                            int attackSpeed = int.Parse(data[2]); // Makes attackSpeed based on third element of data
                            int damage = int.Parse(data[3]); // Makes damage based on fourth element of data
                            boss = new Boss(health, moveSpeed, attackSpeed, damage, nagaBossTexture, new Rectangle(500, 500, 75, 75), takeDamadge, WindowHeight, WindowWidth, bossName.NagaBoss, attackTextureNA); // Makes Naga boss using data gathered from the file
                        }
                    }
                    catch (Exception e)
                    {
                        // A way to output to user
                    }
                }
                //I commented this out because it would always end up loading the opposite boss dialogue
                /*
                while (randomChoice == 2)
                {
                    randomChoice = rng.Next(1, 3);
                }
                */
            }
            if (boss != null)
            {
                boss.Center();
            }
            
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
                string name = "DEFAULT"; // We would need a way to get user input for name
                names.Add(name);
                times.Add(scoreTimer);
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

        //Adds dialougue to list, after reading in textures type in text and true or false based on if the player is speaking
        //insert null to break to fight
        private void DialogueListAdd()
        {
            switch (randomChoice)
            {
             case 1:
                    //Adds RiceGoddess dialougue
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNDefault, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, "All this life in a place of death, kinda ironic.", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNDefault, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, "This is not a place of death, it's a place of rebirth.", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNDefault, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, "Who are you supposed to be?", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNDefault, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, "I'm the rice goddess and I'm acting as a substitute for now.", false));
                    dialougeList.Add(new Dialogue(playerVNAlternate, RGVNDefault, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, "So you got stuck as a temp, because some other god decided to bail?", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNWise, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, "Yes, even though it is a duty I would rather not wish to perform I must still take \n responsibility!", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNAlternate, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, "Responsibility, which entails me dealing with souls like you!", false));
                    dialougeList.Add(null);
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNDefault, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, "I think that's enough.", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNDefault, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, "Had enough?", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNAlternate, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, "We can go back to the fields if you'd like.", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNDefault, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, "No, I'm good.", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNDefault, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, "I'll give you my blessing, but remember that this is your path.", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNWise, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, "ALL the responsiblity for what happens next lies on your shoulders.", false));
                    dialougeList.Add(null);

             break;

                case 2:
                    //Adds naga dialogue
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, "It's hot as hell down here.", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, "Some heat will do some good to warm up that cold heart of yours.", false));
                    dialougeList.Add(new Dialogue(playerVNAlternate, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, "Who are you calling cold, you snake...thing...whatever you are!", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, "I'm a Naga, and you should really hold your tongue when asking for favors.", false));
                    dialougeList.Add(new Dialogue(playerVNAlternate, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, "I didn't ask you for anything yet, you slimy snake.", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, "Yet, but you wouldn't be here if you weren't seeking my blessing.", false));
                    dialougeList.Add(new Dialogue(playerVNAlternate, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, "Just shut up and get on with it already, eel!", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, "I hate the stubborn ones.", false));
                    dialougeList.Add(null);
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, "...", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, "Well I won, where's the blessing?", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, "I'd never bless such an arrogant soul!", false));
                    dialougeList.Add(new Dialogue(playerVNAlternate, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, "That's not fair!", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, "What's not fair is for everyone to put up with your attitiude.", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, "You're nothing but a pain in the tail!", false));
                    dialougeList.Add(new Dialogue(playerVNAlternate, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, "...", true));
                    dialougeList.Add(new Dialogue(playerVNAlternate, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, "I'm sorry Naga.", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, "See that wasn't so hard, recieve my blessing.", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, "As you progress, don't forget that true strength manifests itself from within.", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, "...", true));
                    dialougeList.Add(null);
                    break;
            
            }
          
        }
    }
}
