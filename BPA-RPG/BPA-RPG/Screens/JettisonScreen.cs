using EtStellas.GameItems;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EtStellas.GameObjects;
using Microsoft.Xna.Framework.Audio;

namespace EtStellas.Screens
{
    /// <summary>
    /// Screen for player item disposal
    /// </summary>
    public class JettisonScreen : Screen
    {
        //Hold variables
        private List<GameItem> hold => PlayerData.inventory;
        private int firstRenderH;
        private List<GameObject> itemRectsH;
        private ClickableObject scrollArrowTopH;
        private ClickableObject scrollArrowBotH;

        //Jettison variables
        private List<GameItem> jettison;
        private int firstRenderJ;
        private List<GameObject> itemRectsJ;
        private ClickableObject scrollArrowTopJ;
        private ClickableObject scrollArrowBotJ;

        private int holdSize => PlayerData.ship.holdSize;
        private bool ready => hold.Count <= holdSize;

        private SpriteFont font;
        private SpriteFont titleFont;
        private Texture2D menu;
        private Texture2D holdScrollArrowReg;
        private Texture2D holdScrollArrowBlue;
        private Texture2D partInv;

        private ClickableObject okButton;

        private Background background;

        private SoundEffectInstance select;
        private SoundEffectInstance error;

        /// <summary>
        /// Creates a new JettisonScreen
        /// </summary>
        public JettisonScreen()
            : base("Jettison")
        {
            translucent = true;

            jettison = new List<GameItem>();
            itemRectsH = new List<GameObject>();
            itemRectsJ = new List<GameObject>();
        }

        public override void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("Fonts/ChoiceFont");
            titleFont = content.Load<SpriteFont>("Fonts/InfoFont");
            menu = content.Load<Texture2D>("Images/JettisonScreen");
            holdScrollArrowReg = content.Load<Texture2D>("Images/HoldScrollArrow");
            holdScrollArrowBlue = content.Load<Texture2D>("Images/HoldScrollArrowBlue");
            partInv = content.Load<Texture2D>("Images/PartInv");

            //Hold inv  setup
            scrollArrowTopH = new ClickableObject(holdScrollArrowReg,
                () => firstRenderH--,
                () => scrollArrowTopH.texture = holdScrollArrowBlue,
                () => scrollArrowTopH.texture = holdScrollArrowReg)
            {
                position = new Vector2(369, 185)
            };

            scrollArrowBotH = new ClickableObject(holdScrollArrowReg,
                () => firstRenderH++,
                () => scrollArrowBotH.texture = holdScrollArrowBlue,
                () => scrollArrowBotH.texture = holdScrollArrowReg)
            {
                position = new Vector2(369, 365),
                rotation = MathHelper.Pi
            };

            Texture2D itemHoverRect = content.Load<Texture2D>("Images/ItemHoverRect");
            for (int i = 0; i < 5; i++)
            {
                int k = i; // keep lambda from referencing i
                itemRectsH.Add(new ClickableObject(itemHoverRect,
                    () =>
                    {
                        if (k < hold.Count - firstRenderH)
                        {
                            jettison.Add(hold[k + firstRenderH]);
                            hold.RemoveAt(k + firstRenderH);
                        }
                    },
                    () => itemRectsH[k].visible = true,
                    () => itemRectsH[k].visible = false)
                {
                    position = new Vector2(369, 215 + (k * 30)),
                    visible = false
                });
            }


            //Jettison inv setup
            scrollArrowTopJ = new ClickableObject(holdScrollArrowReg,
                () => firstRenderJ--,
                () => scrollArrowTopJ.texture = holdScrollArrowBlue,
                () => scrollArrowTopJ.texture = holdScrollArrowReg)
            {
                position = new Vector2(655, 185)
            };

            scrollArrowBotJ = new ClickableObject(holdScrollArrowReg,
                () => firstRenderJ++,
                () => scrollArrowBotJ.texture = holdScrollArrowBlue,
                () => scrollArrowBotJ.texture = holdScrollArrowReg)
            {
                position = new Vector2(655, 365),
                rotation = MathHelper.Pi
            };
            
            for (int i = 0; i < 5; i++)
            {
                int k = i; // keep lambda from referencing i
                itemRectsJ.Add(new ClickableObject(itemHoverRect,
                    () =>
                    {
                        if (k < jettison.Count - firstRenderJ)
                        {
                            hold.Add(jettison[k + firstRenderJ]);
                            jettison.RemoveAt(k + firstRenderJ);
                        }
                    },
                    () => itemRectsJ[k].visible = true,
                    () => itemRectsJ[k].visible = false)
                {
                    position = new Vector2(655, 215 + (k * 30)),
                    visible = false
                });
            }

            okButton = new ClickableObject(content.Load<Texture2D>("Images/InfoBoxButton"), () =>
            {
                if (ready)
                {
                    select.Play();
                    manager.Push(new InfoBoxScreen("Jettison", "Cargo specified successfully\njettisoned into space.", () => manager.Pop()));
                }
                else error.Play();
            })
            {
                position = new Vector2(512, 418)
            };

