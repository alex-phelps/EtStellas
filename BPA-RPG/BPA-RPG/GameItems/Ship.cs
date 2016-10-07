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
        public static Ship StarterShip;


        public readonly float maxSpeed;
        public readonly float accel;
        public readonly float maxRotSpeed;
        public readonly float rotAccel;

        public Ship(string name, Texture2D texture, float maxSpeed, float accel, float maxRotSpeed, float rotAccel) 
            : base(name, texture)
        {
            this.maxSpeed = maxSpeed;
            this.accel = accel;
            this.maxRotSpeed = maxRotSpeed;
            this.rotAccel = rotAccel;
        }
    }
}
