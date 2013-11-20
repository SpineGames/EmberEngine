using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EmberEngine.UI
{
    /// <summary>
    /// A UI element that renders a string
    /// </summary>
    public class UISpacer : UIElement
    {
        int height;
        /// <summary>
        /// Gets or sets the height of this spacer
        /// </summary>
        public int Height
        {
            get { return height; }
            set { height = value >= 0 ? value : 0; }
        }
        
        /// <summary>
        /// Gets the size of this string
        /// </summary>
        public override Vector2 Size
        {
            get
            {
                return new Vector2(0, height);
            }
        }

        /// <summary>
        /// Creates a new UI string
        /// </summary>
        /// <param name="font">The spritefont to render with</param>
        /// <param name="Text">The initial text for this string</param>
        /// <param name="color">The color of this text</param>
        public UISpacer(int height)
        {
            this.height = height;
        }

        /// <summary>
        /// Called when this string should be rendered
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public override void Render(SpriteBatch spriteBatch)
        {
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
