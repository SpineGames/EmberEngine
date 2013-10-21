using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EmberEngine.UI
{
    /// <summary>
    /// Base class for UI elements
    /// </summary>
    public abstract class UIElement
    {
        private Vector2 position; //private acessor for Position

        /// <summary>
        /// Gets the size of this UI element
        /// </summary>
        public abstract Vector2 Size { get; }
        /// <summary>
        /// Gets or sets the position of this UI element
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        /// <summary>
        /// Gets the bottom-right corner of this UI element
        /// </summary>
        public Vector2 Max { get { return Position + Size; } }
        /// <summary>
        /// Gets the bounds of this UI element
        /// </summary>
        public Rectangle Rect
        {
            get
            {
                return new Rectangle
                (
                    (int)Position.X,
                    (int)Position.Y,
                    (int)Max.X,
                    (int)Max.Y
                );
            }
        }

        /// <summary>
        /// Gets or sets this UIElement's meta-tag
        /// </summary>
        public object Tag;

        /// <summary>
        /// Called when this UI element should render
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw with</param>
        public abstract void Render(SpriteBatch spriteBatch);

        /// <summary>
        /// Called when this UI element should update
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public abstract void Update(GameTime gameTime);
    }
}
