using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RIPRUSH.Entities.CollisionShapes;

namespace RIPRUSH.Entities.Actors {

    /// <summary>
    /// A class representing a platform
    /// </summary>
    public class Platform : Sprite {
        
        /// <summary>
        /// The direction the platform is moving
        /// </summary>
        public Direction Direction;

        private BoundingRectangle bounds;

        //private Texture2D collisiontestshape;

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        public Platform(ContentManager content, float scale) {
            
            Scale = scale;
            //collisiontestshape = content.Load<Texture2D>("Assets/test_shape");
            _texture = content.Load<Texture2D>("Assets/Button");

            // Set bounds radius
            bounds = new BoundingRectangle(Position, _texture.Width * Scale, _texture.Height * Scale);

        }

        /// <summary>
        /// The method responsible for moving
        /// </summary>
        /// <param name="gameTime">the time state of the game</param>
        public virtual void Move(GameTime gameTime) { }

        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance used to draw textures and sprites to the screen.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {

            //any specific  draw logic here in the future?
            // In case I ever let the play customize it in any way
            // Color or hats or something

            base.Draw(gameTime, spriteBatch);

            //To show collision bounds --DEBUG ONLY
            //var rect = new Rectangle((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height);
            //spriteBatch.Draw(collisiontestshape, rect, Color.DarkRed);

        }

        /// <summary>
        /// The update logic
        /// </summary>
        /// <param name="gameTime">the time state of the game</param>
        public override void Update(GameTime gameTime) {

            Move(gameTime);

            bounds.X = Position.X;
            bounds.Y = Position.Y;

            base.Update(gameTime);
        }

    }
}