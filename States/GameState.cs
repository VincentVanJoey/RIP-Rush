using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RIPRUSH.Entities;
using System.Collections.Generic;

namespace RIPRUSH.States {

    /// <summary>
    /// A class representing the actual playable game state of the game
    /// </summary>
    public class GameState : State {

        /// <summary>
        /// The list of the game state's components to be drawn/updated/interacted with
        /// </summary>
        private List<Component> _components;

        /// <summary>
        /// the player's sprite
        /// </summary>
        private Pumpkin _player;

        /// <summary>
        /// The constructor for the game state
        /// </summary>
        /// <param name="content">The Content state's contentmanager</param>
        /// <param name="game">The actual game base object</param>
        /// <param name="graphicsDevice">The graphics device that handles the rendering</param>
        public GameState(ContentManager content, Game1 game, GraphicsDevice graphicsDevice) : base(content, game, graphicsDevice) {

            _player = new Pumpkin(content, true);
            _player.Scale = 2.0f;
            _player.Position = new Vector2(100, 350);

            _components = new List<Component>(){
                _player
            };

        }

        /// <summary>
        /// Draws the game state
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance used to draw textures and sprites to the screen.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            foreach (var component in _components) {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }



        /// <summary>
        /// The actual playable game state's update logic
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        public override void Update(GameTime gameTime) {
            foreach (var component in _components) {
                component.Update(gameTime);
            }
        }

        /// <summary>
        /// post update logic for the game state, if any
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        public override void PostUpdate(GameTime gameTime) { }
    }
}
