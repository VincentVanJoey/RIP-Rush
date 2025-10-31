using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RIPRUSH.Entities.CollisionShapes;

namespace RIPRUSH.Entities.Actors {

    /// <summary>
    /// A class representing a win flag 
    /// </summary>
    public class WinFlag : Sprite {

        private BoundingRectangle bounds;

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        public WinFlag(ContentManager content, float scale, Vector2 position) {
            Position = position;
            Scale = scale;
            _texture = content.Load<Texture2D>("Assets/WinFlag");

            // Set bounds radius
            bounds = new BoundingRectangle(Position, _texture.Width * Scale, _texture.Height * Scale);

        }

        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance used to draw textures and sprites to the screen.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            base.Draw(gameTime, spriteBatch);
        }

        /// <summary>
        /// The update logic
        /// </summary>
        /// <param name="gameTime">the time state of the game</param>
        public override void Update(GameTime gameTime) {
            bounds.X = Position.X;
            bounds.Y = Position.Y;

            base.Update(gameTime);
        }

    }
}