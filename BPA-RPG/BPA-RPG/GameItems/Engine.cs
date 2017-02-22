using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace EtStellas.GameItems
{
    public class Engine : ShipPart
    {
        //Static engines
        public static List<Engine> engines = new List<Engine>();

        
        /// <summary>
        /// Acceleration boost the engine gives
        /// </summary>
        public readonly int power;

        /// <summary>
        /// Fuel used per 10,000 meters (pixels)
        /// </summary>
        public readonly float fuelRate;


        public Engine(string name, Texture2D texture, int power, float fuelRate, string info = "")
            : base(name, texture, info)
        {
            this.power = power;
            this.fuelRate = fuelRate;

            engines.Add(this);
        }

        public static new void LoadContent(ContentManager content)
        {
            MainGame.eventLogger.Log(typeof(Engine), "Begin loading engines");

            new Engine("Basic Engine", content.Load<Texture2D>("Images/Items/BasicEngine"), 10, 1.5f, "A basic plutonium engine");
            new Engine("Cryothermal Engine", content.Load<Texture2D>("Images/Items/CryothermalEngine"), 15, 1.3f, "An engine powered by absolute zero temperatures");
            new Engine("Antimatter Engine", content.Load<Texture2D>("Images/Items/AntimatterEngine"), 35, 1, "An engine that generates mass amounts of power from anti particles");

            MainGame.eventLogger.Log(typeof(Engine), "Finished loading engines");
        }
    }
}
