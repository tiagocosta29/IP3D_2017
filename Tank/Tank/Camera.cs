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
        private Vector3 cameraPosition = Vector3.Zero;

        /// <summary>
        /// The view Matrix of the camera
        /// </summary>
        private Matrix viewMatrix;

        /// <summary>
        /// The projection Matrix of the camera
        /// </summary>
        private Matrix projectionMatrix;

        public Camera(GraphicsDevice device)
        {
            graphicsDevice = device;
        }

        /// <summary>
        /// Switchs settings bettween cameras
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

        private void FreeRoamCameraSettings()
        {
            viewMatrix = Matrix.CreateLookAt(new Vector3(60, 80, -80), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, graphicsDevice.Viewport.AspectRatio, 1.0f, 300.0f);
        }

    }
}
