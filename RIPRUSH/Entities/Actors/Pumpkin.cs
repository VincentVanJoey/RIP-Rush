using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Input;
using RIPRUSH.Entities.CollisionShapes;
using RIPRUSH.Entities.Environment;
using RIPRUSH.Entities.Particles;
using RIPRUSH.Scenes;

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

        private bool isHurtInvincible = false;
        private double hurtInvincibleTimer = 0;
        private const double HurtInvincibleDuration = 1.0; 
        private double blinkTimer = 0;
        private const double HurtBlinkInterval = 0.05;

        private bool isPowerInvincible = false;
        private double powerInvincibleTimer = 0;
        private const double PowerInvincibleDuration = 8.0;
        private const double PowerBlinkInterval = 0.1;
        private bool showPowerFlash = false;

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
                if (platform.Bounds.CollidesWith(Bounds)) {   // Bounds = pumpkin circle
                    Vector2 circleCenter = Bounds.Center;
                    float radius = Bounds.Radius;

                    // Closest point on RectData
                    float closestX = MathHelper.Clamp(circleCenter.X, platform.Bounds.X, platform.Bounds.X + platform.Bounds.Width);
                    float closestY = MathHelper.Clamp(circleCenter.Y, platform.Bounds.Y, platform.Bounds.Y + platform.Bounds.Height);
                    Vector2 closestPoint = new Vector2(closestX, closestY);

                    Vector2 delta = circleCenter - closestPoint;
                    float distance = delta.Length();

                    if (distance < radius) {
                        float penetration = radius - distance;
                        Vector2 pushDir = (distance == 0) ? Vector2.UnitY : Vector2.Normalize(delta);

                        Position += pushDir * penetration;

                        if (Math.Abs(pushDir.X) > Math.Abs(pushDir.Y)) {
                            velocity.X = 0;
                        }
                        else {
                            velocity.Y = 0;
                            if (pushDir.Y < 0) onGround = true;
                        }

                        int boundswidth = _isAnimated ? animationManager.animation.FrameWidth : _texture.Width;
                        int boundsheight = _isAnimated ? animationManager.animation.FrameHeight : _texture.Height;
                        bounds.Center = Position + new Vector2(boundswidth * Scale / 2, boundsheight * Scale / 2);
                    }
                }
            }
        }

        public void CheckPickupCollision(IEnumerable<CandyPickup> pickups) {
            foreach (var pickup in pickups) {
                if (!pickup.Collected && Bounds.Intersects(pickup.Bounds)) {
                    pickup.Collected = true;

                    Core.Audio.PlaySoundEffect(pickup.PickupSound);

                    switch (pickup.Type) {
                        case CandyType.Lollipop: 
                            // heal 1 health
                            Health = Math.Min(MaxHealth, Health + 1);
                            break;
                        case CandyType.Chocobar:
                            var scene = Core.GetActiveScene() as GameScene;
                            onGround = true; // gives mid-air jump instead of speed boost? get feedback

                            //scene?.worldManager.ApplySpeedBoost(); -- seems detrimental, not sure
                            break;
                        case CandyType.ZapCandy:
                            // Activate special invincibility for a fixed time
                            isPowerInvincible = true;
                            powerInvincibleTimer = PowerInvincibleDuration;
                            blinkTimer = 0;
                            foreach (var animation in animations.Values)
                                animation.Color = Color.Yellow;
                            break;
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
                if ((Core.Input.Keyboard.WasKeyJustPressed(Keys.Space) || Core.Input.Mouse.WasButtonJustPressed(MouseButton.Left)) && onGround) {
                    velocity.Y = -JUMP; // Moves the pumpkin "higher" on the level
                    onGround = false;
                    Core.Audio.PlaySoundEffect(_jumpSound);
                }

                // If player releases jump early while still going up, cut the jump short?
                // should make it variable????
                if ((Core.Input.Keyboard.WasKeyJustReleased(Keys.Space) || Core.Input.Mouse.WasButtonJustReleased(MouseButton.Left)) && velocity.Y < 0) {
                    velocity.Y *= 0.2f; // TODO - play with value to see what feels best
                }

                // Apply gravity
                velocity.Y += GRAVITY * (float)gameTime.ElapsedGameTime.TotalSeconds;

            }
        }

        public void TakeDamage() {
            if (isHurtInvincible || isPowerInvincible || isDead)
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
                isHurtInvincible = true;
                hurtInvincibleTimer = HurtInvincibleDuration;
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

                // ZapCandy invincibility flash (additive bright yellow)
                if (isPowerInvincible && showPowerFlash) {
                    spriteBatch.End(); // End current batch to change blend mode
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

                    var anim = animationManager.animation;
                    spriteBatch.Draw(
                        anim.Texture,
                        animationManager.Position,
                        new Rectangle(anim.CurrentFrame * anim.FrameWidth, 0, anim.FrameWidth, anim.FrameHeight),
                        Color.Yellow,
                        anim.Rotation,
                        anim.Origin,
                        anim.Scale,
                        anim.SpriteEffect,
                        anim.LayerDepth
                    );

                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                }

            }
            else{
                pumpkinParticles?.Draw(gameTime);
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

                    Core.Instance.Components.Remove(pumpkinParticles);
                    pumpkinParticles.Dispose();


                    Core.ChangeScene(resultsScene);
                }

                return;
            }

            // Damange invincibility logic
            if (isHurtInvincible) {
                hurtInvincibleTimer -= dt;
                blinkTimer -= dt;

                if (blinkTimer <= 0) {
                    foreach (var animation in animations.Values)
                        animation.Color = (animation.Color == Color.White) ? Color.Red * 0.5f : Color.White;
                    blinkTimer = HurtBlinkInterval;
                }

                if (hurtInvincibleTimer <= 0) {
                    isHurtInvincible = false;
                    foreach (var animation in animations.Values)
                        animation.Color = Color.White;
                }
            }

            // ZapCandy invincibility
            if (isPowerInvincible) {
                powerInvincibleTimer -= dt;

                // Adjust blink interval to speed up near the end
                double timeFraction = powerInvincibleTimer / PowerInvincibleDuration;
                double dynamicBlink = MathHelper.Lerp(0.05f, (float)PowerBlinkInterval, (float)timeFraction);

                blinkTimer -= dt;
                if (blinkTimer <= 0) {
                    showPowerFlash = !showPowerFlash; // toggle visibility of the bright flash
                    blinkTimer = dynamicBlink;
                }

                if (powerInvincibleTimer <= 0) {
                    isPowerInvincible = false;
                    showPowerFlash = false;
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