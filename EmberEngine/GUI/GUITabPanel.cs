using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EmberEngine.GUI
{
    /// <summary>
    /// Represents a simple conatiner for GUI elements
    /// </summary>
    public class GUITabPanel : GUIContainer
    {
        Texture2D texture;
        /// <summary>
        /// The texture to apply to this panel
        /// </summary>
        public Texture2D Texture
        {
            get { return texture; }
            set
            {
                texture = value;
                ShouldInvalidate = true;
            }
        }
        Texture2D mask;

        /// <summary>
        /// The mask for this panel
        /// </summary>
        public Texture2D Mask
        {
            get { return mask; }
            set
            {
                mask = value;
                ShouldInvalidate = true;
            }
        }
        Color color = Color.White;

        /// <summary>
        /// Gets or sets whether this tabbed panel is expanded
        /// </summary>
        public bool Expanded { get; set; }

        /// <summary>
        /// Creates a new GUI panel
        /// </summary>
        /// <param name="Bounds">The bounds of the panel</param>
        public GUITabPanel(Rectangle Bounds) : base(Bounds) { }

        /// <summary>
        /// Called when this panel should be re-drawn
        /// </summary>
        /// <param name="SpriteBatch">The spritebatch to draw to</param>
        public override void Invalidate(SpriteBatch SpriteBatch)
        {
            base.BeginInvalidate(SpriteBatch);

            if (Expanded)
            {
                if (mask != null & texture != null)
                {
                    SpriteBatch.Begin();
                    SpriteBatch.Draw(mask, new Rectangle(0, 0, Width, Height), color);
                    SpriteBatch.End();

                    SpriteBatch.Begin(SpriteSortMode.BackToFront, GUI.Multiply, SamplerState.LinearWrap,
                        DepthStencilState.Default, RasterizerState.CullCounterClockwise);
                    SpriteBatch.Draw(texture, new Rectangle(0, 0, Width, Height), color);
                    SpriteBatch.End();
                }
                else if (texture != null)
                {
                    SpriteBatch.Begin();
                    SpriteBatch.Draw(texture, ScreenRect, color);
                    SpriteBatch.End();
                }
            }
            else
            {

            }

            base.EndInvalidate(SpriteBatch);
        }
    }
}
