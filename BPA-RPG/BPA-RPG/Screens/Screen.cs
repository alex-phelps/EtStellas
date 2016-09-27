using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace BPA_RPG.Screens
{
    /// <summary>
    /// A screen to be added to a screen manager. 
    /// </summary>
    public class Screen
    {
        /// <summary>
        /// Whether or not this screen covers the entire game window.
        /// </summary>
        public bool translucent { get; set; }

        /// <summary>
        /// A reference to the ScreenManager.
        /// </summary>
        public ScreenManager manager { get; set; }
        public bool loaded { get; private set; }

        public Screen()
        {
            translucent = false;
        }

        /// <summary>
        /// This method is called when this screen is back at the top of the stack.
        /// </summary>
        public virtual void Activated()
        {
        }

        /// <summary>
        /// This method is called when a screen is deactivated by an other screen.
        /// </summary>
        public virtual void Deactivated()
        {
        }

        /// <summary>
        /// Loads every component of this screen.
        /// </summary>
        public virtual void LoadContent(ContentManager content)
        {
            loaded = true;
            MainGame.eventLogger.Log(this, "Loaded");
        }

        /// <summary>
        /// Updates every component of this screen.
        /// </summary>
        public virtual void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Draw every visible component of this screen.
        /// </summary>
        public virtual void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
        }
    }
}