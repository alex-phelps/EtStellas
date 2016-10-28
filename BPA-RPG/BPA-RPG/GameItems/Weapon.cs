using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA_RPG.GameItems
{
    abstract class Weapon : GameItem
    {
        public readonly int damage;
        public int currentCooldown;
        public readonly int maxCooldown;

        public Weapon(string name, Texture2D texture, int damage, int maxCooldown)
            : base(name, texture)
        {
            this.damage = damage;
            this.maxCooldown = maxCooldown;
            currentCooldown = maxCooldown;
        }
    }
}
