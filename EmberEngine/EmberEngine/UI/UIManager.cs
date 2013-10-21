using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EmberEngine.UI
{
    /// <summary>
    /// Handler for rendering multiple UI elements
    /// </summary>
    class UIManager : UIElement
    {
        static Texture2D blankTex;
        Dictionary<string, UIElement> elements = new Dictionary<string, UIElement>();

        int margine = 0;
        /// <summary>
        /// Gets the edge-margines for this UI manager
        /// </summary>
        public int Margine
        {
            get { return margine; }
            set { margine = value >= 0 ? value : 0; }
        }

        GraphicsDevice graphics;
        Color color = Color.Gray;
        bool drawBackground = false;

        /// <summary>
        /// Gets the total size of this UI manager
        /// </summary>
        public override Vector2 Size
        {
            get
            {
                Vector2 min = GetMin();
                Vector2 max = GetMax();
                return max - min;
            }
        }

        /// <summary>
        /// Gets the top left corner of all UI elements
        /// </summary>
        private Vector2 GetMin()
        {
            Vector2 min = new Vector2(float.PositiveInfinity, float.PositiveInfinity);

            foreach (UIElement e in elements.Values)
            {
                if (e.Position.X < min.X)
                    min.X = e.Position.X;

                if (e.Position.Y < min.Y)
                    min.Y = e.Position.Y;
            }

            return min;
        }

        /// <summary>
        /// Gets the bottom-right corner of all UI elements
        /// </summary>
        private Vector2 GetMax()
        {
            Vector2 max = new Vector2(0, 0);

            foreach (UIElement e in elements.Values)
            {
                if (e.Max.X > max.X)
                    max.X = e.Max.X;

                if (e.Max.Y > max.Y)
                    max.Y = e.Max.Y;
            }

            return max;
        }

        /// <summary>
        /// Creates a new UI manager with the given parameters
        /// </summary>
        /// <param name="Position"></param>
        public UIManager(GraphicsDevice graphics, Vector2 Position, int margine)
        {
            this.graphics = graphics;
            this.Position = Position;
            this.margine = margine;

            elements.Add("NullSpacer", new UISpacer(0));

            if (blankTex == null)
            {
                blankTex = new Texture2D(graphics, 1, 1);
                blankTex.SetData<Color>(new Color[] { Color.White });
            }
        }

        /// <summary>
        /// Creates a new UI manager with the given parameters
        /// </summary>
        /// <param name="Position"></param>
        public UIManager(GraphicsDevice graphics, Vector2 Position, int margine, Color color, byte alpha)
        {
            this.graphics = graphics;
            this.Position = Position;
            this.margine = margine;

            this.color = color;
            this.color.A = alpha;
            drawBackground = true;

            elements.Add("NullSpacer", new UISpacer(0));

            if (blankTex == null)
            {
                blankTex = new Texture2D(graphics, 1, 1);
                blankTex.SetData<Color>(new Color[] { Color.White });
            }
        }

        /// <summary>
        /// Creates a new UI manager with the given parameters
        /// </summary>
        /// <param name="Position"></param>
        public UIManager(GraphicsDevice graphics, Vector2 Position, int margine, Color color, float alpha)
        {
            this.graphics = graphics;
            this.Position = Position;
            this.margine = margine;

            this.color = color;
            this.color.A = (byte)MathHelper.Lerp(0, 255, alpha);
            drawBackground = true;

            elements.Add("NullSpacer", new UISpacer(0));

            if (blankTex == null)
            {
                blankTex = new Texture2D(graphics, 1, 1);
                blankTex.SetData<Color>(new Color[] { Color.White });
            }
        }

        /// <summary>
        /// Renders this UI manager
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw with</param>
        public override void Render(SpriteBatch spriteBatch)
        {
            if (drawBackground)
            {
                spriteBatch.Draw(blankTex, Rect, color);
            }

            foreach (UIElement e in elements.Values)
            {
                e.Render(spriteBatch);
            }
        }

        /// <summary>
        /// Updates all of the UI elements in this UI manager
        /// </summary>
        /// <param name="gameTime">The current gameTime</param>
        public override void Update(GameTime gameTime)
        {
            foreach (UIElement e in elements.Values)
            {
                e.Update(gameTime);
            }
        }

        /// <summary>
        /// Adds an element to this UI manager
        /// </summary>
        /// <param name="element">The element to add</param>
        /// <param name="name">The name of the element. If null, will auto-generate one</param>
        /// <returns>True if the element was sucessfully added</returns>
        public bool AddElement(UIElement element, string name = null)
        {
            if (name == null)
            {
                int c = 0;

                while (!elements.ContainsKey("element_" + c))
                    c++;

                name = "element_" + c;
            }

            if (!elements.ContainsKey(name))
            {
                
                element.Position = Position + new Vector2(0, Max.Y);
                elements.Add(name, element);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Removes an element from this UI manager
        /// </summary>
        /// <param name="name">The name of the element to search for</param>
        /// <returns>True if the element was sucessfully removed</returns>
        public bool RemoveElement(string name)
        {
            return elements.Remove(name);
        }

        /// <summary>
        /// Gets a UI element from the given name
        /// </summary>
        /// <param name="name">The name of the element to get</param>
        /// <returns>The element, or null if the name does not exist</returns>
        public UIElement GetElement(string name)
        {
            if (elements.ContainsKey(name))
                return elements[name];
            else
                return null;
        }
    }
}
