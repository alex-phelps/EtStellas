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
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

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

        private SoundEffectInstance buy;

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

        ~ShipYardScreen()
        {
            if (MainGame.ContentUnloaded)
                return;
            buy.Dispose();
        }

        public override void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("Fonts/ChoiceFont");
            menu = content.Load<Texture2D>("Images/ChoiceMenu");

            for (int i = 0; i < deals.Count; i++)
            {
                Vector2 pos = new Vector2(312 + (i % 3) * 200, 145 + (i / 3) * 210);
                
                textures.Add(new GameObject(deals[i].item.texture)
                {
                    position = pos,
                    source = new Rectangle(0, 0, deals[i].item.texture.Width / 2, deals[i].item.texture.Height)
                });
                textures[i].ScaleTo(140, 140);

                pos += new Vector2(0, 75);

                int k = i; //keep lambda from referencing i

                itemNames.Add(new DrawableString(font, deals[i].item.name, pos - font.MeasureString(deals[i].item.name) / 2, Color.Gold));

                pos += new Vector2(0, 20);

                string text = "Buy: " + (deals[i].canBuy ? deals[i].buyPrice + " " + deals[i].currency : "N/A");
                buyPrices.Add(new DrawableString(font, text, pos - font.MeasureString(text) / 2, Color.White, () =>
                {
                    if (PlayerData.GetMoney(deals[k].currency) >= deals[k].buyPrice && deals[k].item as Ship != null &&
                            !PlayerData.ship.Equals(deals[k].item as Ship))
                    {
                        PlayerData.ship.baseShip = (Ship)deals[k].item;
                        PlayerData.AddMoney(deals[k].currency, -deals[k].buyPrice);

                        buy.Pause(); //because stop doesnt work for some reason
                        buy.Play();
                    }
                    else
                    {
                        manager.Push(new InfoBoxScreen("No Money", "Not enough " + deals[k].currency));
                    }
                },
                () => buyPrices[k].color = new Color(0, 60, 255),
                () => buyPrices[k].color = Color.White));
            }

            //Sounds
            buy = SoundManager.GetEffectInstance("Buy1");

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (DrawableString buyPrice in buyPrices)
                buyPrice.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            spritebatch.Draw(menu, MainGame.WindowCenter, new Rectangle(0, 0, menu.Width, menu.Height), Color.White, 0, new Vector2(menu.Width / 2, menu.Height / 2), 1, SpriteEffects.None, 1);

            //Draw w/o linear interpol
            spritebatch.End();
            spritebatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            foreach (GameObject texture in textures)
            {
                texture.Draw(gameTime, spritebatch);
            }
            spritebatch.End();
            spritebatch.Begin();

            foreach (DrawableString name in itemNames)
            {
                name.Draw(gameTime, spritebatch);
            }

            foreach (DrawableString buyPrice in buyPrices)
            {
                buyPrice.Draw(gameTime, spritebatch);
            }

            for (int i = 0; i < textures.Count; i++)
            {
                Rectangle rect = textures[i].boundingRectangle;
                if (itemNames[i].boundingRectangle.Contains(InputManager.newMouseState.Position) ||
                    new Rectangle(rect.X + rect.Width / 4, rect.Y, rect.Width / 2, rect.Height).Contains(InputManager.newMouseState.Position))
                    deals[i].item.DrawInfo(spritebatch);

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
