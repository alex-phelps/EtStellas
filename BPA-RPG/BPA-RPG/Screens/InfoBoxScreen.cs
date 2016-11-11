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
        private EventHandler onExit;
        private Texture2D infoBox;

        private ClickableObject button;

        public InfoBoxScreen(string title, string text, EventHandler onExit)
            : base(title)
        {
            this.text = text;
            this.onExit = onExit;
        }

        public override void LoadContent(ContentManager content)
        {
            button = new ClickableObject(content.Load<Texture2D>("Images/DebugTexture"),
                (o, s) => { onExit?.Invoke(this, new EventArgs()); manager.Pop(); });
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
            button.Draw(gameTime, spritebatch);

            base.Draw(gameTime, spritebatch);
        }
    }
}
