using Microsoft.Xna.Framework.Graphics;

namespace EtStellas.GameItems
{
    public abstract class ShipPart : GameItem
    {
        public ShipPart(string name, Texture2D texture, string info = "")
            : base(name, texture, info)
        {
        }
    }
}
