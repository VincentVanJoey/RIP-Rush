using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MonoGameLibrary;
using System;

namespace RIPRUSH.Entities.Environment {

    public class WorldColorChanger {

        /// made this class to debloat GameScene, probably could be done better

        public Color Current { get; private set; }
        public Color Target { get; private set; }
        private float _speed;

        public WorldColorChanger(Color start, float speed = 5f) {
            Current = start;
            Target = start;
            _speed = speed;
        }

        public void SetTarget(Color target) {
            Target = target;
        }

        public void Update(float deltaTime) {
            // Exponential smoothing
            Current = Color.Lerp(
                Current,
                Target,
                1f - MathF.Exp(-_speed * deltaTime)
            );
        }
    }
}
