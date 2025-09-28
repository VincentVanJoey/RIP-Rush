using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;
using MonoGameLibrary;
using RenderingLibrary.Graphics;
using RIPRUSH.Scenes;
using System;
using System.Linq;

namespace RIPRUSH.Screens
{
    partial class TitleScreen
    {
        partial void CustomInitialize()
        {
            PlayButton.Click += PlayButton_Click;
            SettingsButton.Click += SomethingButton_Click;
            QuitButton.Click += QuitButton_Click;
        }

        #region Button Click Events

        /// <summary>
        /// The event handler for when the play button is clicked
        /// </summary>
        /// <param name="sender">The object signaling the event</param>
        /// <param name="e">Information about the event</param>
        private void PlayButton_Click(object sender, System.EventArgs e) {
            Core.ChangeScene(new GameScene());
        }

        /// <summary>
        /// The event handler for when the settings button is clicked
        /// </summary>
        /// <param name="sender">The object signaling the event</param>
        /// <param name="e">Information about the event</param>
        private void SomethingButton_Click(object sender, System.EventArgs e) {
            Console.WriteLine("something");
        }

        /// <summary>
        /// The event handler for when the quit button is clicked
        /// </summary>
        /// <param name="sender">The object signaling the event</param>
        /// <param name="e">Information about the event</param>
        private void QuitButton_Click(object sender, System.EventArgs e) {
            Core.Instance.Exit();
        }
        #endregion
    }
}
