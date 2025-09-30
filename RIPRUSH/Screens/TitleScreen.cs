using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using MonoGameGum;
using MonoGameLibrary;
using RenderingLibrary.Graphics;
using RIPRUSH.Scenes;
using System.Linq;

namespace RIPRUSH.Screens
{
    partial class TitleScreen
    {
        private SoundEffect _uiSound;
        public Song MenuSong;

        partial void CustomInitialize()
        {

            _uiSound = Core.Content.Load<SoundEffect>("Assets/Audio/UI");
            PlayButton.Click += PlayButton_Click;
            SettingsButton.Click += SettingsButton_Click;
            QuitButton.Click += QuitButton_Click;

            MenuSong = Core.Content.Load<Song>("Assets/Audio/Music/MenuMusic");
            
            if (MediaPlayer.State != MediaState.Playing)
                Core.Audio.PlaySong(MenuSong);

        }

        #region Button Click Events

        /// <summary>
        /// The event handler for when the play button is clicked
        /// </summary>
        /// <param name="sender">The object signaling the event</param>
        /// <param name="e">Information about the event</param>
        private void PlayButton_Click(object sender, System.EventArgs e) {
            Core.Audio.PlaySoundEffect(_uiSound);
            Core.ChangeScene(new GameScene());
        }

        /// <summary>
        /// The event handler for when the settings button is clicked
        /// </summary>
        /// <param name="sender">The object signaling the event</param>
        /// <param name="e">Information about the event</param>
        private void SettingsButton_Click(object sender, System.EventArgs e) {
            Core.Audio.PlaySoundEffect(_uiSound);
            GumService.Default.Root.Children.Clear();
            var screen = new Proj2SettingsScreen();
            screen.AddToRoot();
        }

        /// <summary>
        /// The event handler for when the quit button is clicked
        /// </summary>
        /// <param name="sender">The object signaling the event</param>
        /// <param name="e">Information about the event</param>
        private void QuitButton_Click(object sender, System.EventArgs e) {
            Core.Audio.PlaySoundEffect(_uiSound);
            Core.Instance.Exit();
        }
        #endregion
    }
}
