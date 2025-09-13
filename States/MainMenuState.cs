using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RIPRUSH.Entities;
using RIPRUSH.Sprites;
using System;
using System.Collections.Generic;

namespace RIPRUSH.States {

    /// <summary>
    /// A class representing the main menu state of the game
    /// </summary>
    public class MainMenuState : State {

        /// <summary>
        /// A list of the game state's components to be drawn/updated/interacted with
        /// </summary>
        private List<Component> _components;

        /// <summary>
        /// The texture for the buttons
        /// </summary>
        private Texture2D _buttonTexture;

        /// <summary>
        /// The text font for the buttons
        /// </summary>
        private SpriteFont _buttonFont;


        /// <summary>
        /// The font for "R.I.P" in the title
        /// </summary>
        private SpriteFont _titleFont;

        /// <summary>
        /// The Font for "Rush" in the title
        /// </summary>
        private SpriteFont _titleFont2;

        /// <summary>
        /// texture for the moon
        /// </summary>
        private Texture2D _moon_texture;

        /// <summary>
        /// A texture for the pumpkin face that goes on the moon
        /// </summary>
        private Texture2D _pumpkinFaceTexture;

        /// <summary>
        /// The color of the pumpkin face that goes on the moon
        /// </summary>
        private Color _pumpkinFaceColor = Color.Black;

        /// <summary>
        /// texture for tree 1
        /// </summary>
        private Texture2D _tree1_texture;
        
        /// <summary>
        /// textrure for tree 2
        /// </summary>
        private Texture2D _tree2_texture;

        /// <summary>
        /// texture for grave 1
        /// </summary>
        private Texture2D _grave1_texture;

        /// <summary>
        /// texture for grave 2
        /// </summary>
        private Texture2D _grave2_texture;

        /// <summary>
        /// the player object
        /// </summary>
        private PumpkinSprite _player;

        public MainMenuState(ContentManager content, Game1 game, GraphicsDevice graphicsDevice) : base(content, game, graphicsDevice) {

            #region -- Aesthetics --

            _titleFont = content.Load<SpriteFont>("Fonts/raven-scream");
            _titleFont2 = content.Load<SpriteFont>("Fonts/october-crow");

            _pumpkinFaceTexture = content.Load<Texture2D>("Assets/face2");
            _moon_texture = content.Load<Texture2D>("Assets/the moon");

            _tree1_texture = content.Load<Texture2D>("Assets/tree1");
            _tree2_texture = content.Load<Texture2D>("Assets/tree2");

            _grave1_texture = content.Load<Texture2D>("Assets/headstone");
            _grave2_texture = content.Load<Texture2D>("Assets/woodstone");

            #endregion


            #region -- Menu Buttons --

            _buttonTexture = content.Load<Texture2D>("Assets/Button2");
            _buttonFont = content.Load<SpriteFont>("Fonts/coralines-cat");

            var playButton = new Button(_buttonTexture, _buttonFont, Color.Green, Color.OrangeRed) {
                Position = new Vector2(25, 250),
                Text = "Play",
            };

            var somethingButton = new Button(_buttonTexture, _buttonFont, Color.Green, Color.OrangeRed) {
                Position = new Vector2(25, 330),
                Text = "???",
            };

            var quitButton = new Button(_buttonTexture, _buttonFont, Color.Green, Color.OrangeRed) {
                Position = new Vector2(25, 410),
                Text = "Quit",
            };

            playButton.Click += PlayButton_Click;
            somethingButton.Click += SomethingButton_Click;
            quitButton.Click += QuitButton_Click;

            #endregion


            _player = new PumpkinSprite(new Dictionary<string, Animation>() {
                { "Roll", new Animation(content.Load<Texture2D>("Player/Roll"), 15, true, Color.White, Vector2.Zero, 0, 6) },
                { "Idle", new Animation(content.Load<Texture2D>("Player/Idle"), 20, true, Color.White, Vector2.Zero, 0, 6) },
            });
            _player.Position = new Vector2(335, 350);

            _components = new List<Component>(){
                playButton,
                somethingButton,
                quitButton,
                _player
            };

        }

        #region Button Click Events

        /// <summary>
        /// The event handler for when the play button is clicked
        /// </summary>
        /// <param name="sender">The object signaling the event</param>
        /// <param name="e">Information about the event</param>
        private void PlayButton_Click(object sender, System.EventArgs e) {
            _game.ChangeState(new GameState(_content, _game, _graphicsDevice));
        }

        /// <summary>
        /// The event handler for when the ??? button is clicked
        /// </summary>
        /// <param name="sender">The object signaling the event</param>
        /// <param name="e">Information about the event</param>
        private void SomethingButton_Click(object sender, System.EventArgs e) {
            var random = new Random();
            Color randomColor = new Color(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
            _game.backgroundColor = randomColor;
            _pumpkinFaceColor = randomColor;
        }

        /// <summary>
        /// The event handler for when the quit button is clicked
        /// </summary>
        /// <param name="sender">The object signaling the event</param>
        /// <param name="e">Information about the event</param>
        private void QuitButton_Click(object sender, System.EventArgs e) {
            _game.Exit();
        }
        #endregion

        /// <summary>
        /// Draws the main menu 
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance used to draw textures and sprites to the screen.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            Rectangle sprite32 = new Rectangle(0, 0, 32, 32);
            Rectangle sprite64 = new Rectangle(0, 0, 64, 64);

            spriteBatch.Draw(_moon_texture, new Vector2(350, 50), sprite32, Color.White, 0, new Vector2(0, 0), 6f, SpriteEffects.None , 0);
            spriteBatch.Draw(_pumpkinFaceTexture, new Vector2(365, 0), sprite32, _pumpkinFaceColor, .2f, new Vector2(0, 0), 7f, SpriteEffects.None , 0);
            
            spriteBatch.Draw(_grave1_texture, new Vector2(200, 400), sprite32, Color.White, -.2f, new Vector2(0, 0), 4f, SpriteEffects.None , 0);
            spriteBatch.Draw(_grave2_texture, new Vector2(100, 350), sprite32, Color.White, .2f, new Vector2(0, 0), 4f, SpriteEffects.None , 0);

            spriteBatch.Draw(_grave2_texture, new Vector2(650, 375), sprite32, Color.White, -.2f, new Vector2(0, 0), 4f, SpriteEffects.None , 0);
            spriteBatch.Draw(_grave1_texture, new Vector2(550, 375), sprite32, Color.White, .2f, new Vector2(0, 0), 4f, SpriteEffects.None , 0);
            
            spriteBatch.Draw(_tree1_texture, new Vector2(-100, 50), sprite64, Color.SaddleBrown, .3f, new Vector2(0, 0), 6f, SpriteEffects.None , 0);
            spriteBatch.Draw(_tree2_texture, new Vector2(550, 100), sprite64, Color.SaddleBrown, -.3f, new Vector2(0, 0), 6f, SpriteEffects.None , 0);

            spriteBatch.DrawString(_titleFont, "R.I.P", new Vector2(80, 25), Color.Green);
            spriteBatch.DrawString(_titleFont2, "RUSH", new Vector2(50, 125), Color.OrangeRed);

            foreach (var component in _components) {
                component.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }

        /// <summary>
        /// The current state's post-update logic
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        public override void PostUpdate(GameTime gameTime) {}

        /// <summary>
        /// The main menu's update logic
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        public override void Update(GameTime gameTime) {
            
            foreach (var component in _components) {
                component.Update(gameTime);
            }
        }
    }
}
