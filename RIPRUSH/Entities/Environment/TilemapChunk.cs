using CustomTilemapPipeline;
using RIPRUSH.Entities.CollisionShapes;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RIPRUSH.Entities.Environment {
    public class TilemapChunk {
        public TilemapData Data { get; }
        public Vector2 Position;
        public bool IsActive = true;

        private readonly List<BoundingRectangle> _collisionRects = new();

        public TilemapChunk(TilemapData data, Vector2 position) {
            Data = data;
            Position = position;

            // Build collision rects so we can stand on em
            foreach (var rect in Data.CollisionRectangles) {
                var worldRect = new BoundingRectangle(
                    rect.X + Position.X,
                    rect.Y + Position.Y,
                    rect.Width,
                    rect.Height
                );
                _collisionRects.Add(worldRect);
            }
        }

        public List<Platform> GetCollisionPlatforms() {
            var platforms = new List<Platform>();
            foreach (var rect in _collisionRects) {
                platforms.Add(new Platform(rect.X, rect.Y, rect.Width, rect.Height));
            }
            return platforms;
        }

        public void UpdateCollisionPositions() {
            // Update rectangles to follow chunk position????
            _collisionRects.Clear();
            foreach (var rect in Data.CollisionRectangles) {
                _collisionRects.Add(new BoundingRectangle(
                    rect.X + Position.X,
                    rect.Y + Position.Y,
                    rect.Width,
                    rect.Height
                ));
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D tileset) {
            if (Data.Tiles == null) return;

            for (int y = 0; y < Data.Height; y++) {
                for (int x = 0; x < Data.Width; x++) {
                    int tileId = Data.Tiles[x, y];
                    
                    if (tileId <= 0) continue;

                    Vector2 pos = Position + new Vector2(
                        x * Data.TileWidth,
                        y * Data.TileHeight
                    );

                    int tilesetCols = tileset.Width / Data.TileWidth;
                    int tileIndex = tileId - 1;
                    int tileX = tileIndex % tilesetCols;
                    int tileY = tileIndex / tilesetCols;

                    Rectangle source = new(tileX * Data.TileWidth, tileY * Data.TileHeight, Data.TileWidth, Data.TileHeight);
                    spriteBatch.Draw(tileset, pos, source, Color.White);
                }
            }
        }
    }
}
