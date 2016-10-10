using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using BPA_RPG.GameObjects;

namespace BPA_RPG.Screens
{
    public class MainMenuScreen : Screen
    {
        private List<MenuButton> buttons;

        public MainMenuScreen() 
            : base("Main Menu")
        {

        }

        public override void LoadContent(ContentManager content)
        {
            buttons = new List<MenuButton>();
            buttons.Add(new MenuButton(content, PlayButtonEvent, new Vector2(MainGame.WindowWidth / 2, 200), "Play"));

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (MenuButton button in buttons)
            {
                button.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            foreach (MenuButton button in buttons)
            {
                button.Draw(gameTime, spritebatch);
            }

            base.Draw(gameTime, spritebatch);
        }

        private void PlayButtonEvent(object sender, EventArgs e)
        {
            manager.Push(new GameScreen());
        }
    }
}
