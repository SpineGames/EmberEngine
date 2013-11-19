using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace EmberEngine.Tools.Management
{
    public static class FontManager
    {
        public static Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();

        public static void LoadFont(ContentManager c, string name)
        {
            Fonts.Add(Path.GetFileName(name), c.Load<SpriteFont>(name));
        }

        public static float GetWidth(string font, string text)
        {
            if (Fonts.ContainsKey(font))
                return Fonts[font].MeasureString(text).X;
            return -1;
        }

        public static float GetHeight(string font, string text)
        {
            if (Fonts.ContainsKey(font))
                return Fonts[font].MeasureString(text).Y;
            return -1;
        }

    }
}
