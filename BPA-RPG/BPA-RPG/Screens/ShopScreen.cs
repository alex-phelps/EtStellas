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
using Microsoft.Xna.Framework.Audio;

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
        private SpriteFont infoFont;
        private Texture2D menu;

        private SoundEffectInstance buy;

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
            infoFont = content.Load<SpriteFont>("Fonts/InfoFont");
            menu = content.Load<Texture2D>("Images/ChoiceMenu");
            
            for (int i = 0; i < deals.Count; i++)
            {
                Vector2 pos = new Vector2(240, 155 + i * 35);

                textures.Add(new GameObject(deals[i].item.texture)
                {
                    position = pos
                });

                pos += new Vector2(30, 0);
                itemNames.Add(new DrawableString(font, deals[i].item.name, new Vector2(pos.X, pos.Y - font.MeasureString(deals[i].item.name).Y / 2), Color.White));

                pos += new Vector2(330, 0);

                int k = i; //keep lambdas from referencing i

                if (deals[i].canBuy) 
                    buyPrices.Add(
                        new DrawableString(font, deals[i].buyPrice + " " + deals[i].currency.ToString(),
                        pos - font.MeasureString(deals[i].buyPrice + " " + deals[i].currency.ToString()) / 2, Color.White, () =>
                        {
                            if (PlayerData.GetMoney(deals[k].currency) >= deals[k].buyPrice)
                            {
                                PlayerData.inventory.Add(deals[k].item);
                                PlayerData.AddMoney(deals[k].currency, -deals[k].buyPrice);

                                buy.Pause(); //stop doesnt work for some reason
                                buy.Play();
                            }
                            else
                            {
                                manager.Push(new InfoBoxScreen("No Money", "Not enough " + deals[k].currency));
                            }
                        },
                        () => buyPrices[k].color = new Color(0, 60, 255),
                        () => buyPrices[k].color = Color.White));
                else buyPrices.Add(
                    new DrawableString(font, "N/A", pos - font.MeasureString("N/A") / 2, Color.White));

                pos += new Vector2(150, 0);
                if (deals[i].canSell)
                    sellPrices.Add(
                        new DrawableString(font, deals[i].sellPrice + " " + deals[i].currency.ToString(),
                        pos - font.MeasureString(deals[i].sellPrice + " " + deals[i].currency.ToString()) / 2, Color.White, () =>
                        {
                            if (PlayerData.inventory.Remove(deals[k].item))
                            {
                                PlayerData.AddMoney(deals[k].currency, deals[k].sellPrice);

                                buy.Pause();
                                buy.Play();
                            }
                            else
                            {
                                manager.Push(new InfoBoxScreen("No Item", "Not enough " + deals[k].item.name));
                            }
                        },
                        () => sellPrices[k].color = new Color(0, 60, 255),
                        () => sellPrices[k].color = Color.White));
                else sellPrices.Add(new DrawableString(font, "N/A", pos - font.MeasureString("N/A") / 2, Color.White));
            }

            //Sounds
            buy = SoundEffectManager.GetEffectInstance("Buy1");

            base.LoadContent(content);
        }
        
        public override void Update(GameTime gameTime)
        {
            foreach (DrawableString buyPrice in buyPrices)
                buyPrice.Update(gameTime);

            foreach (DrawableString sellPrice in sellPrices)
                sellPrice.Update(gameTime);
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            spritebatch.Draw(menu, MainGame.WindowCenter, new Rectangle(0, 0, menu.Width, menu.Height), Color.White, 0, new Vector2(menu.Width / 2, menu.Height / 2), 1, SpriteEffects.None, 1);

            //Draw info text
            spritebatch.DrawString(infoFont, "Item", new Vector2(280, 115) - font.MeasureString("Item") / 2, Color.Gold);
            spritebatch.DrawString(infoFont, "Buy", new Vector2(600, 115) - font.MeasureString("Buy") / 2, Color.Gold);
            spritebatch.DrawString(infoFont, "Sell", new Vector2(750, 115) - font.MeasureString("Sell") / 2, Color.Gold);

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


            for (int i = 0; i < itemNames.Count; i++)
            {
                if (itemNames[i].boundingRectangle.Contains(InputManager.newMouseState.Position))
                    deals[i].item.DrawInfo(spritebatch);
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
