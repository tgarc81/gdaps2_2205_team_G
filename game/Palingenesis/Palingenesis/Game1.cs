﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

//Name: G-Force
//Date: 3/16/21
//Updated: 5/3/21
//Professor Mesh
//Purpose: To intialize the base states of our game.
//check
namespace Palingenesis
{
    /// <summary>
    /// enum used in the FSM
    /// </summary>
    enum gameState
    {
        Menu,
        ScoreBoard,
        Game,
        GameOver,
        Dialouge,
        Pause,
        Instructions,
        EnterName
    }
    
    
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch _spriteBatch;

        Rectangle fullScreen = new Rectangle(0, 0, 1920, 1080);

        private Random rng = new Random();

        //game starts in the menu state
        private gameState currentState = gameState.Menu;

        private GameTime gametime = new GameTime();

        private KeyboardState kbState;
        private KeyboardState prevKbState;

        private SpriteFont font;
        private SpriteFont fontVN;

        private double ringTimer; 
        private double specialTimer; // Represents the time elapsed in the game to be used for boss attacks
        private double time; // Represents total time elapsed in the game
        private double bossUpdateTimer;
        private double scoreTimer = 0;
        private double attackTimer;
        private double elapsed = 2;
        private const double Elapsed = 2;

        private Player player; // Represents the actual player
        private Boss boss; //boss object

        //health bar objects
        HealthBar BossHealth;
        HealthBar PlayerHealth;

        private string name = "";

        int randomChoice;
        private int dialogueNum = 0;
        private int numberOfDialougeFrames;
        private int bossesDefeated;
        private const int NumberOfBosses = 2;
        private const int WindowWidth = 1920;
        private const int WindowHeight = 1080;

        // private List<string> dialougeList = new List<string>();
        private List<Dialogue> dialougeList;
        private List<string> names; // Represents all the names to go in leaderboard
        private List<double> times; // Represents all the times to go in leaderboard

        private String currentLine;

        private bool isRiceGoddessLoaded; // Bool that represents whether Rice Goddess boss has been loaded in this game
        private bool isNagaBossLoaded; // Bool that represents whether Naga boss has been loaded in this game
        private bool Boss1Beaten;
        private bool Boss2Beaten;
        private bool IsMusicPlaying;
        private bool hasFoughtBoss2;

        //Texture2D attributes
        private Texture2D playerAsset; // Represents player image
        private Texture2D attackTextureRG;
        private Texture2D attackTextureNA;
        private Texture2D attackTexturePlayer;
        private Texture2D riceGoddessTexture;
        private Texture2D RiceGoddessBackground;
        private Texture2D NagaBackground;
        private Texture2D titleScreen;
        private Texture2D instructions;
        private Texture2D scoreBoard;
        private Texture2D gameOver;
        private Texture2D bar;
        private Texture2D RGteleportTexture;
        private Texture2D NAteleportTexture;
        private Texture2D nagaBossTexture;

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
        private Texture2D NAVNAlternate;
        private Texture2D NAVNExasperated;
        private Texture2D NAVNMad;
        private Texture2D NAbackgroundVN;
        private Texture2D pauseScreen;

        //Load sound effects and songs
        private SoundEffect takeDamadge;
        private SoundEffect deathSound;
        private SoundEffect forwardVN;
        private SoundEffect hit;
        private SoundEffect error;
        private Song RiceGoddessOST;
        private Song VNRiceGoddessOST;
        private Song NagaOST;
        private Song VNNagaOST;

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
            isNagaBossLoaded = false; // Sets it so Naga boss is not loaded yet
            bossesDefeated = 0;
            Boss1Beaten = false;
            Boss2Beaten = false;
            hasFoughtBoss2 = false;
            names = new List<string>();
            times = new List<double>();
            dialougeList = new List<Dialogue>();

            Random rng = new Random();
            randomChoice = rng.Next(1,3);

