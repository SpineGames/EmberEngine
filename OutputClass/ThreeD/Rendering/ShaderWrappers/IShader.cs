using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EmberEngine.ThreeD.Rendering.ShaderWrappers
{
    public abstract class Shader
    {
        public abstract string TexName {get;}

        Dictionary<string, EffectParameter> Parameters = new Dictionary<string, EffectParameter>();

        Effect baseEffect;
        /// <summary>
        /// Gets this Shader's base effect
        /// </summary>
        public Effect BaseEffect
        {
            get { return baseEffect; }
        }

        /// <summary>
        /// Creates a new Shader
        /// </summary>
        /// <param name="BaseEffect">The Effect to wrap around</param>
        public Shader(Effect BaseEffect)
        {
            this.baseEffect = BaseEffect;

            foreach (EffectParameter p in baseEffect.Parameters)
            {
                Parameters.Add(p.Name, p);
            }
        }

        /// <summary>
        /// Creates a clone of this shader
        /// </summary>
        /// <returns>A copy of this shader</returns>
        public abstract Shader Clone();

        /// <summary>
        /// Applies this shader
        /// </summary>
        /// <param name="pass">The pass to apply <b>default: 0</b></param>
        public void Apply(int pass = 0)
        {
            BaseEffect.CurrentTechnique.Passes[0].Apply();
        }

        /// <summary>
        /// Gets a dictionary of all of this shader's parameters
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, EffectParameter> GetParameters()
        {
            return Parameters;
        }

        /// <summary>
        /// Checks if this shader has a given parameter
        /// </summary>
        /// <param name="name">The name of the parameter to search for</param>
        /// <returns>True if a parameter called <i>name</i> exists</returns>
        public bool HasParameter(string name)
        {
            return Parameters.ContainsKey(name);
        }

        #region Get Operators
        /// <summary>
        /// Gets the float value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to get</param>
        /// <returns>The float value of <i>name</i></returns>
        public float GetParameterFloat(string name)
        {
            if (HasParameter(name))
                return Parameters[name].GetValueSingle();
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Gets the int value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to get</param>
        /// <returns>The int value of <i>name</i></returns>
        public int GetParameterInt(string name)
        {
            if (HasParameter(name))
                return Parameters[name].GetValueInt32();
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Gets the bool value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to get</param>
        /// <returns>The bool value of <i>name</i></returns>
        public bool GetParameterBool(string name)
        {
            if (HasParameter(name))
                return Parameters[name].GetValueBoolean();
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Gets the Matrix value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to get</param>
        /// <returns>The Matrix value of <i>name</i></returns>
        public Matrix GetParameterMatrix(string name)
        {
            if (HasParameter(name))
                return Parameters[name].GetValueMatrix();
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Gets the Vector2 value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to get</param>
        /// <returns>The Vector2 value of <i>name</i></returns>
        public Vector2 GetParameterVector2(string name)
        {
            if (HasParameter(name))
                return Parameters[name].GetValueVector2();
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Gets the Vector3 value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to get</param>
        /// <returns>The Vector3 value of <i>name</i></returns>
        public Vector3 GetParameterVector3(string name)
        {
            if (HasParameter(name))
                return Parameters[name].GetValueVector3();
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Gets the Vector4 value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to get</param>
        /// <returns>The Vector4 value of <i>name</i></returns>
        public Vector4 GetParameterVector4(string name)
        {
            if (HasParameter(name))
                return Parameters[name].GetValueVector4();
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Gets the Color value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to get</param>
        /// <returns>The Color value of <i>name</i></returns>
        public Color GetParameterColor(string name)
        {
            if (HasParameter(name))
                return Color.FromNonPremultiplied(Parameters[name].GetValueVector4());
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Gets the Texture value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to get</param>
        /// <returns>The Texture value of <i>name</i></returns>
        public Texture2D GetParameterTexture(string name)
        {
            if (HasParameter(name))
                return Parameters[name].GetValueTexture2D();
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Gets the Matrix[] value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to get</param>
        /// <returns>The Matrix[] value of <i>name</i></returns>
        public Matrix[] GetParameterMatrixArray(string name, int ElementCount)
        {
            if (HasParameter(name))
                return Parameters[name].GetValueMatrixArray(ElementCount);
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Gets the float[] value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to get</param>
        /// <returns>The float[] value of <i>name</i></returns>
        public float[] GetParameterFloatArray(string name)
        {
            if (HasParameter(name))
                return Parameters[name].GetValueSingleArray();
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Gets the int[] value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to get</param>
        /// <returns>The int[] value of <i>name</i></returns>
        public int[] GetParameterIntArray(string name)
        {
            if (HasParameter(name))
                return Parameters[name].GetValueInt32Array();
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Gets the bool[] value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to get</param>
        /// <returns>The bool[] value of <i>name</i></returns>
        public bool[] GetParameterBoolArray(string name)
        {
            if (HasParameter(name))
                return Parameters[name].GetValueBooleanArray();
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Gets the Vector2[] value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to get</param>
        /// <returns>The Vector2[] value of <i>name</i></returns>
        public Vector2[] GetParameterVector2Array(string name)
        {
            if (HasParameter(name))
                return Parameters[name].GetValueVector2Array();
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Gets the Vector3[] value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to get</param>
        /// <returns>The Vector3[] value of <i>name</i></returns>
        public Vector3[] GetParameterVector3Array(string name)
        {
            if (HasParameter(name))
                return Parameters[name].GetValueVector3Array();
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Gets the Vector4[] value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to get</param>
        /// <returns>The Vector4[] value of <i>name</i></returns>
        public Vector4[] GetParameterVector4Array(string name)
        {
            if (HasParameter(name))
                return Parameters[name].GetValueVector4Array();
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Gets the Color[] value of a parameter
        /// NOTE: This method is fairly slow
        /// </summary>
        /// <param name="name">The name of the parameter to get</param>
        /// <returns>The Color[] value of <i>name</i></returns>
        public Color[] GetParameterColorArray(string name)
        {
            if (HasParameter(name))
            {
                Vector4[] temp =  Parameters[name].GetValueVector4Array();
                Color[] temp2 = new Color[temp.Length];

                for (int i = 0; i < temp.Length; i++)
                {
                    temp2[i] = Color.FromNonPremultiplied(temp[i]);
                }

                return temp2;
            }
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }
        #endregion

        #region Set Operators
        /// <summary>
        /// Sets the float value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="Value">The Matrix value to set <i>name</i> to</param>
        public void SetParameterMatrix(string name, Matrix Value)
        {
            if (HasParameter(name))
            {
                Parameters[name].SetValue(Value);
                baseEffect.Parameters[name].SetValue(Value);
            }
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Sets the float value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="Value">The Vector2 value to set <i>name</i> to</param>
        public void SetParameterVector2(string name, Vector2 Value)
        {
            if (HasParameter(name))
            {
                Parameters[name].SetValue(Value);
                baseEffect.Parameters[name].SetValue(Value);
            }
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Sets the float value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="Value">The Vector3 value to set <i>name</i> to</param>
        public void SetParameterVector3(string name, Vector3 Value)
        {
            if (HasParameter(name))
            {
                Parameters[name].SetValue(Value);
                baseEffect.Parameters[name].SetValue(Value);
            }
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Sets the float value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="Value">The Vector4 value to set <i>name</i> to</param>
        public void SetParameterVector4(string name, Vector4 Value)
        {
            if (HasParameter(name))
            {
                Parameters[name].SetValue(Value);
                baseEffect.Parameters[name].SetValue(Value);
            }
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Sets the float value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="Value">The int value to set <i>name</i> to</param>
        public void SetParameterInt(string name, int Value)
        {
            if (HasParameter(name))
            {
                Parameters[name].SetValue(Value);
                baseEffect.Parameters[name].SetValue(Value);
            }
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Sets the float value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="Value">The float value to set <i>name</i> to</param>
        public void SetParameterFloat(string name, float Value)
        {
            if (HasParameter(name))
            {
                Parameters[name].SetValue(Value);
                baseEffect.Parameters[name].SetValue(Value);
            }
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Sets the float value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="Value">The bool value to set <i>name</i> to</param>
        public void SetParameterBool(string name, bool Value)
        {
            if (HasParameter(name))
            {
                Parameters[name].SetValue(Value);
                baseEffect.Parameters[name].SetValue(Value);
            }
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Sets the Color value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="Value">The Color value to set <i>name</i> to</param>
        public void SetParameterColor(string name, Color Value)
        {
            if (HasParameter(name))
            {
                Parameters[name].SetValue(Value.ToVector4());
                baseEffect.Parameters[name].SetValue(Value.ToVector4());
            }
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }
        
        /// <summary>
        /// Sets the Texture value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to set</param>
        /// <returns>The Texture value of <i>name</i></returns>
        public void SetParameterTexture(string name, Texture2D Value)
        {
            if (HasParameter(name))
            {
                Parameters[name].SetValue(Value);
                baseEffect.Parameters[name].SetValue(Value);
            }
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Sets the float value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="Value">The Matrix[] value to set <i>name</i> to</param>
        public void SetParameterMatrixArray(string name, Matrix[] Value)
        {
            if (HasParameter(name))
            {
                Parameters[name].SetValue(Value);
                baseEffect.Parameters[name].SetValue(Value);
            }
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Sets the float value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="Value">The Vector2[] value to set <i>name</i> to</param>
        public void SetParameterVector2Array(string name, Vector2[] Value)
        {
            if (HasParameter(name))
            {
                Parameters[name].SetValue(Value);
                baseEffect.Parameters[name].SetValue(Value);
            }
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Sets the float value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="Value">The Vector3[] value to set <i>name</i> to</param>
        public void SetParameterVector3Array(string name, Vector3[] Value)
        {
            if (HasParameter(name))
            {
                Parameters[name].SetValue(Value);
                baseEffect.Parameters[name].SetValue(Value);
            }
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Sets the float value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="Value">The Vector4[] value to set <i>name</i> to</param>
        public void SetParameterVector4Array(string name, Vector4[] Value)
        {
            if (HasParameter(name))
            {
                Parameters[name].SetValue(Value);
                baseEffect.Parameters[name].SetValue(Value);
            }
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Sets the float value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="Value">The int[] value to set <i>name</i> to</param>
        public void SetParameterIntArray(string name, int[] Value)
        {
            if (HasParameter(name))
            {
                Parameters[name].SetValue(Value);
                baseEffect.Parameters[name].SetValue(Value);
            }
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Sets the float value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="Value">The float[] value to set <i>name</i> to</param>
        public void SetParameterFloatArray(string name, float[] Value)
        {
            if (HasParameter(name))
            {
                Parameters[name].SetValue(Value);
                baseEffect.Parameters[name].SetValue(Value);
            }
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }

        /// <summary>
        /// Sets the float value of a parameter
        /// </summary>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="Value">The bool[] value to set <i>name</i> to</param>
        public void SetParameterBoolArray(string name, bool[] Value)
        {
            if (HasParameter(name))
            {
                Parameters[name].SetValue(Value);
                baseEffect.Parameters[name].SetValue(Value);
            }
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }
        
        /// <summary>
        /// Sets the Color[] value of a parameter
        /// NOTE: this function is fairly slow
        /// </summary>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="Value">The Color[] value to set <i>name</i> to</param>
        public void SetParameterColorArray(string name, Color[] Value)
        {
            if (HasParameter(name))
            {
                Vector4[] temp = new Vector4[Value.Length];

                for (int i = 0; i < temp.Length; i++)
                    temp[i] = Value[i].ToVector4();

                Parameters[name].SetValue(temp);
                baseEffect.Parameters[name].SetValue(temp);
            }
            else
                throw new ArgumentException("This shader does not have a parameter called " + name);
        }
        #endregion
    }
}