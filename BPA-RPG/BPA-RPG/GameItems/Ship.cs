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
        public static Ship StarterShip { get; private set; }


        public readonly int holdSize;
        public readonly float maxSpeed;
        public readonly float accel;
        public readonly float maxRotSpeed;
        public readonly float rotAccel;

        public Ship(string name, Texture2D texture, int holdSize, float maxSpeed, float accel, float maxRotSpeed, float rotAccel) 
            : base(name, texture)
        {
            this.holdSize = holdSize;
            this.maxSpeed = maxSpeed;
            this.accel = accel;
            this.maxRotSpeed = maxRotSpeed;
            this.rotAccel = rotAccel;
        }

        public static new void LoadContent(ContentManager content)
        {
            MainGame.eventLogger.Log(typeof(Ship), "Begin loading static ships");

            //Define static ships
            StarterShip = new Ship("Starter Ship", content.Load<Texture2D>("Images/StarterShip"), 20, 7f, 0.05f, 0.025f, 0.0005f);

            MainGame.eventLogger.Log(typeof(Ship), "Finished loading static ships");
        }
    }
}
