using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank
{
    public static class Helper
    {
        /// <summary>
        /// Converts color to vector3
        /// </summary>
        /// <param name="color"></param>
        /// <returns>color vector</returns>
        public static Vector3 ColorToVector(Color color)
        {
            if (color != null)
                return new Vector3(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f);
            else
                return new Vector3(1, 1, 1);
        }

        /// <summary>
        /// Converts rgb to vector3 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns>color vector3</returns>
        public static Vector3 ColorToVector(float r = 0f, float g = 0f, float b = 0f)
        {
            return new Vector3(r / 255.0f, g / 255.0f, b / 255.0f);
        }
    }
}
