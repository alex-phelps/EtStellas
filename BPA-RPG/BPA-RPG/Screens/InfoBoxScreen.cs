using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BPA_RPG.GameObjects;
using Microsoft.Xna.Framework.Content;

namespace BPA_RPG.Screens
{
    public class InfoBoxScreen : Screen
    {
        private string text;
        private Action onExit;
        private Texture2D infoBox;
        private SpriteFont font;

        private ClickableObject button;

        public InfoBoxScreen(string title, string text, Action onExit)
            : base(title)
        {
            this.text = text;
            this.onExit = onExit;

            translucent = true;
        }

        public override void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("Fonts/ChoiceFont");

            button = new ClickableObject(content.Load<Texture2D>("Images/DebugTexture"), () =>
            {
                manager.Pop();
                onExit?.Invoke();
            })
            {
                position = MainGame.WindowCenter + new Vector2(0, 50)
            };
            infoBox = content.Load<Texture2D>("Images/InfoBox");

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            button.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            spritebatch.Draw(infoBox, MainGame.WindowCenter - new Vector2(infoBox.Width / 2, infoBox.Height / 2), Color.White);
            spritebatch.DrawString(font, text, MainGame.WindowCenter - new Vector2(0, 50) - font.MeasureString(text) / 2, Color.White);
            button.Draw(gameTime, spritebatch);

            base.Draw(gameTime, spritebatch);
        }
    }
}
