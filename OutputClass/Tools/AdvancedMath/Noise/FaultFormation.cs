using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace EmberEngine.Tools.AdvancedMath.Noise
{
    /// <summary>
    /// Uses the fault-formation algorithm to generate terrain
    /// </summary>
    public static class FaultFormation
    {
        static Random rand = new Random();

        /// <summary>
        /// Generates a map using the fault formation algorithm
        /// </summary>
        /// <param name="width">The width of the terrain</param>
        /// <param name="height">The height of the terrain</param>
        /// <param name="iterations">The number of lines to cut per level</param>
        /// <param name="h">The initial cut height range</param>
        /// <param name="erode">True if the terrain should erode after generating</param>
        /// <param name="erodeLevel">Erode level <b>default 2</b></param>
        /// <returns> A map generated using fualt formation</returns>
        public static float[,] GenMap(int width, int height, int iterations, float h, bool erode = true, int erodeLevel = 2)
        {
            float[,] temp = new float[width, height];

            int i = 0;

            while (h > 1)
            {
                while (i < iterations)
                {
                    Vector2 p1 = new Vector2(rand.Next(0, width), rand.Next(0, height));
                    Vector2 p2 = new Vector2(rand.Next(0, width), rand.Next(0, height));

                    if (p1 != p2)
                    {
                        SetLine(temp, new Line(p1, p2), GetRand(-h / 2.0F, h / 2.0F), GetRand(-h / 2.0F, h / 2.0F),
                            new Point(width, height));

                        i++;
                    }
                }
                h = h / 2.0F;
            }

            if (erode)
                temp = Erode.erode(temp, new Point(width, height), erodeLevel);

            return temp;
        }

        /// <summary>
        /// Generates a map using the fault formation algorithm
        /// </summary>
        /// <param name="width">The width of the terrain</param>
        /// <param name="height">The height of the terrain</param>
        /// <param name="iterations">The number of lines to cut per level</param>
        /// <param name="h">The initial cut height range</param>
        /// <param name="seed">The level seed to use</param>
        /// <param name="erode">True if the terrain should erode after generating</param>
        /// <param name="erodeLevel">Erode level <b>default 2</b></param>
        /// <returns> A map generated using fualt formation</returns>
        public static float[,] GenMap(int width, int height, int iterations, float h, int seed,
            bool erode = true, int erodeLevel = 2)
        {
            rand = new Random(seed);

            float[,] temp = new float[width, height];

            int i = 0;

            while (h > 1)
            {
                while (i < iterations)
                {
                    Vector2 p1 = new Vector2(rand.Next(0, width), rand.Next(0, height));
                    Vector2 p2 = new Vector2(rand.Next(0, width), rand.Next(0, height));

                    if (p1 != p2)
                    {
                        SetLine(temp, new Line(p1, p2), GetRand(-h / 2.0F, h / 2.0F), GetRand(-h / 2.0F, h / 2.0F),
                            new Point(width, height));

                        i++;
                    }
                }
                h = h / 2.0F;
            }

            if (erode)
                temp = Erode.erode(temp, new Point(width, height), erodeLevel);

            return temp;
        }

        /// <summary>
        /// Gets a value between min and max
        /// </summary>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        /// <returns>A random value between min and max</returns>
        private static float GetRand(float min, float max)
        {
            return (float)(rand.NextDouble() * (max - min) + min);
        }

        /// <summary>
        /// Cuts the terrain from the line
        /// </summary>
        /// <param name="values">The map to cut</param>
        /// <param name="line">The line to cut along</param>
        /// <param name="left">The value on the left</param>
        /// <param name="right">The value on the right</param>
        /// <param name="size">The size of the map</param>
        /// <returns><i>values</i>, cut along <i>line</i></returns>
        private static float[,] SetLine(float[,] values, Line line, float left, float right, Point size)
        {
            for (int x = 0; x < size.X; x++)
            {
                for (int y = 0; y < size.Y; y++)
                {
                    if (y >= (line.Slope * x) + line.P2.Y)
                        values[x, y] += left;
                    else
                        values[x, y] += right;
                }
            }

            return values;
        }

        /// <summary>
        /// Represents a line to gut along
        /// </summary>
        private struct Line
        {
            public Vector2 P1;
            public Vector2 P2;
            public float Slope;

            public Line(Vector2 p1, Vector2 p2)
            {
                this.P1 = p1;
                this.P2 = p2;
                this.Slope = (float)(p2.Y - P1.Y) / (float)(p2.X - p1.X);
            }
        }
    }
}
