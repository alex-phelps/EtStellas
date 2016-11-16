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
        private List<Planet> planets => Planet.planets;
        private Background starBackground;
        private Background starBackground2;

        private SpriteFont keyFont;
        private GameObject wKey;
        private GameObject sKey;
        private GameObject aKey;
        private GameObject dKey;
        private GameObject spaceKey;
        
        private Texture2D fuelBar;
        private Texture2D fuel;

        private SpriteFont planetInfoFont;
        private Texture2D planetInfoBox;
        private ClickableObject planetInfoLandButton;

        private Camera camera;

        private bool drawHUD;
        
        public GameScreen()
            : base("Game")
        {
        }

        public override void LoadContent(ContentManager content)
        {
            keyFont = content.Load<SpriteFont>("Fonts/KeyFont");
            planetInfoFont = content.Load<SpriteFont>("Fonts/ChoiceTabFont");

            camera = new Camera();

            ship.position = planets[0].position - new Vector2(planets[0].orbitDistance - 1, 0);

            starBackground = new Background(content.Load<Texture2D>("Images/StarBackground"));
            starBackground2 = new Background(content.Load<Texture2D>("Images/StarBackground2"));

            wKey = new GameObject(content.Load<Texture2D>("Images/WKey")) { position = new Vector2(70, 25) };
            sKey = new GameObject(content.Load<Texture2D>("Images/SKey")) { position = new Vector2(70, 70) };
            aKey = new GameObject(content.Load<Texture2D>("Images/AKey")) { position = new Vector2(25, 70) };
            dKey = new GameObject(content.Load<Texture2D>("Images/DKey")) { position = new Vector2(115, 70) };
            spaceKey = new GameObject(content.Load<Texture2D>("Images/SpaceKey")) { position = new Vector2(70, 175) };

            fuelBar = content.Load<Texture2D>("Images/FuelBar");
            fuel = content.Load<Texture2D>("Images/Fuel");

            planetInfoBox = content.Load<Texture2D>("Images/PlanetInfoBox");
            planetInfoLandButton = new ClickableObject(content.Load<Texture2D>("Images/PlanetInfoLandButton"), () =>
                manager.Push(new TabMenuScreen(new MenuChoiceScreen(ship.lastPlanet.name, ship.lastPlanet.name.Replace(" ", "")), new ShipHoldScreen())))
            {
                position = new Vector2(70, MainGame.WindowHeight - 30)
            };

            base.LoadContent(content);
        }

        public override void Activated()
        {
            drawHUD = true;
            base.Activated();
        }

        public override void Deactivated()
        {
            drawHUD = false;
            base.Deactivated();
        }

        public override void Update(GameTime gameTime)
        {
            //Key Input
            if (InputManager.newKeyState.IsKeyDown(Keys.W))
                wKey.scale = .95f;
            else wKey.scale = 1;
            if (InputManager.newKeyState.IsKeyDown(Keys.S))
                sKey.scale = .95f;
            else sKey.scale = 1;
            if (InputManager.newKeyState.IsKeyDown(Keys.A))
                aKey.scale = .95f;
            else aKey.scale = 1;
            if (InputManager.newKeyState.IsKeyDown(Keys.D))
                dKey.scale = .95f;
            else dKey.scale = 1;

            if (ship.inOrbit)
               planetInfoLandButton.Update(gameTime);
            
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

            //Draw HUD
            if (drawHUD)
            {
                wKey.Draw(gameTime, spritebatch);
                spritebatch.DrawString(keyFont, "Thrust", wKey.position - keyFont.MeasureString("Thrust") / 2, Color.White);

                sKey.Draw(gameTime, spritebatch);
                spritebatch.DrawString(keyFont, "Brake", sKey.position - keyFont.MeasureString("Brake") / 2, Color.White);
                aKey.Draw(gameTime, spritebatch);
                spritebatch.DrawString(keyFont, "Left\nTurn", aKey.position - keyFont.MeasureString("Left\nTurn") / 2, Color.White);
                dKey.Draw(gameTime, spritebatch);
                spritebatch.DrawString(keyFont, "Right\nTurn", dKey.position - keyFont.MeasureString("Right\nTurn") / 2, Color.White);

                spritebatch.Draw(fuelBar, new Vector2(5, 95), Color.White);
                spritebatch.Draw(fuel, new Vector2(5, 95), new Rectangle(0, 0, (int)(fuel.Width * (1 - ship.fuelUsed)), fuel.Height), Color.White);
                string fuelText = "Fuel Storage:  " + PlayerData.inventory.Count(s => s == GameItem.Fuel);
                spritebatch.DrawString(keyFont, fuelText, new Vector2(70, 115) - keyFont.MeasureString(fuelText) / 2, Color.White);

                if (ship.inOrbit)
                {
                    spaceKey.Draw(gameTime, spritebatch);
                    spritebatch.DrawString(keyFont, "Leave Orbit", spaceKey.position - keyFont.MeasureString("Leave Orbit") / 2, Color.White);

                    spritebatch.Draw(planetInfoBox, new Vector2(0, MainGame.WindowHeight - planetInfoBox.Height), Color.White);
                    spritebatch.DrawString(keyFont, "Planet Info", new Vector2(10, MainGame.WindowHeight - 114), Color.White);

                    string info = "Name: " + ship.lastPlanet.name;
                    spritebatch.DrawString(planetInfoFont, info, new Vector2(20, MainGame.WindowHeight - 90), Color.White);
                    spritebatch.Draw(ship.lastPlanet.texture, new Rectangle(145, MainGame.WindowHeight - 60, 50, 50), Color.White);

                    planetInfoLandButton.Draw(gameTime, spritebatch);
                    spritebatch.DrawString(planetInfoFont, "Land", planetInfoLandButton.position - planetInfoFont.MeasureString("Land") / 2, Color.White);
                }
            }

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
