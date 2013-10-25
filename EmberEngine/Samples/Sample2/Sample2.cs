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
using EmberEngine.ThreeD.Tools;

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

namespace Samples.Sample2
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Sample : GameComponent, ISample
    {
        SpriteBatch spriteBatch;
        
        World world;

        KeyManager keys;
        UIManager UI;

        public Sample(Game game)
            : base(game)
        {
            Initialize();
            game.Window.Title = "Slots Demo";
            Enabled = true;

            PolyRender_VPNTC.Initialize(CullMode.CullCounterClockwiseFace);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public override void Initialize()
        {
            //GraphicsDevice.RasterizerState.CullMode = CullMode.None;
            keys = new KeyManager();

            keys.AddKeyWatcher(new KeyWatcher(Key.Enter));
            keys.Watchers[0].AddPressed(KeyPressed);
            keys.AddKeyWatcher(new KeyWatcher(Key.Escape));
            keys.Watchers[1].AddPressed(EscPressed);
            keys.AddKeyWatcher(new KeyWatcher(Key.R));
            keys.Watchers[2].AddPressed(ResetPressed);
            
            LoadContent();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        private void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ThreeDCamera currentCamera = new ThreeDCamera(new Vector3(0, -10, 2), GraphicsDevice, ThreeDCamera.STATIC);
            currentCamera.UpVector = new Vector3(0, 0, 1);
            currentCamera.CameraYaw = -90;

            world = new World(currentCamera);

            SlotsGame.Intialize(ref world, Game.GraphicsDevice, Game.Content, spriteBatch);

            UI = new UIManager(Game.GraphicsDevice, new Vector2(10, 10), 1, Color.Gray, 0.2F);
            UI.AddElement(new UIString(FontManager.Fonts["QuartzFont"], "ga", Color.Green), "Money");
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            world.Update(gameTime);
            
            keys.Update();

            UpdateUI();

            base.Update(gameTime);
        }

        /// <summary>
        /// Updates the User interface
        /// </summary>
        private void UpdateUI()
        {
            UI.GetElement("Money").Tag = "$" + SlotsGame.money;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Blue);
            
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

            world.Render();

            spriteBatch.Begin();
            UI.Render(spriteBatch);
            spriteBatch.End();

            FramerateCounter.OnDraw(gameTime);
        }

        private void ResetPressed(KeyDownEventArgs args)
        {
            SlotsGame.ResetGame(args);
        }

        /// <summary>
        /// Invoked when the action key is pressed
        /// </summary>
        /// <param name="args">The KeyDownEventArgs to use</param>
        private void KeyPressed(KeyDownEventArgs args)
        {
            SlotsGame.StartSpin();
        }

        /// <summary>
        /// Invoked when the Escape key is pressed
        /// </summary>
        /// <param name="args">The KeyDownEventArgs to use</param>
        private void EscPressed(KeyDownEventArgs args)
        {
            Enabled = false;
        }
        
        /// <summary>
        /// Loads a byte[] from an embedded resource
        /// </summary>
        /// <param name="name">The name of the embeddded resource to load</param>
        /// <returns>A byte[] containing the file's data</returns>
        internal static byte[] LoadEffectResource(string name)
        {
            Stream stream = File.OpenRead(Directory.GetCurrentDirectory() + "\\Content\\Shaders\\BasicShader.xnb");

            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
