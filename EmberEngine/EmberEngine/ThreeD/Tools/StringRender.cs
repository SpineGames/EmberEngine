using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EmberEngine.ThreeD.Tools
{
    /// <summary>
    /// Renders a spritespriteBatch string to a texture
    /// </summary>
    public static class StringRender
    {
        /// <summary>
        /// Renders the string to the render target
        /// </summary>
        /// <param name="spriteBatch">The spritespriteBatch to draw with</param>
        /// <param name="text">The text to render</param>
        public static Texture2D RenderToTarget(GraphicsDevice graphics, SpriteBatch spriteBatch, string text, SpriteFont font, Color TextColor, Color BackColor)
        {
            Vector2 size = font.MeasureString(text);
            RenderTarget2D renderTarget = new RenderTarget2D(graphics, (int)size.X, (int)size.Y);

            graphics.SetRenderTarget(renderTarget);
            graphics.Clear(BackColor);

            spriteBatch.Begin();
            spriteBatch.DrawString(font, text, new Vector2(0, 0), TextColor);
            spriteBatch.End();

            graphics.SetRenderTarget(null);

            return (Texture2D)renderTarget;
        }
    }
}
