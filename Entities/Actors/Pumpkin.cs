using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RIPRUSH.Entities.CollisionShapes;
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
        private const float SPEED = 100f;
        private const float GRAVITY = 200f;
        private const float JUMP = 200f;
        public Vector2 _velocity;
        public bool _onGround;

        /// <summary>
        /// The direction the pumpkin is moving
        /// </summary>
        public Direction Direction;

        private BoundingCircle bounds;

        private Texture2D collisiontestshape;

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingCircle Bounds => bounds;

        public Pumpkin(ContentManager content, bool isAnimated, float scale) {
            animations = new Dictionary<string, Animation>();
            Scale = scale;

            // Set _isAnimated based on the constructor parameter
            _isAnimated = isAnimated;
            collisiontestshape = content.Load<Texture2D>("Assets/test_shape");

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
                _texture = content.Load<Texture2D>("Assets/test_shape");
            }

            // Calculate the size of the bounding circle depending on whether it's animated or not
            int boundswidth = _isAnimated ? animationManager.animation.FrameWidth : _texture.Width;
            int boundsheight = _isAnimated ? animationManager.animation.FrameHeight : _texture.Height;

            // Set bounds center position
            Vector2 bound_center = Position + new Vector2(boundswidth * Scale / 2, boundsheight * Scale / 2);

            // Set bounds radius
            bounds = new BoundingCircle(bound_center, boundswidth * Scale / 2);

        }

        public void LoadAnimations(ContentManager content) {
            Animation idleAnimation = new(content.Load<Texture2D>("Player/Idle"), 20, true, Color, Origin, Rotation, Scale);
            Animation rollAntimation = new(content.Load<Texture2D>("Player/Roll"), 15, true, Color, Origin, Rotation, Scale);

            animations.Add("Idle", idleAnimation);
            animations.Add("Roll", rollAntimation);
        }

        /// <summary>
        /// The method responsible for determining which animation to play
        /// </summary>
        public virtual void SetAnimations() {
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
        public virtual void Move(GameTime gameTime) {
            if (!_isAnimated) {
                return; // No movement if not animated 
            }
            else {
                var keyboardState = Keyboard.GetState();

                if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) {
                    _velocity.X = -SPEED;
                    Direction = Direction.Left;
                    animationManager.animation.SpriteEffect = SpriteEffects.FlipHorizontally;
                }
                else if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)) {
                    _velocity.X = SPEED;
                    Direction = Direction.Right;
                    animationManager.animation.SpriteEffect = SpriteEffects.None;
                }
                else {
                    Direction = Direction.Idle;
                    _velocity.X = 0;
                }
                
                _velocity.Y += GRAVITY * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (keyboardState.IsKeyDown(Keys.Space) && _onGround) {
                    _velocity.Y = -JUMP; // Moves the pumpkin "higher" on the level
                    _onGround = false;
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

            //To show collision bounds --DEBUG ONLY
            var rect = new Rectangle((int)(bounds.Center.X - bounds.Radius), (int)(bounds.Center.Y - bounds.Radius), 2 * (int)bounds.Radius, 2 * (int)bounds.Radius);
            spriteBatch.Draw(collisiontestshape, rect, Color.DarkRed);

        }

        /// <summary>
        /// The pumpkin's update logic
        /// </summary>
        /// <param name="gameTime">the time state of the game</param>
        public override void Update(GameTime gameTime) {

            Move(gameTime);
            SetAnimations();
            Position += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Set bounds center position
            int boundswidth = _isAnimated ? animationManager.animation.FrameWidth : _texture.Width;
            int boundsheight = _isAnimated ? animationManager.animation.FrameHeight : _texture.Height;
            bounds.Center = Position + new Vector2(boundswidth * Scale / 2, boundsheight * Scale / 2);

            base.Update(gameTime);
        }

    }
}