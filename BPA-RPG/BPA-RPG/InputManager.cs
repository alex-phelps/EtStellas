using Microsoft.Xna.Framework.Input;

namespace BPA_RPG
{
    public static class InputManager
    {
        public static KeyboardState newKeyState { get; private set; }
        public static KeyboardState oldKeyState { get; private set; }
        public static MouseState newMouseState { get; private set; }
        public static MouseState oldMouseState { get; private set; }

        static InputManager()
        {
            oldKeyState = Keyboard.GetState();
            oldMouseState = Mouse.GetState();
        }

        public static void Begin()
        {
            newKeyState = Keyboard.GetState();
            newMouseState = Mouse.GetState();
        }

        public static void End()
        {
            oldKeyState = newKeyState;
            oldMouseState = newMouseState;
        }
    }
}
