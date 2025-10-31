using CustomTilemapPipeline;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RIPRUSH.Entities.Actors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RIPRUSH.Entities.Environment {
    public class WorldManager {

        private Random _rng = new();

        private float _baseY;
        private const float InitialScale = 2f;

        private const float OverlapFix = 1f;

        private float _scrollSpeed = 400f;
        public float TotalScrollX { get; private set; } = 0f;
        private float _timeElapsed = 0f;

        private float _screenWidth;
        private float _chunkWidth;

        private const float MinGapWidth = 0f;
        private const float MaxGapWidth = 200f;
        private int _maxConsecutiveGaps = 1;
        private int _consecutiveGaps = 0;
        private float _gapChance = 0.35f;

        public List<TilemapChunk> _chunks = new();
        private Texture2D _tileset;


        public WorldManager(float baseY, GraphicsDevice graphics) {
            _baseY = baseY;
            _screenWidth = graphics.Viewport.Width;
        }

        public List<Platform> GetActivePlatforms() {
            var platforms = new List<Platform>();
            foreach (var chunk in _chunks) {
                platforms.AddRange(chunk.GetCollisionPlatforms());
            }
            return platforms;
        }

        private bool ShouldMakeGap() {
            bool makeGap = _rng.NextDouble() < _gapChance;

            if (_consecutiveGaps >= _maxConsecutiveGaps)
                makeGap = false;

            _consecutiveGaps = makeGap ? _consecutiveGaps + 1 : 0;

            return makeGap;
        }

        private void UpdateGapChance() {
            // Every 20,000 pixels scrolled increases gap chance by 1%.
            _gapChance = MathHelper.Clamp(0.35f + TotalScrollX / 20000f, 0.35f, 0.8f);
        }


        public void Initialize(ContentManager content, string tilemapAsset, int chunkCount) {
            _tileset = content.Load<Texture2D>("Assets/World/nature-paltformer-tileset-16x16");

            TilemapData tilemapData = content.Load<TilemapData>(tilemapAsset);
            float chunkWidth = tilemapData.Width * tilemapData.TileWidth;

            for (int i = 0; i < chunkCount; i++) {
                Vector2 pos = new Vector2(i * chunkWidth, _baseY - tilemapData.Height * tilemapData.TileHeight);
                _chunks.Add(new TilemapChunk(tilemapData, pos));
            }
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            foreach (var chunk in _chunks) {
                if (chunk.IsActive)
                    chunk.Draw(spriteBatch, _tileset);
            }
        }

        public void Update(GameTime gameTime) {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _timeElapsed += dt;

            UpdateGapChance();
            TotalScrollX += _scrollSpeed * dt;

            foreach (var chunk in _chunks) {
                chunk.Position.X -= _scrollSpeed * dt;

                if (chunk.Position.X + chunk.Data.Width * chunk.Data.TileWidth < 0) {
                    float rightMostX = _chunks.Max(c => c.Position.X + c.Data.Width * c.Data.TileWidth);

                    bool makeGap = ShouldMakeGap();
                    float gapWidth = makeGap ? _rng.Next((int)MinGapWidth, (int)MaxGapWidth) : 0f;

                    chunk.Position.X = rightMostX + gapWidth;
                    chunk.IsActive = !makeGap;
                }

                chunk.UpdateCollisionPositions();
            }

        }


    }
}