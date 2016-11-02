using Microsoft.Xna.Framework.Input;

namespace BPA_RPG
{
    public class InputManager
    {
        public KeyboardState newKeyState { get; private set; }
        public KeyboardState oldKeyState { get; private set; }
        public MouseState newMouseState { get; private set; }
        public MouseState oldMouseState { get; private set; }

        public InputManager()
        {
            oldKeyState = Keyboard.GetState();
            oldMouseState = Mouse.GetState();
        }

        public void Begin()
        {
            newKeyState = Keyboard.GetState();
            newMouseState = Mouse.GetState();
        }

        public void End()
        {
            oldKeyState = newKeyState;
            oldMouseState = newMouseState;
        }
    }
}
