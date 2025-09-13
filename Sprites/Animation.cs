using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RIPRUSH.Sprites {
    /// <summary>
    /// A class representing a 2D animation using a spritesheet.
    /// </summary>
    public class Animation {


        /// <summary>
        /// The current frame of the animation being displayed.
        /// </summary>
        public int CurrentFrame { get; set; }


        /// <summary>
        /// The number of frames in the animation
        /// </summary>
        public int FrameCount { get; private set; }

        /// <summary>
        /// The height of a single frame in the animation, which is equal to the total texture height.
        /// </summary>
        public int FrameHeight { get { return Texture.Height; } }

        /// <summary>
        /// The speed of the animation, defined as the time (in seconds) each frame is displayed.
        /// </summary>
        public float FrameSpeed { get; set; }

        /// <summary>
        /// The width of a single frame in the animation, calculated by dividing the total texture width by the number of frames.
        /// </summary>
        public int FrameWidth { get { return Texture.Width / FrameCount; } }

        /// <summary>
        /// Whether the animation is supposed to loop or stop at the last frame.
        /// </summary>
        public bool IsLooping { get; set; }

        /// <summary>
        /// The animation's spritesheet texture.
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// The color used to tint the animation. Default is Color.White (no tint).
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// The rotation of the animation in radians.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// The origin of rotation for the animation
        /// </summary>
        public Vector2 Origin { get; set; }

        /// <summary>
        /// The scale of the animation
        /// </summary>
        public float Scale { get; set; }


        /// <summary>
        /// The sprite effects to apply to the animation (e.g., flip horizontally or vertically).
        /// </summary>
        public SpriteEffects SpriteEffect { get; set; }

        /// <summary>
        /// The layer depth of the animation, used for rendering order (0.0f = front, 1.0f = back).
        /// </summary>
        public float LayerDepth { get; set; }

        public Animation( Texture2D texture, int frameCount, bool isLooping = true, Color? color = null, Vector2? origin = null,
            float rotation = 0.0f, float scale = 1.0f, SpriteEffects spriteEffect = SpriteEffects.None, float frameSpeed = 0.1f, float layerDepth = 0.0f) {

            Texture = texture;
            Color = color ?? Color.White;
            Origin = origin ?? Vector2.Zero;
            this.Rotation = rotation;
            this.Scale = scale;
            this.SpriteEffect = spriteEffect;
            this.LayerDepth = layerDepth;
            FrameCount = frameCount;
            FrameSpeed = frameSpeed;
            IsLooping = isLooping;

        }
    }
}
