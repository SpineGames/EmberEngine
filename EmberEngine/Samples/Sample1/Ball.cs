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
            PolyRender renderer = new PolyRender(effect.Clone());
            Color color = Color.White;

            Vector3 rad = new Vector3(radius, 0, 0);

            List<VertexPositionColorNormalTexture> temp = new List<VertexPositionColorNormalTexture>();

            for (int x = 0; x < stepping; x++) //90 circles, difference between each is 4 degrees
            {
                float difx = 360.0f / (float)stepping;
                for (int y = 0; y < stepping; y++) //90 veritces, difference between each is 4 degrees 
                {
                    float dify = 360.0f / (float)stepping;
                    Matrix zrot = Matrix.CreateRotationZ(MathHelper.ToRadians(y * dify)); //rotate vertex around z
                    Matrix yrot = Matrix.CreateRotationY(MathHelper.ToRadians(x * difx)); //rotate circle around y
                    Vector3 point = Vector3.Transform(Vector3.Transform(rad, zrot), yrot);//transformation

                    Matrix zrot2 = Matrix.CreateRotationZ(MathHelper.ToRadians((y + 1) * dify)); //rotate vertex around z
                    Matrix yrot2 = Matrix.CreateRotationY(MathHelper.ToRadians((x + 1) * difx)); //rotate circle around y
                    Vector3 point2 = Vector3.Transform(Vector3.Transform(rad, zrot2), yrot2);//transformation

                    Matrix zrot3 = Matrix.CreateRotationZ(MathHelper.ToRadians((y + 1) * dify)); //rotate vertex around z
                    Matrix yrot3 = Matrix.CreateRotationY(MathHelper.ToRadians(x * difx)); //rotate circle around y
                    Vector3 point3 = Vector3.Transform(Vector3.Transform(rad, zrot3), yrot3);//transformation

                    Matrix zrot4 = Matrix.CreateRotationZ(MathHelper.ToRadians(y * dify)); //rotate vertex around z
                    Matrix yrot4 = Matrix.CreateRotationY(MathHelper.ToRadians((x + 1) * difx)); //rotate circle around y
                    Vector3 point4 = Vector3.Transform(Vector3.Transform(rad, zrot4), yrot4);//transformation

                    Vector3 n1 = Math2.GetFromYawPitch(
                        (y * dify + (y + 1) * dify) / 2,
                        (x * difx + (x + 1) * difx) / 2, 1);

                    float UVMult = (360.0F / texCount);

                    temp.Add(FV.VPCNT(point, n1, x * difx / UVMult, y * dify / UVMult, color));
                    temp.Add(FV.VPCNT(point2, n1, (x + 1) * difx / UVMult, (y + 1) * dify / UVMult, color));
                    temp.Add(FV.VPCNT(point3, n1, x * difx / UVMult, (y + 1) * dify / UVMult, color));

                    temp.Add(FV.VPCNT(point, n1, x * difx / UVMult, y * dify / UVMult, color));
                    temp.Add(FV.VPCNT(point4, n1, (x + 1) * difx / UVMult, (y + 1) * dify / UVMult, color));
                    temp.Add(FV.VPCNT(point2, n1, (x + 1) * difx / UVMult, y * dify / UVMult, color));
                }
            }
            renderer.AddPolys(temp.ToArray());
            renderer.FinalizePolys();

            this.Renderer = renderer;
        }
    }
}
