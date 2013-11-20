using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using EmberEngine.ThreeD.Instancing;
using EmberEngine.ThreeD;
using EmberEngine.ThreeD.Rendering;
using EmberEngine.Tools.AdvancedMath;
using EmberEngine.Tools;

namespace EmberEngine.ThreeD.Instancing
{
    /// <summary>
    /// Represents a 3D instance
    /// </summary>
    public class Instance : IWorldComponent
    {
        /// <summary>
        /// Gets or sets this instance's Position
        /// </summary>
        public Vector3 Position;
        /// <summary>
        /// Gets or sets the normal for this instance's velocity
        /// </summary>
        public Vector3 VelocityNormal;
        /// <summary>
        /// Gets or sets this instance's speed
        /// </summary>
        public float Velocity;
        /// <summary>
        /// Gets or sets this instance's rotation around each axis in <b>RADIANS</b>
        /// </summary>
        public Vector3 Rotation;
        /// <summary>
        /// Gets or set's this instaces rotation speed around each axis in <b>RADIANS</b>
        /// </summary>
        public Vector3 RotationSpeed;
        /// <summary>
        /// Gets or sets this instance's Renderer
        /// </summary>
        public IRenderable Renderer;
        private World world;
        /// <summary>
        /// Gets the world that this instance exists in
        /// </summary>
        public World World
        {
            get { return world; }
        }

        /// <summary>
        /// Called when this instance is updated
        /// </summary>
        public UpdateEventHandler UpdateEvent;
        /// <summary>
        /// Called when this instance is rendered
        /// </summary>
        public RenderEventHandler RenderEvent;
        /// <summary>
        /// Invoked when the tag object is changed
        /// </summary>
        public CustomEventHandler TagChanged;

        private int id;
        /// <summary>
        /// Gets this instance's world ID
        /// </summary>
        public int ID
        {
            get { return id; }
        }

        private object tag;
        /// <summary>
        /// Gets or sets the tag associated with this instance
        /// </summary>
        public object Tag
        {
            get { return tag; }
            set
            {
                tag = value;

                if (TagChanged != null)
                    TagChanged.Invoke(this, new CustomEventArgs(tag));
            }
        }
        
        public Instance(Vector3 Position)
        {
            this.Position = Position;
        }

        /// <summary>
        /// Initializes this intance and adds it to the world
        /// </summary>
        /// <param name="world">The world to add the instance to</param>
        public void Initialize(World world)
        {
            this.world = world;
            id = world.AddInstance(this);
        }

        /// <summary>
        /// Updates this instance
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public void Update(GameTime gameTime)
        {
            if (UpdateEvent != null)
                UpdateEvent.Invoke(new UpdateEventArgs(World, this, gameTime));

            Rotation += RotationSpeed;
            Rotation.X = (float)Math2.Wrap(0, Math.PI * 2, Rotation.X);
            Rotation.Y = (float)Math2.Wrap(0, Math.PI * 2, Rotation.Y);
            Rotation.Z = (float)Math2.Wrap(0, Math.PI * 2, Rotation.Z);
            
            Position += VelocityNormal * Velocity;
        }

        /// <summary>
        /// Renders this instance
        /// </summary>
        /// <param name="camera">The camera to render with</param>
        public void Render(ThreeDCamera camera)
        {
            if (RenderEvent != null)
                RenderEvent.Invoke(new RenderEventArgs(World, ref World.instances[ID], camera));

            Renderer.World =
                Matrix.CreateRotationX(Rotation.X) *
                Matrix.CreateRotationY(Rotation.Y) *
                Matrix.CreateRotationZ(Rotation.Z) * 
                Matrix.CreateTranslation(Position);

            Renderer.Render(camera);
        }
    }

    public delegate void UpdateEventHandler(UpdateEventArgs args);

    public class UpdateEventArgs : EventArgs
    {
        public readonly World World;
        public readonly Instance Instance;
        public readonly GameTime GameTime;

        public UpdateEventArgs(World world, Instance instance, GameTime gameTime)
        {
            this.World = world;
            this.Instance = instance;
            this.GameTime = gameTime;
        }
    }

    public delegate void RenderEventHandler(RenderEventArgs args);

    public class RenderEventArgs : EventArgs
    {
        public readonly World World;
        public readonly Instance Instance;
        public readonly ThreeDCamera Camera;
        
        public RenderEventArgs(World world, ref Instance instance, ThreeDCamera camera)
        {
            this.World = world;
            this.Instance = instance;
            this.Camera = camera;
        }
    }
}
