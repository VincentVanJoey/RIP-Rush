using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RIPRUSH.States {
    public class GameState : State {
        public GameState(ContentManager content, Game1 game, GraphicsDevice graphicsDevice) : base(content, game, graphicsDevice) {
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {}
        public override void PostUpdate(GameTime gameTime) {}
        public override void Update(GameTime gameTime) {}
    }
}
