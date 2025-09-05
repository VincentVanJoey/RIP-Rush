using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RIPRUSH.Sprites {
    public class Animation {

        public int CurrentFrame { get; set; }

        public int FrameCount { get; private set; }

        public int FrameHeight { get { return Texture.Height; } }

        public float FrameSpeed { get; set; }

        public int FrameWidth { get { return Texture.Width / FrameCount; } }

        public bool IsLooping { get; set; } 

        public Texture2D Texture { get; set; }

        public Color Color { get; set; }

        public float rotation { get; set; }

        public Vector2 Origin { get; set; }

        public float scale { get; set; }

        public SpriteEffects spriteEffect { get; set; }

        public float layerDepth { get; set; }

        public Animation( Texture2D texture, int frameCount, bool isLooping = true, Color? color = null, Vector2? origin = null,
            float rotation = 0.0f, float scale = 1.0f, SpriteEffects spriteEffect = SpriteEffects.None, float frameSpeed = 0.1f, float layerDepth = 0.0f) {

            Texture = texture;
            Color = color ?? Color.White;
            Origin = origin ?? Vector2.Zero;
            this.rotation = rotation;
            this.scale = scale;
            this.spriteEffect = spriteEffect;
            this.layerDepth = layerDepth;
            FrameCount = frameCount;
            FrameSpeed = frameSpeed;
            IsLooping = isLooping;

        }
    }
}
