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
        public static float CameraSpeed = 0.5f;

        /// <summary>
        /// Top speed of the tank
        /// </summary>
        public static float TankTopSpeed = 0.5f;

        /// <summary>
        /// Tank scaling factor
        /// </summary>
        public static float TankScalingFactor = 0.005f;
    }
}
