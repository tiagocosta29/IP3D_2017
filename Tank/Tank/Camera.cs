using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        ///     Camera Constructor
        /// </summary>
        /// <param name="device"></param>
        public Camera(GraphicsDevice device)
        {
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

        }

        /// <summary>
        ///     Settings to apply to the Free Roam Camera
        /// </summary>
        private void FreeRoamCameraSettings()
        {
            cameraPosition = new Vector3(60, 80, -80);
            cameraLookAt = new Vector3(0, 0, 0);
            ViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraLookAt, new Vector3(0, 1, 0));
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, graphicsDevice.Viewport.AspectRatio, 1.0f, 300.0f);
        }

        /// <summary>
        ///     Updates the Free Roam Camera
        /// </summary>
        private void FreeRoamUpdate()
        {
            cameraRotationMatrix = Matrix.CreateRotationZ(cameraAngle);
            cameraLookAt = Vector3.Transform(cameraLookAt, cameraRotationMatrix);
            cameraLookAt += cameraPosition;
            ViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraLookAt, new Vector3(0, 1, 0));
        }

    }
}
