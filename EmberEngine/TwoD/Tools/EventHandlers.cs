using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EmberEngine.TwoD.Tools
{
    public delegate void OnUpdateEventHandler(object sender, GameTime GameTime);

    public delegate void OnDrawEventHandler(object sender, SpriteBatch SpriteBatch);
}
