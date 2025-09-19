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
    /// An enumeration representing possible movement directions for the pumpkin sprite.
    /// </summary>
    public enum Direction {
        Idle = 0,
        Down = 1,
        Right = 2,
        Up = 3,
        Left = 4,
    }

    /// <summary>
    /// A class representing the player pumpkin sprite in the game
    /// </summary>
    public class Pumpkin : Sprite {

        // Fields to avoid "magic numbers"
        private const float SPEED = 120f;
        private const float GRAVITY = 200f;
        private const float JUMP = 200f;

        public Vector2 velocity;
        public bool onGround;

        /// <summary>
        /// The direction the pumpkin is moving
        /// </summary>
        public Direction Direction;

        private BoundingCircle bounds;

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingCircle Bounds => bounds;

        public Pumpkin(ContentManager content, bool isAnimated, float scale) {
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

        public void LoadAnimations(ContentManager content) {
            Animation idleAnimation = new(content.Load<Texture2D>("Assets/Player/Idle"), 20, true, Color, Origin, Rotation, Scale);
            Animation rollAntimation = new(content.Load<Texture2D>("Assets/Player/Roll"), 15, true, Color, Origin, Rotation, Scale);

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

            switch (Direction) {
                case Direction.Up:
                    animationManager.Play(animations["Roll"]);
                    break;
                case Direction.Down:
                    animationManager.Play(animations["Roll"]);
                    break;
                case Direction.Left:
                    animationManager.Play(animations["Roll"]);
                    break;
                case Direction.Right:
                    animationManager.Play(animations["Roll"]);
                    break;
                default:
                    animationManager.Play(animations["Idle"]);
                    break;
            }

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
                var keyboardState = Keyboard.GetState();

                if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) {
                    velocity.X = -SPEED;
                    Direction = Direction.Left;
                    animationManager.animation.SpriteEffect = SpriteEffects.FlipHorizontally;
                }
                else if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)) {
                    velocity.X = SPEED;
                    Direction = Direction.Right;
                    animationManager.animation.SpriteEffect = SpriteEffects.None;
                }
                else {
                    Direction = Direction.Idle;
                    velocity.X = 0;
                }
                
                velocity.Y += GRAVITY * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (keyboardState.IsKeyDown(Keys.Space) && onGround) {
                    velocity.Y = -JUMP; // Moves the pumpkin "higher" on the level
                    onGround = false;
                } 

            }
        }

        /// <summary>
        /// Draws the pumpkin sprite
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance used to draw textures and sprites to the screen.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {

            //any specific pumpkin draw logic here in the future?
            // In case I ever let the play customize it in any way
            // Color or hats or something

            base.Draw(gameTime, spriteBatch);
        }

        /// <summary>
        /// The pumpkin's update logic
        /// </summary>
        /// <param name="gameTime">the time state of the game</param>
        public override void Update(GameTime gameTime) {

            if (_isAnimated){
                Move(gameTime);
                SetAnimations();
                Position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                int boundwidth = _isAnimated ? animationManager.animation.FrameWidth : _texture.Width;
                int boundheight = _isAnimated ? animationManager.animation.FrameHeight : _texture.Height;
                bounds.Center = Position + new Vector2(boundwidth * Scale / 2f, boundheight * Scale / 2f);
                bounds.Radius = .8f * ((boundwidth * Scale) / 2f);
            }
            base.Update(gameTime);
        }

    }
}