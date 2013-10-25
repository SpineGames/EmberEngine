using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using EmberEngine.ThreeD.Tools;
using Microsoft.Xna.Framework;
using EmberEngine.ThreeD.Rendering;

namespace EmberEngine.ThreeD.Tools
{
    /// <summary>
    /// Class for making vertices with very little code
    /// stands for Fast Vertices
    /// </summary>
    public static class FV
    {
        /// <summary>
        /// Creates a new VertexPositionColorNormaltexture
        /// </summary>
        /// <param name="X">The vertices's X co-ord</param>
        /// <param name="Y">The vertices's Y co-ord</param>
        /// <param name="Z">The vertices's Z co-ord</param>
        /// <param name="nX">The normal's X component</param>
        /// <param name="nY">The normal's Y component</param>
        /// <param name="nZ">The normal's Y component</param>
        /// <param name="tX">The texture co-ords X component</param>
        /// <param name="tY">The texture co-ords Y component</param>
        /// <param name="color">The color of the vertice</param>
        /// <returns>A new VertexPositionColorNormaltexture with the given values</returns>
        public static VertexPositionNormalTextureColor VPCNT
            (float X, float Y, float Z, float nX, float nY, float nZ, float tX, float tY, 
            Color color)
        {
            return new VertexPositionNormalTextureColor(new Vector3(X, Y, Z), new Vector3(nX, nY, nZ), new Vector2(tX, tY), color);
        }

        /// <summary>
        /// Creates a new VertexPositionColorNormaltexture
        /// </summary>
        /// <param name="X">The vertices's X co-ord</param>
        /// <param name="Y">The vertices's Y co-ord</param>
        /// <param name="Z">The vertices's Z co-ord</param>
        /// <param name="Normal">The vertice's normal</param>
        /// <param name="TexCoords">The texture co-ordinates of the vertice</param>
        /// <param name="color">The color of the vertice</param>
        /// <returns>A new VertexPositionColorNormaltexture with the given values</returns>
        public static VertexPositionNormalTextureColor VPCNT
            (float X, float Y, float Z, Vector3 Normal, Vector2 TexCoords,
            Color color)
        {
            return new VertexPositionNormalTextureColor(new Vector3(X, Y, Z), Normal, TexCoords, color);
        }

        /// <summary>
        /// Creates a new VertexPositionColorNormaltexture
        /// </summary>
        /// <param name="Position">The vertice's position</param>
        /// <param name="nX">The normal's X component</param>
        /// <param name="nY">The normal's Y component</param>
        /// <param name="nZ">The normal's Y component</param>
        /// <param name="tX">The texture co-ords X component</param>
        /// <param name="tY">The texture co-ords Y component</param>
        /// <param name="color">The color of the vertice</param>
        /// <returns>A new VertexPositionColorNormaltexture with the given values</returns>
        public static VertexPositionNormalTextureColor VPCNT
            (Vector3 Position, float nX, float nY, float nZ, float tX, float tY,
            Color color)
        {
            return new VertexPositionNormalTextureColor(Position, new Vector3(nX, nY, nZ), new Vector2(tX, tY), color);
        }

        /// <summary>
        /// Creates a new VertexPositionColorNormaltexture
        /// </summary>
        /// <param name="Position">The vertice's position</param>
        /// <param name="Normal">The vertice's normal</param>
        /// <param name="tX">The texture co-ords X component</param>
        /// <param name="tY">The texture co-ords Y component</param>
        /// <param name="color">The color of the vertice</param>
        /// <returns>A new VertexPositionColorNormaltexture with the given values</returns>
        public static VertexPositionNormalTextureColor VPCNT
            (Vector3 Position, Vector3 Normal, float tX, float tY,
            Color color)
        {
            return new VertexPositionNormalTextureColor(Position, Normal, new Vector2(tX, tY), color);
        }

        /// <summary>
        /// Creates a new VertexPositionColorNormaltexture
        /// </summary>
        /// <param name="Position">The vertice's position</param>
        /// <param name="Normal">The vertice's normal</param>
        /// <param name="TexCoords">The texture co-ordinates of the vertice</param>
        /// <param name="color">The color of the vertice</param>
        /// <returns>A new VertexPositionColorNormaltexture with the given values</returns>
        public static VertexPositionNormalTextureColor VPCNT
            (Vector3 Position, Vector3 Normal, Vector2 TexCoords,
            Color color)
        {
            return new VertexPositionNormalTextureColor(Position, Normal, TexCoords, color);
        }
    }
}
