using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGameGum;
using MonoGameLibrary;
using RenderingLibrary.Graphics;
using RIPRUSH.Components.Joelements;
using RIPRUSH.Entities.Actors;
using RIPRUSH.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RIPRUSH.Screens
{
    partial class TitleScreen
    {
        private SoundEffect _uiSound;
        private SoundEffect _exitSound;
        private SoundEffectInstance _exitSoundInstance;
        public Song MenuSong;
        private MainMenuButton[] _buttons;
        private int _focusedIndex = 0;
        private bool _isExiting = false;

        partial void CustomInitialize()
        {

            _uiSound = Core.Content.Load<SoundEffect>("Assets/Audio/UI");
            _exitSound = Core.Content.Load<SoundEffect>("Assets/Audio/Quit");
            PlayButton.Click += PlayButton_Click;
            SettingsButton.Click += SettingsButton_Click;
            QuitButton.Click += QuitButton_Click;
            HTPButton.Click += HTP_Click;
                                    
            InitializeButtons();
        }

        public void LoadContentSongs() {
            // Load menu music in LoadContent(), not Initialize()
            MenuSong = Core.Content.Load<Song>("Assets/Audio/Music/MenuMusic");

            // Play immediately if not already playing
            if (MediaPlayer.State != MediaState.Playing) {
                Core.Audio.PlaySong(MenuSong);
                MediaPlayer.Volume = 1f; // optional: fade in later
            }
        }

        public void InitializeButtons() {
            _buttons = [ PlayButton, SettingsButton, QuitButton, HTPButton ];
            _focusedIndex = 0;
            _buttons[_focusedIndex].ButtonCategoryState = MainMenuButton.ButtonCategory.Highlighted;
        }

        public void UpdateInput() {
            if (_isExiting) {
                // If the exit sound has finished, actually quit
                if (_exitSoundInstance.State == SoundState.Stopped) {
                    Core.Instance.Exit();
                }
                return; // skip other input while exiting
            }

            if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Down)) {
                ChangeFocus(1);
                Core.Audio.PlaySoundEffect(_uiSound);
            }
            else if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Up)) {
                ChangeFocus(-1);
                Core.Audio.PlaySoundEffect(_uiSound);
            }
            else if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Space)) {
                var focusedButton = _buttons[_focusedIndex];
                focusedButton?.PerformClick();
            }
        }
        
        private void ChangeFocus(int direction) {
            _buttons[_focusedIndex].ButtonCategoryState = MainMenuButton.ButtonCategory.Enabled;
            _focusedIndex = (_focusedIndex + direction + _buttons.Length) % _buttons.Length;
            _buttons[_focusedIndex].ButtonCategoryState = MainMenuButton.ButtonCategory.Highlighted;
        }

        #region Button Click Events

        /// <summary>
        /// The event handler for when the play button is clicked
        /// </summary>
        /// <param name="sender">The object signaling the event</param>
        /// <param name="e">Information about the event</param>
        private void PlayButton_Click(object sender, System.EventArgs e) {
            Core.Audio.PlaySoundEffect(_uiSound);
            Core.Audio.PauseAudio();
            Core.ChangeScene(new GameScene());
        }

        /// <summary>
        /// The event handler for when the settings button is clicked
        /// </summary>
        /// <param name="sender">The object signaling the event</param>
        /// <param name="e">Information about the event</param>
        private void SettingsButton_Click(object sender, System.EventArgs e) {
            Core.Audio.PlaySoundEffect(_uiSound);
            MainMenuScene mainMenuScene = Core.GetActiveScene() as MainMenuScene;
            mainMenuScene?.ShowSettingsScreen();
        }

        private void QuitButton_Click(object sender, System.EventArgs e) {
            if (_isExiting) return; // prevent multiple triggers

            _isExiting = true;

            // Play the exit sound as an instance
            _exitSoundInstance = Core.Audio.PlaySoundEffect(_exitSound);
        }

        private void HTP_Click(object sender, System.EventArgs e) {
            Core.Audio.PlaySoundEffect(_uiSound);
            MainMenuScene mainMenuScene = Core.GetActiveScene() as MainMenuScene;
            mainMenuScene?.ShowHTPScreen();
        }

        #endregion
    }
}
