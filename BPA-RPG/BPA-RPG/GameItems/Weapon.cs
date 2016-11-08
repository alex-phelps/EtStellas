using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BPA_RPG.GameItems
{
    public abstract class Weapon : ShipPart
    {
        public readonly int damage;
        public readonly int shots;
        public readonly int maxCooldown;
        public readonly float hitChance;

        protected Weapon(string name, Texture2D texture, int damage, int shots, int maxCooldown, float hitChance)
            : base(name, texture)
        {
            this.damage = damage;
            this.shots = shots;
            this.maxCooldown = maxCooldown;
            this.hitChance = hitChance;
        }

        public static new void LoadContent(ContentManager content)
        {
            MainGame.eventLogger.Log(typeof(Weapon), "Begin loading weapons");

            LaserWeapon.LoadContent(content);

            MainGame.eventLogger.Log(typeof(Weapon), "Finished loading weapons");
        }
    }
}
