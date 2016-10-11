using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BPA_RPG.GameObjects;

namespace BPA_RPG.Screens
{
    public class ShopScreen : Screen
    {
        private readonly List<Deal> deals;
        private readonly List<GameObject> textures;
        private readonly List<DrawableString> itemNames;
        private readonly List<DrawableString> buyPrices;
        private readonly List<DrawableString> sellPrices;

        private SpriteFont font;
        private Texture2D menu;
        private MouseState oldMouseState;

        public ShopScreen(List<Deal> deals) 
            : base("Shop")
        {
            this.deals = deals;
            buyPrices = new List<DrawableString>();
            sellPrices = new List<DrawableString>();
        }

        public override void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("Fonts/ChoiceFont");
            menu = content.Load<Texture2D>("Images/ChoiceMenu");
            
            for (int i = 0; i < deals.Count; i++)
            {
                if (deals[i].canBuy)
                    buyPrices.Add(new DrawableString(font, deals[i].buyPrice + " " + deals[i].currency.ToString()));
                else buyPrices.Add(new DrawableString(font, "N/A"));

                if (deals[i].canSell)
                    sellPrices.Add(new DrawableString(font, deals[i].sellPrice + " " + deals[i].currency.ToString()));
                else sellPrices.Add(new DrawableString(font, "N/A"));
            }

            base.LoadContent(content);
        }
        
        public override void Update(GameTime gameTime)
        {
            MouseState newMouseState = Mouse.GetState();
            
            

            oldMouseState = newMouseState;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            spritebatch.Draw(menu, MainGame.WindowCenter, new Rectangle(0, 0, menu.Width, menu.Height), Color.White, 0, new Vector2(menu.Width / 2, menu.Height / 2), 1, SpriteEffects.None, 1);



            base.Draw(gameTime, spritebatch);
        }

        public static ShopScreen ShopFromText(List<string> lines)
        {
            List<Deal> deals = new List<Deal>();

            foreach (string line in lines)
            {
                deals.Add(Deal.DealFromText(line));
            }

            return new ShopScreen(deals);
        }
    }
}
