using BPA_RPG.GameItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BPA_RPG.GameObjects
{
    /// <summary>
    /// Represents some object in the game
    /// </summary>
    public class GameObject
    {
        private Texture2D Texture;
        public Texture2D texture
        {
            get
            {
                return Texture;
            }

            set
            {
                //Set all values dependant on the texture

                Texture = value;
                width = value.Width;
                height = value.Height;
                source = new Rectangle(0, 0, width, height);

                colorData = new Color[width * height];
                value.GetData(colorData);
            }

        }

        public Vector2 position;
        public Rectangle source;
        public float rotation = 0;
        public float scale = 1;
        public bool visible = true;

        public int width { get; private set; }
        public int height { get; private set; }

        /// <summary>
        /// Color data for the pixels of this objects image
        /// </summary>
        public Color[] colorData { get; protected set; }

        /// <summary>
        /// Matrix that represents all the transformations done on the object
        /// </summary>
        public Matrix transformMatrix
        {
            get
            {
                return
                Matrix.CreateTranslation(new Vector3(-width / 2, -height / 2, 0.0f)) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateScale(scale) *
                Matrix.CreateTranslation(new Vector3(position, 0.0f));
            }
        }

        /// <summary>
        /// Rectangle that represents bounds of the object
        /// </summary>
        public Rectangle boundingRectangle
        {
            get
            {
                Rectangle rectangle = new Rectangle(0, 0, width, height);
                Matrix transform = transformMatrix;

                //Get all four corners in local space
                Vector2 topLeft = new Vector2(rectangle.Left, rectangle.Top);
                Vector2 topRight = new Vector2(rectangle.Right, rectangle.Top);
                Vector2 botLeft = new Vector2(rectangle.Left, rectangle.Bottom);
                Vector2 botRight = new Vector2(rectangle.Right, rectangle.Bottom);

                //Transform corners into work space
                Vector2.Transform(ref topLeft, ref transform, out topLeft);
                Vector2.Transform(ref topRight, ref transform, out topRight);
                Vector2.Transform(ref botLeft, ref transform, out botLeft);
                Vector2.Transform(ref botRight, ref transform, out botRight);

                //Find minimum and maximum extents of the rectangle in world space
                Vector2 min = Vector2.Min(Vector2.Min(topLeft, topRight),
                    Vector2.Min(botLeft, botRight));
                Vector2 max = Vector2.Max(Vector2.Max(topLeft, topRight),
                    Vector2.Max(botLeft, botRight));

                return new Rectangle((int)min.X, (int)min.Y,
                    (int)(max.X - min.X), (int)(max.Y - min.Y));
            }
        }


        public GameObject(Texture2D texture)
        {
            this.texture = texture;
            position = new Vector2(0, 0);

            if (!(this is GameItem))
                MainGame.eventLogger.Log(this, "Loaded " + "\"" + texture.ToString() + "\"");
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gametime">Provides a snapshot of timing values.</param>
        public virtual void Update(GameTime gameTime)
        {
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="spritebatch">Spritebatch object to draw objects with</param>
        /// <param name="color">Color to draw in.</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spritebatch, Color color)
        {
            if (visible)
            {
                spritebatch.Draw(texture, position, source, color, rotation,
                    new Vector2(source.Width / 2, source.Height / 2), scale, SpriteEffects.None, 1);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="spritebatch">Spritebatch object to draw objects with</param>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            Draw(gameTime, spritebatch, Color.White);
        }

        /// <summary>
        /// Checks if this is colliding with the given GameObject
        /// </summary>
        public bool IntersectPixels(GameObject obj)
        {
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAToB = transformMatrix * Matrix.Invert(obj.transformMatrix);

            //For each row of pixel in A
            for (int yA = 0; yA < height; yA++)
            {
                //For each pixel in that row
                for (int xA = 0; xA < width; xA++)
                {
                    //Calculate this pixel's location in B
                    Vector2 positionInB = Vector2.Transform(new Vector2(xA, yA), transformAToB);

                    int xB = (int)Math.Round(positionInB.X);
                    int yB = (int)Math.Round(positionInB.Y);

                    if (xB >= 0 && xB < obj.width &&
                        yB >= 0 && yB < obj.height)
                    {
                        //Get colors of the overlapping pixels
                        Color colorA = colorData[xA + yA * width];
                        Color colorB = obj.colorData[xB + yB * obj.width];

                        //If both pixels are not completely transparent
                        if (colorA.A * colorB.A != 0)
                        {
                            //Intersection found
                            return true;
                        }
                    }
                }
            }

            //No intersection
            return false;
        }

        /// <summary>
        /// Scales the object to given width and height
        /// </summary>
        /// <param name="scale">Scale of area</param>
        public void ScaleTo(int width, int height, float scale = 1)
        {
            this.scale = source.Width >= source.Height
                ? (width * scale) / source.Width
                : (height * scale) / source.Height;
        }

        /// <summary>
        /// Scales object to a given rectangle
        /// </summary>
        /// <param name="scale">Scale of area</param>
        public void ScaleTo(Rectangle rect, float scale = 1)
        {
            ScaleTo(rect.Width, rect.Height, scale);
        }
    }
}