using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RIPRUSH.Sprites {
    public class AnimationManager {
        
        public Animation animation;

        private float timer;

        public Vector2 Position { get; set; }

        public AnimationManager(Animation animation) {
            this.animation = animation;
        }

        public void Play(Animation animation) {
            if (this.animation == animation)
                return;
            this.animation = animation;
            this.animation.CurrentFrame = 0;
            timer = 0;
        }

        public void Stop() {
            timer = 0f;
            animation.CurrentFrame = 0;
        }

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
                animation.rotation, 
                animation.Origin,
                animation.scale,
                animation.spriteEffect,
                animation.layerDepth
            );
        }

        public void Update(GameTime gameTime) {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer > animation.FrameSpeed) {
                timer = 0f;
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
