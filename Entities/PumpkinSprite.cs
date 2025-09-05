using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RIPRUSH.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RIPRUSH.Entities {
    public enum Direction {
        Idle = 0,
        Down = 1,
        Right = 2,
        Up = 3,
        Left = 4,
    }
    public class PumpkinSprite: Component {
        #region Fields

        public AnimationManager animationManager;

        public Dictionary<string, Animation> animations;

        protected Vector2 position;

        protected Texture2D texture;

        public Direction Direction;

        #endregion

        #region Properties

        public Vector2 Position {
            get { return position; }
            set {
                position = value;

                if (animationManager != null)
                    animationManager.Position = position;
            }
        }

        #endregion

        #region Methods

        public PumpkinSprite(Dictionary<string, Animation> animations) {
            this.animations = animations;
            animationManager = new AnimationManager(animations.First().Value);
        }

        public PumpkinSprite(Texture2D texture) {
            this.texture = texture;
        }

        public virtual void Move(GameTime gameTime) {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) {
                Position += new Vector2(-1, 0) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Direction = Direction.Left;
                animationManager.animation.spriteEffect = SpriteEffects.FlipHorizontally;
            }
            else if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)) {
                Position += new Vector2(1, 0) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Direction = Direction.Right;
                animationManager.animation.spriteEffect = SpriteEffects.None;
            }
            else Direction = Direction.Idle;

        }

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

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            if (texture != null)
                spriteBatch.Draw(texture, Position, Color.White);
            else if (animationManager != null)
                animationManager.Draw(spriteBatch);
            else throw new Exception("This ain't right..!");
        }

        public override void Update(GameTime gameTime) {
            Move(gameTime);
            SetAnimations();

            animationManager.Update(gameTime);

        }

        #endregion
    }
}