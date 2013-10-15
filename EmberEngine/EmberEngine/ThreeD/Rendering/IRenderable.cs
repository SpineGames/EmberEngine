using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmberEngine.ThreeD;
using Microsoft.Xna.Framework;

namespace EmberEngine.ThreeD.Rendering
{
    /// <summary>
    /// An interface for renderable classes
    /// </summary>
    public interface IRenderable
    {
        /// <summary>
        /// Gets this renderable's world matrix
        /// </summary>
        Matrix World { get; set; }
          
        /// <summary>
        /// Called when this object should render
        /// </summary>
        /// <param name="camera">The camera to render with</param>
        void Render(ThreeDCamera camera);
    }
}
