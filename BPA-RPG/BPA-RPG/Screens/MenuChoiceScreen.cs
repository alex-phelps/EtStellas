using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BPA_RPG.Choice;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace BPA_RPG.Screens
{
    public class MenuChoiceScreen : Screen
    {
        private MenuChoice CurrentChoice;
        public MenuChoice currentChoice
        {
            get
            {
                return CurrentChoice;
            }

            set
            {
                CurrentChoice = value;

                synopsis = new DrawableString(choiceFont, "", new Vector2(250, 100), Color.White);
                options = new List<DrawableString>();

                foreach (string line in value.synopsis)
                {
                    synopsis.text += line + "\n";
                }

                for (int i = 0; i < value.options.Count; i++)
                {
                    options.Add(new DrawableString(choiceFont, value.options[i].synopsis, synopsis.position + new Vector2(0, synopsis.boundingRectangle.Y + 30 + 40 * i), Color.White));
                }
            }
        }

        private string scriptName;
        private DrawableString synopsis;
        private List<DrawableString> options;
        private SpriteFont choiceFont;
        private Texture2D choiceMenu;
        private MouseState oldMouseState;

        public MenuChoiceScreen(string scriptName)
        {
            this.scriptName = scriptName;
            translucent = true;
        }

        public override void LoadContent(ContentManager content)
        {
            choiceFont = content.Load<SpriteFont>("Fonts/ChoiceFont");
            choiceMenu = content.Load<Texture2D>("Images/ChoiceMenu");

            LoadEvents();

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState newMouseState = Mouse.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                manager.Pop();
            }

            for (int i = 0; i < options.Count; i++)
                if (options[i].boundingRectangle.Contains(newMouseState.Position) &&
                    newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
                    currentChoice.options[i].Activate();

            oldMouseState = newMouseState;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            spritebatch.Draw(choiceMenu, MainGame.WindowCenter, new Rectangle(0, 0, choiceMenu.Width, choiceMenu.Height), Color.White, 0, new Vector2(choiceMenu.Width / 2, choiceMenu.Height / 2), 1, SpriteEffects.None, 1);

            synopsis.Draw(spritebatch, gameTime);
            foreach (DrawableString option in options)
            {
                option.Draw(spritebatch, gameTime);
            }

            base.Draw(gameTime, spritebatch);
        }

        /// <summary>
        /// Loads the dedicated script for this screen's planet.
        /// </summary>
        private void LoadEvents()
        {
            try
            {
                StreamReader file;

                file = File.OpenText("Content/Scripts/" + scriptName + ".txt");

                //Loop through line for the choice
                List<string> lines = new List<string>();
                while (!file.EndOfStream)
                {
                    lines.Add(file.ReadLine());
                }

                currentChoice = MenuChoice.ChoiceFromText(this, lines);
            }
            catch (Exception e)
            {
                MainGame.eventLogger.Log(this, "ERROR: " + e.Message);
                throw e;
            }
        }
    }
}
