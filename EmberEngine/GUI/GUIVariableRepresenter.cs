using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using EmberEngine.Tools.Variables;

namespace EmberEngine.GUI
{
    /// <summary>
    /// Represents a GUI label that tracks a variable
    /// </summary>
    /// <typeparam name="T">The type of variable to track</typeparam>
    public class GUIVariableRepresenter<T> : GUILabel
    {
        /// <summary>
        /// Creates a new variable representer
        /// </summary>
        /// <param name="Bounds">The initial bounds of the label</param>
        /// <param name="Font">The font to use</param>
        /// <param name="Trackable">The trackable variable to track</param>
        public GUIVariableRepresenter(Rectangle Bounds, SpriteFont Font, 
            TrackableVariable<T> Trackable) : base(Bounds, Font, "null")
        {
            Trackable.Changed += VarChanged;
        }

        /// <summary>
        /// Called when the variable has changed
        /// </summary>
        /// <param name="args">The VariableChangedArgs containing the new value</param>
        private void VarChanged(VariableChangedEventArgs args)
        {
            Text = GUIUtils.WrapText(Font, args.Value.ToString(), Width);
        }
    }
}
