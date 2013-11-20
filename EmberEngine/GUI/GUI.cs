using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace EmberEngine.GUI
{
    /// <summary>
    /// Represents a class holding important GUI stuff
    /// </summary>
    public class GUI
    {
        /// <summary>
        /// The GUI style to use
        /// </summary>
        public static GUIStyleV1 Style;
        /// <summary>
        /// A blend state that multiplies the source and destination
        /// </summary>
        public static BlendState Multiply;
        /// <summary>
        /// The spacing in pixels between lines of font
        /// </summary>
        public static float TextSpacing = 3.0F;
        /// <summary>
        /// A dictionary of all loaded fonts
        /// </summary>
        public static Dictionary<string, SpriteFont> Fonts 
            = new Dictionary<string, SpriteFont>();

        /// <summary>
        /// Initializes the variables
        /// </summary>
        /// <param name="Content">The content manager to load from</param>
        /// <param name="FontFolder">The path to the local font folder</param>
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
    }
}
