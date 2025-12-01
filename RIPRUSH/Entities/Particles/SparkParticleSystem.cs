using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RIPRUSH.Entities.Particles {
    public class SparkParticleSystem : ParticleSystem {

        IParticleEmitter _emitter;

        public SparkParticleSystem(Game game, IParticleEmitter emitter)
            : base(game, maxParticles: 400) {
            _emitter = emitter;
        }

        protected override void InitializeConstants() {
            textureFilename = "Assets/circle";

            minNumParticles = 3;
            maxNumParticles = 7;

            blendState = BlendState.Additive;
            DrawOrder = AdditiveBlendDrawOrder;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where) {

            Vector2 baseVel = -_emitter.Velocity * 0.2f;

            baseVel += new Vector2(
                RandomHelper.NextFloat(-40f, 40f),
                RandomHelper.NextFloat(-40f, 40f)
            );

            Vector2 acceleration = baseVel * -3.0f;

            // Small sparks
            float scale = RandomHelper.NextFloat(0.05f, 0.15f);

            // Very fast-fading
            float lifetime = RandomHelper.NextFloat(0.15f, 0.35f);

            // Color: bright → fades quickly
            Color c = Color.Lerp(Color.Orange, Color.Yellow, RandomHelper.NextFloat());

            p.Initialize(
                position: where,
                velocity: baseVel,
                acceleration: acceleration,
                color: c,
                lifetime: lifetime,
                scale: scale
            );
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            AddParticles(_emitter.Position);
        }
    }
}
