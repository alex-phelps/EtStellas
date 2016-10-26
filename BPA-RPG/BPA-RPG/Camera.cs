using Microsoft.Xna.Framework;

namespace BPA_RPG
{
    public class Camera
    {
        public Matrix transform;

        public void Update(Vector2 position)
        {
            Update(position, MainGame.WindowCenter);
        }

        public void Update(Vector2 position, Vector2 center)
        {
            center = position - center;
            transform = Matrix.CreateScale(new Vector3(1, 1, 0)) *
                Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0));

        }
    }
}