using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RIPRUSH.Entities;

namespace RIPRUSH {
    public class Button : Component {


        #region fields

        private MouseState currentMouse;
        private MouseState previousMouse;
        private SpriteFont font;
        private Texture2D texture;
        private bool isHovering;

        #endregion

        #region properties

        public event EventHandler Click;

        public bool Clicked { get; private set; }

        public Color PenColor { get; set; }

        public Color HoverColor { get; set; }

        public Vector2 Position { get; set; }

        public Rectangle Rectangle {
            get {
                return new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
            }
        }

        public string Text { get; set; }

        #endregion

        #region methods

        public Button(Texture2D texture, SpriteFont font, Color fontcolor, Color hovercolor) {
            this.texture = texture;
            this.font = font;
            PenColor = fontcolor;
            HoverColor = hovercolor;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            var color = Color.White;
            if (isHovering) {
                color = HoverColor;
            }
            spriteBatch.Draw(texture, Rectangle, color);
            
            if(!string.IsNullOrEmpty(Text)) {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (font.MeasureString(Text).Y / 2);
                spriteBatch.DrawString(font, Text, new Vector2(x, y), PenColor);
            }
        }

        public override void Update(GameTime gameTime) {
            
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);
            isHovering = false;
            
            if (mouseRectangle.Intersects(Rectangle)) {
                isHovering = true;
                if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed) {
                    Click?.Invoke(this, new EventArgs());
                }
            }

        }

        #endregion
    }
}
