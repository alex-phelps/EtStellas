using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BPA_RPG.GameObjects
{
    public class Shield : GameObject
    {
        bool canActivate = true;

        public double shieldTimeMax { get; private set; }
        public double shieldTimeTime { get; private set; }
        public double shieldTimeOld { get; private set; }

        public double cooldownMax { get; private set; }
        public double cooldownTime { get; private set; }
        public double cooldownOld { get; private set; }

        public Shield(Texture2D texture, BattleShip ship)
            : base(texture)
        {
            position = ship.position;
            ScaleTo(ship.width, ship.height, ship.scale);

            shieldTimeMax = 4000;
            cooldownMax = 12000;
        }

        public override void Update(GameTime gameTime)
        {
            if (shieldTimeOld == 0)
                shieldTimeOld = gameTime.TotalGameTime.TotalMilliseconds;
            if (cooldownOld == 0)
                cooldownOld = gameTime.TotalGameTime.TotalMilliseconds;

            if (shieldTimeTime > 0 &&
                gameTime.TotalGameTime.TotalMilliseconds > shieldTimeOld + 200)
            {
                shieldTimeTime -= 200;
                shieldTimeOld = gameTime.TotalGameTime.TotalMilliseconds;
            }
            else if (shieldTimeTime < 0)
                shieldTimeTime = 0;
            else if (shieldTimeTime == 0)
                visible = false;
            else visible = true;

            if (cooldownTime < cooldownMax && !visible &&
                gameTime.TotalGameTime.TotalMilliseconds > cooldownOld + 200)
            {
                cooldownTime += 200;
                cooldownOld = gameTime.TotalGameTime.TotalMilliseconds;
            }
            else if (cooldownTime > cooldownMax)
                cooldownTime = cooldownMax;
            else if (cooldownTime == cooldownMax)
                canActivate = true;
            else canActivate = false;

            base.Update(gameTime);
        }

        public void Activate()
        {
            if (!visible && canActivate)
            {
                shieldTimeTime = shieldTimeMax;
                cooldownTime = 0;
            }
        }

        public void EMP()
        {
            visible = false;
            shieldTimeTime = 0;
            cooldownTime = 0;
        }
    }
}
