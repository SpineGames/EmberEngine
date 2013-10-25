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
    public static class SpritebatchToTexture
    {
        static RenderTarget2D temp;

        /// <summary>
        /// Starts a new render
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        /// <param name="width">The width of the texture</param>
        /// <param name="height">The height of the texture</param>
        public static void StartRender(ref SpriteBatch spriteBatch, int width, int height)
        {
            temp = new RenderTarget2D(spriteBatch.GraphicsDevice, width, height);
            spriteBatch.GraphicsDevice.SetRenderTarget(temp);
            spriteBatch.GraphicsDevice.Clear(Color.White);
        }

        /// <summary>
        /// Ends the renderer and returns the texture
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        /// <returns></returns>
        public static Texture2D EndRender(ref SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.SetRenderTarget(null);
            return (Texture2D)temp;
        }
    }
}
