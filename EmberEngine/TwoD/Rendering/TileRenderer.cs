using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;

namespace EmberEngine.TwoD.Rendering
{
    /// <summary>
    /// Represents a set of tiles that can be rendered
    /// </summary>
    public class TileRenderer
    {
        Texture2D tileSheet;
        int xTiles;
        int yTiles;
        int tileWidth;
        int tileHeight;

        /// <summary>
        /// Gets or sets the scale for each tile
        /// </summary>
        public Vector2 Scale = Vector2.One;

        /// <summary>
        /// Gets the width of each individual tile
        /// </summary>
        public int TileWidth
        {
            get { return tileWidth; }
        }
        /// <summary>
        /// Gets the height of each individual tile
        /// </summary>
        public int TileHeight
        {
            get { return tileHeight; }
        }

        /// <summary>
        /// Gets the number of tiles along the X-axis
        /// </summary>
        public int XTiles
        {
            get { return xTiles; }
        }
        /// <summary>
        /// Gets the number of tiles along the Y-axis
        /// </summary>
        public int YTiles
        {
            get { return yTiles; }
        }

        /// <summary>
        /// Creates a new tile renderer
        /// </summary>
        /// <param name="tileSheet">The tile sheet to draw from</param>
        /// <param name="xTiles">The number of tiles along the x axis</param>
        /// <param name="yTiles">The number of tiles along the y axis</param>
        public TileRenderer(Texture2D tileSheet, int xTiles, int yTiles)
        {
            this.tileSheet = tileSheet;

            this.xTiles = xTiles;
            this.yTiles = yTiles;

            this.tileWidth = tileSheet.Width / xTiles;
            this.tileHeight= tileSheet.Height / yTiles;
        }

        /// <summary>
        /// Draws a tile from this tile renderer to the position
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw with</param>
        /// <param name="xTile">The tile index of the tile</param>
        /// <param name="TopLeft">The top left corner of the tile</param>
        /// <param name="depth">The depth of the tile</param>
        public void Draw(SpriteBatch spriteBatch, int tileID, Vector2 TopLeft, float depth)
        {
            int yTile = tileID / yTiles;
            int xTile = tileID % yTiles;

            spriteBatch.Draw(tileSheet, TopLeft,
                new Rectangle(xTile * tileWidth, yTile * tileHeight, tileWidth, tileHeight), Color.White, 0F,
                Vector2.Zero, Scale, SpriteEffects.None, depth);
        }

        /// <summary>
        /// Draws a tile from this tile renderer to the position
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw with</param>
        /// <param name="xTile">The x index of the tile</param>
        /// <param name="yTile">The y index of the tile</param>
        /// <param name="TopLeft">The top left corner of the tile</param>
        /// <param name="depth">The depth of the tile</param>
        public void Draw(SpriteBatch spriteBatch, int xTile, int yTile, Vector2 TopLeft, float depth)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(tileSheet, TopLeft,
                new Rectangle(xTile * tileWidth, xTile * tileHeight, tileWidth, tileHeight), Color.White, 0F,
                Vector2.Zero, 1F, SpriteEffects.None, depth);
            spriteBatch.End();
        }

        /// <summary>
        /// Writes this TileRenderer to the stream
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        public void WriteToStream(Stream stream)
        {
            BinaryWriter w = new BinaryWriter(stream);

            w.Write(tileSheet.Width);
            w.Write(tileSheet.Height);

            w.Write(xTiles);
            w.Write(yTiles);
            
            Color[] colorData = new Color[tileSheet.Width * tileSheet.Height];
            tileSheet.GetData<Color>(colorData);

            for (int y = 0; y < tileSheet.Height; y++)
            {
                for (int x = 0; x < tileSheet.Width; x++)
                {
                    w.Write(colorData[x + (y * tileSheet.Height)].A);
                    w.Write(colorData[x + (y * tileSheet.Height)].R);
                    w.Write(colorData[x + (y * tileSheet.Height)].G);
                    w.Write(colorData[x + (y * tileSheet.Height)].B);
                }
            }
        }

        /// <summary>
        /// Reads a tile renderer from the stream
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        /// <param name="Graphics">The graphics device to bind the texture to</param>
        /// <returns>A TileRenderer loaded from stream</returns>
        public static TileRenderer ReadFromStream(Stream stream, GraphicsDevice Graphics)
        {
            BinaryReader r = new BinaryReader(stream);

            int iWidth = r.ReadInt32();
            int iHeight = r.ReadInt32();

            int xTiles = r.ReadInt32();
            int yTiles = r.ReadInt32();

            Color[] colorData = new Color[iWidth * iHeight];

            for (int y = 0; y < iHeight; y++)
            {
                for (int x = 0; x < iWidth; x++)
                {
                    byte A = r.ReadByte();
                    byte R = r.ReadByte();
                    byte G = r.ReadByte();
                    byte B = r.ReadByte();

                    colorData[x + (y * iHeight)] = new Color(R, G, B, A);
                }
            }
            Texture2D texture = new Texture2D(Graphics, iWidth, iHeight);

            texture.SetData<Color>(colorData);

            return new TileRenderer(texture, xTiles, yTiles);
        }
    }
}
