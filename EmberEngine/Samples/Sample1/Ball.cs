using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmberEngine.ThreeD.Instancing;
using EmberEngine.ThreeD.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EmberEngine.ThreeD.Tools;
using EmberEngine.Tools.AdvancedMath;
using EmberEngine.ThreeD.Rendering.ShaderWrappers;
using Samples.Samples.Sample1;

namespace Samples.Sample1
{
    public class Ball : Instance
    {
        public Ball(float radius, int stepping, Vector3 position, BasicShader effect, int texCount = 1)
            : base(position)
        {
            PolyRender_VPNTC renderer = new PolyRender_VPNTC(effect.Clone(), CullMode.None);

            SphereProvider.GenSphere(renderer, 50, 12, Color.Gray);
            renderer.RenderType = PrimitiveType.TriangleList;

            this.Renderer = renderer;
        }
    }
}
