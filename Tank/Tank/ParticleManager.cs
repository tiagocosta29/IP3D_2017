using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank
{
    class ParticleManager
    {
        /// <summary>
        ///  The dust particles array
        /// </summary>
        private Particle[] dustParticles;

        /// <summary>
        ///     Tank reference
        /// </summary>
        private Tank tankPlaceholder;

        /// <summary>
        ///     The map vertices reference
        /// </summary>
        private VertexPositionNormalTexture[] vertices;

        /// <summary>
        ///     The Vertices for the dust particles
        /// </summary>
        private VertexPositionColor[] dustParticlesVertices;

        /// <summary>
        ///     The basic effect for the particles
        /// </summary>
        private BasicEffect effect;

        /// <summary>
        ///     Graphics device reference
        /// </summary>
        private GraphicsDevice device;

        private Vector3 newDir;
        private Vector3 newPos;

        /// <summary>
        /// The ParticleManager Constructor
        /// </summary>
        public ParticleManager(Tank tank, VertexPositionNormalTexture[] terrainVertices, GraphicsDevice graphicsDevice)
        {
            device = graphicsDevice;
            effect = new BasicEffect(device);
            tankPlaceholder = tank;
            vertices = terrainVertices;
            dustParticles = new Particle[5000];
            DustParticlesInit();

            dustParticlesVertices = new VertexPositionColor[dustParticles.Length * 2];
        }

        /// <summary>
        ///     Creates the dust particles, this is a pool, so we create them in bulk and then reuse them.
        /// </summary>
        private void DustParticlesInit()
        {
            Vector3 vectorZero = Vector3.Zero;
            Random randomValue = new Random();

            var position = tankPlaceholder.WorldMatrix.Translation;
            for (int i = 0; i < dustParticles.Length; i++)
            {
                newDir = new Vector3(((float)randomValue.NextDouble() * 0.05f),
                                                                 ((float)randomValue.NextDouble() * 0.1f),
                                                                 ((float)randomValue.NextDouble() * 0.05f));

                dustParticles[i] = new Particle(position, newDir, vectorZero);

                dustParticles[i].ParticlePosition = new Vector3(position.X, Helper.SurfaceFollow(position, vertices), position.Z);
                dustParticles[i].ParticleDirection = new Vector3(((float)randomValue.NextDouble() * 0.05f),
                                                                 ((float)randomValue.NextDouble() * 0.1f),
                                                                 ((float)randomValue.NextDouble() * 0.05f));
            }
        }

        /// <summary>
        ///     Updated dust particles
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="position"></param>
        public void UpdateDust(GameTime gameTime)
        {
            Random randomValue = new Random();
            int j = 0;
            var position = tankPlaceholder.WorldMatrix.Translation;
            for (int i = 0; i < dustParticles.Length; i++)
            {
                newPos = new Vector3(position.X, position.Y, position.Z - 3f);
                newDir = new Vector3(((float)randomValue.NextDouble() * 0.05f),
                                                                 ((float)randomValue.NextDouble() * 0.1f),
                                                                 ((float)randomValue.NextDouble() * 0.05f));

                if (tankPlaceholder.IsMoving)
                {
                    dustParticles[i].UpdateParticle(gameTime, newPos, newDir);
                }
                else
                {
                    dustParticles[i].ResetParticle(newPos, newDir);
                }

                dustParticlesVertices[j] = new VertexPositionColor(dustParticles[i].ParticlePosition, Color.BlueViolet);
                dustParticlesVertices[j + 1] = new VertexPositionColor(dustParticles[i].ParticlePosition + new Vector3(0.01f, 0.0f, 0.01f), Color.Blue);
                j += 2;
            }

        }

        /// <summary>
        ///     Draws the dust particles
        /// </summary>
        public void DrawDust(Camera camera)
        {
            if (dustParticlesVertices != null)
            {
                effect.World = Matrix.Identity;
                effect.View = camera.ViewMatrix;
                effect.Projection = camera.ProjectionMatrix;

                effect.CurrentTechnique.Passes[0].Apply();

                device.DrawUserPrimitives(PrimitiveType.LineList, dustParticlesVertices, 0, dustParticles.Length);
            }
        }
    }
}
