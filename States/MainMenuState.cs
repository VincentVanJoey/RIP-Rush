using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;

namespace RIPRUSH.States {
    public class MainMenuState : State {

        //private Desktop desktop;
        //private FontSystem ordinaryFontSystem;

        private List<Component> components;
        
        private Texture2D buttonTexture;
        private SpriteFont buttonFont;

        private SpriteFont titleFont;
        private SpriteFont titleFont2;

        private Texture2D moon_texture;
        private Texture2D tree1_texture;
        private Texture2D tree2_texture;
        private Texture2D grave1_texture;
        private Texture2D grave2_texture;
        private Texture2D pumpkin_face_texture;
        

        public MainMenuState(ContentManager content, Game1 game, GraphicsDevice graphicsDevice) : base(content, game, graphicsDevice) {

            titleFont = content.Load<SpriteFont>("Fonts/raven-scream");
            titleFont2 = content.Load<SpriteFont>("Fonts/october-crow");

            pumpkin_face_texture = content.Load<Texture2D>("Assets/face");
            moon_texture = content.Load<Texture2D>("Assets/the moon");

            tree1_texture = content.Load<Texture2D>("Assets/tree1");
            tree2_texture = content.Load<Texture2D>("Assets/tree2");

            grave1_texture = content.Load<Texture2D>("Assets/headstone");
            grave2_texture = content.Load<Texture2D>("Assets/woodstone");

            #region -- Menu Buttons --

            buttonTexture = content.Load<Texture2D>("Assets/Button");
            buttonFont = content.Load<SpriteFont>("Fonts/coralines-cat");

            var playButton = new Button(buttonTexture, buttonFont, Color.Orange) {
                Position = new Vector2(50, 250),
                Text = "Play",
            };

            var somethingButton = new Button(buttonTexture, buttonFont, Color.Orange) {
                Position = new Vector2(50, 300),
                Text = "???",
            };

            var quitButton = new Button(buttonTexture, buttonFont, Color.Orange) {
                Position = new Vector2(50, 350),
                Text = "Quit",
            };

            playButton.Click += PlayButton_Click;
            somethingButton.Click += SomethingButton_Click;
            quitButton.Click += QuitButton_Click;

            #endregion

            components = new List<Component>(){
                playButton,
                somethingButton,
                quitButton,
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

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            Rectangle Sprite32 = new Rectangle(0, 0, 32, 32);
            Rectangle Sprite64 = new Rectangle(0, 0, 64, 64);

            spriteBatch.Draw(moon_texture, new Vector2(350, 50), Sprite32, Color.White, 0, new Vector2(0, 0), 6f, SpriteEffects.None , 0);
            spriteBatch.Draw(pumpkin_face_texture, new Vector2(365, 0), Sprite32, Color.White, .2f, new Vector2(0, 0), 7f, SpriteEffects.None , 0);
            
            spriteBatch.Draw(grave1_texture, new Vector2(200, 400), Sprite32, Color.White, -.2f, new Vector2(0, 0), 4f, SpriteEffects.None , 0);
            spriteBatch.Draw(grave2_texture, new Vector2(100, 350), Sprite32, Color.White, .2f, new Vector2(0, 0), 4f, SpriteEffects.None , 0);

            spriteBatch.Draw(grave2_texture, new Vector2(650, 375), Sprite32, Color.White, -.2f, new Vector2(0, 0), 4f, SpriteEffects.None , 0);
            spriteBatch.Draw(grave1_texture, new Vector2(550, 375), Sprite32, Color.White, .2f, new Vector2(0, 0), 4f, SpriteEffects.None , 0);
            
            spriteBatch.Draw(tree1_texture, new Vector2(-100, 50), Sprite64, Color.SaddleBrown, .3f, new Vector2(0, 0), 6f, SpriteEffects.None , 0);
            spriteBatch.Draw(tree2_texture, new Vector2(550, 100), Sprite64, Color.SaddleBrown, -.3f, new Vector2(0, 0), 6f, SpriteEffects.None , 0);

            spriteBatch.DrawString(titleFont, "R.I.P", new Vector2(75, 25), Color.Green);
            spriteBatch.DrawString(titleFont2, "RUSH", new Vector2(50, 125), Color.OrangeRed);

            foreach (var component in components) {
                component.Draw(gameTime, spriteBatch);
            }

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
