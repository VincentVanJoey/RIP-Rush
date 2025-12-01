using Microsoft.Xna.Framework;

namespace RIPRUSH.Entities.Particles {
    public interface IParticleEmitter {
        public Vector2 Position { get; }
        public Vector2 Velocity { get; }
    }
}