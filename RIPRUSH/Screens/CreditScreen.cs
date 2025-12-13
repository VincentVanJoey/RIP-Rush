using Gum.Forms.Controls;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using MonoGameGum.GueDeriving;
using MonoGameLibrary;
using RIPRUSH.Components.Joelements;
using RIPRUSH.Scenes;
using System;
using System.IO;

namespace RIPRUSH.Screens {
    partial class CreditScreen {
        private SoundEffect _uiSound;
        private FrameworkElement[] _focusableElements;
        private int _focusedIndex = 0;

        partial void CustomInitialize() {
            _uiSound = Core.Content.Load<SoundEffect>("Assets/Audio/UI");

            TurnBackButton.Click += BackButton_Click;

            _focusableElements = [TurnBackButton];
            SetElementFocus(_focusableElements[_focusedIndex], true);

            // Load credits from assets.txt
            LoadCreditsFromFile();
        }
        private void LoadCreditsFromFile() {
            string filePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "..", "..", "..",
                "ASSETS.txt"
            );

            string fileContents = File.Exists(filePath) ? File.ReadAllText(filePath) : "Error: assets.txt not found!";

            if (CreditScroll?.InnerPanel != null) {
                var textElement = CreditScroll.InnerPanel.GetGraphicalUiElementByName("CreditsText") as TextRuntime;
                if (textElement != null) {
                    textElement.Text = fileContents;
                }

                CreditScroll.InnerPanel.Y = 0;
            }
        }

        public void UpdateInput() {

            var focusedElement = _focusableElements[_focusedIndex];

            if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Down)) {
                ChangeFocus(1);
                Core.Audio.PlaySoundEffect(_uiSound);
            }
            else if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Up)) {
                ChangeFocus(-1);
                Core.Audio.PlaySoundEffect(_uiSound);
            }
            else if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Space)) {
                if (focusedElement is MainMenuButton button) {
                    button.PerformClick();
                }
            }

        }

        private void ChangeFocus(int direction) {
            // Remove previous focus
            SetElementFocus(_focusableElements[_focusedIndex], false);

            _focusedIndex = (_focusedIndex + direction + _focusableElements.Length) % _focusableElements.Length;

            SetElementFocus(_focusableElements[_focusedIndex], true);

            Core.Audio.PlaySoundEffect(_uiSound);
        }

        private void SetElementFocus(FrameworkElement element, bool isFocused) {
            switch (element) {
                case Slider s:
                    s.IsFocused = isFocused;
                    break;
                case MainMenuButton b:
                    b.ButtonCategoryState = isFocused ? MainMenuButton.ButtonCategory.Highlighted : MainMenuButton.ButtonCategory.Enabled;
                    break;
            }
        }

        /// <summary>
        /// The event handler for when the back button is clicked
        /// </summary>
        /// <param name="sender">The object signaling the event</param>
        /// <param name="e">Information about the event</param>
        private void BackButton_Click(object sender, System.EventArgs e) {
            Core.Audio.PlaySoundEffect(_uiSound);
            MainMenuScene mainMenuScene = Core.GetActiveScene() as MainMenuScene;

            if (mainMenuScene != null) {
                mainMenuScene.titleFrameCheck = true;
            }
        }
    }
}