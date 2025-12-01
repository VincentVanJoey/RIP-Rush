using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using RIPRUSH.Entities.CollisionShapes;
using RIPRUSH.Entities.Particles;
using RIPRUSH.Scenes;
using System.Collections.Generic;
using System.Linq;

namespace RIPRUSH.Entities.Actors {

    public class Fireeye : Enemy, IParticleEmitter {

        private float _speed = 700f;  // horizontal speed
        private Vector2 _spawnPosition;
        private BoundingCircle bounds;
        public BoundingCircle Bounds => bounds;

        private float _launchDelay = 0.5f; // seconds before moving
        private float _timeSinceSpawn = 0f;
        public Vector2 Velocity => new Vector2(-_speed, 0f); // always moving left

        private SparkParticleSystem sparkSystem;

        public Fireeye(ContentManager content, float scale, Vector2 playerPosition, bool isAnimated = true) {
            Scale = scale;
            _spawnPosition = new Vector2(
                Core.GraphicsDevice.Viewport.Width + 20 ,
                playerPosition.Y
            );
            Position = _spawnPosition;

            animations = new Dictionary<string, Animation>();
            _isAnimated = isAnimated;

            LoadAnimations(content);

            // Initialize AnimationManager
            if (animations.Count != 0) {
                animationManager = new AnimationManager(animations.First().Value);
            }

            int width = animationManager?.animation.FrameWidth ?? 32;  // fallback
            int height = animationManager?.animation.FrameHeight ?? 32;

            Origin = new Vector2(width / 2f, height / 2f);

            // Initialize bounding circle at the visual center
            bounds = new BoundingCircle(Position + new Vector2(width * Scale / 2f, height * Scale / 2f),0.5f * (width / 2f * Scale));
            IsActive = true;

            sparkSystem = new SparkParticleSystem(Core.Instance, this);
            Core.Instance.Components.Add(sparkSystem);
        }

        private void LoadAnimations(ContentManager content) {
            Animation flyAnimation = new Animation(
                content.Load<Texture2D>("Assets/Enemy/eye_fire"),
                frameCount: 7,
                isLooping: true,
                Color.White,
                Origin,
                Rotation,
                Scale
            );
            animations.Add("Fly", flyAnimation);
        }

        /// <summary>
        /// Update animation properties each frame
        /// </summary>
        private void SetAnimations() {
            if (animationManager == null) return;

            animationManager.animation.Scale = Scale;
            animationManager.animation.Rotation = Rotation;
            animationManager.animation.Origin = Origin;
        }

        public override void Move(GameTime gameTime) {
            var scene = Core.GetActiveScene() as GameScene;
            float worldSpeed = (float)scene?.worldManager._scrollSpeed;
            float scrollFactor = worldSpeed / 400f; // baseline
            float effectiveSpeed = _speed * scrollFactor;

            _timeSinceSpawn += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Only move after launch delay
            if (_timeSinceSpawn >= _launchDelay) {
                _position.X -= effectiveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            // Update animation position AFTER moving
            if (animationManager != null) {
                animationManager.Position = _position;

                int boundWidth = animationManager.animation.FrameWidth;
                int boundHeight = animationManager.animation.FrameHeight;
                Origin = new Vector2(boundWidth / 2f, boundHeight / 2f);

                // Update bounding circle
                bounds.Center = Position;
                bounds.Radius = 0.8f * (boundWidth / 2f * Scale);
            }

            // Deactivate if offscreen
            if (Position.X + (animationManager?.animation.FrameWidth ?? 32) < 0) {
                IsActive = false;
                sparkSystem.Enabled = false;
                Core.Instance.Components.Remove(sparkSystem);
            }
        }


        public override void CheckCollision(Pumpkin player) {
            if (bounds.CollidesWith(player.Bounds)) {
                player.TakeDamage();
            }
        }

        public override void Update(GameTime gameTime) {
            Move(gameTime);

            if (_isAnimated && animationManager != null) {
                animationManager.Update(gameTime);

                // Update bounds each frame like UFO
                int boundWidth = animationManager.animation.FrameWidth;
                int boundHeight = animationManager.animation.FrameHeight;
                Origin = new Vector2(boundWidth / 2f, boundHeight / 2f);
                bounds.Center = Position;
                bounds.Radius = 0.8f * (boundWidth / 2f * Scale);
            }

            SetAnimations();
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance used to draw textures and sprites to the screen.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            if (IsActive) {
                base.Draw(gameTime, spriteBatch);
                //sparkSystem?.Draw(gameTime);
            }
            
        }
    }
}
