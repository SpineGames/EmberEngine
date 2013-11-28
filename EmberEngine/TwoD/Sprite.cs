using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using EmberEngine.Tools.AdvancedMath;

namespace EmberEngine.TwoD
{
    /// <summary>
    /// Represents an object that can draw an animated sprite
    /// </summary>
    public class Sprite
    {
        static Random Random = new Random();

        #region private variables
        Texture2D sourceSheet;
        
        Vector2 orgin = new Vector2(0,0);
        Vector2 scale = new Vector2(1);
        Color color = Color.White;
        SpriteEffects spriteEffects = SpriteEffects.None;

        int hSprites;
        int vSprites;

        int spriteWidth;
        int spriteHeight;

        int xFrame;
        int yFrame;

        float radRotation;
        float depth = 0;

        int elapsedTime = 0;
        int targetElapsedTime = 1000;
        #endregion

        #region Public Variables
        /// <summary>
        /// Gets the source texture sheet
        /// </summary>
        public Texture2D SourceSheet
        {
            get { return sourceSheet; }
        }

        /// <summary>
        /// Gets or sets the orgin of the sprite
        /// </summary>
        public Vector2 Orgin
        {
            get { return orgin; }
            set { orgin = value; }
        }
        /// <summary>
        /// Gets or sets the scale of the sprite
        /// </summary>
        public Vector2 Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                //orgin *= scale;
            }
        }
        /// <summary>
        /// Gets or sets the color multiplier. Default white
        /// </summary>
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
        /// <summary>
        /// Gets or sets the sprite effects. Default SpriteEffects.None
        /// </summary>
        public SpriteEffects SpriteEffects
        {
            get { return spriteEffects; }
            set { spriteEffects = value; }
        }
        /// <summary>
        /// Gets or sets whether the sprite should shake
        /// </summary>
        public bool ShouldShake { get; set; }
        /// <summary>
        /// Gets or sets the amount in pixels to shake by
        /// </summary>
        public int ShakeAmount { get; set; }

        /// <summary>
        /// Gets the number of sprites along the x axis
        /// </summary>
        public int HSprites
        {
            get { return hSprites; }
        }
        /// <summary>
        /// Gets the number of sprites along the y axis
        /// </summary>
        public int VSprites
        {
            get { return vSprites; }
        }

        /// <summary>
        /// Gets the width of the sprite in the source rectangle
        /// </summary>
        public int SpriteWidth
        {
            get { return spriteWidth; }
        }
        /// <summary>
        /// Gets the height of the sprite in the source rectangle
        /// </summary>
        public int SpriteHeight
        {
            get { return spriteHeight; }
        }

        /// <summary>
        /// Gets the x - index for the current frame
        /// </summary>
        public int XFrame
        {
            get { return xFrame; }
            set 
            {
                xFrame = value.Wrap(0, HSprites - 1); 
            }
        }
        /// <summary>
        /// Gets the y - index for the current frame
        /// </summary>
        public int YFrame
        {
            get { return yFrame; }
            set
            {
                yFrame = value.Wrap(0, VSprites - 1);
            }
        }

        /// <summary>
        /// Gets or sets the rotation in 3D
        /// </summary>
        public float Rotation
        {
            get { return radRotation.ToDeg(); }
            set { radRotation = value.ToRad(); }
        }
        /// <summary>
        /// Gets or sets the sprite depth
        /// </summary>
        public float Depth
        {
            get { return depth; }
            set { depth = value; }
        }
        #endregion

        /// <summary>
        /// Creates a new sprite
        /// </summary>
        /// <param name="SourceSheet">The source texture atlas to use</param>
        /// <param name="hSprites">The number of sprites along the x axis in the image</param>
        /// <param name="vSprites">The number of sprites along the y axis in the image</param>
        /// <param name="frameRate">The target tie between frames</param>
        public Sprite(Texture2D SourceSheet, int hSprites, int vSprites, TimeSpan frameRate)
        {
            this.sourceSheet = SourceSheet;
            this.hSprites = hSprites;
            this.vSprites = vSprites;

            this.spriteWidth = sourceSheet.Width / hSprites;
            this.spriteHeight = sourceSheet.Height / vSprites;

            this.Orgin = new Vector2(spriteWidth / 2, spriteHeight / 2);

            this.targetElapsedTime = (int)frameRate.TotalMilliseconds;
        }

        /// <summary>
        /// Creates a new sprite
        /// </summary>
        /// <param name="SourceSheet">The source texture atlas to use</param>
        /// <param name="hSprites">The number of sprites along the x axis in the image</param>
        /// <param name="vSprites">The number of sprites along the y axis in the image</param>
        /// <param name="Orgin">The orgin within the sprite, defaults to the centre of the sprite</param>
        /// <param name="frameRate">The target tie between frames</param>
        public Sprite(Texture2D SourceSheet, int hSprites, int vSprites, Vector2 Orgin, TimeSpan frameRate)
        {
            this.sourceSheet = SourceSheet;
            this.hSprites = hSprites;
            this.vSprites = vSprites;

            this.spriteWidth = sourceSheet.Width / hSprites;
            this.spriteHeight = sourceSheet.Height / vSprites;

            this.orgin = Orgin;

            this.targetElapsedTime = (int)frameRate.TotalMilliseconds;
        }

        /// <summary>
        /// Updates this sprite
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public void Update(GameTime gameTime)
        {
            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedTime >= targetElapsedTime)
            {
                elapsedTime = 0;
                XFrame ++;
            }            
        }

        /// <summary>
        /// Draws this sprite at the given position
        /// </summary>
        /// <param name="SpriteBatch">The spritebatch to draw with</param>
        /// <param name="Position">The position to draw at</param>
        public void Draw(SpriteBatch SpriteBatch, Vector2 Position)
        {
            if (ShouldShake)
            {
                Position.X += Random.Next(-ShakeAmount, ShakeAmount);
                Position.Y += Random.Next(-ShakeAmount, ShakeAmount);
            }

            SpriteBatch.Begin();
            SpriteBatch.Draw(sourceSheet, Position,
                new Rectangle(spriteWidth * xFrame, spriteHeight * yFrame, spriteWidth, spriteHeight), color,
                radRotation, orgin, scale, spriteEffects, depth);
            SpriteBatch.End();

            AdvancedRendering.DrawCrossHairs(Position, 5, Color.Red, SpriteBatch.GraphicsDevice);
        }

        /// <summary>
        /// Creates a clone of this sprite
        /// </summary>
        /// <returns>A sprite identical to this one</returns>
        public Sprite Clone()
        {
            Sprite temp = new Sprite(SourceSheet, HSprites, VSprites, Orgin, TimeSpan.FromMilliseconds(targetElapsedTime));
            temp.XFrame = XFrame;
            temp.YFrame = YFrame;
            temp.Color = Color;
            temp.Scale = Scale;
            temp.SpriteEffects = SpriteEffects;
            temp.radRotation = radRotation;
            temp.ShouldShake = ShouldShake;
            temp.ShakeAmount = ShakeAmount;

            return temp;
        }
    }
}
