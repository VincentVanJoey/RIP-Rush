using Gum.Forms.Controls;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using RIPRUSH.Components.Joelements;
using RIPRUSH.Scenes;
using System;
using System.Linq;

namespace RIPRUSH.Components.Joelements
{
    partial class PauseMenu
    {
        private MainMenuButton[] _buttons;
        private int _focusedIndex = 0;

        private SoundEffect _uiSound;
        public bool InputEnabled = true;

        partial void CustomInitialize()
        {

            _uiSound = Core.Content.Load<SoundEffect>("Assets/Audio/UI");
            PauseResumeButton.Click += PauseResumeButton_Click;
            PauseTitleButton.Click += PauseTitleButton_Click;

            InitializeButtons();

        }

        public void InitializeButtons() {
            _buttons = [PauseResumeButton, PauseTitleButton];
            _focusedIndex = 0;
            _buttons[_focusedIndex].ButtonCategoryState = MainMenuButton.ButtonCategory.Highlighted;
        }

        public void UpdateInput() {

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
            _buttons[_focusedIndex].ButtonCategoryState = MainMenuButton.ButtonCategory.Enabled; ;
            _focusedIndex = (_focusedIndex + direction + _buttons.Length) % _buttons.Length;
            _buttons[_focusedIndex].ButtonCategoryState = MainMenuButton.ButtonCategory.Highlighted;
        }

        private void PauseTitleButton_Click(object sender, System.EventArgs e) {
            Core.Audio.PlaySoundEffect(_uiSound);
            Core.Audio.PauseAudio();
            Core.ChangeScene(new MainMenuScene());
        }
        private void PauseResumeButton_Click(object sender, System.EventArgs e) {
            IsVisible = false;
            GameScene gameplay = Core.GetActiveScene() as GameScene;
            if (gameplay != null) {
                gameplay.GameActive = true;
            }
        }

    }
}
