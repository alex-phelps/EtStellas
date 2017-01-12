using BPA_RPG.GameItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BPA_RPG.Screens
{
    public class JettisonScreen : Screen
    {
        private List<GameItem> inventory => PlayerData.inventory;
        private List<GameItem> jettison;

        public JettisonScreen()
            : base("Jettison")
        {
            jettison = new List<GameItem>();
        }

        public override void LoadContent(ContentManager content)
        {


            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {


            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {


            base.Draw(gameTime, spritebatch);
        }
    }
}
