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
        private float cameraAngle;

        /// <summary>
        ///     Look at vector to aplly to the camera
        /// </summary>
        private Vector3 cameraLookAt;

        /// <summary>
        ///     Rotation Matrix to apply to the camera
        /// </summary>
        private Matrix cameraRotationMatrix;

        /// <summary>
        ///     Vertices to calculate surface follow
        /// </summary>
        private VertexPositionTexture[] terrainVertices;

        /// <summary>
        ///     Camera Constructor
        /// </summary>
        /// <param name="device"></param>
        public Camera(GraphicsDevice device, VertexPositionTexture[] vertices)
        {
            terrainVertices = vertices;
            graphicsDevice = device;
            InitCamera();
        }

        /// <summary>
        ///     Switchs settings bettween cameras
        /// </summary>
        private void InitCamera()
        {
            switch(GameConfig.CameraSetting)
            {
                case GameCamera.FreeRoam:
                    FreeRoamCameraSettings();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        ///     Updates the camera
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            switch (GameConfig.CameraSetting)
            {
                case GameCamera.FreeRoam:
                    FreeRoamUpdate(gameTime);
                    break;

                case GameCamera.SurfaceFollow:
                    SurfaceFollowCameraUpdate();
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
            //cameraPosition = new Vector3(60, 80, -80);
            cameraPosition = new Vector3(1,1,1);
            cameraLookAt = new Vector3(0f, 0f, 0f);
            ViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraLookAt, new Vector3(0, 1, 0));
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, graphicsDevice.Viewport.AspectRatio, 1.0f, 300.0f);
        }

        /// <summary>
        ///     Updates the Free Roam Camera
        /// </summary>
        private void FreeRoamUpdate(GameTime gameTime)
        {
            // Gets the keyboard state
            var kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Up))
            {
                var forwardVector = new Vector3(0f, 0f, 1f);
                var rotationMatrix = Matrix.CreateRotationZ(cameraAngle);
                forwardVector = Vector3.Transform(forwardVector, rotationMatrix);
                cameraPosition += forwardVector * GameConfig.CameraSpeed *
                (float)gameTime.ElapsedGameTime.TotalSeconds;
                CameraUpdate();
            }
            else if (kb.IsKeyDown(Keys.Down))
            {
                var forwardVector = new Vector3(0f, 0f, -1f);
                var rotationMatrix = Matrix.CreateRotationZ(cameraAngle);
                forwardVector = Vector3.Transform(forwardVector, rotationMatrix);
                cameraPosition += forwardVector * GameConfig.CameraSpeed *
                (float)gameTime.ElapsedGameTime.TotalSeconds;
                CameraUpdate();
            }
            else if (kb.IsKeyDown(Keys.Right))
            {
                cameraAngle += (float)gameTime.ElapsedGameTime.TotalSeconds;
                CameraUpdate();
            }
            else if (kb.IsKeyDown(Keys.Left))
            {
                cameraAngle -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                CameraUpdate();
            }
        }

        /// <summary>
        ///     The surface follow camera
        /// </summary>
        private float SurfaceFollowCameraUpdate()
        {
            float distance = 0f;
            float newDistance = float.MaxValue;
            float newCameraPositionY = 0f;

            for (int i = 0; i < terrainVertices.Length; i+=4)
            {
                var tempVector = new Vector3(terrainVertices[i].Position.X, cameraPosition.Y, terrainVertices[i].Position.Z);
                Vector3.Distance(ref cameraPosition, ref tempVector, out distance);
                if (distance < newDistance)
                {
                    newDistance = distance;
                    newCameraPositionY = terrainVertices[i].Position.Y;
                }
            }

            return newCameraPositionY + 5f;
        }
        
        /// <summary>
        /// Updates the camera 
        /// </summary>
        private void CameraUpdate()
        {
            cameraRotationMatrix = Matrix.CreateRotationY(cameraAngle);
            cameraLookAt = Vector3.Transform(cameraLookAt, cameraRotationMatrix);
            cameraLookAt += cameraPosition;
            ViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraLookAt, new Vector3(0, 1, 0));
        }

    }
}
