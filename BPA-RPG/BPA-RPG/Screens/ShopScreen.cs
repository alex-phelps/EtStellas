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

        public ShopScreen(List<Deal> deals) 
            : base("Shop")
        {
            this.deals = deals;
            textures = new List<GameObject>();
            itemNames = new List<DrawableString>();
            buyPrices = new List<DrawableString>();
            sellPrices = new List<DrawableString>();
        }

        public override void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("Fonts/ChoiceFont");
            menu = content.Load<Texture2D>("Images/ChoiceMenu");
            
            for (int i = 0; i < deals.Count; i++)
            {
                Vector2 pos = new Vector2(240, 115 + i * 35);

                textures.Add(new GameObject(deals[i].item.texture)
                {
                    position = pos
                });

                pos += new Vector2(80, 0);
                itemNames.Add(new DrawableString(font, deals[i].item.name, pos - font.MeasureString(deals[i].item.name) / 2, Color.White));

                pos += new Vector2(280, 0);
                if (deals[i].canBuy) 
                    buyPrices.Add(new DrawableString(font, deals[i].buyPrice + " " + deals[i].currency.ToString(),
                        pos - font.MeasureString(deals[i].buyPrice + " " + deals[i].currency.ToString()) / 2, Color.White));
                else buyPrices.Add(new DrawableString(font, "N/A", pos - font.MeasureString("N/A") / 2, Color.White));

                pos += new Vector2(150, 0);
                if (deals[i].canSell)
                    sellPrices.Add(new DrawableString(font, deals[i].sellPrice + " " + deals[i].currency.ToString(),
                        pos - font.MeasureString(deals[i].sellPrice + " " + deals[i].currency.ToString()) / 2, Color.White));
                else sellPrices.Add(new DrawableString(font, "N/A", pos - font.MeasureString("N/A") / 2, Color.White));
            }

            base.LoadContent(content);
        }
        
        public override void Update(GameTime gameTime)
        {
            foreach (DrawableString buyPrice in buyPrices)
            {
                int i = buyPrices.IndexOf(buyPrice);

                if (deals[i].canBuy && buyPrice.boundingRectangle.Contains(MainGame.input.newMouseState.Position))
                {
                    buyPrice.color = new Color(0, 60, 255);

                    if (MainGame.input.newMouseState.LeftButton == ButtonState.Pressed && MainGame.input.oldMouseState.LeftButton == ButtonState.Released)
                    {
                        if (PlayerData.GetMoney(deals[i].currency) >= deals[i].buyPrice)
                        {
                            PlayerData.inventory.Add(deals[i].item);
                            PlayerData.AddMoney(deals[i].currency, -deals[i].buyPrice);
                        }
                        else
                        {

                        }
                    }
                }
                else buyPrice.color = Color.White;
            }

            foreach (DrawableString sellPrice in sellPrices)
            {
                int i = sellPrices.IndexOf(sellPrice);

                if (deals[i].canSell && sellPrice.boundingRectangle.Contains(MainGame.input.newMouseState.Position))
                {
                    sellPrice.color = new Color(0, 60, 255);

                    if (MainGame.input.newMouseState.LeftButton == ButtonState.Pressed && MainGame.input.oldMouseState.LeftButton == ButtonState.Released)
                    {
                        if (PlayerData.inventory.Remove(deals[i].item))
                        {
                            PlayerData.AddMoney(deals[i].currency, deals[i].sellPrice);

                            // Add sounds here
                        }
                        else
                        {

                        }
                    }
                }
                else sellPrice.color = Color.White;
            }
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            spritebatch.Draw(menu, MainGame.WindowCenter, new Rectangle(0, 0, menu.Width, menu.Height), Color.White, 0, new Vector2(menu.Width / 2, menu.Height / 2), 1, SpriteEffects.None, 1);

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

            foreach (DrawableString sellPrice in sellPrices)
            {
                sellPrice.Draw(gameTime, spritebatch);
            }

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
