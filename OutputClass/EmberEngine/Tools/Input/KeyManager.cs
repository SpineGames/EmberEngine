using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;

namespace EmberEngine.Tools.Input
{
    /// <summary>
    /// Wraps multiple KeyWatcher's together to make updating easier
    /// </summary>
    public class KeyManager
    {
        List<KeyWatcher> watchers = new List<KeyWatcher>();
        /// <summary>
        /// Gets the key watchers managed by this key manager
        /// </summary>
        public List<KeyWatcher> Watchers { get { return watchers; } }

        /// <summary>
        /// Adds a new key watcher to this manager
        /// </summary>
        /// <param name="watcher">The keywatcher to manage</param>
        public void AddKeyWatcher(KeyWatcher watcher)
        {
            watchers.Add(watcher);
        }

        /// <summary>
        /// Disables a key for all key watchers
        /// </summary>
        /// <param name="key">The key to disable</param>
        public void DisableKey(Key key)
        {
            foreach (KeyWatcher k in watchers)
                k.DisableKey(key);
        }

        /// <summary>
        /// Enables a key for all keywatchers, only if they are configured to
        /// handle that key
        /// </summary>
        /// <param name="key">The key to enable</param>
        public void EnableKey(Key key)
        {
            foreach (KeyWatcher k in watchers)
                k.EnableKey(key);
        }

        /// <summary>
        /// Updates all key watchers in this manager
        /// </summary>
        public void Update()
        {
            foreach (KeyWatcher k in watchers)
                k.Update();
        }
    }
}
