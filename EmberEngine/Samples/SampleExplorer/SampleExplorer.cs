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
using System.Threading;
using EmberEngine.GUI;

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

namespace Samples.SampleExplorer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SampleExplorer : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        UIManager UI;
        KeyManager keys;

        ISample sample;

        public SampleExplorer()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Window.Title = "Slots Demo";

            PolyRender_VPNTC.Initialize(CullMode.CullCounterClockwiseFace);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            GraphicsDevice.RasterizerState.CullMode = CullMode.None;
            keys = new KeyManager();

            this.graphics.CreateDevice();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            string s = Content.RootDirectory;

            GUI.Initialize(Content, "Common/Font");

            spriteBatch = new SpriteBatch(GraphicsDevice);
            FontManager.LoadFont(Content, "Common/Font/sf1");
            FontManager.LoadFont(Content, "Common/Font/QuartzFont");

            UI = new UIManager(GraphicsDevice, new Vector2(5, 5), 0, Color.Gray, 0.1F);

            UI.AddElement(new UISClickableString("sf1", "< Terrain Demo >", Color.Black), "Sample1");
            UI.AddElement(new UISClickableString("sf1", "<  Slots Game  >", Color.Black), "Sample2");
            UI.AddElement(new UISClickableString("sf1", "<   GUI Demo   >", Color.Black), "Sample3");
            UI.AddElement(new UISClickableString("sf1", "< Shadow Test  >", Color.Black), "Sample4");

            ((UIClickable)UI.GetElement("Sample1")).OnClick += Sample1Pressed;
            ((UIClickable)UI.GetElement("Sample2")).OnClick += Sample2Pressed;
            ((UIClickable)UI.GetElement("Sample3")).OnClick += Sample3Pressed;
            ((UIClickable)UI.GetElement("Sample4")).OnClick += Sample4Pressed;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (!(sample != null && sample.Enabled))
            {
                if (Keyboard.GetState().IsKeyDown(Key.Escape))
                    this.Exit();

                keys.Update();

                UpdateUI(gameTime);

                base.Update(gameTime);
            }
            else
            {
                sample.Update(gameTime);
            }
        }
        
        /// <summary>
        /// Updates the User interface
        /// </summary>
        private void UpdateUI(GameTime gameTime)
        {
            UI.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (!(sample != null && sample.Enabled))
            {
                GraphicsDevice.Clear(Color.Black);

                spriteBatch.Begin();
                UI.Render(spriteBatch);
                spriteBatch.End();

                FramerateCounter.OnDraw(gameTime);

                base.Draw(gameTime);
            }
            else
            {
                sample.Draw(gameTime);
            }
        }

        /// <summary>
        /// Invoked when sample is pressed
        /// </summary>
        /// <param name="args">The object tag</param>
        public void Sample1Pressed(object Tag)
        {
            if (sample == null || sample.GetType() != typeof(Sample1.Sample))
                sample = new Sample1.Sample(this);
            else
                sample.Enabled = true;
        }

        /// <summary>
        /// Invoked when sample 2 is pressed
        /// </summary>
        /// <param name="args">The object tag</param>
        public void Sample2Pressed(object Tag)
        {
            if (sample == null || sample.GetType() != typeof(Sample2.Sample))
                sample = new Sample2.Sample(this);
            else
                sample.Enabled = true;
        }

        /// <summary>
        /// Invoked when sample 3 is pressed
        /// </summary>
        /// <param name="args">The object tag</param>
        public void Sample3Pressed(object Tag)
        {
            if (sample == null || sample.GetType() != typeof(Sample3.Sample))
                sample = new Sample3.Sample(this);
            else
                sample.Enabled = true;
        }

        /// <summary>
        /// Invoked when sample 4 is pressed
        /// </summary>
        /// <param name="args">The object tag</param>
        public void Sample4Pressed(object Tag)
        {
            if (sample == null || sample.GetType() != typeof(Sample4.Sample))
                sample = new Sample4.Sample(this);
            else
                sample.Enabled = true;
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
