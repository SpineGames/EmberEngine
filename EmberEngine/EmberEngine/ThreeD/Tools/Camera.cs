///Represents a camera with it's own view parameters
///© 2013 Spine Games

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using EmberEngine.Tools.AdvancedMath;
using EmberEngine.Tools.Input;
using Microsoft.Xna.Framework.Graphics;
using OpenTK.Input;
using EmberEngine.ThreeD.Rendering.ShaderWrappers;

namespace EmberEngine.ThreeD
{
    /// <summary>
    /// Represents a view from an orgin
    /// </summary>
    public class ThreeDCamera
    {
        #region Private Variables
        /// <summary>
        /// The orgin point of the camera
        /// </summary>
        Vector3 cameraPos;
        /// <summary>
        /// The normal for the camera
        /// </summary>
        Vector3 cameraNormal;
        /// <summary>
        /// The Yaw around the z-axis
        /// </summary>
        float cameraYaw;
        /// <summary>
        /// The up/down pitch of the camera
        /// </summary>
        float cameraPitch;

        /// <summary>
        /// The graphics device manager that this camera was created on
        /// </summary>
        GraphicsDevice graphics;

        /// <summary>
        /// The private acessor for keyManager
        /// </summary>
        private KeyManager keyManager;

        /// <summary>
        /// The view parameters for this camera
        /// </summary>
        ViewParameters view;

        /// <summary>
        /// Private acessor for fov
        /// </summary>
        float fov = 45;
        /// <summary>
        /// Private value of NearClipPlane
        /// </summary>
        float nearClipPlane = 0.1F;
        /// <summary>
         /// Private value of FarClipPlane
         /// </summary>
        float farClipPlane = 1000.0F;
        /// <summary>
        /// PRivate acessor for AspectRatio
        /// </summary>
        float aspectRatio = 1.25F;
        #endregion

        #region Public Variables
        /// <summary>
        /// True if the mouse can be used to look around
        /// </summary>
        public bool MouseLook;
        /// <summary>
        /// The orgin point of the camera
        /// </summary>
        public Vector3 CameraPos
        {
            get { return cameraPos; }
            set { cameraPos = value; }
        }
        /// <summary>
        /// The normal for the camera
        /// </summary>
        public Vector3 CameraNormal
        {
            get
            {
                Vector3 temp = Math2.GetFromYawPitch(CameraYaw, CameraPitch - 90, 1);
                temp.Normalize();
                return temp;
            }
        }
        /// <summary>
        /// The left-facing vector to the camera
        /// </summary>
        public Vector3 CameraCrossNormal
        {
            get
            {
                Vector3 temp = Vector3.Cross(CameraNormal, UpVector);
                temp.Normalize();
                return temp;
            }
        }
        /// <summary>
        /// The Yaw around the z-axis
        /// </summary>
        public float CameraYaw
        {
            get { return cameraYaw; }
            set { cameraYaw = value.Wrap(0, 360); }
        }
        /// <summary>
        /// The up/down pitch of the camera
        /// </summary>
        public float CameraPitch
        {
            get { return cameraPitch; }
            set { cameraPitch = MathHelper.Clamp(value, -89, 89); }
        }
        /// <summary>
        /// Gets the movement options for this camera
        /// </summary>
        public CameraMoveOptions MoveOptions;
        /// <summary>
        /// Gets this camera's key manager
        /// </summary>
        public KeyManager KeyManager { get { return keyManager; } }

        /// <summary>
        /// Sets the feild of view for the projection matrix
        /// </summary>
        public float FOV
        {
            get { return fov; }
            set
            {
                fov = value;

                RebuildProjection();
            }
        }
        /// <summary>
         /// The distance to the caera at which object begin to render
         /// <b>default 0.1</b>
         /// </summary>
        public float NearClipPane
         {
             get { return nearClipPlane; }
             set { nearClipPlane = value; RebuildProjection(); }
         }
        /// <summary>
         /// The distance to the caera at which object cease to render
         /// <b>default 1000.0</b>
         /// </summary>
        public float FarClipPlane
         {
             get { return farClipPlane; }
             set { farClipPlane = value; RebuildProjection(); }
         }
        /// <summary>
        /// The camera's Aspect ratio <b>default 1.25</b>
        /// <i>Can be calculated using width / height</i>
        /// </summary>
        public float AspectRatio
         {
             get { return aspectRatio; }
             set { aspectRatio = value; RebuildProjection(); }
         }

