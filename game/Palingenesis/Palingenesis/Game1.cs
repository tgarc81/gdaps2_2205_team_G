using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

            player = new Player(100, 10, 10, 20, playerAsset, new Rectangle(0, 0, 10, 10), windowWidth, windowHieght);
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

                    break;

                case gameState.GameOver:
                    //pressing neter on the game over screen sends you to the menu
                    if(SingleKeyPress(Keys.Enter, kbState))
                    {
                        currentState = gameState.Menu;
                    }
                    break;

                case gameState.Dialouge:

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
                    
                    break;

                case gameState.Game:

                    break;

                case gameState.GameOver:

                    break;

                case gameState.Dialouge:

                    break;

                case gameState.Pause:

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
