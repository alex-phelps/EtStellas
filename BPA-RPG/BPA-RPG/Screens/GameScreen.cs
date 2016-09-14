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
        private GameObject playerShip;
        private List<Planet> planets;

        private Camera camera;

        public override void LoadContent(ContentManager content)
        {
            camera = new Camera();

            planets = new List<Planet>();

            planets.Add(new Planet(content, "Debug Planet", 
                new Vector2(MainGame.WindowWidth / 2, MainGame.WindowHeight / 2), DebugPlanetEvent));

            playerShip = new GameObject(PlayerData.ship.texture)
                { position = new Vector2(MainGame.WindowWidth / 2, MainGame.WindowHeight / 2) };

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            camera.Update(playerShip.position);

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

            playerShip.Draw(gameTime, spritebatch);

            spritebatch.End();
            spritebatch.Begin();
        }

        private void DebugPlanetEvent(object sender, EventArgs e)
        {

        }
    }
}
