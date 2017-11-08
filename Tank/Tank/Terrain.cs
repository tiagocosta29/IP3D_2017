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
        public int TerrainWidth;

        /// <summary>
        ///     The height of the terrain texture
        /// </summary>
        public int TerrainHeight;

        /// <summary>
        ///     Stores the Terrain data from texture
        /// </summary>
        private float[,] terrainData;

        /// <summary>
        ///     Stores the Terrain Vertices
        /// </summary>
        public VertexPositionNormalTexture[] Vertices;

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
                TerrainWidth = heightMap.Width;
            else
                TerrainWidth = heightMap.Width - 1;

            if (heightMap.Height % 2 == 0)
                TerrainHeight = heightMap.Height;
            else
                TerrainHeight = heightMap.Height - 1;

            GetTerrainData();
            GetVertices(device);
            TerrainEffect(device);
        }

        /// <summary>
        /// Setups the effect to be applied to the terrain
        /// </summary>
        /// <param name="device"></param>
        private void TerrainEffect(GraphicsDevice device)
        {
            effect = new BasicEffect(device);
            effect.LightingEnabled = true;
            effect.DirectionalLight0.Enabled = true;
            effect.DirectionalLight0.Direction = new Vector3(1.0f, -1.0f, 1.0f);
            effect.DirectionalLight0.SpecularColor = Helper.ColorToVector(Color.BlueViolet);
            effect.DirectionalLight0.DiffuseColor = Helper.ColorToVector(Color.BlueViolet);
            effect.AmbientLightColor = Helper.ColorToVector(Color.White);
            effect.SpecularPower = 1000f;
            effect.SpecularColor = Helper.ColorToVector(Color.White);
            effect.DirectionalLight1.Enabled = true;
            effect.DirectionalLight2.Enabled = true;

            effect.FogEnabled = false;
            effect.FogColor = Helper.ColorToVector(Color.Black);
            effect.FogStart = 15f;
            effect.FogEnd = 60f;
            effect.TextureEnabled = true;
            effect.Texture = texture;
        }

        /// <summary>
        /// Gets the terrain data and then stores it into an array
        /// </summary>
        private void GetTerrainData()
        {
            Color[] greyScale = new Color[TerrainWidth * TerrainHeight];
            heightMap.GetData(greyScale);

            terrainData = new float[TerrainWidth, TerrainHeight];
            for (int i = 0; i < TerrainWidth; i++)
            {
                for (int j = 0; j < TerrainHeight; j++)
                {
                    terrainData[i, j] = greyScale[i + j * TerrainWidth].R / 15.0f;
                }
            }
        }

        /// <summary>
        ///     Setups the vertices with the data from the texture
        /// </summary>
        private void GetVertices(GraphicsDevice device)
        {
            Vertices = new VertexPositionNormalTexture[TerrainWidth * TerrainHeight];
            verticesIndex = new short[(TerrainWidth * TerrainHeight) * 2];

            for (int i = 0; i < TerrainWidth; i++)
            {
                for (int j = 0; j < TerrainHeight; j++)
                {
                    Vertices[i + j * TerrainWidth].Position = new Vector3(i, terrainData[i, j], j);
                    Vertices[i + j * TerrainWidth].Normal = new Vector3(0f, 0f, 0f);
                    Vertices[i + j * TerrainWidth].TextureCoordinate.X = (float)i / 4.0f;
                    Vertices[i + j * TerrainWidth].TextureCoordinate.Y = (float)j / 4.0f;
                }
            }

            // creates an index of vertices
            for (int i = 0; i < verticesIndex.Length; i += 2)
            {
                verticesIndex[i] = (short)(i / 2);
                verticesIndex[i + 1] = (short)((i / 2) + TerrainHeight);
            }

            GetNormals();

            vertexBuffer = new VertexBuffer(device, typeof(VertexPositionNormalTexture), Vertices.Length, BufferUsage.None);
            vertexBuffer.SetData<VertexPositionNormalTexture>(Vertices);

            indexBuffer = new IndexBuffer(device, typeof(short), verticesIndex.Length, BufferUsage.None);
            indexBuffer.SetData<short>(verticesIndex);

            device.Indices = indexBuffer;
            device.SetVertexBuffer(vertexBuffer);
        }

        /// <summary>
        /// Sets up the normals for the terrain vertices
        /// </summary>
        private void GetNormals()
        {
            int index1 = 0;
            int index2 = 0;
            int index3 = 0;

            Vector3 side1 = Vector3.Zero;
            Vector3 side2 = Vector3.Zero;
            Vector3 normal = Vector3.Zero;

            for (int i = 0; i < Vertices.Length; i++)
            {
                if (verticesIndex[i * 3] < Vertices.Length &&
                    verticesIndex[i * 3 + 1] < Vertices.Length &&
                    verticesIndex[i * 3 + 2] < Vertices.Length)
                {
                    index1 = verticesIndex[i * 3];
                    index2 = verticesIndex[i * 3 + 1];
                    index3 = verticesIndex[i * 3 + 2];
                }
                else
                    break;

                side1 = Vertices[index2].Position - Vertices[index1].Position;
                side2 = Vertices[index1].Position - Vertices[index3].Position;

                normal = Vector3.Cross(side1, side2);
                normal.Normalize();

                Vertices[index1].Normal = normal;                
                Vertices[index2].Normal = -normal;
                Vertices[index3].Normal = normal;
            }
        }
        
        /// <summary>
        /// Draws the map
        /// </summary>
        /// <param name="device"></param>
        public void Draw(GraphicsDevice device, Camera camera)
        {
            effect.View = camera.ViewMatrix;
            effect.Projection = camera.ProjectionMatrix;

            effect.World = Matrix.Identity;
            effect.CurrentTechnique.Passes[0].Apply();
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            device.RasterizerState = rs;
            
            for (int i = 0; i < (TerrainWidth * (TerrainHeight - 1) * 2); i += (TerrainWidth * 2))
            {
                device.DrawIndexedPrimitives(PrimitiveType.TriangleStrip, 0, 0, (TerrainWidth * 2), i, (TerrainWidth * 2) - 2);
            }
        }

    }
}
