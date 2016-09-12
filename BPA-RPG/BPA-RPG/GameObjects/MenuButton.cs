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

        public MenuButton(ContentManager content, EventHandler buttonEvent) 
            : base(content)
        {
            this.buttonEvent = buttonEvent;
        }

        public override void Update(GameTime gameTime)
        {
            MouseState newMouseState = Mouse.GetState();





            oldMouseState = newMouseState;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);
        }
    }
}
