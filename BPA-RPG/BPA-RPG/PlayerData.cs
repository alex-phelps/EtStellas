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
        private static PlayerShip Ship;
        public static PlayerShip ship
        {
            get
            {
                return Ship;
            }
            set
            {
                Ship = value;
                MainGame.eventLogger.Log(typeof(PlayerData), "Player ship = " + Ship);
            }
        }
        private static int Credits;
        public static int credits
        {
            get
            {
                return Credits;
            }
            set
            {
                Credits = value;
                MainGame.eventLogger.Log(typeof(PlayerData), "Player credits = " + Credits);
            }
        }

        public static List<GameItem> inventory = new List<GameItem>();

        /// <summary>
        /// Saves player data to file
        /// </summary>
        /// <param name="filename"></param>
        public static void Save(string filename)
        {

            MainGame.eventLogger.Log(typeof(PlayerData), "Saved Game to \"" + filename + "\"");
        }

        /// <summary>
        /// Saves player data to file
        /// </summary>
        /// <param name="filename"></param>
        public static void Load(string filename)
        {


            MainGame.eventLogger.Log(typeof(PlayerData), "Loaded \"" + filename + "\"");
        }
    }
}
