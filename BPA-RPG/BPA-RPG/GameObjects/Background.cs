using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BPA_RPG.GameObjects
{
    public class Background : GameObject
    {
        private int scrollX;
        private int scrollY;

        /// <summary>
        /// Creates a background from a texture
        /// </summary>
        public Background(Texture2D texture) 
            : base(texture)
        {
            position = MainGame.WindowCenter;

            scrollX = 0;
            scrollY = 0;
        }

        /// <summary>
        /// Creates a window size background of specified color
        /// </summary>
        public Background(Color color)
            : this(new Texture2D(MainGame.graphicsDevice, MainGame.WindowWidth, MainGame.WindowHeight))
        {
            Color[] colorData = new Color[texture.Height * texture.Width];
            for (int i = 0; i < colorData.Length; i++)
            {
                colorData[i] = color;
            }

            texture.SetData(colorData);
            this.colorData = colorData;
        }

        public void Scroll(Vector2 scroll, float speed = 1f)
        {
            scrollX = (int)(scroll.X * -speed);
            scrollY = (int)(scroll.Y * -speed);

            scrollX %= texture.Width;
            scrollY %= texture.Height;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch, Color color)
        {
            if (visible)
                spritebatch.Draw(texture, position, new Rectangle(-scrollX, -scrollY, texture.Width, texture.Height), color, rotation,
                    new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 1);
        }
    }
}
