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
    public class BasicShader : Shader
    {
        /// <summary>
        /// Gets the base texture name ofr this shader
        /// </summary>
        public override string TexName 
        {
            get { return "Texture"; }        
        }

        #region Transform
        /// <summary>
        /// The world transform on this shader
        /// </summary>
        public Matrix WorldViewProj
        {
            get { return base.GetParameterMatrix("WorldViewProj"); }
            set
            {
                base.SetParameterMatrix("WorldViewProj", value);
            }
        }
        #endregion
        
        /// <summary>
        /// The texture to render with
        /// </summary>
        public Texture2D Texture
        {
            get { return base.GetParameterTexture("Texture"); }
            set { base.SetParameterTexture("Texture", value); }
        }
        
        /// <summary>
        /// Creates a new standardeffect
        /// </summary>
        /// <param name="BaseEffect">The Effect to use</param>
        public BasicShader(BasicEffect BaseEffect) : base((Effect)BaseEffect)
        {        }

        /// <summary>
        /// Creates a clone of this shader
        /// </summary>
        /// <returns>An exact clone of this shader</returns>
        public override Shader Clone()
        {
            BasicShader temp = new BasicShader((BasicEffect)BaseEffect.Clone());
            
            return temp;
        }
    }
}
