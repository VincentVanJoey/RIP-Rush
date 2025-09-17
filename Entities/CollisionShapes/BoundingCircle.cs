using Microsoft.Xna.Framework;

namespace RIPRUSH.Entities.CollisionShapes {
    /// <summary>
    /// A struct representing circular bounds
    /// </summary>
    public struct BoundingCircle {
        /// <summary>
        /// The center of the BoundingCircle
        /// </summary>
        public Vector2 Center;

        /// <summary>
        /// The radius of the BoundingCircle
        /// </summary>
        public float Radius;

        /// <summary>
        /// Constructs a BoundingCircle
        /// </summary>
        /// <param name="center">The center</param>
        /// <param name="radius">The radius</param>
        public BoundingCircle(Vector2 center, float radius) {
            Center = center;
            Radius = radius;
        }

        public bool Intersects(BoundingCircle other) {
            float distanceSquared = Vector2.DistanceSquared(this.Center, other.Center);
            float radiusSum = this.Radius + other.Radius;
            return distanceSquared <= radiusSum * radiusSum;
        }

        /// <summary>
        /// Tests for a collision between this and another bounding circle
        /// </summary>
        /// <param name="other">The other bounding circle</param>
        /// <returns>true for collision, false otherwise</returns>
        public bool CollidesWith(BoundingCircle other) {
            return CollisionHelper.Collides(this, other);
        }

        public bool CollidesWith(BoundingRectangle other) {
            return CollisionHelper.Collides(this, other);
        }
    }
}
