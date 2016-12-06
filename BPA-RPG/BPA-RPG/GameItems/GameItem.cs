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

                for (int i = 0; i < info.Length; i++)
                {
                    if (font.MeasureString(info.Substring(0, i)).X > 164)
                    {
                        if (info.Contains(" "))
                        {
                            int k = info.Substring(0, i).LastIndexOf(" ");
                            string newLine = info.Remove(k, 1).Insert(k, "\n");

                            if (font.MeasureString(newLine.Substring(0, i)).X <= 164)
                            {
                                info = newLine;
                                i++;
                            }
                        }

                        //If that didn't fix it
                        if (font.MeasureString(info.Substring(0, i)).X > 164)
                        {
                            info = info.Insert(i - 2, "-\n");
                            i += 2;
                        }
                    }
                }


                Vector2 boxPos = pos;
                spritebatch.Draw(infoBoxCap, boxPos, Color.White);
                boxPos += new Vector2(0, infoBoxCap.Height);

                for (int i = 0; i <= info.Count(c => c == '\n'); i++)
                {
                    spritebatch.Draw(infoBox, boxPos, Color.White);
                    boxPos += new Vector2(0, infoBox.Height);
                }
                
                Vector2 b = font.MeasureString("N\nN");

                spritebatch.Draw(infoBoxCap, boxPos, new Rectangle(0, 0, infoBoxCap.Width, infoBoxCap.Height), Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.FlipVertically, 1);
                spritebatch.DrawString(font, info, pos + new Vector2(6, infoBoxCap.Height), Color.White);
            }
        }

        public static void LoadContent(ContentManager content)
        {
            MainGame.eventLogger.Log(typeof(GameItem), "Begin loading game items.");

            infoBoxCap = content.Load<Texture2D>("Images/ItemInfoBoxCap");
            infoBox = content.Load<Texture2D>("Images/ItemInfoBox");
            font = content.Load<SpriteFont>("Fonts/KeyFont");

            Fuel = new GameItem("Fuel", content.Load<Texture2D>("Images/Items/Fuel"), "Plutonium fuel for spacecrafts.");

            MainGame.eventLogger.Log(typeof(GameItem), "Finished loading game items.");
        }

        public static GameItem Parse(string line)
        {
            switch (line.ToLower())
            {
                case "fuel":
                    return Fuel;
                case "startership":
                    return Ship.StarterShip;
                case "neoncruiser":
                    return Ship.NeonCruiser;
                case "basicengine":
                    return Engine.BasicEngine;
                case "cryothermalengine":
                    return Engine.CryoThermalEngine;
                default:
                    throw new FormatException();
            }
        }
    }
}
