using BPA_RPG.GameItems;
using BPA_RPG.GameObjects;
using System.Collections.Generic;

namespace BPA_RPG
{
    public enum Currency
    {
        credits,
        jex
    }

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
                MainGame.eventLogger.Log(typeof(PlayerData), "Player ship = " + Ship.name);
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

        private static int Jex;
        public static int jex
        {
            get
            {
                return Jex;
            }
            set
            {
                Credits = value;
                MainGame.eventLogger.Log(typeof(PlayerData), "Player jex = " + Jex);
            }
        }

        public static List<GameItem> inventory = new List<GameItem>();

        /// <summary>
        /// Saves player data to file
        /// </summary>
        /// <param name="filename">Save location</param>
        public static void Save(string filename)
        {

            MainGame.eventLogger.Log(typeof(PlayerData), "Saved Game to \"" + filename + "\"");
        }

        /// <summary>
        /// Saves player data to file
        /// </summary>
        /// <param name="filename">Load file location</param>
        public static void Load(string filename)
        {


            MainGame.eventLogger.Log(typeof(PlayerData), "Loaded \"" + filename + "\"");
        }
    }
}
