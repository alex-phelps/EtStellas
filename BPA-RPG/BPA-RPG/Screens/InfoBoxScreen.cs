using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BPA_RPG.GameObjects;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace BPA_RPG.Screens
{
    public class InfoBoxScreen : Screen
    {
        private string text;
        private Action onExit;
        private Texture2D infoBox;
        private SpriteFont infoFont;
        private SpriteFont okayFont;

        private ClickableObject button;

        private Background background;

        private SoundEffectInstance select;

        public InfoBoxScreen(string title, string text, Action onExit = null)
            : base(title)
        {
            this.text = text;
            this.onExit = onExit;

            translucent = true;
        }

        public override void LoadContent(ContentManager content)
        {
            infoFont = content.Load<SpriteFont>("Fonts/InfoFont");
            okayFont = content.Load<SpriteFont>("Fonts/ChoiceFont");

            button = new ClickableObject(content.Load<Texture2D>("Images/InfoBoxButton"), () =>
            {
                select.Play();
                manager.Pop();
                onExit?.Invoke();
            })
            {
                position = MainGame.WindowCenter + new Vector2(0, 50)
            };
            infoBox = content.Load<Texture2D>("Images/InfoBox");

            background = new Background(Color.Black * .6f);

            select = SoundEffectManager.GetEffectInstance("Select1");

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            button.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            //Draw background
            background.Draw(gameTime, spritebatch);

            spritebatch.Draw(infoBox, MainGame.WindowCenter - new Vector2(infoBox.Width / 2, infoBox.Height / 2), Color.White);
            spritebatch.DrawString(infoFont, text, MainGame.WindowCenter - new Vector2(0, 45) - infoFont.MeasureString(text) / 2, Color.White);
            button.Draw(gameTime, spritebatch);
            spritebatch.DrawString(okayFont, "Okay",button.position - okayFont.MeasureString("Okay") / 2, Color.White);

            base.Draw(gameTime, spritebatch);
        }
    }
}
