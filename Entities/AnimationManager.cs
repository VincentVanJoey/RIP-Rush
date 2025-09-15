using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RIPRUSH.Entities {
    /// <summary>
    /// The animation manager is responsible for handling the playback and rendering of an animation.
    /// </summary>
    public class AnimationManager {

        /// <summary>
        /// The animation being managed.
        /// </summary>
        public Animation animation;


        /// <summary>
        /// A timer used to track the time elapsed for frame updates.
        /// </summary>
        private float _timer;

        /// <summary>
        /// The position of the animation on the screen.
        /// </summary>
        public Vector2 Position { get; set; }

        public AnimationManager(Animation animation) {
            this.animation = animation;
        }

        /// <summary>
        /// Plays the specified animation from the beginning.
        /// </summary>
        /// <param name="animation">The animation to be played</param>
        public void Play(Animation animation) {
            if (this.animation == animation)
                return;
            this.animation = animation;
            this.animation.CurrentFrame = 0;
            _timer = 0;
        }

        /// <summary>
        /// Stops the animation and resets it to the first frame.
        /// </summary>
        public void Stop() {
            _timer = 0f;
            animation.CurrentFrame = 0;
        }

        /// <summary>
        /// Draws the animation's current frame to the screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the game's timing state, used to synchronize rendering with the game's update loop.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance used to draw textures and sprites to the screen.</param>
        public void Draw(SpriteBatch spriteBatch) {
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
