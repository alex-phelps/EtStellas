using BPA_RPG.GameItems.Weapons;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA_RPG.GameItems.Weapons
{
    public class LaserWeapon : Weapon
    {
        public static LaserWeapon BasicLaser { get; private set; }



        public LaserWeapon(string name, int level, Texture2D texture, Texture2D projectileTexture, int damage, int shots, int maxCooldown, float hitChance, string info = "")
            : base(name, level, texture, projectileTexture, damage, shots, maxCooldown, hitChance, 10, info)
        {
        }

        public static new void LoadContent(ContentManager content)
        {
            MainGame.eventLogger.Log(typeof(LaserWeapon), "Begin loading laser weapons");

            Texture2D projTex = content.Load<Texture2D>("Images/Laser");

            BasicLaser = new LaserWeapon("Basic Laser", 1, content.Load<Texture2D>("Images/Items/Weapons/BasicLaser"), projTex,
                5, 2, 6, 0.95f, "A basic two shot laser weapon.");

            MainGame.eventLogger.Log(typeof(LaserWeapon), "Finished loading laser weapons");
        }
    }
}
