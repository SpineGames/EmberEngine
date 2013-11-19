using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EmberEngine.Tools.Management;

namespace EmberEngine.UI
{
    /// <summary>
    /// A UI element that renders a string that is clickable
    /// </summary>
    public class UISClickableString : UIClickable
    {
        SpriteFont font;
        Color Color;

        /// <summary>
        /// Gets the size of this string
        /// </summary>
        public override Vector2 Size
        {
            get
            {
                if (font != null)
                    return font.MeasureString(Tag.ToString());
                else
                    return new Vector2(0);
            }
        }
        
        /// <summary>
        /// Creates a new UI string
        /// </summary>
        /// <param name="font">The spritefont to render with</param>
        /// <param name="Text">The initial text for this string</param>
        /// <param name="color">The color of this text</param>
        public UISClickableString(SpriteFont font, string Text, Color color)
            : base(new Rectangle(0, 0, (int)font.MeasureString(Text).X, (int)font.MeasureString(Text).Y))
        {
            this.Tag = Text;
            this.font = font;
            this.Color = color;
            TagChanged += TagUpdated;
        }

        /// <summary>
        /// Creates a new UI string
        /// </summary>
        /// <param name="font">The name of the font in the FontManager to render with</param>
        /// <param name="Text">The initial text for this string</param>
        /// <param name="color">The color of this text</param>
        public UISClickableString(string fontName, string Text, Color color)
            : base(new Rectangle(0, 0, 
                (int)FontManager.Fonts[fontName].MeasureString(Text).X,
                (int)FontManager.Fonts[fontName].MeasureString(Text).Y))
        {
            this.Tag = Text;
            this.Color = color;
            TagChanged += TagUpdated;

            if (FontManager.Fonts.ContainsKey(fontName))
                this.font = FontManager.Fonts[fontName];
            else
                throw new ArgumentException("FontManager does not have font '" + fontName + "' loaded"); 
        }

        /// <summary>
        /// Called when this string should be rendered
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, Tag.ToString(), Position, Color);
        }

        /// <summary>
        /// Called when the meta tag is updated
        /// </summary>
        /// <param name="sender">The object that raied the event (this)</param>
        /// <param name="e">The blank event args</param>
        private void TagUpdated(object sender, EventArgs e)
        {
            this.SetSize(font.MeasureString(Tag.ToString()));
        }
    }
}
