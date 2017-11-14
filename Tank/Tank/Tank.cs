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
        ///     Initial turret position
        /// </summary>
        private Matrix turretTransform;

        /// <summary>
        ///     Tank position
        /// </summary>
        private Vector3 tankPosition;

        /// <summary>
        ///     Tank Direction
        /// </summary>
        private Vector3 tankDirection;

        /// <summary>
        ///     Tank Rotation
        /// </summary>
        private Matrix tankRotation;

        /// <summary>
        ///     Tank initial Direction
        /// </summary>
        private Vector3 tankInitialDirection;

        /// <summary>
        ///     Vertices to calculate surface follow
        /// </summary>
        private VertexPositionNormalTexture[] terrainVertices;

        /// <summary>
        /// Matrix to aplly to the tank
        /// </summary>
        public Matrix WorldMatrix { get; set; }

        /// <summary>
        ///     The ammout of steering to apply to the tank
        /// </summary>
        private float steerRotation;

        /// <summary>
        ///     PlaceHolder for the tank transforms
        /// </summary>
        private Matrix[] tankTransforms;

        /// <summary>
        ///     Rotation to apply to the tank turret on the X value
        /// </summary>
        private float turretRotationXAmount = 0f;

        /// <summary>
        ///     Rotation to apply to the tank turret on the Y value
        /// </summary>
        private float turretRotationYAmount = 0f;

        /// <summary>
        /// Tank Constructor
        /// </summary>
        /// <param name="content"></param>
        /// <param name="vertices"></param>
        public Tank(ContentManager content, VertexPositionNormalTexture[] vertices)
        {
            terrainVertices = vertices;
            WorldMatrix = Matrix.Identity;
            LoadTankModel(content);
        }

        public void LoadTankModel(ContentManager content)
        {
            tankModel = content.Load<Model>("RetroTank");
            turretBone = tankModel.Bones["Cannon"];
            //turretBone = tankModel.Bones["TurretBone"];            
            turretTransform = turretBone.Transform;
            tankTransforms = new Matrix[tankModel.Bones.Count];

            tankDirection = Vector3.Zero;
            tankInitialDirection = tankModel.Root.Transform.Backward;
            tankPosition = new Vector3(50f, 50f, 50f);
        }

        public void DrawTank(Camera camera)
        {
            tankPosition.Y = SurfaceFollow() + 3f;
            tankModel.Root.Transform = Matrix.CreateScale(GameConfig.TankScalingFactor) * WorldMatrix;



            Matrix turretRotationX = Matrix.CreateRotationX(turretRotationXAmount);
            //Matrix turretRotationY = Matrix.CreateRotationY(150f);
            turretBone.Transform = turretRotationX * turretTransform;
            //turretBone.Transform = turretRotationY * turretTransform;


            //turretBone.Transform.Rotation = Matrix.CreateRotationY(3f);
            tankModel.CopyAbsoluteBoneTransformsTo(tankTransforms);

            foreach (ModelMesh mesh in tankModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = tankTransforms[mesh.ParentBone.Index];
                    //effect.World = Matrix.Identity;
                    effect.TextureEnabled = false;
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

        /// <summary>
        /// Moves the tank forward
        /// </summary>
        public void MoveForward(float currentSpeed)
        {
            tankPosition = tankPosition + tankDirection * currentSpeed;
        }

        /// <summary>
        /// Moves the tank Backwards
        /// </summary>
        public void MoveBackwards(float currentSpeed)
        {
            tankPosition = tankPosition - tankDirection * currentSpeed;
        }

        /// <summary>
        /// Rotates the tank to the right
        /// </summary>
        public void RotateTankRight()
        {
            steerRotation += 0.02f;
        }

        /// <summary>
        /// Rotates the tank to the left
        /// </summary>
        public void RotateTankLeft()
        {
            steerRotation -= 0.02f;
        }

        /// <summary>
        /// Rotates the turret to the Right
        /// </summary>
        public void RotateTurretRight()
        {
            turretRotationXAmount -= 0.5f;
        }

        /// <summary>
        /// Rotates the turret to the Left
        /// </summary>
        public void RotateTurretLeft()
        {
            turretRotationXAmount += 0.1f;
        }

        public void ApplyRotation()
        {
            tankRotation = Matrix.CreateRotationY(steerRotation);
            tankDirection = Vector3.Transform(tankInitialDirection, tankRotation);

            tankRotation.Up = NormaisTankFollow();

            tankRotation.Right = Vector3.Cross(tankRotation.Forward, NormaisTankFollow());
            tankRotation.Right = Vector3.Normalize(tankRotation.Right);
            tankRotation.Forward = Vector3.Cross(NormaisTankFollow(), tankRotation.Right);
            tankRotation.Forward = Vector3.Normalize(tankRotation.Forward);

            WorldMatrix = tankRotation * Matrix.CreateTranslation(tankPosition);
        }

        /// <summary>
        ///     Adjusts the tank to the terrain
        /// </summary>
        public Vector3 NormaisTankFollow()
        {
           float x = (int)WorldMatrix.Translation.X;
           float z = (int)WorldMatrix.Translation.Z;

            Vector3 topLeft = Vector3.Zero;
            Vector3 topRight = Vector3.Zero;
            Vector3 downRight = Vector3.Zero;
            Vector3 downLeft = Vector3.Zero;
            Vector3 result = Vector3.Zero;

            //Searchs for the vertices on the previous stores coodinates, and then stores it's positon into vectors
            foreach (VertexPositionNormalTexture item in terrainVertices)
            {
                if (item.Position.X == x && item.Position.Z == z)
                    topLeft = item.Normal;
                else if (item.Position.X == x && item.Position.Z == z + 1)
                    downLeft = item.Normal;
                else if (item.Position.X == x + 1 && item.Position.Z == z)
                    topRight = item.Normal;
                else if (item.Position.X == x + 1 && item.Position.Z == z + 1)
                    downRight = item.Normal;
            }

            result = (topLeft + downLeft + topRight + downRight) / 4;
            result.Normalize();

            return result;
        }
    }
}
