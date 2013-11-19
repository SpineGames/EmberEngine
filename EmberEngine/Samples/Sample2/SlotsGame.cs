using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using EmberEngine.ThreeD;
using EmberEngine.ThreeD.Rendering.ShaderWrappers;
using System.Timers;
using EmberEngine.ThreeD.Tools;
using EmberEngine.Tools.Management;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using EmberEngine.ThreeD.Rendering;
using EmberEngine.Tools.AdvancedMath;
using EmberEngine.Tools.Input;
using OpenTK.Input;

namespace Samples.Sample2
{
    public static class SlotsGame
    {
        static float AngleMulti = 360.0F / 6.0F;
        static Dictionary<string, int> IDS = new Dictionary<string, int>();
        static Dictionary<int, Texture2D> WinTexs = new Dictionary<int, Texture2D>();
        static Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
        static World world;

        static Timer Spinner1;
        static Timer Spinner2;
        static Timer Spinner3;
        static Timer Spinner4;

        static int[] Results = new int[4];

        static bool CanSpin = true;
        public static int money;

        static Random rand1 = new Random();
        static Random rand2 = new Random(DateTime.Now.Millisecond + rand1.Next());
        static Random rand3 = new Random(DateTime.Now.Hour + rand2.Next());
        static Random rand4 = new Random(DateTime.Now.Second + rand3.Next());
        
        /// <summary>
        /// Gets or sets the speen of the spinners
        /// </summary>
        public static float SpinnerSpeed = 0.4F;

