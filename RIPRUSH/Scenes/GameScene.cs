using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;
using RIPRUSH.Entities;
using RIPRUSH.Entities.Actors;
using System;
using System.Collections.Generic;

namespace RIPRUSH.States {

    /// <summary>
    /// A class representing the actual playable game state of the game
    /// </summary>
    public class GameScene : Scene {

        /// <summary>
        /// The list of the game state's components to be drawn/updated/interacted with
        /// </summary>
        private List<Component> _components;

        /// <summary>
        /// The list of the game state's components to be drawn/updated/interacted with
        /// </summary>
        private List<Platform> _platforms;

        private Pumpkin _player;
        private UFO _ufo;
        private WinFlag _winflag; 

        private SpriteFont timerfont;
        private TimeSpan timer = TimeSpan.Zero;
        private bool timerActive = true;
        private string timerText = "Get to the Flag!\n";
        private string quitdirections = "";

        public override void Initialize() {
            // LoadContent is called during base.Initialize().
            base.Initialize();

            // During the game scene, we want to disable exit on escape. Instead,
            // the escape key will be used to return back to the title screen
            Core.ExitOnEscape = false;

            _player = new Pumpkin(Core.Content, true, 2.0f) { Position = new Vector2(65, 350) };
            _ufo = new UFO(Core.Content, true, 3.0f, new Vector2(390, 250));

            Platform _platform = new Platform(Core.Content, 2.0f, new Vector2(-50, 450));
            Platform _platform2 = new Platform(Core.Content, 1.0f, new Vector2(200, 350));
            Platform _platform3 = new Platform(Core.Content, 1.0f, new Vector2(550, 350));
            Platform _platform4 = new Platform(Core.Content, 1.0f, new Vector2(690, 150));

            _winflag = new WinFlag(Core.Content, 2, new Vector2(700, 100));

            _platform.Color = Color.DarkGreen;
            _platform4.Color = Color.DarkGreen;
            _platform3.moving = true;
            _platform3.move_distance = 100;

            _platforms = new List<Platform>(){
                _platform,
                _platform2,
                _platform3,
                _platform4
            };

            _components = new List<Component>(){
                _winflag,
                _player,
                _platform,
                _platform2,
                _ufo,
                _platform3,
                _platform4
            };

        }

        public override void LoadContent() {
            timerfont = Core.Content.Load<SpriteFont>("Fonts/timer");
        }


        /// <summary>
        /// Checks if the pumpkin is out of the viewport and handles it accordingly.
        /// </summary>
        private void CheckPumpkinOutOfBounds() {
            // Get the viewport dimensions
            var viewport = Core.GraphicsDevice.Viewport;

            // Check if the pumpkin is off the left side of the screen
            if (_player.Position.X < 0 || _player.Position.X > Core.GraphicsDevice.Viewport.Width) {
                _player.Position = new Vector2(0, _player.Position.Y);
                _player.velocity.X = 0;
            }

            // Check if the pumpkin is falling below the bottom of the screen
            if ( _player.Position.Y > viewport.Height) {
                _player.Position = new Vector2(100, 350);
                _player.velocity = Vector2.Zero;
            }
        }

        /// <summary>
        /// Draws the game state
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance used to draw textures and sprites to the screen.</param>
        public override void Draw(GameTime gameTime) {
            Core.GraphicsDevice.Clear(Color.Black);

            Core.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            foreach (var component in _components) {
                component.Draw(gameTime, Core.SpriteBatch);
            }
            Core.SpriteBatch.DrawString(timerfont, $"{timerText}{timer:mm\\:ss}{quitdirections}", new Vector2(20, 20), Color.Gold);
            Core.SpriteBatch.End();
        }

        /// <summary>
        /// The actual playable game state's update logic
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        public override void Update(GameTime gameTime) {
            if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Escape))
                Core.Instance.Exit();

            CheckPumpkinOutOfBounds();
            _player.CheckPumpkinPlatTouch(_platforms);

            if (_ufo.Bounds.CollidesWith(_player.Bounds)) {
                _player.Position = new Vector2(100, 350);
                _player.velocity = Vector2.Zero;
            }

            if (timerActive) {
                timer += gameTime.ElapsedGameTime;

                if (_winflag.Bounds.CollidesWith(_player.Bounds)){
                    timerActive = false;
                    timerText = "You Win!\nTime: ";
                    quitdirections = "\nPress ESC to quit";
                }
            }

            foreach (var component in _components) {
                component.Update(gameTime);
            }
        }

    }
}
