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
    public class PolyRender_MULTITEX : PolyRender<VertexMultitextured> 
    {
        /// <summary>
        /// Creates a new multitextured polygon renderer
        /// </summary>
        /// <param name="Graphics">The graphics device to use</param>
        /// <param name="cullMode">The culling to use. Default CullCounterClockwiseFace</param>
        public PolyRender_MULTITEX(GraphicsDevice Graphics, CullMode cullMode = CullMode.CullCounterClockwiseFace) 
            : base(Graphics, cullMode) { }
        
        /// <summary>
        /// Creates a new multitextured polygon renderer
        /// </summary>
        /// <param name="Shader">The shader to use</param>
        /// <param name="cullMode">The culling to use. Default CullCounterClockwiseFace</param>
        public PolyRender_MULTITEX(Shader Shader, CullMode cullMode = CullMode.CullCounterClockwiseFace) : 
            base(Shader, cullMode) { } 
    }
}
