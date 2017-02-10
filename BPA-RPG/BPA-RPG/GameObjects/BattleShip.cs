using BPA_RPG.GameItems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using BPA_RPG.GameItems.Weapons;

namespace BPA_RPG.GameObjects
{
    /// <summary>
    /// Represents a ship in a battle screen
    /// </summary>
    public class BattleShip : GameObject
    {
        public Shield shield;

        public readonly Weapon[] weapons;
        public readonly string name;
        public int hullPoints;
        public int maxHullPoints;

        public List<double> cooldowns;
        public List<int> maxCooldowns;
        public List<double> cooldownTimes;

        public int fireCount;
        private double fireTime;
        
        /// <summary>
        /// Creates player BattleShip
        /// </summary>
        /// <param name="player">Player this ship belongs to</param>
        public BattleShip(PlayerShip player)
            : base(player.texture)
        {
            name = player.name;
            maxHullPoints = player.maxHullPoints;
            hullPoints = maxHullPoints;

            weapons = PlayerData.weapons;

            cooldowns = new List<double>();
            maxCooldowns = new List<int>();

            foreach (Weapon weapon in weapons)
            {
                if (weapon == null)
                    continue;

                cooldowns.Add(0);
                maxCooldowns.Add(weapon.maxCooldown);
            }
            
            rotation = MathHelper.PiOver2;
            scale = 4;
        }

        /// <summary>
        /// Creates enemy BattleShip
        /// </summary>
        /// <param name="baseShip">Ship to base enemy on</param>
        /// <param name="weapons">Enemy's weapons</param>
        private BattleShip(Ship baseShip, List<Weapon> weapons)
            : base(baseShip.texture)
        {
            name = "Enemy " + baseShip.name;
            maxHullPoints = baseShip.maxHullPoints;
            this.weapons = weapons.ToArray();

            cooldowns = new List<double>();
            maxCooldowns = new List<int>();
            hullPoints = maxHullPoints;

            foreach (Weapon weapon in weapons)
            {
                cooldowns.Add(0);
                maxCooldowns.Add((int)(weapon.maxCooldown * 1.2));
            }


            position.X = 800;
            rotation = -MathHelper.PiOver2;
            scale = 4;
        }

        public override void Update(GameTime gameTime)
        {
            if (cooldownTimes == null)
            {
                cooldownTimes = new List<double>();

                foreach (Weapon weapon in weapons)
                    cooldownTimes.Add(gameTime.TotalGameTime.TotalMilliseconds);
            }

            if (fireTime == 0) //not practically initialized
                fireTime = gameTime.TotalGameTime.TotalMilliseconds;

            //update fire timer
            if (fireCount > 0 && gameTime.TotalGameTime.TotalMilliseconds > fireTime + 500)
            {
                hullPoints -= fireCount;
                fireTime = gameTime.TotalGameTime.TotalMilliseconds;
            }

            //update cooldown timers
            for (int i = 0; i < cooldowns.Count; i++)
            {
                if (cooldowns[i] < maxCooldowns[i] &&
                    gameTime.TotalGameTime.TotalMilliseconds > cooldownTimes[i] + 250)
                {
                    cooldowns[i] += 0.25;
                    cooldownTimes[i] = gameTime.TotalGameTime.TotalMilliseconds;
                }
                else if (cooldowns[i] > maxCooldowns[i])
                    cooldowns[i] = maxCooldowns[i];
            }
            
            shield.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch, Color color)
        {
            spritebatch.Draw(texture, position, new Rectangle(0, 0, width / 2, height),
                color, rotation, new Vector2(width / 4, height / 2), scale,
                SpriteEffects.None, 1);

            shield?.Draw(gameTime, spritebatch);
        }

        public void EMP()
        {
            for (int i = 0; i < cooldowns.Count; i++)
            {
                cooldowns[i] = 0;
            }

            shield.EMP();
        }

        /// <summary>
        /// Creates a random enemy based on level/difficulty
        /// </summary>
        /// <returns></returns>
        public static BattleShip CreateEnemyShip(int level)
        {
            Random rand = new Random();

            //Get a random ship that has a level less than given level parameter
            Ship ship = Ship.ships.Where(x => x.level <= level).OrderBy(x => rand.Next()).First();

            List<Weapon> weapons = new List<Weapon>();
            foreach (Type type in ship.weaponTypes)
                weapons.Add(Weapon.weapons.Where(x => x.GetType() == type && x.level <= level).OrderBy(x => rand.Next()).First());

            //TEMP
            return new BattleShip(ship, weapons);
        }
    }
}
