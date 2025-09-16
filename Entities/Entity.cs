using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace RIPRUSH.Entities {

    public class Entity : Component {
        
        #region Fields

        protected Vector2 _position;

        protected Texture2D _texture;

        private bool _isAnimated = false;

        public AnimationManager animationManager;

        public Dictionary<string, Animation> animations;

        #endregion

        #region Properties

        public Vector2 Position {
            get { return _position; }
            set {
                _position = value;

                if (animationManager != null)
                    animationManager.Position = _position;
            }
        }

        /// <summary>
        /// The color used to tint. Default is Color.White (no tint).
        /// </summary>
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// The rotation in radians.
        /// </summary>
        public float Rotation { get; set; } = 0f;

        /// <summary>
        /// The origin of rotation
        /// </summary>
        public Vector2 Origin { get; set; } = Vector2.Zero;

        /// <summary>
        /// The factor by which we scale the entity
        /// </summary>
        public float Scale { get; set; } = 1f;


        /// <summary>
        /// The sprite effects to apply (e.g., flip horizontally or vertically).
        /// </summary>
        public SpriteEffects SpriteEffect { get; set; } = SpriteEffects.None;

        /// <summary>
        /// The layer depth, used for rendering order (0.0f = front, 1.0f = back).
        /// </summary>
        public float LayerDepth { get; set; } = 0f;

        #endregion

        #region Methods

        /// <summary>
        /// Draws the entity
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance used to draw textures and sprites to the screen.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {

            if (!_isAnimated) { //If the sprite is a static image
                spriteBatch.Draw(_texture, Position, new Rectangle(0, 0, _texture.Width, _texture.Height), Color, Rotation, Origin, Scale, SpriteEffect, LayerDepth);
            }
            else if (_isAnimated) { //else if it has an animations, draw those
                animationManager.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// The update logic for an entity
        /// </summary>
        /// <param name="gameTime">the time state of the game</param>
        public override void Update(GameTime gameTime) {
            animationManager.Update(gameTime);
        }

        #endregion
    }
}