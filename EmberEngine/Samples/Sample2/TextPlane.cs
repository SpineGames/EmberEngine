using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmberEngine.ThreeD.Instancing;
using Microsoft.Xna.Framework;
using EmberEngine.ThreeD.Tools;
using EmberEngine.Tools.AdvancedMath;
using EmberEngine.ThreeD.Rendering;
using EmberEngine.ThreeD.Rendering.ShaderWrappers;
using Microsoft.Xna.Framework.Graphics;

namespace Samples.Sample2
{
    /// <summary>
    /// Represents a plane that has text on it
    /// </summary>
    class TextPlane : Instance
    {
        /// <summary>
        /// Creates a new Text plane
        /// </summary>
        /// <param name="Min">The minimum position</param>
        /// <param name="Max">The maximum position</param>
        /// <param name="stringRenderer">The StringRenderer to apply</param>
        /// <param name="shader">The shader to use</param>
        /// <param name="texTile">The number of times the texture is tiled</param>
        public TextPlane(Vector3 Min, Vector3 Max, GraphicsDevice graphics, string text, 
            SpriteBatch spriteBatch, SpriteFont spriteFont, Shader shader, int texTile = 1)
            : base(Min + ((Max - Min) / 2))
        {

            texTile = texTile < 1 ? 1: texTile;

            VertexPositionNormalTextureColor[] verts = new VertexPositionNormalTextureColor[4];
            int[] indices = new int[6];
            
            Vector3 TL = new Vector3(Min.X, Max.Y, Max.Z);
            Vector3 BR = new Vector3(Max.X, Min.Y, Min.Z);
           
            //TL, BR, Min
            //v0 - v1, v1 - v2
            Vector3 N = Vector3.Cross(TL - BR, BR - Min);
            N.Normalize();

            verts[0] = FV.VPCNT(TL, N, new Vector2(0, 0), Color.White);
            verts[1] = FV.VPCNT(Max, N, new Vector2(texTile, 0), Color.White);
            verts[2] = FV.VPCNT(BR, N, new Vector2(texTile, texTile), Color.White);
            verts[3] = FV.VPCNT(Min, N, new Vector2(0, texTile), Color.White);

            indices[0] = 0;
            indices[1] = 2;
            indices[2] = 3;

            indices[3] = 0;
            indices[4] = 1;
            indices[5] = 2;

            PolyRender_VPNTC render = new PolyRender_VPNTC(shader);
            render.Texture = StringRender.RenderToTarget(graphics, spriteBatch, text, spriteFont, Color.Black, Color.White);
            render.AddPolys(verts);
            render.FinalizePolys(indices);
            this.Renderer = render;
        }
    }
}
