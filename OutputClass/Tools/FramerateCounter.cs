using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace EmberEngine.Tools
{
    /// <summary>
    /// Handles calculate the games current framerate
    /// </summary>
    public static class FramerateCounter
    {
        static float fps = 0;

        /// <summary>
        /// Gets the framerate
        /// </summary>
        public static float FPS
        {
            get { return fps; }
        }

        /// <summary>
        /// Handles the FPS counter for the draw event
        /// </summary>
        /// <param name="gameTime">The current GameTime</param>
        public static void OnDraw(GameTime gameTime)
        {
            fps = 1.0F / (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// Gets a multiplier that concides with the given framerate. For example,
        /// if an object moves at speed <i>n</i> at 30 frames per second, it should
        /// move at <i>n * v_25</i> at 25 frames per second.
        /// 
        /// <b>Note!</b>
        /// This function is currently useless untill we get realtime framerate counting
        /// working
        /// </summary>
        /// <param name="preferedFramerate">The framerate to compare against</param>
        /// <returns>what <i>FPS</i> should be multiplied by to get <i>preferedFramerate</i></returns>
        public static double GetMultiplier(int preferedFramerate = 60)
        {
            return 1.0 / (FPS / (double)preferedFramerate);
        }
    }
}
