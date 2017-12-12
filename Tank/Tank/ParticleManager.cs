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

        /// <summary>
        /// The ParticleManager Constructor
        /// </summary>
        public ParticleManager(Tank tank, VertexPositionNormalTexture[] terrainVertices, GraphicsDevice graphicsDevice)
        {
            device = graphicsDevice;
            effect = new BasicEffect(device);
            tankPlaceholder = tank;
            vertices = terrainVertices;
            dustParticles = new Particle[1000];
        }

        /// <summary>
        ///     Creates the dust particles, this is a pool, so we create them in bulk and then reuse them.
        /// </summary>
        private void DustParticlesInit()
        {
            Vector3 vectorZero = Vector3.Zero;

            for (int i = 0; i < dustParticles.Length; i++)
            {
                dustParticles[i] = new Particle(vectorZero, vectorZero);
            }
        }

        /// <summary>
        ///     Updated dust particles
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="position"></param>
        public void UpdateDust(GameTime gameTime, Vector3 position)
        {
            Random randomValue = new Random();

            for (int i = 0; i < dustParticles.Length; i++)
            {
                // This uses the surface follow to spawn the dust particles at the tank bottom
                if (dustParticles[i].LifeTime == 0f)
                {
                    dustParticles[i].ParticlePosition = new Vector3(position.X, Helper.SurfaceFollow(position, vertices), position.Z);
                    dustParticles[i].ParticleDirection = new Vector3(((float)randomValue.NextDouble() * 0.05f), 
                                                                     ((float)randomValue.NextDouble() * 0.1f),
                                                                     ((float)randomValue.NextDouble() * 0.05f));
                } else
                {
                    dustParticles[i].UpdateParticle(gameTime);
                }
            }

            dustParticlesVertices = new VertexPositionColor[dustParticles.Length * 2];

            for (int i = 0; i < dustParticles.Length; i++)
            {
                dustParticlesVertices[i * 2] = new VertexPositionColor(dustParticles[i].ParticlePosition, Color.GreenYellow);
                dustParticlesVertices[i * 2 + 1] = new VertexPositionColor(dustParticles[i].ParticlePosition + new Vector3(0.01f, 0.0f, 0.01f), Color.Green);
            }
        }

        /// <summary>
        ///     Draws the dust particles
        /// </summary>
        public void DrawDust(Camera camera)
        {
            effect.World = Matrix.Identity;
            effect.View = camera.ViewMatrix;
            effect.Projection = camera.ProjectionMatrix;

            effect.CurrentTechnique.Passes[0].Apply();
            
            device.DrawUserPrimitives(PrimitiveType.LineList, dustParticlesVertices, 0, dustParticles.Length);
        }
    }
}
