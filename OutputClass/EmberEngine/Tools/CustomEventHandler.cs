using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmberEngine.Tools
{
    /// <summary>
    /// A handler for the custom event handlers
    /// </summary>
    /// <param name="sender">The object that invoked this custom event handler</param>
    /// <param name="args">The custom event handler to use</param>
    public delegate void CustomEventHandler(object sender, CustomEventArgs args);

    /// <summary>
    /// Represents a custom event argument
    /// </summary>
    public class CustomEventArgs : EventArgs
    {
        public readonly object Argument;

        public CustomEventArgs(object argument)
        {
            this.Argument = argument;
        }
    }
}
