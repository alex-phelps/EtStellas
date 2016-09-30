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

namespace BPA_RPG.Screens
{
    class PlanetScreen : Screen
    {
        private Planet planet;
        private MenuChoice currentChoice;        


        public PlanetScreen(Planet planet)
        {
            this.planet = planet;
            translucent = true;
            
            LoadEvents();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            base.Draw(gameTime, spritebatch);
        }

        private void LoadEvents()
        {
            StreamReader file;

            try
            {
                file = File.OpenText("Content/PlanetScripts/" + planet.name.Replace(" ", "") + ".txt");
            }
            catch (Exception e)
            {
                MainGame.eventLogger.Log(this, "ERROR: " + e.Message);
                throw e;
            }

            //Loop through line for the choice
            List<string> lines = new List<string>();
            while (!file.EndOfStream)
            {
                lines.Add(file.ReadLine());
            }

            currentChoice = MenuChoice.CreateChoice(lines);
        }
    }
}
