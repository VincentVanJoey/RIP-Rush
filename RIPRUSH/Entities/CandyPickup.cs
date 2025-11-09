using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using RIPRUSH.Entities.CollisionShapes;

namespace RIPRUSH.Entities.Environment {
    public enum CandyType { Lollipop, Chocobar, ZapCandy }

    public class CandyPickup {
        public Vector3 Position;
        public bool Collected;
        public CandyType Type;
        public SoundEffect PickupSound;

        public BoundingCircle Bounds;
        private const float CollisionRadius = 30f;

        private BasicEffect _effect;
        private VertexBuffer _vb;
        private IndexBuffer _ib;
        private GraphicsDevice _graphics;
        private Color _color;
        private Matrix _world;

        public CandyPickup(GraphicsDevice graphics, CandyType type, Vector3 position) {
            _graphics = graphics;
            Type = type;
            Position = position;
            _color = GetColor(type);

            Bounds = new BoundingCircle(new Vector2(position.X, position.Y), CollisionRadius);

            InitializeGeometry();
            InitializeEffect();
        }

        private Color GetColor(CandyType type) {
            return type switch {
                CandyType.Chocobar => Color.LimeGreen,
                CandyType.Lollipop => Color.Red,
                CandyType.ZapCandy => Color.Blue,
                _ => Color.White
            };
        }

        private void InitializeGeometry() {

            //taking some inspiration from the tutorial and making some flat cubes until I make models

            var vertices = new VertexPositionColor[]
            {
                new VertexPositionColor(new Vector3(-20,  20, -20), _color),
                new VertexPositionColor(new Vector3( 20,  20, -20), _color),
                new VertexPositionColor(new Vector3(-20, -20, -20), _color),
                new VertexPositionColor(new Vector3( 20, -20, -20), _color),
                new VertexPositionColor(new Vector3(-20,  20,  20), _color),
                new VertexPositionColor(new Vector3( 20,  20,  20), _color),
                new VertexPositionColor(new Vector3(-20, -20,  20), _color),
                new VertexPositionColor(new Vector3( 20, -20,  20), _color)
            };

            short[] indices = {
                0,1,2, 2,1,3,
                4,0,6, 6,0,2,
                7,5,6, 6,5,4,
                3,1,7, 7,1,5,
                4,5,0, 0,5,1,
                3,7,2, 2,7,6
            };

            _vb = new VertexBuffer(_graphics, typeof(VertexPositionColor), vertices.Length, BufferUsage.None);
            _vb.SetData(vertices);
            _ib = new IndexBuffer(_graphics, IndexElementSize.SixteenBits, indices.Length, BufferUsage.None);
            _ib.SetData(indices);
        }

        private void InitializeEffect() {
            _effect = new BasicEffect(_graphics) {
                VertexColorEnabled = true,
                LightingEnabled = false
            };
        }

        public void Update(GameTime gameTime) {
            float angle = (float)gameTime.TotalGameTime.TotalSeconds;
            _world = Matrix.CreateRotationY(angle) *
                     Matrix.CreateTranslation(Position);

            // Update bouynds
            Bounds.Center = new Vector2(Position.X, Position.Y);
        }

        public void Draw(Matrix view, Matrix projection) {
            _effect.World = _world;
            _effect.View = view;
            _effect.Projection = projection;

            foreach (var pass in _effect.CurrentTechnique.Passes) {
                pass.Apply();
                _graphics.SetVertexBuffer(_vb);
                _graphics.Indices = _ib;
                _graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 12);
            }
        }
    }
}