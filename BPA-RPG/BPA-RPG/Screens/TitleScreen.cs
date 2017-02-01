using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using BPA_RPG.GameObjects;
using BPA_RPG.GameItems;
using System.IO;

namespace BPA_RPG.Screens
{
    public class TitleScreen : Screen
    {
        private Background background1;
        private Background background2;
        private Vector2 backgroundScroll;

        private Texture2D titleScreen;

        private SpriteFont font;
        private List<DrawableString> options;

        private GameObject ship;

        public TitleScreen() 
            : base("Title Screen")
        {

        }

        public override void LoadContent(ContentManager content)
        {
            background1 = new Background(content.Load<Texture2D>("Images/StarBackground"));
            background2 = new Background(content.Load<Texture2D>("Images/StarBackground2"));
            backgroundScroll = Vector2.Zero;

            //Get a random ship from /Images/Ships/
            Texture2D randomShip = content.Load<Texture2D>(Directory.GetFiles("Content/Images/Ships").OrderBy(x => new Random()
            .Next()).First().Replace("Content/", "").Replace(".xnb", ""));

            ship = new GameObject(randomShip)
            {
                source = new Rectangle(randomShip.Width / 2, 0, randomShip.Width / 2, randomShip.Height),
                position = new Vector2(MainGame.WindowWidth / 5, MainGame.WindowHeight / 2),
                scale = 2.5f
            };

            titleScreen = content.Load<Texture2D>("Images/TitleScreen");

            font = content.Load<SpriteFont>("Fonts/TitleFont");

            options = new List<DrawableString>();

            //New Game option
            options.Add(CreateOption("New Game", 0, () =>
            {
                manager.Push(new GameScreen());
                manager.Push(new TabMenuScreen(false, new MenuChoiceScreen("Intro", "NewGameScript")));
            }));

            //Load Game option
            options.Add(CreateOption("Load Game", 1, () =>
            {
                if (File.Exists("saveData.sav"))
                {
                    manager.Push(new GameScreen());
                    PlayerData.Load();
                }
            }));

            //Options option
            options.Add(CreateOption("Options", 2)); // add options

            //Exit option
            options.Add(CreateOption("Exit", 3, () => Environment.Exit(0)));

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            background1.Scroll(backgroundScroll, .25f);
            background2.Scroll(backgroundScroll, .28f);
            backgroundScroll.Y -= 18;

            foreach (DrawableString option in options)
                option.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            //Draw with wrapping
            spritebatch.End();
            spritebatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap);

            background1.Draw(gameTime, spritebatch);
            background2.Draw(gameTime, spritebatch);

            //Draw without linear interpolation
            spritebatch.End();
            spritebatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            
            ship.Draw(gameTime, spritebatch);

            spritebatch.End();
            spritebatch.Begin();

            spritebatch.Draw(titleScreen, Vector2.Zero, Color.White);

            foreach (DrawableString option in options)
                option.Draw(gameTime, spritebatch);

            base.Draw(gameTime, spritebatch);
        }

        private DrawableString CreateOption(string name, int index, Action onClick = null)
        {
            return new DrawableString(font, name, new Vector2(880, 280 +  60 * index) - font.MeasureString(name), Color.White,
                onClick, 
                () =>
                {
                    options[index].text = name + "  ";
                    options[index].position.X = 880 - font.MeasureString(options[index].text).X;
                },
                () =>
                {
                    options[index].text = name;
                    options[index].position.X = 880 - font.MeasureString(options[index].text).X;
                });
        }
    }
}
