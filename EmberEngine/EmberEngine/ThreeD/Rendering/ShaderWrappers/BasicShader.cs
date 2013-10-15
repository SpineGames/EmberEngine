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
    public class BasicShader
    {
        /// <summary>
        /// Gets a dictionary of all Effect Parameters
        /// </summary>
        public Dictionary<string, EffectParameter> Parameters = new Dictionary<string, EffectParameter>();

        /// <summary>
        /// The Effect to render with
        /// </summary>
        public readonly Effect BaseEffect;

        #region Transform
        Matrix world;
        public Matrix World
        {
            get { return world; }
            set
            {
                world = value;
                BaseEffect.Parameters["World"].SetValue(world);
            }
        }
        Matrix view;
        public Matrix View
        {
            get { return view; }
            set
            {
                view = value;
                BaseEffect.Parameters["View"].SetValue(view);
            }
        }
        Matrix projection;
        public Matrix Projection
        {
            get { return projection; }
            set
            {
                projection = value;
                BaseEffect.Parameters["Projection"].SetValue(projection);
            }
        }
        #endregion

        #region DefaultLighting
        /// <summary>
        /// The color of the ambient lighting
        /// </summary>
        public Vector4 AmbientLightColor
        {
            get { return BaseEffect.Parameters["AmbientColor"].GetValueVector4(); }
            set { BaseEffect.Parameters["AmbientColor"].SetValue(value); }
        }

        /// <summary>
        /// The intensity for the ambient lighting. Default is 0.1
        /// </summary>
        public float AmbientLightIntensity
        {
            get { return BaseEffect.Parameters["AmbientIntensity"].GetValueSingle(); }
            set { BaseEffect.Parameters["AmbientIntensity"].SetValue(value); }
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
            get { return BaseEffect.Parameters["Texture"].GetValueTexture2D(); }
            set { BaseEffect.Parameters["Texture"].SetValue(value); }
        }
        
        /// <summary>
        /// Creates a new standardeffect
        /// </summary>
        /// <param name="BaseEffect">The Effect to use</param>
        public BasicShader(Effect BaseEffect)
        {
            this.BaseEffect = BaseEffect;

            foreach (EffectParameter param in BaseEffect.Parameters)
            {
                this.Parameters.Add(param.Name, param);
            }
        }

        /// <summary>
        /// Creates a clone of this shader
        /// </summary>
        /// <returns>An exact clone of this shader</returns>
        public BasicShader Clone()
        {
            BasicShader temp = new BasicShader(BaseEffect.Clone());
            
            return temp;
        }
    }
}
