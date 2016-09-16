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
        public Planet currentPlanet
        {
            get
            {
                return CurrentPlanet;
            }
            set
            {
                CurrentPlanet = value;
                inOrbit = false;
            }
        }
        private Planet CurrentPlanet;

        public bool inOrbit;

        public Vector2 velocity;
        public float speed;
        public float accel;

        public PlayerShip(Ship ship) 
            : base(ship.texture)
        {
            velocity = new Vector2();
            speed = 1f;
            accel = 0f;
        }

        public override void Update(GameTime gameTime)
        {
            //Get angle to orbit planet
            if (inOrbit)
            {
                //Find angle to the target planet
                double angleToPlanet = Math.Atan2(position.Y - currentPlanet.position.Y, position.X - currentPlanet.position.X);

                rotation = (float)(angleToPlanet + Math.PI);

                if (Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
                    Console.WriteLine(angleToPlanet);
            }
            else
            {
                //Find angle to the target planet
                double angleToPlanet = Math.Atan2(position.Y - currentPlanet.position.Y, position.X - currentPlanet.position.X);
                angleToPlanet -= Math.PI / 4; //Adjust angle origin to north instead of east

                //Find angle from sightline to target planet
                if (angleToPlanet > 0)
                    angleToPlanet -= rotation;
                else angleToPlanet += rotation;

                if (angleToPlanet > 0)
                {
                    rotation -= 0.005f;
                }
                else
                {
                    rotation += 0.005f;
                }

                rotation %= (float)Math.PI * 2;
                if (rotation < 0)
                    rotation = (float)(Math.PI * 2 - rotation);

                //if (angleToPlanet <= 0.1 && angleToPlanet >= -0.1)
                //    rotation += (float)angleToPlanet;
            }

            //Update velocity
            velocity.X = (float)Math.Sin(rotation) * speed;
            velocity.Y = (float)Math.Cos(rotation) * -speed;

            //Update position
            position += velocity;

            base.Update(gameTime);
        }
    }
}
