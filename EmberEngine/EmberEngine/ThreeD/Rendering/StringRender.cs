using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EmberEngine.ThreeD.Rendering
{
    /// <summary>
    /// Renders a spritebatch string to a texture
    /// </summary>
    public class StringRender
    {
        RenderTarget2D renderTarget;
        string text;
        SpriteFont font;
        Color backColor;
        Color textColor;
        Texture2D temp;
        /// <summary>
        /// Gets this object's texture
        /// </summary>
        public Texture2D Texture
        {
            get { return temp; }
        }

        /// <summary>
        /// Creates a new string render target
        /// </summary>
        /// <param name="graphics">The graphics device to render with</param>
        /// <param name="text">The text to render</param>
        /// <param name="font">The spritefont to use</param>
        public StringRender(GraphicsDevice graphics, string text, SpriteFont font, Color TextColor, Color BackColor)
        {
            this.font = font;
            this.text = text;
            this.textColor = TextColor;
            this.backColor = BackColor;

            Vector2 size = font.MeasureString(text);
            renderTarget = new RenderTarget2D(graphics, (int)size.X, (int)size.Y);
        }

        /// <summary>
        /// Renders the string to the render target
        /// </summary>
        /// <param name="batch">The spritebatch to draw with</param>
        public void RenderToTarget(SpriteBatch batch)
        {
            batch.GraphicsDevice.SetRenderTarget(renderTarget);

            batch.GraphicsDevice.Clear(backColor);
            batch.DrawString(font, text, new Vector2(0, 0), textColor);

            batch.GraphicsDevice.SetRenderTarget(null);

            temp = (Texture2D)renderTarget;
        }

        /// <summary>
        /// Renders the string to the render target
        /// </summary>
        /// <param name="batch">The spritebatch to draw with</param>
        /// <param name="text">The text to render</param>
        public void RenderToTarget(SpriteBatch batch, string text)
        {
            this.text = text;

            Vector2 size = font.MeasureString(text);
            renderTarget = new RenderTarget2D(renderTarget.GraphicsDevice, (int)size.X, (int)size.Y);

            batch.GraphicsDevice.SetRenderTarget(renderTarget);

            batch.GraphicsDevice.Clear(backColor);
            batch.DrawString(font, text, new Vector2(0, 0), textColor);

            batch.GraphicsDevice.SetRenderTarget(null);

            temp = (Texture2D)renderTarget;
        }

        /// <summary>
        /// Clears the render target for rendering
        /// </summary>
        private void PrepData()
        {
            Color[] t = new Color[renderTarget.Width * renderTarget.Height];

            for (int i = 0; i < renderTarget.Width * renderTarget.Height; i++)
                t[i] = backColor;

            renderTarget.SetData<Color>(t);
        }

        /// <summary>
        /// Casts this StringRender to a texture
        /// </summary>
        /// <param name="t">The StringRender to cast</param>
        /// <returns><i>t</i> cast to a Texture2D</returns>
        public static implicit operator Texture2D(StringRender t)
        {
            return t.Texture;
        }
    }
}
