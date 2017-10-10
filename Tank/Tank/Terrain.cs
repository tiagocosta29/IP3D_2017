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
#region Variables
        /// <summary>
        ///     Texture of the heightMap
        /// </summary>
        private Texture2D heightMap;

        /// <summary>
        ///     textures to apply to the map
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
        ///     Stores the Terrain data from texture
        /// </summary>
        private float[,] terrainData;

        /// <summary>
        ///     Stores the Terrain Vertices
        /// </summary>
        private VertexPositionTexture[] vertices;

        /// <summary>
        ///     Index to store the vertices
        /// </summary>
        private short[] verticesIndex;

        /// <summary>
        ///     The vertex buffer of the terrain
        /// </summary>
        private VertexBuffer vertexBuffer;

        /// <summary>
        ///     The Index Buffer of the terrain
        /// </summary>
        private IndexBuffer indexBuffer;

        /// <summary>
        /// Effect to apply to the terrain
        /// </summary>
        private BasicEffect effect;

#endregion

        /// <summary>
        ///     The Terrain constructor
        /// </summary>
        /// <param name="terrainTexture"></param>
        public Terrain(Texture2D terrainHeightMap, Texture2D terrainTexture, GraphicsDevice device)
        {
            heightMap = terrainHeightMap;
            texture = terrainTexture;

            //Checks if the terrain has the right size, if not reduces it to get even values
            if (heightMap.Width % 2 == 0)
                terrainWidth = heightMap.Width;
            else
                terrainWidth = heightMap.Width - 1;

            if (heightMap.Height % 2 == 0)
                terrainHeight = heightMap.Height;
            else
                terrainHeight = heightMap.Height - 1;

            GetTerrainData();
            GetVertices(device);

            // Setups the effect to be applied to the terrain
            effect = new BasicEffect(device);
            effect.LightingEnabled = false;
            effect.TextureEnabled = true;
            effect.Texture = texture;
        }

        /// <summary>
        /// Gets the terrain data and then stores it into an array
        /// </summary>
        private void GetTerrainData()
        {
            Color[] greyScale = new Color[terrainWidth * terrainHeight];
            heightMap.GetData(greyScale);

            terrainData = new float[terrainWidth, terrainHeight];
            for (int i = 0; i < terrainWidth; i++)
            {
                for (int j = 0; j < terrainHeight; j++)
                {
                    terrainData[i, j] = greyScale[i + j * terrainWidth].R / 20.0f;
                }
            }
        }

        /// <summary>
        ///     Setups the vertices with the data from the texture
        /// </summary>
        private void GetVertices(GraphicsDevice device)
        {
            vertices = new VertexPositionTexture[terrainWidth * terrainHeight];
            verticesIndex = new short[(terrainWidth * terrainHeight) * 2];

            for (int i = 0; i < terrainWidth; i++)
            {
                for (int j = 0; j < terrainHeight; j++)
                {
                    vertices[i + j * terrainWidth].Position = new Vector3(i, terrainData[i, j], -j);
                    vertices[i + j * terrainWidth].TextureCoordinate.X = (float)i / 8.0f;
                    vertices[i + j * terrainWidth].TextureCoordinate.Y = (float)j / 8.0f;
                }
            }

            // creates an index of vertices
            for (int i = 0; i < verticesIndex.Length; i += 2)
            {
                verticesIndex[i] = (short)(i / 2);
                verticesIndex[i + 1] = (short)((i / 2) + terrainHeight);
            }

            vertexBuffer = new VertexBuffer(device, typeof(VertexPositionTexture), vertices.Length, BufferUsage.None);
            vertexBuffer.SetData<VertexPositionTexture>(vertices);

            indexBuffer = new IndexBuffer(device, typeof(short), verticesIndex.Length, BufferUsage.None);
            indexBuffer.SetData<short>(verticesIndex);

            device.Indices = indexBuffer;
            device.SetVertexBuffer(vertexBuffer);
        }
        
        /// <summary>
        /// Draws the map
        /// </summary>
        /// <param name="device"></param>
        public void Draw(GraphicsDevice device, Camera camera)
        {
            //effect.View = Matrix.CreateLookAt(new Vector3(60, 80, -80), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            //effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 1.0f, 300.0f);
            effect.View = camera.ViewMatrix;
            effect.Projection = camera.ProjectionMatrix;

            effect.World = Matrix.CreateTranslation(-terrainWidth / 2.0f, 0, terrainHeight / 2.0f);
            effect.CurrentTechnique.Passes[0].Apply();
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            device.RasterizerState = rs;

            for (int i = 0; i < ((terrainWidth * terrainHeight - 1) * 2); i += (terrainWidth * 2))
            {
                device.DrawIndexedPrimitives(PrimitiveType.TriangleStrip, 0, 0, (terrainWidth * 2), i, (terrainWidth * 2) - 2);
            }
        }

    }
}
