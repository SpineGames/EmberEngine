using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using EmberEngine.ThreeD.Tools;
using EmberEngine.Tools.AdvancedMath;
using EmberEngine.ThreeD.Instancing;
using EmberEngine.ThreeD.Rendering;
using EmberEngine.ThreeD.Rendering.ShaderWrappers;

namespace Samples.Sample2
{
    public class Spinner : Instance
    {
        public Spinner(Vector3 min, Vector3 max, Shader shader, int radius = 1, int stepping = 20) :
            base(min)
        {
            stepping = stepping < 4 ? 4: stepping;

            VertexPositionColorNormalTexture[] verts = new VertexPositionColorNormalTexture[2 + stepping * 2];
            int[] indexBuffer = new int[12 * stepping];

            Vector3 n = max - min;
            n.Normalize();

            Capsole(n, (max - min).Length(), radius, stepping, out verts);

            PolyRender_VPCNT render = new PolyRender_VPCNT(shader);
            render.AddPolys(verts);
            render.FinalizePolys();

            this.Renderer = render;
        }

        static Random ms_random = new Random();

        private static void Capsole(Vector3 Normal, float height, float radius, int stepping,
                           out VertexPositionColorNormalTexture[] vertices)
        {
            List<VertexPositionColorNormalTexture> verts = new List<VertexPositionColorNormalTexture>();

            Vector3 a = Math2.GetPitchRollYaw(Normal);
            Matrix Transform = Matrix.CreateFromYawPitchRoll(a.Z, a.X, 0);

            Vector2 center = new Vector2(0, 0);

            //figure out the difference
            double increment = 360.0 / stepping;

            Vector3 Bottom = new Vector3(0);
            Vector3 BN = new Vector3(0,0,-1);

            Vector3 Top = new Vector3(0, 0, height);
            Vector3 TN = new Vector3(0,0,1);

            float texOff = 10F / stepping;
            //render
            double angle = 0;
            for (int i = 0; i < stepping; i++, angle += increment)
            {
                Vector3 RB = new Vector3((float)Math2.LengthdirX(angle, radius), (float)Math2.LengthdirY(angle, radius), 0);//Math2.GetFromYawPitch(angle, 0, radius);
                Vector3 RT = new Vector3((float)Math2.LengthdirX(angle, radius), (float)Math2.LengthdirY(angle, radius), 0) +Top;

                Vector3 LB = new Vector3((float)Math2.LengthdirX(angle + increment, radius), (float)Math2.LengthdirY(angle + increment, radius), 0);
                Vector3 LT = new Vector3((float)Math2.LengthdirX(angle + increment, radius), (float)Math2.LengthdirY(angle + increment, radius), 0) +Top;

                Vector3 RN = new Vector3(RB.X, RB.Y, RB.Z);
                RN.Normalize();
                Vector3 LN = new Vector3(LB.X, LB.Y, LB.Z);
                LN.Normalize();

                verts.Add(FV.VPCNT(RB, RN, GetTexCoord(angle, false), Color.White));
                verts.Add(FV.VPCNT(RT, RN, GetTexCoord(angle, true), Color.White));
                verts.Add(FV.VPCNT(LB, LN, GetTexCoord(angle + increment, false), Color.White));

                verts.Add(FV.VPCNT(RT, RN, GetTexCoord(angle, true), Color.White));
                verts.Add(FV.VPCNT(LT, LN, GetTexCoord(angle + increment, true), Color.White));
                verts.Add(FV.VPCNT(LB, LN, GetTexCoord(angle + increment, false), Color.White));

                verts.Add(FV.VPCNT(LT, TN, Vector2.Zero, Color.White));
                verts.Add(FV.VPCNT(RT, TN, Vector2.Zero, Color.White));
                verts.Add(FV.VPCNT(Top, TN, Vector2.Zero, Color.White));

                verts.Add(FV.VPCNT(RB, BN, Vector2.Zero, Color.White));
                verts.Add(FV.VPCNT(LB, BN, Vector2.Zero, Color.White));
                verts.Add(FV.VPCNT(Bottom, BN, Vector2.Zero, Color.White));
            }

            for(int i = 0; i < verts.Count; i++)
            {
                VertexPositionColorNormalTexture temp;

                temp.Position = Vector3.Transform(verts[i].Position, Transform);
                temp.Normal = Vector3.TransformNormal(verts[i].Normal, Transform);
                temp.TexCoords = verts[i].TexCoords;
                temp.Color = verts[i].Color;

                verts[i] = temp;
            }

            vertices = verts.ToArray();
        }

        private static Vector2 GetTexCoord(double angle, bool top)
        {
            if (top)
                return new Vector2(MathHelper.Lerp(0, 1, (float)(angle / 360.0)), 0);
            else
                return new Vector2(MathHelper.Lerp(0, 1, (float)(angle / 360.0)), 1);
        }
    }
}