        /// <summary>
        /// Gets or sets this camera's up vector
        /// <b>default {0,0,1}</b>
        /// </summary>
        public Vector3 UpVector = new Vector3(0, 0, 1);
        #endregion

        /// <summary>
        /// Gets this camera's view matrix
        /// </summary>
        public Matrix View
        {
            get { return view.View; }
        }
        /// <summary>
        /// Gets or sets this camera's projection
        /// <b>note: changes to FOV, NearClipPlane, FarClipPlane, and AspectRatio
        /// will ovveride this</b>
        /// </summary>
        public Matrix Projection
        {
            get { return view.Projection; }
            set { view.Projection = value; }
        }
        /// <summary>
        /// Gets or sets the camera's world projection
        /// </summary>
        public Matrix World
        {
            get { return view.World; }
            set { view.World = value; }
        }

        /// <summary>
        /// Creates a new camera instance
        /// </summary>
        /// <param name="Position">The camera's position</param>
        /// <param name="graphics">The GraphicsDeviceManager to use</param>
        public ThreeDCamera(Vector3 Position, GraphicsDevice graphics, CameraMoveOptions MoveOptions)
        {
            this.cameraPos = Position;
            this.cameraNormal = new Vector3(1, 0, 0);
            this.graphics = graphics;

            view = new ViewParameters();
            view.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(60),
                (float)graphics.Viewport.Width /
                (float)graphics.Viewport.Height,
                0.1f, 1000.0f);

            view.View = Matrix.CreateLookAt(CameraPos, CameraPos + CameraNormal, new Vector3(0, 0, 1));
            view.World = Matrix.CreateTranslation(Vector3.Zero);

            this.MoveOptions = MoveOptions;
            keyManager = new KeyManager();

            if (MoveOptions.CanMove)
            {
                keyManager.AddKeyWatcher(MoveOptions.Forward.AddDown(OnForwardDown));
                keyManager.AddKeyWatcher(MoveOptions.Backward.AddDown(OnBackwardDown));
                keyManager.AddKeyWatcher(MoveOptions.Left.AddDown(OnLeftDown));
                keyManager.AddKeyWatcher(MoveOptions.Right.AddDown(OnRightDown));
            }
            if (MoveOptions.CanLook & !MoveOptions.MouseLook)
            {

                keyManager.AddKeyWatcher(MoveOptions.LookUp.AddDown(OnLookUpDown));
                keyManager.AddKeyWatcher(MoveOptions.LookDown.AddDown(OnLookDownDown));
                keyManager.AddKeyWatcher(MoveOptions.LookLeft.AddDown(OnLookLeftDown));
                keyManager.AddKeyWatcher(MoveOptions.LookRight.AddDown(OnLookRightDown));
            }

            int centreX = graphics.Viewport.Width / 2;
            int centreY = graphics.Viewport.Height / 2;

            Microsoft.Xna.Framework.Input.Mouse.SetPosition(centreX, centreY);
        }

        /// <summary>
        /// Force update the view parameter
        /// </summary>
        public void UpdateViewParameters()
        {
            view.View = Matrix.CreateLookAt(CameraPos, CameraPos + CameraNormal,
                UpVector);
        }

