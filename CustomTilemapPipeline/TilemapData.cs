using System.Collections.Generic;

namespace CustomTilemapPipeline {

    public class TilemapData {

        public int Width { get; set; }
        public int Height { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }

        // Plain rectangles(?) for collisions
        public List<RectData> CollisionRectangles { get; set; } = new();

        // tile IDs for rendering -- not sure if I want to make more use of this later
        public int[,] Tiles { get; set; }

        // Name of the tileset texture
        public string TilesetTexture { get; set; } = "Assets/World/nature-paltformer-tileset-16x16";

        public TilemapData() { } // required for pipeline????

        public TilemapData(int width, int height, int tileWidth, int tileHeight) {
            Width = width;
            Height = height;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            Tiles = new int[width, height];
        }
    }

    /// <summary>
    /// Need this so I can pass some of the data between projects
    /// </summary>
    public struct RectData {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public RectData(float x, float y, float width, float height) {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
