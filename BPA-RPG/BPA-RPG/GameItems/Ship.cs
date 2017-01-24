using BPA_RPG.GameItems.Weapons;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA_RPG.GameItems
{
    public class Ship : GameItem
    {
        //Static ships
        public static List<Ship> ships = new List<Ship>();


        public readonly int level;
        public readonly int holdSize;
        public readonly int maxHullPoints;
        public readonly float maxSpeed;
        public readonly float accel;
        public readonly float maxRotSpeed;
        public readonly float rotAccel;
        public readonly List<Type> weaponTypes;

        public Ship(string name, int level, Texture2D texture, int maxHullPoints, int holdSize, float maxSpeed, float accel,
            float maxRotSpeed, float rotAccel, List<Type> weaponHold, string info = "") 
            : base(name, texture, info + "\n\nSpeed: " + maxSpeed * 100 + "\nHull: " + maxHullPoints + "\nHold Size: " + holdSize) // Add stats to info
        {
            this.level = level;
            this.maxHullPoints = maxHullPoints;
            this.holdSize = holdSize;
            this.maxSpeed = maxSpeed;
            this.accel = accel;
            this.maxRotSpeed = maxRotSpeed;
            this.rotAccel = rotAccel;
            
            for (int i = 0; i < weaponHold.Count; i++)
            {
                //Make sure types are a weapon
                if (!typeof(Weapon).IsAssignableFrom(weaponHold[i]))
                {
                    weaponHold.RemoveAt(i);
                    i--;
                }
            }
            this.weaponTypes = weaponHold;

            ships.Add(this);
        }

        public static new void LoadContent(ContentManager content)
        {
            MainGame.eventLogger.Log(typeof(Ship), "Begin loading ships");

            //Define static ships
            new Ship("Discovery", 1, content.Load<Texture2D>("Images/Ships/Discovery"),
                80, 20, 6, 0.05f, 0.025f, 0.0003f, new List<Type>() { typeof(LaserWeapon), typeof(MissileWeapon) }, 
                "A basic, slow trading ship.");
            new Ship("Neon Cruiser", 2, content.Load<Texture2D>("Images/Ships/NeonCruiser"),
                120, 25, 12, 0.1f, 0.03f, 0.0004f, new List<Type>() { typeof(LaserWeapon), typeof(LaserWeapon), typeof(BombWeapon) },
                "A quick starcraft with cryothermal thusters.");
            new Ship("Dumpster", 0, content.Load<Texture2D>("Images/Ships/Dumpster"),
                40, 8, 3, 0.01f, 0.015f, 0.0001f, new List<Type>() { typeof(LaserWeapon) },
                "A small, slow, rusty ship that's dirt cheap on the black market.");
            new Ship("Viridian", 3, content.Load<Texture2D>("Images/Ships/Viridian"),
                130, 35, 6, 0.04f, 0.025f, 0.0002f, new List<Type>() { typeof(MissileWeapon), typeof(MissileWeapon), typeof(BombWeapon) },
                "A slower ship specializing in explosives.");
            new Ship("Eos", 2, content.Load<Texture2D>("Images/Ships/Eos"),
                80, 25, 10, 0.04f, 0.03f, 0.0003f, new List<Type>() { typeof(BombWeapon), typeof(BombWeapon), typeof(BombWeapon) },
                "An impractical ship that is easily spotted and only utilizes bomb weapons.");
            new Ship("Ocean Jet", 3, content.Load<Texture2D>("Images/Ships/OceanJet"),
                170, 40, 18, 0.12f, 0.04f, 0.0005f, new List<Type>() { typeof(LaserWeapon), typeof(LaserWeapon), typeof(MissileWeapon), typeof(BombWeapon) },
                "A high class transport ship with a kritonium hull and antimatter thrusters.");

            MainGame.eventLogger.Log(typeof(Ship), "Finished loading ships");
        }
    }
}
