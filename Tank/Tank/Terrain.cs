using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank
{
    class Terrain
    {
        /// <summary>
        ///     Texture to apply to the terrain
        /// </summary>
        private Texture2D texture;

        /// <summary>
        ///     The widht of the terrain texture
        /// </summary>
        private int terrainWidth;

        /// <summary>
        ///     The height of the terrain texture
        /// </summary>
        private int terrainHeight;

        /// <summary>
        ///     Stores the Terrain data
        /// </summary>
        private float[,] terrainData;

        /// <summary>
        ///     The Terrain constructor
        /// </summary>
        /// <param name="terrainTexture"></param>
        public Terrain(Texture2D terrainTexture)
        {
            texture = terrainTexture;

            //Checks if the terrain has the right size, if not reduces its size to get even values
            if (texture.Width % 2 == 0)
                terrainWidth = texture.Width;
            else
                terrainWidth = texture.Width - 1;

            if (texture.Height % 2 == 0)
                terrainHeight = texture.Height;
            else
                terrainHeight = texture.Height - 1;
        }

        /// <summary>
        /// Gets the terrain data and then stores it into an array
        /// </summary>
        private void GetTerrainData()
        {
            Color[] greyScale = new Color[terrainWidth * terrainHeight];
            texture.GetData(greyScale);

            terrainData = new float[terrainWidth, terrainHeight];
            for (int i = 0; i < terrainWidth; i++)
            {
                for (int j = 0; j < terrainHeight; j++)
                {
                    terrainData[i, j] = greyScale[i + j * terrainWidth].R / 5.0f;
                }
            }
        }


    }
}
