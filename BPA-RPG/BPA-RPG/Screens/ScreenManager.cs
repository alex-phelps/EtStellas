
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BPA_RPG.Screens
{
    /// <summary>
    /// A collection/stack of active screens in the game.
    /// </summary>
    public class ScreenManager
    {
        public bool loaded { get; private set; }

        private Stack<Screen> screens;

        private ContentManager content;

        public ScreenManager(ContentManager content)
        {
            this.content = content;
            this.screens = new Stack<Screen>();
            this.loaded = false;
        }

        /// <summary>
        /// Loads all screens in the active screen list.
        /// </summary>
        public void LoadContent()
        {
            foreach (Screen s in screens)
                s.LoadContent(content);
            loaded = true;

            MainGame.eventLogger.Log(this, "Loaded");
        }

        /// <summary>
        /// Only update the top of the screen stack.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            this.screens.Peek().Update(gameTime);
        }

        /// <summary>
        /// Draw all visible screens. A screen that is not translucent will stop the iteration of screens.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            List<Screen> visible = new List<Screen>();
            foreach (Screen s in screens)
            {
                visible.Add(s);
                if (!s.translucent)
                    break;
            }

            // Draw from back to front:
            for (int i = visible.Count - 1; i >= 0; i--)
                visible[i].Draw(gameTime, spritebatch);
        }

        /// <summary>
        /// Add a screen to the top of the stack, if the manager is initialized but the screen isn't initialize it.
        /// Also set it's manager to this.
        /// </summary>
        /// <param name="screen">The screen to add.</param>
        public void Push(Screen screen)
        {
            screen.manager = this;
            if (!screen.loaded && loaded)
                screen.LoadContent(content);
            Peek()?.Deactivated();
            screens.Push(screen);
        }

        /// <summary>
        /// Get the top of the screen stack. The most active screen.
        /// </summary>
        /// <returns>The active screen.</returns>
        public Screen Peek()
        {
            if (screens.Count < 1)
                return null;
            return screens.Peek();
        }

        /// <summary>
        /// Remove a screen from the screen stack.
        /// </summary>
        /// <returns>The removed screen.</returns>
        public Screen Pop()
        {
            if (screens.Count < 1)
                return null;
            Screen prev = screens.Pop();
            prev.Deactivated();

            Peek()?.Activated();
            return prev;
        }
    }
}