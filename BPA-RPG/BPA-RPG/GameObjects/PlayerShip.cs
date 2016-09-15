using BPA_RPG.GameItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA_RPG.GameObjects
{
    /// <summary>
    /// Represents the player's specific ship
    /// </summary>
    class PlayerShip : GameObject
    {
        public Planet currentPlanet;
        public bool inOrbit;

        public Vector2 velocity;
        public Vector2 accel;

        public PlayerShip(Ship ship) 
            : base(ship.texture)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (inOrbit)
            {
                Vector2 distanceToPlanet = new Vector2(Math.Abs(position.X - currentPlanet.position.X),
                    Math.Abs(position.Y - currentPlanet.position.Y));

                float angle = (float)Math.Atan(distanceToPlanet.Y / distanceToPlanet.X);
                rotation = angle - (float)(Math.PI / 2);
            }

            base.Update(gameTime);
        }
    }
}
