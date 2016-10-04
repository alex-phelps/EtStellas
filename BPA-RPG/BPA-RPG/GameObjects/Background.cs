using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BPA_RPG.GameObjects
{
    class Background : GameObject
    {
        private int scrollX;
        private int scrollY;

        public Background(Texture2D texture) 
            : base(texture)
        {
            position = MainGame.WindowCenter;

            scrollX = 0;
            scrollY = 0;
        }

        public void Scroll(Vector2 scroll, float speed = 1f)
        {
            scrollX = (int)(scroll.X * -speed);
            scrollY = (int)(scroll.Y * -speed);

            scrollX %= texture.Width;
            scrollY %= texture.Height;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, position, new Rectangle(-scrollX, -scrollY, texture.Width, texture.Height), Color.White, rotation, 
                new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 1);
        }
    }
}
