using Gum.Forms.Controls;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using RIPRUSH.Components.Joelements;
using RIPRUSH.Scenes;
using System;
using System.Linq;

namespace RIPRUSH.Screens
{
    partial class Proj2SettingsScreen
    {
        private SoundEffect _uiSound;
        private FrameworkElement[] _focusableElements;
        private int _focusedIndex = 0;

        partial void CustomInitialize()
        {
            _uiSound = Core.Content.Load<SoundEffect>("Assets/Audio/UI");

            MusicSlider.SliderPercent = Core.Audio.SongVolume * 100;
            SoundSlider.SliderPercent = Core.Audio.SoundEffectVolume * 100;

            MusicSlider.ValueChanged += MusicSliderChanged;
            SoundSlider.ValueChanged += SoundSliderChanged;
            MusicSlider.ValueChangeCompleted += (_,_) => Core.Audio.PlaySoundEffect(_uiSound);
            SoundSlider.ValueChangeCompleted += (_,_) => Core.Audio.PlaySoundEffect(_uiSound);

            TurnBackButton.Click += BackButton_Click;

            _focusableElements = [ MusicSlider,SoundSlider, TurnBackButton ];
            SetElementFocus(_focusableElements[_focusedIndex], true);
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

            // Adjust slider values if currently focused on one
            
            if (focusedElement is RIPRUSH.Components.Controls.Slider slider) {
                if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Left)) {
                    slider.SliderPercent = Math.Max(0, slider.SliderPercent - 5);
                }
                else if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Right)) {
                    slider.SliderPercent = Math.Min(100, slider.SliderPercent + 5);
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

        private void MusicSliderChanged(object sender, EventArgs e) {
            float ratio = MusicSlider.SliderPercent / 100;
            Core.Audio.SongVolume = ratio;
        }

        private void SoundSliderChanged(object sender, EventArgs e) {
            float ratio = SoundSlider.SliderPercent / 100;
            Core.Audio.SoundEffectVolume = ratio;
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
