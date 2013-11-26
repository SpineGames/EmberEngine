using EmberEngine.Tools;
using EmberEngine.Tools.Management;
using EmberEngine.ThreeD;
using EmberEngine.ThreeD.Instancing;
using EmberEngine.ThreeD.Rendering;
using EmberEngine.ThreeD.Rendering.ShaderWrappers;
using EmberEngine.UI;
using EmberEngine.Tools.Input;
using EmberEngine.Tools.AdvancedMath;
//
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using OpenTK.Input;
using Microsoft.Xna.Framework.Content;

///<Notes to self:>
///Fix the hackish-as-fuck shader setup... like seriously, sit down and do some thinking
///
///Runtime initialization of Random is fucked
/// - ex:
///  class foo
///  {
///     Random rand = new Random();
///  }
///</Note>

namespace Samples
{
    /// <summary>
    /// A base saple interface
    /// </summary>
    public abstract class ISample
    {
        /// <summary>
        /// Checks if this sample is enabled
        /// </summary>
        public bool Enabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the host game for this sample
        /// </summary>
        public readonly Game Host;

        /// <summary>
        /// Creates a new sample
        /// </summary>
        /// <param name="Host">The host game</param>
        public ISample(Game Host)
        {
            this.Host = Host;
            Initialize();
            LoadContent();

            Enabled = true;
        }

        /// <summary>
        /// Initialize this sample
        /// </summary>
        protected abstract void Initialize();

        /// <summary>
        /// Loads all the content for this sample
        /// </summary>
        protected abstract void LoadContent();

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public abstract void Draw(GameTime gameTime);
    }
}
