using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using EmberEngine.ThreeD;

namespace EmberEngine.ThreeD.Instancing
{
    public interface IWorldComponent
    {
        void Render(ThreeDCamera camera);

        void Update(GameTime gameTime);

        void Initialize(World world);
    }
}
