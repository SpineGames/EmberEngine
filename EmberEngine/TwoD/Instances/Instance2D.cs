using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using EmberEngine.Tools.AdvancedMath;
using Microsoft.Xna.Framework.Graphics;
using EmberEngine.TwoD.Tools;
using EmberEngine.Tools.Input;

namespace EmberEngine.TwoD.Instances
{
    /// <summary>
    /// Represents an instance that moves around the world and has a sprite
    /// </summary>
    public abstract class Instance2D
    {
        /// <summary>
        /// The target elapsed time between update ticks
        /// </summary>
        public static int TargetElapsedTime = 1000 / 60;

        int elapsedTime;
        Vector2 position;
        float direction;
        float speed = 0;
        float rotationSpeed = 0;
        float friction = 1;
        Sprite sprite;

        /// <summary>
        /// Gets or sets the position of this instnace
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        /// <summary>
        /// Gets or sets the direction of this instance in <b>degrees</b>
        /// 0 is positive x axis
        /// </summary>
        public float Direction
        {
            get { return direction; }
            set { direction = value.Wrap(0, 360); }
        }
        /// <summary>
        /// Gets or sets the speed of this instance (default 0)
        /// </summary>
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        /// <summary>
        /// Gets or sets the rotation speed of this instance in Degrees per Tick (default 0)
        /// </summary>
        public float RotationSpeed
        {
            get { return rotationSpeed; }
            set { rotationSpeed = value; }
        }
        /// <summary>
        /// Gets or sets the friction for this instance (default 1)
        /// </summary>
        public float Friction
        {
            get { return friction; }
            set
            {
                friction = value > 0 ? value : 0;
            }
        }
        /// <summary>
        /// Gets the instances sprite
        /// </summary>
        public Sprite Sprite
        {
            get { return sprite; }
        }
        /// <summary>
        /// Gets or sets the tag for this object
        /// </summary>
        public object Tag { get; set; }
        /// <summary>
        /// Gets or sets the input key manager for this instance
        /// </summary>
        public KeyManager InputKeys = new KeyManager();

        /// <summary>
        /// Gets or sets the ID for this instance, should ONLY be called by the world class
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Invoked when this instance updates
        /// </summary>
        public OnUpdateEventHandler OnUpdate;
        /// <summary>
        /// Invoked when this instance draws
        /// </summary>
        public OnDrawEventHandler OnDraw;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="sprite">The sprite to draw with, or null</param>
        /// <param name="Position">The instance's initial positon</param>
        public Instance2D(Sprite sprite, Vector2 Position)
        {
            this.sprite = sprite;
            this.Position = Position;
        }

        /// <summary>
        /// Updates this instance
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public virtual void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);

            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (OnUpdate != null)
                OnUpdate.Invoke(this, gameTime);

            if (elapsedTime >= TargetElapsedTime)
            {
                position += Math2.Lengthdir(Direction, Speed);
                Direction += RotationSpeed;

                speed /= friction;

                sprite.Rotation = Direction;

                elapsedTime = 0;

                InputKeys.Update();
            }
        }

        /// <summary>
        /// Draws this instance
        /// </summary>
        /// <param name="SpriteBatch">The spritebatch to draw with</param>
        public virtual void Draw(SpriteBatch SpriteBatch, Vector2 cameraPos)
        {
            sprite.Draw(SpriteBatch, Position + cameraPos);

            if (OnDraw != null)
                OnDraw.Invoke(this, SpriteBatch);
        }

        /// <summary>
        /// Called when this instance should be destroyed
        /// </summary>
        public virtual void Destroy()
        {

        }
    }
}
