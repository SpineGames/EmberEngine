using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpineLibraryTestGame.SpineLibrary.ThreeD.CollisionDetection
{
    public interface ICollidable
    {
        bool IsColliding(Vector3 Point);
    }
}
