using Microsoft.Xna.Framework.Content.Pipeline;
using System.Text.Json;

namespace CustomTilemapPipeline {

    [ContentProcessor(DisplayName = "Tiled Map Processor")]
    public class TiledMapProcessor : ContentProcessor<string, TilemapData> {

        public override TilemapData Process(string input, ContentProcessorContext context) {

            using var doc = JsonDocument.Parse(input);
            var root = doc.RootElement;

            int width = root.GetProperty("width").GetInt32();
            int height = root.GetProperty("height").GetInt32();
            int tileWidth = root.GetProperty("tilewidth").GetInt32();
            int tileHeight = root.GetProperty("tileheight").GetInt32();

            var tilemap = new TilemapData(width, height, tileWidth, tileHeight);

            foreach (var layer in root.GetProperty("layers").EnumerateArray()) {
                string type = layer.GetProperty("type").GetString();

                // Collision layer -- needs more testing I think, super dirty solutions oplenty here
                if (type == "objectgroup" && layer.GetProperty("name").GetString() == "Collisions") {
                    foreach (var obj in layer.GetProperty("objects").EnumerateArray()) {
                        float x = obj.GetProperty("x").GetSingle();
                        float y = obj.GetProperty("y").GetSingle();
                        float w = obj.GetProperty("width").GetSingle();
                        float h = obj.GetProperty("height").GetSingle();

                        tilemap.CollisionRectangles.Add(new RectData(x, y, w, h));
                    }
                }

                // Tile layer
                if (type == "tilelayer" && layer.GetProperty("name").GetString() == "ground") {
                    int i = 0, j = 0;
                    tilemap.Tiles = new int[width, height];
                    foreach (var tile in layer.GetProperty("data").EnumerateArray()) {
                        tilemap.Tiles[i, j] = tile.GetInt32();
                        i++;
                        if (i >= width) { i = 0; j++; }
                    }
                }
            }

            return tilemap;
        }
    }
}
