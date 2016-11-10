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
        private List<WeaponProjectile> deleteList;
        private Weapon weapon;
        private BattleShip target;
        private bool hasMissed;
        private int speed;

        private Random rand = new Random();

        public WeaponProjectile(Weapon weapon, Vector2 position, BattleShip target, int speed, List<WeaponProjectile> deleteList)
            : base (weapon.projectileTexture)
        {
            this.weapon = weapon;
            this.position = position;
            this.target = target;
            this.speed = speed;
            this.deleteList = deleteList;
        }

        public override void Update(GameTime gameTime)
        {
            position.X += (float)Math.Cos(rotation) * speed;
            position.Y += (float)Math.Sin(rotation) * speed;

            //check collide
            if (!hasMissed && IntersectPixels(target))
            {
                if (rand.NextDouble() < weapon.hitChance)
                {
                    target.hullPoints -= weapon.damage;
                    deleteList.Add(this);
                }
                else hasMissed = true;
            }
        }
    }
}
