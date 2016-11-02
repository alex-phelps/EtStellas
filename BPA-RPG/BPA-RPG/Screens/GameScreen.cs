using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using BPA_RPG.GameObjects;
using Microsoft.Xna.Framework.Input;
using BPA_RPG.GameItems;

namespace BPA_RPG.Screens
{
    public class GameScreen : Screen
    {
        private PlayerShip ship => PlayerData.ship;
        private List<Planet> planets;
        private Background starBackground;
        private Background starBackground2;

        private Camera camera;
        
        public GameScreen()
            : base("Game")
        {
            //Viewport vp = MainGame.graphicsDevice.Viewport;
            //vp.X = 100;
            //MainGame.graphicsDevice.Viewport = vp;

        }

        public override void LoadContent(ContentManager content)
        {
            camera = new Camera();

            planets = new List<Planet>();
            planets.Add(Planet.DebugPlanet);
            planets.Add(Planet.DebugPlanet2);

            ship.position = planets[0].position - new Vector2(planets[0].orbitDistance - 1, 0);

            starBackground = new Background(content.Load<Texture2D>("Images/StarBackground"));
            starBackground2 = new Background(content.Load<Texture2D>("Images/StarBackground2"));

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            if (MainGame.input.newKeyState.IsKeyDown(Keys.Enter) && MainGame.input.oldKeyState.IsKeyUp(Keys.Enter) && ship.inOrbit)
                manager.Push(new TabMenuScreen(new MenuChoiceScreen(ship.lastPlanet.name, ship.lastPlanet.name.Replace(" ", "")), new ShipHoldScreen()));

            camera.Update(ship.position);
            
            ship.Update(gameTime);

            foreach (Planet planet in planets)
            {
                planet.Update(gameTime);

                //If player is within planet distance, set the player ship
                if (ship.inOrbit == false && ship.autoPilotActive == false &&
                    Vector2.Distance(ship.position, planet.position) <= planet.orbitDistance)
                {
                    ship.lastPlanet = planet;
                    manager.Push(new TabMenuScreen(new MenuChoiceScreen(planet.name, planet.name.Replace(" ", "")), new ShipHoldScreen()));
                }
            }

            starBackground.Scroll(ship.position, .25f);
            starBackground2.Scroll(ship.position, .28f);
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            DrawBackground(gameTime, spritebatch);
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

            ship.Draw(gameTime, spritebatch);

            spritebatch.End();
            spritebatch.Begin();
        }

        private void DrawBackground(GameTime gameTime, SpriteBatch spritebatch)
        {
            spritebatch.End();
            spritebatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap);

            starBackground.Draw(gameTime, spritebatch);
            starBackground2.Draw(gameTime, spritebatch);

            spritebatch.End();
            spritebatch.Begin();
        }
    }
}
