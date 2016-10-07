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
        public readonly string name;

        public GameItem(string name, Texture2D texture)
            : base(texture)
        {
            this.name = name;

            MainGame.eventLogger.Log(this, "Loaded");
        }
    }
}
