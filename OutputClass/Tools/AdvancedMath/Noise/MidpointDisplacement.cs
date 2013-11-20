using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmberEngine.Tools.AdvancedMath.Noise
{
    /// <summary>
    /// Used to generate midpoint displacement maps
    /// </summary>
    public static class MidpointDisplacement
    {
        /// <summary>
        /// Uses the midpoint displacement algorithm to return
        /// an array of floats
        /// </summary>
        /// <param name="h">The smoothing level to use (higher = rougher)</param>
        /// <param name="baseVal">The base value to generate around</param>
        /// <param name="length">The length of the array (must be a multiple of 2)</param>
        /// <param name="seed">The seed to use, or -1 for a random seed</param>
        /// <returns>A integer heightmap array</returns>
        public static float[] MidpointDisplacement2D(int h, int baseVal, int length, int seed = -1)
        {
            float[] output = new float[length + 1];
            Random RandNum;

            if (seed == -1)
                RandNum = new Random();
            else
                RandNum = new Random(seed);


            for (int xx = 0; xx <= length; xx++)
            {
                output[xx] = baseVal;
            }
            output[0] = baseVal;

            //generate values
            for (int rep = 2; rep < length; rep *= 2)
            {
                for (int i = 1; i <= rep; i += 1)
                {

                    int x1 = (length / rep) * (i - 1);
                    int x2 = (length / rep) * i;
                    float avg = (output[x1] + output[x2]) / 2;
                    int Rand = RandNum.Next(-h, h);
                    output[(x1 + x2) / 2] = avg + (Rand);
                }
                h /= 2;
            }

            //returns array
            return output;
        }

        /// <summary>
        /// Uses the midpoint displacement algorithm to return
        /// an array of doubles
        /// </summary>
        /// <param name="h">The smoothing level to use (higher = rougher)</param>
        /// <param name="baseVal">The base value to generate around</param>
        /// <param name="length">The length of the array (must be a multiple of 2)</param>
        /// <param name="seed">The seed to use, or -1 for a random seed</param>
        /// <returns>A integer heightmap array</returns>
        public static double[] MidpointDisplacement2D_D(int h, int baseVal, int length, int seed = -1)
        {
            double[] output = new double[length + 1];
            Random RandNum;

            if (seed == -1)
                RandNum = new Random();
            else
                RandNum = new Random(seed);

            for (int xx = 0; xx <= length; xx++)
            {
                output[xx] = baseVal;
            }
            output[0] = baseVal;

            //generate values
            for (int rep = 2; rep < length; rep *= 2)
            {
                for (int i = 1; i <= rep; i += 1)
                {

                    int x1 = (length / rep) * (i - 1);
                    int x2 = (length / rep) * i;
                    double avg = (output[x1] + output[x2]) / 2;
                    int Rand = RandNum.Next(-h, h);
                    output[(x1 + x2) / 2] = avg + (Rand);
                }
                h /= 2;
            }

            //returns array
            return output;
        }
    }
}
