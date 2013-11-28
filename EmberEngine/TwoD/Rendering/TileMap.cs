using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;

namespace EmberEngine.TwoD.Rendering
{
    public class TileMap
    {
        TileRenderer renderer;

        int width;
        int height;
        int[,] tileIDs;

        Vector2 position;

        private bool ShouldInvalidate = true;
        RenderTarget2D renderTarget;

        float depth;
        Color backgroundColor = Color.Black;

        /// <summary>
        /// Gets or sets the scale of the tile sheet
        /// </summary>
        public Vector2 Scale
        {
            get { return renderer.Scale; }
            set { renderer.Scale = value; }
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public float Depth
        {
            get { return depth; }
            set { depth = value; }
        }
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        public TileMap(TileRenderer baseRenderer, int width, int height, Vector2 position, float depth)
        {
            tileIDs = new int[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tileIDs[x, y] = -1;
                }
            }

            this.width = width;
            this.height = height;
            this.renderer = baseRenderer;
            this.position = position;
            this.depth = depth;
        }

        /// <summary>
        /// Sets a tile in this tile map
        /// </summary>
        /// <param name="x">The x co-ord of the tile</param>
        /// <param name="y">The y co-ord of the tile</param>
        /// <param name="tileID">The tile ID, or -1 for no tile</param>
        public void SetTile(int x, int y, int tileID)
        {
            if (x >= 0 & x < width & y >= 0 & y < height)
            {
                tileIDs[x, y] = tileID;
                ShouldInvalidate = true;
            }
        }

        /// <summary>
        /// Draws this tile map
        /// </summary>
        /// <param name="SpriteBatch">The spritebatch to draw with</param>
        public void Draw(SpriteBatch SpriteBatch, Vector2 viewPos)
        {
            if (ShouldInvalidate)
            {
                renderTarget = new RenderTarget2D(SpriteBatch.GraphicsDevice, 
                    width * renderer.TileWidth, height * renderer.TileHeight);

                SpriteBatch.GraphicsDevice.SetRenderTarget(renderTarget);
                SpriteBatch.GraphicsDevice.Clear(backgroundColor);

                SpriteBatch.Begin();
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (tileIDs[x, y] != -1)
                            renderer.Draw(SpriteBatch, tileIDs[x, y],
                                position + (new Vector2(x * renderer.TileWidth, y * renderer.TileHeight) * Scale), 
                                depth);
                    }
                }
                SpriteBatch.End();

                SpriteBatch.GraphicsDevice.SetRenderTarget(null);
                ShouldInvalidate = false;
            }

            SpriteBatch.Begin();
            SpriteBatch.Draw(renderTarget, position + viewPos, Color.White);
            SpriteBatch.End();
        }

        /// <summary>
        /// Saves this tile map to a new or existing file
        /// </summary>
        /// <param name="fileName">The name of the file to save to</param>
        public void Save(string fileName)
        {
            Stream s = File.OpenWrite(fileName);

            WriteToStream(s);

            s.Dispose();
        }

        /// <summary>
        /// Loads a tile map from an existing file
        /// </summary>
        /// <param name="fileName">The name of the file to load from</param>
        /// <param name="renderer">The Tile Renderer to use</param>
        /// <returns>A tile map loaded from the file</returns>
        public static TileMap Load(string fileName, TileRenderer renderer)
        {
            Stream s = File.OpenRead(fileName);

            TileMap ret = ReadFromStream(s, renderer);

            s.Dispose();

            return ret;
        }

        /// <summary>
        /// Writes this tile map to a stream
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        public void WriteToStream(Stream stream)
        {
            BinaryWriter w = new BinaryWriter(stream);

            w.Write(position.X);
            w.Write(position.Y);

            w.Write(depth);

            w.Write(width);
            w.Write(height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    w.Write(tileIDs[x, y]);
                }
            }
        }

        /// <summary>
        /// Reads a tile map from a stream
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        /// <param name="renderer">The renderer to render with</param>
        /// <returns>A tilemap loaded from the stream</returns>
        public static TileMap ReadFromStream(Stream stream, TileRenderer renderer)
        {
            BinaryReader r = new BinaryReader(stream);

            Vector2 Position = new Vector2(r.ReadSingle(), r.ReadSingle());

            float depth = r.ReadSingle();

            int width = r.ReadInt32();
            int height = r.ReadInt32();

            TileMap temp = new TileMap(renderer, width, height, Position, depth);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    temp.SetTile(x, y, r.ReadInt32());
                }
            }

            return temp;
        }
    }
}
