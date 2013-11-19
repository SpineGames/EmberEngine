using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace EmberEngine.GUI
{
    /// <summary>
    /// Builds a GUI style from a texture sheet
    /// </summary>
    public static class GUIStyleBuilder
    {
        /// <summary>
        /// Gets the GUI style from the texture sheet using the texture layout
        /// </summary>
        /// <param name="Sheet">The texture sheet to use</param>
        /// <param name="SheetLayout">The texture sheet layout to use</param>
        /// <returns>A GUIStyle sheet</returns>
        public static GUIStyleV1 Get(Texture2D Sheet, TextureLayout SheetLayout)
        {
            switch (SheetLayout)
            {
                case TextureLayout.V1:
                    return new GUIStyleV1(ImportTextureV1(Sheet));
                default:
                    throw new NotImplementedException();

            }
        }

        /// <summary>
        /// Imports a dictionary of textures from a V1 texture map
        /// </summary>
        /// <param name="Sheet">The texture atlas to get the textures from</param>
        /// <returns>A dictionairy of texture to be used in a GUI style</returns>
        private static Dictionary<string, Texture2D> ImportTextureV1(Texture2D Sheet)
        {
            Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();

            int Size_8 = Sheet.Width / 8;
            int Size_4 = Sheet.Width / 4;
            int Size_2 = Sheet.Width / 2;

            Textures.Add("ScrollUp", GetFromSheet(
                new Rectangle(0, 0, Size_8, Size_8), Sheet));

            Textures.Add("ScrollDown", GetFromSheet(
                new Rectangle(0, Size_8, Size_8, Size_8), Sheet));

            Textures.Add("ScrollLeft", GetFromSheet(
                new Rectangle(Size_8, 0, Size_8, Size_8), Sheet));

            Textures.Add("ScrollRight", GetFromSheet(
                new Rectangle(Size_8, Size_8, Size_8, Size_8), Sheet));

            Textures.Add("ScrollBarBackVertical", GetFromSheet(
                new Rectangle(0, Size_4, Size_8, Size_4 + Size_2), Sheet));

            Textures.Add("ScrollBarSmallVertical", GetFromSheet(
                new Rectangle(Size_8, Size_4, Size_8, Size_4), Sheet));

            Textures.Add("ScrollBarLargeVertical", GetFromSheet(
                new Rectangle(Size_8, Size_2, Size_8, Size_2), Sheet));

            Textures.Add("ScrollBarBackHorizontal", GetFromSheet(
                new Rectangle(Size_4, 0, Size_4 + Size_2, Size_8), Sheet));

            Textures.Add("ScrollBarSmallHorizontal", GetFromSheet(
                new Rectangle(Size_4, Size_8, Size_4, Size_8), Sheet));

            Textures.Add("ScrollBarLargeHorizontal", GetFromSheet(
                new Rectangle(Size_2, Size_8, Size_2, Size_8), Sheet));

            return Textures;
        }

        /// <summary>
        /// Gets a single texture from the texture sheet
        /// </summary>
        /// <param name="rect">The source rectangle to get</param>
        /// <param name="Texture">The sheet to get the texture from</param>
        /// <returns>The texture retreived from the source texture</returns>
        private static Texture2D GetFromSheet(Rectangle rect, Texture2D Texture)
        {
            int datSize = rect.Width * rect.Height;

            Color[] originalData = new Color[Texture.Width * Texture.Height];
            Texture.GetData<Color>(originalData);

            Texture2D temp = new Texture2D(Texture.GraphicsDevice, rect.Width, rect.Height);
            //The data for part
            Color[] partData = new Color[datSize];

            //Fill the part data with colors from the original texture
            for (int py = 0; py < rect.Height; py++)
            {
                for (int px = 0; px < rect.Width; px++)
                {
                    int partIndex = px + py * rect.Width;
                    //If a part goes outside of the source texture, then fill the overlapping part with Color.Transparent
                    if (rect.Y + py >= Texture.Height || rect.X + px >= Texture.Width)
                        partData[partIndex] = Color.Transparent;
                    else
                        partData[partIndex] = originalData[(rect.X + px) + (rect.Y + py) * Texture.Width];
                }
            }

            //Fill the part with the extracted data
            temp.SetData<Color>(partData);

            return temp;
        }
    }

    /// <summary>
    /// Specifies the texture layout to use
    /// </summary>
    public enum TextureLayout
    {
        V1
    }

    /// <summary>
    /// Represents a style to use for GUI's
    /// </summary>
    public struct GUIStyleV1
    {
        #region Textures
        public readonly Texture2D ScrollUp;
        public readonly Texture2D ScrollDown;
        public readonly Texture2D ScrollLeft;
        public readonly Texture2D ScrollRight;

        public readonly Texture2D ScrollBarBackVertical;
        public readonly Texture2D ScrollBarSmallVertical;
        public readonly Texture2D ScrollBarLargeVertical;

        public readonly Texture2D ScrollBarBackHorizontal;
        public readonly Texture2D ScrollBarSmallHorizontal;
        public readonly Texture2D ScrollBarLargeHorizontal;
        #endregion

        /// <summary>
        /// Creates a new version 1 GUI style from a dictionary passed from the 
        /// GUI style builder
        /// </summary>
        /// <param name="Textures">The dictionary of textures</param>
        public GUIStyleV1(Dictionary<string, Texture2D> Textures)
        {
            this.ScrollUp = Textures["ScrollUp"];
            this.ScrollDown = Textures["ScrollDown"];
            this.ScrollLeft = Textures["ScrollLeft"];
            this.ScrollRight = Textures["ScrollRight"];

            this.ScrollBarBackVertical = Textures["ScrollBarBackVertical"];
            this.ScrollBarSmallVertical = Textures["ScrollBarSmallVertical"];
            this.ScrollBarLargeVertical = Textures["ScrollBarLargeVertical"];

            this.ScrollBarBackHorizontal = Textures["ScrollBarBackHorizontal"];
            this.ScrollBarSmallHorizontal = Textures["ScrollBarSmallHorizontal"];
            this.ScrollBarLargeHorizontal = Textures["ScrollBarLargeHorizontal"];
        }
    }
}
