using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RIPRUSH.Entities;
using RIPRUSH.Sprites;
using System.Collections.Generic;

namespace RIPRUSH.States {
    public class GameState : State {

        private List<Component> components;

        private PumpkinSprite player;

        public GameState(ContentManager content, Game1 game, GraphicsDevice graphicsDevice) : base(content, game, graphicsDevice) {

            player = new PumpkinSprite(new Dictionary<string, Animation>() {
                { "Roll", new Animation(content.Load<Texture2D>("Player/Roll"), 15, true, Color.White, Vector2.Zero, 0, 6) },
                { "Idle", new Animation(content.Load<Texture2D>("Player/Idle"), 20, true, Color.White, Vector2.Zero, 0, 6) },
            });
            player.Position = new Vector2(300, 100);

            components = new List<Component>(){
                player
            };


        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            foreach (var component in components) {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }
        public override void PostUpdate(GameTime gameTime) {}
        public override void Update(GameTime gameTime) {
            foreach (var component in components) {
                component.Update(gameTime);
            }
        }
    }
}
