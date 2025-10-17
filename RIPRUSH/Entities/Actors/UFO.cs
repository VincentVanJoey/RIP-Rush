using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RIPRUSH.Entities.CollisionShapes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RIPRUSH.Entities.Actors {
    /// <summary>
    /// A class representing the enemy ufo sprite in the game
    /// </summary>
    public class UFO : Enemy {

        // Fields to avoid "magic numbers"
        private const float SPEED = 5f;
        private const float HORIZONTAL_SPEED = 200f;
        private Vector2 _initialPosition;
        public float move_distance = 75f;
        private float amplitude;
        public Vector2 velocity;

        private BoundingRectangle bounds;

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        public UFO(ContentManager content, bool isAnimated, float scale, Vector2 position) {
            animations = new Dictionary<string, Animation>();
            Scale = scale;

            // Set _isAnimated based on the constructor parameter
            _isAnimated = isAnimated;

            // Load specific animations if the sprite is animated
            if (_isAnimated) {
                LoadAnimations(content);

                // Initialize the AnimationManager (it will be set to null if not animated)
                if (animations.Count != 0) {
                    animationManager = new AnimationManager(animations.First().Value);
                }
            }
            else {
                // Load a static texture if not animated
                _texture = content.Load<Texture2D>("Assets/face"); //placeholder, don't need a static ufo for right now
            }


            Position = position;
            _initialPosition = position;

            int boundwidth = _isAnimated ? animationManager.animation.FrameWidth : _texture.Width;
            int boundheight = _isAnimated ? animationManager.animation.FrameHeight : _texture.Height;

            Random rng = new Random(); // You can make this static to avoid reseeding issues
            amplitude = move_distance * (0.5f + (float)rng.NextDouble());

            bounds = new BoundingRectangle(Position, (boundwidth - 4) * Scale, (boundheight - 4) * Scale);
        }

        public void LoadAnimations(ContentManager content) {
            Animation flyAnimation = new(content.Load<Texture2D>("Assets/Enemy/UFO"), 19, true, Color, Origin, Rotation, Scale);
            animations.Add("Fly", flyAnimation);
        }

        /// <summary>
        /// The method responsible for determining which animation to play
        /// </summary>
        public void SetAnimations() {
            if (animationManager == null) {
                return;
            }
            // Update animation properties to match the entity's current state
            animationManager.animation.Scale = Scale;
            // TODO: Might need to do this for Color, Rotation, SpriteEffect, etc if I ever change them too

        }

        /// <summary>
        /// The method responsible for moving the ufo sprite
        /// </summary>
        /// <param name="gameTime">the time state of the game</param>
        public override void Move(GameTime gameTime) {

            Position = new Vector2(
                Position.X - HORIZONTAL_SPEED * (float)(gameTime.ElapsedGameTime.TotalSeconds),
                _initialPosition.Y + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * SPEED) * amplitude
            );
            bounds.X = Position.X;
            bounds.Y = Position.Y;

            // Mark inactive if offscreen
            if (Position.X + bounds.Width < 0) {
                IsActive = false;
            }

        }

        public override void CheckCollision(Pumpkin player) {
            if (Bounds.CollidesWith(player.Bounds)) {
                player.TakeDamage();
            }
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

            if (_isAnimated) {
                Move(gameTime);
                SetAnimations();
            }

            base.Update(gameTime);
        }

    }
}