        /// <summary>
        /// Initializes this Slots game
        /// </summary>
        /// <param name="World">The world to add the components to</param>
        /// <param name="SpinnerEffect">The shader to use for the spinners</param>
        public static void Intialize(ref World World, GraphicsDevice graphics, ContentManager Content, 
            SpriteBatch spriteBatch)
        {
            world = World;
            
            #region Content Loading
            Texture2D spinner = Content.Load<Texture2D>("Sample2/Textures/Spinner");
            Texture2D grid = Content.Load<Texture2D>("Sample2/Textures/grid");
            SpriteFont spriteFont = FontManager.Fonts["QuartzFont"];

            CelShader toon = new CelShader(Content.Load<Effect>("Common/Shaders/CelShader"));
            CelShader toon2 = new CelShader(Content.Load<Effect>("Common/Shaders/CelShader2"));

            toon.DiffuseColor = Color.White;
            toon.ToonLevel = 4;
            toon.Texture = spinner;

            toon2.DiffuseColor = Color.LightGray;
            toon2.ToonLevel = 4;
            toon2.Texture = spinner;
            #endregion

            #region Dual Money Tile
            DrawWinningTagMoney(spriteBatch, spriteFont, "x2 ~ $50", spinner, "2MoneyWin");
            #endregion

            #region Triple Money Tile
            DrawWinningTagMoney(spriteBatch, spriteFont, "x3 ~ $200", spinner, "3MoneyWin");
            #endregion

            #region Quadruple Money Tile
            DrawWinningTagMoney(spriteBatch, spriteFont, "x4 ~ $1000", spinner, "4MoneyWin");
            #endregion

            #region Double Any Tile
            DrawWinningTagAny(spriteBatch, spriteFont, "x 2 ~ $5", "2AnyWin");
            #endregion

            #region Triple Any Tile
            DrawWinningTagAny(spriteBatch, spriteFont, "x 3 ~ $10", "3AnyWin");
            #endregion

            #region Quadruple Any Tile
            DrawWinningTagAny(spriteBatch, spriteFont, "x 4 ~ $50", "4AnyWin");
            #endregion

            spriteBatch.GraphicsDevice.SetRenderTarget(null);

            #region WinTex Initialization
            Textures.Add("Blank", StringRender.RenderToTarget(graphics, spriteBatch, " ",
                spriteFont, Color.Red, Color.White));
            WinTexs.Add(-1, StringRender.RenderToTarget(graphics, spriteBatch, "Winnings",
                spriteFont, Color.Red, Color.White));
            WinTexs.Add(0, StringRender.RenderToTarget(graphics, spriteBatch, "   $0   ",
                spriteFont, Color.Red, Color.White));
            WinTexs.Add(5, StringRender.RenderToTarget(graphics, spriteBatch, "   $5   ",
                spriteFont, Color.Red, Color.White));
            WinTexs.Add(10, StringRender.RenderToTarget(graphics, spriteBatch, "  $10   ",
                spriteFont, Color.Red, Color.White));
            WinTexs.Add(50, StringRender.RenderToTarget(graphics, spriteBatch, "  $50  ",
                spriteFont, Color.Red, Color.White));
            WinTexs.Add(200, StringRender.RenderToTarget(graphics, spriteBatch, "  $200  ",
                spriteFont, Color.Red, Color.White));
            WinTexs.Add(1000, StringRender.RenderToTarget(graphics, spriteBatch, "  $1000 ",
                spriteFont, Color.Red, Color.White));
            #endregion

            #region Add Spinners
            float x = -5.5F;
            for (int i = 0; i < 4; i++)
            {
                Spinner temp = new Spinner(new Vector3(x, 0, 2), new Vector3(x + 2, 0, 2), 
                    toon, 1);
                temp.RotationSpeed = new Vector3(0, 0, 0);
                temp.Rotation = new Vector3(GetAngleForResult(0), 0, 0);
                temp.Initialize(world);
                IDS.Add("Spinner" + (i + 1), temp.ID);
                x += 3F;
            }
            #endregion

            #region Front
            Plane front = new Plane(new Vector3(-6F, -0.86F, -1), new Vector3(6F, -0.86F, 3), 
                toon, new Vector2(1, -1));
            ((PolyRender_VPNTC)front.Renderer).Texture = Textures["Blank"];
            front.Initialize(world);
            IDS.Add("Front", front.ID);
            #endregion

            #region Add Header
            toon.Texture = WinTexs[-1];
            Plane header = new Plane(new Vector3(-6, 0, 4), new Vector3(6, 2, 7), toon, new Vector2(1, -1));
            header.Initialize(world);
            IDS.Add("Header", header.ID);
            #endregion

            #region Add Grid Plane
            toon.Texture = grid;
            Plane gridPlane = new Plane(
                new Vector3(-10, -10, 0), new Vector3(10, 10, 0), toon, 20);
            gridPlane.Initialize(world);
            //IDS.Add("Grid", gridPlane.ID);
            #endregion

            #region Win Tiles
            Plane w1 = new Plane(new Vector3(-5.5F, -0.861F, 0.0F), new Vector3(-3.5F, -0.861F, 0.75F), 
                toon2, new Vector2(1, -1));
            ((PolyRender_VPNTC)w1.Renderer).Texture = Textures["3MoneyWin"];
            w1.Initialize(world);
            IDS.Add("WinTile_Money3", w1.ID);

            w1 = new Plane(new Vector3(-2.5F, -0.861F, 0.0F), new Vector3(-0.5F, -0.861F, 0.75F),
                toon2, new Vector2(1, -1));
            ((PolyRender_VPNTC)w1.Renderer).Texture = Textures["4MoneyWin"];
            w1.Initialize(world);
            IDS.Add("WinTile_Money4", w1.ID);

            w1 = new Plane(new Vector3(0.5F, -0.861F, 0.0F), new Vector3(2.5F, -0.861F, 0.75F),
                toon2, new Vector2(1, -1));
            ((PolyRender_VPNTC)w1.Renderer).Texture = Textures["2AnyWin"];
            w1.Initialize(world);
            IDS.Add("WinTile_Any2", w1.ID);

            w1 = new Plane(new Vector3(3.5F, -0.861F, 0.0F), new Vector3(5.5F, -0.861F, 0.75F),
                toon2, new Vector2(1, -1));
            ((PolyRender_VPNTC)w1.Renderer).Texture = Textures["3AnyWin"];
            w1.Initialize(world);
            IDS.Add("WinTile_Any3", w1.ID);

            w1 = new Plane(new Vector3(-1F, -0.861F, -1.0F), new Vector3(1F, -0.861F, -0.25F),
                toon2, new Vector2(1, -1));
            ((PolyRender_VPNTC)w1.Renderer).Texture = Textures["4AnyWin"];
            w1.Initialize(world);
            IDS.Add("WinTile_Any4", w1.ID);
            #endregion

            #region Timer Setup
            Spinner1 = new Timer(TimeSpan.FromSeconds(2).TotalMilliseconds);
            Spinner1.Elapsed += Spinner1_Elapsed;
            Spinner1.AutoReset = true;

            Spinner2 = new Timer(TimeSpan.FromSeconds(3.5F).TotalMilliseconds);
            Spinner2.Elapsed += new ElapsedEventHandler(Spinner2_Elapsed);
            Spinner2.AutoReset = true;

            Spinner3 = new Timer(TimeSpan.FromSeconds(5).TotalMilliseconds);
            Spinner3.Elapsed += new ElapsedEventHandler(Spinner3_Elapsed);
            Spinner3.AutoReset = true;

            Spinner4 = new Timer(TimeSpan.FromSeconds(6.5F).TotalMilliseconds);
            Spinner4.Elapsed += new ElapsedEventHandler(Spinner4_Elapsed);
            Spinner4.AutoReset = true;
            #endregion

            #region Shift Instances
            foreach (int i in IDS.Values)
            {
                world.instances[i].Position += new Vector3(0, 0, 1);
            }
            #endregion

            toon.DiffuseColor = new Color(0.9F, 0.9F, 0.9F);
            toon.DiffuseLightDirection = new Vector3(1, 1, 0);
            toon.ToonLevel = 4;
            toon.Texture = spinner;
        }

