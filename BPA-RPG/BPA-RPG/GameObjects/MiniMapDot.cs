using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA_RPG.GameObjects
{
    public class MiniMapDot : GameObject
    {
        public GameObject repOf { get; private set; }
        public Color color;

        public MiniMapDot(ContentManager content, GameObject repOf, Color color)
            : base(content.Load<Texture2D>("Images/MiniMapDot"))
        {
            this.repOf = repOf;
            this.color = color;
        }

        public override void Update(GameTime gameTime)
        {
            Update(gameTime, 1);
        }

        public void Update(GameTime gameTime, int scale)
        {

            position = repOf.position / scale;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch, Color color)
        {
            base.Draw(gameTime, spritebatch, this.color);
        }
    }
}
