using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EmberEngine.Tools.Management;
using Microsoft.Xna.Framework.Input;

namespace EmberEngine.UI
{
    /// <summary>
    /// A UI element that renders a string
    /// </summary>
    abstract class UIClickable : UIElement
    {
        bool clicked = false;

        Rectangle clickableArea;
        /// <summary>
        /// Gets the clickable area for this clickable object
        /// </summary>
        public Rectangle ClickableArea
        {
            get { return clickableArea; }
            set { clickableArea = value; }
        }

        /// <summary>
        /// Gets or sets the event raised on click
        /// </summary>
        public OnClickHandler OnClick;

        /// <summary>
        /// Gets the size of this string
        /// </summary>
        public override Vector2 Size
        {
            get
            {
                return new Vector2(clickableArea.Width, clickableArea.Height);
            }
        }
        
        /// <summary>
        /// Creates a new UI clickable
        /// </summary>
        /// <param name="ClickableArea">The clickable area for this clickable object</param>
        public UIClickable(Rectangle ClickableArea)
        {
            this.clickableArea = ClickableArea;
        }

        /// <summary>
        /// Sets the size of the clickable rectangle
        /// </summary>
        /// <param name="size">The new size of the rectangle</param>
        public void SetSize(Vector2 size)
        {
            clickableArea.Width = (int)size.X;
            clickableArea.Height = (int)size.Y;
        }

        /// <summary>
        /// Handles updating this UI element
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public override void Update(GameTime gameTime)
        {
            clickableArea.X = (int)Position.X;
            clickableArea.Y = (int)Position.Y;

            MouseState m;
            try
            {
                m = Mouse.GetState();
            }
            catch (Exception)
            {
                m = new MouseState(0, 0, 0, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
            }

            if (m.LeftButton == ButtonState.Pressed)
            {
                if (clickableArea.Contains(new Point(m.X, m.Y)) & !clicked)
                {
                    clicked = true;

                    if (OnClick != null)
                        OnClick.Invoke(Tag);
                }
            }
            else
            {
                if (clicked)
                    clicked = false;
            }
        }
    }

    /// <summary>
    /// Represents an event handler for on clicked
    /// </summary>
    /// <param name="tag">The clickable object's tag</param>
    public delegate void OnClickHandler(object tag);
}
