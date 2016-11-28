using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA_RPG.GameObjects
{
    public class Planet : GameObject
    {
        //Static planets
        public static List<Planet> planets;


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
            MainGame.eventLogger.Log(typeof(Planet), "Begin loading planets");

            planets = new List<Planet>();
            List<List<Vector2>> ranges = new List<List<Vector2>>()
            {
                new List<Vector2>()
                {
                    Vector2.Zero
                },
                new List<Vector2>() // lvl 1, under 10k
                {
                    new Vector2(-4500, -3000),
                    new Vector2(1200, -7000),
                    new Vector2(-2500, 5000),
                    new Vector2(4000, 3500),
                    new Vector2(7500, -2000)
                },
                new List<Vector2>() // lvl 2, under 30k
                {
                    new Vector2(-11000, -11500),
                    new Vector2(-13000, -12000),
                    new Vector2(-19000, -6000),
                }
            };

            try
            {
                Texture2D debugImage = content.Load<Texture2D>("Images/DebugPlanet");
                StreamReader file = File.OpenText("Content/Scripts/Planets.txt");
                List<string> lines = new List<string>();
                Random rand = new Random();

                while (!file.EndOfStream)
                    lines.Add(file.ReadLine());

                //Loop through lines randomly
                foreach (string s in lines.OrderBy(x => rand.Next()))
                {
                    //Ignore comments
                    if (s.StartsWith("//"))
                        continue;

                    //Get name and range of planet location
                    int range = int.Parse(s.Substring(s.LastIndexOf(" ")));
                    string name = s.Remove(s.LastIndexOf(" "));

                    if (ranges[range].Count == 0)
                        continue;

                    //Get random pos in range
                    int posIndex = rand.Next(ranges[range].Count);
                    Vector2 pos = ranges[range][posIndex];
                    ranges[range].RemoveAt(posIndex);

                    //Create planet
                    planets.Add(new Planet(name, File.Exists("Content/Images/" + name.Replace(" ", "") + ".xnb") 
                        ? content.Load<Texture2D>("Images/" + name.Replace(" ", "")) : debugImage, pos));
                }
            }
            catch (Exception e)
            {
                MainGame.eventLogger.Log(typeof(Planet), "ERROR LOADING PLANETS: " + e.Message);
                throw e;
            }
            
            MainGame.eventLogger.Log(typeof(Planet), "Finished loading planets");
        }
    }
}
