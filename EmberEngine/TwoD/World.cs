using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmberEngine.TwoD.Instances;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EmberEngine.TwoD.Rendering;

namespace EmberEngine.TwoD
{
    public class Level
    {
        Instance2D[] Instances;
        List<int> IDS = new List<int>();
        public TileMap tileMap;
        public Vector2 cameraPos;

        Vector2 viewportSize = new Vector2(400, 260);

        public int trackID;

        Color backgroundColor = Color.White;
        /// <summary>
        /// Gets or sets the background color for this level
        /// </summary>        
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        /// <summary>
        /// Gets or sets the tile map for this level
        /// </summary>
        public TileMap TileMap
        {
            get { return tileMap; }
            set { tileMap = value; }
        }

        /// <summary>
        /// Creates a new world
        /// </summary>
        /// <param name="maxInstances">The max number of instances in this world</param>
        public Level(int maxInstances)
        {
            Instances = new Instance2D[maxInstances];
        }

        /// <summary>
        /// Called when the world should update itself
        /// </summary>
        /// <param name="GameTime">The current GameTime</param>
        public void Update(GameTime GameTime)
        {
            if (trackID != -1)
            {
                cameraPos = -Instances[trackID].Position + viewportSize;
            }

            foreach (int ID in IDS)
            {
                Instances[ID].Update(GameTime);
            }
        }

        /// <summary>
        /// Called when the world should draw itself
        /// </summary>
        /// <param name="SpriteBatch">The spritebatch to draw with</param>
        public void Draw(SpriteBatch SpriteBatch)
        {
            SpriteBatch.GraphicsDevice.Clear(BackgroundColor);

            if (tileMap != null)
                tileMap.Draw(SpriteBatch, cameraPos);

            foreach (int ID in IDS)
            {
                Instances[ID].Draw(SpriteBatch, cameraPos);
            }
        }

        /// <summary>
        /// Gets the instance with the given ID
        /// </summary>
        /// <param name="InstanceID">The instance ID of the instance to get</param>
        /// <returns>The Instance at ID, or null if it does not exist</returns>
        public Instance2D GetInstance(int InstanceID)
        {
            if (IDS.Contains(InstanceID))
                return Instances[InstanceID];
            else
                return null;
        }

        /// <summary>
        /// Adds a new instance to this world
        /// </summary>
        /// <param name="Instance">The instance to add</param>
        /// <returns>The ID of the instance, or -1 if one could not be added</returns>
        public int AddInstance(Instance2D Instance)
        {
            for (int i = 0; i < Instances.Length; i++)
            {
                if (!IDS.Contains(i))
                {
                    IDS.Add(i);
                    Instances[i] = Instance;
                    Instances[i].ID = i;
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Removes an instance from this world
        /// </summary>
        /// <param name="InstanceID">The ID of the instance to remove</param>
        public void RemoveInstance(int InstanceID)
        {
            if (IDS.Contains(InstanceID))
            {
                Instances[InstanceID].Destroy();
                Instances[InstanceID] = null;
                IDS.Remove(InstanceID);
            }
        }

        /// <summary>
        /// Removes an instance from this world
        /// </summary>
        /// <param name="Instance">The instance to remove</param>
        public void RemoveInstance(Instance2D Instance)
        {
            if (IDS.Contains(Instance.ID))
            {
                Instances[Instance.ID].Destroy();
                Instances[Instance.ID] = null;
                IDS.Remove(Instance.ID);
            }
        }
    }
}
