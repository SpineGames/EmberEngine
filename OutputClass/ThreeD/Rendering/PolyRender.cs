using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using EmberEngine.ThreeD.Tools;
using Microsoft.Xna.Framework;
using EmberEngine.ThreeD;
using EmberEngine.ThreeD.Rendering;
using EmberEngine.ThreeD.Rendering.ShaderWrappers;
using EmberEngine.EmberEngine.ThreeD.Rendering;

namespace EmberEngine.ThreeD.Rendering
{
    /// <summary>
    /// Used to render polygons
    /// </summary>
    public class PolyRender<T> : IRenderable where T : struct, IVertexType
    {
        /// <summary>
        /// The buffer for all vertices
        /// </summary>
        T[] Buffer = new T[] { };
        /// <summary>
        /// The indeces used to speed up rendering
        /// </summary>
        int[] Indices;

        /// <summary>
        /// The number of primitives (triangles or lines)
        /// </summary>
        int PrimitiveCount = 0;
        /// <summary>
        /// The number of vertices
        /// </summary>
        int VertCount = 0;
        /// <summary>
        /// The temporary buffer for the non-opaque vertices
        /// </summary>
        List<T> Temp = new List<T>();

        /// <summary>
        /// The world transformation for this renderer
        /// </summary>
        Matrix world = Matrix.Identity;
        /// <summary>
        /// Gets or sets the world transformation for this renderer
        /// </summary>
        public Matrix World
        {
            get { return world; }
            set { world = value; }
        }

        /// <summary>
        /// Gets or sets this poly-renderer's Effect
        /// </summary>
        public Shader Effect;
        /// <summary>
        /// The effects name for the World matrix
        /// </summary>
        public string WorldParameter;
        /// <summary>
        /// The effects name for the View matrix
        /// </summary>
        public string ViewParameter;
        /// <summary>
        /// The effects name for the Projection matrix
        /// </summary>
        public string ProjectionParameter;

        /// <summary>
        /// Gets or sets the primitive type to render with
        /// </summary>
        public PrimitiveType RenderType;

        /// <summary>
        /// Gets or sets this poly renderer's GraphicsDevice
        /// </summary>
        public GraphicsDevice Graphics;

        Texture2D tex;
        /// <summary>
        /// Gets or sets the texture
        /// Note: All shader's must have texture parameter called 'Texture'
        /// </summary>
        public Texture2D Texture
        {
            get { return tex; }
            set
            {
                tex = value;
                Effect.SetParameterTexture(Effect.TexName, tex);
            }
        }

        bool wireFrame;
        /// <summary>
        /// Gets or sets whether this poly renderer draws in wireframe
        /// </summary>
        public bool WireFrame
        {
            get { return wireFrame; }
            set
            {
                wireFrame = value;
                Graphics.RasterizerState = value ? wireframe : solid;
            }
        }

        static RasterizerState wireframe;
        /// <summary>
        /// Gets a wireframe rasterizer
        /// </summary>
        public static RasterizerState Wireframe
        {
            get
            {
                return wireframe;
            }
        }

        static RasterizerState solid;
        /// <summary>
        /// Gets a solid rasterizer
        /// </summary>
        public static RasterizerState Solid
        {
            get
            {
                return solid;
            }
        }

        /// <summary>
        /// Initializes the Poly renderer's static parameters
        /// </summary>
        /// <param name="cullMode">The culling to usee in this game</param>
        public static void Initialize(CullMode cullMode)
        {
            wireframe = new RasterizerState();
            wireframe.FillMode = FillMode.WireFrame;
            wireframe.CullMode = cullMode;

            solid = new RasterizerState();
            solid.FillMode = FillMode.Solid;
            solid.CullMode = cullMode;
        }

        /// <summary>
        /// Creates a new poly renderer
        /// </summary>
        /// <param name="graphics">The GraphicsDevice to use</param>
        public PolyRender(GraphicsDevice graphics)
        {
            this.Graphics = graphics;
        }

        /// <summary>
        /// Creates a new poly renderer
        /// </summary>
        /// <param name="World">The world transformation to use</param>
        public PolyRender(Shader shader)
        {
            this.Effect = shader.Clone();
            this.Graphics = shader.BaseEffect.GraphicsDevice;
        }

        /// <summary>
        /// clears the temporary and final buffers
        /// </summary>
        public void Clear()
        {
            Temp.Clear();
            Buffer = new T[0];
            Indices = new int[0];
        }
        
        /// <summary>
        /// Adds a range of non-opaque vertices to the temp buffer
        /// </summary>
        /// <param name="buffer">The buffer to append</param>
        public void AddPolys(T[] buffer)
        {
            Temp.AddRange(buffer);
        }

        /// <summary>
        /// Copies over and clears the temp buffers. This will automagically create
        /// the index buffers
        /// </summary>
        public void FinalizePolys()
        {
            Buffer = Temp.ToArray();
            Temp.Clear();
            PrimitiveCount = Buffer.Length / 3;
            VertCount = Buffer.Length;

            Indices = new int[Buffer.Length];

            for (int i = 0; i < Buffer.Length; i++)
            {
                Indices[i] = i;
            }
        }

        /// <summary>
        /// Copies over and clears the temp buffers, also uses a pre-defined
        /// index buffer
        /// </summary>
        /// <param name="indexBuffer">The index buffer to use</param>
        public void FinalizePolys(int[] indexBuffer)
        {
            Buffer = Temp.ToArray();
            Temp.Clear();
            PrimitiveCount = indexBuffer.Length / 3;
            VertCount = Buffer.Length;

            Indices = indexBuffer;
        }

        /// <summary>
        /// Renders Opaque and non-opaque polys
        /// </summary>
        /// <param name="view">The view paramaters to render with</param>
        public void Render(ThreeDCamera view)
        {
            if (PrimitiveCount > 0)
            {
                view.ApplyToEffect(Effect, world);

                foreach (EffectPass p in Effect.BaseEffect.CurrentTechnique.Passes)
                {
                    p.Apply();

                    Graphics.DrawUserIndexedPrimitives<T>(
                        RenderType, Buffer, 0, VertCount, Indices, 0, PrimitiveCount);
                }
            }
        }
    }
}
