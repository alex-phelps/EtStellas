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
            enemyView.X = (int)MainGame.WindowCenter.X;
            enemyView.Y = (int)MainGame.WindowCenter.Y;
        }

        public override void LoadContent(ContentManager content)
        {


            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {


            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            //Draw player viewport
            MainGame.graphicsDevice.Viewport = playerView;

            //Draw enemy viewport
            MainGame.graphicsDevice.Viewport = enemyView;

            //Draw the rest of the screen
            MainGame.graphicsDevice.Viewport = defaultView;

            base.Draw(gameTime, spritebatch);
        }
    }
}
