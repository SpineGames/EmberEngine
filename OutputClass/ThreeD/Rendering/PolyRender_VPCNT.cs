﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using EmberEngine.ThreeD;
using EmberEngine.ThreeD.Tools;
using EmberEngine.ThreeD.Rendering.ShaderWrappers;

namespace EmberEngine.ThreeD.Rendering
{
    /// <summary>
    /// Used to render polygons
    /// </summary>
    public class PolyRender_VPNTC : PolyRender<VertexPositionNormalTextureColor> 
    {
        /// <summary>
        /// Creates a new VPNTC polygon renderer
        /// </summary>
        /// <param name="Graphics">The graphics device to use</param>
        /// <param name="cullMode">The culling to use. Default CullCounterClockwiseFace</param>
        public PolyRender_VPNTC(GraphicsDevice Graphics, CullMode cullMode = CullMode.CullCounterClockwiseFace) : 
            base(Graphics, cullMode) { }
        
        /// <summary>
        /// Creates a new VPNTC polygon renderer
        /// </summary>
        /// <param name="Shader">The shader to use</param>
        /// <param name="cullMode">The culling to use. Default CullCounterClockwiseFace</param>
        public PolyRender_VPNTC(Shader Shader, CullMode cullMode = CullMode.CullCounterClockwiseFace) : 
            base(Shader, cullMode) { }
    }
}
