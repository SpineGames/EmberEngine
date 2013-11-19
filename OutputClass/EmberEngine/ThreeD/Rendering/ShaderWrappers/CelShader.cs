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
    /// A wrapper around the cell shader
    /// </summary>
    public class CelShader : Shader
    {
        /// <summary>
        /// Gets the base texture name ofr this shader
        /// </summary>
        public override string TexName
        {
            get { return "Texture"; }
        }

        #region Lightning
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

        #region Toon
        /// <summary>
        /// Gets or sets this shader's line color
        /// </summary>
        public Color LineColor
        {
            get { return base.GetParameterColor("LineColor"); }
            set
            {
                base.SetParameterColor("LineColor", value);
            }
        }
        /// <summary>
        /// Gets or sets the shaders line thickness
        /// </summary>
        public float LineThickness
        {
            get { return base.GetParameterFloat("LineThickness"); }
            set
            {
                base.SetParameterFloat("LineThickness", value);
            }
        }
        /// <summary>
        /// Gets or sets the intensity of toon shading (1 is max)
        /// </summary>
        public float ToonLevel
        {
            get { return base.GetParameterFloat("ToonLevel"); }
            set
            {
                base.SetParameterFloat("ToonLevel", value);
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
                base.SetParameterMatrix("WorldInverseTranspose", Matrix.Transpose(Matrix.Invert(value)));
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
        public Texture2D Texture
        {
            get { return base.GetParameterTexture("Texture"); }
            set { base.SetParameterTexture("Texture", value); }
        }
        
        /// <summary>
        /// Creates a new standardeffect
        /// </summary>
        /// <param name="BaseEffect">The Effect to use</param>
        public CelShader(Effect BaseEffect)
            : base(BaseEffect)
        {        
            BaseEffect.CurrentTechnique = BaseEffect.Techniques["Toon"];        
        }

        /// <summary>
        /// Creates a clone of this shader
        /// </summary>
        /// <returns>An exact clone of this shader</returns>
        public override Shader Clone()
        {
            CelShader temp = new CelShader(BaseEffect.Clone());
            
            return temp;
        }
    }
}
