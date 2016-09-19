using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using BPA_RPG.GameObjects;
using Microsoft.Xna.Framework.Input;

namespace BPA_RPG.Screens
{
    class GameScreen : Screen
    {
        private List<Planet> planets;

        private Camera camera;
        private KeyboardState oldKeyState;

        public override void LoadContent(ContentManager content)
        {
            camera = new Camera();

            planets = new List<Planet>();
            planets.Add(Planet.DebugPlanet);
            planets.Add(Planet.DebugPlanet2);
            
            PlayerData.ship.lastPlanet = planets[0];
            PlayerData.ship.inOrbit = true;
            PlayerData.ship.position = PlayerData.ship.lastPlanet.position - new Vector2(planets[0].orbitDistance, 0);

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState newKeyState = Keyboard.GetState();

            camera.Update(PlayerData.ship.position);

            PlayerData.ship.Update(gameTime);

            foreach (Planet planet in planets)
            {
                planet.Update(gameTime);

                if (PlayerData.ship.inOrbit == false && PlayerData.ship.autoPilotActive == false &&
                    Math.Tan(Math.Abs(PlayerData.ship.position.Y - planet.position.Y) / 
                    Math.Abs(PlayerData.ship.position.X - planet.position.X)) <= planet.orbitDistance)
                {
                    PlayerData.ship.lastPlanet = planet;
                }
            }

            oldKeyState = newKeyState;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            DrawSprites(gameTime, spritebatch, camera);

            base.Draw(gameTime, spritebatch);
        }

        /// <summary>
        /// Draws sprites in accordence to the posistion of the camera.
        /// </summary>
        /// <param name="spritebatch"></param>
        /// <param name="camera"></param>
        private void DrawSprites(GameTime gameTime, SpriteBatch spritebatch, Camera camera)
        {
            spritebatch.End();
            spritebatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, 
                null, null, null, null, camera.transform);


            foreach (Planet planet in planets)
            {
                planet.Draw(gameTime, spritebatch);
            }

            PlayerData.ship.Draw(gameTime, spritebatch);

            spritebatch.End();
            spritebatch.Begin();
        }

        private void DebugPlanetEvent(object sender, EventArgs e)
        {

        }
    }
}
