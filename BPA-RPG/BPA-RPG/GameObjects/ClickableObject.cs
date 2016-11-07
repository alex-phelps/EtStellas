using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BPA_RPG.GameObjects
{
    public class ClickableObject : GameObject
    {
        private EventHandler onClick;
        private EventHandler onHover;
        private EventHandler onUnHover;

        public ClickableObject(Texture2D texture)
            : this(texture, (o, s) => { }, (o, s) => { }, (o, s) => { })
        {
        }

        public ClickableObject(Texture2D texture, EventHandler onClick = null, 
            EventHandler onHover = null, EventHandler onUnHover = null)
            : base(texture)
        {
            if (onClick == null)
                onClick = (o, s) => { };
            if (onHover == null)
                onHover = (o, s) => { };
            if (onUnHover == null)
                onUnHover = (o, s) => { };

            this.onClick = onClick;
            this.onHover = onHover;
            this.onUnHover = onUnHover;
        }

        public override void Update(GameTime gameTime)
        {
            if (boundingRectangle.Contains(InputManager.newMouseState.Position))
            {
                onHover.Invoke(this, new EventArgs());

                if (InputManager.newMouseState.LeftButton == ButtonState.Pressed &&
                    InputManager.oldMouseState.LeftButton == ButtonState.Released)
                    onClick.Invoke(this, new EventArgs());
            }
            else onUnHover.Invoke(this, new EventArgs());

            base.Update(gameTime);
        }
    }
}
