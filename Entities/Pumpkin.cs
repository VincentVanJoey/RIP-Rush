using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
    public class Pumpkin : Component {
        #region Fields

        /// <summary>
        /// The pumpkin's position on screen
        /// </summary>
        protected Vector2 _position;

        /// <summary>
        /// The pumpkin's sprite texture
        /// </summary>
        /// <remarks>This protected field holds a reference to a <see cref="Texture2D"/> object, which can
        /// be used  by derived classes to define or manipulate the visual appearance of the object.</remarks>
        protected Texture2D _texture;

        /// <summary>
        /// The animation manager that handles the pumpkin's animations
        /// </summary>
        public AnimationManager animationManager;

        /// <summary>
        /// The dictionary that holds all the animations for the pumpkin
        /// </summary>
        public Dictionary<string, Animation> animations;

        /// <summary>
        /// The direction the pumpkin is moving
        /// </summary>
        public Direction Direction;

        #endregion

        #region Properties

        /// <summary>
        /// Where the sprite is located on screen
        /// </summary>
        public Vector2 Position {
            get { return _position; }
            set {
                _position = value;

                if (animationManager != null)
                    animationManager.Position = _position;
            }
        }

        #endregion

        #region Methods

        public Pumpkin(Dictionary<string, Animation> animations) {
            this.animations = animations;
            animationManager = new AnimationManager(animations.First().Value);
        }

        public Pumpkin(Texture2D texture) {
            this._texture = texture;
        }

        public void LoadContent(ContentManager content) {
            if (_texture != null) { //If the sprite is a static image
                _texture = content.Load<Texture2D>("32x32-bat-sprite");
                animationManager = null;
            }
            else if (animationManager != null) { //else if it has an animations, draw those
                this.animations = new Dictionary<string, Animation>() {
                    { "Roll", new Animation(content.Load<Texture2D>("Player/Roll"), 15, true, Color.White, Vector2.Zero, 0, 2) },
                    { "Idle", new Animation(content.Load<Texture2D>("Player/Idle"), 20, true, Color.White, Vector2.Zero, 0, 2) },
                };
                animationManager = new AnimationManager(animations.First().Value);
            }
        }

        /// <summary>
        /// The method responsible for moving the pumpkin sprite
        /// </summary>
        /// <param name="gameTime">the time state of the game</param>
        public virtual void Move(GameTime gameTime) {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) {
                Position += new Vector2(-1, 0) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Direction = Direction.Left;
                animationManager.animation.SpriteEffect = SpriteEffects.FlipHorizontally;
            }
            else if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)) {
                Position += new Vector2(1, 0) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Direction = Direction.Right;
                animationManager.animation.SpriteEffect = SpriteEffects.None;
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

            if (_texture != null) { //If the sprite is a static image
                spriteBatch.Draw(_texture, Position, Color.White);

            }
            else if (animationManager != null) { //else if it has an animations, draw those
                animationManager.Draw(spriteBatch);

            }
            else throw new Exception("This ain't right..!"); //else throw an exception
        }

        /// <summary>
        /// The pumpkin's update logic
        /// </summary>
        /// <param name="gameTime">the time state of the game</param>
        public override void Update(GameTime gameTime) {
            Move(gameTime);
            SetAnimations();

            animationManager.Update(gameTime);
        }

        #endregion
    }
}