using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RIPRUSH.Entities;

namespace RIPRUSH {
    /// <summary>
    /// A class representing a clickable button UI component.
    /// </summary>
    public class Button : Component {


        #region fields

        /// <summary>
        /// The current state of the mouse
        /// </summary>
        private MouseState _currentMouse;

        /// <summary>
        /// The current state of the mouse in the previous frame
        /// </summary>
        private MouseState _previousMouse;

        /// <summary>
        /// The font of the text drawn on the button
        /// </summary>
        private SpriteFont _font;

        /// <summary>
        /// The "texture" or image drawn for the button
        /// </summary>
        private Texture2D _texture;

        /// <summary>
        /// Bool to determine if the mouse is hovering over the button
        /// </summary>
        private bool _isHovering;

        #endregion

        #region properties

        public event EventHandler Click;

        /// <summary>
        /// The bool to determined if the button was clicked
        /// </summary>
        public bool Clicked { get; private set; }

        /// <summary>
        /// The color of the font drawn on the button
        /// </summary>
        public Color PenColor { get; set; }

        /// <summary>
        /// The color shifted to when the button is hovered over
        /// </summary>
        public Color HoverColor { get; set; }

        /// <summary>
        /// The position of the button
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The "collision" rectangle of the button that is used to determine
        /// if the mouse is interacting with it
        /// </summary>
        public Rectangle Rectangle {
            get {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }

        /// <summary>
        /// The text displayed on the button 
        /// </summary>
        public string Text { get; set; }

        #endregion

        #region methods

        public Button(Texture2D texture, SpriteFont font, Color fontcolor, Color hovercolor) {
            this._texture = texture;
            this._font = font;
            PenColor = fontcolor;
            HoverColor = hovercolor;
        }

        /// <summary>
        /// Draws the button
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance used to draw textures and sprites to the screen.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            var color = Color.White;
            if (_isHovering) {
                color = HoverColor;
            }
            spriteBatch.Draw(_texture, Rectangle, color);
            
            if(!string.IsNullOrEmpty(Text)) {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);
                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColor);
            }
        }

        /// <summary>
        /// A Button's update logic
        /// </summary>
        /// <param name="gameTime">the time state of the game</param>
        public override void Update(GameTime gameTime) {
            
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);
            _isHovering = false;
            
            if (mouseRectangle.Intersects(Rectangle)) {
                _isHovering = true;
                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed) {
                    Click?.Invoke(this, new EventArgs());
                }
            }

        }

        #endregion
    }
}
