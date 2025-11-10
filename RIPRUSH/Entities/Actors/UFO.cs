using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using RIPRUSH.Entities.CollisionShapes;
using RIPRUSH.Scenes;
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
        private Vector2 _initialPosition;
        public float move_distance = 75f;
        private float amplitude;
        public Vector2 velocity;

        // Horizontal speed (the one that actualy matters
        private float _horizontalSpeed = 400f;

        private BoundingCircle bounds;

        public bool isFrozen = false;

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingCircle Bounds => bounds;

        public UFO(ContentManager content, bool isAnimated, float scale, Vector2 position) {
            animations = new Dictionary<string, Animation>();
            Scale = scale;
            _isAnimated = isAnimated;

            LoadAnimations(content);

            // Initialize the AnimationManager (it will be set to null if not animated)
            if (animations.Count != 0) {
                animationManager = new AnimationManager(animations.First().Value);
            }

            Position = position;
            _initialPosition = position;

            int boundWidth = animationManager.animation.FrameWidth;
            int boundHeight =  animationManager.animation.FrameHeight;

            Origin = new Vector2(boundWidth / 2f, boundHeight / 2f); // Make sure Origin is the center
            bounds.Center = Position;  // Bounding circle always at visual center
            bounds.Radius = 0.8f * (boundWidth / 2f * Scale); // Radius scales with the sprite
            Vector2 circleCenter = Position + new Vector2(boundWidth * Scale / 2f, boundHeight * Scale / 2f);
            bounds = new BoundingCircle(circleCenter, bounds.Radius);

            Random rng = new Random(); // You can make this static to avoid reseeding issues
            amplitude = move_distance * (0.5f + (float)rng.NextDouble());
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
            animationManager.animation.Rotation = Rotation;
            animationManager.animation.Origin = Origin;
            // TODO: Might need to do this for Color, Rotation, SpriteEffect, etc if I ever change them too

        }

        /// <summary>
        /// The method responsible for moving the ufo sprite
        /// </summary>
        /// <param name="gameTime">the time state of the game</param>
        public override void Move(GameTime gameTime) {

            var scene = Core.GetActiveScene() as GameScene;
            

            // Dynamically adjust horizontal speed to match world scroll
            float worldSpeed = (float)scene?.worldManager._scrollSpeed;
            float scrollFactor = worldSpeed / 400f; // 400f is your old baseline
            float effectiveSpeed = _horizontalSpeed * scrollFactor;

            Position = new Vector2(
                Position.X - effectiveSpeed * (float)(gameTime.ElapsedGameTime.TotalSeconds),
                _initialPosition.Y + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * SPEED) * amplitude
            );

            int boundWidth = animationManager.animation.FrameWidth;
            int boundHeight = animationManager.animation.FrameHeight;
            Origin = new Vector2(boundWidth / 2f, boundHeight / 2f); // Make sure Origin is the center
            bounds.Center = Position;  // Bounding circle always at visual center
            bounds.Radius = 0.8f * (boundWidth / 2f * Scale); // Radius scales with the sprite

            // Mark inactive if offscreen
            if (Position.X + boundWidth < 0) {
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

            if (!isFrozen) {
                Move(gameTime);   
            }
            SetAnimations();


            int boundWidth = animationManager.animation.FrameWidth;
            int boundHeight = animationManager.animation.FrameHeight;
            Origin = new Vector2(boundWidth / 2f, boundHeight / 2f); // Make sure Origin is the center
            bounds.Center = Position;  // Bounding circle always at visual center
            bounds.Radius = 0.8f * (boundWidth / 2f * Scale); // Radius scales with the sprite

            base.Update(gameTime);
        }

    }
}