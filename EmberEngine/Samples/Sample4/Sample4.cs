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

namespace Sample4
{
    public struct MyOwnVertexFormat
    {
        public Vector3 position;
        private Vector2 texCoord;
        private Vector3 normal;

        public MyOwnVertexFormat(Vector3 position, Vector2 texCoord, Vector3 normal)
        {
            this.position = position;
            this.texCoord = texCoord;
            this.normal = normal;
        }

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
             (
                 new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                 new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
                 new VertexElement(sizeof(float) * (3 + 2), VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
             );
    }

    public class Sample : ISample
    {
        Effect effect;
        Matrix viewMatrix;
        Matrix projectionMatrix;
        VertexBuffer vertexBuffer;
        Vector3 cameraPos;
        Texture2D streetTexture;

        Model carModel;
        Texture2D[] carTextures;
        Vector3 lightPos;
        float lightPower;
        float ambientPower;
        Matrix lightsViewProjectionMatrix;
        RenderTarget2D renderTarget;
        Texture2D shadowMap;
        Texture2D carLight;
        
        public Sample(Game Host) : base(Host)
        {
        }

        protected override void Initialize()
        {
            Host.Window.Title = "Riemer's XNA Tutorials -- Shadowed scene";
        }

        protected override void LoadContent()
        {
            effect = Host.Content.Load<Effect>("Common/Shaders/Shadowed");
            SetUpVertices();
            SetUpCamera();


            streetTexture = Host.Content.Load<Texture2D>("Common/Textures/dirt"); 

            PresentationParameters pp = Host.GraphicsDevice.PresentationParameters;
            renderTarget = new RenderTarget2D(Host.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);


            carLight = Host.Content.Load<Texture2D>("Common/Textures/carlight");
        }

        private void SetUpVertices()
        {
            MyOwnVertexFormat[] vertices = new MyOwnVertexFormat[18];

            vertices[0] = new MyOwnVertexFormat(new Vector3(-20, 0, 10), new Vector2(-0.25f, 25.0f), new Vector3(0, 1, 0));
            vertices[1] = new MyOwnVertexFormat(new Vector3(-20, 0, -100), new Vector2(-0.25f, 0.0f), new Vector3(0, 1, 0));
            vertices[2] = new MyOwnVertexFormat(new Vector3(2, 0, 10), new Vector2(0.25f, 25.0f), new Vector3(0, 1, 0));
            vertices[3] = new MyOwnVertexFormat(new Vector3(2, 0, -100), new Vector2(0.25f, 0.0f), new Vector3(0, 1, 0));
            vertices[4] = new MyOwnVertexFormat(new Vector3(2, 0, 10), new Vector2(0.25f, 25.0f), new Vector3(-1, 0, 0));
            vertices[5] = new MyOwnVertexFormat(new Vector3(2, 0, -100), new Vector2(0.25f, 0.0f), new Vector3(-1, 0, 0));
            vertices[6] = new MyOwnVertexFormat(new Vector3(2, 1, 10), new Vector2(0.375f, 25.0f), new Vector3(-1, 0, 0));
            vertices[7] = new MyOwnVertexFormat(new Vector3(2, 1, -100), new Vector2(0.375f, 0.0f), new Vector3(-1, 0, 0));
            vertices[8] = new MyOwnVertexFormat(new Vector3(2, 1, 10), new Vector2(0.375f, 25.0f), new Vector3(0, 1, 0));
            vertices[9] = new MyOwnVertexFormat(new Vector3(2, 1, -100), new Vector2(0.375f, 0.0f), new Vector3(0, 1, 0));
            vertices[10] = new MyOwnVertexFormat(new Vector3(3, 1, 10), new Vector2(0.5f, 25.0f), new Vector3(0, 1, 0));
            vertices[11] = new MyOwnVertexFormat(new Vector3(3, 1, -100), new Vector2(0.5f, 0.0f), new Vector3(0, 1, 0));
            vertices[12] = new MyOwnVertexFormat(new Vector3(13, 1, 10), new Vector2(0.75f, 25.0f), new Vector3(0, 1, 0));
            vertices[13] = new MyOwnVertexFormat(new Vector3(13, 1, -100), new Vector2(0.75f, 0.0f), new Vector3(0, 1, 0));
            vertices[14] = new MyOwnVertexFormat(new Vector3(13, 1, 10), new Vector2(0.75f, 25.0f), new Vector3(-1, 0, 0));
            vertices[15] = new MyOwnVertexFormat(new Vector3(13, 1, -100), new Vector2(0.75f, 0.0f), new Vector3(-1, 0, 0));
            vertices[16] = new MyOwnVertexFormat(new Vector3(13, 21, 10), new Vector2(1.25f, 25.0f), new Vector3(-1, 0, 0));
            vertices[17] = new MyOwnVertexFormat(new Vector3(13, 21, -100), new Vector2(1.25f, 0.0f), new Vector3(-1, 0, 0));

            vertexBuffer = new VertexBuffer(Host.GraphicsDevice, MyOwnVertexFormat.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);

        }

        private void SetUpCamera()
        {
            cameraPos = new Vector3(-25, 13, 18);
            viewMatrix = Matrix.CreateLookAt(cameraPos, new Vector3(0, 2, -12), new Vector3(0, 1, 0));
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 
                Host.GraphicsDevice.Viewport.AspectRatio, 1.0f, 200.0f);
        }
        
        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Enabled = false;

            UpdateLightData();
        }

        private void UpdateLightData()
        {
            ambientPower = 0.2f;

            lightPos = new Vector3(-18, 5, -2);
            lightPower = 2f;

            Matrix lightsView = Matrix.CreateLookAt(lightPos, new Vector3(-2, 3, -10), new Vector3(0, 1, 0));
            Matrix lightsProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, 1f, 5f, 100f);

            lightsViewProjectionMatrix = lightsView * lightsProjection;
        }
        
        public override void Draw(GameTime gameTime)
        {
            Host.GraphicsDevice.SetRenderTarget(renderTarget);
            Host.GraphicsDevice.Clear(Color.Black);

            DrawScene("ShadowMap");

            Host.GraphicsDevice.SetRenderTarget(null);

            shadowMap = (Texture2D)renderTarget;

            Host.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            DrawScene("ShadowedScene");
            shadowMap = null;
        }

        private void DrawScene(string technique)
        {
            effect.CurrentTechnique = effect.Techniques[technique];
            effect.Parameters["xCamerasViewProjection"].SetValue(viewMatrix * projectionMatrix);
            effect.Parameters["xLightsViewProjection"].SetValue(lightsViewProjectionMatrix);
            effect.Parameters["xWorld"].SetValue(Matrix.Identity);
            effect.Parameters["xTexture"].SetValue(streetTexture);
            effect.Parameters["xLightPos"].SetValue(lightPos);
            effect.Parameters["xLightPower"].SetValue(lightPower);
            effect.Parameters["xAmbient"].SetValue(ambientPower);
            effect.Parameters["xShadowMap"].SetValue(shadowMap);
            effect.Parameters["xCarLightTexture"].SetValue(carLight);

            //Host.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                Host.GraphicsDevice.SetVertexBuffer(vertexBuffer);
                Host.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 16);
            }
        }
    }
}