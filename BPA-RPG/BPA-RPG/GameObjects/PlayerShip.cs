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
    public class PlayerShip : GameObject
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

        public bool inOrbit;
        public bool autoPilotActive;
        public bool accelerating;
        private double autoPilotTimer;

        public string name => PlayerData.name + "'s " + baseShip.name;
        public int hullPoints;
        public int maxHullPoints => baseShip.maxHullPoints;
        public int holdSize => baseShip.holdSize;
        public float accel => baseShip.accel;
        public Vector2 velocity;
        public float speed;
        public float maxSpeed => baseShip.maxSpeed;
        public float rotSpeed;
        public float maxRotSpeed => baseShip.maxRotSpeed;
        public float rotAccel => baseShip.rotAccel;
        public List<Type> weaponHold => baseShip.weaponHold;

        private float travelDistance;
        private float fuelUsed;

        public PlayerShip(Ship ship) 
            : base(ship.texture)
        {
            baseShip = ship;

            velocity = new Vector2();
            speed = 1.5f;

            hullPoints = maxHullPoints;
        }

        public override void Update(GameTime gameTime)
        {
            //Get angle to orbit planet
            if (inOrbit)
            {
                //Reset values
                speed = 1.5f;
                rotSpeed = 0;
                accelerating = true;

                //Find angle to the target planet
                double angleToPlanet = Math.Atan2(position.Y - lastPlanet.position.Y, position.X - lastPlanet.position.X);

                rotation = (float)(angleToPlanet + Math.PI);


                // Check keyboard input

                if (InputManager.newKeyState.IsKeyDown(Keys.Space))
                {
                    inOrbit = false;
                    autoPilotActive = true;
                }
            }
            else if (autoPilotActive)
            {
                // Wait 4.5 seconds until player is given control

                autoPilotTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

                if (autoPilotTimer >= 4500)
                {
                    autoPilotTimer -= 4500;
                    autoPilotActive = false;
                }
            }
            else
            {

                //Keyboard input

                if (PlayerData.inventory.Contains(GameItem.Fuel))
                {
                    if (InputManager.newKeyState.IsKeyDown(Keys.W))
                    {
                        speed += accel;
                        accelerating = true;
                    }
                    else accelerating = false;

                    if (InputManager.newKeyState.IsKeyDown(Keys.S))
                        if (speed > 0)
                            speed -= accel;

                    if (InputManager.newKeyState.IsKeyDown(Keys.D))
                        rotSpeed += rotAccel;

                    if (InputManager.newKeyState.IsKeyDown(Keys.A))
                        rotSpeed -= rotAccel;

                    // Cap speeds
                    if (speed > maxSpeed)
                        speed = maxSpeed;

                    if (rotSpeed > maxRotSpeed)
                        rotSpeed = maxRotSpeed;
                    else if (rotSpeed < -maxRotSpeed)
                        rotSpeed = -maxRotSpeed;
                }
                else accelerating = false;

                if (InputManager.newKeyState.IsKeyUp(Keys.W) && InputManager.newKeyState.IsKeyUp(Keys.S))
                {
                    if (speed < -0.25f) // Not 0 here to fix any rounding errors
                        speed += 0.025f;
                    else if (speed > 0.25f) //Not 0 here to fix any rounding errors
                        speed -= 0.025f;
                    else speed = 0;
                }
                if (InputManager.newKeyState.IsKeyUp(Keys.A) && InputManager.newKeyState.IsKeyUp(Keys.D))
                {
                    if (rotSpeed < -0.002f) // Not 0 here to fix any rounding errors
                        rotSpeed += 0.001f;
                    else if (rotSpeed > 0.002f) //Not 0 here to fix any rounding errors
                        rotSpeed -= 0.001f;
                    else rotSpeed = 0;
                }
            }
            
            

            Vector2 prevPos = position;

            //Update velocity
            velocity.X = (float)Math.Sin(rotation) * speed;
            velocity.Y = (float)Math.Cos(rotation) * -speed;

            //Update position
            position += velocity;

            //Update rotation
            rotation += rotSpeed;
            rotation %= (float)(Math.PI * 2);

            if (PlayerData.inventory.Contains(GameItem.Fuel))
            {
                travelDistance += Math.Abs(prevPos.X - position.X) + Math.Abs(prevPos.Y - position.Y);

                if (travelDistance >= 10000)
                {
                    fuelUsed += PlayerData.engine.fuelRate;
                    travelDistance = 0;
                }

                if (fuelUsed >= 1)
                {
                    PlayerData.inventory.Remove(GameItem.Fuel);
                    fuelUsed = 0;
                }
            }
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch, Color color)
        {
            if (visible)
            {
                // Draw right half of texture if idle, draw left half if accelerating
                Rectangle source;
                if (accelerating)
                    source = new Rectangle(texture.Width / 2, 0, texture.Width / 2, texture.Height);
                else source = new Rectangle(0, 0, texture.Width / 2, texture.Height);

                spritebatch.Draw(texture, position, source, color, rotation, new Vector2(texture.Width / 4, texture.Height / 2), scale, SpriteEffects.None, 1);
            }
        }
    }
}
