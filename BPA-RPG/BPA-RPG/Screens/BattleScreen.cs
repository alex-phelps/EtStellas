using BPA_RPG.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace BPA_RPG.Screens
{
    public class BattleScreen : Screen
    {
        private PlayerShip player;
        private EnemyShip enemy;

        private Vector2 backgroundScrollPos;
        private Background stars;
        private Background stars2;

        private SpriteFont nameFont;
        private Texture2D overlay;
        private Texture2D shipHealthBar;
        private Texture2D healthGreen;

        private Viewport defaultView, playerView, enemyView;

        public BattleScreen(PlayerShip player, EnemyShip enemy)
            : base("Battle")
        {
            this.player = player;
            this.enemy = enemy;

            defaultView = MainGame.graphicsDevice.Viewport;

            playerView = defaultView;
            playerView.Width /= 2;
            playerView.Height /= 2;

            enemyView = playerView;
            enemyView.X = playerView.Width;
            enemyView.Y = playerView.Height;
        }

        public override void LoadContent(ContentManager content)
        {
            stars = new Background(content.Load<Texture2D>("Images/StarBackground"));
            stars2 = new Background(content.Load<Texture2D>("Images/StarBackground2"));

            overlay = content.Load<Texture2D>("Images/BattleOverlay");

            shipHealthBar = content.Load<Texture2D>("Images/ShipHealthBar");
            healthGreen = content.Load<Texture2D>("Images/HealthGreen");

            nameFont = content.Load<SpriteFont>("Fonts/ChoiceFont");

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            backgroundScrollPos += new Vector2(.2f, 0.01f);

            stars.Scroll(backgroundScrollPos, .7f);
            stars2.Scroll(backgroundScrollPos);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            MainGame.graphicsDevice.Clear(new Color(31, 27, 35));


            //Draw player viewport
            spritebatch.End(); // Aparently you cant switch viewports without ending the spritebatch...
            spritebatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap);

            MainGame.graphicsDevice.Viewport = playerView;

            //Draw background then change its location
            stars.Draw(gameTime, spritebatch);
            stars2.Draw(gameTime, spritebatch);
            stars.position -= MainGame.WindowCenter / 2;
            stars2.position -= MainGame.WindowCenter / 2;

            spritebatch.Draw(player.texture, MainGame.WindowCenter / 2, new Rectangle(0, 0, player.Width / 2, player.Height),
                Color.White, MathHelper.PiOver2, new Vector2(player.Width / 4, player.Height / 2), 4,
                SpriteEffects.None, 1);
            
            //Draw enemy viewport

            spritebatch.End(); // Aparently you cant switch viewports without ending the spritebatch...
            spritebatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap);

            MainGame.graphicsDevice.Viewport = enemyView;

            //Draw background then change its location back
            stars.Draw(gameTime, spritebatch);
            stars2.Draw(gameTime, spritebatch);
            stars.position += MainGame.WindowCenter / 2;
            stars2.position += MainGame.WindowCenter / 2;

            spritebatch.Draw(enemy.texture, MainGame.WindowCenter / 2, new Rectangle(0, 0, enemy.Width / 2, enemy.Height),
                Color.White, -MathHelper.PiOver2, new Vector2(enemy.Width / 4, enemy.Height / 2), 4,
                SpriteEffects.None, 1);

            //Draw the rest of the screen

            spritebatch.End(); // Aparently you cant switch viewports without ending the spritebatch...
            spritebatch.Begin();

            MainGame.graphicsDevice.Viewport = defaultView;


            //Draw Player health
            spritebatch.Draw(healthGreen, new Vector2(MainGame.WindowWidth / 4, MainGame.WindowHeight / 2 + 60),
                new Rectangle(0, 0, healthGreen.Width, healthGreen.Height), Color.White, 0,
                new Vector2(healthGreen.Width / 2, healthGreen.Height / 2), 1, SpriteEffects.None, 1);
            spritebatch.Draw(shipHealthBar, new Vector2(MainGame.WindowWidth / 4, MainGame.WindowHeight / 2 + 60),
                new Rectangle(0, 0, shipHealthBar.Width, shipHealthBar.Height), Color.White, 0,
                new Vector2(shipHealthBar.Width / 2, shipHealthBar.Height / 2), 1, SpriteEffects.None, 1);

            //Draw enemy health
            spritebatch.Draw(healthGreen, new Vector2(MainGame.WindowWidth * 3 / 4, 60),
                new Rectangle(0, 0, healthGreen.Width, healthGreen.Height), Color.White, 0,
                new Vector2(healthGreen.Width / 2, healthGreen.Height / 2), 1, SpriteEffects.None, 1);
            spritebatch.Draw(shipHealthBar, new Vector2(MainGame.WindowWidth * 3 / 4, 60),
                new Rectangle(0, 0, shipHealthBar.Width, shipHealthBar.Height), Color.White, 0,
                new Vector2(shipHealthBar.Width / 2, shipHealthBar.Height / 2), 1, SpriteEffects.None, 1);

            //Draw ship names
            spritebatch.DrawString(nameFont, player.name, new Vector2(MainGame.WindowWidth / 4 - shipHealthBar.Width / 2, MainGame.WindowHeight / 2 + 30), Color.White);
            spritebatch.DrawString(nameFont, enemy.name, new Vector2(MainGame.WindowWidth * 3 / 4 - shipHealthBar.Width / 2, 30), Color.White);

            spritebatch.Draw(overlay, Vector2.Zero, Color.White);


            base.Draw(gameTime, spritebatch);
        }
    }
}
