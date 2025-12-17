using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MonoGameLibrary;
using System;

namespace RIPRUSH.Entities {
    public class CountdownHelper {

        /// made this class to debloat GameScene, probably could be done better

        public bool IsActive { get; private set; }
        public float Scale { get; private set; }
        public int CurrentNumber { get; private set; }

        private float _timer;
        private int _lastNumber;
        private SoundEffect _tickSound;

        public CountdownHelper(SoundEffect tickSound) {
            _tickSound = tickSound;
        }

        public void Start(float seconds) {
            _timer = seconds;
            _lastNumber = -1;
            Scale = 3.5f;
            IsActive = true;
        }

        public bool Update(GameTime gameTime) {
            if (!IsActive)
                return false;

            _timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            CurrentNumber = Math.Max(1, (int)MathF.Ceiling(_timer));

            if (CurrentNumber != _lastNumber) {
                Core.Audio.PlaySoundEffect(_tickSound);
                Scale = 3.5f;
                _lastNumber = CurrentNumber;
            }

            Scale = MathHelper.Lerp(Scale, 1f, 8f * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (_timer <= 0f) {
                IsActive = false;
                return true; // ✅ finished THIS frame
            }

            return false;
        }

    }
}
