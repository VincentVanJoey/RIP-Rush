using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RIPRUSH.Entities {

    /// <summary>
    /// A base class for all game components that would be needed to be drawn and updated.
    /// </summary>
    public abstract class Component {

        /// <summary>
        /// Renders the game object using the specified sprite batch.
        /// </summary>
        /// <remarks>This method is abstract and must be implemented by derived classes to define the
        /// specific rendering behavior of the game object.</remarks>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance used to draw textures and sprites to the screen.</param>
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        /// <summary>
        /// Updates the game object using the specified sprite batch.
        /// </summary>
        /// <remarks>This method is abstract and must be implemented by derived classes to define the
        /// specific update behavior of the game object.</remarks>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        public abstract void Update(GameTime gameTime);
    }
}