            //Media player properties (change volume if sounds are too loud)
            MediaPlayer.Volume = 1;
            MediaPlayer.IsRepeating = true;
            IsMusicPlaying = false;

            graphics.PreferredBackBufferWidth = WindowWidth;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = WindowHeight;   // set this value to the desired height of your window

            graphics.ApplyChanges();
            base.Initialize();

            
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            
            //Loading fonts
            font = Content.Load<SpriteFont>("font");
            fontVN = Content.Load<SpriteFont>("bigfont");
            
            //Load title screen textures
            titleScreen = Content.Load<Texture2D>("titlescreen");
            instructions = Content.Load<Texture2D>("InstructionsPlaceHolder");
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
            RGteleportTexture = Content.Load<Texture2D>("RGTeleport");
            NAteleportTexture = Content.Load<Texture2D>("NagaTeleport");
            takeDamadge = Content.Load<SoundEffect>("takeDamadge"); 
            deathSound= Content.Load<SoundEffect>("deathSound");
            hit = Content.Load<SoundEffect>("Bonk");
            RiceGoddessOST = Content.Load<Song>("Sad but True - The HU");
            NagaOST = Content.Load<Song>("Landia - Kirby's Return to Dream Land");

            //Load GameOver Textures
            gameOver = Content.Load<Texture2D>("GameOverScreen");

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
            NAVNDefault= Content.Load<Texture2D>("nagaBossVNMain");
            NAVNAlternate = Content.Load<Texture2D>("nagaBossVnAlternate");
            NAVNExasperated = Content.Load<Texture2D>("nagaclosedNervous");
            NAVNMad = Content.Load<Texture2D>("nagaopenMad");
            forwardVN = Content.Load<SoundEffect>("forward_sound");
            VNRiceGoddessOST = Content.Load<Song>("10 Min.Meditation Music for Positive Energy - GUARANTEED Find Inner Peace within 10 Min.");
            VNNagaOST = Content.Load<Song>("Fire Sanctuary - The Legend of Zelda Skyward Sword");
           


            player = new Player(100, 10, 10, 20, playerAsset, new Rectangle(200, 200, 50, 50),hit, WindowHeight, WindowWidth, attackTexturePlayer);
            // TODO: make a placeholder asset for the boss
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
                       
