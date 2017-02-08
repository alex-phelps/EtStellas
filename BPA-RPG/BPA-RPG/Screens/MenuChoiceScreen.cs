using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BPA_RPG.Choice;
using Microsoft.Xna.Framework.Content;
using BPA_RPG.GameItems;

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

                synopsis = new DrawableString(choiceFont, "", new Vector2(240, 105), Color.White);
                options = new List<DrawableString>();

                foreach (string line in value.synopsis)
                {
                    synopsis.text += line + "\n";
                }

                //Keep track of number of invis option so that visible options are draw in correct place on screen
                int numInvis = 0;
                for (int i = 0; i < value.options.Count; i++)
                {
                    bool canChoose = value.options[i].MeetsRequirements();

                    //If you can't choose and option is "invisible", dont draw it.
                    if (canChoose || !value.options[i].invisible)
                    {
                        string requirements = "";

                        //If requirements include money and/or items, tell the player about them. Variables are kept secret
                        if (value.options[i].moneyRequirements.Count != 0 || value.options[i].itemRequirements.Count != 0)
                        {
                            requirements = "(Need: ";

                            //Money requirements
                            foreach (KeyValuePair<Currency, int> money in value.options[i].moneyRequirements)
                            {
                                if (money.Value < 0)
                                    requirements += "< ";
                                else requirements += "> ";

                                requirements += Math.Abs(money.Value) + " " + money.Key + "  ";

                            }

                            //Item requirements
                            foreach (KeyValuePair<GameItem, int> item in value.options[i].itemRequirements)
                            {
                                if (item.Value < 0)
                                    requirements += "< ";
                                else requirements += "> ";

                                requirements += Math.Abs(item.Value) + " " + item.Key.name + "  ";

                            }

                            requirements = requirements.TrimEnd();
                            requirements += ")";
                        }

                        int j = i; //keep lambda from referencing i
                        int k = j - numInvis; //correct index

                        options.Add(new DrawableString(choiceFont, value.options[i].synopsis + (canChoose ? "" : " " + requirements),
                            synopsis.position + new Vector2(0, synopsis.boundingRectangle.Height + 20 + 30 * (k)),
                            canChoose ? Color.White : Color.Gray, 
                            () => { if (canChoose) value.options[j].Activate(); },
                            () => { if (canChoose) options[k].color = new Color(0, 60, 255); },
                            () => { if (canChoose) options[k].color = Color.White; }));
                    }
                    else numInvis++;
                }
            }
        }
        
        private readonly string scriptName;

        public ShopScreen shop { get; private set; }
        public ShipYardScreen shipyard { get; private set; }

        private DrawableString synopsis;
        private List<DrawableString> options;
        private SpriteFont choiceFont;
        private Texture2D choiceMenu;

        public MenuChoiceScreen(string title, string scriptName)
            : base(title)
        {
            this.scriptName = scriptName;
            translucent = true;
        }

        public override void Activated()
        {
            base.Activated();

            //Re-parse choice
            currentChoice = currentChoice;
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
            for (int i = 0; i < options.Count; i++)
                options[i].Update(gameTime);
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            spritebatch.Draw(choiceMenu, MainGame.WindowCenter, new Rectangle(0, 0, choiceMenu.Width, choiceMenu.Height), Color.White, 0, new Vector2(choiceMenu.Width / 2, choiceMenu.Height / 2), 1, SpriteEffects.None, 1);

            synopsis.Draw(gameTime, spritebatch);
            foreach (DrawableString option in options)
                option.Draw(gameTime, spritebatch);

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

                if (File.Exists("Content/Scripts/" + scriptName + ".txt"))
                    file = File.OpenText("Content/Scripts/" + scriptName + ".txt");
                else file = File.OpenText("Content/Scripts/DefaultPlanetScript.txt");

                //Loop through line for the choice
                List<string> lines = new List<string>();
                while (!file.EndOfStream)
                {
                    lines.Add(file.ReadLine());
                }

                currentChoice = MenuChoice.ChoiceFromText(this, lines, manager);

                MainGame.eventLogger.Log(this, "Loaded script \"" + scriptName + "\"");
            }
            catch (Exception e)
            {
                MainGame.eventLogger.Log(this, "ERROR: " + e.Message);
                throw e;
            }

            try
            {
                StreamReader file = File.OpenText("Content/Scripts/" + scriptName + "Shop.txt");

                //Loop through line for the choice
                List<string> lines = new List<string>();
                while (!file.EndOfStream)
                {
                    lines.Add(file.ReadLine());
                }

                shop = ShopScreen.ShopFromText(lines);

                MainGame.eventLogger.Log(this, "Loaded script \"" + scriptName + "Shop\"");
            }
            catch (FileNotFoundException)
            {
                MainGame.eventLogger.Log(this, "No shop file found, proceeding without shop.");
            }
            catch (Exception e)
            {
                MainGame.eventLogger.Log(this, "ERROR: " + e.Message);
                throw e;
            }

            try
            {
                StreamReader file = File.OpenText("Content/Scripts/" + scriptName + "Shipyard.txt");

                //Loop through line for the choice
                List<string> lines = new List<string>();
                while (!file.EndOfStream)
                {
                    lines.Add(file.ReadLine());
                }

                shipyard = ShipYardScreen.ShipYardFromText(lines);

                MainGame.eventLogger.Log(this, "Loaded script \"" + scriptName + "Shipyard\"");
            }
            catch (FileNotFoundException)
            {
                MainGame.eventLogger.Log(this, "No shipyard file found, proceeding without shipyard.");
            }
            catch (Exception e)
            {
                MainGame.eventLogger.Log(this, "ERROR: " + e.Message);
                throw e;
            }
        }
    }
}
