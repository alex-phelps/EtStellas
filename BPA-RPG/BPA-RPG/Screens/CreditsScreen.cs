using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using BPA_RPG.GameObjects;
using Microsoft.Xna.Framework.Input;

namespace BPA_RPG.Screens
{
    /// <summary>
    /// Screen to display a credits sequence
    /// </summary>
    public class CreditsScreen : Screen
    {
        private Texture2D credits;

        private Background background;

        private double time;

        public CreditsScreen(Texture2D credits)
            : base("Credits")
        {
            this.credits = credits;
        }

        public override void LoadContent(ContentManager content)
        {
            background = new Background(content.Load<Texture2D>("Images/StarBackground"));

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            if (time == 0)
                time = gameTime.TotalGameTime.TotalMilliseconds;

            if (gameTime.TotalGameTime.TotalMilliseconds >= time + 4000 ||
                InputManager.newKeyState.IsKeyDown(Keys.Enter))
                manager.Pop();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            background.Draw(gameTime, spritebatch);

            spritebatch.Draw(credits, Vector2.Zero, Color.White);

            base.Draw(gameTime, spritebatch);
        }
    }
}
