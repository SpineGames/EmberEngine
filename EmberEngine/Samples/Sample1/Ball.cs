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

namespace Samples.Sample1
{
    public class Ball : Instance
    {
        public Ball(float radius, int stepping, Vector3 position, BasicShader effect, int texCount = 1)
            : base(position)
        {
            PolyRender_VPNTC renderer = new PolyRender_VPNTC(effect.Clone());
            Color color = Color.White;

            Vector3 rad = new Vector3(radius, 0, 0);

            List<VertexPositionNormalTextureColor> temp = new List<VertexPositionNormalTextureColor>();

            for (int yaw = 0; yaw < stepping; yaw++) //90 circles, difference between each is 4 degrees
            {
                float difx = 360.0f / (float)stepping;

                for (int pitch = 0; pitch < stepping; pitch++) //90 veritces, difference between each is 4 degrees 
                {
                    float dify = 360.0f / (float)stepping;

                    Vector3 point = Math2.GetFromYawPitch(yaw * difx, pitch * dify, radius);

                    Vector3 point2 = Math2.GetFromYawPitch((yaw + 1) * difx, (pitch) * dify, radius);
                    
                    Vector3 point3 = Math2.GetFromYawPitch((yaw) * difx, (pitch + 1) * dify, radius);

                    Vector3 point4 = Math2.GetFromYawPitch((yaw + 1) * difx, (pitch + 1) * dify, radius);

                    Vector3 n1 = Math2.GetFromYawPitch(
                        ((pitch * dify) + ((pitch + 1) * dify)) / 2.0F,
                        ((yaw * difx) + ((yaw + 1) * difx)) / 2.0F, 1);
                    
                    temp.Add(FV.VPCNT(point, n1, Math2.GetUVForSphere(yaw * difx, pitch * dify, radius), color));
                    temp.Add(FV.VPCNT(point2, n1, Math2.GetUVForSphere((yaw + 1) * difx, (pitch) * dify, radius), color));
                    temp.Add(FV.VPCNT(point3, n1, Math2.GetUVForSphere((yaw) * difx, (pitch + 1) * dify, radius), color));

                    temp.Add(FV.VPCNT(point, n1, Math2.GetUVForSphere(yaw * difx, pitch * dify, radius), color));
                    temp.Add(FV.VPCNT(point4, n1, Math2.GetUVForSphere((yaw + 1) * difx, (pitch + 1) * dify, radius), color));
                    temp.Add(FV.VPCNT(point2, n1, Math2.GetUVForSphere((yaw + 1) * difx, (pitch) * dify, radius), color));
                }
            }
            renderer.AddPolys(temp.ToArray());
            renderer.FinalizePolys();

            this.Renderer = renderer;
        }
    }
}
