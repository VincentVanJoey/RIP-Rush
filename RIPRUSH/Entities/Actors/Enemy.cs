using Microsoft.Xna.Framework;
using MonoGameLibrary;
using RIPRUSH.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIPRUSH.Entities.Actors {
    public abstract class Enemy: Sprite {

        public bool IsActive { get; set; } = true;

        // Each enemy decides its own movement
        public abstract void Move(GameTime gameTime);

        // Each enemy decides how it interacts with the player
        public abstract void CheckCollision(Pumpkin player);

        public void Update(GameTime gameTime, Pumpkin player) {
            Move(gameTime);
            CheckCollision(player);
            base.Update(gameTime);
        }

    }
}
