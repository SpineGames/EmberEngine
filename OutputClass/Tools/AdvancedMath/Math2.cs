using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace EmberEngine.Tools.AdvancedMath
{
    /// <summary>
    /// Contains some advanced math functions
    /// </summary>
    public static class Math2
    {
        const double toRad = Math.PI / 180.00;
        const double toDeg = 180.00 * Math.PI;

        const float toRadF = (float)(Math.PI / 180.00);
        const float toDegF = (float)(180.00 * Math.PI);
        
        /// <summary>
        /// Gets this degree as radians
        /// </summary>
        /// <returns>this double in radians</returns>
        public static double ToRad(this double degrees)
        {
            return degrees * toRad;
        }

        /// <summary>
        /// Gets this radian as a degree
        /// </summary>
        /// <returns>degrees in this radian</returns>
        public static double ToDeg(this double radians)
        {
            return radians * toDeg;
        }

        /// <summary>
        /// Gets this degree as radians
        /// </summary>
        /// <returns>this double in radians</returns>
        public static float ToRad(this float degrees)
        {
            return degrees * toRadF;
        }

        /// <summary>
        /// Gets this radian as a degree
        /// </summary>
        /// <returns>degrees in this radian</returns>
        public static float ToDeg(this float radians)
        {
            return radians * toDegF;
        }

        /// <summary>
        /// Wraps the given value between a min and a max
        /// </summary>
        /// <param name="min">The minimum value to wrap to</param>
        /// <param name="max">The maximum value to wrap to</param>
        /// <param name="val">The value to wrap</param>
        /// <returns><i>val</i> wrapped between <i>max</i> and <i>min</i></returns>
        public static double Wrap(this double val, double min, double max)
        {
            double Min = Math.Min(min, max);
            double Max = Math.Max(min, max);

            double range = Max - Min;

            if (range == 0)
                return min;

            while (val < Min)
                val += range;
            while (val > Max)
                val -= range;

            return val;
        }

        /// <summary>
        /// Wraps the given value between a min and a max
        /// </summary>
        /// <param name="min">The minimum value to wrap to</param>
        /// <param name="max">The maximum value to wrap to</param>
        /// <param name="val">The value to wrap</param>
        /// <returns><i>val</i> wrapped between <i>max</i> and <i>min</i></returns>
        public static float Wrap(this float val, float min, float max)
        {
            float Min = Math.Min(min, max);
            float Max = Math.Max(min, max);

            float range = Max - Min;

            if (range == 0)
                return min;

            while (val < Min)
                val += range;
            while (val > Max)
                val -= range;

            return val;
        }

        /// <summary>
        /// Wraps the given value between a min and a max
        /// </summary>
        /// <param name="min">The minimum value to wrap to</param>
        /// <param name="max">The maximum value to wrap to</param>
        /// <param name="val">The value to wrap</param>
        /// <returns><i>val</i> wrapped between <i>max</i> and <i>min</i></returns>
        public static int Wrap(this int val, int min, int max)
        {
            int Min = Math.Min(min, max);
            int Max = Math.Max(min, max);

            int range = Max - Min;

            if (range == 0)
                return min;

            while (val < Min)
                val += range;
            while (val > Max)
                val -= range;

            return val;
        }

        /// <summary>
        /// Gets the change in x over the given length from an angle
        /// </summary>
        /// <param name="angle">The angle in <b>degrees</b></param>
        /// <param name="length">The length of the arm</param>
        /// <returns>The change in x over <i>length</i></returns>
        public static double LengthdirX(double angle, double length)
        {
            return length * Math.Cos(ToRad(angle));
        }

        /// <summary>
        /// Gets the change in y over the given length from an angle
        /// </summary>
        /// <param name="angle">The angle in <b>degrees</b></param>
        /// <param name="length">The length of the arm</param>
        /// <returns>The change in y over <i>length</i></returns>
        public static double LengthdirY(double angle, double length)
        {
            return length * Math.Sin(ToRad(angle));
        }

        /// <summary>
        /// Gets the change in z over the given length from an angle
        /// </summary>
        /// <param name="angle">The angle in <b>degrees</b></param>
        /// <param name="length">The length of the arm</param>
        /// <returns>The change in z over <i>length</i></returns>
        public static double LengthdirZ(double angle, double length)
        {
            return length * Math.Sin(angle);
        }

        /// <summary>
        /// Gets the change in x and y for the given angle and length
        /// </summary>
        /// <param name="angle">The angle in <b>degrees</b></param>
        /// <param name="length">The length of the terminal arm</param>
        /// <returns>A vector2 representing the change in x over the change in y</returns>
        public static Vector2 Lengthdir(double angle, double length)
        {
            return new Vector2((float)LengthdirX(angle, length), (float)LengthdirY(angle, length));
        }

        /// <summary>
        /// Gets the texture co-ord for a sphere
        /// </summary>
        /// <param name="yaw">The yaw from centre</param>
        /// <param name="pitch">The pitch from centre</param>
        /// <param name="length">The radius</param>
        /// <param name="texTile">The number of times to tile the texture</param>
        /// <returns>The tex co-ords for the given spot on the sphere</returns>
        public static Vector2 GetUVForSphere(double yaw, double pitch, double length, float texTile = 1)
        {
            Vector3 Normal = GetFromYawPitch(yaw, pitch, length);
            
            float x = MathHelper.Lerp(0.0F, texTile, (float)(yaw / 360.0)); ///0.5F + (float)(Math.Atan2(Normal.Y, Normal.X) / (Math.PI * 2)) * texTile;
            float y = MathHelper.Lerp(0.0F, texTile, (float)(pitch / 360.0)); ///0.5F - (float)(Math.Asin(Normal.Y) / Math.PI) * texTile;

            return new Vector2(x, y);
        }

        /// <summary>
        /// Gets a Vector3 from yaw, pitch, and roll
        /// </summary>
        /// <param name="yaw">The yaw to use in <b>degrees</b></param>
        /// <param name="pitch">The pitch to use int <b>degrees</b></param>
        /// <param name="length">The length of the vector</param>
        /// <returns>A vector from the given yaw/pitch/length</returns>
        public static Vector3 GetFromYawPitch(double yaw, double pitch, double length)
        {
            yaw = ToRad(yaw);
            pitch = ToRad(pitch);

            return new Vector3(
                (float)((Math.Sin(pitch) * Math.Cos(yaw)) * length),
                (float)((Math.Sin(pitch) * Math.Sin(yaw)) * length),
                (float)((Math.Cos(pitch)) * length));
        }

        /// <summary>
        /// Get the yaw pitch and roll from the normal in <b>radians</b>
        /// </summary>
        /// <param name="normal">The normal to calculate the angle from</param>
        /// <returns>{Pitch, Roll, Yaw} all in <b>radians</b></returns>
        public static Vector3 GetPitchRollYaw(Vector3 normal)
        {
            normal.Normalize();

            float pitch = (float)(-(90 * toRad) - Math.Acos(normal.Z));
            float yaw = (float)(Math.Atan2(normal.Y, normal.X) - (90.0F * toRad));

            return new Vector3(pitch, 0, yaw);
        }
    }
}
