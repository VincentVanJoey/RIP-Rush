using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGameLibrary {
    public class VirtualResolutionManager {
        public readonly int VirtualWidth;
        public readonly int VirtualHeight;

        private RenderTarget2D _target;

        public float Scale { get; private set; }
        public Rectangle DestinationRect { get; private set; }

        public VirtualResolutionManager(GraphicsDevice device, int width, int height) {
            VirtualWidth = width;
            VirtualHeight = height;

            _target = new RenderTarget2D(
                device,
                VirtualWidth,
                VirtualHeight,
                false,
                SurfaceFormat.Color,
                DepthFormat.None
            );

            ComputeScale(device);
        }

        public void ComputeScale(GraphicsDevice device) {
            var vp = device.Viewport;

            float scaleX = vp.Width / (float)VirtualWidth;
            float scaleY = vp.Height / (float)VirtualHeight;

            Scale = MathF.Min(scaleX, scaleY);

            int outWidth = (int)(VirtualWidth * Scale);
            int outHeight = (int)(VirtualHeight * Scale);

            DestinationRect = new Rectangle(
                (vp.Width - outWidth) / 2,
                (vp.Height - outHeight) / 2,
                outWidth,
                outHeight
            );
        }

        public void BeginDraw(GraphicsDevice device) {
            device.SetRenderTarget(_target);
            device.Clear(Color.Black);
        }

        public void EndDraw(GraphicsDevice device, SpriteBatch spriteBatch) {
            device.SetRenderTarget(null);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(_target, DestinationRect, Color.White);
            spriteBatch.End();
        }
    }

}