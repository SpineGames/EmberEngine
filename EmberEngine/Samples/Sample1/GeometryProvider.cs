using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EmberEngine.ThreeD.Rendering;
using EmberEngine.ThreeD.Tools;
using EmberEngine.Tools.AdvancedMath;

namespace Samples.Samples.Sample1
{
    public class SphereProvider
    {
        public static void GenSphere(PolyRender_VPNTC polyRenderer,
                               float diameter, int tessellation, Color color)
        {
            List<VertexPositionNormalTextureColor> vertices = new List<VertexPositionNormalTextureColor>();
            List<int> indices = new List<int>();

            if (tessellation < 2)
                throw new ArgumentOutOfRangeException("tessellation");

            int verticalSegments = tessellation;
            int horizontalSegments = tessellation * 2;

            float radius = diameter / 2;

            // Start with a single vertex at the bottom of the sphere.
            vertices.Add(
                FV.VPCNT(Vector3.Down * radius, Vector3.Down, new Vector2(0.5F, 1), color));

            // Create rings of vertices at progressively higher latitudes.
            for (int i = 0; i < verticalSegments - 1; i++)
            {
                float latitude = ((i + 1) * MathHelper.Pi /
                                            verticalSegments) - MathHelper.PiOver2;

                float dy = (float)Math.Sin(latitude);
                float dxz = (float)Math.Cos(latitude);

                // Create a single ring of vertices at this latitude.
                for (int j = 0; j < horizontalSegments; j++)
                {
                    float longitude = j * MathHelper.TwoPi / horizontalSegments;

                    float dx = (float)Math.Cos(longitude) * dxz;
                    float dz = (float)Math.Sin(longitude) * dxz;

                    Vector3 normal = new Vector3(dx, dy, dz);

                    Vector2 texCoord = new Vector2(
                            ((longitude) / MathHelper.TwoPi).Wrap(0, 1),
                            ((latitude + MathHelper.PiOver2) / MathHelper.Pi).Wrap(0, 1));

                    vertices.Add(
                        FV.VPCNT(normal * radius, Vector3.Down, texCoord, color));
                }
            }

            vertices.Add(
                FV.VPCNT(Vector3.Up * radius, Vector3.Down, new Vector2(0.5F, 1), color));

            // Create a fan connecting the bottom vertex to the bottom latitude ring.
            for (int i = 0; i < horizontalSegments; i++)
            {
                indices.Add(0);
                indices.Add(1 + (i + 1) % horizontalSegments);
                indices.Add(1 + i);
            }

            // Fill the sphere body with triangles joining each pair of latitude rings.
            for (int i = 0; i < verticalSegments - 2; i++)
            {
                for (int j = 0; j < horizontalSegments; j++)
                {
                    int nextI = i + 1;
                    int nextJ = (j + 1) % horizontalSegments;

                    indices.Add(1 + i * horizontalSegments  + j);
                    indices.Add(1 + i * horizontalSegments  + nextJ);
                    indices.Add(1 + nextI * horizontalSegments  + j);

                    indices.Add(1 + i * horizontalSegments  + nextJ);
                    indices.Add(1 + nextI * horizontalSegments  + nextJ);
                    indices.Add(1 + nextI * horizontalSegments  + j);
                }
            }

            // Create a fan connecting the top vertex to the top latitude ring.
            for (int i = 0; i < horizontalSegments; i++)
            {
                indices.Add(vertices.Count - 1);
                indices.Add(vertices.Count - 2 - (i + 1) % horizontalSegments);
                indices.Add(vertices.Count - 2 - i);
            }

            polyRenderer.AddPolys(vertices.ToArray());
            polyRenderer.FinalizePolys(indices.ToArray());
        }
    }
}
