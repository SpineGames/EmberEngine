using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace EmberEngine.GUI
{
    public class GUI
    {
        public static GUIStyleV1 Style;
        public static BlendState Multiply;
        public static float TextSpacing = 3.0F;

        public static void Initialize(ContentManager Content, string FontFolder)
        {
            Multiply = new BlendState();
            Multiply.ColorBlendFunction = BlendFunction.Add;
            Multiply.ColorSourceBlend = Blend.DestinationColor;
            Multiply.ColorDestinationBlend = Blend.Zero;
            
            foreach (string path in  Directory.GetFiles(Directory.GetCurrentDirectory() + "/" + Content.RootDirectory + "/" + FontFolder))
            {
                string cPath = FontFolder + "/" + Path.GetFileNameWithoutExtension(path);
                Fonts.Add(Path.GetFileNameWithoutExtension(path),
                    GUIUtils.FixFontSpacing(Content.Load<SpriteFont>(cPath), TextSpacing));
            }

            Style = GUIStyleBuilder.Get(Content.Load<Texture2D>("Common/GUIStyles/template"),
                TextureLayout.V1);
        }

        public static Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();
    }
}
