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
        public string name { get; private set; }
        private EventHandler storyEvent;

        public Planet(ContentManager content, string name, Vector2 position, EventHandler storyEvent) 
            : base(content.Load<Texture2D>("Images/DebugPlanet"))
        {
            this.name = name;
            this.position = position;
            this.storyEvent = storyEvent;
        }
    }
}
