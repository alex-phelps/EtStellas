using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BPA_RPG.Screens;
using BPA_RPG.GameItems;
using Microsoft.Xna.Framework.Content;
using BPA_RPG.GameObjects;

namespace BPA_RPG
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Game
    {
        public static int WindowWidth = 1024;
        public static int WindowHeight = 576;
        public static EventLogger eventLogger;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private ScreenManager screenManager;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.IsMouseVisible = true;

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
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //Create new EventLogger to log important events
            eventLogger = new EventLogger();

            //Define static ships
            Ship.StarterShip = new Ship("Starter Ship", Content.Load<Texture2D>("Images/StarterShip"), 7f, 0.05f, 0.025f, 0.0005f);

            eventLogger.Log(this, "Finished loading static ships");
            
            //Define static planets
            Planet.DebugPlanet = new Planet("Debug Planet", Content.Load<Texture2D>("Images/DebugPlanet"));
            Planet.DebugPlanet2 = new Planet("Debug Planet 2", Content.Load<Texture2D>("Images/DebugPlanet"), new Vector2(3000, 2000));

            eventLogger.Log(this, "Finished loading static planets");

            //Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Create a new ScreenManager to handle different screens.
            screenManager = new ScreenManager(Content);

            //Create the Main Menu Screen
            screenManager.Push(new MainMenuScreen());

            //Set default player ship
            PlayerData.ship = new PlayerShip(Ship.StarterShip);

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
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            screenManager.Update(gameTime);

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
