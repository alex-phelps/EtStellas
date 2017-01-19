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
        public static MissileWeapon BasicMissile { get; private set; }



        private Random rand;
        private int damageBonus;

        public MissileWeapon(string name, Texture2D texture, Texture2D projectileTexture, int damage, int damageBonus, int shots, int maxCooldown, float hitChance, string info = "", bool passShield = true)
            : base(name, texture, projectileTexture, damage, shots, maxCooldown, hitChance, 8, info, passShield)
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

            BasicMissile = new MissileWeapon("Basic Missile", content.Load<Texture2D>("Images/Items/Weapons/BasicMissile"), projTex,
                20, 10, 1, 15, .75f, "A basic missile weapon that can pierce shields.");

            MainGame.eventLogger.Log(typeof(LaserWeapon), "Finished loading missile weapons");
        }
    }
}
