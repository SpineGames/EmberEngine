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

namespace Samples.Sample1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Sample : ISample
    {
        SpriteBatch spriteBatch;

        Texture2D tex;

        World world;

        ThreeDTerrain terrain;
        Texture2D terrainHeight;

        UIManager UI;
        KeyManager keys;
        
        public Sample(Game game)
            : base(game)
        {
            Initialize();

            game.Window.Title = "Ember Engine Test";
            Enabled = true;

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
            //GraphicsDevice.RasterizerState.CullMode = CullMode.None;
            keys = new KeyManager();

            keys.AddKeyWatcher(new KeyWatcher(Key.Q));
            keys.Watchers[0].AddPressed(QPressed);
            keys.AddKeyWatcher(new KeyWatcher(Key.Escape));
            keys.Watchers[1].AddPressed(EscPressed);

            LoadContent();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            QuadTexShader terrainShader = new QuadTexShader(Host.Content.Load<Effect>("Common/Shaders/QuadTex"));
            BasicShader effect = new BasicShader(new BasicEffect(Host.GraphicsDevice));

            spriteBatch = new SpriteBatch(Host.GraphicsDevice);

            tex = Host.Content.Load<Texture2D>("Sample1/Terrain/terrain_2");

            Texture2D tex0 = Host.Content.Load<Texture2D>("Sample1/Terrain/terrain_0");
            Texture2D tex1 = Host.Content.Load<Texture2D>("Sample1/Terrain/terrain_1");
            Texture2D tex2 = Host.Content.Load<Texture2D>("Sample1/Terrain/terrain_2");
            Texture2D tex3 = Host.Content.Load<Texture2D>("Sample1/Terrain/terrain_3");

            CelShader toon = new CelShader(Host.Content.Load<Effect>("Common/Shaders/CelShader"));

            toon.Texture = tex0;
            toon.DiffuseColor = Color.White;
            toon.ToonLevel = 4;

            terrainShader.Texture0 = tex0;
            terrainShader.Texture1 = tex1;
            terrainShader.Texture2 = tex2;
            terrainShader.Texture3 = tex3;

            terrainShader.DiffuseColor = Color.White;
            terrainShader.DiffuseLightDirection = new Vector3(1, 1, 1);
            terrainShader.DiffuseIntensity = 1F;

            terrainShader.AmbientColor = Color.Gray;
            terrainShader.AmbientIntensity = 1.0F;
            
            effect.Texture = tex;

            ((BasicEffect)effect.BaseEffect).VertexColorEnabled = true;
            ((BasicEffect)effect.BaseEffect).TextureEnabled = true;
            ((BasicEffect)effect.BaseEffect).LightingEnabled = true;

            ((BasicEffect)effect.BaseEffect).AmbientLightColor = Color.Gray.ToVector3();

            ((BasicEffect)effect.BaseEffect).DirectionalLight0.DiffuseColor = Color.LightGray.ToVector3();
            ((BasicEffect)effect.BaseEffect).DirectionalLight0.Direction = new Vector3(0, 0, -1);
            ((BasicEffect)effect.BaseEffect).DirectionalLight0.Enabled = true;

            ThreeDCamera currentCamera = new ThreeDCamera(new Vector3(0, 0, 5), Host.GraphicsDevice, ThreeDCamera.PLAYER_WASD_MOUSE);
            currentCamera.UpVector = new Vector3(0, 0, 1);            
            currentCamera.MouseLook = true;

            world = new World(currentCamera);

            UI = new UIManager(Host.GraphicsDevice, new Vector2(5, 5), 0, Color.Gray, 0.1F);

            UI.AddElement(new UIString("sf1", currentCamera.ToString(), Color.Black), "camera");
            UI.AddElement(new UIString("sf1", "FPS: ", Color.Black), "fps");
            UI.AddElement(new UIString("sf1", "Mouse Pos: ", Color.Black), "mouse");

            terrain = new ThreeDTerrain(Host.GraphicsDevice, new Point(256, 256), 4F, new Random().Next());
            terrain.Initialize(world);
            terrainHeight = terrain.GetHeightmapTex();

            Ball b = new Ball(4, 16, new Vector3(0, 0, 0), effect, 8);
            b.UpdateEvent += UpdateBall;
            ((PolyRender_VPNTC)b.Renderer).Texture = tex2;
            ((PolyRender_VPNTC)b.Renderer).WireFrame = true;
            b.Initialize(world);

            ((PolyRender_MULTITEX)terrain.Renderer).Effect = terrainShader;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            world.Update(gameTime);

            if (!Host.IsActive)
                world.MainCamera.MouseLook = false;

            keys.Update();

            UpdateUI();
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
        public override void Draw(GameTime gameTime)
        {
            Host.GraphicsDevice.Clear(Color.CornflowerBlue);

            Host.GraphicsDevice.BlendState = BlendState.Opaque;
            Host.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Host.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

            world.Render();
            
            spriteBatch.Begin();
            spriteBatch.Draw(terrainHeight,
                new Rectangle(
                    Host.GraphicsDevice.Viewport.Width - 120,
                    Host.GraphicsDevice.Viewport.Height - 120,
                    120, 120), Color.White);

            UI.Render(spriteBatch);

            spriteBatch.End();

            FramerateCounter.OnDraw(gameTime);
        }

        /// <summary>
        /// Updates the camera tracking ball
        /// </summary>
        /// <param name="args">The update event args</param>
        public void UpdateBall(UpdateEventArgs args)
        {
            Ball ball = (Ball)args.Instance;
            
            ball.Position = terrain.TranslateVector(world.MainCamera.CameraPos);
            ball.Rotation = Math2.GetPitchRollYaw(world.MainCamera.CameraNormal);
        }

        public void QPressed(KeyDownEventArgs args)
        {
            world.MainCamera.MouseLook = !world.MainCamera.MouseLook;
        }

        public void EscPressed(KeyDownEventArgs args)
        {
            Enabled = false;
        }
        
        internal static byte[] LoadEffectResource(string name)
        {
            Stream stream = File.OpenRead(Directory.GetCurrentDirectory()  + "/" + name);

            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
