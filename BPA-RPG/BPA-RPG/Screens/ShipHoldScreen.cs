using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using BPA_RPG.GameItems;
using BPA_RPG.GameObjects;
using Microsoft.Xna.Framework.Input;

namespace BPA_RPG.Screens
{
    public class ShipHoldScreen : Screen
    {
        private List<GameItem> inventory => PlayerData.inventory;
        private int holdSize => PlayerData.ship.holdSize;
        private List<GameObject> itemRects;

        private int firstRender = 2;

        private SpriteFont font;
        private Texture2D menu;

        public ShipHoldScreen()
            : base("Hold")
        {
            itemRects = new List<GameObject>();

            for (int i = 0; i < 7; i++)
                inventory.Add(GameItem.Fuel);
        }

        public override void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("Fonts/ChoiceFont");
            menu = content.Load<Texture2D>("Images/ShipHoldMenu");

            Texture2D itemHoverRect = content.Load<Texture2D>("Images/ItemHoverRect");
            for (int i = 0; i < 11; i++)
            {
                itemRects.Add(new GameObject(itemHoverRect) { position = new Vector2(337, 140 + (i * 30)), visible = false } );
            }

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GameObject obj in itemRects)
            {
                if (obj.boundingRectangle.Contains(Mouse.GetState().Position))
                {
                    obj.visible = true;
                }
                else obj.visible = false;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            spritebatch.Draw(menu, MainGame.WindowCenter, new Rectangle(0, 0, menu.Width, menu.Height), Color.White, 0, new Vector2(menu.Width / 2, menu.Height / 2), 1, SpriteEffects.None, 1);
            
            for (int i = 0; i < itemRects.Count; i++)
            {
                if (firstRender + i >= inventory.Count)
                    break;

                itemRects[i].Draw(gameTime, spritebatch);
            }

            for (int i = 0; i < 11; i++)
            {
                int k = i + firstRender;

                if (k >= inventory.Count)
                    break;

                spritebatch.Draw(inventory[k].texture, new Vector2(250, 130 + (i * 30)), Color.White);
                spritebatch.DrawString(font, k + inventory[k].name, 
                    new Vector2(300 + font.MeasureString(inventory[k].name).X / 2, 130 + (i * 30)), Color.White);
            }

            base.Draw(gameTime, spritebatch);
        }
    }
}
