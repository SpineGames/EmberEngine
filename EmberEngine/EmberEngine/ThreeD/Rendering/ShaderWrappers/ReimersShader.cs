using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.IO;
using System.Reflection;

namespace EmberEngine.ThreeD.Rendering.ShaderWrappers
{
    /// <summary>
    /// A wrapper around the standard shader
    /// </summary>
    public class ReimersShader : Shader
    {        
        #region Transform
        /// <summary>
        /// Gets or sets this shaders world transform
        /// </summary>
        public Matrix World
        {
            get { return base.GetParameterMatrix("World"); }
            set
            {
                base.SetParameterMatrix("World", value);
            }
        }

        /// <summary>
        /// gets or sets this shaders view transformation
        /// </summary>
        public Matrix View
        {
            get { return base.GetParameterMatrix("View"); }
            set
            {
                base.SetParameterMatrix("View", value);
            }
        }

        /// <summary>
        /// Gets or sets this shaders projection transformation
        /// </summary>
        public Matrix Projection
        {
            get { return base.GetParameterMatrix("Projection"); }
            set
            {
                base.SetParameterMatrix("Projection", value);
            }
        }
        #endregion

        /// <summary>
        /// The texture to render with
        /// </summary>
        public Texture2D Texture0
        {
            get { return base.GetParameterTexture("xTexture0"); }
            set { base.SetParameterTexture("xTexture0", value); }
        }
        /// <summary>
        /// The texture to render with
        /// </summary>
        public Texture2D Texture1
        {
            get { return base.GetParameterTexture("xTexture1"); }
            set { base.SetParameterTexture("xTexture1", value); }
        }
        /// <summary>
        /// The texture to render with
        /// </summary>
        public Texture2D Texture2
        {
            get { return base.GetParameterTexture("xTexture2"); }
            set { base.SetParameterTexture("xTexture2", value); }
        }
        /// <summary>
        /// The texture to render with
        /// </summary>
        public Texture2D Texture3
        {
            get { return base.GetParameterTexture("xTexture3"); }
            set { base.SetParameterTexture("xTexture3", value); }
        }
        
        /// <summary>
        /// Creates a new standardeffect
        /// </summary>
        /// <param name="BaseEffect">The Effect to use</param>
        public ReimersShader(Effect BaseEffect)
            : base(BaseEffect)
        {        
            BaseEffect.CurrentTechnique = BaseEffect.Techniques["MultiTextured"];        
        }

        /// <summary>
        /// Creates a clone of this shader
        /// </summary>
        /// <returns>An exact clone of this shader</returns>
        public override Shader Clone()
        {
            ReimersShader temp = new ReimersShader(BaseEffect.Clone());
            
            return temp;
        }
    }

    /// <summary>
    /// Used to declare a multitextured vertex
    /// </summary>
    public struct VertexMultitextured : IVertexType
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TextureCoordinate;
        public Vector4 TexWeights;

        public VertexMultitextured(Vector3 Position, Vector3 Normal, Vector2 TexCoords, Vector4 TexWeights)
        {
            this.Position = Position;
            this.Normal = Normal;
            this.TextureCoordinate = TexCoords;
            this.TexWeights = TexWeights;
        }

        public static int SizeInBytes = (3 + 3 + 2 + 4) * sizeof(float);

        public VertexDeclaration VertexDeclaration
        {
            get
            {
                return new VertexDeclaration(
                    new VertexElement[]
                    {
                        new VertexElement( 0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0 ),
                        new VertexElement( 0, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0 ),
                        new VertexElement( 0, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 0 ),
                        new VertexElement( 0, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 1 ),
                    }
                );
            }

        }
    }
}
