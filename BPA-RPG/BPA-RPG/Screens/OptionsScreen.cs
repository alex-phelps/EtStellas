using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using EtStellas.GameObjects;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace EtStellas.Screens
{
    public class OptionsScreen : Screen
    {
        private Background background;
        private Texture2D overlay;

        private SpriteFont font;
        private List<DrawableString> options;

        private SoundEffectInstance selectSound;

        public OptionsScreen()
            : base("Options")
        {
            translucent = true;
        }

        public override void LoadContent(ContentManager content)
        {
            background = new Background(Color.Black * 0.6f);
            overlay = content.Load<Texture2D>("Images/TitleScreen");

            font = content.Load<SpriteFont>("Fonts/TitleFont");
            options = new List<DrawableString>();

            //Back option
            options.Add(CreateOption("Back", 0, () => manager.Pop()));

            //Fullscreen option
            options.Add(CreateOption("Fullscreen", 1, () =>
            {
                MainGame.graphicsDeviceManager.IsFullScreen = !MainGame.graphicsDeviceManager.IsFullScreen;
                MainGame.graphicsDeviceManager.ApplyChanges();
            }));

            //Volume option
            options.Add(CreateOption("Volume", 2, () =>
            {
                float volume = SoundEffect.MasterVolume + 0.1f;
                if (volume > 1)
                    volume = 0;
                SoundEffect.MasterVolume = volume;

                volume = MediaPlayer.Volume + 0.1f;
                if (volume > 1)
                    volume = 0;
                MediaPlayer.Volume = volume;
            }));

            //Sounds
            selectSound = SoundEffectManager.GetEffectInstance("Select1");

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.newKeyState.IsKeyDown(Keys.Escape) && InputManager.oldKeyState.IsKeyUp(Keys.Escape))
                manager.Pop();

            foreach (DrawableString option in options)
                option.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            background.Draw(gameTime, spritebatch);
            spritebatch.Draw(overlay, Vector2.Zero, Color.White);

            foreach (DrawableString option in options)
                option.Draw(gameTime, spritebatch);

            base.Draw(gameTime, spritebatch);
        }

        private DrawableString CreateOption(string name, int index, Action onClick = null)
        {
            return new DrawableString(font, name, new Vector2(880, 280 + 60 * index) - font.MeasureString(name), Color.White,
                onClick + (() => selectSound.Play()),
                () =>
                {
                    options[index].text = name + "  ";
                    options[index].position.X = 880 - font.MeasureString(options[index].text).X;
                },
                () =>
                {
                    options[index].text = name;
                    options[index].position.X = 880 - font.MeasureString(options[index].text).X;
                });
        }
    }
}
