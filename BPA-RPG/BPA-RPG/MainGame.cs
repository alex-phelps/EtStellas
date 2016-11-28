using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BPA_RPG.Screens;
using BPA_RPG.GameItems;
using Microsoft.Xna.Framework.Content;
using BPA_RPG.GameObjects;
using System.Collections.Generic;

namespace BPA_RPG
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Game
    {
        public static readonly int WindowWidth = 1024;
        public static readonly int WindowHeight = 576;
        public static Vector2 WindowCenter => new Vector2(WindowWidth / 2, WindowHeight / 2);

        public static GraphicsDevice graphicsDevice { get; private set; }
        public static EventLogger eventLogger { get; private set; }

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private ScreenManager screenManager;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = WindowWidth;
            graphics.PreferredBackBufferHeight = WindowHeight;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphicsDevice = GraphicsDevice;

            IsMouseVisible = true;

            //Create new EventLogger to log important events
            eventLogger = new EventLogger();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //Load game items, ships, and planets
            GameItem.LoadContent(Content);
            Engine.LoadContent(Content);
            Weapon.LoadContent(Content);
            Ship.LoadContent(Content);
            Planet.LoadContent(Content);

            //Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Create a new ScreenManager to handle different screens.
            screenManager = new ScreenManager(Content);

            //Set default player ship
            PlayerData.ship = new PlayerShip(Ship.NeonCruiser);

            //Give player starter items
            PlayerData.inventory.AddRange(new GameItem[] 
            {
                GameItem.Fuel,
                GameItem.Fuel,
                GameItem.Fuel,
                GameItem.Fuel,
                GameItem.Fuel,
                LaserWeapon.BasicLaser
            });
            PlayerData.engine = Engine.BasicEngine;
            PlayerData.weapons[0] = LaserWeapon.BasicLaser;
            PlayerData.weapons[1] = LaserWeapon.BasicLaser;

            //Create the Main Menu Screen
            screenManager.Push(new MainMenuScreen());
            screenManager.Push(new GameScreen());

            //Load ScreenManager
            screenManager.LoadContent();

            eventLogger.Log(this, "Done loading");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            InputManager.Begin();
            screenManager.Update(gameTime);
            InputManager.End();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            spriteBatch.Begin();
            screenManager.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