        public static void ResetGame(KeyDownEventArgs e)
        {
            money = 0;
        }

        /// <summary>
        /// Draws a texture from the spinner texture
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw with</param>
        /// <param name="spinner">The spinner texture</param>
        /// <param name="texID">The ID of the texture to draw</param>
        private static void DrawTexture(SpriteBatch spriteBatch, Texture2D spinner, int texID)
        {
            int texWidth = spinner.Width / 6;
            int texHeight = spinner.Height;
            spriteBatch.Draw(spinner, new Vector2(25, 25), new Rectangle(texID * texWidth, 0, texWidth, texHeight),
                Color.White, (float)(90.0).ToRad(), new Vector2(texWidth / 2, texHeight / 2),
                0.15F, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draws and saves a money winning tag
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw with</param>
        /// <param name="spriteFont">The font for the text</param>
        /// <param name="text">The text in the tag</param>
        /// <param name="spinner">The spinners texture</param>
        /// <param name="name">The texture-name</param>
        private static void DrawWinningTagMoney(SpriteBatch spriteBatch, SpriteFont spriteFont, 
            string text, Texture2D spinner, string name)
        {
            Vector2 fontSize = spriteFont.MeasureString(text) * 0.5F;
            Point Size = new Point((int)(45 + fontSize.X), 50);

            spriteBatch.Begin();
            RenderTarget2D tempRender = new RenderTarget2D(spriteBatch.GraphicsDevice, Size.X, 50);
            spriteBatch.GraphicsDevice.SetRenderTarget(tempRender);
            spriteBatch.GraphicsDevice.Clear(Color.White);

            DrawTexture(spriteBatch, spinner, 3);

            spriteBatch.DrawString(spriteFont, text, new Vector2(45, 10), Color.Red,
                0, new Vector2(0, 0), 0.5F, SpriteEffects.None, 0);
            Texture2D t = tempRender;
            Textures.Add(name, t);
            spriteBatch.End();
        }

        /// <summary>
        /// Draws and saves an any winning tag
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw with</param>
        /// <param name="spriteFont">The font for the text</param>
        /// <param name="text">The text in the tag</param>
        /// <param name="name">The texture-name</param>
        private static void DrawWinningTagAny(SpriteBatch spriteBatch, SpriteFont spriteFont,
            string text, string name)
        {
            Vector2 fontSize = spriteFont.MeasureString("Any " + text) * 0.5F;
            Point Size = new Point((int)(fontSize.X), 50);

            spriteBatch.Begin();
            RenderTarget2D tempRender = new RenderTarget2D(spriteBatch.GraphicsDevice, Size.X, 50);
            spriteBatch.GraphicsDevice.SetRenderTarget(tempRender);
            spriteBatch.GraphicsDevice.Clear(Color.White);

            spriteBatch.DrawString(spriteFont, "Any " + text, new Vector2(0, 10), Color.Red,
                0, new Vector2(0, 0), 0.5F, SpriteEffects.None, 0);
            Texture2D t = tempRender;
            Textures.Add(name, t);
            spriteBatch.End();
        }

        /// <summary>
        /// Starts a spine cycle
        /// </summary>
        public static void StartSpin()
        {
            if (CanSpin)
            {
                foreach (string s in IDS.Keys)
                {
                    if (s.Contains("Spinner"))
                        world.instances[IDS[s]].RotationSpeed = new Vector3(SpinnerSpeed, 0, 0);
                    if (s.Contains("WinTile"))
                        ((CelShader)((PolyRender_VPNTC)world.instances[IDS[s]].Renderer).Effect).
                            DiffuseColor = Color.LightGray;
                }

                CanSpin = false;

                Spinner1.Start();
                Spinner2.Start();
                Spinner3.Start();
                Spinner4.Start();

                money -= 10;
            }
        }

        /// <summary>
        /// Gets the Angle in radians for the given result
        /// </summary>
        /// <param name="result"> a result that is >= 0 and < 6</param>
        /// <returns></returns>
        public static float GetAngleForResult(int result)
        {
            if (result >= 0 & result < 6)
            {
                return (float)Math2.ToRad((AngleMulti * result) + 60);
            }
            else
                throw new IndexOutOfRangeException("The result " + result + "is out of range");
        }

        /// <summary>
        /// Updates this slots game
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Gets the amount won for the current results
        /// </summary>
        /// <returns></returns>
        private static int GetWinnings()
        {
            int maxCount = 0;

            for (int i = 0; i < 6; i++)
            {
                int equalCount = GetCount(i);
                maxCount = equalCount > maxCount ? equalCount : maxCount;
            }

            if (GetCount(3) == 3)
            {
                ((CelShader)((PolyRender_VPNTC)world.instances[IDS["WinTile_Money3"]].Renderer).Effect).
                    DiffuseColor = Color.Orange;
                return 200;
            }
            if (GetCount(3) == 4)
            {
                ((CelShader)((PolyRender_VPNTC)world.instances[IDS["WinTile_Money4"]].Renderer).Effect).
                    DiffuseColor = Color.Orange;
                return 1000;
            }
            if (maxCount == 2)
            {
                ((CelShader)((PolyRender_VPNTC)world.instances[IDS["WinTile_Any2"]].Renderer).Effect).
                    DiffuseColor = Color.Orange;
                return 5;
            }
            if (maxCount == 3)
            {
                ((CelShader)((PolyRender_VPNTC)world.instances[IDS["WinTile_Any3"]].Renderer).Effect).
                    DiffuseColor = Color.Orange;
                return 10;
            }
            if (maxCount == 4)
            {
                ((CelShader)((PolyRender_VPNTC)world.instances[IDS["WinTile_Any4"]].Renderer).Effect).
                    DiffuseColor = Color.Orange;
                return 50;
            }
            return 0;
        }

        /// <summary>
        /// Gets the count of spinners for the given state
        /// </summary>
        /// <param name="state">The state to check for (the ID on the spinner)</param>
        /// <returns>The number of spinners with a state of <i>ID</i></returns>
        private static int GetCount(int state)
        {
            return
                (Results[0] == state ? 1 : 0) +
                (Results[1] == state ? 1 : 0) +
                (Results[2] == state ? 1 : 0) +
                (Results[3] == state ? 1 : 0);
        }

        static void Spinner4_Elapsed(object sender, ElapsedEventArgs e)
        {
            world.instances[IDS["Spinner4"]].RotationSpeed = Vector3.Zero;
            Spinner4.Stop();

            CanSpin = true;

            Results[3] = rand4.Next(0, 6);
            world.instances[IDS["Spinner4"]].Rotation = new Vector3(GetAngleForResult(Results[3]), 0, 0);

            int epicWinnings = GetWinnings();
            money += epicWinnings;
            ((PolyRender_VPNTC)world.instances[IDS["Header"]].Renderer).Texture = WinTexs[epicWinnings];
        }

        static void Spinner3_Elapsed(object sender, ElapsedEventArgs e)
        {
            world.instances[IDS["Spinner3"]].RotationSpeed = Vector3.Zero;
            Spinner3.Stop();

            Results[2] = rand3.Next(0, 6);
            world.instances[IDS["Spinner3"]].Rotation = new Vector3(GetAngleForResult(Results[2]), 0, 0);
        }

        static void Spinner2_Elapsed(object sender, ElapsedEventArgs e)
        {
            world.instances[IDS["Spinner2"]].RotationSpeed = Vector3.Zero;
            Spinner2.Stop();

            Results[1] = rand2.Next(0, 6);
            world.instances[IDS["Spinner2"]].Rotation = new Vector3(GetAngleForResult(Results[1]), 0, 0);
        }

        static void Spinner1_Elapsed(object sender, ElapsedEventArgs e)
        {
            world.instances[IDS["Spinner1"]].RotationSpeed = Vector3.Zero;
            Spinner1.Stop();

            Results[0] = rand1.Next(0, 6);
            world.instances[IDS["Spinner1"]].Rotation = new Vector3(GetAngleForResult(Results[0]), 0, 0);
        }
    }
}
