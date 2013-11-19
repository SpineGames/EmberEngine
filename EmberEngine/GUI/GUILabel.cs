using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using EmberEngine.Tools.Variables;

namespace EmberEngine.GUI
{
    /// <summary>
    /// Represents a component that draws text
    /// </summary>
    public class GUILabel: GUIComponent
    {
        #region Drawing Vars
        SpriteFont font;
        /// <summary>
        /// Gets or sets the font for this label
        /// </summary>
        public SpriteFont Font
        {
            get { return font; }
            set
            {
                font = value;
                Height = (int)font.MeasureString(text).Y;
                ShouldInvalidate = true;
            }
        }

        string text = "null";
        string DrawnText = "null";
        /// <summary>
        /// Gets or sets the text for this label
        /// </summary>
        public string Text
        {
            get { return text; }
            set
            {
                text = value.Replace("\n", "");
                DrawnText = GUIUtils.WrapText(Font, text, MaxWidth);
                ShouldInvalidate = true;

                if (AutoSize)
                {
                    Height = (int)Font.MeasureString(DrawnText).Y;
                }
            }
        }

        Color textColor = Color.White;
        /// <summary>
        /// Gets or sets the text color for this label
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

        Color backColor = Color.Transparent;
        /// <summary>
        /// Gets or sets the background color for this label
        /// <b>Default: </b> Color.Transparent
        /// </summary>
        public Color BackColor
        {
            get { return backColor; }
            set
            {
                backColor = value;
                ShouldInvalidate = true;
            }
        }
        #endregion

        #region Size Vars
        private bool autoSize = false;
        /// <summary>
        /// Gets or sets whether this component should resize itself to fit the text
        /// </summary>
        public bool AutoSize
        {
            get { return autoSize; }
            set
            {
                autoSize = value;
                ShouldInvalidate = true;
            }
        }

        private int maxwidth;
        /// <summary>
        /// Gets or sets the maximum width for this label
        /// </summary>
        public int MaxWidth
        {
            get { return maxwidth; }
            set
            {
                maxwidth = value;
                ShouldInvalidate = true;
            }
        }
        #endregion

        /// <summary>
        /// Creates a new GUI label
        /// </summary>
        /// <param name="Bounds">The initial bounds of the label</param>
        /// <param name="Font">The font to draw with</param>
        /// <param name="text">The initial text to draw</param>
        public GUILabel(Rectangle Bounds, SpriteFont Font, string text) : base(Bounds)
        {
            this.Height = (int)Font.MeasureString(text).Y;

            this.MaxWidth = Width;

            this.font = Font;
            this.Text = text;
        }

        /// <summary>
        /// Called when this label should be redrawn
        /// </summary>
        /// <param name="SpriteBatch">The spritebatch to draw with</param>
        public override void Invalidate(SpriteBatch SpriteBatch)
        {
            if (AutoSize)
            {
                Width = (int)Font.MeasureString(DrawnText).X;
                Height = (int)Font.MeasureString(DrawnText).Y;

                if (Width > MaxWidth)
                    Width = MaxWidth;
            }

            base.BeginInvalidate(SpriteBatch, BackColor);

            SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.Additive,
                    SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            SpriteBatch.DrawString(font, DrawnText, new Vector2(0, 0), TextColor);
            SpriteBatch.End();

            base.EndInvalidate(SpriteBatch);
        }
    }
}
