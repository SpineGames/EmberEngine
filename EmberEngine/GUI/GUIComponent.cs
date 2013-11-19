using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EmberEngine.GUI
{
    /// <summary>
    /// Represents a component in a GUI
    /// </summary>
    public abstract class GUIComponent
    {
        #region Position
        int left;
        /// <summary>
        /// Gets or sets the left boundry of the element
        /// </summary>
        public int Left
        {
            get { return left; }
            set
            {
                int width = Width;
                left = value;
                right = left + width;
                screenRect = GetScreenRect();
                localRect = GetLocalRect();
            }
        }
        int right;
        /// <summary>
        /// gets or sets the right edge of the container
        /// </summary>
        public int Right
        {
            get { return right; }
            set
            {
                right = value;
                screenRect = GetScreenRect();
                localRect = GetLocalRect();
            }
        }
        /// <summary>
        /// Gets or sets the width of this container
        /// </summary>
        public int Width
        {
            get { return right - left; }
            set
            {
                right = left + value;
                ShouldInvalidate = true;
            }
        }

        int top;
        /// <summary>
        /// Gets or sets the top bounds of this container
        /// </summary>
        public int Top
        {
            get { return top; }
            set
            {
                int height = Height;
                top = value;
                bottom = top + height;
                screenRect = GetScreenRect();
                localRect = GetLocalRect();
            }
        }
        int bottom;
        /// <summary>
        /// Gets or sets the bottom of this panel
        /// </summary>
        public int Bottom
        {
            get { return bottom; }
            set
            {
                bottom = value;
                screenRect = GetScreenRect();
                localRect = GetLocalRect();
            }
        }
        /// <summary>
        /// Gets or sets the height of this container
        /// </summary>
        public int Height
        {
            get { return bottom - top; }
            set
            {
                bottom = top + value;
                ShouldInvalidate = true;
            }
        }

        private Rectangle localRect;
        /// <summary>
        /// Gets the local rectangle
        /// </summary>
        public Rectangle LocalRect
        {
            get { return localRect; }
        }
        /// <summary>
        /// Gets the local rectangle
        /// </summary>
        /// <returns></returns>
        private Rectangle GetLocalRect()
        {
            return new Rectangle(Left, Top, Width, Height);
        }

        private Rectangle screenRect;
        /// <summary>
        /// Gets this component's screen rectangle
        /// </summary>
        public Rectangle ScreenRect
        {
            get { return screenRect; }
        }
        /// <summary>
        /// Gets the rectangle to draw to
        /// </summary>
        /// <returns></returns>
        private Rectangle GetScreenRect()
        {
            if (Container == null)
                return new Rectangle(Left, Top, Width, Height);
            else
            {
                return new Rectangle(Container.Left + Left, Container.Top + Top,
                    Width, Height);
            }
        }
        #endregion

        #region Vars
        private bool shouldInvalidate = true;
        /// <summary>
        /// Gets or sets whether or not this object should invalidate
        /// on the next draw call
        /// </summary>
        public bool ShouldInvalidate
        {
            get { return shouldInvalidate; }
            set { shouldInvalidate = value; }
        }

        private readonly string name;
        /// <summary>
        /// Gets the name of this component
        /// </summary>
        public string Name
        {
            get { return name; }
        }
        /// <summary>
        /// Gets or sets this element's container
        /// </summary>
        public GUIContainer Container { get; set; }

        /// <summary>
        /// Gets or sets the render target for this component
        /// </summary>
        public RenderTarget2D RenderTarget
        {
            get;
            set;
        }
        #endregion

        public GUIComponent(Rectangle Bounds)
        {
            Width = Bounds.Width;
            Height = Bounds.Height;
            Left = Bounds.X;
            Top = Bounds.Y;
        }

        /// <summary>
        /// Handles updating this component
        /// </summary>
        /// <param name="args">The UpdateEventArgs to use</param>
        public virtual void Update(GUIUpdateEventArgs args)
        {
            if (ShouldInvalidate)
                Invalidate(args.SpriteBatch);
        }

        /// <summary>
        /// Begins an invalidation
        /// </summary>
        /// <param name="SpriteBatch">The spritebatch to draw with</param>
        public void BeginInvalidate(SpriteBatch SpriteBatch)
        {
            BeginInvalidate(SpriteBatch, Color.Transparent, Width, Height);
        }

        /// <summary>
        /// Begins an invalidation
        /// </summary>
        /// <param name="SpriteBatch">The spritebatch to draw with</param>
        /// <param name="BackColor">The background color to refresh with</param>
        public void BeginInvalidate(SpriteBatch SpriteBatch, Color BackColor)
        {
            BeginInvalidate(SpriteBatch, BackColor, Width, Height);
        }

        /// <summary>
        /// Begins an invalidation
        /// </summary>
        /// <param name="SpriteBatch">The spritebatch to draw with</param>
        /// <param name="BackColor">The background color to refresh with</param>
        /// <param name="Width">The width of the render target</param>
        /// <param name="Height">The height of the render target</param>
        public void BeginInvalidate(SpriteBatch SpriteBatch, Color BackColor, int Width, int Height)
        {
            if (RenderTarget != null)
                RenderTarget.Dispose();
            RenderTarget = new RenderTarget2D(SpriteBatch.GraphicsDevice,
                Width, Height);

            SpriteBatch.GraphicsDevice.SetRenderTarget(RenderTarget);
            SpriteBatch.GraphicsDevice.Clear(BackColor);

        }

        /// <summary>
        /// Called when this component should refresh it's visual aspects
        /// </summary>
        /// <param name="SpriteBatch">The spritebatch to draw elements with</param>
        public abstract void Invalidate(SpriteBatch SpriteBatch);

        /// <summary>
        /// Called when this component is ending it's invalidation
        /// </summary>
        /// <param name="SpriteBatch">The spritebatch to draw elements with</param>
        public void EndInvalidate(SpriteBatch SpriteBatch)
        {
            SpriteBatch.GraphicsDevice.SetRenderTarget(null);

            ShouldInvalidate = false;
        }

        /// <summary>
        /// Draws this component to the spritebatch
        /// </summary>
        /// <param name="args">The DrawEventArgs to use</param>
        public virtual void Draw(GUIDrawEventArgs args)
        {
            if (RenderTarget != null)
            {
                args.SpriteBatch.Begin();
                args.SpriteBatch.Draw(RenderTarget, localRect, Color.White);
                args.SpriteBatch.End();
            }
        }
    }
}
