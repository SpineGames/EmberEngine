using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EmberEngine.GUI
{
    /// <summary>
    /// Represents a container holding GUI elements
    /// </summary>
    public abstract class GUIContainer : GUIComponent
    {
        List<GUIComponent> Components = new List<GUIComponent>();

        AlignMode alignMode;
        /// <summary>
        /// The alignment mode for new components
        /// </summary>
        public AlignMode AlignMode
        {
            get { return alignMode; }
            set
            {
                alignMode = value;
                MoveComponents();
            }
        }

        int margine = 5;
        /// <summary>
        /// The margine around the edges
        /// </summary>
        public int Margine
        {
            get { return margine; }
            set
            {
                margine = value;
                MoveComponents();
            }
        }

        public GUIContainer(Rectangle Bounds) : base(Bounds) { }

        public override void Update(GUIUpdateEventArgs args)
        {
            foreach (GUIComponent c in Components)
                c.Update(args);

            base.Update(args);
        }

        /// <summary>
        /// Draws this component to the spritebatch
        /// </summary>
        /// <param name="args">The DrawEventArgs to use</param>
        public override void Draw(GUIDrawEventArgs args)
        {
            args.SpriteBatch.Begin();
            if (RenderTarget != null)
                args.SpriteBatch.Draw(RenderTarget, LocalRect, Color.White);
            args.SpriteBatch.End();

            DrawComponents(args);
        }

        /// <summary>
        /// Draws all the components in this container
        /// </summary>
        /// <param name="args">The drawing args to use</param>
        public void DrawComponents(GUIDrawEventArgs args)
        {
            foreach (GUIComponent c in Components)
                c.Draw(args);
        }

        /// <summary>
        /// Re-aligns all components
        /// </summary>
        private void MoveComponents()
        {
            int x = Margine;
            int y = Margine;

            for (int i = 0; i < Components.Count; i++)
            {
                Components[i].Left = x;
                Components[i].Top = y;

                y += Components[i].Height;
            }
        }

        /// <summary>
        /// Adds a component to this container
        /// </summary>
        /// <param name="Component">The component to add</param>
        public void AddComponent(GUIComponent Component)
        {
            Component.Container = this;
            Components.Add(Component);
            MoveComponents();
            ShouldInvalidate = true;
        }

        /// <summary>
        /// Removes the first component ith the specified name
        /// </summary>
        /// <param name="name">The name of the component to remove</param>
        public void RemoveComponent(string name)
        {
            foreach (GUIComponent c in Components)
            {
                if (c.Name == name)
                {
                    Components.Remove(c);
                    ShouldInvalidate = true;
                    break;
                }
            }
        }
    }
}
