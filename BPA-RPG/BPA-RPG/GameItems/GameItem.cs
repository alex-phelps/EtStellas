using BPA_RPG.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA_RPG.GameItems
{
    public class GameItem : GameObject
    {
        //Static items
        public static GameItem Fuel { get; private set; }



        public readonly string name;

        protected GameItem(string name, Texture2D texture)
            : base(texture)
        {
            this.name = name;

            MainGame.eventLogger.Log(this, "Loaded \"" + name + "\"");
        }

        public static void LoadContent(ContentManager content)
        {
            MainGame.eventLogger.Log(typeof(GameItem), "Begin loading game items.");

            Fuel = new GameItem("Fuel", content.Load<Texture2D>("Images/Items/Fuel"));

            MainGame.eventLogger.Log(typeof(GameItem), "Finished loading game items.");
        }

        public static GameItem ItemFromText(string line)
        {
            switch (line.ToLower())
            {
                case "fuel":
                    return Fuel;
                default:
                    return Fuel;
            }
        }
    }
}
