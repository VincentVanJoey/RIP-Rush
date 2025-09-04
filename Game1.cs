using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RIPRUSH.States;

namespace RIPRUSH
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private State currentState;
        private State nextState;

        public Color backgroundColor;

        public void ChangeState(State state)
        {
            nextState = state;
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            currentState = new MainMenuState(Content, this, graphics.GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (nextState != null) { 
                currentState = nextState;
                nextState = null;
            }

            currentState.Update(gameTime);

            currentState.PostUpdate(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColor);

            currentState.Draw(gameTime, spriteBatch);

            base.Draw(gameTime);
        }
    }
}
