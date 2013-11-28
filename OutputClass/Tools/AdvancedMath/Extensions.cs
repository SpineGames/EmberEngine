using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace EmberEngine.Tools.AdvancedMath
{
    /// <summary>
    /// Holds some extension methods for math purposes
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Returns true if either the X or y values are larger than the other
        /// vector's
        /// </summary>
        /// <param name="v1">The first vector to compare</param>
        /// <param name="v2">The second vector to compare</param>
        /// <returns>True if x or y in v1 is greater than the same component in
        /// v2</returns>
        public static bool IsGreater(this Vector2 v1, Vector2 v2)
        {
            return (v1.X > v2.X || v1.Y > v2.Y);
        }

        #region Array
        /// <summary>
        /// Gets the minimum value in this array
        /// </summary>
        /// <param name="vals">The array to search</param>
        /// <returns>The minimum value in the array</returns>
        public static float Min(this float[,] vals)
        {
            float min = float.PositiveInfinity;

            foreach (float f in vals)
            {
                if (f < min)
                    min = f;
            }

            return min;
        }

        /// <summary>
        /// Gets the maximum value in this array
        /// </summary>
        /// <param name="vals">The array to search</param>
        /// <returns>The maximum value in the array</returns>
        public static float Max(this float[,] vals)
        {
            float max = float.NegativeInfinity;

            foreach (float f in vals)
            {
                if (f > max)
                    max = f;
            }

            return max;
        }
        #endregion
    }
}
