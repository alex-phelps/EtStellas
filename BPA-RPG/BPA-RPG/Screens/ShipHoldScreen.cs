﻿using System;
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
        private GameObject holdScrollArrowTop;
        private GameObject holdScrollArrowBot;

        private int firstRender;

        private MouseState oldMouseState;

        private SpriteFont font;
        private Texture2D menu;
        private Texture2D holdScrollArrowReg;
        private Texture2D holdScrollArrowBlue;

        public ShipHoldScreen()
            : base("Hold")
        {
            itemRects = new List<GameObject>();

            for (int i = 0; i < 20; i++)
                inventory.Add(GameItem.Fuel);
        }

        public override void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("Fonts/ChoiceFont");
            menu = content.Load<Texture2D>("Images/ShipHoldMenu");
            holdScrollArrowReg = content.Load<Texture2D>("Images/HoldScrollArrow");
            holdScrollArrowBlue = content.Load<Texture2D>("Images/HoldScrollArrowBlue");

            holdScrollArrowTop = new GameObject(holdScrollArrowReg) { position = new Vector2(325, 125) };
            holdScrollArrowBot = new GameObject(holdScrollArrowReg) { position = new Vector2(325, 452), rotation = (float)Math.PI };

            Texture2D itemHoverRect = content.Load<Texture2D>("Images/ItemHoverRect");
            for (int i = 0; i < 10; i++)
            {
                itemRects.Add(new GameObject(itemHoverRect) { position = new Vector2(337, 155 + (i * 30)), visible = false } );
            }

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState newMouseState = Mouse.GetState();

            foreach (GameObject obj in itemRects)
            {
                if (obj.boundingRectangle.Contains(Mouse.GetState().Position))
                {
                    obj.visible = true;
                }
                else obj.visible = false;
            }

            // Check if mouse is on up scroll arrow
            if (holdScrollArrowTop.boundingRectangle.Contains(newMouseState.Position))
            {
                holdScrollArrowTop.texture = holdScrollArrowBlue;

                // If clicking
                if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
                    firstRender--;
            }
            else holdScrollArrowTop.texture = holdScrollArrowReg;

            // Check if mouse is on down scroll arrow
            if (holdScrollArrowBot.boundingRectangle.Contains(newMouseState.Position))
            {
                holdScrollArrowBot.texture = holdScrollArrowBlue;

                // If clicking
                if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
                    firstRender++;
            }
            else holdScrollArrowBot.texture = holdScrollArrowReg;

            // Check Mouse scroll input
            if (newMouseState.ScrollWheelValue > oldMouseState.ScrollWheelValue) // scroll up
                firstRender--;
            else if (newMouseState.ScrollWheelValue < oldMouseState.ScrollWheelValue) //scroll down
                firstRender++;

            if (firstRender < 0)
                firstRender = 0;
            else if (firstRender > inventory.Count - 10)
                firstRender = inventory.Count - 10;

            oldMouseState = newMouseState;
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

            for (int i = 0; i < 10; i++)
            {
                int k = i + firstRender;

                if (k >= inventory.Count)
                    break;

                spritebatch.Draw(inventory[k].texture, new Vector2(250, 145 + (i * 30)), Color.White);
                spritebatch.DrawString(font, k + inventory[k].name, 
                    new Vector2(300 + font.MeasureString(inventory[k].name).X / 2, 145 + (i * 30)), Color.White);

                holdScrollArrowTop.Draw(gameTime, spritebatch);
                holdScrollArrowBot.Draw(gameTime, spritebatch);
            }

            base.Draw(gameTime, spritebatch);
        }
    }
}