using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BPA_RPG.GameObjects
{
    public class ClickableObject : GameObject
    {
        private Action onClick;
        private Action onHover;
        private Action onUnHover;

        public ClickableObject(Texture2D texture, Action onClick = null,
            Action onHover = null, Action onUnHover = null)
            : base(texture)
        {

            this.onClick = onClick;
            this.onHover = onHover;
            this.onUnHover = onUnHover;
        }

        public override void Update(GameTime gameTime)
        {
            if (boundingRectangle.Contains(InputManager.newMouseState.Position))
            {
                onHover?.Invoke();

                if (InputManager.newMouseState.LeftButton == ButtonState.Pressed &&
                    InputManager.oldMouseState.LeftButton == ButtonState.Released)
                    onClick?.Invoke();
            }
            else onUnHover?.Invoke();

            base.Update(gameTime);
        }
    }
}
