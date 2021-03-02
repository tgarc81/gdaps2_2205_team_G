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
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardState kbState;
        private KeyboardState prevKbState;
        private SpriteFont font;

        //game starts in the menu state
        private gameState currentState = gameState.Menu;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
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
            font = This.LoadContent<SpriteFont>("font");
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
    }
}
