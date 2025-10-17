using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RIPRUSH.Entities.Particles {
    public class PumpkinParticleSystem : ParticleSystem {
        Color[] colors = {
            Color.DarkOrange,
            Color.Orange,
            Color.DarkGreen
        };

        Color color;

        public PumpkinParticleSystem(Game game, int maxExplosions) : base(game, maxExplosions * 25) { }

        protected override void InitializeConstants() {
            textureFilename = "Assets/the moon";
            minNumParticles = 20;
            maxNumParticles = 50;

            blendState = BlendState.Additive;
            DrawOrder = AdditiveBlendDrawOrder;
        }
        protected override void InitializeParticle(ref Particle p, Vector2 where) {

            var velocity = RandomHelper.NextDirection() * RandomHelper.NextFloat(40, 200);

            var lifetime = RandomHelper.NextFloat(0.5f, 0.7f);

            var acceleration = -velocity / lifetime;

            var rotation = RandomHelper.NextFloat(0, MathHelper.TwoPi);

            var angularVelocity = RandomHelper.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4);

            var scale = RandomHelper.NextFloat(4, 6);

            p.Initialize(where, velocity, acceleration, color, lifetime: lifetime, rotation: rotation, angularVelocity: angularVelocity);
        }

        protected override void UpdateParticle(ref Particle particle, float dt) {
            base.UpdateParticle(ref particle, dt);

            float normalizedLifetime = particle.TimeSinceStart / particle.Lifetime;

            particle.Scale = .1f + .25f * normalizedLifetime;
        }

        public void Explode(Vector2 where) {
            color = colors[RandomHelper.Next(colors.Length)];
            AddParticles(where);

        }
    }

}
