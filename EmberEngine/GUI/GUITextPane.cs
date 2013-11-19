using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EmberEngine.GUI
{
    public class GUITextPane : GUIScrollPanel
    {
        private string text;
        private string drawnText;
        /// <summary>
        /// Gets or sets the text for this text pane
        /// </summary>
        public string Text
        {
            get { return text; }
            set
            {
                text = value.Replace("\n", "");
                drawnText = GUIUtils.WrapText(font, text, Width - ScrollWidth);
                this.PanelHeight = (int)font.MeasureString(drawnText).Y;
            }
        }

        SpriteFont font;
        /// <summary>
        /// Gets or sets the font for this text pane
        /// </summary>
        public SpriteFont Font
        {
            get { return font; }
            set
            {
                font = value;
                drawnText = GUIUtils.WrapText(font, text, Width);
                this.PanelHeight = (int)font.MeasureString(drawnText).Y;
                ShouldInvalidate = true;
            }
        }
                
        Color textColor = Color.Black;
        /// <summary>
        /// Gets or sets the color for the text in this panel
        /// </summary>
        public Color TextColor
        {
            get { return textColor; }
            set
            {
                textColor = value;
                ShouldInvalidate = true;
            }
        }

        /// <summary>
        /// Creates a new GUI text panel
        /// </summary>
        /// <param name="Bounds">The bounds of the panel in it's container</param>
        /// <param name="font">The font to draw with</param>
        public GUITextPane(Rectangle Bounds, SpriteFont font)
            : base(Bounds, Bounds.Width, Bounds.Height)
        {
            this.Font = font;
        }

        /// <summary>
        /// Called when this Scroll panel should re-draw itself
        /// </summary>
        /// <param name="SpriteBatch">The spritebatch to draw with</param>
        public override void Invalidate(SpriteBatch SpriteBatch)
        {
            base.BeginInvalidate(SpriteBatch, Color.Transparent, PanelWidth, PanelHeight);

            if (Mask != null & Texture != null)
            {
                SpriteBatch.Begin();
                SpriteBatch.Draw(Mask, new Rectangle(0, 0, PanelWidth, PanelHeight), Color.White);
                SpriteBatch.End();

                SpriteBatch.Begin(SpriteSortMode.BackToFront, GUI.Multiply, SamplerState.LinearWrap,
                    DepthStencilState.Default, RasterizerState.CullCounterClockwise);
                SpriteBatch.Draw(Texture, new Rectangle(0, 0, PanelWidth, PanelHeight), Color.White);
                SpriteBatch.End();
            }
            else if (Texture != null)
            {
                SpriteBatch.Begin();
                SpriteBatch.Draw(Texture, new Rectangle(0, 0, PanelWidth, PanelHeight), Color.White);
                SpriteBatch.End();
            }

            SpriteBatch.Begin();
            SpriteBatch.DrawString(Font, drawnText, new Vector2(0), TextColor);
            SpriteBatch.End();

            RebuildScrollRects();

            base.EndInvalidate(SpriteBatch);
        }
    }
}
