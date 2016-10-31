using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA_RPG.GameItems
{
    public abstract class Weapon : ShipPart
    {
        public readonly int damage;
        public readonly int shots;
        public readonly int maxCooldown;

        protected Weapon(string name, Texture2D texture, int damage, int shots, int maxCooldown)
            : base(name, texture)
        {
            this.damage = damage;
            this.shots = shots;
            this.maxCooldown = maxCooldown;
        }

        public static new void LoadContent(ContentManager content)
        {
            MainGame.eventLogger.Log(typeof(Weapon), "Begin loading weapons");

            LaserWeapon.LoadContent(content);

            MainGame.eventLogger.Log(typeof(Weapon), "Finished loading weapons");
        }
    }
}
