using BPA_RPG.GameItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        public Planet lastPlanet
        {
            get
            {
                return LastPlanet;
            }
            set
            {
                LastPlanet = value;
                inOrbit = true;
            }
        }
        private Planet LastPlanet;

        private Ship baseShip;
        private KeyboardState oldKeyState;

        public bool inOrbit;
        public bool autoPilotActive;
        private double autoPilotTimer;

        public Vector2 velocity;
        public float speed;
        public float accel => baseShip.accel;
        public float maxSpeed => baseShip.maxSpeed;
        public float rotSpeed;
        public float maxRotSpeed => baseShip.maxRotSpeed;
        public float rotAccel => baseShip.rotAccel;

        public PlayerShip(Ship ship) 
            : base(ship.texture)
        {
            baseShip = ship;

            velocity = new Vector2();
            speed = 1.5f;
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState newKeyState = Keyboard.GetState();

            //Get angle to orbit planet
            if (inOrbit)
            {
                //Reset speeds
                speed = 1.5f;
                rotSpeed = 0;

                //Find angle to the target planet
                double angleToPlanet = Math.Atan2(position.Y - lastPlanet.position.Y, position.X - lastPlanet.position.X);

                rotation = (float)(angleToPlanet + Math.PI);

                if (newKeyState.IsKeyDown(Keys.Space))
                {
                    inOrbit = false;
                    autoPilotActive = true;
                }
            }
            else if (autoPilotActive)
            {
                autoPilotTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

                if (autoPilotTimer >= 4500)
                {
                    autoPilotTimer -= 4500;
                    autoPilotActive = false;
                }
            }
            else
            {
                if (newKeyState.IsKeyDown(Keys.W))
                    speed += accel;

                if (newKeyState.IsKeyDown(Keys.S))
                    speed -= accel;

                if (newKeyState.IsKeyDown(Keys.D))
                    rotSpeed += rotAccel;

                if (newKeyState.IsKeyDown(Keys.A))
                    rotSpeed -= rotAccel;

                if (newKeyState.IsKeyUp(Keys.W) && newKeyState.IsKeyUp(Keys.S))
                {
                    if (speed < -0.25f) // Not 0 here to fix any rounding errors
                        speed += 0.025f;
                    else if (speed > 0.25f) //Not 0 here to fix any rounding errors
                        speed -= 0.025f;
                    else speed = 0;
                }
                if (newKeyState.IsKeyUp(Keys.A) && newKeyState.IsKeyUp(Keys.D))
                {
                    if (rotSpeed < -0.05f) // Not 0 here to fix any rounding errors
                        rotSpeed += 0.03f;
                    else if (rotSpeed > 0.05f) //Not 0 here to fix any rounding errors
                        rotSpeed -= 0.03f;
                    else rotSpeed = 0;
                }
            }
            
            

            //Update velocity
            velocity.X = (float)Math.Sin(rotation) * speed;
            velocity.Y = (float)Math.Cos(rotation) * -speed;

            //Update position
            position += velocity;

            //Update rotation
            rotation += rotSpeed;
            rotation %= (float)(Math.PI * 2);

            oldKeyState = newKeyState;
            base.Update(gameTime);
        }
    }
}
