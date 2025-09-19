using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RIPRUSH.Entities {
    /// <summary>
    /// The animation manager is responsible for handling the playback and rendering of an animation.
    /// </summary>
    public class AnimationManager {

        /// <summary>
        /// A timer used to track the time elapsed for frame updates.
        /// </summary>
        private float _timer;

        /// <summary>
        /// A flag to indicate whether the animation is playing.
        /// </summary>
        private bool _isPlaying;

        /// <summary>
        /// The animation being managed.
        /// </summary>
        public Animation animation;

        /// <summary>
        /// The position of the animation on the screen.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Initializes the AnimationManager with a given animation.
        /// </summary>
        public AnimationManager(Animation animation) {
            this.animation = animation;
            _isPlaying = animation != null; // Start playing if the animation is not null
            _timer = 0f;
        }

        /// <summary>
        /// Plays the specified animation from the beginning.
        /// </summary>
        /// <param name="animation">The animation to be played</param>
        public void Play(Animation animation) {
            if (this.animation == animation && _isPlaying)
                return;

            this.animation = animation;
            this.animation.CurrentFrame = 0;
            _timer = 0f;
            _isPlaying = true;
        }

        /// <summary>
        /// Stops the animation and resets it to the first frame.
        /// </summary>
        public void Stop() {
            _timer = 0f;
            if (animation != null) {
                animation.CurrentFrame = 0;
                _isPlaying = false;
            }
        }

        /// <summary>
        /// Draws the animation's current frame to the screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance used to draw textures and sprites to the screen.</param>
        public void Draw(SpriteBatch spriteBatch) {
            if (animation == null || !_isPlaying)
                return; // Don't draw if there's no animation or it's stopped

            spriteBatch.Draw(
                animation.Texture,
                Position, 
                new Rectangle(
                    animation.CurrentFrame * animation.FrameWidth, 
                    0, 
                    animation.FrameWidth, 
                    animation.FrameHeight
                ),
                animation.Color, 
                animation.Rotation, 
                animation.Origin,
                animation.Scale,
                animation.SpriteEffect,
                animation.LayerDepth
            );
        }

        /// <summary>
        /// The animation manager's update logic
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        public void Update(GameTime gameTime) {
            if (animation == null || !_isPlaying)
                return; // Don't update if there's no animation or it's stopped

            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer > animation.FrameSpeed) {
                _timer = 0f;
                animation.CurrentFrame++;
                if (animation.CurrentFrame >= animation.FrameCount) {
                    if (animation.IsLooping)
                        animation.CurrentFrame = 0;
                    else
                        animation.CurrentFrame = animation.FrameCount - 1;
                }
            }
        }

    }
}
