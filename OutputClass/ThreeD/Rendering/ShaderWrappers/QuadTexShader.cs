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
    public class QuadTexShader : Shader
    {
        /// <summary>
        /// Gets the base texture name ofr this shader
        /// </summary>
        public override string TexName
        {
            get { return "Texture0"; }
        }

        #region Lightning
        /// <summary>
        /// Gets or sets this shader's ambient color
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
        /// Gets or sets the intensity of this shader's ambient lighting
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
        /// Gets or sets this shader's diffuse direction
        /// </summary>
        public Vector3 DiffuseLightDirection
        {
            get { return base.GetParameterVector3("DiffuseLightDirection"); }
            set
            {
                base.SetParameterVector3("DiffuseLightDirection", value);
            }
        }
        /// <summary>
        /// Gets or sets this shader's diffuse color
        /// </summary>
        public Color DiffuseColor
        {
            get { return base.GetParameterColor("DiffuseColor"); }
            set
            {
                base.SetParameterColor("DiffuseColor", value);
            }
        }
        /// <summary>
        /// Gets or sets the intensity of this shader's diffuse lighting
        /// </summary>
        public float DiffuseIntensity
        {
            get { return base.GetParameterFloat("DiffuseIntensity"); }
            set
            {
                base.SetParameterFloat("DiffuseIntensity", value);
            }
        }
        #endregion

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
            get { return base.GetParameterTexture("Texture0"); }
            set { base.SetParameterTexture("Texture0", value); }
        }
        /// <summary>
        /// The texture to render with
        /// </summary>
        public Texture2D Texture1
        {
            get { return base.GetParameterTexture("Texture1"); }
            set { base.SetParameterTexture("Texture1", value); }
        }
        /// <summary>
        /// The texture to render with
        /// </summary>
        public Texture2D Texture2
        {
            get { return base.GetParameterTexture("Texture2"); }
            set { base.SetParameterTexture("Texture2", value); }
        }
        /// <summary>
        /// The texture to render with
        /// </summary>
        public Texture2D Texture3
        {
            get { return base.GetParameterTexture("Texture3"); }
            set { base.SetParameterTexture("Texture3", value); }
        }
        
        /// <summary>
        /// Creates a new standardeffect
        /// </summary>
        /// <param name="BaseEffect">The Effect to use</param>
        public QuadTexShader(Effect BaseEffect)
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
            QuadTexShader temp = new QuadTexShader(BaseEffect.Clone());
            
            return temp;
        }
    }
}
