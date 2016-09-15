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
    class Planet : GameObject
    {
        //Static planets
        public static Planet DebugPlanet;


        public string name { get; private set; }
        private EventHandler storyEvent;

        public Planet(string name, Texture2D texture) 
            : base(texture)
        {
            this.name = name;
        }
    }
}
