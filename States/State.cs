using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RIPRUSH.States {

    /// <summary>
    /// An abstract class representing a generic state of the game
    /// </summary>
    public abstract class State {

        #region Fields

        /// <summary>
        /// The current state's contentmanager
        /// </summary>
        protected ContentManager _content;

        /// <summary>
        /// The actual game base object
        /// </summary>
        protected Game1 _game;

        /// <summary>
        /// The graphics device that handles the rendering
        /// </summary>
        protected GraphicsDevice _graphicsDevice;

        #endregion

        #region Methods

        /// <summary>
        /// Draws the current state
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance used to draw textures and sprites to the screen.</param>
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        /// <summary>
        /// The current state's post update logic, if any
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        public abstract void PostUpdate(GameTime gameTime);

        /// <summary>
        /// The current state's update logic
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        public abstract void Update(GameTime gameTime);

        public State(ContentManager content, Game1 game, GraphicsDevice graphicsDevice) {
            this._content = content;
            this._game = game;
            this._graphicsDevice = graphicsDevice;
        }

        #endregion

    }
}
