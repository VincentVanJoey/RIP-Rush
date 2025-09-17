using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RIPRUSH.Entities.CollisionShapes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RIPRUSH.Entities {

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
    public class Pumpkin : Entity {
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
                if (animations.Any()) {
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
            Vector2 bound_center = Position + new Vector2((boundswidth * Scale) / 2, (boundsheight * Scale) / 2);

            // Set bounds radius
            bounds = new BoundingCircle(bound_center, (boundswidth * Scale) / 2);

        }

        public void LoadAnimations(ContentManager content) {
            Animation idleAnimation = new(content.Load<Texture2D>("Player/Idle"), 20, true, Color, Origin, Rotation, Scale);
            Animation rollAntimation = new(content.Load<Texture2D>("Player/Roll"), 15, true, Color, Origin, Rotation, Scale);

            animations.Add("Idle", idleAnimation);
            animations.Add("Roll", rollAntimation);
        }

        /// <summary>
        /// The method responsible for moving the pumpkin sprite
        /// </summary>
        /// <param name="gameTime">the time state of the game</param>
        public virtual void Move(GameTime gameTime) {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) {

                if (_isAnimated) {
                    Position += new Vector2(-1, 0) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Direction = Direction.Left;
                    animationManager.animation.SpriteEffect = SpriteEffects.FlipHorizontally;
                }
            }
            else if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)) {

                if (_isAnimated) {
                    Position += new Vector2(1, 0) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Direction = Direction.Right;
                    animationManager.animation.SpriteEffect = SpriteEffects.None;
                }
            }
            else Direction = Direction.Idle;

        }

        /// <summary>
        /// The method responsible for determining which animation to play
        /// </summary>
        protected virtual void SetAnimations() {
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
            var rect = new Rectangle((int)(bounds.Center.X - bounds.Radius), (int)(bounds.Center.Y - bounds.Radius), 2 * (int)(bounds.Radius), 2 * (int)(bounds.Radius));
            spriteBatch.Draw(collisiontestshape, rect, Color.DarkRed);

        }

        /// <summary>
        /// The pumpkin's update logic
        /// </summary>
        /// <param name="gameTime">the time state of the game</param>
        public override void Update(GameTime gameTime) {

            Move(gameTime);

            SetAnimations();

            // Set bounds center position
            int boundswidth = _isAnimated ? animationManager.animation.FrameWidth : _texture.Width;
            int boundsheight = _isAnimated ? animationManager.animation.FrameHeight : _texture.Height;
            bounds.Center = Position + new Vector2((boundswidth * Scale) / 2, (boundsheight * Scale) / 2);

            base.Update(gameTime);
        }

    }
}