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
        public static Ship StarterShip;



        public string name { get; private set; }
        public Texture2D texture { get; private set; }
        public float maxSpeed { get; private set; }
        public float accel { get; private set; }
        public float maxRotSpeed { get; private set; }
        public float rotAccel { get; private set; }

        public Ship(string name, Texture2D texture, float maxSpeed, float accel, float maxRotSpeed, float rotAccel) 
            : base()
        {
            this.name = name;
            this.texture = texture;
            this.maxSpeed = maxSpeed;
            this.accel = accel;
            this.maxRotSpeed = maxRotSpeed;
            this.rotAccel = rotAccel;
        }
    }
}
