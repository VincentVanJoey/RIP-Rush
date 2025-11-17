using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;
using RIPRUSH.Entities;
using RIPRUSH.Entities.Actors;
using RIPRUSH.Screens;
using System;
using System.Collections.Generic;
using Gum.Forms.Controls;


namespace RIPRUSH.Scenes {

    /// <summary>
    /// A class representing the main menu state of the game
    /// </summary>
    public class MainMenuScene : Scene {

        /// <summary>
        /// A list of the game state's components to be drawn/updated/interacted with
        /// </summary>
        private List<Component> _components;

        #region -- Aesthetic Fields --

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
        #endregion

        /// <summary>
        /// the player object
        /// </summary>
        private Pumpkin _player;


        private TitleScreen titlescreen;
        private Proj2SettingsScreen settingsScreen;
        private HTPScreen htpScreen;

        public bool titleFrameCheck = false;
        private enum MenuState { Title, Settings, HTP }
        private MenuState _currentMenuState;

        public override void Initialize() {

            // LoadContent is called during base.Initialize().
            base.Initialize();



            // While on the title screen, we can enable exit on escape so the player
            // can close the game by pressing the escape key.
            Core.ExitOnEscape = true;

            _player = new Pumpkin(Core.Content, false, 6) { Position = new Vector2(335, 350) };

            _components = new List<Component>(){
                _player
            };

            settingsScreen = new Proj2SettingsScreen();
            htpScreen = new HTPScreen();
            ShowTitleScreen();
        }

        public override void LoadContent() {
            _pumpkinFaceTexture = Core.Content.Load<Texture2D>("Assets/face2");
            _moon_texture = Core.Content.Load<Texture2D>("Assets/the moon");

            _tree1_texture = Core.Content.Load<Texture2D>("Assets/tree1");
            _tree2_texture = Core.Content.Load<Texture2D>("Assets/tree2");

            _grave1_texture = Core.Content.Load<Texture2D>("Assets/headstone");
            _grave2_texture = Core.Content.Load<Texture2D>("Assets/woodstone");

            // recreate titlescreen
            titlescreen = new TitleScreen();
            titlescreen.LoadContentSongs(); // now this will play immediately
        }

        public void ShowTitleScreen() {
            GumService.Default.Root.Children.Clear();

            titlescreen.AddToRoot();
            settingsScreen.RemoveFromRoot();

            titlescreen.InitializeButtons(); // reset keyboard focus
            _currentMenuState = MenuState.Title;
        }

        public void ShowSettingsScreen() {
            GumService.Default.Root.Children.Clear();

            settingsScreen.AddToRoot();
            titlescreen.RemoveFromRoot();

            _currentMenuState = MenuState.Settings;
        }

        public void ShowHTPScreen() {
            GumService.Default.Root.Children.Clear();

            htpScreen.AddToRoot();
            titlescreen.RemoveFromRoot();

            _currentMenuState = MenuState.HTP;
        }

        /// <summary>
        /// Draws the main menu 
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance used to draw textures and sprites to the screen.</param>
        public override void Draw(GameTime gameTime) {

            Core.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            Rectangle sprite32 = new Rectangle(0, 0, 32, 32);
            Rectangle sprite64 = new Rectangle(0, 0, 64, 64);

            Core.SpriteBatch.Draw(_moon_texture, new Vector2(350, 50), sprite32, Color.White, 0, new Vector2(0, 0), 6f, SpriteEffects.None , 0);
            Core.SpriteBatch.Draw(_pumpkinFaceTexture, new Vector2(365, 0), sprite32, _pumpkinFaceColor, .2f, new Vector2(0, 0), 7f, SpriteEffects.None , 0);

            Core.SpriteBatch.Draw(_grave1_texture, new Vector2(200, 400), sprite32, Color.White, -.2f, new Vector2(0, 0), 4f, SpriteEffects.None , 0);
            Core.SpriteBatch.Draw(_grave2_texture, new Vector2(100, 350), sprite32, Color.White, .2f, new Vector2(0, 0), 4f, SpriteEffects.None , 0);

            Core.SpriteBatch.Draw(_grave2_texture, new Vector2(650, 375), sprite32, Color.White, -.2f, new Vector2(0, 0), 4f, SpriteEffects.None , 0);
            Core.SpriteBatch.Draw(_grave1_texture, new Vector2(550, 375), sprite32, Color.White, .2f, new Vector2(0, 0), 4f, SpriteEffects.None , 0);

            Core.SpriteBatch.Draw(_tree1_texture, new Vector2(-100, 50), sprite64, Color.SaddleBrown, .3f, new Vector2(0, 0), 6f, SpriteEffects.None , 0);
            Core.SpriteBatch.Draw(_tree2_texture, new Vector2(550, 100), sprite64, Color.SaddleBrown, -.3f, new Vector2(0, 0), 6f, SpriteEffects.None , 0);

            foreach (var component in _components) {
                component.Draw(gameTime, Core.SpriteBatch);
            }

            Core.SpriteBatch.End();
        }

        /// <summary>
        /// The main menu's update logic
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        public override void Update(GameTime gameTime) {

            // if returning to the title screen, skip the rest of this frame's update, or else we'll double press on a button 
            if (titleFrameCheck) {
                titleFrameCheck = false;
                ShowTitleScreen();
                return; // skip the rest of this frame
            }

            switch (_currentMenuState) {
                case MenuState.Title:
                    titlescreen.UpdateInput();
                    break;
                case MenuState.Settings:
                    settingsScreen.UpdateInput();
                    break;
                case MenuState.HTP:
                    htpScreen.UpdateInput();
                    break;
            }

            foreach (var component in _components) {
                    component.Update(gameTime);
                }
        }

    }
}
