using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RIPRUSH.Entities.CollisionShapes;
using System;

namespace RIPRUSH.Entities.Actors {

    /// <summary>
    /// A class representing a platform
    /// </summary>
    public class Platform : Sprite {

        private const float SPEED = 1f;
        private Vector2 _initialPosition;
        private float moveProgress = 75f;
        private bool movingUp = true;
        public float move_distance = 0f;
        public bool moving = false;

        private BoundingRectangle bounds;

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        public Platform(ContentManager content, float scale, Vector2 position) {

            _initialPosition = position;
            Position = position;
            Scale = scale;
            _texture = content.Load<Texture2D>("Assets/candycornplat");

            // Set bounds radius
            bounds = new BoundingRectangle(Position, _texture.Width * Scale, _texture.Height * Scale);

        }

        /// <summary>
        /// The method responsible for moving
        /// </summary>
        /// <param name="gameTime">the time state of the game</param>
        public void Move(GameTime gameTime) {
            if (movingUp) {
                moveProgress += SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (moveProgress >= 1f) {
                    moveProgress = 1f;
                    movingUp = false;
                }
            }
            else {
                moveProgress -= SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (moveProgress <= 0f) {
                    moveProgress = 0f; 
                    movingUp = true;
                }
            }

            float offset = MathHelper.Lerp(0f, move_distance, moveProgress);
            Position = new Vector2(_initialPosition.X, _initialPosition.Y - offset);
        }

        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance used to draw textures and sprites to the screen.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {

            //any specific  draw logic here in the future?
            // In case I ever let the play customize it in any way

            base.Draw(gameTime, spriteBatch);
        }

        /// <summary>
        /// The update logic
        /// </summary>
        /// <param name="gameTime">the time state of the game</param>
        public override void Update(GameTime gameTime) {
            if (moving) {
                Move(gameTime);
            }

            bounds.X = Position.X;
            bounds.Y = Position.Y;

            base.Update(gameTime);
        }

    }
}