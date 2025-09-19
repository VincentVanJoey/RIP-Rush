using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGameLibrary;
using MonoGameLibrary.Input;
using RIPRUSH.Screens;
using RIPRUSH.States;
using MonoGameGum;
using MonoGameGum.Forms;
using Gum.Forms.Controls;

namespace RIPRUSH
{
    /// <summary>
    /// The main game class, responsible for initializing and running the game.
    /// </summary>
    public class Game1 : Core
    {

        /// <summary>
        /// The background color of the game window.
        /// </summary>
        public Color backgroundColor;

        /// <summary>
        /// The Gum UI service, used for managing UI elements and interactions.
        /// </summary>
        GumService GumUI => GumService.Default;

        /// <summary>
        /// The graphics device manager, responsible for managing the graphics settings and device.
        /// </summary>
        private GraphicsDeviceManager _graphics;

        /// <summary>
        /// The sprite batch, used for drawing textures and sprites to the screen.
        /// </summary>
        private SpriteBatch _spriteBatch;


        /// <summary>
        /// The current state of the game, representing the active screen or mode.
        /// </summary>
        private State _currentState;

        /// <summary>
        /// The next state of the game, used for transitioning between different screens or modes.
        /// </summary>
        private State _nextState;


        /// <summary>
        /// Changes the "state" or current screen of the game
        /// </summary>
        /// <param name="state">The current state of the game</param>
        public void ChangeState(State state)
        {
            _nextState = state;
        }

        public Game1() : base("RIP RUSH", 800, 480, false) {
            _graphics = Graphics;
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
        }

        /// <summary>
        /// The load content method, called once per game
        /// Loads the game's assets
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = SpriteBatch;
            _currentState = new MainMenuState(Content, this, _graphics.GraphicsDevice);

            base.LoadContent();
        }

        /// <summary>
        /// The game's update logic
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        protected override void Update(GameTime gameTime)
        {   
            if (_nextState != null) { 
                _currentState = _nextState;
                _nextState = null;
            }

            _currentState.Update(gameTime);
            GumUI.Update(gameTime);
            _currentState.PostUpdate(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance used to draw textures and sprites to the screen.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColor);

            _currentState.Draw(gameTime, _spriteBatch);
            GumUI.Draw();
            base.Draw(gameTime);
        }
    }
}
