using Microsoft.Xna.Framework.Content.Pipeline;
using System.IO;

namespace CustomTilemapPipeline {
    [ContentImporter(".json", DisplayName = "Tiled JSON Importer", DefaultProcessor = "TiledMapProcessor")]
    public class TiledJsonImporter : ContentImporter<string> {
        public override string Import(string filename, ContentImporterContext context) {
            return File.ReadAllText(filename);
        }
    }
}
