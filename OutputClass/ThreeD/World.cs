using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmberEngine.ThreeD.Instancing;
using Microsoft.Xna.Framework;

namespace EmberEngine.ThreeD
{
    /// <summary>
    /// Represents a 3D world
    /// </summary>
    public class World
    {
        public Instance[] instances = new Instance[0];

        /// <summary>
        /// The camera used to render this world to the screen
        /// </summary>
        public ThreeDCamera MainCamera;
        
        /// <summary>
        /// Creates a new world
        /// </summary>
        public World(ThreeDCamera camera, EventHandler initialize = null)
        {
            MainCamera = camera;

            Initialize(initialize);
        }

        /// <summary>
        /// Calls this worlds initialize event handler
        /// </summary>
        /// <param name="initialize">The event handler to call with</param>
        public void Initialize(EventHandler initialize)
        {
            if (initialize != null)
                initialize.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Adds a new component to this world
        /// </summary>
        /// <param name="instance">The component to add</param>
        /// <returns>The instance ID for the object</returns>
        public int AddInstance(Instance instance)
        {
            return AppendInstanceToArray(instance);
        }

        /// <summary>
        /// Gets the instance from the given ID
        /// </summary>
        /// <param name="ID">The ID to search for</param>
        /// <returns>The compenent with the given ID</returns>
        public Instance GetInstance(int ID)
        {
            if (ID > 0 & ID < instances.Length)
                return (Instance)instances[ID];
            else
                return null;
        }

        /// <summary>
        /// Gets the first open instance space
        /// </summary>
        /// <returns>The first available ID</returns>
        private int GetFirstOpenID()
        {
            for (int i = 0; i < instances.Length; i++)
            {
                if (instances[i] == null)
                    return i;
            }

            return instances.Length;
        }

        /// <summary>
        /// Adds the component to they array
        /// </summary>
        /// <param name="component">The component to add</param>
        /// <returns>The new ID of the object</returns>
        private int AppendInstanceToArray(Instance component)
        {
            int ID = GetFirstOpenID();
            AppendInstanceToArray(component, ID);

            return ID;
        }

        /// <summary>
        /// Adds the component to they array
        /// </summary>
        /// <param name="component">The component to add</param>
        /// <param name="ID">The ID of the component</param>
        private void AppendInstanceToArray(Instance component, int ID)
        {
            if (ID > 0 & ID < instances.Length)
                instances[ID] = component;
            else
            {
                Instance[] temp = new Instance[instances.Length + 1];
                instances.CopyTo(temp, 0);
                temp[temp.Length - 1] = component;
                instances = temp;
            }           
        }

        /// <summary>
        /// Updates all the components in this world
        /// </summary>
        /// <param name="window">The game window to render with</param>
        /// <param name="gameTime">The current game time</param>
        public void Update(GameTime gameTime)
        {
            MainCamera.Update(gameTime);

            foreach (Instance c in instances)
            {
                if (c!= null)
                    c.Update(gameTime);
            }
        }

        /// <summary>
        /// Renders all the components in this world
        /// </summary>
        public void Render()
        {
            foreach (IWorldComponent c in instances)
            {
                c.Render(MainCamera);
            }
        }
    }
}
