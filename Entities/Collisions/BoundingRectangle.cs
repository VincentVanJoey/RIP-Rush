using Microsoft.Xna.Framework;

namespace RIPRUSH.Entities.Collisions {
    public struct BoundingRectangle {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public float Left => X;
        public float Right => X + Width;
        public float Top => Y;
        public float Bottom => Y + Height;
        public BoundingRectangle(float x, float y, float width, float height) {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public BoundingRectangle(Vector2 position, float width, float height) {
            X = position.X;
            Y = position.Y;
            Width = width;
            Height = height;
        }

        public bool Collides(BoundingRectangle other) {
            return CollisionHelper.Collides(this, other);
        }

        public bool CollidesWith(BoundingCircle other) {
            return CollisionHelper.Collides(other, this);
        }
    }
}
