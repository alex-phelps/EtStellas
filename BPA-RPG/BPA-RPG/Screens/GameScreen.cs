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
        private Viewport defaultView;
        private Viewport miniMapView;

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

        private int miniMapScale = 50;
        private SpriteFont miniMapFont;
        private Texture2D miniMapOverlay;
        private MiniMapDot playerDot;
        private List<MiniMapDot> planetDots;
        private Texture2D mmScrollBar;
        private ClickableObject mmScrollIcon;

        private Camera playerCamera;
        private Camera miniMapCamera;

        private bool drawHUD;
        
        public GameScreen()
            : base("Game")
        {
            defaultView = MainGame.graphicsDevice.Viewport;

            miniMapView = defaultView;
            miniMapView.X = MainGame.WindowWidth - 250;
            miniMapView.Y = MainGame.WindowHeight - 150;
            miniMapView.Width = 250;
            miniMapView.Height = 150;

            playerCamera = new Camera();
            miniMapCamera = new Camera();
        }

        public override void LoadContent(ContentManager content)
        {
            keyFont = content.Load<SpriteFont>("Fonts/KeyFont");
            planetInfoFont = content.Load<SpriteFont>("Fonts/ChoiceTabFont");
            miniMapFont = content.Load<SpriteFont>("Fonts/MiniMapFont");

            Planet home = planets.First(p => p.position == Vector2.Zero);
            ship.position = home.position - new Vector2(home.orbitDistance - 1, 0);
            
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
            {
                if (ship.inOrbit)
                    manager.Push(new TabMenuScreen(new MenuChoiceScreen(ship.lastPlanet.name, ship.lastPlanet.name.Replace(" ", "")), new ShipHoldScreen()));
                else manager.Push(new TabMenuScreen(new ShipHoldScreen()));
            })
            {
                position = new Vector2(70, MainGame.WindowHeight - 30)
            };

            miniMapOverlay = content.Load<Texture2D>("Images/MiniMapOverlay");
            playerDot = new MiniMapDot(content, ship, Color.LimeGreen)
            {
                position = ship.position / miniMapScale,
                scale = .8f
            };

            planetDots = new List<MiniMapDot>();
            foreach (Planet p in Planet.planets)
            {
                planetDots.Add(new MiniMapDot(content, p, Color.Purple) { position = p.position / miniMapScale });
            }

            mmScrollBar = content.Load<Texture2D>("Images/MMScrollBar");
            mmScrollIcon = new ClickableObject(content.Load<Texture2D>("Images/MMScrollIcon"), null, null, null, () =>
            {
                if (InputManager.newMouseState.Position.Y > mmScrollIcon.position.Y + 8)
                    mmScrollIcon.position.Y += 11;
                else if (InputManager.newMouseState.Position.Y < mmScrollIcon.position.Y - 8)
                    mmScrollIcon.position.Y -= 11;

                mmScrollIcon.position.Y = MathHelper.Clamp(mmScrollIcon.position.Y, MainGame.WindowHeight - 137, MainGame.WindowHeight - 16);

                miniMapScale = (int)(mmScrollIcon.position.Y - (MainGame.WindowHeight - 137)) + 50;
            })
            {
                position = new Vector2(MainGame.WindowWidth - 10, MainGame.WindowHeight - 137)
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

            planetInfoLandButton.Update(gameTime);
            
            playerCamera.Update(ship.position);
            
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

            //Update minimap
            if (drawHUD)
            {
                mmScrollIcon.Update(gameTime);

                if (miniMapView.Bounds.Contains(InputManager.newMouseState.Position))
                {
                   if (InputManager.newMouseState.ScrollWheelValue > InputManager.oldMouseState.ScrollWheelValue && miniMapScale > 50)
                    {
                        miniMapScale -= 11;
                        mmScrollIcon.position.Y -= 11;
                    }
                    else if (InputManager.newMouseState.ScrollWheelValue < InputManager.oldMouseState.ScrollWheelValue && miniMapScale < 171)
                    {
                        miniMapScale += 11;
                        mmScrollIcon.position.Y += 11;
                    }
                }
            }

            miniMapCamera.Update(playerDot.position, new Vector2(125, 75));
            playerDot.Update(gameTime, miniMapScale);
            foreach (MiniMapDot d in planetDots)
                d.Update(gameTime, miniMapScale);
                        
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            DrawBackground(gameTime, spritebatch);
            DrawSprites(gameTime, spritebatch, playerCamera);

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
                string fuelText = "Fuel Storage:  " + PlayerData.Inventory.Count(s => s == GameItem.Fuel);
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
                else
                {
                    spritebatch.Draw(planetInfoBox, new Vector2(0, MainGame.WindowHeight - planetInfoBox.Height), Color.White);
                    spritebatch.DrawString(keyFont, "Ship Access", new Vector2(10, MainGame.WindowHeight - 114), Color.White);

                    string info = ship.name;
                    spritebatch.DrawString(planetInfoFont, info, new Vector2(20, MainGame.WindowHeight - 90), Color.White);
                    spritebatch.Draw(ship.texture, new Rectangle(145, MainGame.WindowHeight - 60, 50, 50), new Rectangle(0, 0, ship.Width / 2, ship.Height), Color.White);

                    planetInfoLandButton.Draw(gameTime, spritebatch);
                    spritebatch.DrawString(planetInfoFont, "Open Hold", planetInfoLandButton.position - planetInfoFont.MeasureString("Open Hold") / 2, Color.White);
                }


                //Draw minimap

                spritebatch.Draw(miniMapOverlay, new Vector2(MainGame.WindowWidth - miniMapOverlay.Width,
                    MainGame.WindowHeight - miniMapOverlay.Height), Color.White);

                spritebatch.End();
                spritebatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, miniMapCamera.transform);

                MainGame.graphicsDevice.Viewport = miniMapView;

                playerDot.Draw(gameTime, spritebatch);

                foreach (MiniMapDot d in planetDots)
                {
                    string name = (d.repOf as Planet).name;
                    d.Draw(gameTime, spritebatch);
                    spritebatch.DrawString(miniMapFont, name, d.position - new Vector2(0, 10) - miniMapFont.MeasureString(name) / 2, Color.White);
                }

                spritebatch.End();
                spritebatch.Begin();

                MainGame.graphicsDevice.Viewport = defaultView;

                spritebatch.DrawString(planetInfoFont, "GPS", new Vector2(MainGame.WindowWidth - 245, MainGame.WindowHeight - 146), Color.White);
                spritebatch.Draw(mmScrollBar, new Vector2(MainGame.WindowWidth - 12, MainGame.WindowHeight - 140), Color.White);
                mmScrollIcon.Draw(gameTime, spritebatch);
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
            spritebatch.Begin(SpriteSortMode.Deferred, null, 
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
