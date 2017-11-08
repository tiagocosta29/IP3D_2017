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
    class Tank
    {
        /// <summary>
        ///     Placeholder for the tank model
        /// </summary>
        private Model tankModel;

        /// <summary>
        /// Turret bone placeholder
        /// </summary>
        private ModelBone turretBone;

        /// <summary>
        /// Initial turret position
        /// </summary>
        private Matrix turretTransform;

        /// <summary>
        /// Tank position
        /// </summary>
        private Vector3 tankPosition;

        /// <summary>
        /// Tank Rotation
        /// </summary>
        private Vector3 tankDirection;

        /// <summary>
        /// Tank Rotation
        /// </summary>
        private Vector3 tankInitialDirection;
        /// <summary>
        ///     Vertices to calculate surface follow
        /// </summary>
        private VertexPositionNormalTexture[] terrainVertices;

        /// <summary>
        /// Matrix to aplly to the tank
        /// </summary>
        private Matrix worldMatrix;

        /// <summary>
        /// Rotation of the turret
        /// </summary>
        private float turretRotationValue = 0f;

        /// <summary>
        /// Tank Constructor
        /// </summary>
        /// <param name="content"></param>
        /// <param name="vertices"></param>
        public Tank(ContentManager content, VertexPositionNormalTexture[] vertices)
        {
            terrainVertices = vertices;
            worldMatrix = Matrix.Identity;
            LoadTankModel(content);
        }

        public void LoadTankModel(ContentManager content)
        {
            tankModel = content.Load<Model>("RetroTank");
            turretBone = tankModel.Bones["TurretBone"];
            turretTransform = turretBone.Transform;

            tankDirection = Vector3.Zero;
            tankInitialDirection = tankModel.Root.Transform.Backward;
        }

        public void DrawTank(Camera camera)
        {
            tankPosition.Y = SurfaceFollow() + 5f;
            tankModel.Root.Transform = Matrix.CreateScale(GameConfig.TankScalingFactor) * worldMatrix;

            //Matrix turretRotation = Matrix.CreateRotationY(turretRotationValue);
            //turretBone.Transform = turretRotation * turretTransform;

            foreach (ModelMesh mesh in tankModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effect.World = turretBone.Transform;
                    effect.World = Matrix.Identity;
                    effect.TextureEnabled = true;
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;

                    effect.EnableDefaultLighting();
                }
                mesh.Draw();
            }
        }

        /// <summary>
        /// Returns the height to aplly to the tank
        /// </summary>
        /// <returns></returns>
        private float SurfaceFollow()
        {
            int x = (int)tankModel.Root.Transform.Translation.X;
            int z = (int)tankModel.Root.Transform.Translation.Z;

            return (Helper.SurfaceFollow(x, z, tankModel.Root.Transform.Translation, terrainVertices));
        }
    }
}
