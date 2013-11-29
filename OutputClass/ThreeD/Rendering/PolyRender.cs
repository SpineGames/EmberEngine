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
using System.Reflection;

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
        VertexBuffer VBuffer;
        /// <summary>
        /// The indeces used to speed up rendering
        /// </summary>
        int[] Indices;
        IndexBuffer IBuffer;

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
            get;
            set;
        }

        static RasterizerState wireframe;
        /// <summary>
        /// Gets a wireframe rasterizer
        /// </summary>
        public RasterizerState Wireframe
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
        public RasterizerState Solid
        {
            get
            {
                return solid;
            }
        }

        /// <summary>
        /// The field info for the vertex declaration
        /// </summary>
        private static readonly PropertyInfo field = typeof(T).GetProperty("VertexDeclaration");
        /// <summary>
        /// Represents the vertex declaration for this renderer
        /// </summary>
        VertexDeclaration vertexDeclaration;

        /// <summary>
        /// Initializes the Poly renderer's static parameters
        /// </summary>
        /// <param name="cullMode">The culling to usee in this game</param>
        public void Initialize(CullMode cullMode)
        {
            wireframe = new RasterizerState();
            wireframe.FillMode = FillMode.WireFrame;
            wireframe.CullMode = cullMode;

            solid = new RasterizerState();
            solid.FillMode = FillMode.Solid;
            solid.CullMode = cullMode;

            vertexDeclaration = (VertexDeclaration)field.GetValue(new T(), null);
        }

        /// <summary>
        /// Creates a new poly renderer
        /// </summary>
        /// <param name="graphics">The GraphicsDevice to use</param>
        public PolyRender(GraphicsDevice graphics, CullMode cullMode)
        {
            this.Graphics = graphics;

            Initialize(cullMode);
        }

        /// <summary>
        /// Creates a new poly renderer
        /// </summary>
        /// <param name="World">The world transformation to use</param>
        public PolyRender(Shader shader, CullMode cullMode)
        {
            this.Effect = shader.Clone();
            this.Graphics = shader.BaseEffect.GraphicsDevice;

            Initialize(cullMode);
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
        /// Gets the vertices from this poly renderer
        /// </summary>
        /// <returns></returns>
        public T[] GetPolies()
        {
            return Buffer.ToArray();
        }

        /// <summary>
        /// Copies over and clears the temp buffers. This will automagically create
        /// the index buffers
        /// </summary>
        public void FinalizePolys()
        {
            Buffer = Temp.ToArray();
            VertCount = Buffer.Length;
            Temp.Clear();

            Indices = new int[Buffer.Length];

            for (int i = 0; i < Buffer.Length; i++)
            {
                Indices[i] = i;
            }

            SetPolyCount();

            VBuffer = new VertexBuffer(Graphics, vertexDeclaration, VertCount, BufferUsage.WriteOnly);
            VBuffer.SetData<T>(Buffer);

            IBuffer = new IndexBuffer(Graphics, IndexElementSize.ThirtyTwoBits, Indices.Length, BufferUsage.WriteOnly);
            IBuffer.SetData<int>(Indices);
        }

        /// <summary>
        /// Copies over and clears the temp buffers, also uses a pre-defined
        /// index buffer
        /// </summary>
        /// <param name="indexBuffer">The index buffer to use</param>
        public void FinalizePolys(int[] indexBuffer)
        {
            Buffer = Temp.ToArray();
            VertCount = Buffer.Length;
            Temp.Clear();

            Indices = indexBuffer;

            SetPolyCount();
            
            VBuffer = new VertexBuffer(Graphics, vertexDeclaration, VertCount, BufferUsage.WriteOnly);
            VBuffer.SetData<T>(Buffer);

            IBuffer = new IndexBuffer(Graphics, IndexElementSize.ThirtyTwoBits, Indices.Length, BufferUsage.WriteOnly);
            IBuffer.SetData<int>(Indices);
        }

        /// <summary>
        /// Sets the correct poly count
        /// </summary>
        private void SetPolyCount()
        {
            switch (RenderType)
            {
                case PrimitiveType.TriangleList:
                    PrimitiveCount = Indices.Length / 3;
                    break;

                case PrimitiveType.LineList:
                    PrimitiveCount = Indices.Length / 2;
                    break;

                case PrimitiveType.LineStrip:
                    PrimitiveCount = Indices.Length - 1;
                    break;

                case PrimitiveType.TriangleStrip:
                    PrimitiveCount = Indices.Length - 2;
                    break;
            }
        }

        /// <summary>
        /// Renders Opaque and non-opaque polys
        /// </summary>
        /// <param name="view">The view paramaters to render with</param>
        public void Render(ThreeDCamera view)
        {
            if (PrimitiveCount > 0)
            {
                if (WireFrame)
                    Graphics.RasterizerState = wireframe;
                else
                    Graphics.RasterizerState = solid;

                view.ApplyToEffect(Effect, world);

                foreach (EffectPass p in Effect.BaseEffect.CurrentTechnique.Passes)
                {
                    p.Apply();

                    Graphics.SetVertexBuffer(VBuffer);
                    Graphics.Indices = IBuffer;

                    //Graphics.DrawPrimitives(RenderType, 0,  PrimitiveCount);

                    Graphics.DrawUserIndexedPrimitives<T>(RenderType, Buffer, 0, VertCount,
                        Indices, 0, PrimitiveCount);
                }
            }
        }
    }
}
