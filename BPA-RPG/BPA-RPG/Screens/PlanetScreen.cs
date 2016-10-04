using BPA_RPG.GameObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BPA_RPG.Choice;
using Microsoft.Xna.Framework.Content;

namespace BPA_RPG.Screens
{
    class PlanetScreen : Screen
    {
        private MenuChoice CurrentChoice;
        private MenuChoice currentChoice
        {
            get
            {
                return CurrentChoice;
            }

            set
            {
                CurrentChoice = value;

                synopsis = new DrawableString(choiceFont, "", new Vector2(300, 150), Color.White);
                options = new List<DrawableString>();

                foreach (string line in value.synopsis)
                {
                    synopsis.text += line + "\n";
                }

                for (int i = 0; i < value.options.Count; i++)
                {
                    string optionSynopsis = "";
                    foreach (string line in value.options[i].synopsis)
                    {
                        optionSynopsis += line + "\n";
                    }

                    options.Add(new DrawableString(choiceFont, optionSynopsis, synopsis.position + new Vector2(0, synopsis.boundingRectangle.Y + 50 + 20 * i), Color.White));
                }
            }
        }

        private Planet planet;
        private DrawableString synopsis;
        private List<DrawableString> options;

        private SpriteFont choiceFont;
        private Texture2D planetMenu;

        public PlanetScreen(Planet planet)
        {
            this.planet = planet;
            translucent = true;
        }

        public override void LoadContent(ContentManager content)
        {
            choiceFont = content.Load<SpriteFont>("Fonts/ChoiceFont");
            planetMenu = content.Load<Texture2D>("Images/PlanetMenu");

            LoadEvents();

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            if (Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                manager.Pop();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            spritebatch.Draw(planetMenu, MainGame.WindowCenter, new Rectangle(0, 0, planetMenu.Width, planetMenu.Height), Color.White, 0, new Vector2(planetMenu.Width / 2, planetMenu.Height / 2), 1, SpriteEffects.None, 1);

            

            base.Draw(gameTime, spritebatch);
        }

        private void LoadEvents()
        {
            StreamReader file;

            file = File.OpenText("Content/PlanetScripts/" + planet.name.Replace(" ", "") + ".txt");

            //Loop through line for the choice
            List<string> lines = new List<string>();
            while (!file.EndOfStream)
            {
                lines.Add(file.ReadLine());
            }

            currentChoice = MenuChoice.ChoiceFromText(lines);

            try
            {
                
            }
            catch (Exception e)
            {
                MainGame.eventLogger.Log(this, "ERROR: " + e.Message);
                throw e;
            }
        }
    }
}
