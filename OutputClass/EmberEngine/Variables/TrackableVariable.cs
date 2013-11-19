using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmberEngine.Tools.Variables
{
    /// <summary>
    /// Represents a variable that can be tracked through event handlers
    /// </summary>
    public abstract class TrackableVariable<T>
    {
        public VariableChangedEventHandler Changed;
        T value;
        /// <summary>
        /// Gets or sets the value of this variable
        /// </summary>
        public T Value
        {
            get { return value; }
            set
            {
                this.value = value;

                if (Changed != null)
                    Changed.Invoke(new VariableChangedEventArgs(value));
            }
        }
    }

    public delegate void VariableChangedEventHandler(VariableChangedEventArgs args);

    public class VariableChangedEventArgs
    {
        public readonly object Value;

        public VariableChangedEventArgs(object Value)
        {
            this.Value = Value;
        }
    }
}
