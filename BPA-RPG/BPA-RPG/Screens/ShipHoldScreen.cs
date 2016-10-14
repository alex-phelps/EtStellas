using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using BPA_RPG.GameItems;

namespace BPA_RPG.Screens
{
    public class ShipHoldScreen : Screen
    {
        private List<GameItem> inventory => PlayerData.inventory;
        private int holdSize => PlayerData.ship.holdSize;

        private SpriteFont font;
        private Texture2D menu;

        public ShipHoldScreen()
            : base("Hold")
        {

        }

        public override void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("Fonts/ChoiceFont");
            menu = content.Load<Texture2D>("Images/ChoiceMenu");

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {


            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            spritebatch.Draw(menu, MainGame.WindowCenter, new Rectangle(0, 0, menu.Width, menu.Height), Color.White, 0, new Vector2(menu.Width / 2, menu.Height / 2), 1, SpriteEffects.None, 1);

            base.Draw(gameTime, spritebatch);
        }
    }
}
