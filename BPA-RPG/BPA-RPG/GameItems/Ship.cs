using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA_RPG.GameItems
{
    class Ship : GameItem
    {
        //Static ships
        public static Ship DebugShip;



        public string name { get; private set; }
        public Texture2D texture { get; private set; }

        public Ship(string name, Texture2D texture) 
            : base()
        {
            this.name = name;
            this.texture = texture;
        }
    }
}
