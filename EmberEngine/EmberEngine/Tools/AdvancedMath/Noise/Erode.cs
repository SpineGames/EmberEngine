using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace EmberEngine.Tools.AdvancedMath.Noise
{
    /// <summary>
    /// Handles eroding maps
    /// </summary>
    public static class Erode
    {
        /// <summary>
        /// Erored the given array
        /// </summary>
        /// <param name="values">The map to erode</param>
        /// <param name="size">The size of the map to erode</param>
        /// <param name="iterations">The number of iterations to perform</param>
        /// <returns><i>values</i> eroded <i>iterations</i> times</returns>
        public static float[,] erode(float[,] values, Point size, int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                for (int x = 1; x < size.X - 1; x++)
                {
                    for (int y = 1; y < size.Y - 1; y++)
                    {
                        if (x == y)
                            values[x, y] = Average(values, x - 1, x + 1, y - 1, y + 1);
                        values[x, y] = Average(values, x - 1, x + 1, y - 1, y + 1);
                    }
                }
            }

            return values;
        }

        /// <summary>
        /// Calculates the average of the given rectangle
        /// </summary>
        /// <param name="values">The map to get the average from</param>
        /// <param name="minX">The minimum x of the rectangle</param>
        /// <param name="maxX">The maximum x of the rectangle</param>
        /// <param name="minY">The minimum y of the rectangle</param>
        /// <param name="maxY">The maximum y of the rectangle</param>
        /// <returns>The average of the point between the min and max</returns>
        private static float Average(float[,] values, int minX, int maxX, int minY, int maxY)
        {
            int count = 0;
            float avg = 0;

            for (int x = minX; x < maxX; x++)
            {
                for (int y = minY; y < maxY; y++)
                {
                    avg += values[x, y];
                    count++;
                }
            }

            return avg / count;
        }
    }
}
