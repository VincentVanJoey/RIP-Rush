using System;
using System.Linq;
using Gum.Forms.Controls;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using MonoGameLibrary;
using RIPRUSH.Components.Joelements;
using RIPRUSH.Entities;
using RIPRUSH.Scenes;

namespace RIPRUSH.Screens
{
    partial class Proj2SettingsScreen
    {
        private SoundEffect _uiSound;
        private FrameworkElement[] _focusableElements;
        private int _focusedIndex = 0;
        private Point _previousResolution;
        private bool _isApplyingResolutionProgrammatically = false;


        partial void CustomInitialize()
        {
            _uiSound = Core.Content.Load<SoundEffect>("Assets/Audio/UI");

            MusicSlider.SliderPercent = SaveFileManager.Data.MusicVolume * 100;
            SoundSlider.SliderPercent = SaveFileManager.Data.SoundVolume * 100;

            Core.Audio.SongVolume = SaveFileManager.Data.MusicVolume;
            Core.Audio.SoundEffectVolume = SaveFileManager.Data.SoundVolume;

            // Load saved resolution immediately
            ApplyResolution(
                SaveFileManager.Data.ResolutionWidth,
                SaveFileManager.Data.ResolutionHeight,
                SaveFileManager.Data.IsFullscreen
            );

            PopulateResolutions();

            ResolutionBox.SelectionChanged += ResolutionChanged;
            FullScreenCheckbox.Checked += FullScreenChanged;
            FullScreenCheckbox.Unchecked += FullScreenChanged;

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

        private void PopulateResolutions() {
            ResolutionBox.Items.Clear();

            var displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            int maxWidth = displayMode.Width;
            int maxHeight = displayMode.Height;

            var supported = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes
                .Where(m => m.Format == SurfaceFormat.Color)
                .Select(m => new Point(m.Width, m.Height))
                .Distinct()
                .Where(p => p.X >= 800 && p.Y >= 480)
                .Where(p => p.X <= maxWidth && p.Y <= maxHeight)
                .OrderBy(p => p.X * p.Y)
                .ToList();

            // Ensure 800x480 always exists
            var minimum = new Point(800, 480);
            if (!supported.Any(p => p.X == 800 && p.Y == 480))
                supported.Insert(0, minimum);

            foreach (var res in supported) {
                ResolutionBox.Items.Add($"{res.X} x {res.Y}");
            }

            // Select saved resolution if it exists, otherwise default to 800x480
            var saved = SaveFileManager.Data;
            string savedResolution = $"{saved.ResolutionWidth} x {saved.ResolutionHeight}";

            if (supported.Any(p => $"{p.X} x {p.Y}" == savedResolution)) {
                _previousResolution = new Point(saved.ResolutionWidth, saved.ResolutionHeight);
            }
            else {
                _previousResolution = new Point(800, 480);
                savedResolution = "800 x 480";
            }

            _isApplyingResolutionProgrammatically = true;
            ResolutionBox.SelectedObject = savedResolution;
            _isApplyingResolutionProgrammatically = false;

            // Set fullscreen checkbox from saved data
            FullScreenCheckbox.IsChecked = saved.IsFullscreen;
            ResolutionBox.IsEnabled = !(saved.IsFullscreen);
        }


        private void ResolutionChanged(object sender, SelectionChangedEventArgs e) {
            if (_isApplyingResolutionProgrammatically || ResolutionBox.SelectedObject == null)
                return;

            string selected = ResolutionBox.SelectedObject.ToString();
            var parts = selected.Split('x');

            int width = int.Parse(parts[0].Trim());
            int height = int.Parse(parts[1].Trim());

            // Update only if not fullscreen
            if (!(FullScreenCheckbox.IsChecked ?? false)) {
                _previousResolution = new Point(width, height);
            }

            ApplyResolution(width, height, FullScreenCheckbox.IsChecked ?? false);
            SaveFileManager.Set(d =>
            {
                d.ResolutionWidth = width;
                d.ResolutionHeight = height;
            });
        }


        private void ApplyResolution(int width, int height, bool fullscreen) {
            Core.Graphics.PreferredBackBufferWidth = width;
            Core.Graphics.PreferredBackBufferHeight = height;
            Core.Graphics.IsFullScreen = fullscreen;
            Core.Graphics.ApplyChanges();

            // Recompute virtual resolution scaling
            Core.VirtualResolution.ComputeScale(Core.GraphicsDevice);

            // Update Gum canvas to match virtual resolution
            GraphicalUiElement.CanvasWidth = Core.VirtualResolution.VirtualWidth;
            GraphicalUiElement.CanvasHeight = Core.VirtualResolution.VirtualHeight;
            GumService.Default.Root?.UpdateLayout();

            // Update Gum cursor transform
            var cursor = GumService.Default.Cursor;
            float scale = Core.VirtualResolution.Scale;
            cursor.TransformMatrix = Matrix.CreateScale(1f / scale, 1f / scale, 1f) *
                                     Matrix.CreateTranslation(
                                        -Core.VirtualResolution.DestinationRect.X / scale,
                                        -Core.VirtualResolution.DestinationRect.Y / scale,
                                        0
                                     );

            Core.Audio.PlaySoundEffect(_uiSound);
        }


        private void FullScreenChanged(object sender, EventArgs e) {
            bool fullscreen = FullScreenCheckbox.IsChecked ?? false;

            if (fullscreen) {
                // Save the current windowed resolution only if we're not already fullscreen
                if (!Core.Graphics.IsFullScreen) {
                    _previousResolution = new Point(Core.Graphics.PreferredBackBufferWidth, Core.Graphics.PreferredBackBufferHeight);
                }

                // Apply fullscreen mode with monitor resolution
                var mode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
                ApplyResolution(mode.Width, mode.Height, true);

                _isApplyingResolutionProgrammatically = true;
                ResolutionBox.SelectedObject = $"{mode.Width} x {mode.Height}";
                _isApplyingResolutionProgrammatically = false;

                ResolutionBox.IsEnabled = false;
            }
            else {
                // Restore last windowed resolution
                if (_previousResolution != Point.Zero) {
                    ApplyResolution(_previousResolution.X, _previousResolution.Y, false);

                    _isApplyingResolutionProgrammatically = true;
                    ResolutionBox.SelectedObject = $"{_previousResolution.X} x {_previousResolution.Y}";
                    _isApplyingResolutionProgrammatically = false;
                }

                ResolutionBox.IsEnabled = true;
            }

            SaveFileManager.Set(d =>
            {
                d.IsFullscreen = fullscreen;

                if (!fullscreen) {
                    d.ResolutionWidth = _previousResolution.X;
                    d.ResolutionHeight = _previousResolution.Y;
                }
                else {
                    var mode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
                    d.ResolutionWidth = mode.Width;
                    d.ResolutionHeight = mode.Height;
                }
            });

        }

        private void MusicSliderChanged(object sender, EventArgs e) {
            float ratio = MusicSlider.SliderPercent / 100;
            Core.Audio.SongVolume = ratio;
            SaveFileManager.Set(d => d.MusicVolume = ratio);
        }

        private void SoundSliderChanged(object sender, EventArgs e) {
            float ratio = SoundSlider.SliderPercent / 100;
            Core.Audio.SoundEffectVolume = ratio;
            SaveFileManager.Set(d => d.SoundVolume = ratio);
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
