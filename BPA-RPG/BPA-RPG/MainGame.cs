using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BPA_RPG.Screens;
using BPA_RPG.GameItems;
using BPA_RPG.GameObjects;
using BPA_RPG.GameItems.Weapons;
using System.Xml.Serialization;
using System.IO;
using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace BPA_RPG
{
    /// <summary>
    /// This is the main type for the game.
    /// </summary>
    public class MainGame : Game
    {
        public static readonly int WindowWidth = 1024;
        public static readonly int WindowHeight = 576;
        public static Vector2 WindowCenter => new Vector2(WindowWidth / 2, WindowHeight / 2);

        public static GraphicsDeviceManager graphicsDeviceManager { get; private set; }
        public static GraphicsDevice graphicsDevice { get; private set; }
        public static EventLogger eventLogger { get; private set; }

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private ScreenManager screenManager;

        private Texture2D mouseIcon;

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
        /// </summary>
        protected override void Initialize()
        {
            graphicsDeviceManager = graphics;
            graphicsDevice = GraphicsDevice;

            //Create new EventLogger to log important events
            eventLogger = new EventLogger();

            //Set mediaplayer to repeat songs
            MediaPlayer.IsRepeating = true;

            base.Initialize();
        }

        /// <summary>
        /// Called once per game and is loads the game content
        /// </summary>
        protected override void LoadContent()
        {
            mouseIcon = Content.Load<Texture2D>("Images/MouseIcon");

            //Load sounds
            SoundEffectManager.LoadContent(Content);

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

            //Set default player ship as Discorvery
            PlayerData.ship = new PlayerShip(Ship.ships[0]);

            //Create the Main Menu Screen
            screenManager.Push(new TitleScreen());

            //Load ScreenManager
            screenManager.LoadContent();
            
            //Have options save when game closes
            AppDomain.CurrentDomain.ProcessExit += (s, e) => Save();

            //Load options
            Load();

            eventLogger.Log(this, "Done loading");
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
            spriteBatch.Draw(mouseIcon, InputManager.newMouseState.Position.ToVector2(), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Saves game options to file
        /// </summary>
        private void Save(string filename = "options")
        {
            filename += ".dat";

            XmlSerializer xml = new XmlSerializer(typeof(OptionsData));
            OptionsData data = new OptionsData()
            {
                isFullscreen = graphics.IsFullScreen,
                volume = SoundEffect.MasterVolume
            };

            using (TextWriter writer = new StreamWriter(filename))
                xml.Serialize(writer, data);

            eventLogger.Log(this, "Saved options");
        }

        /// <summary>
        /// Loads game options from file
        /// </summary>
        private void Load(string filename = "options")
        {
            filename += ".dat";

            if (File.Exists(filename))
            {
                XmlSerializer xml = new XmlSerializer(typeof(OptionsData));
                OptionsData data;

                using (TextReader reader = new StreamReader(filename))
                    data = (OptionsData)xml.Deserialize(reader);

                graphics.IsFullScreen = data.isFullscreen;
                graphics.ApplyChanges();

                SoundEffect.MasterVolume = data.volume;
                MediaPlayer.Volume = data.volume;

                eventLogger.Log(this, "Loaded options");
            }
        }
    }
}
