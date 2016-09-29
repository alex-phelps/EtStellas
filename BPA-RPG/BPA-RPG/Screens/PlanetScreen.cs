using BPA_RPG.GameObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BPA_RPG.Screens
{
    class PlanetScreen : Screen
    {
        Planet planet;

        public PlanetScreen(Planet planet)
        {
            this.planet = planet;

            translucent = true;

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

            
        }
    }
}
