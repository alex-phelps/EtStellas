using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPA_RPG.GameObjects;

namespace BPA_RPG.GameItems.Weapons
{
    public class BombWeapon : Weapon
    {
        public enum Effect
        {
            emp,
            fire
        }

        public static BombWeapon FireBomb { get; private set; }
        public static BombWeapon EMPBomb { get; private set; }


        public Effect effect { get; private set; }

        public BombWeapon(string name, Texture2D texture, Texture2D projectileTexture, int damage, int maxCooldown, float hitChance, Effect effect, string info = "")
            : base(name, texture, projectileTexture, damage, 1, maxCooldown, hitChance, 4, info, true)
        {
            this.effect = effect;
        }

        protected override void GotHit(BattleShip target)
        {
            base.GotHit(target);

            switch (effect)
            {
                case Effect.emp:
                    target.EMP();
                    break;

                case Effect.fire:
                    target.fireCount++;
                    break;
            }
        }

        public static new void LoadContent(ContentManager content)
        {
            MainGame.eventLogger.Log(typeof(LaserWeapon), "Begin loading bomb weapons");

            FireBomb = new BombWeapon("Fire Bomb", content.Load<Texture2D>("Images/DebugTexture"), content.Load<Texture2D>("Images/FireBomb"),
                4, 8, .55f, Effect.fire, "A bomb weapon that sets enemy's systems on fire.");
            EMPBomb = new BombWeapon("EMP Bomb", content.Load<Texture2D>("Images/DebugTexture"), content.Load<Texture2D>("Images/EMPBomb"),
                0, 8, .7f, Effect.emp, "Launches a bomb that released an electromagnetic pulse to disable enemy systems.");

            MainGame.eventLogger.Log(typeof(LaserWeapon), "Finished loading bomb weapons");
        }
    }
}
