using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank
{
    class Bullet
    {
        /// <summary>
        ///     Position of the bullet
        /// </summary>
        private Vector3 position;

        /// <summary>
        ///     Starting Position of the bullet
        /// </summary>
        private Vector3 startPosition;

        /// <summary>
        ///     Direction of the bullet
        /// </summary>
        private Vector3 direction;

        /// <summary>
        ///     Bullet old position
        /// </summary>
        private Vector3 oldPos;

        /// <summary>
        ///     
        /// </summary>
        private BoundingSphere boundingSphere;

        /// <summary>
        ///     The model of the bullet
        /// </summary>
        private Model bullet;

        /// <summary>
        ///     
        /// </summary>
        private float time;

        /// <summary>
        ///     Bullet Scaling factor
        /// </summary>
        private float scale;

        /// <summary>
        ///     Pitch
        /// </summary>
        private float pitch;

        /// <summary>
        ///     Velocity of the bullet
        /// </summary>
        private float velocity;

        /// <summary>
        /// Rotation matrix
        /// </summary>
        private Matrix rotation;

        /// <summary>
        ///     World Matrix
        /// </summary>
        private Matrix world;

        /// <summary>
        ///     Checks if the bullet still exists
        /// </summary>
        public bool IsALive = false;

        /// <summary>
        ///     Tank Class reference
        /// </summary>
        private Tank tank;

        /// <summary>
        ///     References to the terrain vertices
        /// </summary>
        private VertexPositionNormalTexture[] terrainVertices;

        public Bullet(GraphicsDevice device, ContentManager content, Vector3 startPos, float cannonPitch, Vector3 dir, VertexPositionNormalTexture[] terrainVert, Tank tankReference)
        {
            bullet = content.Load<Model>("bullet");
            position = startPos;
            position.Y = position.Y + 2f;
            startPosition = startPos;

            terrainVertices = terrainVert;
            tank = tankReference;

            velocity = 0.3f;
            time = 0;
            scale = 0.3f;
            pitch = cannonPitch;
            IsALive = true;
            direction = new Vector3(dir.X, dir.Y, dir.Z);
            direction.Normalize();
        }

        public void Update(GameTime gametime)
        {
            oldPos = position;
            if (position.X <= 0 || position.Z <= 0 || position.X >= Helper.MapHeight || position.Z >= Helper.MapWidth)
            {
                IsALive = false;
            }
            else if (Helper.SurfaceFollow(position, terrainVertices) >= position.Y)
            {
                IsALive = false;
            }

            if (IsALive)
            {
                position.X += -direction.X * velocity;
                position.Y += -direction.Y * velocity - time;
                position.Z += -direction.Z * velocity;

                time += 0.01f;                
            }
        }
        
        public void Draw(GraphicsDevice device, Camera camera)
        {
            if (IsALive)
            {
                Vector3 normal = Helper.NormalFollow(position.X, position.Z, terrainVertices);
                Vector3 right = Vector3.Cross(direction, normal);
                Vector3 d = Vector3.Cross(normal, right);

                rotation = Matrix.Identity;
                rotation.Forward = d;
                rotation.Up = normal;
                rotation.Right = right;

                foreach (ModelMesh mesh in bullet.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        world = Matrix.CreateScale(scale) * rotation *
                            Matrix.CreateRotationY(-MathHelper.PiOver2) *
                            Matrix.CreateFromYawPitchRoll(-direction.X * velocity, -direction.Y * velocity - time, -direction.Z * velocity) * Matrix.CreateTranslation(position);
                        effect.World = world;
                        effect.View = camera.ViewMatrix;
                        effect.Projection = camera.ProjectionMatrix;
                    }
                    mesh.Draw();
                }
            }
        }
    }
}
