using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA_RPG.GameObjects
{
    public class Planet : GameObject
    {
        //Static planets
        public static Planet DebugPlanet { get; private set; }
        public static Planet DebugPlanet2 { get; private set; }


        public readonly string name;
        public float orbitDistance => texture.Width * 2f / 3f;

        public Planet(string name, Texture2D texture) 
            : base(texture)
        {
            this.name = name;
        }

        public Planet(string name, Texture2D texture, Vector2 position) 
            : this(name, texture)
        {
            this.position = position;
        }

        public static void LoadContent(ContentManager content)
        {
            MainGame.eventLogger.Log(typeof(Planet), "Begin loading static ships");
            
            //Define static planets
            DebugPlanet = new Planet("Debug Planet", content.Load<Texture2D>("Images/DebugPlanet"));
            DebugPlanet2 = new Planet("Debug Planet 2", content.Load<Texture2D>("Images/PlanetA"), new Vector2(3000, 2000));

            MainGame.eventLogger.Log(typeof(Planet), "Finished loading static planets");
        }
    }
}
