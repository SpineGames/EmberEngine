using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using EmberEngine.ThreeD.Instancing;
using EmberEngine.ThreeD;
using EmberEngine.ThreeD.Rendering;

namespace EmberEngine.ThreeD.Instancing
{
    public class Instance : IWorldComponent
    {
        public Vector3 Position;
        public Vector3 VelocityNormal;
        public float Velocity;
        public Vector3 FacingPolar;
        public IRenderable Renderer;
        public World World;

        public UpdateEventHandler UpdateEvent;
        public RenderEventHandler RenderEvent;

        private int id;
        /// <summary>
        /// Gets this instance's world ID
        /// </summary>
        public int ID
        {
            get { return id; }
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
            this.World = world;
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
                Matrix.CreateRotationX(FacingPolar.X) *
                Matrix.CreateRotationZ(FacingPolar.Z) * 
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
