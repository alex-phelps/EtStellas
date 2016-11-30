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


        private static Texture2D infoBoxCap;
        private static Texture2D infoBox;
        private static SpriteFont font;


        public readonly string name;
        public string info { get; private set; }

        protected GameItem(string name, Texture2D texture, string info = "")
            : base(texture)
        {
            this.name = name;
            this.info = info;

            MainGame.eventLogger.Log(this, "Loaded \"" + name + "\"");
        }

        public void DrawInfo(SpriteBatch spritebatch)
        {
            if (info != "")
            {
                Vector2 pos = InputManager.newMouseState.Position.ToVector2() + new Vector2(12, 0);

                spritebatch.Draw(infoBoxCap, pos, Color.White);
                pos += new Vector2(0, infoBoxCap.Height);

                int lines = 1;
                foreach(char c in info)
                {
                    if (font.MeasureString(info).X > 172)
                    {
                        int i = info.LastIndexOf(" ");
                        info = info.Remove(i, i + 1).Insert(i, "\n");
                    }
                }

                for (int i = 0; i < lines; i++)
                {
                    spritebatch.Draw(infoBox, pos, Color.White);
                    spritebatch.DrawString(font, info, pos + new Vector2(6, 3), Color.White);
                    pos += new Vector2(0, infoBox.Height);
                }

                spritebatch.Draw(infoBoxCap, pos, new Rectangle(0, 0, infoBoxCap.Width, infoBoxCap.Height), Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.FlipVertically, 1);
            }
        }

        public static void LoadContent(ContentManager content)
        {
            MainGame.eventLogger.Log(typeof(GameItem), "Begin loading game items.");

            infoBoxCap = content.Load<Texture2D>("Images/ItemInfoBoxCap");
            infoBox = content.Load<Texture2D>("Images/ItemInfoBox");
            font = content.Load<SpriteFont>("Fonts/KeyFont");

            Fuel = new GameItem("Fuel", content.Load<Texture2D>("Images/Items/Fuel"), "Fuel for spacecrafts. blahhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh");

            MainGame.eventLogger.Log(typeof(GameItem), "Finished loading game items.");
        }

        public static GameItem ItemFromText(string line)
        {
            switch (line.ToLower())
            {
                case "fuel":
                    return Fuel;
                case "neoncruiser":
                    return Ship.NeonCruiser;
                default:
                    return Fuel;
            }
        }
    }
}
