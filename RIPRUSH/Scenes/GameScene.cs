using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGameGum;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;
using RIPRUSH.Components.Joelements;
using RIPRUSH.Entities;
using RIPRUSH.Entities.Actors;
using RIPRUSH.Screens;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RIPRUSH.Scenes {

    /// <summary>
    /// A class representing the actual playable game state of the game
    /// </summary>
    public class GameScene : Scene {

        /// <summary>
        /// The list of the game state's components to be drawn/updated/interacted with
        /// </summary>
        private List<Component> _components;

        private List<UFO> _ufos;

        private Song GameSong;
        private SoundEffect WinSound;

        private Pumpkin _player;
        //private WinFlag _winflag;

        private SpriteFont timerfont;
        private TimeSpan timer = TimeSpan.Zero;
        private bool timerActive = true;
        private string timerText = "Time: ";
        private string quitdirections = "";

        private PauseMenu _pauseMenu;

        private WorldManager worldManager;

        public override void Initialize() {

            #region -- Boilerplate code --
            // LoadContent called during base.Initialize().
            base.Initialize();

            Core.Audio.PauseAudio();
            Core.Audio.PlaySong(GameSong);

            // During the game scene, we want to disable exit on escape. Instead,
            // the escape key will be used to return back to the title screen
            Core.ExitOnEscape = false;
            #endregion

            worldManager = new WorldManager(baseY: 450f, Core.GraphicsDevice);
            worldManager.Initialize(Content, chunkCount: 6);

            _player = new Pumpkin(Core.Content, true, 1.75f) { Position = new Vector2(65, 350) };
            UFO _ufo = new UFO(Core.Content, true, 3.0f, new Vector2(390, 250));

            _components = new List<Component>();
            _ufos = new List<UFO>();
            
            _components.Add(_player);

            #region -- Pause Menu Logic --

            GumService.Default.Root.Children.Clear();
            _pauseMenu = new PauseMenu();
            _pauseMenu.AddToRoot();
            _pauseMenu.IsVisible = false;

            _pauseMenu.PauseResumeButton.Click += (s, e) => {
                _pauseMenu.IsVisible = false;
                timerActive = true;
            };

            _pauseMenu.PauseTitleButton.Click += (s, e) => {
                Core.ChangeScene(new MainMenuScene());
                Core.Audio.PauseAudio();
            };

            #endregion
        }

        public override void LoadContent() {
            GameSong = Core.Content.Load<Song>("Assets/Audio/Music/GameMusic");
            WinSound = Core.Content.Load<SoundEffect>("Assets/Audio/Win");
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
            if (_player.Position.Y > viewport.Height) {
                _player.TakeDamage();
                _player.Position = new Vector2(100, 350);
                _player.velocity = Vector2.Zero;
                _player.onGround = false;
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
            worldManager.Draw(gameTime, Core.SpriteBatch);

            Core.SpriteBatch.DrawString(timerfont, $"HP: {_player.Health}/{3}", new Vector2(600, 20), Color.Red);

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

            #region -- Pausing logic --

            if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Escape)) {
                if (_pauseMenu.IsVisible) {
                    _pauseMenu.IsVisible = false;  // hide panel
                    timerActive = true;             // resume gameplay
                }
                else {
                    _pauseMenu.IsVisible = true;         // show panel
                    timerActive = false;            // pause gameplay
                }
            }

            if (_pauseMenu.IsVisible) {
                return;
            }
            #endregion

            worldManager.Update(gameTime);


            if (_player.Health <= 0) {
                Core.ChangeScene(new MainMenuScene());
            }

            // Keeps pumpkin on left side of screen
            _player.Position = new Vector2(50, _player.Position.Y);
            CheckPumpkinOutOfBounds();
            _player.CheckPumpkinPlatTouch(worldManager.GetActivePlatforms());

            foreach (var ufo in _ufos) {
                if (ufo.TouchingPumpkin(_player)) {
                    Core.Audio.PlaySoundEffect(_player._deathSound);
                }
            }
                
            if (timerActive) {
                timer += gameTime.ElapsedGameTime;

                //if (_winflag.Bounds.CollidesWith(_player.Bounds)) {
                //    timerActive = false;
                //    Core.Audio.PauseAudio();
                //    Core.Audio.PlaySoundEffect(WinSound);
                //    timerText = "You Win!\nTime: ";
                //    quitdirections = "\nPress ESC to Pause & Quit\n (or close game window)";
                //}
            }

            foreach (var component in _components) {
                component.Update(gameTime);
            }

        }

    }
}