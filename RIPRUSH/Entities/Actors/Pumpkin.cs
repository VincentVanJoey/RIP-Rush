using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using RIPRUSH.Entities.CollisionShapes;
using RIPRUSH.Entities.Particles;
using RIPRUSH.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RIPRUSH.Entities.Actors {

    /// <summary>
    /// A class representing the player pumpkin sprite in the game
    /// </summary>
    public class Pumpkin : Sprite {

        // Fields to avoid "magic numbers"
        private const float GRAVITY = 1000f;
        private const float JUMP = 800f;

        public Vector2 velocity;
        public bool onGround;

        private BoundingCircle bounds;

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingCircle Bounds => bounds;

        public SoundEffect _jumpSound;
        public SoundEffect _deathSound;
        public SoundEffect _fallSound;
        public SoundEffect _hurtSound;

        private SoundEffectInstance deathSoundInstance;

        private PumpkinParticleSystem pumpkinParticles;

        public int Health { get; private set; } = 3;
        public const int MaxHealth = 3;

        private bool isDead = false;
        private double deathTimer = 0;
        private bool deathParticlesSpawned = false;

        private bool isInvincible = false;
        private double invincibleTimer = 0;
        private const double InvincibleDuration = 1.0; 
        private double blinkTimer = 0;
        private const double blinkSecondsInterval = 0.05; 


        public Pumpkin(ContentManager content, bool isAnimated, float scale) {
            animations = new Dictionary<string, Animation>();
            Scale = scale;

            // Set _isAnimated based on the constructor parameter
            _isAnimated = isAnimated;

            // Load specific animations if the sprite is animated
            if (_isAnimated) {
                LoadActiveContent(content);

                // Initialize the AnimationManager (it will be set to null if not animated)
                if (animations.Count != 0) {
                    animationManager = new AnimationManager(animations.First().Value);
                }
            }
            else {
                // Load a static texture if not animated
                _texture = content.Load<Texture2D>("Assets/Player/pumsit");
            }

            int boundwidth = _isAnimated ? animationManager.animation.FrameWidth : _texture.Width;
            int boundheight = _isAnimated ? animationManager.animation.FrameHeight : _texture.Height;
            float radius = (boundwidth - 1 * Scale) / 2f;
            Vector2 circleCenter = Position + new Vector2(boundwidth * Scale / 2f, boundheight * Scale / 2f);

            bounds = new BoundingCircle(circleCenter, radius);

        }

        public void CheckPumpkinPlatTouch(List<Platform> _platforms) {
            foreach (var platform in _platforms) {
                if (platform.Bounds.CollidesWith(Bounds)) {
                    Vector2 circleCenter = Bounds.Center;
                    float radius = Bounds.Radius;

                    // Find closest point on the platform rectangle to the circle center
                    float closestX = MathHelper.Clamp(circleCenter.X, platform.Bounds.Left, platform.Bounds.Right);
                    float closestY = MathHelper.Clamp(circleCenter.Y, platform.Bounds.Top, platform.Bounds.Bottom);
                    Vector2 closestPoint = new Vector2(closestX, closestY);

                    // Vector from closest point to circle center
                    Vector2 delta = circleCenter - closestPoint;
                    float distance = delta.Length();

                    if (distance < radius) {
                        float penetration = radius - distance;
                        Vector2 pushDir = (distance == 0) ? Vector2.UnitY : Vector2.Normalize(delta);

                        Position += pushDir * penetration;

                        // Handle velocity depending on push direction
                        if (Math.Abs(pushDir.X) > Math.Abs(pushDir.Y)) {
                            velocity.X = 0; // Horizontal collision (sides)                            
                        }
                        else {
                            velocity.Y = 0; // Vertical collision (top/bottom)

                            if (pushDir.Y < 0) {
                                onGround = true;
                            }
                        }
                        // sync bounds
                        int boundswidth = _isAnimated ? animationManager.animation.FrameWidth : _texture.Width;
                        int boundsheight = _isAnimated ? animationManager.animation.FrameHeight : _texture.Height;
                        bounds.Center = Position + new Vector2(boundswidth * Scale / 2, boundsheight * Scale / 2);
                    }
                }
            }

        }

        public void LoadActiveContent(ContentManager content) {
            // SFX
            _jumpSound = content.Load<SoundEffect>("Assets/Audio/Jump");
            _deathSound = content.Load<SoundEffect>("Assets/Audio/Death");
            _fallSound = content.Load<SoundEffect>("Assets/Audio/Fall");
            _hurtSound = content.Load<SoundEffect>("Assets/Audio/hurt");

            pumpkinParticles = new PumpkinParticleSystem(Core.Instance, 100);
            Core.Instance.Components.Add(pumpkinParticles);

            // Animation data
            Animation idleAnimation = new(content.Load<Texture2D>("Assets/Player/Idle"), 20, true, Color, Origin, Rotation, Scale);
            Animation rollAntimation = new(content.Load<Texture2D>("Assets/Player/Roll"), 15, true, Color, Origin, Rotation, Scale);
            rollAntimation.FrameSpeed = 0.05f;

            animations.Add("Idle", idleAnimation);
            animations.Add("Roll", rollAntimation);
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

            animationManager.Play(animations["Roll"]);

        }

        /// <summary>
        /// The method responsible for moving the pumpkin sprite
        /// </summary>
        /// <param name="gameTime">the time state of the game</param>
        public void Move(GameTime gameTime) {
            if (!_isAnimated) {
                return; // No movement if not animated 
            }
            else {

                // Jump logic
                if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Space) && onGround) {
                    velocity.Y = -JUMP; // Moves the pumpkin "higher" on the level
                    onGround = false;
                    Core.Audio.PlaySoundEffect(_jumpSound);
                }

                // If player releases jump early while still going up, cut the jump short?
                // should make it variable????
                if (Core.Input.Keyboard.WasKeyJustReleased(Keys.Space) && velocity.Y < 0) {
                    velocity.Y *= 0.2f; // TODO - play with value to see what feels best
                }

                // Apply gravity
                velocity.Y += GRAVITY * (float)gameTime.ElapsedGameTime.TotalSeconds;

            }
        }

        public void TakeDamage() {
            if (isInvincible || isDead)
                return;

            Health--;

            if (Health <= 0) {
                // Mark as dead
                isDead = true;

                // Play death sound via instance
                deathSoundInstance = _deathSound.CreateInstance();
                deathSoundInstance.Play();

                // Set timer to wait for sound length
                deathTimer = _deathSound.Duration.TotalSeconds;

                // Spawn particles in next Update (so scene and pumpkin are still active)
                deathParticlesSpawned = false;
            }
            else {
                // Normal hurt logic
                isInvincible = true;
                invincibleTimer = InvincibleDuration;
                blinkTimer = 0;
                Core.Audio.PlaySoundEffect(_hurtSound);
            }
        }

        /// <summary>
        /// Draws the pumpkin sprite
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance used to draw textures and sprites to the screen.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {

            // any specific pumpkin draw logic here in the future?
            // In case I ever let the play customize it in any way
            // Color or hats or something

            if (!isDead) {
                base.Draw(gameTime, spriteBatch);
            }
        }

        /// <summary>
        /// The pumpkin's update logic
        /// </summary>
        /// <param name="gameTime">the time state of the game</param>
        public override void Update(GameTime gameTime) {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (isDead) {
                // Spawn particles once
                if (!deathParticlesSpawned) {
                    pumpkinParticles.Explode(Position + new Vector2(bounds.Radius, bounds.Radius));
                    pumpkinParticles.Explode(Position + new Vector2(bounds.Radius, bounds.Radius));
                    pumpkinParticles.Explode(Position + new Vector2(bounds.Radius, bounds.Radius));
                    deathParticlesSpawned = true;
                }

                // Wait until sound is over to change so we can see the death happen (morbid, but we have to)
                deathTimer -= dt;
                if (deathTimer <= 0 && deathSoundInstance.State == SoundState.Stopped) {
                    Core.Audio.PauseAudio();

                    GameScene game = Core.GetActiveScene() as GameScene;
                    ResultsScene resultsScene = new ResultsScene();
                    resultsScene._finalDistance = game._currentScore;
                    resultsScene._wasNewHighScore = game._newHighScore;

                    Core.ChangeScene(resultsScene);
                }

                return;
            }

            // invincible logic
            if (isInvincible) {
                invincibleTimer -= dt;
                blinkTimer -= dt;

                if (blinkTimer <= 0) {
                    foreach (var animation in animations.Values)
                        animation.Color = (animation.Color == Color.White) ? Color.Red * 0.5f : Color.White;
                    blinkTimer = blinkSecondsInterval;
                }

                if (invincibleTimer <= 0) {
                    isInvincible = false;
                    foreach (var animation in animations.Values)
                        animation.Color = Color.White;
                }
            }

            // standard update stuff
            Move(gameTime);
            SetAnimations();
            Position += velocity * dt;

            // Update bounds
            int boundwidth = _isAnimated ? animationManager.animation.FrameWidth : _texture.Width;
            int boundheight = _isAnimated ? animationManager.animation.FrameHeight : _texture.Height;
            bounds.Center = Position + new Vector2(boundwidth * Scale / 2f, boundheight * Scale / 2f);
            bounds.Radius = 0.8f * ((boundwidth * Scale) / 2f);

            base.Update(gameTime);
        }

    }
}