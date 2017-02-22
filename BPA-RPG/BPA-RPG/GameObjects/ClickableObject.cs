using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EtStellas.GameObjects
{
    public class ClickableObject : GameObject
    {
        private Action onClick;
        private Action onHover;
        private Action onUnHover;
        private Action onHold;

        private bool holding;

        public ClickableObject(Texture2D texture, Action onClick = null,
            Action onHover = null, Action onUnHover = null, Action onHold = null)
            : base(texture)
        {

            this.onClick = onClick;
            this.onHover = onHover;
            this.onUnHover = onUnHover;
            this.onHold = onHold;
        }

        public override void Update(GameTime gameTime)
        {
            if (boundingRectangle.Contains(InputManager.newMouseState.Position))
            {
                onHover?.Invoke();

                if (InputManager.newMouseState.LeftButton == ButtonState.Pressed &&
                    InputManager.oldMouseState.LeftButton == ButtonState.Released)
                {
                    onClick?.Invoke();
                    holding = true;
                }
            }
            else onUnHover?.Invoke();

            if (holding)
            {
                if (InputManager.newMouseState.LeftButton == ButtonState.Pressed)
                {
                    onHold?.Invoke();
                }
                else holding = false;
            }

            base.Update(gameTime);
        }
    }
}
