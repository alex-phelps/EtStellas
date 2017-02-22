using BPA_RPG.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using BPA_RPG.GameItems;
using BPA_RPG.GameItems.Weapons;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace BPA_RPG.Screens
{
    public class BattleScreen : Screen
    {
        private BattleShip player;
        private BattleShip enemy;

        private List<WeaponProjectile> projectiles;
        private List<WeaponProjectile> projDelete;

        private Vector2 backgroundScrollPos;
        private Background stars;
        private Background stars2;

        private SpriteFont nameFont;
        private SpriteFont subFont;

        private Texture2D overlay;
        private Texture2D shipHealthBar;
        private Texture2D health;
        private Texture2D weaponCooldownBar;
        private Texture2D weaponCooldown;

        private ClickableObject playerShieldButton;
        private GameObject enemyShieldButton;
        private Texture2D shieldButtonCooldown;

        private List<ClickableObject> fireButtons;
        private List<double> playerShotOldTime;
        private List<double> playerShotCount;
        
        private List<double> enemyShotOldTime;
        private List<double> enemyShotCount;

        private Viewport defaultView;
        private Viewport playerView;
        private Viewport enemyView;

        private Camera playerCamera = new Camera();
        private Camera enemyCamera = new Camera();

        private Song battleSong;

        private SoundEffectInstance laser;
        private SoundEffectInstance shield;
        private SoundEffectInstance death;

        public BattleScreen(PlayerShip player, int level) // or something
            : base("Battle")
        {
            this.player = new BattleShip(player);
            enemy = BattleShip.CreateEnemyShip(level);

            defaultView = MainGame.graphicsDevice.Viewport;

            playerView = defaultView;
            playerView.Width /= 2;
            playerView.Height /= 2;

            enemyView = playerView;
            enemyView.X = playerView.Width;
            enemyView.Y = playerView.Height;

            projectiles = new List<WeaponProjectile>();
            projDelete = new List<WeaponProjectile>();
        }

        public override void LoadContent(ContentManager content)
        {
            player.shield = new Shield(content.Load<Texture2D>("Images/Shield"), player);
            enemy.shield = new Shield(content.Load<Texture2D>("Images/Shield"), enemy);

            nameFont = content.Load<SpriteFont>("Fonts/ChoiceFont");
            subFont = content.Load<SpriteFont>("Fonts/ChoiceTabFont");

            stars = new Background(content.Load<Texture2D>("Images/StarBackground"))
            {
                position = Vector2.Zero
            };
            stars2 = new Background(content.Load<Texture2D>("Images/StarBackground2"))
            {
                position = Vector2.Zero
            };

            overlay = content.Load<Texture2D>("Images/BattleOverlay");

            shipHealthBar = content.Load<Texture2D>("Images/ShipHealthBar");
            health = content.Load<Texture2D>("Images/Health");

            weaponCooldownBar = content.Load<Texture2D>("Images/WeaponCooldownBar");
            weaponCooldown = content.Load<Texture2D>("Images/WeaponCooldown");

            playerShieldButton = new ClickableObject(content.Load<Texture2D>("Images/ShieldButton"),
                () =>
                {
                    shield.Pause();
                    shield.Play();
                    player.shield.Activate();
                })
            {
                position = new Vector2(MainGame.WindowWidth / 4, MainGame.WindowHeight / 2 + 115)
            };
            shieldButtonCooldown = content.Load<Texture2D>("Images/ShieldButtonCooldown");

            enemyShieldButton = new GameObject(playerShieldButton.texture)
            {
                position = new Vector2(MainGame.WindowWidth * 3 / 4, 115)
            };

            fireButtons = new List<ClickableObject>();
            playerShotCount = new List<double>();

            int h = 0; //index for fireButtons
            for (int i = 0; i < PlayerData.weapons.Length; i++)
            {
                if (PlayerData.weapons[i] == null)
                    continue;

                int k = i; //keep lambda from referencing i
                int j = h; //keep lambda from referencing h
                fireButtons.Add(new ClickableObject(content.Load<Texture2D>("Images/FireButton"), () =>
                {
                    if (player.cooldowns[j] == player.maxCooldowns[j])
                    {
                        PlayerFire(PlayerData.weapons[k]);
                        playerShotCount[j] = PlayerData.weapons[k].shots - 1;
                        player.cooldowns[j] = 0;
                    }
                }));
                playerShotCount.Add(0);
                h++;
            }

            for (int i = 0; i < fireButtons.Count; i++)
            {
                Vector2 pos = new Vector2(0, MainGame.WindowHeight / 2 + 100);
                switch (i)
                {
                    case 0:
                        pos.X = MainGame.WindowWidth / 4 - 170;
                        break;
                    case 1:
                        pos.X = MainGame.WindowWidth / 4 + 170;
                        break;
                    case 2:
                        pos.X = MainGame.WindowWidth / 4 - 100;
                        break;
                    case 3:
                        pos.X = MainGame.WindowWidth / 4 + 100;
                        break;
                }
                fireButtons[i].position = pos;
            }
            
            enemyShotCount = new List<double>();
            foreach (Weapon weapon in enemy.weapons)
                enemyShotCount.Add(0);

            //Sounds
            battleSong = content.Load<Song>("Sounds/Music/BattleSong");
            laser = SoundEffectManager.GetEffectInstance("Laser3");
            shield = SoundEffectManager.GetEffectInstance("Powerup1");
            death = SoundEffectManager.GetEffectInstance("Thruster2");

            //Start song
            MediaPlayer.Play(battleSong);

            base.LoadContent(content);
        }

        public override void Activated()
        {
            MediaPlayer.Play(battleSong);

            base.Activated();
        }

        public override void Deactivated()
        {
            MediaPlayer.Stop();

            base.Deactivated();
        }

        public override void Update(GameTime gameTime)
        {
            if (playerShotOldTime == null)
            {
                playerShotOldTime = new List<double>();
                foreach (double c in playerShotCount)
                    playerShotOldTime.Add(gameTime.TotalGameTime.TotalMilliseconds);
            }
            if (enemyShotOldTime == null)
            {
                enemyShotOldTime = new List<double>();
                foreach (double c in enemyShotCount)
                    enemyShotOldTime.Add(gameTime.TotalGameTime.TotalMilliseconds);
            }

            backgroundScrollPos += new Vector2(.2f, 0.01f);

            stars.Scroll(backgroundScrollPos, .7f);
            stars2.Scroll(backgroundScrollPos);

            playerShieldButton.Update(gameTime);

            foreach(ClickableObject fb in fireButtons)
                fb.Update(gameTime);

            player.Update(gameTime);

            enemy.Update(gameTime);

            for (int i = 0; i < enemy.cooldowns.Count; i++)
            {
                if (enemy.cooldowns[i] >= enemy.maxCooldowns[i])
                {
                    EnemyFire(enemy.weapons[i]);
                    enemyShotCount[i] = enemy.weapons[i].shots - 1;
                    enemy.cooldowns[i] = 0;
                }
            }

            if (projectiles.Count(p => p.speed > 0) > 0)
            {
                if (enemy.shield.Activate())
                {
                    shield.Pause();
                    shield.Play();
                }
            }

            playerCamera.Update(player.position, MainGame.WindowCenter / 2);
            enemyCamera.Update(enemy.position, MainGame.WindowCenter / 2);

            projDelete.Clear();
            foreach (WeaponProjectile proj in projectiles)
                proj.Update(gameTime);
            foreach (WeaponProjectile proj in projDelete)
                projectiles.Remove(proj);

            // Handle shot timing
            int j = 0; // count PlayerData.weapons where items can be null
            for (int i = 0; i < playerShotCount.Count; i++)
            {
                while (PlayerData.weapons[j] == null)
                    j++;

                if (playerShotCount[i] > 0)
                {
                    if (gameTime.TotalGameTime.TotalMilliseconds > playerShotOldTime[i] + 200)
                    {
                        PlayerFire(PlayerData.weapons[j]);
                        playerShotCount[i]--;
                        playerShotOldTime[i] = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                }
                else playerShotOldTime[i] = gameTime.TotalGameTime.TotalMilliseconds;

                j++;
            }

            for (int i = 0; i < enemyShotCount.Count; i++)
            {
                if (enemyShotCount[i] > 0)
                {
                    if (gameTime.TotalGameTime.TotalMilliseconds > enemyShotOldTime[i] + 200)
                    {
                        EnemyFire(enemy.weapons[i]);
                        enemyShotCount[i]--;
                        enemyShotOldTime[i] = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                }
                else enemyShotOldTime[i] = gameTime.TotalGameTime.TotalMilliseconds;
            }

            //Close if battle is over
            if (enemy.hullPoints < 0)
            {
                death.Play();
                manager.Push(new InfoBoxScreen("Battle Result", "You won!", () => manager.Pop()));
            }

            if (player.hullPoints <= 0)
            {
                death.Play();
                manager.Push(new InfoBoxScreen("Battle Result", "You Lost!", () =>
                {
                    manager.Pop();
                    manager.Pop();
                    manager.Push(new TabMenuScreen(new MenuChoiceScreen("Ship Destroyed", "GameOverDmg")));
                }));
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            MainGame.graphicsDevice.Clear(new Color(31, 27, 35));


            //Draw player viewport
            spritebatch.End(); // Aparently you can't switch viewports without ending the spritebatch.
            spritebatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null, null, playerCamera.transform);

            MainGame.graphicsDevice.Viewport = playerView;
            
            stars.position = player.position;
            stars2.position = player.position;
            stars.Draw(gameTime, spritebatch);
            stars2.Draw(gameTime, spritebatch);

            foreach (WeaponProjectile proj in projectiles)
                proj.Draw(gameTime, spritebatch);

            player.Draw(gameTime, spritebatch);

            
            //Draw enemy viewport

            spritebatch.End(); // Aparently you can't switch viewports without ending the spritebatch.
            spritebatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null, null, enemyCamera.transform);

            MainGame.graphicsDevice.Viewport = enemyView;
            
            stars.position = enemy.position;
            stars2.position = enemy.position;
            stars.Draw(gameTime, spritebatch);
            stars2.Draw(gameTime, spritebatch);

            foreach (WeaponProjectile proj in projectiles)
                proj.Draw(gameTime, spritebatch);

            enemy.Draw(gameTime, spritebatch);

            //Draw the rest of the screen

            spritebatch.End(); // Aparently you can't switch viewports without ending the spritebatch.
            spritebatch.Begin();

            MainGame.graphicsDevice.Viewport = defaultView;


            //Draw Player health
            spritebatch.Draw(health, new Vector2(MainGame.WindowWidth / 4, MainGame.WindowHeight / 2 + 60),
                new Rectangle(0, 0, (int)(health.Width * (player.hullPoints / (float)player.maxHullPoints)), health.Height), Color.White, 0,
                new Vector2(health.Width / 2, health.Height / 2), 1, SpriteEffects.None, 1);
            spritebatch.Draw(shipHealthBar, new Vector2(MainGame.WindowWidth / 4, MainGame.WindowHeight / 2 + 60),
                new Rectangle(0, 0, shipHealthBar.Width, shipHealthBar.Height), Color.White, 0,
                new Vector2(shipHealthBar.Width / 2, shipHealthBar.Height / 2), 1, SpriteEffects.None, 1);

            //Draw enemy health
            spritebatch.Draw(health, new Vector2(MainGame.WindowWidth * 3 / 4, 60),
                new Rectangle(0, 0, (int)(health.Width * (enemy.hullPoints / (float)enemy.maxHullPoints)), health.Height), Color.White, 0,
                new Vector2(health.Width / 2, health.Height / 2), 1, SpriteEffects.None, 1);
            spritebatch.Draw(shipHealthBar, new Vector2(MainGame.WindowWidth * 3 / 4, 60),
                new Rectangle(0, 0, shipHealthBar.Width, shipHealthBar.Height), Color.White, 0,
                new Vector2(shipHealthBar.Width / 2, shipHealthBar.Height / 2), 1, SpriteEffects.None, 1);

            //Draw ship names
            spritebatch.DrawString(nameFont, player.name, new Vector2(MainGame.WindowWidth / 4 - shipHealthBar.Width / 2, MainGame.WindowHeight / 2 + 30), Color.White);
            spritebatch.DrawString(nameFont, enemy.name, new Vector2(MainGame.WindowWidth * 3 / 4 - shipHealthBar.Width / 2, 30), Color.White);

            //Draw player shield button
            playerShieldButton.Draw(gameTime, spritebatch);
            double time;
            double max;
            Color color;
            if (player.shield.visible)
            {
                time = player.shield.shieldTimeTime;
                max = player.shield.shieldTimeMax;
                color = Color.Gray;
            }
            else
            {
                time = player.shield.cooldownTime;
                max = player.shield.cooldownMax;
                color = Color.White;
            }

            spritebatch.Draw(shieldButtonCooldown, playerShieldButton.position + new Vector2(0, shieldButtonCooldown.Height - shieldButtonCooldown.Height * (float)(time / max)),
                new Rectangle(0, shieldButtonCooldown.Height - (int)(shieldButtonCooldown.Height * (time / max)), shieldButtonCooldown.Width, shieldButtonCooldown.Height), color, 0, 
                new Vector2(shieldButtonCooldown.Width / 2, shieldButtonCooldown.Height / 2), 1, SpriteEffects.None, 1);
            spritebatch.DrawString(subFont, "Shield", playerShieldButton.position - subFont.MeasureString("Shield") / 2, color);

            //Draw player shield button
            enemyShieldButton.Draw(gameTime, spritebatch);
            if (enemy.shield.visible)
            {
                time = enemy.shield.shieldTimeTime;
                max = enemy.shield.shieldTimeMax;
                color = Color.Gray;
            }
            else
            {
                time = enemy.shield.cooldownTime;
                max = enemy.shield.cooldownMax;
                color = Color.White;
            }

            spritebatch.Draw(shieldButtonCooldown, enemyShieldButton.position + new Vector2(0, shieldButtonCooldown.Height - shieldButtonCooldown.Height * (float)(time / max)),
                new Rectangle(0, shieldButtonCooldown.Height - (int)(shieldButtonCooldown.Height * (time / max)), shieldButtonCooldown.Width, shieldButtonCooldown.Height), color, 0,
                new Vector2(shieldButtonCooldown.Width / 2, shieldButtonCooldown.Height / 2), 1, SpriteEffects.None, 1);
            spritebatch.DrawString(subFont, "Shield", enemyShieldButton.position - subFont.MeasureString("Shield") / 2, color);

            //Draw player weapons systems
            int j = 0;
            for (int i = 0; i < fireButtons.Count; i++)
            {
                color = player.cooldowns[i] >= player.maxCooldowns[i] ? Color.White : Color.Gray;
                fireButtons[i].Draw(gameTime, spritebatch, color);

                spritebatch.DrawString(subFont, "Fire!", fireButtons[i].position - subFont.MeasureString("Fire!") / 2, Color.Black);
                
                spritebatch.Draw(weaponCooldownBar, fireButtons[i].position + new Vector2(-weaponCooldownBar.Width / 2, 22),
                    Color.White);
                spritebatch.Draw(weaponCooldown, fireButtons[i].position + new Vector2(-weaponCooldown.Width / 2, 22 + weaponCooldown.Height - weaponCooldown.Height * ((float)player.cooldowns[i] / player.maxCooldowns[i])),
                    new Rectangle(0, weaponCooldown.Height - (int)(weaponCooldown.Height * ((float)player.cooldowns[i] / player.maxCooldowns[i])), weaponCooldown.Width, weaponCooldown.Height),
                    Color.White);


                while (j < PlayerData.weapons.Length && PlayerData.weapons[j] == null)
                    j++;

                string name = PlayerData.weapons[j].name.Replace(' ', '\n');
                spritebatch.DrawString(subFont, name,
                    (fireButtons[i].position + new Vector2(0, 165)) - subFont.MeasureString(name) / 2, Color.White);
                
                j++;
            }
            
            //Draw enemy weapon systems
            for (int i = 0; i < enemy.weapons.Length; i++)
            {
                Vector2 pos = new Vector2(0, 100);
                switch (i)
                {
                    case 0:
                        pos.X = MainGame.WindowWidth * 3 / 4 - 170;
                        break;
                    case 1:
                        pos.X = MainGame.WindowWidth * 3 / 4 + 170;
                        break;
                    case 2:
                        pos.X = MainGame.WindowWidth * 3 / 4 - 100;
                        break;
                    case 3:
                        pos.X = MainGame.WindowWidth * 3 / 4 + 100;
                        break;
                }

                string name = enemy.weapons[i].name.Replace(' ', '\n');
                spritebatch.DrawString(subFont, name, pos - subFont.MeasureString(name) / 2, Color.White);

                spritebatch.Draw(weaponCooldownBar, pos + new Vector2(-weaponCooldownBar.Width / 2, 25),
                    Color.White);
                spritebatch.Draw(weaponCooldown, pos + new Vector2(-weaponCooldown.Width / 2, 25 + weaponCooldown.Height - weaponCooldown.Height * ((float)enemy.cooldowns[i] / enemy.maxCooldowns[i])),
                    new Rectangle(0, weaponCooldown.Height - (int)(weaponCooldown.Height * ((float)enemy.cooldowns[i] / enemy.maxCooldowns[i])), weaponCooldown.Width, weaponCooldown.Height),
                    Color.White);
            }

            //Draw overlay
            spritebatch.Draw(overlay, Vector2.Zero, Color.White);
            
            base.Draw(gameTime, spritebatch);
        }

        private void PlayerFire(Weapon weapon)
        {
            if (weapon is LaserWeapon)
            {
                laser.Pause();
                laser.Play();
            }

            projectiles.Add(new WeaponProjectile(weapon, player.position, enemy, weapon.speed, projDelete));
        }

        private void EnemyFire(Weapon weapon)
        {
            if (weapon is LaserWeapon)
            {
                laser.Pause();
                laser.Play();
            }

            projectiles.Add(new WeaponProjectile(weapon, enemy.position, player, -weapon.speed, projDelete));
        }
    }
}