        /// <summary>
        /// Rebuilds the projection matrix
        /// </summary>
        private void RebuildProjection()
        {
            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(FOV),
                AspectRatio,
                NearClipPane, FarClipPlane);
        }
        
        /// <summary>
        /// Handles moing the camera around using W/A/S/D and arrow keys
        /// </summary>
        /// <param name="window">The window to render with</param>
        /// <param name="gameTime">The current game time</param>
        public void Update(GameTime gameTime)
        {
            keyManager.Update();

            if (MouseLook & MoveOptions.MouseLook)
            {
                Microsoft.Xna.Framework.Input.MouseState m = Microsoft.Xna.Framework.Input.Mouse.GetState();

                int centreX = graphics.Viewport.Width / 2;
                int centreY = graphics.Viewport.Height / 2;

                int xChange = centreX - m.X;
                int yChange = centreY - m.Y;

                if (Math.Abs(xChange) > 1 | Math.Abs(yChange) > 1)
                {
                    CameraYaw += (float)xChange * MoveOptions.MouseSensitivity;
                    CameraPitch += (float)yChange * MoveOptions.MouseSensitivity;

                Microsoft.Xna.Framework.Input.Mouse.SetPosition(centreX, centreY);
                }
            }
            if (!MouseLook)
            {
                System.Diagnostics.Debug.Write("f");
            }

            UpdateViewParameters();
        }

        /// <summary>
        /// Forces the camera's normal to a given normal
        /// </summary>
        /// <param name="Normal">The normal to forc to</param>
        public void ForceNormal(Vector3 Normal)
        {
            this.cameraNormal = Normal;
        }

        /// <summary>
        /// Applies this camera's view parameters to an effect
        /// </summary>
        /// <param name="Effect">The basic effect to apply to</param>
        public void ApplyToEffect(BasicEffect Effect)
        {
            Effect.View = View;
            Effect.Projection = Projection;
            Effect.World = World;
        }

        /// <summary>
        /// Applies this camera's view parameters to an effect
        /// </summary>
        /// <param name="Effect">The effect to apply to</param>
        /// <param name="ViewName">The effect parameter name to apply the view to</param>
        /// <param name="ProjectionName">The effect parameter name to apply the projection to</param>
        /// <param name="WorldName">The effect parameter name to apply the world to</param>
        public void ApplyToEffect(Shader Shader, Matrix World, string ViewName = "View", 
            string ProjectionName = "Projection", string WorldName = "World")
        {
            if (!Shader.HasParameter("WorldViewProj"))
            {
                Shader.SetParameterMatrix(ViewName, View);
                Shader.SetParameterMatrix(ProjectionName, Projection);
                Shader.SetParameterMatrix(WorldName, this.World * World);
            }
            else
                Shader.SetParameterMatrix("WorldViewProj", this.World * World * View * Projection);
        }

        #region Input
        #region Move
        private void OnForwardDown(KeyDownEventArgs e)
        {
            CameraPos += CameraNormal * MoveOptions.ForwardSpeed;
        }

        private void OnBackwardDown(KeyDownEventArgs e)
        {
            CameraPos -= CameraNormal * MoveOptions.BackwardSpeed;
        }

        private void OnLeftDown(KeyDownEventArgs e)
        {
            CameraPos -= CameraCrossNormal * MoveOptions.StrafeSpeed; ;
        }

        private void OnRightDown(KeyDownEventArgs e)
        {
            CameraPos += CameraCrossNormal * MoveOptions.StrafeSpeed;
        }
        #endregion

        #region Look
        private void OnLookUpDown(KeyDownEventArgs e)
        {
            CameraPitch += MoveOptions.KeyLookSensitivity;
        }

        private void OnLookDownDown(KeyDownEventArgs e)
        {
            CameraPitch -= MoveOptions.KeyLookSensitivity;
        }

        private void OnLookLeftDown(KeyDownEventArgs e)
        {
            CameraYaw += MoveOptions.KeyLookSensitivity;
        }

        private void OnLookRightDown(KeyDownEventArgs e)
        {
            CameraYaw -= MoveOptions.KeyLookSensitivity;
        }
        #endregion
        #endregion

        #region Prebuild Camera Options
        #region PLAYER_WASD_NOMOUSE
        /// <summary>
        /// Prebuilt camera for W/A/S/D player with no mouse look
        /// </summary>
        public static readonly CameraMoveOptions PLAYER_WASD_NOMOUSE = new CameraMoveOptions()
        { 
            CanMove = true,

            ForwardSpeed = 1F,
            BackwardSpeed = 1F,
            StrafeSpeed = 1,

            Forward = new KeyWatcher(Key.W),
            Backward = new KeyWatcher(Key.S),
            Left = new KeyWatcher(Key.A),
            Right = new KeyWatcher(Key.D),
            
            CanLook = true,

            MouseLook = false,
            MouseSensitivity = 1F,

            KeyLookSensitivity = 1F,

            LookUp = new KeyWatcher(Key.Up),
            LookDown = new KeyWatcher(Key.Down),
            LookLeft = new KeyWatcher(Key.Left),
            LookRight = new KeyWatcher(Key.Right)
        };
        #endregion

        #region PLAYER_WASD_MOUSE
        /// <summary>
        /// Prebuilt camera for W/A/S/D player with mouse look
        /// </summary>
        public static readonly CameraMoveOptions PLAYER_WASD_MOUSE = new CameraMoveOptions()
        {
            CanMove = true,

            ForwardSpeed = 1F,
            BackwardSpeed = 1F,
            StrafeSpeed = 1,

            Forward = new KeyWatcher(Key.W),
            Backward = new KeyWatcher(Key.S),
            Left = new KeyWatcher(Key.A),
            Right = new KeyWatcher(Key.D),

            CanLook = true,

            MouseLook = true,
            MouseSensitivity = 0.1F,

            KeyLookSensitivity = 1F,

            LookUp = new KeyWatcher(Key.Up),
            LookDown = new KeyWatcher(Key.Down),
            LookLeft = new KeyWatcher(Key.Left),
            LookRight = new KeyWatcher(Key.Right)
        };
        #endregion

        #region STATIC
        /// <summary>
        /// Prebuilt camera options for a static camera
        /// </summary>
        public static readonly CameraMoveOptions STATIC = new CameraMoveOptions()
        {
            CanMove = false,

            ForwardSpeed = 0F,
            BackwardSpeed = 0F,
            StrafeSpeed = 0,

            Forward = new KeyWatcher(Key.W),
            Backward = new KeyWatcher(Key.S),
            Left = new KeyWatcher(Key.A),
            Right = new KeyWatcher(Key.D),

            CanLook = false,

            MouseLook = false,
            MouseSensitivity = 0F,

            KeyLookSensitivity = 0,

            LookUp = new KeyWatcher(Key.Up),
            LookDown = new KeyWatcher(Key.Down),
            LookLeft = new KeyWatcher(Key.Left),
            LookRight = new KeyWatcher(Key.Right)
        };
        #endregion
        #endregion

        /// <summary>
        /// overrides the ToString() method
        /// </summary>
        /// <returns>This camera's debug details</returns>
        public override string ToString()
        {
            return 
                "Position: " + CameraPos.ToString() + "\n" +
                "Normal:   " + CameraNormal.ToString() + "\n" +
                "Yaw:      " + CameraYaw.ToString() + "\n" +
                "Pitch:    " + CameraPitch.ToString();
        }

        /// <summary>
        /// Represents the possible camera movement options
        /// </summary>
        public struct CameraMoveOptions
        {
            /// <summary>
            /// True if this camera can move, will disable all move input if false
            /// </summary>
            public bool CanMove;

            /// <summary>
            /// The speed at which the camera can move forwards
            /// </summary>
            public float ForwardSpeed;
            /// <summary>
            /// The speed at which the camera can move backwards
            /// </summary>
            public float BackwardSpeed;
            /// <summary>
            /// The speed at which the camera can move side to side
            /// </summary>
            public float StrafeSpeed;

            /// <summary>
            /// The key watcher for the forward movement key
            /// </summary>
            public KeyWatcher Forward;
            /// <summary>
            /// The key watcher for the backward movement key
            /// </summary>
            public KeyWatcher Backward;
            /// <summary>
            /// The key watcher for the left strafe key
            /// </summary>
            public KeyWatcher Left;
            /// <summary>
            /// The key watcher for the right strafe key
            /// </summary>
            public KeyWatcher Right;
            
            /// <summary>
            /// True if this camera can look around, will disable all look input if false
            /// </summary>
            public bool CanLook;

            /// <summary>
            /// True if this camera uses mouse look
            /// </summary>
            public bool MouseLook;
            /// <summary>
            /// The sensitivity of the mouse looking
            /// </summary>
            public float MouseSensitivity;

            /// <summary>
            /// The sensitivity of the keyboard looking
            /// </summary>
            public float KeyLookSensitivity;

            /// <summary>
            /// The key watcher for looking up
            /// </summary>
            public KeyWatcher LookUp;
            /// <summary>
            /// The keywatcher for looking down
            /// </summary>
            public KeyWatcher LookDown;
            /// <summary>
            /// The key watcher for looking left
            /// </summary>
            public KeyWatcher LookLeft;
            /// <summary>
            /// The key watcher for looking right
            /// </summary>
            public KeyWatcher LookRight;
        }
    }

    /// <summary>
    /// Represents a camera's view paramaters
    /// </summary>
    public struct ViewParameters
    {
        /// <summary>
        /// The view matrix
        /// </summary>
        public Matrix View;
        /// <summary>
        /// The projection matrix
        /// </summary>
        public Matrix Projection;
        /// <summary>
        /// The world matrix
        /// </summary>
        public Matrix World;
    }
}
