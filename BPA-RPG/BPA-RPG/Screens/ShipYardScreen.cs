using BPA_RPG.GameItems;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using BPA_RPG.GameObjects;
using Microsoft.Xna.Framework;

namespace BPA_RPG.Screens
{
    public class ShipYardScreen : Screen
    {
        private readonly List<Deal> deals;
        private readonly List<GameObject> textures;
        private readonly List<DrawableString> itemNames;
        private readonly List<DrawableString> buyPrices;

        private SpriteFont font;
        private Texture2D menu;

        public ShipYardScreen(List<Deal> deals)
            : base("Ship Yard")
        {
            for (int i = 0; i < deals.Count; i++)
            {
                if (!(deals[i].item is Ship))
                {
                    deals.Remove(deals[i]);
                    i--;
                }
            }

            this.deals = deals;
            textures = new List<GameObject>();
            itemNames = new List<DrawableString>();
            buyPrices = new List<DrawableString>();
        }

        public override void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("Fonts/ChoiceFont");
            menu = content.Load<Texture2D>("Images/ChoiceMenu");

            for (int i = 0; i < deals.Count; i++)
            {
                Vector2 pos = new Vector2(250 + i * 50, 130);
                textures.Add(new GameObject(deals[i].item.texture)
                {
                    position = pos
                });

                pos += new Vector2(0, 30);
            }

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            spritebatch.Draw(menu, MainGame.WindowCenter, Color.White);

            foreach (GameObject texture in textures)
            {
                texture.Draw(gameTime, spritebatch);
            }

            foreach (DrawableString name in itemNames)
            {
                name.Draw(gameTime, spritebatch);
            }

            foreach (DrawableString buyPrice in buyPrices)
            {
                buyPrice.Draw(gameTime, spritebatch);
            }

            base.Draw(gameTime, spritebatch);
        }

        public static ShipYardScreen ShipYardFromText(List<string> lines)
        {
            List<Deal> deals = new List<Deal>();

            foreach (string line in lines)
            {
                deals.Add(Deal.DealFromText(line));
            }

            return new ShipYardScreen(deals);
        }
    }
}
