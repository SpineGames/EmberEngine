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
        /// Get the yaw pitch and roll from the normal
        /// </summary>
        /// <param name="normal">The normal to calculate the angle from</param>
        /// <returns>{Pitch, Roll, Yaw} all in <b>radians</b></returns>
        public static Vector3 GetYawPitchRoll(Vector3 normal)
        {
            normal.Normalize();

            float pitch = (float)(-(90 * toRad) - Math.Acos(normal.Z));
            float yaw = (float)(Math.Atan2(normal.Y, normal.X) - (90.0F * toRad));

            return new Vector3(pitch, 0, yaw);
        }
    }
}
