using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPA_RPG.GameObjects;

namespace BPA_RPG.GameItems.Weapons
{
    public class MissileWeapon : Weapon
    {
        private Random rand;
        private int damageBonus;

        public MissileWeapon(string name, int level, Texture2D texture, Texture2D projectileTexture, int damage, int damageBonus, int shots, int maxCooldown, float hitChance, string info = "", bool passShield = true)
            : base(name, level, texture, projectileTexture, damage, shots, maxCooldown, hitChance, 8, info, passShield)
        {
            this.damageBonus = damageBonus;

            rand = new Random();
        }

        protected override void GotHit(BattleShip target)
        {
            target.hullPoints -= damage + rand.Next(damageBonus);
        }

        public static new void LoadContent(ContentManager content)
        {
            MainGame.eventLogger.Log(typeof(LaserWeapon), "Begin loading missile weapons");

            Texture2D projTex = content.Load<Texture2D>("Images/Missile");

            new MissileWeapon("Basic Missile", 1, content.Load<Texture2D>("Images/Items/Weapons/BasicMissile"), projTex,
                20, 10, 1, 15, .75f, "A basic missile weapon that can pierce shields.");
            new MissileWeapon("Heavy Missile", 3, content.Load<Texture2D>("Images/Items/Weapons/HeavyMissile"), projTex,
                15, 20, 2, 20, .7f, "A two shot missile with high damage but somewhat unrealiable.");

            MainGame.eventLogger.Log(typeof(LaserWeapon), "Finished loading missile weapons");
        }
    }
}
