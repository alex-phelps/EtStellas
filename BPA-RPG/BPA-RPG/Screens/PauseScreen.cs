using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using BPA_RPG.GameObjects;
using Microsoft.Xna.Framework.Input;

namespace BPA_RPG.Screens
{
    public class PauseScreen : Screen
    {
        private Background background;
        private Texture2D overlay;

        private SpriteFont font;
        private List<DrawableString> options;

        public PauseScreen()
            : base("Pause")
        {
            translucent = true;
        }

        public override void LoadContent(ContentManager content)
        {
            background = new Background(Color.Black * 0.6f);
            overlay = content.Load<Texture2D>("Images/TitleScreen");

            font = content.Load<SpriteFont>("Fonts/TitleFont");
            options = new List<DrawableString>();

            //Resume option
            options.Add(CreateOption("Resume", 0, () => manager.Pop()));

            //Save option
            options.Add(CreateOption("Save", 1, () => PlayerData.Save()));

            //Options option
            options.Add(CreateOption("Options", 2)); // add options

            //Exit option
            options.Add(CreateOption("Exit", 3, () =>
            {
                manager.Pop();
                manager.Pop();
            }));

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.newKeyState.IsKeyDown(Keys.Escape) && InputManager.oldKeyState.IsKeyUp(Keys.Escape))
                manager.Pop();

            foreach (DrawableString option in options)
                option.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            background.Draw(gameTime, spritebatch);
            spritebatch.Draw(overlay, Vector2.Zero, Color.White);

            foreach (DrawableString option in options)
                option.Draw(gameTime, spritebatch);

            base.Draw(gameTime, spritebatch);
        }

        private DrawableString CreateOption(string name, int index, Action onClick = null)
        {
            return new DrawableString(font, name, new Vector2(880, 280 + 60 * index) - font.MeasureString(name), Color.White,
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
