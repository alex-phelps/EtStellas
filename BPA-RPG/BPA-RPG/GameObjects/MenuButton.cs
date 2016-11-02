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
    public class MenuButton : GameObject
    {
        private EventHandler buttonEvent;
        private string text;

        private SpriteFont menuFont;

        public MenuButton(ContentManager content, EventHandler buttonEvent, Vector2 position, string text) 
            : base(content.Load<Texture2D>("Images/MenuButton"))
        {
            this.buttonEvent = buttonEvent;
            this.position = position;
            this.text = text;

            menuFont = content.Load<SpriteFont>("Fonts/MenuButtonFont");
        }

        public override void Update(GameTime gameTime)
        {
            if (boundingRectangle.Contains(MainGame.input.newMouseState.Position))
            {
                if (MainGame.input.newMouseState.LeftButton == ButtonState.Pressed)
                    buttonEvent.Invoke(this, new EventArgs());
            }
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch, Color color)
        {
            base.Draw(gameTime, spritebatch, color);

            if (visible)
                spritebatch.DrawString(menuFont, text, position -
                    new Vector2(menuFont.MeasureString(text).X / 2, menuFont.MeasureString(text).Y / 2), Color.Black);
        }
    }
}
