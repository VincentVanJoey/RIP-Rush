using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGameGum;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;
using RIPRUSH.Components.Joelements;
using RIPRUSH.Entities;
using RIPRUSH.Entities.Actors;
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

        private List<Enemy> _enemies;
        private float _enemySpawnTimer = 0f;
        private float _enemySpawnInterval = 5f; // initial interval
        private Random _rng = new Random();
        private int _maxActiveEnemies = 5; // starting cap

        private Song GameSong;
        private SoundEffect WinSound;

        private Pumpkin _player;
        //private WinFlag _winflag;

        private SpriteFont timerfont;
        public TimeSpan timer = TimeSpan.Zero;
        public bool timerActive = true;
        private string timerText = "Time: ";
        private string quitdirections = "";

        private PauseMenu _pauseMenu;

        private WorldManager worldManager;

        private Texture2D _midground;
        private Texture2D _background;
        private Color worldColor;

        private bool _shaking;
        private float _shakeTime;


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

            worldColor = Color.OrangeRed;

            worldManager = new WorldManager(baseY: 450f, Core.GraphicsDevice);
            worldManager.Initialize(Content, chunkCount: 6);

            _player = new Pumpkin(Core.Content, true, 1.75f) { Position = new Vector2(65, 350) };

            _components = new List<Component>();
            _enemies = new List<Enemy>();
            
            _components.Add(_player);

            #region -- Pause Menu Logic --

            GumService.Default.Root.Children.Clear();
            _pauseMenu = new PauseMenu();
            _pauseMenu.AddToRoot();
            _pauseMenu.IsVisible = false;

            #endregion
        }

        public override void LoadContent() {
            GameSong = Core.Content.Load<Song>("Assets/Audio/Music/GameMusic");
            WinSound = Core.Content.Load<SoundEffect>("Assets/Audio/Win");
            timerfont = Core.Content.Load<SpriteFont>("Fonts/timer");
            _background = Content.Load<Texture2D>("Assets/background");
            _midground = Content.Load<Texture2D>("Assets/midground");
        }

        private void SpawnRandomEnemy() {
            // we only have one enemy rn, but hypotetically we could rng switch many
            if (_enemies.Count >= _maxActiveEnemies) return;

            float yPos = _rng.Next(150, 270);
            float xPos = Core.GraphicsDevice.Viewport.Width + 50;

            Enemy enemy = new UFO(Core.Content, true, 3f, new Vector2(xPos, yPos));

            _enemies.Add(enemy);
            _components.Add(enemy);
        }

        private void UpdateEnemySpawnInterval() {
            // Gradually decrease spawn interval from 5s -> 1s over 2 minutes
            float difficultyFactor = MathHelper.Clamp((float)timer.TotalSeconds / 120f, 0, 1);
            _enemySpawnInterval = MathHelper.Lerp(5f, 1f, difficultyFactor);

            // Gradually increase max active enemies
            _maxActiveEnemies = 5 + (int)(difficultyFactor * 10); // 5->15 enemies max
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

            float offsetX = worldManager.TotalScrollX;

            #region -- Screen Shake Effect --
            Matrix shakeTransform = Matrix.Identity;
            if (_shaking) {
                _shakeTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                Matrix shakeTranslation = Matrix.CreateTranslation(10 * MathF.Sin(_shakeTime), 10 * MathF.Cos(_shakeTime), 0);
                shakeTransform = shakeTranslation;
                if (_shakeTime > 3000) _shaking = false;
            }
            #endregion

            // Although I don't want it, I have to use this long begin to apply the matrix??? TODO:find work around
            Core.SpriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullNone,
                null,
                shakeTransform
            );

            #region -- Parallax Scrolling Background --
            // === 1. Background (does not move) ===
            Core.SpriteBatch.Draw(_background, Vector2.Zero, worldColor);

            // === 2. Midground (repeats) ===
            float scrollSpeed = 0.5f; // slower than player movement
            float scrollPosition = -offsetX * scrollSpeed; // parallax movement

            // Wrap using modulo
            int textureWidth = _midground.Width;
            float modX = scrollPosition % textureWidth;
            if (modX > 0)
                modX -= textureWidth; // ensure seamless leftward wrapping

            // Draw two copies side-by-side to cover screen width
            Core.SpriteBatch.Draw(_midground, new Vector2(modX, 50), worldColor);
            Core.SpriteBatch.Draw(_midground, new Vector2(modX + textureWidth, 50), worldColor);
            #endregion


            worldManager.Draw(gameTime, Core.SpriteBatch);

            Core.SpriteBatch.DrawString(timerfont, $"HP: {_player.Health}/{3}", new Vector2(650, 20), Color.Gold);

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
                _pauseMenu.UpdateInput();
                return;
            }
            #endregion

            worldManager.Update(gameTime);

            if (_player.Health <= 0 && !_shaking) {
                _shakeTime = 0f;
                _shaking = true;
            }

            // Keeps pumpkin on left side of screen
            _player.Position = new Vector2(50, _player.Position.Y);
            CheckPumpkinOutOfBounds();
            _player.CheckPumpkinPlatTouch(worldManager.GetActivePlatforms());

            // Update difficulty scaling
            UpdateEnemySpawnInterval();

            // Countdown timer
            _enemySpawnTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (_enemySpawnTimer <= 0f) {
                SpawnRandomEnemy();
                _enemySpawnTimer = _enemySpawnInterval;
            }

            foreach (var enemy in _enemies.ToList()) {
                if (!enemy.IsActive) {
                    _enemies.Remove(enemy);
                    _components.Remove(enemy);
                }
                else { enemy.CheckCollision(_player); }
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