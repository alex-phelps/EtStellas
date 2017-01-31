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
using System.Text.RegularExpressions;
using BPA_RPG.GameItems.Weapons;

namespace BPA_RPG.Screens
{
    /// <summary>
    /// Screen for managing player inventory (hold) and items
    /// </summary>
    public class ShipHoldScreen : Screen
    {
        private List<GameItem> inventory => PlayerData.inventory;
        private List<Type> weaponHold => PlayerData.ship.weaponHold;
        private Weapon[] weapons => PlayerData.weapons;
        private int holdSize => PlayerData.ship.holdSize;
        private List<GameObject> itemRects;
        private ClickableObject holdScrollArrowTop;
        private ClickableObject holdScrollArrowBot;

        private GameItem mouseItem;

        private int firstRender;

        private SpriteFont font;
        private Texture2D menu;
        private Texture2D holdScrollArrowReg;
        private Texture2D holdScrollArrowBlue;
        private Texture2D partInv;

        /// <summary>
        /// Creates a new ShipHoldScreen
        /// </summary>
        public ShipHoldScreen()
            : base("Hold")
        {
            itemRects = new List<GameObject>();
        }

        public override void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("Fonts/ChoiceFont");
            menu = content.Load<Texture2D>("Images/ShipHoldMenu");
            holdScrollArrowReg = content.Load<Texture2D>("Images/HoldScrollArrow");
            holdScrollArrowBlue = content.Load<Texture2D>("Images/HoldScrollArrowBlue");
            partInv = content.Load<Texture2D>("Images/PartInv");

            holdScrollArrowTop = new ClickableObject(holdScrollArrowReg, 
                () => firstRender--, 
                () => holdScrollArrowTop.texture = holdScrollArrowBlue,
                () => holdScrollArrowTop.texture = holdScrollArrowReg)
            {
                position = new Vector2(325, 125)
            };

            holdScrollArrowBot = new ClickableObject(holdScrollArrowReg,
                () => firstRender++,
                () => holdScrollArrowBot.texture = holdScrollArrowBlue,
                () => holdScrollArrowBot.texture = holdScrollArrowReg)
            {
                position = new Vector2(325, 452), rotation = MathHelper.Pi
            };

            Texture2D itemHoverRect = content.Load<Texture2D>("Images/ItemHoverRect");
            for (int i = 0; i < 10; i++)
            {
                int k = i; // keep lambda from referencing i
                itemRects.Add(new ClickableObject(itemHoverRect,
                    () => 
                    {
                        if (k < inventory.Count - firstRender)
                        {
                            if (mouseItem == null)
                            {
                                mouseItem = inventory[k + firstRender];
                                inventory.RemoveAt(k + firstRender);
                            }
                            else
                            {
                                Rectangle rect = itemRects[k].boundingRectangle;
                                rect.Height /= 2;
                                if (rect.Contains(InputManager.newMouseState.Position))
                                    inventory.Insert(k + firstRender, mouseItem);
                                else inventory.Insert(k + firstRender + 1, mouseItem);

                                mouseItem = null;
                            }
                        }
                        else if (mouseItem != null)
                        {
                            inventory.Insert(inventory.Count, mouseItem);
                            mouseItem = null;
                        }
                    },
                    () => itemRects[k].visible = true,
                    () => itemRects[k].visible = false)
                {
                    position = new Vector2(337, 155 + (k * 30)),
                    visible = false
                });
            }

            base.LoadContent(content);
        }

