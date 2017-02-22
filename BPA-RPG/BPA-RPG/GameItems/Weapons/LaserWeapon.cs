using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EtStellas.GameItems.Weapons
{
    public class LaserWeapon : Weapon
    {
        public LaserWeapon(string name, int level, Texture2D texture, Texture2D projectileTexture, int damage, int shots, int maxCooldown, float hitChance, string info = "")
            : base(name, level, texture, projectileTexture, damage, shots, maxCooldown, hitChance, 10, info)
        {
        }

        public static new void LoadContent(ContentManager content)
        {
            MainGame.eventLogger.Log(typeof(LaserWeapon), "Begin loading laser weapons");

            Texture2D projTex = content.Load<Texture2D>("Images/Laser");

            new LaserWeapon("Basic Laser", 1, content.Load<Texture2D>("Images/Items/Weapons/BasicLaser"), projTex,
                5, 2, 6, 0.95f, "A basic two shot laser weapon.");
            new LaserWeapon("Weak Laser", 0, content.Load<Texture2D>("Images/Items/Weapons/WeakLaser"), projTex,
                5, 1, 4, 0.95f, "A basic single shot laser weapon.");
            new LaserWeapon("Burst Laser", 3, content.Load<Texture2D>("Images/Items/Weapons/BurstLaser"), projTex,
                6, 3, 8, 0.9f, "A three shot laser weapon. Standard among veteran vessels.");

            MainGame.eventLogger.Log(typeof(LaserWeapon), "Finished loading laser weapons");
        }
    }
}
