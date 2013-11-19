using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EmberEngine.GUI
{
    /// <summary>
    /// Represents a panel that can draw a 'view' of it's components
    /// </summary>
    public class GUIScrollPanel : GUIPanel
    {
        int hScroll;
        int vScroll;

        int panelWidth;
        /// <summary>
        /// Gets or sets the panel width
        /// </summary>
        public int PanelWidth
        {
            get { return panelWidth; }
            set
            {
                panelWidth = value;
                panelWidth = panelWidth < Width ? Width : panelWidth;
                ShouldInvalidate = true;
            }
        }

        int panelHeight;
        /// <summary>
        /// Gets or sets the panel height
        /// </summary>
        public int PanelHeight
        {
            get { return panelHeight; }
            set
            {
                panelHeight = value;
                panelHeight = panelHeight < Height ? Height : panelHeight;
                ShouldInvalidate = true;
            }
        }

        int scrollWidth = 12;
        /// <summary>
        /// Gets or sets the width/height of the scroll bars
        /// </summary>
        public int ScrollWidth
        {
            get { return scrollWidth; }
            set
            {
                scrollWidth = value;
            }
        }

        /// <summary>
        /// Gets or sets the scroll speed for this panel
        /// </summary>
        public int ScrollSpeed = 2;

        Rectangle ScrollLeft;
        Rectangle ScrollRight;
        Rectangle ScrollUp;
        Rectangle ScrollDown;
        Rectangle HorizontalBack;
        Rectangle VerticalBack;

        public GUIScrollPanel(Rectangle Bounds, int PanelWidth, int PanelHeight)
            : base(Bounds)
        {
            this.PanelWidth = PanelWidth;
            this.PanelHeight = PanelHeight;
        }

        /// <summary>
        /// Rebuilds all of this scroll panel's scroll button bounds
        /// </summary>
        public void RebuildScrollRects()
        {
            ScrollLeft = new Rectangle(ScreenRect.X,
                    ScreenRect.Y + ScreenRect.Height - ScrollWidth, ScrollWidth, ScrollWidth);

            ScrollRight = new Rectangle( ScreenRect.X + ScreenRect.Width - ScrollWidth,
                    ScreenRect.Y + ScreenRect.Height - ScrollWidth, ScrollWidth, ScrollWidth);

            ScrollUp = new Rectangle(ScreenRect.X + ScreenRect.Width - ScrollWidth,
                    ScreenRect.Y, ScrollWidth , ScrollWidth);

            ScrollDown = new Rectangle(ScreenRect.X + ScreenRect.Width - ScrollWidth,
                    ScreenRect.Y + ScreenRect.Height - (ScrollWidth * 2), ScrollWidth, ScrollWidth);

            HorizontalBack = new Rectangle(ScreenRect.X + ScrollWidth,
                    ScreenRect.Y + ScreenRect.Height - ScrollWidth,
                    ScreenRect.Width - (ScrollWidth * 2), ScrollWidth);

            VerticalBack = new Rectangle(ScreenRect.X + ScreenRect.Width - ScrollWidth,
                    ScreenRect.Y + ScrollWidth, ScrollWidth, 
                    ScreenRect.Height - (ScrollWidth * 3));
        }

        /// <summary>
        /// Updates this scroll panel
        /// </summary>
        /// <param name="args">The updating arguments to use</param>
        public override void Update(GUIUpdateEventArgs args)
        {
            base.Update(args);

            MouseState mouse = Mouse.GetState();
            Point MousePos = new Point(mouse.X, mouse.Y);

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (ScrollLeft.Contains(MousePos))
                    hScroll -= ScrollSpeed;
                if (ScrollRight.Contains(MousePos))
                    hScroll += ScrollSpeed;

                if (ScrollUp.Contains(MousePos))
                    vScroll -= ScrollSpeed;
                if (ScrollDown.Contains(MousePos))
                    vScroll += ScrollSpeed;

                hScroll = hScroll < 0 ?
                    0 : hScroll;
                hScroll = hScroll > panelWidth - Width ?
                    panelWidth - Width : hScroll;

                vScroll = vScroll < 0 ?
                    0 : vScroll;
                vScroll = vScroll > PanelHeight - Height ?
                    PanelHeight - Height : vScroll;
            }
        }

        /// <summary>
        /// Called when this panel should bew re-drawn
        /// </summary>
        /// <param name="SpriteBatch"></param>
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

            DrawComponents(new GUIDrawEventArgs(SpriteBatch));
            
            RebuildScrollRects();

            base.EndInvalidate(SpriteBatch);
        }

        /// <summary>
        /// Overrides the draw method to draw this scroll panel
        /// </summary>
        /// <param name="args">The drawing args to use</param>
        public override void Draw(GUIDrawEventArgs args)
        {
            if (RenderTarget != null)
            {
                args.SpriteBatch.Begin();
                Rectangle screen = new Rectangle(
                    ScreenRect.X, ScreenRect.Y,
                    PanelWidth > Width ? ScreenRect.Width - ScrollWidth : Width,
                    PanelHeight > Height ? ScreenRect.Height - ScrollWidth : Height);

                args.SpriteBatch.Draw(RenderTarget, screen,
                    new Rectangle(hScroll, vScroll, Width, Height), Color.White);

                if (panelWidth > Width)
                {
                    args.SpriteBatch.Draw(GUI.Style.ScrollLeft, ScrollLeft, Color.White);

                    args.SpriteBatch.Draw(GUI.Style.ScrollRight, ScrollRight, Color.White);

                    args.SpriteBatch.Draw(GUI.Style.ScrollBarBackHorizontal, HorizontalBack,
                        Color.White);
                }

                if (panelHeight > Height)
                {
                    args.SpriteBatch.Draw(GUI.Style.ScrollUp, ScrollUp, Color.White);

                    args.SpriteBatch.Draw(GUI.Style.ScrollDown, ScrollDown, Color.White);

                    args.SpriteBatch.Draw(GUI.Style.ScrollBarBackVertical, VerticalBack,
                        Color.White);
                }

                args.SpriteBatch.End();
            }
        }
    }
}
