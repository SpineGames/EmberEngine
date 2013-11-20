using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;

namespace EmberEngine.Tools.Input
{
    /// <summary>
    /// Handles watching a specific key for input
    /// Needs to updated every game tick
    /// </summary>
    public class KeyWatcher
    {
        List<Key> keys = new List<Key>();
        /// <summary>
        /// The list of keys in this key watcher
        /// </summary>
        public List<Key> Keys { get { return keys; } }

        List<bool> isWatching = new List<bool>();
        /// <summary>
        /// The list of keys in this key watcher
        /// </summary>
        public List<bool> IsWatching { get { return isWatching; } }

        KeyType keyType = KeyType.Single;
        /// <summary>
        /// This key watcher's key type
        /// </summary>
        public KeyType ThisKeyType
        {
            get { return keyType; }
        }

        /// <summary>
        /// The tag asocciated with the key watcher
        /// </summary>
        public object Tag;

        /// <summary>
        /// Raised each tick while the key is down (not invoked on first press)
        /// </summary>
        public KeyDownHandler KeyDown;
        /// <summary>
        /// Raised when the key is first pressed
        /// </summary>
        public KeyDownHandler KeyPressed;
        /// <summary>
        /// Raised when the key has been realeased
        /// </summary>
        public KeyDownHandler KeyReleased;

        private bool LastPressed;

        #region Constructors
        /// <summary>
        /// Creates a new KeyWatcher
        /// </summary>
        /// <param name="key">The key to watch</param>
        public KeyWatcher(Key key)
        {
            keys.Add(key);
            isWatching.Add(true);
        }

        /// <summary>
        /// Creates a new KeyWatcher
        /// </summary>
        /// <param name="key">The key to watch</param>
        /// <param name="Tag">The tag associated with this key watcher</param>
        public KeyWatcher(Key key, object Tag)
        {
            keys.Add(key);
            isWatching.Add(true);
            this.Tag = Tag;
        }

        /// <summary>
        /// Creates a new keywatcher with multiple keys
        /// </summary>
        /// <param name="keys">The keys to watch</param>
        /// <param name="type">The keytype to watch with. <b>Default KeyType.AllDown</b></param>
        public KeyWatcher(Key[] keys, KeyType type = KeyType.AllDown)
        {
            foreach (Key k in keys)
            {
                this.keys.Add(k);
                this.isWatching.Add(true);
            }
            this.keyType = type;
        }

        /// <summary>
        /// Creates a new keywatcher with multiple keys
        /// </summary>
        /// <param name="keys">The keys to watch</param>
        /// <param name="Tag">The tag associated with this key watcher</param>
        /// <param name="type">The keytype to watch with. <b>Default KeyType.AllDown</b></param>
        public KeyWatcher(Key[] keys, object Tag, KeyType type = KeyType.AllDown)
        {
            foreach (Key k in keys)
            {
                this.keys.Add(k);
                this.isWatching.Add(true);
            }
            this.Tag = Tag;
            this.keyType = type;
        }

        ///// <summary>
        ///// Creates a new KeyWatcher
        ///// </summary>
        ///// <param name="key">The key to watch</param>
        ///// <param name="onPressed">The KeyDown event to invoke when the key is pressed</param>
        ///// <param name="onDown">The KeyDown event to invoke when the key is down</param>
        ///// <param name="onRelease">The KeyDown event to invoke when the key is released</param>
        //public KeyWatcher(Key key, KeyDownHandler onPressed = null, 
        //    KeyDownHandler onDown = null, KeyDownHandler onRelease = null)
        //{
        //    keys.Add(key);
        //    this.KeyPressed += onPressed;
        //    this.KeyDown += onDown;
        //    this.KeyReleased = onRelease;
        //}

