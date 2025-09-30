using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;
using Microsoft.Xna.Framework.Audio;
using MonoGameGum;
using MonoGameLibrary;
using RenderingLibrary.Graphics;
using RIPRUSH.Scenes;
using System;
using System.Linq;

namespace RIPRUSH.Screens
{
    partial class Proj2SettingsScreen
    {
        private SoundEffect _uiSound;
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
            GumService.Default.Root.Children.Clear();
            var screen = new TitleScreen();
            screen.AddToRoot();
        }
    }
}