        public override void Deactivated()
        {
            if (mouseItem != null)
            {
                inventory.Add(mouseItem);
                mouseItem = null;
            }

            base.Deactivated();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (ClickableObject rect in itemRects)
                rect.Update(gameTime);

            if (new Rectangle(520, 110, 60, 60).Contains(InputManager.newMouseState.Position) &&
                InputManager.newMouseState.LeftButton == ButtonState.Pressed && InputManager.oldMouseState.LeftButton == ButtonState.Released)
            {
                if (mouseItem == null)
                {
                    mouseItem = PlayerData.engine;
                    PlayerData.engine = null;
                }
                else if (mouseItem is Engine)
                {
                    Engine engine = PlayerData.engine;
                    PlayerData.engine = mouseItem as Engine;
                    mouseItem = engine;
                }
            }

            for (int i = 0; i < weaponHold.Count; i++)
            {
                if (new Rectangle(520, 180 + i * 70, 60, 60).Contains(InputManager.newMouseState.Position) &&
                    InputManager.newMouseState.LeftButton == ButtonState.Pressed && InputManager.oldMouseState.LeftButton == ButtonState.Released)
                {
                    if (mouseItem == null)
                    {
                        mouseItem = weapons[i];
                        weapons[i] = null;
                    }
                    else if (mouseItem.GetType() == weaponHold[i])
                    {
                        Weapon weapon = weapons[i];
                        weapons[i] = mouseItem as Weapon;
                        mouseItem = weapon;
                    }
                }
            }

            holdScrollArrowTop.Update(gameTime);
            holdScrollArrowBot.Update(gameTime);

            // Check Mouse scroll input
            if (InputManager.newMouseState.ScrollWheelValue > InputManager.oldMouseState.ScrollWheelValue) // scroll up
                firstRender--;
            else if (InputManager.newMouseState.ScrollWheelValue < InputManager.oldMouseState.ScrollWheelValue) //scroll down
                firstRender++;

            if (firstRender > inventory.Count - 10)
                firstRender = inventory.Count - 10;
            if (firstRender < 0)
                firstRender = 0;
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            spritebatch.End();
            spritebatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            spritebatch.Draw(menu, MainGame.WindowCenter, new Rectangle(0, 0, menu.Width, menu.Height), Color.White,
                0, new Vector2(menu.Width / 2, menu.Height / 2), 1, SpriteEffects.None, 1);
            
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
                spritebatch.DrawString(font, inventory[k].name, new Vector2(290, 145 + (i * 30)), Color.White);
            }

            holdScrollArrowTop.Draw(gameTime, spritebatch);
            holdScrollArrowBot.Draw(gameTime, spritebatch);

            spritebatch.Draw(partInv, new Vector2(517, 107), new Rectangle(0, 0, partInv.Width, partInv.Height), Color.White);

            for (int i = 0; i < weaponHold.Count; i++)
                spritebatch.Draw(partInv, new Vector2(517, 177 + i * 70), new Rectangle(0, 0, partInv.Width, partInv.Height), Color.White);

            spritebatch.DrawString(font, "[Engine]", new Vector2(700, 125) - font.MeasureString("[Engine]") / 2, Color.White);
            if (PlayerData.engine != null)
                spritebatch.Draw(PlayerData.engine.texture, new Vector2(550, 140), new Rectangle(0, 0, 20, 20), Color.White,
                    0, new Vector2(PlayerData.engine.width / 2, PlayerData.engine.height / 2), 3, SpriteEffects.None, 1);

            string name = PlayerData.engine != null ? PlayerData.engine.name : "None!";
            spritebatch.DrawString(font, name, new Vector2(700, 155) - font.MeasureString(name) / 2, Color.White);

            for (int i = 0; i < weaponHold.Count; i++)
            {
                //Put a space before capital letters (not including first letter)
                string type = new Regex(@"(?!^)(?=[A-Z])").Replace(weaponHold[i].Name, " ");

                spritebatch.DrawString(font, "[" + type + "]", new Vector2(700, 195 + i * 70) - font.MeasureString("[" + type + "]") / 2, Color.White);
                if (weapons[i] != null)
                    spritebatch.Draw(weapons[i].texture, new Vector2(550, 210 + i * 70), new Rectangle(0, 0, 20, 20), Color.White,
                        0, new Vector2(weapons[i].width / 2, weapons[i].height / 2), 3, SpriteEffects.None, 1); ;
                
                spritebatch.DrawString(font, weapons[i] != null ? weapons[i].name : "None!", new Vector2(700, 225 + i * 70) - font.MeasureString(weapons[i] != null ? weapons[i].name : "None!") / 2, Color.White);
            }

            if (mouseItem != null)
                spritebatch.Draw(mouseItem.texture, InputManager.oldMouseState.Position.ToVector2() - new Vector2(mouseItem.width, mouseItem.height), 
                    new Rectangle(0, 0, 20, 20), Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 1);

            spritebatch.End();
            spritebatch.Begin();


            //Draw item infobox for inv
            for (int i = 0; i < 10; i++)
                if (itemRects[i].visible && !(i + firstRender >= inventory.Count))
                    inventory[i + firstRender].DrawInfo(spritebatch);

            //Draw item infobox for engine and weapons
            if (new Rectangle(520, 110, 60, 60).Contains(InputManager.newMouseState.Position))
                PlayerData.engine?.DrawInfo(spritebatch);
            for (int i = 0; i < weaponHold.Count; i++)
            {
                if (new Rectangle(520, 180 + i * 70, 60, 60).Contains(InputManager.newMouseState.Position))
                    weapons[i]?.DrawInfo(spritebatch);
            }

            base.Draw(gameTime, spritebatch);
        }
    }
}
