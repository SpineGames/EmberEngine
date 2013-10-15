using EmberEngine.Tools;
using EmberEngine.Tools.Management;
using EmberEngine.ThreeD;
using EmberEngine.ThreeD.Instancing;
using EmberEngine.ThreeD.Rendering;
using EmberEngine.ThreeD.Rendering.ShaderWrappers;
using EmberEngine.SpineLibrary.UI;
//
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using EmberEngine.Tools.Input;
using OpenTK.Input;
using EmberEngine.Tools.AdvancedMath;

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

namespace Samples.Sample1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Sample : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        BasicShader effect;

        Texture2D tex;

        World world;

        ThreeDTerrain terrain;
        Texture2D terrainHeight;

        UIManager UI;
        KeyManager keys;
        
        public Sample()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Window.Title = "Mono Test Game";

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

            keys.AddKeyWatcher(new KeyWatcher(Key.Q));
            keys.Watchers[0].AddPressed(QPressed);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            byte[] temp = LoadEffectResource("Content/Shaders/StandardShader.mgfx");

            byte _Version = temp[4];

            if (_Version == 3)
                effect = new BasicShader(new Effect(GraphicsDevice, temp));
            else
                effect = new BasicShader(new BasicEffect(GraphicsDevice));

            spriteBatch = new SpriteBatch(GraphicsDevice);

            tex = Content.Load<Texture2D>("Terrain/terrain_2");
            Texture2D tex2 = Content.Load<Texture2D>("Terrain/terrain_0");
            FontManager.LoadFont(Content, "Font/sf1");
            
            effect.Texture = tex;

            ((BasicEffect)effect.BaseEffect).TextureEnabled = true;
            ((BasicEffect)effect.BaseEffect).LightingEnabled = true;

            ((BasicEffect)effect.BaseEffect).AmbientLightColor = Color.Gray.ToVector3();

            ((BasicEffect)effect.BaseEffect).DirectionalLight0.DiffuseColor = Color.LightGray.ToVector3();
            ((BasicEffect)effect.BaseEffect).DirectionalLight0.Direction = new Vector3(0, 0, -1);
            ((BasicEffect)effect.BaseEffect).DirectionalLight0.Enabled = true;
            
            ThreeDCamera currentCamera = new ThreeDCamera(new Vector3(0, 0, 5), graphics, ThreeDCamera.PLAYER_WASD_MOUSE);
            currentCamera.UpVector = new Vector3(0, 0, 1);            
            currentCamera.MouseLook = true;

            world = new World(currentCamera);

            UI = new UIManager(GraphicsDevice, new Vector2(5, 5), 0, Color.Gray, 0.1F);

            UI.AddElement(new UIString("sf1", currentCamera.ToString(), Color.Black), "camera");
            UI.AddElement(new UIString("sf1", "FPS: ", Color.Black), "fps");
            UI.AddElement(new UIString("sf1", "Mouse Pos: ", Color.Black), "mouse");

            terrain = new ThreeDTerrain(GraphicsDevice, new Point(256, 256), 4F, new Random().Next());
            terrain.Initialize(world);
            terrainHeight = terrain.GetHeightmapTex();

            Ball b = new Ball(4, 16, new Vector3(0, 0, 0), effect, 8);
            b.UpdateEvent += UpdateBall;
            ((PolyRender)b.Renderer).Texture = tex2;
            ((PolyRender)b.Renderer).WireFrame = true;
            b.Initialize(world);

            ((PolyRender)terrain.Renderer).Effect = effect;
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
            UI.GetElement("camera").Tag = world.MainCamera.ToString();
            UI.GetElement("fps").Tag = "FPS: " + FramerateCounter.FPS;
            UI.GetElement("mouse").Tag = "Mouse Pos: " + Mouse.GetState().X + ", " + Mouse.GetState().Y;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            world.Render();
            
            spriteBatch.Begin();
            spriteBatch.Draw(terrainHeight,
                new Rectangle(
                    GraphicsDevice.Viewport.Width - 120,
                    GraphicsDevice.Viewport.Height - 120,
                    120, 120), Color.White);

            UI.Render(spriteBatch);

            spriteBatch.End();

            FramerateCounter.OnDraw(gameTime);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Updates the camera tracking ball
        /// </summary>
        /// <param name="args">The update event args</param>
        public void UpdateBall(UpdateEventArgs args)
        {
            Ball ball = (Ball)args.Instance;
            
            ball.Position = terrain.TranslateVector(world.MainCamera.CameraPos);
            ball.FacingPolar = Math2.GetYawPitchRoll(world.MainCamera.CameraNormal);
        }

        public void QPressed(KeyDownEventArgs args)
        {
            world.MainCamera.MouseLook = !world.MainCamera.MouseLook;
        }
        
        internal static byte[] LoadEffectResource(string name)
        {
            Stream stream = File.OpenRead(Directory.GetCurrentDirectory() + "\\Content\\Shaders\\StandardShader.mgfx");

            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
