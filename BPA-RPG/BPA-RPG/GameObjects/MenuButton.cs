using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BPA_RPG.GameObjects
{
    class MenuButton : GameObject
    {
        private EventHandler buttonEvent;
        private MouseState oldMouseState;
        private string text;

        private SpriteFont menuFont;

        public MenuButton(ContentManager content, EventHandler buttonEvent, Vector2 position, string text) 
            : base(content)
        {
            this.buttonEvent = buttonEvent;
            this.position = position;
            this.text = text;

            menuFont = content.Load<SpriteFont>("Fonts/MenuButtonFont");
            texture = content.Load<Texture2D>("Images/MenuButton");
        }

        public override void Update(GameTime gameTime)
        {
            MouseState newMouseState = Mouse.GetState();

            if (boundingRectangle.Contains(newMouseState.Position))
            {
                if (newMouseState.LeftButton == ButtonState.Pressed)
                    buttonEvent.Invoke(this, new EventArgs());
            }

            oldMouseState = newMouseState;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            base.Draw(gameTime, spritebatch);
            spritebatch.DrawString(menuFont, text, position - 
                new Vector2(menuFont.MeasureString(text).X / 2, menuFont.MeasureString(text).Y / 2), Color.Black);
        }
    }
}
