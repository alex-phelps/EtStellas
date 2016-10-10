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
    class TabManuScreen : Screen
    {
        private readonly List<Screen> menuScreens;
        private List<GameObject> menuTabs;
        private List<DrawableString> tabStrings;
        private MouseState oldMouseState;
        private int selectedScreen = 0;

        public TabManuScreen(params Screen[] menuScreens)
            : base("Tab Menu")
        {
            this.menuScreens = new List<Screen>(menuScreens);

            translucent = true;
            menuTabs = new List<GameObject>();
            tabStrings = new List<DrawableString>();
        }

        public override void LoadContent(ContentManager content)
        {
            Texture2D menuTab = content.Load<Texture2D>("Images/MenuTab");
            SpriteFont tabFont = content.Load<SpriteFont>("Fonts/ChoiceTabFont");
            
            for (int i = 0; i < menuScreens.Count; i++)
            {
                menuScreens[i].manager = manager;
                menuScreens[i].LoadContent(content);
                menuTabs.Add(new GameObject(menuTab)
                {
                    position = MainGame.WindowCenter - new Vector2(228 - (i * 165), 242)
                });
                tabStrings.Add(new DrawableString(tabFont, menuScreens[i].title, MainGame.WindowCenter - new Vector2(228 - (i * 165), 242), Color.White));
            }

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState newMouseState = Mouse.GetState();

            menuScreens[selectedScreen].Update(gameTime);

            for (int i = 0; i < menuTabs.Count; i++)
            {
                //Check if tab is clicked
                if (menuTabs[i].boundingRectangle.Contains(newMouseState.Position) &&
                    newMouseState.LeftButton == ButtonState.Pressed && 
                    oldMouseState.LeftButton == ButtonState.Pressed)
                    selectedScreen = i;
            }

            oldMouseState = newMouseState;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            menuScreens[selectedScreen].Draw(gameTime, spritebatch);

            for (int i = 0; i < menuTabs.Count; i++)
            {
                if (i != selectedScreen)
                {
                    menuTabs[i].Draw(gameTime, spritebatch, Color.Gray);
                    tabStrings[i].color = Color.Gray;
                    tabStrings[i].DrawCenter(gameTime, spritebatch);
                }
            }
            menuTabs[selectedScreen].Draw(gameTime, spritebatch);
            tabStrings[selectedScreen].color = Color.White;
            tabStrings[selectedScreen].DrawCenter(gameTime, spritebatch);

            base.Draw(gameTime, spritebatch);
        }
    }
}
