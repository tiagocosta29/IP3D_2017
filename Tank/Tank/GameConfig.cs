using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank
{
    /// <summary>
    /// Class that holds configurations during the game
    /// </summary>
    static class GameConfig
    {
        /// <summary>
        /// Variable that holds the current camera in use
        /// </summary>
        public static GameCamera CameraSetting = GameCamera.SurfaceFollow;

        /// <summary>
        /// Speed to move the camera
        /// </summary>
        public static float CameraSpeed = 0.50f;

        /// <summary>
        ///     Distance of the camera to the tank
        /// </summary>
        public static float CameraTrailingDistance = -10f;

        /// <summary>
        ///     Offset of the camera to the tank
        /// </summary>
        public static float CameraLookAheadDistance = 0f;

        /// <summary>
        ///     Camera offset on the horizontal axis
        /// </summary>
        public static float CameraHorizontalOffset = 0f;

        /// <summary>
        ///     Camera offset on the vertical axis
        /// </summary>
        public static float CameraVerticalOffset = -5f;

        /// <summary>
        /// Top speed of the tank
        /// </summary>
        public static float TankTopSpeed = 0.50f;

        /// <summary>
        ///     Ammount of acceleration for the tank
        /// </summary>
        public static float TankAcceleration = 0.05f;

        /// <summary>
        ///     Ammout of deceleration
        /// </summary>
        public static float TankDeceleration = 0.10f;
            
        /// <summary>
        /// Tank scaling factor
        /// </summary>
        public static float TankScalingFactor = 0.003f;

    }
}
