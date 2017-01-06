using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA_RPG.GameItems.Weapons
{
    public class BombWeapon : Weapon
    {
        public enum Effect
        {
            emp,
            fire
        }

        public BombWeapon(string name, Texture2D texture, Texture2D projectileTexture, int damage, int maxCooldown, float hitChance, string info = "")
            : base(name, texture, projectileTexture, damage, 1, maxCooldown, hitChance, 4, info)
        {

        }
    }
}
