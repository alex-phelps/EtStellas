using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA_RPG.GameItems
{
    public class LaserWeapon : Weapon
    {
        public static LaserWeapon BasicLaser { get; private set; }



        public LaserWeapon(string name, Texture2D texture, int damage, int shots, int maxCooldown)
            : base(name, texture, damage, shots, maxCooldown)
        {

        }

        public static new void LoadContent(ContentManager content)
        {
            MainGame.eventLogger.Log(typeof(LaserWeapon), "Begin loading laser weapons");

            BasicLaser = new LaserWeapon("Basic Laser", content.Load<Texture2D>("Images/DebugTexture"), 5, 2, 6);

            MainGame.eventLogger.Log(typeof(LaserWeapon), "Finished loading laser weapons");
        }
    }
}
