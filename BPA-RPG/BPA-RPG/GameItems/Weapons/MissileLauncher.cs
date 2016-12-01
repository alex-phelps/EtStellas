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
    public class MissileLauncher : Weapon
    {
        public static MissileLauncher BasicMissile { get; private set; }



        private Random rand;
        private int damageBonus;

        public MissileLauncher(string name, Texture2D texture, Texture2D projectileTexture, int damage, int damageBonus, int shots, int maxCooldown, float hitChance, string info = "", bool passShield = true)
            : base(name, texture, projectileTexture, damage, shots, maxCooldown, hitChance, passShield, info)
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

            BasicMissile = new MissileLauncher("Basic Missile", content.Load<Texture2D>("Images/DebugTexture"), projTex,
                20, 10, 1, 18, .75f, "A basic missile weapon that can pierce shields.");

            MainGame.eventLogger.Log(typeof(LaserWeapon), "Finished loading missile weapons");
        }
    }
}