            background = new Background(Color.Black * .6f);

            //Sounds
            select = SoundEffectManager.GetEffectInstance("Select1");
            error = SoundEffectManager.GetEffectInstance("Hazard1");

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            //Update rectangles
            foreach (ClickableObject rect in itemRectsH)
                rect.Update(gameTime);
            foreach (ClickableObject rect in itemRectsJ)
                rect.Update(gameTime);

            //Update arrows
            scrollArrowTopH.Update(gameTime);
            scrollArrowBotH.Update(gameTime);
            scrollArrowTopJ.Update(gameTime);
            scrollArrowBotJ.Update(gameTime);

            // Check Mouse scroll input
            if (InputManager.newMouseState.ScrollWheelValue > InputManager.oldMouseState.ScrollWheelValue) // scroll up
                if (InputManager.newMouseState.Position.X < MainGame.WindowCenter.X)
                    firstRenderH--;
                else firstRenderJ--;
            else if (InputManager.newMouseState.ScrollWheelValue < InputManager.oldMouseState.ScrollWheelValue) //scroll down
                if (InputManager.newMouseState.Position.X < MainGame.WindowCenter.X)
                    firstRenderH++;
                else firstRenderJ++;

            //Make sure firstRender's are within bounds
            if (firstRenderH > hold.Count - 5)
                firstRenderH = hold.Count - 5;
            if (firstRenderH < 0)
                firstRenderH = 0;
            if (firstRenderJ > jettison.Count - 5)
                firstRenderJ = jettison.Count - 5;
            if (firstRenderJ < 0)
                firstRenderJ = 0;

            okButton.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            //Draw background
            background.Draw(gameTime, spritebatch);

            //Draw w/o linear interpolation
            spritebatch.End();
            spritebatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            //Draw menu
            spritebatch.Draw(menu, MainGame.WindowCenter, new Rectangle(0, 0, menu.Width, menu.Height), Color.White,
                0, new Vector2(menu.Width / 2, menu.Height / 2), 1, SpriteEffects.None, 1);

            //Draw rectangles
            for (int i = 0; i < itemRectsH.Count; i++)
            {
                if (firstRenderH + i >= hold.Count)
                    break;

                itemRectsH[i].Draw(gameTime, spritebatch);
            }

            for (int i = 0; i < itemRectsJ.Count; i++)
            {
                if (firstRenderJ + i >= jettison.Count)
                    break;

                itemRectsJ[i].Draw(gameTime, spritebatch);
            }

            //Draw items
            for (int i = 0; i < 5; i++)
            {
                int k = i + firstRenderH;

                if (k >= hold.Count)
                    break;

                spritebatch.Draw(hold[k].texture, new Vector2(282, 205 + (i * 30)), Color.White);
                spritebatch.DrawString(font, hold[k].name, new Vector2(322, 205 + (i * 30)), Color.White);
            }

            for (int i = 0; i < 5; i++)
            {
                int k = i + firstRenderJ;

                if (k >= jettison.Count)
                    break;

                spritebatch.Draw(jettison[k].texture, new Vector2(568, 205 + (i * 30)), Color.White);
                spritebatch.DrawString(font, jettison[k].name, new Vector2(608, 205 + (i * 30)), Color.White);
            }

            //Draw arrows
            scrollArrowTopH.Draw(gameTime, spritebatch);
            scrollArrowBotH.Draw(gameTime, spritebatch);
            scrollArrowTopJ.Draw(gameTime, spritebatch);
            scrollArrowBotJ.Draw(gameTime, spritebatch);

            //Draw ok button
            okButton.Draw(gameTime, spritebatch, ready ? Color.White : Color.Gray);
            string text = ready ? "OK" : "Hold Full!";
            spritebatch.DrawString(font, text, okButton.position - font.MeasureString(text) / 2, ready ? Color.White : Color.Gray);

            //Draw title text
            text = hold.Count + "/" + holdSize;
            spritebatch.DrawString(titleFont, text, new Vector2(512, 145) - titleFont.MeasureString(text) / 2, ready ? Color.Lime : Color.Red);
            spritebatch.DrawString(titleFont, "Hold", new Vector2(239, 145 - titleFont.MeasureString("Hold").Y / 2), Color.White);
            spritebatch.DrawString(titleFont, "Jettison", new Vector2(785 - titleFont.MeasureString("Jettison").X, 145 - titleFont.MeasureString("Jettison").Y / 2), Color.White);

            //Draw item infoboxes
            for (int i = 0; i < 5; i++)
            {
                //for hold
                if (itemRectsH[i].visible && !(i + firstRenderH >= hold.Count))
                    hold[i + firstRenderH].DrawInfo(spritebatch);
                //for jettison
                if (itemRectsJ[i].visible && !(i + firstRenderJ >= jettison.Count))
                    jettison[i + firstRenderJ].DrawInfo(spritebatch);
            }

            //restart spritebatch
            spritebatch.End();
            spritebatch.Begin();

            base.Draw(gameTime, spritebatch);
        }
    }
}