        ///// <summary>
        ///// Creates a new KeyWatcher
        ///// </summary>
        ///// <param name="key">The key to watch</param>
        ///// <param name="Tag">The tag associated with this key watcher</param>
        ///// <param name="onPressed">The KeyDown event to invoke when the key is pressed</param>
        ///// <param name="onDown">The KeyDown event to invoke when the key is down</param>
        ///// <param name="onRelease">The KeyDown event to invoke when the key is released</param>
        //public KeyWatcher(Key key, object Tag, KeyDownHandler onPressed = null,
        //    KeyDownHandler onDown = null, KeyDownHandler onRelease = null)
        //{
        //    keys.Add(key);
        //    this.Tag = Tag;
        //    this.KeyPressed += onPressed;
        //    this.KeyDown += onDown;
        //    this.KeyReleased = onRelease;
        //}

        ///// <summary>
        ///// Creates a new keywatcher with multiple keys
        ///// </summary>
        ///// <param name="keys">The keys to watch</param>
        ///// <param name="type">The keytype to watch with. <b>Default KeyType.AllDown</b></param>
        ///// <param name="onPressed">The KeyDown event to invoke when the key is pressed</param>
        ///// <param name="onDown">The KeyDown event to invoke when the key is down</param>
        ///// <param name="onRelease">The KeyDown event to invoke when the key is released</param>
        //public KeyWatcher(Key[] keys, KeyType type = KeyType.AllDown, 
        //    KeyDownHandler onPressed = null, KeyDownHandler onDown = null, 
        //    KeyDownHandler onRelease = null)
        //{
        //    this.keys.AddRange(keys);
        //    this.keyType = type;
        //    this.KeyPressed += onPressed;
        //    this.KeyDown += onDown;
        //    this.KeyReleased = onRelease;
        //}

        ///// <summary>
        ///// Creates a new keywatcher with multiple keys
        ///// </summary>
        ///// <param name="keys">The keys to watch</param>
        ///// <param name="Tag">The tag associated with this key watcher</param>
        ///// <param name="type">The keytype to watch with. <b>Default KeyType.AllDown</b></param>
        ///// <param name="onPressed">The KeyDown event to invoke when the key is pressed</param>
        ///// <param name="onDown">The KeyDown event to invoke when the key is down</param>
        ///// <param name="onRelease">The KeyDown event to invoke when the key is released</param>
        //public KeyWatcher(Key[] keys, object Tag, KeyType type = KeyType.AllDown, 
        //    KeyDownHandler onPressed = null, KeyDownHandler onDown = null, 
        //    KeyDownHandler onRelease = null)
        //{
        //    this.keys.AddRange(keys);
        //    this.Tag = Tag;
        //    this.keyType = type;
        //    this.KeyPressed += onPressed;
        //    this.KeyDown += onDown;
        //    this.KeyReleased = onRelease;
        //}
        #endregion

        /// <summary>
        /// Adds a new event to the KeyPressed event handler
        /// </summary>
        /// <param name="Event">The event to add</param>
        /// <returns>This keywatcher with the event added</returns>
        public KeyWatcher AddPressed(KeyDownHandler Event)
        {
            KeyPressed += Event;

            return this;
        }

        /// <summary>
        /// Adds a new event to the KeyDown event handler
        /// </summary>
        /// <param name="Event">The event to add</param>
        /// <returns>This keywatcher with the event added</returns>
        public KeyWatcher AddDown(KeyDownHandler Event)
        {
            KeyDown += Event;

            return this;
        }

        /// <summary>
        /// Adds a new event to the KeyReleased event handler
        /// </summary>
        /// <param name="Event">The event to add</param>
        /// <returns>This keywatcher with the event added</returns>
        public KeyWatcher AddReleased(KeyDownHandler Event)
        {
            KeyReleased += Event;

            return this;
        }

        /// <summary>
        /// Checks if this key watcher watches a specific key
        /// </summary>
        /// <param name="key">The key to look for</param>
        /// <returns>True if this key watcher watches <i>key</i></returns>
        public bool WatchesKey(Key key)
        {
            return keys.Contains(key);
        }

        /// <summary>
        /// Disables the given key i this keywatcher
        /// </summary>
        /// <param name="key">The key to disable</param>
        public void DisableKey(Key key)
        {
            if (WatchesKey(key))
                isWatching[keys.IndexOf(key)] = false;
        }

