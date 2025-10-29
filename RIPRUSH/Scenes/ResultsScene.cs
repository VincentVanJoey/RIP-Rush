using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;
using RIPRUSH.Components.Joelements;
using RIPRUSH.Entities;
using RIPRUSH.Entities.Actors;
using System;
using System.Collections.Generic;
namespace RIPRUSH.Scenes {

    public class ResultsScene : Scene {

        private Texture2D _gradientTexture;
        private UFO _ufo_left;
        private UFO _ufo_right;

        private ResultMenu _resultMenu;

        private SpriteFont _youdied;
        private SpriteFont _subdied;

        public float _finalDistance;      // final distance traveled in the current run
        public bool _wasNewHighScore;     // whether or not it was a new high score

        private float _blinkTimer = 0f;
        private Color _blinkColor = Color.White;


        private List<Component> _components;
        private float _rotation = 0f;

        public override void Initialize() {
            base.Initialize();

            Core.Audio.PauseAudio();

            Core.ExitOnEscape = true;

            _gradientTexture = new Texture2D(Core.GraphicsDevice, 1, 2);

            _ufo_left = new UFO(Core.Content, true, 3f, new Vector2(100, 250));
            _ufo_left.isFrozen = true;

            _ufo_right = new UFO(Core.Content, true, 3f, new Vector2(700, 250));
            _ufo_right.isFrozen = true;

            _components = new List<Component>() { _ufo_left, _ufo_right};

            GumService.Default.Root.Children.Clear();
            _resultMenu = new ResultMenu();
            _resultMenu.AddToRoot();
        }

        public override void LoadContent() {
            _subdied = Core.Content.Load<SpriteFont>("fonts/coralines-cat");
            _youdied = Core.Content.Load<SpriteFont>("fonts/october-crow");
        }
        public override void Draw(GameTime gameTime) {

            // Draw gradient background stretched to screen
            Core.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearClamp);
            Core.SpriteBatch.Draw(
                _gradientTexture,
                new Rectangle(0, 0, Core.GraphicsDevice.Viewport.Width, Core.GraphicsDevice.Viewport.Height),
                Color.White
            );
            Core.SpriteBatch.End();

            // Draw everything else on the screen
            Core.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            string mainwords = ("YOU DIED");
            string scorewords = _wasNewHighScore ? $"NEW HIGH SCORE RUSH: {_finalDistance:F0}m !" : $"You Rushed: {_finalDistance:F0}m";
            Vector2 mainSize = _youdied.MeasureString(mainwords);
            Vector2 subSize = _subdied.MeasureString(scorewords);


            Core.SpriteBatch.DrawString(_youdied, mainwords, new Vector2((Core.GraphicsDevice.Viewport.Width - mainSize.X) / 2f - 20, (Core.GraphicsDevice.Viewport.Height - mainSize.Y) / 2f - 100), Color.White);
            Core.SpriteBatch.DrawString(_subdied, scorewords, new Vector2((Core.GraphicsDevice.Viewport.Width - subSize.X) / 2f, (Core.GraphicsDevice.Viewport.Height - mainSize.Y) / 2f), _blinkColor);

            foreach (var component in _components) {
                component.Draw(gameTime, Core.SpriteBatch);
            }

            Core.SpriteBatch.End();
        }

        public override void Update(GameTime gameTime) {

            float time = (float)gameTime.TotalGameTime.TotalSeconds;
            float t = (MathF.Sin(time) + 1f) / 2f; // 0 -> 1
            Color topColor = Color.Lerp(Color.DarkRed, Color.Black, t);
            Color bottomColor = Color.Lerp(Color.Black, Color.DarkRed, t);

            _resultMenu.UpdateInput();

            _gradientTexture.SetData(new Color[] { topColor, bottomColor });

            _rotation += 2f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _ufo_right.Rotation = -_rotation;
            _ufo_left.Rotation = _rotation;

            if (_wasNewHighScore) {
                _blinkTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_blinkTimer >= 0.3f) {
                    _blinkColor = (_blinkColor == Color.White) ? Color.LightSkyBlue : Color.White;
                    _blinkTimer = 0f;
                }
            }
            else {
                _blinkColor = Color.White; // normal color if not high score
            }

            foreach (var component in _components) {
                component.Update(gameTime);
            }

        }


    }
}
