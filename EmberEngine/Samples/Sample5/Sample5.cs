using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Samples;
using EmberEngine.TwoD.Instances;
using EmberEngine.TwoD;
using EmberEngine.TwoD.Rendering;
using System.IO;

namespace Sample5
{
    public class Sample : ISample
    {
        Level MainLevel;
        SpriteBatch SpriteBatch;

        public Sample(Game Host) : base(Host)
        {
        }

        protected override void Initialize()
        {
            Host.Window.Title = "Sample 5 -- 2D";
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(Host.GraphicsDevice);

            Sprite test = new Sprite(Host.Content.Load<Texture2D>("Sample5/testSprite"), 8, 6, 
                TimeSpan.FromSeconds(0.5));
            test.YFrame = 2;
            test.Scale = new Vector2(2, 2);

            Sprite test2 = new Sprite(Host.Content.Load<Texture2D>("Sample5/Sprites/car_ragTop"), 1, 1, 
                TimeSpan.FromSeconds(0.5));
            test2.Scale = new Vector2(0.5F);

            MainLevel = new Level(1000);
            MainLevel.AddInstance(new BasicInstance(test.Clone(), new Vector2(100, 100)));
            MainLevel.AddInstance(new BasicInstance(test.Clone(), new Vector2(250, 175)));
            MainLevel.AddInstance(new BasicInstance(test.Clone(), new Vector2(300, 72)));
            int id = MainLevel.AddInstance(new BasicCar(test2.Clone(), new Vector2(300, 72)));

            MainLevel.trackID = id;

            TileRenderer tRender = new TileRenderer(Host.Content.Load<Texture2D>("Sample5/tileSheet"), 5, 5);
            TileMap tileMap = new TileMap(tRender, 100, 50, new Vector2(0, 0), 0);

            tileMap.SetTile(0, 0, 3);
            tileMap.SetTile(1, 0, 1);
            tileMap.SetTile(2, 0, 15);
            tileMap.SetTile(3, 0, 1);
            tileMap.SetTile(4, 0, 4);

            tileMap.SetTile(0, 1, 0);
            tileMap.SetTile(1, 1, 2);
            tileMap.SetTile(2, 1, 0);
            tileMap.SetTile(3, 1, 2);
            tileMap.SetTile(4, 1, 0);

            tileMap.SetTile(0, 2, 20);
            tileMap.SetTile(1, 2, 1);
            tileMap.SetTile(2, 2, 11);
            tileMap.SetTile(3, 2, 1);
            tileMap.SetTile(4, 2, 5);

            tileMap = TileMap.Load(Directory.GetCurrentDirectory() + "/Content/Sample5/Plaza.tlmp", tRender);

            tileMap.Depth = 0.1F;
            tileMap.Position = Vector2.Zero;
            tileMap.Scale = new Vector2(0.5F);

            MainLevel.tileMap = tileMap;

            MainLevel.BackgroundColor = Color.Blue;
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.OemTilde))
                this.Enabled = false;

            MainLevel.Update(gameTime);
        }
        
        public override void Draw(GameTime gameTime)
        {
            MainLevel.Draw(SpriteBatch);
        }
    }
}