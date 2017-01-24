using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using BPA_RPG.GameObjects;
using Microsoft.Xna.Framework.Input;

namespace BPA_RPG.Screens
{
    class TabMenuScreen : Screen
    {
        private readonly List<Screen> menuScreens;
        private readonly bool canExit;

        private List<ClickableObject> menuTabs;
        private List<DrawableString> tabStrings;
        private ClickableObject closeX;
        private ShipInfoBox shipInfoBox;
        private int selectedScreen = 0;

        private Background background;

        public TabMenuScreen(bool canExit, params Screen[] menuScreens)
            : base("Tab Menu")
        {
            this.canExit = canExit;
            this.menuScreens = new List<Screen>(menuScreens);

            translucent = true;
            menuTabs = new List<ClickableObject>();
            tabStrings = new List<DrawableString>();
        }

        public TabMenuScreen(params Screen[] menuScreens)
            : this(true, menuScreens)
        {
        }

        public override void Activated()
        {
            menuScreens[selectedScreen].Activated();

            base.Activated();
        }

        public override void Deactivated()
        {
            menuScreens[selectedScreen].Deactivated();

            base.Deactivated();
        }

        public override void LoadContent(ContentManager content)
        {
            Texture2D menuTab = content.Load<Texture2D>("Images/MenuTab");
            SpriteFont tabFont = content.Load<SpriteFont>("Fonts/ChoiceTabFont");
            
            for (int i = 0; i < menuScreens.Count; i++)
            {
                menuScreens[i].manager = manager;
                menuScreens[i].LoadContent(content);

                // Expression returns null if there is no shipyard or if screen is not a MenuChoiceScreen
                if ((menuScreens[i] as MenuChoiceScreen)?.shipyard != null)
                    // Adds a shipyard if the screen has one
                    menuScreens.Insert(i + 1, ((MenuChoiceScreen)menuScreens[i]).shipyard);


                // Same with shop
                if ((menuScreens[i] as MenuChoiceScreen)?.shop != null)
                    menuScreens.Insert(i + 1, ((MenuChoiceScreen)menuScreens[i]).shop);
                

                int k = i; // keeps lambda from referencing i
                menuTabs.Add(new ClickableObject(menuTab, () =>
                {
                    if (selectedScreen != k)
                    {
                        menuScreens[selectedScreen].Deactivated();
                        selectedScreen = k;
                        menuScreens[selectedScreen].Activated();
                    }
                })
                {
                    position = MainGame.WindowCenter - new Vector2(230 - (i * 150), 242)
                });

                tabStrings.Add(new DrawableString(tabFont, menuScreens[i].title, MainGame.WindowCenter - new Vector2(230 - (i * 150), 242) - tabFont.MeasureString(menuScreens[i].title) / 2, Color.White));
            }

            closeX = new ClickableObject(content.Load<Texture2D>("Images/CloseX"), () => manager.Pop())
            {
                position = MainGame.WindowCenter + new Vector2(331, -206)
            };

            shipInfoBox = new ShipInfoBox(content);

            background = new Background(Color.Black * .6f);

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            if (canExit && (InputManager.newKeyState.IsKeyDown(Keys.Enter) && InputManager.oldKeyState.IsKeyUp(Keys.Enter) ||
                InputManager.newMouseState.RightButton == ButtonState.Pressed && InputManager.oldMouseState.RightButton == ButtonState.Released))
            {
                manager.Pop();
            }

            menuScreens[selectedScreen].Update(gameTime);

            foreach (ClickableObject tab in menuTabs)
                tab.Update(gameTime);

            if (canExit)
                closeX.Update(gameTime);

            shipInfoBox.Update(gameTime);

            //If player has more items than they can hold
            if (PlayerData.inventory.Count > PlayerData.ship.holdSize)
                manager.Push(new JettisonScreen());

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            //Draw backgrouond
            background.Draw(gameTime, spritebatch);

            for (int i = menuTabs.Count - 1; i >= 0; i--)
            {
                if (i != selectedScreen)
                {
                    menuTabs[i].Draw(gameTime, spritebatch, Color.Gray);
                    tabStrings[i].color = Color.Gray;
                    tabStrings[i].Draw(gameTime, spritebatch);
                }
            }
            menuTabs[selectedScreen].Draw(gameTime, spritebatch);
            tabStrings[selectedScreen].color = Color.White;
            tabStrings[selectedScreen].Draw(gameTime, spritebatch);

            menuScreens[selectedScreen].Draw(gameTime, spritebatch);

            if (canExit)
                closeX.Draw(gameTime, spritebatch);

            shipInfoBox.Draw(gameTime, spritebatch);

            base.Draw(gameTime, spritebatch);
        }
    }
}
