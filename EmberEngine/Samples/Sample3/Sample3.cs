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
using EmberEngine.GUI;
using EmberEngine.Tools.Variables;

namespace Samples.Sample3
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Sample : ISample
    {
        SpriteBatch spriteBatch;
        
        KeyManager keys;

        GUIPanel BaseContainer;
        TString FPSTracker;
        
        public Sample(Game game)
            : base(game)
        {
            Initialize();
            game.Window.Title = "GUI Demo";
            Enabled = true;
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

            keys.AddKeyWatcher(new KeyWatcher(Key.Tilde));
            keys.Watchers[0].AddPressed(EscPressed);

            LoadContent();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected  override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Host.GraphicsDevice);

            BaseContainer = new GUIPanel(new Rectangle(0, 0, 240, 240));
            BaseContainer.Mask = Host.Content.Load<Texture2D>("Sample3/Textures/menuRoundMask");
            BaseContainer.Texture = Host.Content.Load<Texture2D>("Sample3/Textures/rust");

            FPSTracker = new TString();

            GUIVariableRepresenter<string> fpsElement = new GUIVariableRepresenter<string>(new Rectangle(5, 5, 230, 10),
                GUI.Fonts["GUIFont1"], FPSTracker);

            GUILabel GUILabel = new GUILabel(new Rectangle(5, 5, 225, 10), GUI.Fonts["GUIFont1"],
                "null");
            GUILabel.BackColor = Color.DarkBlue;
            GUILabel.TextColor = Color.White;
            GUILabel.AutoSize = true;
            GUILabel.Text = "fus ro dah! This is supposed to wrap to the next line!";

            GUITextPane textPane = new GUITextPane(new Rectangle(5, 5, GUILabel.Width, 100), 
                GUI.Fonts["GUIFont1"]);
            textPane.Text =
                "this should be a really long sentance to demonstrate the awesomeness " +
                "of the GUI text panes. :P See how long this sentence is? it extends " + 
                "beyond the height of the pane, so a scroll bar is added, isn't that neat?";
            textPane.Texture = Host.Content.Load<Texture2D>("Sample3/Textures/parchment");
            
            BaseContainer.AddComponent(fpsElement);
            BaseContainer.AddComponent(GUILabel);
            BaseContainer.AddComponent(textPane);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            keys.Update();

            BaseContainer.Update(new GUIUpdateEventArgs(spriteBatch, gameTime));
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            Host.GraphicsDevice.Clear(Color.Black);

            FramerateCounter.OnDraw(gameTime);
            FPSTracker.Value = "FPS: " + FramerateCounter.FPS;
            spriteBatch.Begin();
            spriteBatch.DrawString(GUI.Fonts["sf1"], FramerateCounter.FPS.ToString(), 
                new Vector2(240, 10), Color.White);
            spriteBatch.End();

            BaseContainer.Draw(new GUIDrawEventArgs(spriteBatch));
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
