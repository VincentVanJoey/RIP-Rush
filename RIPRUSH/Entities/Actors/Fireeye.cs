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

        private float _speed = 700f;
        private Vector2 _spawnPosition;

        private BoundingRectangle bounds;
        public BoundingRectangle Bounds => bounds;

        private float _launchDelay = 0.5f;
        private float _timeSinceSpawn = 0f;
        public Vector2 Velocity => new Vector2(-_speed, 0f);

        private SparkParticleSystem sparkSystem;

        public Fireeye(ContentManager content, float scale, Vector2 playerPosition, bool isAnimated = true) {
            Scale = scale;
            _spawnPosition = new Vector2(
                Core.GraphicsDevice.Viewport.Width + 20,
                playerPosition.Y
            );
            Position = _spawnPosition;

            animations = new Dictionary<string, Animation>();
            _isAnimated = isAnimated;

            LoadAnimations(content);

            if (animations.Count != 0) {
                animationManager = new AnimationManager(animations.First().Value);
            }

            int width = animationManager?.animation.FrameWidth ?? 32;  // fallback
            int height = animationManager?.animation.FrameHeight ?? 32;

            Origin = new Vector2(width / 2f, height / 2f);

            bounds = new BoundingRectangle(Position, width * Scale, height * Scale);
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
            float scrollFactor = worldSpeed / 400f;
            float effectiveSpeed = _speed * scrollFactor;

            _timeSinceSpawn += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Only move after launch delay
            if (_timeSinceSpawn >= _launchDelay) {
                _position.X -= effectiveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            // Update animation position AFTER moving
            if (animationManager != null) {
                animationManager.Position = _position;
                UpdateBounds();
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
                Origin = new Vector2(animationManager.animation.FrameWidth / 2f, animationManager.animation.FrameHeight / 2f);
                UpdateBounds();
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

        private void UpdateBounds() {
            if (animationManager == null) return;

            int bw = animationManager.animation.FrameWidth;
            int bh = animationManager.animation.FrameHeight;

            // Shrink slightly
            float paddingFactor = 0.8f;

            bounds.Width = bw * Scale * paddingFactor;
            bounds.Height = bh * Scale * paddingFactor;

            // center
            bounds.X = Position.X - bounds.Width / 2f;
            bounds.Y = Position.Y - bounds.Height / 2f;
        }
    }
}