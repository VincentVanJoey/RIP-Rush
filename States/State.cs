using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RIPRUSH.States {
    public abstract class State {

        #region Fields

        protected ContentManager content;

        protected Game1 game;

        protected GraphicsDevice graphicsDevice;

        #endregion

        #region Methods

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public abstract void PostUpdate(GameTime gameTime);

        public abstract void Update(GameTime gameTime);

        public State(ContentManager content, Game1 game, GraphicsDevice graphicsDevice) {
            this.content = content;
            this.game = game;
            this.graphicsDevice = graphicsDevice;
        }

        #endregion

    }
}
