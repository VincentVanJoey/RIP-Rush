using Gum.Forms;
using Gum.Forms.Controls;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGameGum;
using MonoGameLibrary;
using RIPRUSH.Entities;
using RIPRUSH.Scenes;
using System;

namespace RIPRUSH {
    /// <summary>
    /// The main game class, responsible for initializing and running the game.
    /// </summary>
    public class Game1 : Core {
        public Song GameSong;

        public Game1() : base("RIP RUSH", 800, 480, false) {
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Initializes the game and prepares it for execution.
        /// </summary>
        /// <remarks>This method sets up the initial state of the game, including enabling mouse
        /// visibility. It should be called before the game starts running. Overrides the base implementation to include
        /// additional initialization logic specific to this game.</remarks>
        protected override void Initialize() {
            base.Initialize();
            InitializeGum();
            IsMouseVisible = true;

            Window.ClientSizeChanged += (_, _) => {
                // Recompute your virtual resolution scaling
                VirtualResolution.ComputeScale(GraphicsDevice);
                GraphicalUiElement.CanvasWidth = VirtualResolution.VirtualWidth;
                GraphicalUiElement.CanvasHeight = VirtualResolution.VirtualHeight;
                GumService.Default.Root?.UpdateLayout();

                // Update cursor transform so clicks map correctly
                var cursor = GumService.Default.Cursor;
                float scale = VirtualResolution.Scale;
                cursor.TransformMatrix = Matrix.CreateScale(1f / scale, 1f / scale, 1f);
                cursor.TransformMatrix *= Matrix.CreateTranslation(-VirtualResolution.DestinationRect.X / scale, -VirtualResolution.DestinationRect.Y / scale, 0);
            };

            ChangeScene(new MainMenuScene());
            SaveFileManager.Initialize();
        }

        /// <summary>
        /// The load content method, called once per game
        /// Loads the game's assets
        /// </summary>
        protected override void LoadContent() {
            GameSong = Content.Load<Song>("Assets/Audio/Music/GameMusic");
            base.LoadContent();
        }

        /// <summary>
        /// The game's update logic
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        protected override void Update(GameTime gameTime) {
            // If the M key is pressed, toggle mute state for audio.
            if (Input.Keyboard.WasKeyJustPressed(Keys.M)) {
                Audio.ToggleMute();
            }

            // If the + button is pressed, increase the volume.
            if (Input.Keyboard.WasKeyJustPressed(Keys.OemPlus)) {
                Audio.SongVolume += 0.1f;
                Audio.SoundEffectVolume += 0.1f;
            }

            // If the - button was pressed, decrease the volume.
            if (Input.Keyboard.WasKeyJustPressed(Keys.OemMinus)) {
                Audio.SongVolume -= 0.1f;
                Audio.SoundEffectVolume -= 0.1f;
            }

            GumService.Default.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance used to draw textures and sprites to the screen.</param>
        protected override void Draw(GameTime gameTime) {
            VirtualResolution.BeginDraw(GraphicsDevice);

            // Draw scene (through Core)
            if (Core.GetActiveScene() != null) {
                Core.GetActiveScene().Draw(gameTime);
            }

            // Draw UI
            GumService.Default.Draw();

            VirtualResolution.EndDraw(GraphicsDevice, Core.SpriteBatch);
        }

        private void InitializeGum() {
            // Initialize the Gum service. The second parameter specifies
            // the the GUMUI forms file used
            GumService.Default.Initialize(this, "GumProject/RIPRUSH_GUIS.gumx");

            // Tell the Gum service which content manager to use.  We will tell it to
            // use the global content manager from our Core.
            GumService.Default.ContentLoader.XnaContentManager = Core.Content;

            // Register keyboard/gamepad input for UI control.
            FrameworkElement.KeyboardsForUiControl.Add(GumService.Default.Keyboard);
            FrameworkElement.GamePadsForUiControl.AddRange(GumService.Default.Gamepads);

            // Customize the tab UI navigation to also trigger when the keyboard
            // Up/Down arrow key is pushed.
            FrameworkElement.TabReverseKeyCombos.Add(
               new KeyCombo() { PushedKey = Microsoft.Xna.Framework.Input.Keys.Up });
            FrameworkElement.TabKeyCombos.Add(
               new KeyCombo() { PushedKey = Microsoft.Xna.Framework.Input.Keys.Down });

        }
    }
}