using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;
using MonoGameLibrary;
using RIPRUSH.Screens;
using RIPRUSH.States;

namespace RIPRUSH
{
    /// <summary>
    /// The main game class, responsible for initializing and running the game.
    /// </summary>
    public class Game1 : Core
    {

        /// <summary>
        /// The Gum UI service, used for managing UI elements and interactions.
        /// </summary>
        GumService GumUI => GumService.Default;

        public Game1() : base("RIP RUSH", 800, 480, false) {
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Initializes the game and prepares it for execution.
        /// </summary>
        /// <remarks>This method sets up the initial state of the game, including enabling mouse
        /// visibility. It should be called before the game starts running. Overrides the base implementation to include
        /// additional initialization logic specific to this game.</remarks>
        protected override void Initialize()
        {
            IsMouseVisible = true;
            
            GumUI.Initialize(this, "GumProject/RIPRUSH_GUIS.gumx");

            base.Initialize();

            ChangeScene(new MainMenuScene());
        }

        /// <summary>
        /// The load content method, called once per game
        /// Loads the game's assets
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
        }

        /// <summary>
        /// The game's update logic
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        protected override void Update(GameTime gameTime)
        {   
            GumUI.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance used to draw textures and sprites to the screen.</param>
        protected override void Draw(GameTime gameTime)
        {
            GumUI.Draw();
            base.Draw(gameTime);
        }
    }
}