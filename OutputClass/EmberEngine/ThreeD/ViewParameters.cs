using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpineLibraryTestGame.SpineLibrary.ThreeD
{
    /// <summary>
    /// Represents a camera's view paramaters
    /// </summary>
    public struct ViewParameters
    {
        /// <summary>
        /// The view matrix
        /// </summary>
        public Matrix View;
        /// <summary>
        /// The projection matrix
        /// </summary>
        public Matrix Projection;
        /// <summary>
        /// The world matrix
        /// </summary>
        public Matrix World;
    }
}
