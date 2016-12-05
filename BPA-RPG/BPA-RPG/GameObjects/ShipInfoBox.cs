using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BPA_RPG.GameObjects
{
    public class ShipInfoBox : GameObject
    {
        private bool renderInfo;

        private List<string> info;
        private Texture2D infoBox;
        private SpriteFont font;

        public ShipInfoBox(ContentManager content)
            : base(content.Load<Texture2D>("Images/MenuTab"))
        {
            info = new List<string>();
            infoBox = content.Load<Texture2D>("Images/ShipInfoBox");
            font = content.Load<SpriteFont>("Fonts/ChoiceTabFont");

            position = new Vector2(MainGame.WindowWidth / 2, MainGame.WindowHeight - Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            info = new List<string>();
            info.Add("Money");
            foreach (Currency c in Enum.GetValues(typeof(Currency)))
                info.Add(PlayerData.GetMoney(c) + " " + c);


            if (new Rectangle(boundingRectangle.Location, new Point(boundingRectangle.Width, Height + (renderInfo ? info.Count * infoBox.Height : 0)))
                .Contains(InputManager.newMouseState.Position))
            {
                renderInfo = true;

                position.Y = MainGame.WindowHeight - Height / 2 - info.Count * infoBox.Height;
            }
            else
            {
                renderInfo = false;

                position.Y = MainGame.WindowHeight - Height / 2;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch, Color color)
        {
            base.Draw(gameTime, spritebatch, color);

            spritebatch.DrawString(font, "Ship Info", position - font.MeasureString("Ship Info") / 2, Color.White);

            if (renderInfo)
            {
                for (int i = 0; i < info.Count; i++)
                {
                    Vector2 pos = position + new Vector2(0, Height / 2 + infoBox.Height / 2 + i * infoBox.Height);
                    spritebatch.Draw(infoBox, pos, new Rectangle(0, 0, infoBox.Width, infoBox.Height), Color.White, 0, new Vector2(infoBox.Width / 2, infoBox.Height / 2), 1, SpriteEffects.None, 1);
                    spritebatch.DrawString(font, info[i], pos - font.MeasureString(info[i]) / 2, Color.White);
                }
            }
        }
    }
}
