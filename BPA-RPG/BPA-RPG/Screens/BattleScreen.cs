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

        private Background stars;

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

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {


            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            spritebatch.End();
            spritebatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            
            //Draw player viewport
            MainGame.graphicsDevice.Viewport = playerView;

            stars.Draw(gameTime, spritebatch);
            spritebatch.Draw(player.texture, MainGame.WindowCenter, new Rectangle(0, 0, player.Width / 2, player.Height),
                Color.White, MathHelper.PiOver2, new Vector2(player.Width / 4, player.Height / 2), 4,
                SpriteEffects.None, 1);


            //Draw enemy viewport
            MainGame.graphicsDevice.Viewport = enemyView;



            //Draw the rest of the screen
            MainGame.graphicsDevice.Viewport = defaultView;

            spritebatch.End();
            spritebatch.Begin();

            base.Draw(gameTime, spritebatch);
        }
    }
}
