using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RIPRUSH.Entities.Environment {
    public class GUILabel {
        private SpriteFont _font;
        private Texture2D _pixel;

        public Vector2 Position;
        public string Text;
        public Color TextColor = Color.Gold;
        public Color BackgroundColor = new Color(0, 0, 0, 180);
        public int Padding = 6;
        public float Scale = 1f;

        public GUILabel(SpriteFont font, Texture2D pixel) {
            _font = font;
            _pixel = pixel;
        }

        public void Draw(SpriteBatch spriteBatch, Color outlineColor) {
            if (string.IsNullOrEmpty(Text)) return;

            Vector2 textSize = _font.MeasureString(Text) * Scale;

            Rectangle backgroundRect = new Rectangle(
                (int)Position.X - Padding,
                (int)Position.Y - Padding,
                (int)textSize.X + Padding * 2,
                (int)textSize.Y + Padding * 2
            );

            // Background
            spriteBatch.Draw(_pixel, backgroundRect, BackgroundColor);

            // Outline (1px)
            DrawOutline(spriteBatch, backgroundRect, outlineColor);

            // Text
            spriteBatch.DrawString(
                _font,
                Text,
                Position,
                TextColor,
                0f,
                Vector2.Zero,
                Scale,
                SpriteEffects.None,
                0f
            );
        }

        private void DrawOutline(SpriteBatch spriteBatch, Rectangle r, Color color) {
            // Top
            spriteBatch.Draw(_pixel, new Rectangle(r.X, r.Y, r.Width, 1), color);
            // Bottom
            spriteBatch.Draw(_pixel, new Rectangle(r.X, r.Bottom - 1, r.Width, 1), color);
            // Left
            spriteBatch.Draw(_pixel, new Rectangle(r.X, r.Y, 1, r.Height), color);
            // Right
            spriteBatch.Draw(_pixel, new Rectangle(r.Right - 1, r.Y, 1, r.Height), color);
        }
    }

}