                        LoadBoss(randomChoice);
                        player.Reset();
                        boss.Reset();
                        scoreTimer = 0;
                        DialogueListAdd();
                        currentState = gameState.Instructions;
                        forwardVN.Play();
                        LoadHealthBars();
                       
                    }

                    break;

                case gameState.Instructions:
                    //when the player is finished reading instructions presses 
                    if(SingleKeyPress(Keys.Enter, kbState))
                    {
                        currentState = gameState.EnterName;
                    }

                    break;

                case gameState.EnterName:
                    bool nameEntered = false;

                    while (!nameEntered)
                    {
                        Keys currentKey = ScoreboardInput();

                        if (currentKey != Keys.Back)
                        {
                            name += currentKey.ToString();
                        }
                        else
                        {
                            name = name[name.Length - 1];
                        }
                        //run method to enter letters into the name here

                        if (SingleKeyPress(Keys.Enter, kbState))
                        {
                            nameEntered = true;
                        }
                        else if (SingleKeyPress(Keys.Back, kbState))
                        {

                        }
                    }


                    if (SingleKeyPress(Keys.Enter, kbState) && nameEntered == true)
                    {
                        currentState = gameState.Dialouge;
                    }
                    break;

                case gameState.Game:
                    //updates player and bosses
                    player.Update();
                    boss.Update(player, bossUpdateTimer);
                    player.Attack(boss, prevKbState);
                    BossHealth.Update(boss.Health);
                    PlayerHealth.Update(player.Health);

                    if (randomChoice==1 && !IsMusicPlaying)
                    {
                        MediaPlayer.Play(RiceGoddessOST);
                        IsMusicPlaying = true;
                    }

                    if (randomChoice==2 && !IsMusicPlaying)
                    {
                        MediaPlayer.Play(NagaOST);
                        IsMusicPlaying = true;
                    }

                    //this is the timer component needed for the special attack method
                    float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    elapsed -= time;

                    //logic for drawing each order based on elapsed seconds
                    if (elapsed < 0 && boss.IsCharging == false)
                    {
                        //I moved the random variable generation oustide the AI method to save on memory
                        int tmp = rng.Next(0, 6);
                       
                        boss.AI(tmp, player, elapsed, gameTime);

                        //updates timer
                        elapsed = Elapsed;
                    }

                    //AI method runs every 2 seconds
                    /*if (timer > 2)e
                    */
                    //runs update on each bullet after the pattern is spawned by AI
                    for (int i = 0; i < boss.ProjectileList.Count; i++)
                    {
                        boss.ProjectileList[i].Update(specialTimer, ringTimer, boss);
                    }

                    for (int i = 0; i < player.ShotList.Count; i++)
                    {
                        player.ShotList[i].Update();
                    }

                    for (int i = 0; i < boss.ProjectileList.Count; i++)
                    {
                        if (boss.ProjectileList[i].HasHit == true)
                        {
                            boss.ProjectileList.RemoveAt(i);
                        }
                    }

                    for (int i = 0; i < player.ShotList.Count; i++)
                    {
                        if (player.ShotList[i].HasHit == true)
                        {
                            player.ShotList.RemoveAt(i);
                        }
                    }

                    scoreTimer += gameTime.ElapsedGameTime.TotalSeconds;
                    attackTimer += gameTime.TotalGameTime.TotalSeconds;

                    if(boss.SpecialActive == true)
                    {
                        specialTimer += gameTime.ElapsedGameTime.TotalSeconds;
                    }

                    if(boss.RingActive == true)
                    {
                        ringTimer += gameTime.ElapsedGameTime.TotalSeconds;
                    }

                    if(specialTimer > 4)
                    {
                        specialTimer = 0;

                    }

                    if(boss.ChargeEnded == true)
                    {
                        bossUpdateTimer += gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    else
                    {
                        bossUpdateTimer = 0;   
                    }
                   
                    //Console.WriteLine(gameTime.ElapsedGameTime.TotalSeconds);
                    //pressing escape during the game pauses
                    if (SingleKeyPress(Keys.P, kbState))
                    {
                        currentState = gameState.Pause;
                    }

                    //if the players health reaches zero game over
                    if(player.Health <= 0)
                    {
                        deathSound.Play();
                        currentState = gameState.GameOver;
                        IsMusicPlaying = false;
                        MediaPlayer.Stop();
                    }

                    //when the bosses health hit's zero, starts the next dialouge section
                    if ((boss.Health <= 0) && (Boss1Beaten == true))
                    {
                        Boss2Beaten = true;
                        currentState = gameState.Dialouge;
                        IsMusicPlaying = false;
                        MediaPlayer.Stop();

                    }

                    else if (boss.Health <= 0)
                    {
                        //resets player health and changes to dialogue section
                        Boss1Beaten = true;
                        currentState = gameState.Dialouge;
                        IsMusicPlaying = false;
                        player.Health = player.MaxHealth;
                        PlayerHealth.ResetHealth(player.MaxHealth);
                        MediaPlayer.Stop();

                        //Loads other boss that was not loaded already
                        if(randomChoice==1)
                        {
                            LoadBoss(2);
                            randomChoice = 2;
                            DialogueListAdd();
                            LoadHealthBars();
                        }
                        else if(randomChoice==2)
                        {
                            LoadBoss(1);
                            randomChoice = 1;
                            DialogueListAdd();
                            LoadHealthBars();
                        }

                       
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

                    //Determines which music to play
                    if ((randomChoice == 2) && (Boss1Beaten == true) && (hasFoughtBoss2 == false) && (!IsMusicPlaying))
                    {
                        MediaPlayer.Play(VNRiceGoddessOST);
                        IsMusicPlaying = true;
                    }

                    else if ((randomChoice == 1) && (Boss1Beaten == true) && (hasFoughtBoss2 == false) && (!IsMusicPlaying))
                    {
                        MediaPlayer.Play(VNNagaOST);
                        IsMusicPlaying = true;
                    }

                    else if (randomChoice == 1 && !IsMusicPlaying)
                    {
                        MediaPlayer.Play(VNRiceGoddessOST);
                        IsMusicPlaying = true;
                    }

                    else if (randomChoice == 2 && !IsMusicPlaying)
                    {
                        MediaPlayer.Play(VNNagaOST);
                        IsMusicPlaying = true;
                    }
                    
                    //each time the user presses enter the dialouge advances by one line
                    if (SingleKeyPress((Keys.Enter), kbState))
                    { 
                        dialogueNum++;
                        forwardVN.Play();
                    }

                    //if you finish all the dialouge after beating both bosses it will take you to the scoreboard
                    if ((dialougeList[dialogueNum] == null) && (Boss1Beaten == true) && (Boss2Beaten==true))
                    {
                        currentState = gameState.ScoreBoard;
                        dialogueNum = 0;
                        dialougeList.Clear();
                        IsMusicPlaying = false;
                        Boss1Beaten = false;
                        Boss2Beaten = false;
                        hasFoughtBoss2 = false;
                        MediaPlayer.Stop();
                        LoadScoreboard();
                        break;
                    }

                    //checks for dialogue after game
                    if ((dialougeList[dialogueNum + 1] == null) && (Boss1Beaten == true) &&(Boss2Beaten==false) && (hasFoughtBoss2==false))
                    {
                        currentState = gameState.Dialouge;
                        dialogueNum =  dialogueNum+2;
                        IsMusicPlaying = false;
                        hasFoughtBoss2 = true;
                    }

                    //when there is no remaining dialouge starts the next section of the game
                    if (dialougeList[dialogueNum]== null)
                    {
                        currentState = gameState.Game;
                        IsMusicPlaying = false;
                        MediaPlayer.Stop();
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
                    //drawing title screen
                    _spriteBatch.Draw(titleScreen, fullScreen, Color.White);
                    
                    break;

                case gameState.Instructions:
                    _spriteBatch.Draw(titleScreen, fullScreen, Color.White);
                    // TODO: make image for instructions
                    _spriteBatch.Draw(instructions, new Vector2(), Color.White);
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

                    //drawing game text
                    _spriteBatch.DrawString(font, "PlaceHolder for game", new Vector2(0, 0), Color.White);
                    _spriteBatch.DrawString(font, "Use WASD to move player", new Vector2(0, 20), Color.White);
                    _spriteBatch.DrawString(font, "Use arrow keys to attack", new Vector2(0, 40), Color.White);
                     
                    _spriteBatch.DrawString(font, "Press P to pause", new Vector2(0, 60), Color.White);
                    _spriteBatch.DrawString(font, string.Format("Player Health: {0}", player.Health), new Vector2(0, 80), Color.White);
                    _spriteBatch.DrawString(font, string.Format("Boss Health: {0}", boss.Health), new Vector2(0, 100), Color.White);


                    //debug only
                    //_spriteBatch.DrawString(font, string.Format("{0}", specialTimer), new Vector2(0, 150), Color.White);

                    //calling object draw methods
                    player.Draw(_spriteBatch, Color.White);

                    foreach (Bullet obj in boss.ProjectileList)
                    {
                        if (obj.HasHit)
                        {
                            player.Draw(_spriteBatch, Color.Red);
                        }
                    }
                    //TODO: If bullet collides with player, pass in red instead

                    BossHealth.Draw(_spriteBatch);
                    PlayerHealth.Draw(_spriteBatch);

                    //drawing projectiles
                    boss.Draw(_spriteBatch, Color.White);
                    for(int i =0; i < boss.ProjectileList.Count; i++)
                    {
                        boss.ProjectileList[i].Draw(_spriteBatch, Color.White);
                        
                    }

                    for (int i = 0; i < player.ShotList.Count; i++)
                    {
                        player.ShotList[i].Draw(_spriteBatch, Color.White);

                    }

                   
                    break;

                case gameState.GameOver:
                    _spriteBatch.Draw(gameOver, fullScreen, Color.White);
                    _spriteBatch.DrawString(font, "Press enter to return to Main menu", new Vector2(0, 0), Color.White);
                    
                    break;

                case gameState.Dialouge:
                    //draws object from dialgoe list using that object's dialogue method.
                    dialougeList[dialogueNum].Draw(_spriteBatch); // possible bug
                    // TODO: figure out the actual position later
                    
                   // _spriteBatch.DrawString(font, string.Format("{0}", currentLine), new Vector2(100, 500), Color.White);
                    break;
                  
                case gameState.Pause:
                    _spriteBatch.Draw(pauseScreen, fullScreen, Color.White);
                    break;

                case gameState.ScoreBoard:
                    //drawing final time on scoreboard
                    _spriteBatch.Draw(scoreBoard, fullScreen, Color.White);
                    _spriteBatch.DrawString(fontVN, String.Format("final time: {0:F} seconds", scoreTimer), new Vector2(1150, 200), Color.White);
                    dialogueNum = 0;
                    break;
            }


            _spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Helper method to check if a key has been pressed
        /// </summary>
        /// <param name="key"></param>
        /// <param name="kbState"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Loads in boss data from file
        /// </summary>
        private void LoadBoss(int randomChoice)
        {
            //temporarily set so only the naga will appear
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
                            boss = new Boss(health, moveSpeed, attackSpeed, damage, riceGoddessTexture, new Rectangle(500, 500, 100, 100), takeDamadge, WindowHeight, WindowWidth, bossName.RiceGoddess, attackTextureRG); // Makes Rice Goddess using data gathered from the file
                        }
                    }
                    catch (Exception e)
                    {
                        // TODO: A way to output to user
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
                            boss = new Boss(health, moveSpeed, attackSpeed, damage, nagaBossTexture, new Rectangle(500, 500, 200, 200), takeDamadge, WindowHeight, WindowWidth, bossName.NagaBoss, attackTextureNA); // Makes Naga boss using data gathered from the file
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
                // TODO: Output error message
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
                // TODO: Error message
            }
            // Ensure that we can close the file, as long as it was actually opened in the first place
            if (input != null)
            {
                input.Close();
            }
        }

        /// <summary>
        /// Adds dialougue to list, after reading in textures type in text and true or false based on if the player is speaking, insert null to break to fight
        /// </summary>
        private void DialogueListAdd()
        {
            

            switch (randomChoice)
            {
                
             case 1:
                    //Adds RiceGoddess dialougue
                    string BossName = "Rice Goddess";
                    Color BossColor = Color.Green;
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNDefault, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "All this life in a place of death, kinda ironic.", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNDefault, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "This is not a place of death, it is a place of rebirth.", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNDefault, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName,"Who are you supposed to be?", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNDefault, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "I am the rice goddess and I'm acting as a substitute for now.", false));
                    dialougeList.Add(new Dialogue(playerVNAlternate, RGVNDefault, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "So you got stuck as a temp, because some other god decided to bail?", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNWise, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "Yes, even though it is a duty I would rather not wish to perform I must still take \n responsibility!", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNAlternate, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "Responsibility, which entails me dealing with souls like you!", false));
                    dialougeList.Add(null);
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNDefault, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "I think that's enough.", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNDefault, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "Had enough?", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNAlternate, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "We can continue if you'd like.", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNWise, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "I only stopped for your sake", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNDefault, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "No, I'm good.", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNDefault, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "I'll give you my blessing, but remember that this is your path.", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNWise, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "ALL the responsiblity for what happens next lies on your shoulders.", false));
                    dialougeList.Add(null);

                    break;

                case 2:
                    //Adds naga dialogue
                    BossName = "Naga";
                    BossColor = Color.Orange;
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNAlternate, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "It's hot as hell down here.", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "Some heat will do some good to warm up that cold heart of yours.", false));
                    dialougeList.Add(new Dialogue(playerVNAlternate, NAVNAlternate, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "Who are you calling cold, you snake...thing...whatever you are.", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "I am Naga, and you should really hold your tongue when asking for favors.", false));
                    dialougeList.Add(new Dialogue(playerVNAlternate, NAVNAlternate, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "I didn't ask you for anything yet, you slimy snake.", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "Yet, but you wouldn't be here if you weren't seeking my blessing.", false));
                    dialougeList.Add(new Dialogue(playerVNAlternate, NAVNAlternate, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "Just shut up and get on with it already, eel.", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNExasperated, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "I hate the stubborn ones.", false));
                    dialougeList.Add(null);
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNExasperated, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "...", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNAlternate, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "Well I won, where's the blessing?", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNMad, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "I'd never bless such an arrogant soul!", false));
                    dialougeList.Add(new Dialogue(playerVNAlternate, NAVNAlternate, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "No fair!", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNMad, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "What's not fair is for everyone to put up with your attitiude.", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNMad, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "You're nothing but a pain in the tail!", false));
                    dialougeList.Add(new Dialogue(playerVNAlternate, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "...", true));
                    dialougeList.Add(new Dialogue(playerVNAlternate, NAVNAlternate, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "Sorry Naga.", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "See that wasn't so hard, recieve my blessing.", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "As you progress, don't forget that true strength manifests itself from within.", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNAlternate, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "...", true));
                    dialougeList.Add(null);
                    break;
            
            }
          
        }

        /// <summary>
        /// Loads all the data for the health bars
        /// </summary>
        public void LoadHealthBars()
        {
            //Makes the rectangles for player and boss HP
            Rectangle BossHPBar = new Rectangle(WindowWidth / 2 - WindowWidth / 4, WindowHeight / 10, 800, 50);
            Rectangle PlayerHPBar = new Rectangle(WindowWidth / 25, WindowHeight - WindowHeight / 20, 350, 50);

            //Loads Health bar for Theophania
            PlayerHealth = new HealthBar(textboxVN, bar, PlayerHPBar, new Vector2((WindowWidth / 10 - PlayerHPBar.Width / 4), WindowHeight - WindowHeight / 10), fontVN, "Theophania", player.MaxHealth);
            switch (randomChoice)
            {
                case 1:
                    //Loads bigger rice goddess health bar
                    BossHealth = new HealthBar(textboxVN, bar, BossHPBar, new Vector2(WindowWidth / 2 - BossHPBar.Width / 4, (WindowHeight / 10) - WindowHeight / 20), fontVN, "Rice Goddess", boss.MaxHealth);
                    boss.TeleportTexture = RGteleportTexture;
                    break;

                case 2:
                    //Loads smaller Naga health bar
                    BossHealth = new HealthBar(textboxVN, bar, BossHPBar, new Vector2(WindowWidth / 2 - BossHPBar.Width / 4, (WindowHeight / 10) - WindowHeight / 20), fontVN, "Naga", boss.MaxHealth);
                    boss.TeleportTexture = NAteleportTexture;
                    break;
            }
        }

        /*
         * what we need: name to display + save to scoreboard file
         * 
         * - see which keys are being pressed
         *      - use kbstate.GetKeysPressed
         *      - have conditions for pressing enter or backspace
         * - save pressed keys to a string of characters
         * - return the string and add it as a key to the dictionary (TBD)
        */
        private Keys ScoreboardInput()
        {
            //string name = null;
            //Keys input = Keys.S;

            Keys[] currentKeyArray = kbState.GetPressedKeys();

            Keys LastKeyPressed = currentKeyArray[0];

            return LastKeyPressed;
        }
    
    }
}