        /// <summary>
        /// Enables a key to be watched if this keywatcher is configured
        /// to watch it
        /// </summary>
        /// <param name="key">The key to enable</param>
        public void EnableKey(Key key)
        {
            if (WatchesKey(key))
                isWatching[keys.IndexOf(key)] = true;
        }

        /// <summary>
        /// Updates this KeyWatcher
        /// </summary>
        public void Update()
        {
            KeyboardState k = Keyboard.GetState();
            switch (keyType)
            {
                #region Single Keys
                case KeyType.Single:
                    if (k.IsKeyDown(keys[0]) & isWatching[0] == true)
                    {
                        if (LastPressed)
                        {
                            if (KeyDown != null)
                                KeyDown.Invoke(new KeyDownEventArgs(Tag));
                        }
                        else
                        {
                            LastPressed = true;

                            if (KeyPressed != null)
                                KeyPressed.Invoke(new KeyDownEventArgs(Tag));
                        }
                    }
                    else
                    {
                        if (LastPressed)
                        {
                            LastPressed = false;

                            if (KeyReleased != null)
                                KeyReleased.Invoke(new KeyDownEventArgs(Tag));
                        }
                    }
                    break;
                #endregion

                #region AllDown
                case KeyType.AllDown:
                    foreach (Key key in keys)
                        if (!(k.IsKeyDown(key) & isWatching[keys.IndexOf(key)] == true))
                        {
                            if (LastPressed)
                            {
                                LastPressed = false;

                                if (KeyReleased != null)
                                    KeyReleased.Invoke(new KeyDownEventArgs(Tag));
                            }

                            break;
                        }

                    if (LastPressed)
                    {
                        if (KeyDown != null)
                            KeyDown.Invoke(new KeyDownEventArgs(Tag));
                    }
                    else
                    {
                        LastPressed = true;

                        if (KeyPressed != null)
                            KeyPressed.Invoke(new KeyDownEventArgs(Tag));
                    }
                    break;
                #endregion

                #region OneOf
                case KeyType.OneOf:
                    foreach (Key key in keys)
                        if (k.IsKeyDown(key) & isWatching[keys.IndexOf(key)] == true)
                        {
                            if (LastPressed)
                            {
                                if (KeyDown != null)
                                    KeyDown.Invoke(new KeyDownEventArgs(Tag));
                            }
                            else
                            {
                                LastPressed = true;

                                if (KeyPressed != null)
                                    KeyPressed.Invoke(new KeyDownEventArgs(Tag));
                            }
                            break;
                        }
                    //past this is only invoked if no key was down
                    if (LastPressed)
                    {
                        LastPressed = false;

                        if (KeyReleased != null)
                            KeyReleased.Invoke(new KeyDownEventArgs(Tag));
                    }
                        
                    break;
                #endregion
            }
        }
        
        /// <summary>
        /// The type of keywatcher to use
        /// </summary>
        public enum KeyType
        {
            /// <summary>
            /// A single key is handled
            /// </summary>
            Single,
            /// <summary>
            /// All the keys must be pressed
            /// </summary>
            AllDown,
            /// <summary>
            /// Only one of the keys must be pressed
            /// </summary>
            OneOf
        }
    }
    
    /// <summary>
    /// Invoked when a key is help down
    /// </summary>
    /// <param name="e">The KeyDownEventArgs to use</param>
    public delegate void KeyDownHandler(KeyDownEventArgs e);

    /// <summary>
    /// Represents the agruments for a keyDown event
    /// </summary>
    public class KeyDownEventArgs : EventArgs
    {
        /// <summary>
        /// The key handler's Tag
        /// </summary>
        public readonly object Tag;

        /// <summary>
        /// Creates a new event args for a key down
        /// </summary>
        /// <param name="chunk">The key being pressed</param>
        public KeyDownEventArgs(object Tag = null)
        {
            this.Tag = Tag;
        }
    }
}
