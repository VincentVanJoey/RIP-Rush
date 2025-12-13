using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGameLibrary;
using MonoGameLibrary.Input;
using MonoGameLibrary.Scenes;
using RIPRUSH.Entities;

namespace RIPRUSH.Scenes {

    public class SplashScene : Scene {
        private enum State {
            FadeIn,
            Playing,
            FadeOut
        }
        private State _state = State.FadeIn;

        private Texture2D _splashImage;
        private static Texture2D _blackPixel;

        private float _fadeAlpha = 1f;
        private float _timer = 0f;
        private float _audioDuration;

        private const float FadeDuration = 1.0f;

        private SoundEffect _introAudio;

        public override void LoadContent() {
            _splashImage = Content.Load<Texture2D>("Assets/Splash/Intrjoe");
            _introAudio = Content.Load<SoundEffect>("Assets/Splash/IntrjoeAudio");

            // volume from saved if available
            if (SaveFileManager.Data != null) {
                Core.Audio.SoundEffectVolume = SaveFileManager.Data.SoundVolume;
            }
            else {
                Core.Audio.SoundEffectVolume = 1f;
            }

            Core.Audio.PlaySoundEffect(_introAudio);

            _audioDuration = (float)_introAudio.Duration.TotalSeconds;
        }

        public override void Update(GameTime gameTime) {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _timer += dt;

            Core.Audio.Update();

            switch (_state) {

                case State.FadeIn:
                    _fadeAlpha -= dt / FadeDuration;
                    if (_fadeAlpha <= 0f) {
                        _fadeAlpha = 0f;
                        _state = State.Playing;
                        _timer = 0f;
                    }
                    break;

                case State.Playing:
                    if (_timer >= _audioDuration - FadeDuration) {
                        _state = State.FadeOut;
                        _timer = 0f;
                    }
                    break;

                case State.FadeOut:
                    _fadeAlpha += dt / FadeDuration;
                    if (_fadeAlpha >= 1f) {
                        _fadeAlpha = 1f;
                        Core.ChangeScene(new MainMenuScene());
                    }
                    break;
            }

            // skip option
            if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Escape)
                || Core.Input.Keyboard.WasKeyJustPressed(Keys.Space)
                || Core.Input.Mouse.WasButtonJustPressed(MouseButton.Left)) {
                Core.ChangeScene(new MainMenuScene());
            }
        }

        public override void Draw(GameTime gameTime) {
            Core.SpriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp
            );

            Core.SpriteBatch.Draw(
                _splashImage,
                Core.GraphicsDevice.Viewport.Bounds,
                Color.White
            );

            if (_fadeAlpha > 0f) {
                Core.SpriteBatch.Draw(
                    GetBlackPixel(),
                    Core.GraphicsDevice.Viewport.Bounds,
                    Color.Black * _fadeAlpha
                );
            }

            Core.SpriteBatch.End();
        }

        private Texture2D GetBlackPixel() {
            if (_blackPixel == null) {
                _blackPixel = new Texture2D(Core.GraphicsDevice, 1, 1);
                _blackPixel.SetData(new[] { Color.White });
            }
            return _blackPixel;
        }
    }
}
