using BPA_RPG.GameItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA_RPG.GameObjects
{
    public class WeaponProjectile : GameObject
    {
        private Weapon weapon;
        private BattleShip target;
        private bool hasMissed;

        private Random rand = new Random();

        public WeaponProjectile(Weapon weapon, Vector2 position, BattleShip target)
            : base (weapon.projectileTexture)
        {
            this.weapon = weapon;
            this.position = position;
            this.target = target;
        }

        public override void Update(GameTime gameTime)
        {
            position.X += (float)Math.Cos(rotation) * 10;
            position.Y += (float)Math.Sin(rotation) * 10;

            //check collide
            if (IntersectPixels(target))
            {
                if (rand.NextDouble() < weapon.hitChance)
                {
                    target.hullPoints -= weapon.damage;
                }
            }
        }
    }
}
