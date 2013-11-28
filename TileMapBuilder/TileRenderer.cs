using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace EmberEngine.TwoD.Rendering
{
    /// <summary>
    /// Represents a set of tiles that can be rendered
    /// </summary>
    public class TileRenderer
    {
        Image tileSheet;
        int xTiles;
        int yTiles;
        int tileWidth;
        int tileHeight;

        public PointF Scale = new PointF(1,1);

        public int TileWidth
        {
            get { return tileWidth; }
        }
        public int TileHeight
        {
            get { return tileHeight; }
        }
        public int XTiles
        {
            get { return xTiles; }
        }
        public int YTiles
        {
            get { return yTiles; }
        }

        public TileRenderer(Image tileSheet, int xTiles, int yTiles)
        {
            this.tileSheet = tileSheet;

            this.xTiles = xTiles;
            this.yTiles = yTiles;

            this.tileWidth = tileSheet.Width / xTiles;
            this.tileHeight= tileSheet.Height / yTiles;
        }

        private void FillImage(int TileID)
        {
        }

        /// <summary>
        /// Gets the image from the given tile ID
        /// </summary>
        /// <param name="TileID"></param>
        /// <returns></returns>
        public Image GetImage(int TileID)
        {
            try
            {
                int yTile = TileID / yTiles;
                int xTile = TileID % yTiles;

                RectangleF srcRect = new RectangleF(xTile * tileWidth, yTile * tileHeight, tileWidth, tileHeight);
                return ((Bitmap)tileSheet).Clone(srcRect, tileSheet.PixelFormat);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Draws a tile from this tile renderer to the position
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw with</param>
        /// <param name="xTile">The tile index of the tile</param>
        /// <param name="TopLeft">The top left corner of the tile</param>
        /// <param name="depth">The depth of the tile</param>
        public void Draw(PaintEventArgs args, int tileID, PointF TopLeft, float depth)
        {
            int yTile = tileID / yTiles;
            int xTile = tileID % yTiles;

            args.Graphics.DrawImage(tileSheet, 
                new Rectangle((int)(TopLeft.X), (int)(TopLeft.Y), 
                    (int)Math.Round(tileWidth * Scale.X), (int)Math.Round(tileHeight * Scale.Y)),
                xTile * tileWidth, yTile * tileHeight, tileWidth, tileHeight, 
                GraphicsUnit.Pixel);
        }
    }
}
