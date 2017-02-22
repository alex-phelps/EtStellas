using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace EtStellas
{
    public class DrawableString
    {
        public string text;
        public Vector2 position;
        public SpriteFont font;
        public Color color;

        private Action onClick;
        private Action onHover;
        private Action onUnHover;
        private Action onHold;

        private bool holding;

        public Rectangle boundingRectangle
        {
            get
            {
                Vector2 size = font.MeasureString(text);

                return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            }
        }

        public DrawableString(SpriteFont font, string text, Vector2 position, Color color,
            Action onClick = null, Action onHover = null, Action onUnHover = null, Action onHold = null)
        {
            this.font = font;
            this.text = text;
            this.position = position;
            this.color = color;

            this.onClick = onClick;
            this.onHover = onHover;
            this.onUnHover = onUnHover;
            this.onHold = onHold;
        }

        public DrawableString(SpriteFont font, string text)
            : this(font, text, Vector2.Zero, Color.Black)
        {
        }

        public void Update(GameTime gameTime)
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
        }

        public void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            spritebatch.DrawString(font, text, position, color);
        }
    }
}
