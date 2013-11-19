using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EmberEngine.GUI
{
    /// <summary>
    /// A static class for GUI tools
    /// </summary>
    public static class GUIUtils
    {
        /// <summary>
        /// Returns what <i>text</i> should be in order to fit to a certain width
        /// </summary>
        /// <param name="spriteFont">The spritefont to use</param>
        /// <param name="text">The text to fit</param>
        /// <param name="maxLineWidth">The maximum line width</param>
        /// <returns><i>text</i> as it should be written to stay within <i>maxLineWidth</i></returns>
        public static string WrapText(SpriteFont spriteFont, string text, float maxLineWidth)
        {
            if (text != null)
            {
                string[] words = text.Split(' ');

                StringBuilder sb = new StringBuilder();

                float lineWidth = 0f;

                float spaceWidth = spriteFont.MeasureString(" ").X;

                foreach (string word in words)
                {
                    string checker = word.Replace("\n", "");
                    Vector2 size = spriteFont.MeasureString(checker);

                    if (lineWidth + size.X < maxLineWidth)
                    {
                        sb.Append(word + " ");
                        lineWidth += size.X + spaceWidth;
                    }
                    else
                    {
                        sb.Append("\n" + word + " ");
                        lineWidth = size.X + spaceWidth;
                    }
                }

                return sb.ToString();
            }
            else
                return "";
        }

        /// <summary>
        /// Fixes a font so that it is spaced out by the given amount
        /// </summary>
        /// <param name="SpriteFont">The spritefont to fix</param>
        /// <param name="spacing">The pixel spacing between lines</param>
        public static SpriteFont FixFontSpacing(SpriteFont SpriteFont, float spacing)
        {
            SpriteFont.LineSpacing = (int)((SpriteFont.MeasureString(" ").Y / 2) + spacing);
            return SpriteFont;
        }
    }

    /// <summary>
    /// Represents an align mode for GUI elements
    /// </summary>
    public class AlignMode
    {
        public VerticalAlignMode VAlign;
        public HorizontalAlignMode HAlign;
    }

    /// <summary>
    /// Represents a docking mode for GUI panels
    /// </summary>
    public class DockMode
    {
        public AlignMode AlignMode;
        public DockType DockType;
    }

    /// <summary>
    /// Represents a docking type for a GUIContainer
    /// </summary>
    public enum DockType
    {
        Horizontal,
        Vertical
    }

    /// <summary>
    /// Represents a vertical align mode for a GUI element
    /// </summary>
    public enum VerticalAlignMode
    {
        Top,
        Middle,
        Bottom
    }

    /// <summary>
    /// Represents a horizontal align mode for a GUI element
    /// </summary>
    public enum HorizontalAlignMode
    {
        Left,
        Middle,
        Right
    }

    /// <summary>
    /// An event args for when a GUI element needs to update
    /// </summary>
    public class GUIUpdateEventArgs
    {
        /// <summary>
        /// The spritebatch to invalidate with
        /// </summary>
        public readonly SpriteBatch SpriteBatch;
        /// <summary>
        /// The current game time
        /// </summary>
        public readonly GameTime GameTime;

        /// <summary>
        /// Creates a new GUI update event args
        /// </summary>
        /// <param name="SpriteBatch">The spritebatch to invalidate with</param>
        /// <param name="GameTime">The current game time</param>
        public GUIUpdateEventArgs(SpriteBatch SpriteBatch, GameTime GameTime)
        {
            this.SpriteBatch = SpriteBatch;
            this.GameTime = GameTime;
        }
    }

    /// <summary>
    /// Represents the arguments for drawing a GUI component
    /// </summary>
    public class GUIDrawEventArgs
    {
        /// <summary>
        /// The spritebatch to draw with
        /// </summary>
        public readonly SpriteBatch SpriteBatch;

        /// <summary>
        /// Creates a new GUI draw event
        /// </summary>
        /// <param name="SpriteBatch">The spritebatch to draw with</param>
        public GUIDrawEventArgs(SpriteBatch SpriteBatch)
        {
            this.SpriteBatch = SpriteBatch;
        }
    }
}
