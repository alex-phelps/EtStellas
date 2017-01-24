using BPA_RPG.GameItems;
using BPA_RPG.GameItems.Weapons;
using BPA_RPG.GameObjects;
using System;
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
        public static PlayerShip ship;
        public static string name = "Silvester";
        public static List<GameItem> inventory = new List<GameItem>();
        public static Weapon[] weapons;
        public static Engine engine;

        private static int credits;
        private static int jex;

        public static void AddMoney(Currency currency, int value)
        {
            switch (currency)
            {
                case Currency.credits:
                    credits += value;
                    if (credits < 0)
                        credits = 0;

                    MainGame.eventLogger.Log(typeof(PlayerData), "Player credits = " + credits);
                    break;
                case Currency.jex:
                    jex += value;
                    if (jex < 0)
                        jex = 0;

                    MainGame.eventLogger.Log(typeof(PlayerData), "Player jex = " + jex);
                    break;
            }
        }

        public static int GetMoney(Currency currency)
        {
            switch (currency)
            {
                case Currency.credits:
                    return credits;
                case Currency.jex:
                    return jex;
                default:
                    return -1;
            }
        }

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
