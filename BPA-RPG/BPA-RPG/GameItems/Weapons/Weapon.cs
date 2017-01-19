using BPA_RPG.GameObjects;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BPA_RPG.GameItems.Weapons
{
    public abstract class Weapon : ShipPart
    {
        public readonly Texture2D projectileTexture;
        public readonly int damage;
        public readonly int shots;
        public readonly int maxCooldown;
        public readonly float hitChance;
        public readonly int speed;
        public readonly bool passShield;

        private Random rand;

        protected Weapon(string name, Texture2D texture, Texture2D projectileTexture, int damage, int shots,
            int maxCooldown, float hitChance, int speed, string info = "", bool passShield = false)
            : base(name, texture, info)
        {
            this.projectileTexture = projectileTexture;
            this.damage = damage;
            this.shots = shots;
            this.speed = speed;
            this.maxCooldown = maxCooldown;
            this.hitChance = hitChance;
            this.passShield = passShield;

            rand = new Random();
        }

        public bool HitShip(BattleShip target)
        {
            if (rand.NextDouble() < hitChance)
            {
                GotHit(target);
                return true;
            }
            else return false;
        }

        protected virtual void GotHit(BattleShip target)
        {
            target.hullPoints -= damage;
        }

        public static new void LoadContent(ContentManager content)
        {
            MainGame.eventLogger.Log(typeof(Weapon), "Begin loading weapons");

            LaserWeapon.LoadContent(content);
            MissileWeapon.LoadContent(content);
            BombWeapon.LoadContent(content); 

            MainGame.eventLogger.Log(typeof(Weapon), "Finished loading weapons");
        }
    }
}
