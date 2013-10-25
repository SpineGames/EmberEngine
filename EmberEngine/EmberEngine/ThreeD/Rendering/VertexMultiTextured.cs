using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EmberEngine.EmberEngine.ThreeD.Rendering
{
    /// <summary>
    /// Used to declare a multitextured vertex
    /// </summary>
    public struct VertexMultitextured : IVertexType
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector4 Color;
        public Vector2 TextureCoordinate;
        public Vector4 TexWeights;

        public VertexMultitextured(Vector3 Position, Vector3 Normal, Color color, Vector2 TexCoords, Vector4 TexWeights)
        {
            this.Position = Position;
            this.Normal = Normal;
            this.Color = color.ToVector4();
            this.TextureCoordinate = TexCoords;
            this.TexWeights = TexWeights;
        }

        public static int SizeInBytes = (3 + 3 + 4 + 2 + 4) * sizeof(float);

        public VertexDeclaration VertexDeclaration
        {
            get
            {
                return new VertexDeclaration(
                    new VertexElement[]
                    {
                        new VertexElement( 0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0 ),
                        new VertexElement( 3 * sizeof(float), VertexElementFormat.Vector3, VertexElementUsage.Normal, 0 ),
                        new VertexElement( 6 * sizeof(float), VertexElementFormat.Vector4, VertexElementUsage.Color, 0 ),
                        new VertexElement( 10 * sizeof(float), VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0 ),
                        new VertexElement( 12 * sizeof(float), VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 1 ),
                    }
                );
            }
        }
    }
}
