using BPA_RPG.GameItems;
using BPA_RPG.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA_RPG
{
    public static class PlayerData
    {
        public static List<GameItem> inventory = new List<GameItem>();
        public static PlayerShip ship;
        public static int credits;


        public static void Save(string filename)
        {

            MainGame.eventLogger.Log(typeof(PlayerData), "Saved Game to \"" + filename + "\"");
        }

        public static void Load(string filename)
        {


            MainGame.eventLogger.Log(typeof(PlayerData), "Loaded \"" + filename + "\"");
        }
    }
}
