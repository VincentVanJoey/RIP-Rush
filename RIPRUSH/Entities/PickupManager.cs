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

        private float _baseSpawnInterval = 5f; // seconds(?)
        private float _timeSinceLastSpawn = 0f;
        private float _scrollSpeed;

        // Spawn position range
        private readonly float _minY = 100f;
        private readonly float _maxY = 420f;
        private readonly float _spawnXOffset = 250f;

        // Sound effects
        public SoundEffect basicCandySound;
        public SoundEffect healCandySound;
        public SoundEffect zapCandySound;


        public Model lolipopModel;
        public Model zapcandyModel;
        public Model chocobarModel;

        private Texture2D lollipop_texture;
        private Texture2D zapcandy_texture;
        private Texture2D chocobar_texture;

        public PickupManager(GraphicsDevice graphics, float scrollSpeed) {
            _graphics = graphics;
            _scrollSpeed = scrollSpeed;
        }

        public IEnumerable<CandyPickup> GetPickups() => _pickups;

        public void SetScrollSpeed(float newSpeed) => _scrollSpeed = newSpeed;

        public void LoadContent(ContentManager content) {
            // Pickup sfx here
            basicCandySound = content.Load<SoundEffect>("Assets/Audio/Candy");
            healCandySound = content.Load<SoundEffect>("Assets/Audio/HealCandy");
            zapCandySound = content.Load<SoundEffect>("Assets/Audio/ZapCandy");

            // Pickup models here
            // speedup
            lolipopModel = content.Load<Model>("Assets/Models/joetree01");
            zapcandyModel = content.Load<Model>("Assets/Models/zapcandy");
            chocobarModel = content.Load<Model>("Assets/Models/chocobar");
            // mark grayson

            //Pickup model textures
            lollipop_texture = Core.Content.Load<Texture2D>("Assets/Models/lollipoptexture");
            zapcandy_texture = Core.Content.Load<Texture2D>("Assets/Models/zapcandytexture");
            chocobar_texture = Core.Content.Load<Texture2D>("Assets/Models/chocobartexture");
        }

        private void SpawnPickup() {
            // Randomly pick a type, weighted for rarity
            CandyType type;
            Model model;
            Texture2D texture;
            float roll = _rng.NextSingle();

            if (roll < 0.7f) {
                type = CandyType.Chocobar;
            }
            else if (roll < 0.9f) {
                type = CandyType.Lollipop;
            }
            else {
                type = CandyType.ZapCandy;
            }

            switch (type) {
                case CandyType.ZapCandy:
                    model = zapcandyModel;
                    texture = zapcandy_texture;
                    break;
                case CandyType.Lollipop:
                    model = lolipopModel;
                    texture = lollipop_texture;
                    break;
                default:
                    model = chocobarModel;
                    texture = chocobar_texture;
                    break;
            }

            float viewportWidth = Core.GraphicsDevice.Viewport.Width;
            float x = viewportWidth + _spawnXOffset + (float)(_rng.NextDouble() * 200);
            float y = MathHelper.Lerp(_minY, _maxY, (float)_rng.NextDouble());
            float z = 0f;

            var pickup = new CandyPickup(_graphics, type, new Vector3(x, y, z), model, texture);

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


        public void Update(GameTime gameTime) {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _timeSinceLastSpawn += dt;

            // Adjust spawn interval based on scroll speed (faster = slightly quicker spawns)
            float adjustedSpawnInterval = MathHelper.Clamp(
                _baseSpawnInterval * (20f / (_scrollSpeed + 1f)),
                4f, 8f
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

        public void Draw(Matrix view, Matrix projection) {
            _graphics.RasterizerState = new RasterizerState { CullMode = CullMode.None };
            foreach (var p in _pickups) {
                p.Draw(view, projection);
            }
        }

    }
}
