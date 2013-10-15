using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmberEngine.ThreeD.Instancing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EmberEngine.ThreeD.Tools;
using EmberEngine.ThreeD;
using EmberEngine.ThreeD.Rendering;
using EmberEngine.Tools.AdvancedMath;
using EmberEngine.Tools.AdvancedMath.Noise;

namespace Samples.Sample1
{
    /// <summary>
    /// Represents a 3D terrain
    /// </summary>
    public class ThreeDTerrain : Instance
    {
        /// <summary>
        /// The width/height of the heightmap
        /// </summary>
        Point size;
        /// <summary>
        /// Gets the size of this terrain in world co-ords
        /// </summary>
        public Vector2 Size { get { return new Vector2(size.X, size.Y) * scale; } }
        
        float[,] heightmap;

        float scale = 1.0F;

        GraphicsDevice graphics;

        /// <summary>
        /// Creates a new 3D terrain
        /// </summary>
        /// <param name="Size">The size of the terrain</param>
        /// <param name="scale">The scale (size between points) to use</param>
        /// <param name="seed">The level seed to use</param>
        public ThreeDTerrain(GraphicsDevice graphics, Point Size, float scale, int seed) : base(new Vector3(0))
        {
            this.graphics = graphics;
            this.size = Size;
            this.scale = scale;

            heightmap = FaultFormation.GenMap(size.X, size.Y, 32, 16, seed); //21123
        }

        /// <summary>
        /// Gets a texture that represents this terrain's heightmap
        /// </summary>
        /// <param name="graphics">The graphics device to create texture with</param>
        /// <returns>A texture representing this terrains heightmap</returns>
        public Texture2D GetHeightmapTex()
        {
            Texture2D heightTex = new Texture2D(graphics, size.X, size.Y);

            float max = heightmap.Max();
            float min = heightmap.Min();

            float range = max - min;

            for (int x = 0; x < size.X; x++)
            {
                for (int y = 0; y < size.Y; y++)
                {
                    float val = (heightmap[x, y] - min) / range;

                    heightTex.SetData<Color>(0, new Rectangle(x, y, 1, 1),
                        new Color[]{Color.FromNonPremultiplied(
                        new Vector4(new Vector3(MathHelper.Lerp(0, 1.0F, val)), 1.0F))
                        }, 0, 1);
                }
            }

            return heightTex;
        }

        /// <summary>
        /// Overrides the Initialize method
        /// </summary>
        /// <param name="world">The world to add to</param>
        new public void Initialize(World world)
        {
            RebuildVerts();
            
            base.Initialize(world);
        }

        /// <summary>
        /// Gets the height at the given position
        /// </summary>
        /// <param name="x">The x co-ord to check</param>
        /// <param name="y">The y co-ord to check</param>
        /// <returns>The height at {x, y}</returns>
        public float GetHeight(float x, float y)
        {
            if (ExistsInTerrain(x, y))
            {
                int left = (int)x / (int)scale;
                int top = (int)y / (int)scale;

                float xOffNorm = (x % scale) / scale;
                float yOffNorm = (y % scale) / scale;

                float topHeight = MathHelper.Lerp(
                    heightmap[left, top] * scale,
                    heightmap[left + 1, top] * scale,
                    xOffNorm);

                float bottomHeight = MathHelper.Lerp(
                    heightmap[left, top + 1] * scale,
                    heightmap[left + 1, top + 1] * scale,
                    xOffNorm);

                // next, interpolate between those two values to calculate the height at our
                // position.
                return MathHelper.Lerp(topHeight, bottomHeight, yOffNorm);
            }
            else
                throw new IndexOutOfRangeException("The position {" + x + "," + y + "} is out of this terrain's range.");
        }

        /// <summary>
        /// Gets the height at the given position
        /// </summary>
        /// <param name="position">The position to check</param>
        /// <returns>The height at the position's {x,y}</returns>
        public float GetHeight(Vector3 position)
        {
            return GetHeight(position.X, position.Y);
        }

        /// <summary>
        /// Translates the vector to the surface height at the given x/y
        /// </summary>
        /// <param name="position">The position to check</param>
        /// <returns>The surface position at {x, y}</returns>
        public Vector3 TranslateVector(Vector3 position)
        {
            position.X = MathHelper.Clamp(position.X, 0, Size.X - scale);
            position.Y = MathHelper.Clamp(position.Y, 0, Size.Y - scale);

            return new Vector3(
                position.X,
                position.Y, 
                GetHeight(position.X, position.Y));
        }

        /// <summary>
        /// Checks if the given position is in this heightmap's range
        /// </summary>
        /// <param name="x">The x co-ord to check</param>
        /// <param name="y">The y co-ord to check</param>
        /// <returns>True if {x,y} is within this terrain's range</returns>
        public bool ExistsInTerrain(float x, float y)
        {
            return x >= 0 & y >= 0 & x <= (size.X - 1) * scale & y <= (size.Y - 1) * scale;
        }

        /// <summary>
        /// Rebuild the terrain's vertices
        /// </summary>
        private void RebuildVerts()
        {
            Renderer = new PolyRender(graphics);

            VertexPositionColorNormalTexture[] temp = new VertexPositionColorNormalTexture[size.X * size.Y];
            List<int> iBuffer = new List<int>();

            for (int x = 0; x < size.X; x++)
            {
                for (int y = 0; y < size.Y; y++)
                {
                    Vector3 TL = new Vector3(x * scale, y * scale, heightmap[x,y] * scale);

                    //v0 - v1, v1 - v2
                    Vector3 N1 = new Vector3(0, 0, 1);

                    temp[(y * size.X) + x] = new VertexPositionColorNormalTexture(TL, N1, 
                        new Vector2(x * scale, y * scale), Color.White);
                }
            }

            for (int x = 0; x < size.X - 1; x++)
            {
                for (int y = 0; y < size.Y - 1; y++)
                {
                    iBuffer.Add((x) + ((y) * size.X));
                    iBuffer.Add((x) + ((y + 1) * size.X));
                    iBuffer.Add((x + 1) + ((y) * size.X));

                    iBuffer.Add((x + 1) + ((y) * size.X));
                    iBuffer.Add((x) + ((y + 1) * size.X));
                    iBuffer.Add((x + 1) + ((y + 1) * size.X));
                }
            }

            for (int i = 0; i < iBuffer.Count / 3; i++)
            {
                Vector3 firstvec = temp[iBuffer[i * 3 + 1]].Position - temp[iBuffer[i * 3]].Position;
                Vector3 secondvec = temp[iBuffer[i * 3]].Position - temp[iBuffer[i * 3 + 2]].Position;

                Vector3 normal = Vector3.Cross(firstvec, secondvec);
                normal.Normalize();

                temp[iBuffer[i * 3]].Normal = normal;
                temp[iBuffer[i * 3 + 1]].Normal = normal;
                temp[iBuffer[i * 3 + 2]].Normal = normal;
            }

            ((PolyRender)Renderer).AddPolys(temp);
            ((PolyRender)Renderer).FinalizePolys(iBuffer.ToArray());
        }
    }
}
