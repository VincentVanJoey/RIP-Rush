using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using RIPRUSH.Entities.CollisionShapes;

namespace RIPRUSH.Entities.Environment {
    public enum CandyType { Lollipop, Chocobar, ZapCandy }

public class CandyPickup
    {
        public Vector3 Position;
        public bool Collected;
        public CandyType Type;
        public SoundEffect PickupSound;

        public BoundingCircle Bounds;
        private const float CollisionRadius = 30f;

        private Model _model;
        private Texture2D _texture;
        private Matrix _world;

        public CandyPickup(GraphicsDevice graphics, CandyType type, Vector3 position, Model model, Texture2D texture){
            Type = type;
            Position = position;

            Bounds = new BoundingCircle(new Vector2(position.X, position.Y), CollisionRadius);
            _model = model;
            _texture = texture;
        }

        public void Update(GameTime gameTime){
            float angle = (float)gameTime.TotalGameTime.TotalSeconds * 3f;

            Matrix rotationX = Matrix.CreateRotationX(MathHelper.PiOver2);
            Matrix rotationY = Matrix.CreateRotationY(angle); // spinning
            Matrix translation = Matrix.CreateTranslation(Position);
            Matrix scale = Matrix.CreateScale(2);

            _world = scale * rotationX * rotationY * translation;

            Bounds.Center = new Vector2(Position.X, Position.Y);
        }

        public void Draw(Matrix view, Matrix projection){
            if (_model == null) return;

            foreach (var mesh in _model.Meshes){
                foreach (BasicEffect effect in mesh.Effects) {
                    effect.World = _world;
                    effect.View = view;
                    effect.Projection = projection;
                    effect.Texture = _texture;
                    effect.TextureEnabled = true;
                    effect.AmbientLightColor = new Vector3(1f, 1f, 1f);
                    effect.SpecularPower = 0;
                }
                mesh.Draw();
            }
        }
    }
}