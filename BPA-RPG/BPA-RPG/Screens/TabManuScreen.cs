using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using BPA_RPG.GameObjects;

namespace BPA_RPG.Screens
{
    class TabManuScreen : Screen
    {
        private readonly List<Screen> menuScreens;
        private List<GameObject> menuTabs;
        private int selectedScreen = 0;

        public TabManuScreen(params Screen[] menuScreens)
            : base()
        {
            this.menuScreens = new List<Screen>(menuScreens);

            translucent = true;
            menuTabs = new List<GameObject>();
        }

        public override void LoadContent(ContentManager content)
        {
            Texture2D menuTab = content.Load<Texture2D>("Images/MenuTab");
            
            for (int i = 0; i < menuScreens.Count; i++)
            {
                menuScreens[i].LoadContent(content);
                menuTabs.Add(new GameObject(menuTab)
                {
                    position = MainGame.WindowCenter - new Vector2(323 - i * 172, 242)
                });
            }

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            menuScreens[selectedScreen].Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            menuScreens[selectedScreen].Draw(gameTime, spritebatch);

            for (int i = 0; i < menuTabs.Count; i++)
            {
                if (i != selectedScreen)
                    menuTabs[i].Draw(gameTime, spritebatch);
            }
            menuTabs[selectedScreen].Draw(gameTime, spritebatch);

            base.Draw(gameTime, spritebatch);
        }
    }
}
