using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank
{
    class Particle
    {
        /// <summary>
        ///     Particle position
        /// </summary>
        public Vector3 ParticlePosition { get; set; }

        /// <summary>
        ///     Particle Speed
        /// </summary>
        public Vector3 ParticleSpeed { get; set; }

        /// <summary>
        ///     Stores the initial particle position
        /// </summary>
        private Vector3 initialParticlePosition;

        /// <summary>
        ///     Stores the initial particle speed
        /// </summary>
        private Vector3 initialParticleSpeed;

        /// <summary>
        ///     Direction to aplly to the particle
        /// </summary>
        public Vector3 ParticleDirection;

        /// <summary>
        ///     Particle life time counter
        /// </summary>
        public float LifeTime { get; set; }

        private float maxLifeTime;
        
        /// <summary>
        ///      Particle initializer
        /// </summary>
        /// <param name="position"></param>
        /// <param name="speed"></param>
        public Particle(Vector3 position, Vector3 direction, Vector3 speed)
        {
            initialParticlePosition = position;
            initialParticleSpeed = speed;

            ParticlePosition = position;
            ParticleDirection = direction;
            ParticleSpeed = speed;

            maxLifeTime = (float)new Random().NextDouble();
            LifeTime = 0f; 
        }

        /// <summary>
        ///     Resets the particle state
        /// </summary>
        public void ResetParticle(Vector3 newPos, Vector3 newDir)
        {
            ParticlePosition = newPos;
            ParticleDirection = newDir;
            //ParticleSpeed = initialParticleSpeed;
            LifeTime = 0f;
        }

        public void UpdateParticle(GameTime gametime, Vector3 newPos, Vector3 newDir)
        {
            LifeTime += (float)gametime.ElapsedGameTime.TotalSeconds;
            ParticlePosition += ParticleDirection * new Vector3(LifeTime);

            if (LifeTime > maxLifeTime)
            {
                ResetParticle(newPos, newDir);
            }
        }
    }
}
