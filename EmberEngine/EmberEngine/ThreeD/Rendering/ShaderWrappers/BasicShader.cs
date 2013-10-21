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

        #region DefaultLighting
        /// <summary>
        /// The color of the ambient lighting
        /// </summary>
        public Color AmbientColor
        {
            get { return base.GetParameterColor("AmbientColor"); }
            set
            {
                base.SetParameterColor("AmbientColor", value);
            }
        }

        /// <summary>
        /// The intensity of the ambient lighting
        /// </summary>
        public float AmbientIntensity
        {
            get { return base.GetParameterFloat("AmbientIntensity"); }
            set
            {
                base.SetParameterFloat("AmbientIntensity", value);
            }
        }

        /// <summary>
        /// Represents the normal for the diffuse light
        /// </summary>
        public Vector3 DiffuseDirection
        {
            get { return BaseEffect.Parameters["DiffuseLightDirection"].GetValueVector3(); }
            set { BaseEffect.Parameters["DiffuseLightDirection"].SetValue(value); }
        }

        /// <summary>
        /// The color for the diffuse light
        /// </summary>
        public Vector4 DiffuseColor
        {
            get { return BaseEffect.Parameters["DiffuseColor"].GetValueVector4(); }
            set { BaseEffect.Parameters["DiffuseColor"].SetValue(value); }
        }

        /// <summary>
        /// The color for the diffuse light
        /// </summary>
        public float DiffuseIntensity
        {
            get { return BaseEffect.Parameters["DiffuseIntensity"].GetValueSingle(); }
            set { BaseEffect.Parameters["DiffuseIntensity"].SetValue(value); }
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
