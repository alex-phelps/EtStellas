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
        public static Ship Discovery { get; private set; }
        public static Ship NeonCruiser { get; private set; }
        public static Ship Dumpster { get; private set; }
        public static Ship Viridian { get; private set; }
        public static Ship Eos { get; private set; }


        public readonly int holdSize;
        public readonly int maxHullPoints;
        public readonly float maxSpeed;
        public readonly float accel;
        public readonly float maxRotSpeed;
        public readonly float rotAccel;
        public readonly List<Type> weaponHold;

        public Ship(string name, Texture2D texture, int maxHullPoints, int holdSize, float maxSpeed, float accel,
            float maxRotSpeed, float rotAccel, List<Type> weaponHold, string info = "") 
            : base(name, texture, info + "\n\nSpeed: " + maxSpeed * 100 + "\nHull: " + maxHullPoints + "\nHold Size: " + holdSize) // Add stats to info
        {
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
            this.weaponHold = weaponHold;
        }

        public static new void LoadContent(ContentManager content)
        {
            MainGame.eventLogger.Log(typeof(Ship), "Begin loading ships");

            //Define static ships
            Discovery = new Ship("Discovery", content.Load<Texture2D>("Images/Ships/Discovery"),
                80, 20, 6, 0.05f, 0.025f, 0.0003f, new List<Type>() { typeof(LaserWeapon), typeof(MissileWeapon) }, 
                "A basic, slow trading ship.");
            NeonCruiser = new Ship("Neon Cruiser", content.Load<Texture2D>("Images/Ships/NeonCruiser"),
                120, 25, 15, 0.1f, 0.03f, 0.0004f, new List<Type>() { typeof(LaserWeapon), typeof(LaserWeapon), typeof(BombWeapon) },
                "A quick starcraft with cryothermal thusters.");
            Dumpster = new Ship("Dumpster", content.Load<Texture2D>("Images/Ships/Dumpster"),
                40, 8, 3, 0.01f, 0.015f, 0.0001f, new List<Type>() { typeof(LaserWeapon) },
                "A small, slow, rusty ship that's dirt cheap on the black market.");
            Viridian = new Ship("Viridian", content.Load<Texture2D>("Images/Ships/Viridian"),
                140, 35, 6, 0.04f, 0.025f, 0.0002f, new List<Type>() { typeof(MissileWeapon), typeof(MissileWeapon), typeof(BombWeapon) },
                "A slower ship specializing in explosives.");
            Eos = new Ship("Eos", content.Load<Texture2D>("Images/Ships/Eos"),
                80, 25, 10, 0.04f, 0.03f, 0.0003f, new List<Type>() { typeof(BombWeapon), typeof(BombWeapon), typeof(BombWeapon) },
                "An impractical ship that is easily spotted and only utilizes bomb weapons.");

            MainGame.eventLogger.Log(typeof(Ship), "Finished loading ships");
        }
    }
}
