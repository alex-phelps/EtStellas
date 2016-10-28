using BPA_RPG.GameItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA_RPG.GameObjects
{
    public class EnemyShip : GameObject
    {
        private Ship baseShip;

        public string name => "Enemy " + baseShip.name;
        public int hullPoints;
        public int maxHullPoints => baseShip.maxHullPoints;

        public EnemyShip(Ship baseShip)
            : base(baseShip.texture)
        {
            this.baseShip = baseShip;

            hullPoints = maxHullPoints;
        }
    }
}
