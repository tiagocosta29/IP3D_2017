using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank
{
    class Camera
    {
        /// <summary>
        ///     Graphics device reference 
        /// </summary>
        private GraphicsDevice graphicsDevice;

        /// <summary>
        ///     Holds the camera position
        /// </summary>
        private Vector3 cameraPosition;

        /// <summary>
        ///     The view Matrix of the camera
        /// </summary>
        public Matrix ViewMatrix { get; set; }

        /// <summary>
        ///     The projection Matrix of the camera
        /// </summary>
        public Matrix ProjectionMatrix { get; set; }

        /// <summary>
        ///     Angle to apply rotation to the camera
        /// </summary>
        private float cameraAngleX;

        /// <summary>
        ///     Angle to apply rotation to the camera
        /// </summary>
        private float cameraAngleY;

        /// <summary>
        ///     Look at vector to aplly to the camera
        /// </summary>
        private Vector3 cameraLookAt;
        
        /// <summary>
        ///     Vertices to calculate surface follow
        /// </summary>
        private VertexPositionNormalTexture[] terrainVertices;

        /// <summary>
        ///     Stores the heightmap texture Width
        /// </summary>
        private int terrainWidth;

        /// <summary>
        ///     Stores the heightmap texture Height
        /// </summary>
        private int terrainHeight;

        /// <summary>
        ///     Camera Constructor
        /// </summary>
        /// <param name="device"></param>
        public Camera(GraphicsDevice device, VertexPositionNormalTexture[] vertices)
        {
            terrainWidth = Helper.MapWidth;
            terrainHeight = Helper.MapHeight;

            terrainVertices = vertices;
            graphicsDevice = device;
            InitCamera();
        }

        /// <summary>
        ///     Switchs settings bettween cameras
        /// </summary>
        private void InitCamera()
        {
            switch (GameConfig.CameraSetting)
            {
                case GameCamera.FreeRoam:
                case GameCamera.SurfaceFollow:
                    FreeRoamCameraSettings();
                    break;

                case GameCamera.Static:
                    StaticCameraSettings();
                    break;

                case GameCamera.Tank3rdPerson:
                    break;

                default:
                    throw new NotImplementedException(string.Format("Camera {0} not defined", GameConfig.CameraSetting));
            }
        }

        /// <summary>
        ///     Updates the camera
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime, Matrix cameraMatrix)
        {
            var kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.F3))
                GameConfig.CameraSetting = GameCamera.FreeRoam;
            if (kb.IsKeyDown(Keys.F2))
                GameConfig.CameraSetting = GameCamera.SurfaceFollow;
            if (kb.IsKeyDown(Keys.F4))
                GameConfig.CameraSetting = GameCamera.Static;
            if (kb.IsKeyDown(Keys.F1))
                GameConfig.CameraSetting = GameCamera.Tank3rdPerson;

            switch (GameConfig.CameraSetting)
            {
                case GameCamera.FreeRoam:
                case GameCamera.SurfaceFollow:
                    FreeRoamUpdate(gameTime);
                    break;

                case GameCamera.Static:
                    StaticCameraSettings();
                    break;

                case GameCamera.Tank3rdPerson:
                    ThirdPersonTank(cameraMatrix);
                    break;

                default:
                    throw new NotImplementedException(string.Format("Camera {0} not defined", GameConfig.CameraSetting));
            }
        }

        /// <summary>
        ///     Settings to apply to the Free Roam Camera
        /// </summary>
        private void FreeRoamCameraSettings()
        {
            cameraPosition = new Vector3(64f, 25f, 64f);
            
            cameraLookAt = new Vector3(0f, 0f, -1f) - cameraPosition;
            cameraLookAt.Normalize();
            ViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraPosition + cameraLookAt, Vector3.Up);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, graphicsDevice.Viewport.AspectRatio, 1f, 10000f);
        }

        /// <summary>
        ///     Settings to apply to the Static Camera
        /// </summary>
        private void StaticCameraSettings()
        {
            cameraPosition = new Vector3(95f, 45f, 95f);
            cameraLookAt = new Vector3(0f, 0f, -1f) - cameraPosition;
            cameraLookAt.Normalize();
            ViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraPosition + cameraLookAt, Vector3.Up);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, graphicsDevice.Viewport.AspectRatio, 1f, 10000f);
        }

        /// <summary>
        ///     The 3rd person camera
        /// </summary>
        /// <param name="tankMatrix"></param>
        private void ThirdPersonTank(Matrix tankMatrix)
        {
            Vector3 cameraPosition = tankMatrix.Translation + (tankMatrix.Backward * GameConfig.CameraTrailingDistance) +
                                                         (tankMatrix.Right * GameConfig.CameraHorizontalOffset) +
                                                         (tankMatrix.Up * GameConfig.CameraVerticalOffset);

            Vector3 cameraLookAt = tankMatrix.Translation + (tankMatrix.Forward * GameConfig.CameraLookAheadDistance);

            ViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraLookAt, Vector3.Up);
        }

        /// <summary>
        ///     Updates the Free Roam / surface follow Cameras
        /// </summary>
        private void FreeRoamUpdate(GameTime gameTime)
        {
            // Gets the keyboard state
            var kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.NumPad8))
                cameraPosition += cameraLookAt * GameConfig.CameraSpeed;
            else if (kb.IsKeyDown(Keys.NumPad5))
                cameraPosition -= cameraLookAt * GameConfig.CameraSpeed;

            if (kb.IsKeyDown(Keys.NumPad6))
                cameraPosition -= Vector3.Cross(Vector3.Up, cameraLookAt) * GameConfig.CameraSpeed; 
            else if (kb.IsKeyDown(Keys.NumPad4))
                cameraPosition += Vector3.Cross(Vector3.Up, cameraLookAt) * GameConfig.CameraSpeed;
            

            if (kb.IsKeyDown(Keys.Decimal))
                cameraAngleX += (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (kb.IsKeyDown(Keys.NumPad0))
                cameraAngleX -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            else
                cameraAngleX = 0;

            if (GameConfig.CameraSetting != GameCamera.SurfaceFollow)
            {
                if (kb.IsKeyDown(Keys.NumPad1))
                    cameraAngleY += (float)gameTime.ElapsedGameTime.TotalSeconds;
                else if (kb.IsKeyDown(Keys.NumPad7))
                    cameraAngleY -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                    cameraAngleY = 0;
            }

            if (GameConfig.CameraSetting == GameCamera.SurfaceFollow)
                cameraPosition.Y = SurfaceFollow() + 5f;

            // Rotaes the camera
            cameraLookAt = Vector3.Transform(cameraLookAt, Matrix.CreateFromAxisAngle(Vector3.Cross(Vector3.Up, cameraLookAt), (MathHelper.PiOver4 / 150) * (cameraAngleY))); 
            cameraLookAt = Vector3.Transform(cameraLookAt, Matrix.CreateFromAxisAngle(Vector3.Up, (-MathHelper.PiOver4 / 10f) * (cameraAngleX)));
            ViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraPosition + cameraLookAt, Vector3.Up);
        }

        /// <summary>
        ///     returns the height value to apply to the camera to build a surface follow
        /// </summary>
        private float SurfaceFollow()
        {
            return (Helper.SurfaceFollow(cameraPosition, terrainVertices));
        }
    }
}
