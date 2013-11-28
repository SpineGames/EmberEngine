using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using EmberEngine.TwoD;
using EmberEngine.TwoD.Instances;
using EmberEngine.Tools.AdvancedMath;

namespace Sample5
{
    /// <summary>
    /// Represents a basic instance that just rotates
    /// </summary>
    public class BasicInstance : Instance2D
    {
        /// <summary>
        /// Creates a new basic instance
        /// </summary>
        /// <param name="sprite">The sprite to use</param>
        /// <param name="pos">The position to begin at</param>
        public BasicInstance(Sprite sprite, Vector2 pos) : base(sprite, pos) 
        {
            RotationSpeed = 1;
        }
    }
}
