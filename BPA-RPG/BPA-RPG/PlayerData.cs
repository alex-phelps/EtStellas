using EtStellas.GameItems;
using EtStellas.GameItems.Weapons;
using EtStellas.GameObjects;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;

namespace EtStellas
{
    /// <summary>
    /// The different currencies that money can be stored as
    /// </summary>
    public enum Currency
    {
        credits,
        jex
    }

    /// <summary>
    /// Struct to hold and serialize the player's save data
    /// </summary>
    public struct SaveData
    {
        //Uses two lists because XML can't serialize Dictionaries
        public List<string> planetNames;
        public List<Vector2> planetPosition;

        //Uses two lists because XML can't serialize Dictionaries
        public List<string> varNames;
        public List<int> varValues;

        public string shipName;
        public Vector2 shipPosition;
        public float shipRotation;

        public List<string> inventory;
        public string[] weapons;
        public string engine;
        public int credits;
        public int jex;
    }

    /// <summary>
    /// Struct to hold and serialize the game options
    /// </summary>
    public struct OptionsData
    {
        public bool isFullscreen;
        public float volume;
    }
    
    /// <summary>
    /// Class that holds the player's information and methods to use that information
    /// </summary>
    public static class PlayerData
    {
        public static PlayerShip ship;
        public static string name = "Silvester";
        public static List<GameItem> inventory = new List<GameItem>();
        public static Weapon[] weapons;
        public static Engine engine;

        private static int credits;
        private static int jex;

        /// <summary>
        /// Variables that are created and used by scripts
        /// </summary>
        private static Dictionary<string, int> variables = new Dictionary<string, int>();

        /// <summary>
        /// Set the player variable value for the given variable. If the variable has not been referenced yet, create it
        /// </summary>
        /// <param name="variable">Name of the script variable</param>
        /// <param name="value"></param>
        public static void SetVariable(string variable, int value)
        {
            variable = variable.ToLower();

            if (!variables.ContainsKey(variable))
                variables.Add(variable, value);
            else variables[variable] = value;
        }

        /// <summary>
        /// Gets the player variable value for the given variable. If the variable has not been referenced yet, create it with a default value of 0
        /// </summary>
        /// <param name="variable">Name of the script variable</param>
        /// <returns></returns>
        public static int GetVariable(string variable)
        {
            variable = variable.ToLower();

            if (!variables.ContainsKey(variable))
                variables.Add(variable, 0);

            return variables[variable];
        }

        /// <summary>
        /// Adds an amount of money to the player's total in the specified currency
        /// </summary>
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

        /// <summary>
        /// Gets the player's amount of the specified currency's money. Returns -1 if currency is not found
        /// </summary>
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
        public static void SaveGame()
        {
            string filename = "saveData.sav";

            XmlSerializer xml = new XmlSerializer(typeof(SaveData));

            //Save data to struct
            SaveData saveData = new SaveData()
            {
                planetNames = new List<string>(),
                planetPosition = new List<Vector2>(),
                varNames = new List<string>(),
                varValues = new List<int>(),
                shipName = ship.baseShip.name,
                shipPosition = ship.position,
                shipRotation = ship.rotation,
                inventory = new List<string>(),
                weapons = new string[weapons.Length],
                engine = engine?.name,
                credits = credits,
                jex = jex
            };

            foreach (Planet planet in Planet.planets)
            {
                saveData.planetNames.Add(planet.name);
                saveData.planetPosition.Add(planet.position);
            }

            foreach (KeyValuePair<string, int> variable in variables)
            {
                saveData.varNames.Add(variable.Key);
                saveData.varValues.Add(variable.Value);
            }

            foreach (GameItem item in inventory)
                saveData.inventory.Add(item.name);

            for (int i = 0; i < weapons.Length; i++)
                saveData.weapons[i] = weapons[i]?.name;

            //Serialize and save as XML file
            using (TextWriter writer = new StreamWriter(filename))
                xml.Serialize(writer, saveData);

            MainGame.eventLogger.Log(typeof(PlayerData), "Saved Game to \"" + filename + "\"");
        }

        /// <summary>
        /// Saves player data to file
        /// </summary>
        /// <param name="filename">Load file location</param>
        public static void LoadGame()
        {
            string filename = "saveData.sav";

            XmlSerializer xml = new XmlSerializer(typeof(SaveData));
            SaveData saveData;

            using (TextReader reader = new StreamReader(filename))
                saveData = (SaveData)xml.Deserialize(reader);


            //Load save data

            for (int i = 0; i < saveData.planetNames.Count; i++)
                Planet.planets.First(x => x.name == saveData.planetNames[i]).position = saveData.planetPosition[i];

            variables = new Dictionary<string, int>();
            for (int i = 0; i < saveData.varNames.Count; i++)
                variables.Add(saveData.varNames[i], saveData.varValues[i]);
            
            ship.baseShip = Ship.ships.First(x => x.name == saveData.shipName);
            ship.inOrbit = false;
            ship.position = saveData.shipPosition;
            ship.rotation = saveData.shipRotation;

            inventory = new List<GameItem>();
            foreach (string name in saveData.inventory)
                inventory.Add(GameItem.items.First(x => x.name == name));

            for (int i = 0; i < saveData.weapons.Length; i++)
                if (saveData.weapons[i] != null)
                    weapons[i] = Weapon.weapons.First(x => x.name == saveData.weapons[i]);

            if (saveData.engine != null)
                engine = Engine.engines.First(x => x.name == saveData.engine);

            credits = saveData.credits;
            jex = saveData.jex;

            MainGame.eventLogger.Log(typeof(PlayerData), "Loaded \"" + filename + "\"");
        }
    }
}
