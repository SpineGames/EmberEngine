﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmberEngine.ThreeD.Instancing;
using Microsoft.Xna.Framework;
using EmberEngine.ThreeD.Tools;
using EmberEngine.Tools.AdvancedMath;
using EmberEngine.ThreeD.Rendering;
using EmberEngine.ThreeD.Rendering.ShaderWrappers;

namespace Samples.Sample2
{
    class Plane : Instance
    {
        public Plane(Vector3 Min, Vector3 Max, Shader shader,int texTile = 1)
            : base(Vector3.Zero)
        {
            VertexPositionNormalTextureColor[] verts = new VertexPositionNormalTextureColor[4];
            int[] indices = new int[6];
            
            Vector3 TL = new Vector3(Min.X, Max.Y, Max.Z);
            Vector3 BR = new Vector3(Max.X, Min.Y, Min.Z);
           
            //TL, BR, Min
            //v0 - v1, v1 - v2
            Vector3 N = Vector3.Cross(TL - BR, BR - Min);
            N.Normalize();

            verts[0] = FV.VPCNT(TL, N, new Vector2(0, texTile), Color.White);
            verts[1] = FV.VPCNT(Max, N, new Vector2(texTile), Color.White);
            verts[2] = FV.VPCNT(BR, N, new Vector2(texTile, 0), Color.White);
            verts[3] = FV.VPCNT(Min, N, new Vector2(0), Color.White);

            indices[0] = 0;
            indices[1] = 2;
            indices[2] = 3;

            indices[3] = 0;
            indices[4] = 1;
            indices[5] = 2;

            PolyRender_VPNTC render = new PolyRender_VPNTC(shader);
            render.AddPolys(verts);
            render.FinalizePolys(indices);
            this.Renderer = render;
        }

        public Plane(Vector3 Min, Vector3 Max, Shader shader, Vector2 texTile)
            : base(Vector3.Zero)
        {
            VertexPositionNormalTextureColor[] verts = new VertexPositionNormalTextureColor[4];
            int[] indices = new int[6];

            Vector3 TL = new Vector3(Min.X, Max.Y, Max.Z);
            Vector3 BR = new Vector3(Max.X, Min.Y, Min.Z);

            //TL, BR, Min
            //v0 - v1, v1 - v2
            Vector3 N = Vector3.Cross(TL - BR, BR - Min);
            N.Normalize();

            verts[0] = FV.VPCNT(TL, N, new Vector2(0, texTile.Y), Color.White);
            verts[1] = FV.VPCNT(Max, N, texTile, Color.White);
            verts[2] = FV.VPCNT(BR, N, new Vector2(texTile.X, 0), Color.White);
            verts[3] = FV.VPCNT(Min, N, new Vector2(0), Color.White);

            indices[0] = 0;
            indices[1] = 2;
            indices[2] = 3;

            indices[3] = 0;
            indices[4] = 1;
            indices[5] = 2;

            PolyRender_VPNTC render = new PolyRender_VPNTC(shader);
            render.AddPolys(verts);
            render.FinalizePolys(indices);
            this.Renderer = render;
        }
    }
}
