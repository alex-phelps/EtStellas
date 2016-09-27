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
    class GameItem
    {
        public GameItem()
        {
            MainGame.eventLogger.Log(this, "Loaded");
        }
    }
}
