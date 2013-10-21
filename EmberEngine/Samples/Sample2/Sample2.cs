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
    public class Sample : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        World world;

        UIManager UI;
        KeyManager keys;
        
        public Sample()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Window.Title = "Slots Demo";

            PolyRender.Initialize(CullMode.CullCounterClockwiseFace);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //GraphicsDevice.RasterizerState.CullMode = CullMode.None;
            keys = new KeyManager();

            keys.AddKeyWatcher(new KeyWatcher(Key.Enter));
            keys.Watchers[0].AddPressed(KeyPressed);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            BasicShader effect = new BasicShader(new BasicEffect(GraphicsDevice));

            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D spinner = Content.Load<Texture2D>("Sample2/Textures/Spinner");
            Texture2D grid = Content.Load<Texture2D>("Sample2/Textures/grid");
            FontManager.LoadFont(Content, "Common/Font/sf1");
            
            effect.Texture = spinner;

            ((BasicEffect)effect.BaseEffect).VertexColorEnabled = true;
            ((BasicEffect)effect.BaseEffect).TextureEnabled = true;
            ((BasicEffect)effect.BaseEffect).LightingEnabled = true;

            ((BasicEffect)effect.BaseEffect).AmbientLightColor = Color.Gray.ToVector3();
                      
            ThreeDCamera currentCamera = new ThreeDCamera(new Vector3(0, -10, 2), graphics, ThreeDCamera.STATIC);
            currentCamera.UpVector = new Vector3(0, 0, 1);
            currentCamera.CameraYaw = -90;

            world = new World(currentCamera);

            Plane p = new Plane(new Vector3(-10, -10, 0), new Vector3(10, 10, 0), effect, 20);
            ((PolyRender)p.Renderer).Texture = grid;
            p.Initialize(world);

            SlotsGame.Intialize(ref world, effect);

            UI = new UIManager(GraphicsDevice, new Vector2(5, 5), 0, Color.Gray, 0.1F);

            UI.AddElement(new UIString("sf1", "FPS: ", Color.Black), "fps");
            UI.AddElement(new UIString("sf1", "", Color.Black), "cameraPos");         
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
            if (Keyboard.GetState().IsKeyDown(Key.Escape))
                this.Exit();

            world.Update(gameTime);

            if (!IsActive)
                world.MainCamera.MouseLook = false;

            keys.Update();

            UpdateUI();

            base.Update(gameTime);
        }

        /// <summary>
        /// Updates the User interface
        /// </summary>
        private void UpdateUI()
        {
            UI.GetElement("fps").Tag = "FPS: " + FramerateCounter.FPS;
            UI.GetElement("cameraPos").Tag = world.MainCamera.ToString();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

            world.Render();
            
            spriteBatch.Begin();
            UI.Render(spriteBatch);
            spriteBatch.End();

            FramerateCounter.OnDraw(gameTime);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Invoked when the action key is pressed
        /// </summary>
        /// <param name="args">The KeyDownEventArgs to use</param>
        public void KeyPressed(KeyDownEventArgs args)
        {
            SlotsGame.StartSpin();
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
