using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RIPRUSH.Entities;
using RIPRUSH.Entities.Actors;
using System.Collections.Generic;
using System.Diagnostics;

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
        /// The list of the game state's components to be drawn/updated/interacted with
        /// </summary>
        private List<Platform> _platforms;

        private Pumpkin _player;

        /// <summary>
        /// The constructor for the game state
        /// </summary>
        /// <param name="content">The Content state's contentmanager</param>
        /// <param name="game">The actual game base object</param>
        /// <param name="graphicsDevice">The graphics device that handles the rendering</param>
        public GameState(ContentManager content, Game1 game, GraphicsDevice graphicsDevice) : base(content, game, graphicsDevice) {

            game.backgroundColor = Color.MediumPurple;

            _player = new Pumpkin(content, true, 2.0f) {Position = new Vector2(100, 350) };

            Platform _platform = new Platform(content, 2.0f) { Position = new Vector2(-50, 450) };
            Platform _platform2 = new Platform(content, 2.0f) { Position = new Vector2(300, 250) };

            _components = new List<Component>(){
                _player,
                _platform,
                _platform2
            };

            _platforms = new List<Platform>(){
                _platform,
                _platform2
            };


        }

        /// <summary>
        /// Checks if the pumpkin is out of the viewport and handles it accordingly.
        /// </summary>
        private void CheckPumpkinOutOfBounds() {
            // Get the viewport dimensions
            var viewport = _graphicsDevice.Viewport;

            // Check if the pumpkin is off the left side of the screen
            if (_player.Position.X < 0) {
                _player.Position = new Vector2(0, _player.Position.Y);
                _player._velocity = Vector2.Zero;
            }

            // Check if the pumpkin is falling below the bottom of the screen
            if ( _player.Position.Y > viewport.Height) {
                _player.Position = new Vector2(100, 350);
                _player._velocity = Vector2.Zero;
            }
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

            CheckPumpkinOutOfBounds();

            foreach (var platform in _platforms) {
                if (platform.Bounds.CollidesWith(_player.Bounds)) {
                    Vector2 newPos = _player.Position;

                    if (_player._velocity.Y > 0) { // Falling down
                        newPos.Y = platform.Bounds.Top - _player.Bounds.Radius * 2; // Adjust to not get stuck
                        _player._onGround = true;
                        _player._velocity.Y = 0;
                        _player.Position = newPos;
                    }
                    else if (_player._velocity.Y < 0) { // Going up (jumping)
                        newPos.Y = platform.Bounds.Bottom; // Adjust to not get stuck
                        _player._velocity.Y = 0;
                        _player.Position = newPos;
                    }
                }
            }

        }

        /// <summary>
        /// post update logic for the game state, if any
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        public override void PostUpdate(GameTime gameTime) { }
    }
}
