using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using EtStellas.GameObjects;
using Microsoft.Xna.Framework.Audio;

namespace EtStellas.GameItems.Weapons
{
    public class BombWeapon : Weapon
    {
        public enum Effect
        {
            emp,
            fire
        }
        
        private static SoundEffectInstance emp;


        public Effect effect { get; private set; }

        public BombWeapon(string name, int level, Texture2D texture, Texture2D projectileTexture, int damage, int maxCooldown, float hitChance, Effect effect, string info = "")
            : base(name, level, texture, projectileTexture, damage, 1, maxCooldown, hitChance, 4, info, true)
        {
            this.effect = effect;
        }

        protected override void GotHit(BattleShip target)
        {
            base.GotHit(target);

            switch (effect)
            {
                case Effect.emp:
                    emp.Pause();
                    emp.Play();

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

            new BombWeapon("Fire Bomb", 2, content.Load<Texture2D>("Images/Items/Weapons/FireBomb"), content.Load<Texture2D>("Images/FireBomb"),
                4, 8, .55f, Effect.fire, "A bomb weapon that sets enemy's systems on fire.");
            new BombWeapon("EMP Bomb", 1, content.Load<Texture2D>("Images/Items/Weapons/EMPBomb"), content.Load<Texture2D>("Images/EMPBomb"),
                0, 8, .7f, Effect.emp, "Launches a bomb that released an electromagnetic pulse to disable enemy systems.");
            
            emp = SoundEffectManager.GetEffectInstance("EMP1");

            MainGame.eventLogger.Log(typeof(LaserWeapon), "Finished loading bomb weapons");
        }
    }
}
