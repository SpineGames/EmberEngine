using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EmberEngine.Tools.Management;

namespace EmberEngine.SpineLibrary.UI
{
    /// <summary>
    /// A UI element that renders a string
    /// </summary>
    class UIString : UIElement
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
        public UIString(SpriteFont font, string Text, Color color)
        {
            this.Tag = Text;
            this.font = font;
            this.Color = color;
        }

        /// <summary>
        /// Creates a new UI string
        /// </summary>
        /// <param name="font">The name of the font in the FontManager to render with</param>
        /// <param name="Text">The initial text for this string</param>
        /// <param name="color">The color of this text</param>
        public UIString(string fontName, string Text, Color color)
        {
            this.Tag = Text;
            this.Color = color;

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
        /// snub method
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public override void Update(GameTime gameTime) 
        {
        }
    }
}
