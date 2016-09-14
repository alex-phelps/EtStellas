using Microsoft.Xna.Framework;

namespace BPA_RPG
{
    public class Camera
    {
        public Matrix transform;
        Vector2 center;

        public void Update(Vector2 position)
        {
            center = new Vector2(position.X - MainGame.WindowWidth / 2, position.Y - MainGame.WindowHeight / 2);
            transform = Matrix.CreateScale(new Vector3(1, 1, 0)) *
                Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0));
        }
    }
}