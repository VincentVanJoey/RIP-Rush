using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using System;
using System.Collections.Generic;

namespace RIPRUSH.Entities.Environment {
    public class PickupManager {
        private readonly GraphicsDevice _graphics;
        private readonly Random _rng = new();
        private readonly List<CandyPickup> _pickups = new();

        private float _baseSpawnInterval = 8f; // seconds(?)
        private float _timeSinceLastSpawn = 0f;
        private float _scrollSpeed;

        // Spawn position range
        private readonly float _minY = 220f;
        private readonly float _maxY = 420f;
        private readonly float _spawnXOffset = 250f;

        // Sound effects
        public SoundEffect basicCandySound;
        public SoundEffect healCandySound;
        public SoundEffect zapCandySound;

        public PickupManager(GraphicsDevice graphics, float scrollSpeed) {
            _graphics = graphics;
            _scrollSpeed = scrollSpeed;
        }

        public IEnumerable<CandyPickup> GetPickups() => _pickups;

        public void SetScrollSpeed(float newSpeed) => _scrollSpeed = newSpeed;

        public void LoadContent(ContentManager content) {
            basicCandySound = content.Load<SoundEffect>("Assets/Audio/Candy");
            healCandySound = content.Load<SoundEffect>("Assets/Audio/HealCandy");
            zapCandySound = content.Load<SoundEffect>("Assets/Audio/ZapCandy");
        }

        public void Update(GameTime gameTime) {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _timeSinceLastSpawn += dt;

            // Adjust spawn interval based on scroll speed (faster = slightly quicker spawns)
            float adjustedSpawnInterval = MathHelper.Clamp(
                _baseSpawnInterval * (60f / (_scrollSpeed + 1f)),
                15f, 40f
            );

            // Scroll all pickups left
            foreach (var p in _pickups) {
                p.Position.X -= _scrollSpeed * dt;
            }

            // Remove offscreen or collected pickups
            _pickups.RemoveAll(p => p.Position.X < -200f || p.Collected);

            // Spawn occasionally and unpredictably
            if (_timeSinceLastSpawn >= adjustedSpawnInterval + _rng.NextSingle() * 10f) {
                SpawnPickup();
                _timeSinceLastSpawn = 0f;
            }

            foreach (var p in _pickups) { 
                p.Update(gameTime); 
            }
                
        }

        private void SpawnPickup() {
            // Randomly pick a type, weighted for rarity
            CandyType type;
            float roll = _rng.NextSingle();

            if (roll < 0.5f) {
                type = CandyType.Chocobar;
            }
            else if (roll < 0.8f) {
                type = CandyType.Lollipop;
            }
            else {
                type = CandyType.ZapCandy;
            }

            float viewportWidth = Core.GraphicsDevice.Viewport.Width;
            float x = viewportWidth + _spawnXOffset + (float)(_rng.NextDouble() * 200);
            float y = MathHelper.Lerp(_minY, _maxY, (float)_rng.NextDouble());
            float z = 0f;

            var pickup = new CandyPickup(_graphics, type, new Vector3(x, y, z));

            switch (type) {
                case CandyType.ZapCandy:
                    pickup.PickupSound = zapCandySound;
                    break;
                case CandyType.Lollipop:
                    pickup.PickupSound = healCandySound;
                    break;
                default:
                    pickup.PickupSound = basicCandySound;
                    break;
            }

            _pickups.Add(pickup);
        }

        public void Draw(Matrix view, Matrix projection) {
            _graphics.RasterizerState = new RasterizerState { CullMode = CullMode.None };
            foreach (var p in _pickups) {
                p.Draw(view, projection);
            }
        }

    }
}
