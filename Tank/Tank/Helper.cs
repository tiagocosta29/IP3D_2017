using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        /// <summary>
        /// Function that handles the surface follow of any object
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <param name="currentPosition"></param>
        /// <param name="vertices"></param>
        /// <returns></returns>
        public static float SurfaceFollow(Vector3 currentPosition, VertexPositionNormalTexture[] vertices)
        {
            float y1;
            float y2;
            int x = (int)currentPosition.X;
            int z = (int)currentPosition.Z;

            Vector3 topLeft = Vector3.Zero;
            Vector3 topRight = Vector3.Zero;
            Vector3 downRight = Vector3.Zero;
            Vector3 downLeft = Vector3.Zero;

            //Searchs for the vertices on the previous stores coodinates, and then stores it's positon into vectors
            foreach (VertexPositionNormalTexture item in vertices)
            {
                if (item.Position.X == x && item.Position.Z == z)
                    topLeft = item.Position;
                else if (item.Position.X == x && item.Position.Z == z + 1)
                    downLeft = item.Position;
                else if (item.Position.X == x + 1 && item.Position.Z == z)
                    topRight = item.Position;
                else if (item.Position.X == x + 1 && item.Position.Z == z + 1)
                    downRight = item.Position;
            }

            // This is using bilinear interpolation to calculate the value of the height 
            // https://en.wikipedia.org/wiki/Bilinear_interpolation
            y1 = (((currentPosition.X - topLeft.X) * (topRight.Y - topLeft.Y)) /
                 (topRight.X - topLeft.X) + topLeft.Y);

            y2 = (((currentPosition.X - downLeft.X) * (downRight.Y - downLeft.Y)) /
                  (downRight.X - downLeft.X) + downLeft.Y);

            return (((currentPosition.Z - topRight.Z) * (y2 - y1)) /
                    (downRight.Z - topRight.Z) + y1);
        }
    }
}
