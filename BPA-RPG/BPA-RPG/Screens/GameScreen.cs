using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using BPA_RPG.GameObjects;

namespace BPA_RPG.Screens
{
    class GameScreen : Screen
    {
        private List<Planet> planets;

        private Camera camera;

        public override void LoadContent(ContentManager content)
        {
            camera = new Camera();

            planets = new List<Planet>();
            planets.Add(Planet.DebugPlanet);
            
            PlayerData.ship.currentPlanet = planets[0];
            PlayerData.ship.inOrbit = true;
            PlayerData.ship.position = PlayerData.ship.currentPlanet.position - new Vector2(100, 0);

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            camera.Update(PlayerData.ship.position);

            PlayerData.ship.Update(gameTime);

            foreach (Planet planet in planets)
            {
                planet.Update(gameTime);
            }

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
