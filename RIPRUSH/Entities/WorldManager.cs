using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RIPRUSH.Entities.Actors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RIPRUSH.Entities {
    public class WorldManager {

        private List<Platform> _chunks = new();
        private Random _rng = new();

        private float _baseY;
        private const float InitialScale = 2f;

        private const float OverlapFix = 1f;

        private float _scrollSpeed = 400f;
        public float TotalScrollX { get; private set; } = 0f;
        private float _timeElapsed = 0f;

        private float _screenWidth;
        private float _chunkWidth;

        private const float MinGapWidth = 100f;
        private const float MaxGapWidth = 300f;
        private int _maxConsecutiveGaps = 2;
        private int _consecutiveGaps = 0;
        private float _gapChance = 0.25f;


        public WorldManager(float baseY, GraphicsDevice graphics) {
            _baseY = baseY;
            _screenWidth = graphics.Viewport.Width;
        }

        public List<Platform> GetActivePlatforms() =>
            _chunks.Where(c => c.IsActive).ToList();

        private bool IsOffscreen(Platform chunk) {
            return chunk.Position.X + chunk.Bounds.Width < 0;
        }

        private float GetRightmostX() {
            return _chunks.Max(c => c.Position.X + c.Bounds.Width);
        }

        private void MoveChunk(Platform chunk, float dt) {
            chunk._position.X -= _scrollSpeed * dt;
        }

        private void RecycleChunk(Platform chunk, float rightMostX) {
            bool makeGap = ShouldMakeGap();

            float gapWidth = makeGap ? _rng.Next((int)MinGapWidth, (int)MaxGapWidth) : 0f;
            float spawnX = Math.Max(rightMostX, _screenWidth) + gapWidth - OverlapFix;

            chunk._position.X = spawnX;
            chunk.IsActive = !makeGap;
        }

        private bool ShouldMakeGap() {
            bool makeGap = _rng.NextDouble() < _gapChance;

            if (_consecutiveGaps >= _maxConsecutiveGaps)
                makeGap = false;

            _consecutiveGaps = makeGap ? _consecutiveGaps + 1 : 0;

            return makeGap;
        }

        private void UpdateGapChance() {
            // Increases over time, capped at 80%
            _gapChance = MathHelper.Clamp(0.4f + _timeElapsed * 0.01f, 0.4f, 0.8f);
        }

        private Platform CreatePlatform(ContentManager content, float x) {
            var platform = new Platform(content, InitialScale, new Vector2(x, _baseY)) {
                Color = Color.DarkGreen,
                IsActive = true
            };
            platform.SetBounds();
            return platform;
        }

        public void Initialize(ContentManager content, int chunkCount) {
            // Create initial continuous platform sequence
            for (int i = 0; i < chunkCount; i++) {
                var platform = CreatePlatform(content, i * _chunkWidth);
                if (i == 0) {
                    platform.SetBounds();
                    _chunkWidth = platform.Bounds.Width;
                }
                _chunks.Add(platform);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            foreach (var chunk in _chunks) {
                if (chunk.IsActive)
                    chunk.Draw(gameTime, spriteBatch);
            }
        }

        public void Update(GameTime gameTime) {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _timeElapsed += dt;

            UpdateGapChance();

            // accumulate how much the world moved this frame
            TotalScrollX += _scrollSpeed * dt;

            foreach (var chunk in _chunks) {
                MoveChunk(chunk, dt);

                if (IsOffscreen(chunk)) {
                    float rightMostX = GetRightmostX();
                    RecycleChunk(chunk, rightMostX);
                }

                chunk.SetBounds();
            }
        }

    }
}