using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA_RPG.GameItems
{
    public class Engine : ShipPart
    {
        //Static engines
        public static List<Engine> engines = new List<Engine>();



        public readonly int power;
        public readonly float fuelRate; // fuel used per 10,000 meters (pixels)

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

            new Engine("Basic Engine", content.Load<Texture2D>("Images/Items/BasicEngine"), 20, 1.2f, "A basic plutonium engine");
            new Engine("Cryothermal Engine", content.Load<Texture2D>("Images/Items/CryothermalEngine"), 20, 1.2f, "An engine powered by absolute zero temperatures");

            MainGame.eventLogger.Log(typeof(Engine), "Begin loading engines");
        }
    }
}
