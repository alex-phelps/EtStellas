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
        public static Engine BasicEngine;



        public readonly int power;
        public readonly float fuelRate; // fuel used per 10,000 meters (pixels)

        public Engine(string name, Texture2D texture, int power, float fuelRate)
            : base(name, texture)
        {
            this.power = power;
            this.fuelRate = fuelRate;
        }

        public static new void LoadContent(ContentManager content)
        {
            MainGame.eventLogger.Log(typeof(Engine), "Begin loading engines");

            BasicEngine = new Engine("Basic Engine", content.Load<Texture2D>("Images/DebugTexture"), 20, 1.2f);

            MainGame.eventLogger.Log(typeof(Engine), "Begin loading engines");
        }
    }
}
