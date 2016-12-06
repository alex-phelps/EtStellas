using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA_RPG
{
    public class DrawableString
    {
        public string text;
        public Vector2 position;
        public SpriteFont font;
        public Color color;

        public Rectangle boundingRectangle
        {
            get
            {
                Vector2 size = font.MeasureString(text);

                return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            }
        }

        public DrawableString(SpriteFont font, string text, Vector2 position, Color color)
        {

            // ADD CLIKCING 

            this.font = font;
            this.text = text;
            this.position = position;
            this.color = color;
        }

        public DrawableString(SpriteFont font, string text)
            : this(font, text, Vector2.Zero, Color.Black)
        {
        }

        public void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            spritebatch.DrawString(font, text, position, color);
        }
    }
}
