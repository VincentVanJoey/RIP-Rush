using FontStashSharp;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.IO;

namespace RIPRUSH.States {
    public class MainMenuState : State {

        //private Desktop desktop;
        //private FontSystem ordinaryFontSystem;

        private List<Component> components;

        public MainMenuState(ContentManager content, Game1 game, GraphicsDevice graphicsDevice) : base(content, game, graphicsDevice) {

            #region -- Myra Testing --

            //MyraEnvironment.Game = game;


            //var label = new Label();
            //label.Text = "Test";
            //label.VerticalAlignment = VerticalAlignment.Center;
            //label.HorizontalAlignment = HorizontalAlignment.Center;

            //byte[] ttfData = File.ReadAllBytes("Fonts/Coraline's Cat.ttf");
            //ordinaryFontSystem = new FontSystem();
            //ordinaryFontSystem.AddFont(ttfData);
            //label.Font = ordinaryFontSystem.GetFont(25);

            //var button = new Myra.Graphics2D.UI.Button();
            //button.Width = 125;
            //button.Height = 30;
            //button.HorizontalAlignment = HorizontalAlignment.Left;
            //button.VerticalAlignment = VerticalAlignment.Center;
            //button.Content = label;
            //button.Click += SomethingButton_Click;

            //desktop = new Desktop();
            //desktop.Root = button;

            #endregion

            #region Monogame Menu Buttons
            var buttonTexture = content.Load<Texture2D>("Assets/Button");
            var buttonFont = content.Load<SpriteFont>("Fonts/coralines-cat");

            var playButton = new Button(buttonTexture, buttonFont, Color.Orange) {
                Position = new Vector2(50, 200),
                Text = "Play",
            };

            var somethingButton = new Button(buttonTexture, buttonFont, Color.Orange) {
                Position = new Vector2(50, 250),
                Text = "???",
            };

            var quitButton = new Button(buttonTexture, buttonFont, Color.Orange) {
                Position = new Vector2(50, 300),
                Text = "Quit",
            };


            playButton.Click += PlayButton_Click;
            somethingButton.Click += SomethingButton_Click;
            quitButton.Click += QuitButton_Click;
            #endregion

            components = new List<Component>(){
                playButton,
                somethingButton,
                quitButton
            };
        }

        #region Button Click Events
        private void PlayButton_Click(object sender, System.EventArgs e) {
            game.ChangeState(new GameState(content, game, graphicsDevice));
        }

        private void SomethingButton_Click(object sender, System.EventArgs e) {
            var random = new Random();
            game.backgroundColor = new Color(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
        }

        private void QuitButton_Click(object sender, System.EventArgs e) {
            game.Exit();
        }
        #endregion

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Begin();

            foreach (var component in components) {
                component.Draw(gameTime, spriteBatch);
            }

            //desktop.Render();

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
