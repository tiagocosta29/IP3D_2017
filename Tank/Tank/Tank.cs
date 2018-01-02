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
#region Variables
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
        public Vector3 TankPosition;

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
        ///     The tank effect
        /// </summary>
        private BasicEffect effect;

        /// <summary>
        ///     The tank texture
        /// </summary>
        private Texture2D texture;

        /// <summary>
        ///     The Particle Manager of the tank
        /// </summary>
        private ParticleManager particleManager;

        /// <summary>
        ///     Graphics device placeholder
        /// </summary>
        private GraphicsDevice gDevice;

        /// <summary>
        ///     The bullet class
        /// </summary>
        private Bullet bullet;

        /// <summary>
        ///     Content Manager Reference
        /// </summary>
        private ContentManager contentManager;

        /// <summary>
        ///     Stores the initial position of the tank
        /// </summary>
        private Vector3 tankInitialPosition;

        /// <summary>
        ///     flag to check if tank is moving
        /// </summary>
        public bool IsMoving;

        private Vector3 tankFront;
        private Vector3 tankRight;
        private Vector3 tankNormal;

        #endregion

        /// <summary>
        /// Tank Constructor
        /// </summary>
        /// <param name="content"></param>
        /// <param name="vertices"></param>
        public Tank(GraphicsDevice device, ContentManager content, VertexPositionNormalTexture[] vertices, string tankModelName, Vector3 initialPosition = default(Vector3))
        {
            contentManager = content;
            gDevice = device;
            effect = new BasicEffect(device);
            texture = content.Load<Texture2D>("BottomTank");
            effect.TextureEnabled = true;
            effect.Texture = texture;

            terrainVertices = vertices;
            WorldMatrix = Matrix.Identity;
            TankPosition = initialPosition;
            tankInitialPosition = initialPosition;
            LoadTankModel(content, tankModelName);

            particleManager = new ParticleManager(this, terrainVertices, device);
        }

        /// <summary>
        ///     Loads tank
        /// </summary>
        /// <param name="content"></param>
        /// <param name="tankModelName"></param>
        public void LoadTankModel(ContentManager content, string tankModelName)
        {
            tankModel = content.Load<Model>(tankModelName);
            turretBone = tankModel.Bones["Cannon"];         
            turretTransform = turretBone.Transform;
            tankTransforms = new Matrix[tankModel.Bones.Count];

            tankDirection = Vector3.Zero;
            tankInitialDirection = tankModel.Root.Transform.Backward;
        }

        /// <summary>
        ///     Draws the tank
        /// </summary>
        /// <param name="camera"></param>
        public void DrawTank(Camera camera)
        {
            TankPosition.Y = SurfaceFollow() + 3f;
            tankModel.Root.Transform = Matrix.CreateScale(GameConfig.TankScalingFactor) * WorldMatrix;

            Matrix turretRotationX = Matrix.CreateRotationZ(turretRotationXAmount);
            turretBone.Transform = turretRotationX * turretTransform;
            tankModel.CopyAbsoluteBoneTransformsTo(tankTransforms);

            foreach (ModelMesh mesh in tankModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = tankTransforms[mesh.ParentBone.Index];
                    effect.TextureEnabled = false;
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;
                    effect.Texture = texture;
                    effect.EnableDefaultLighting();
                }
                mesh.Draw();
            }

            if(bullet != null)
            {
                bullet.Draw(gDevice, camera);
            }

            //if (IsMoving)
            //{
                particleManager.DrawDust(camera);
            //}
        }

        /// <summary>
        /// Returns the height to aplly to the tank
        /// </summary>
        /// <returns></returns>
        private float SurfaceFollow()
        {
            return (Helper.SurfaceFollow(tankModel.Root.Transform.Translation, terrainVertices));
        }

        /// <summary>
        /// Moves the tank forward
        /// </summary>
        public void MoveForward(float currentSpeed, GameTime gameTime)
        {
            TankPosition = TankPosition + tankDirection * currentSpeed;

            //particleManager.UpdateDust(gameTime);
        }

        /// <summary>
        /// Moves the tank Backwards
        /// </summary>
        public void MoveBackwards(float currentSpeed)
        {
            TankPosition = TankPosition - tankDirection * currentSpeed;

        }

        /// <summary>
        /// Rotates the tank to the right
        /// </summary>
        public void RotateTankRight()
        {
            steerRotation -= 0.02f;
        }

        /// <summary>
        /// Rotates the tank to the left
        /// </summary>
        public void RotateTankLeft()
        {
            steerRotation += 0.02f;
        }

        /// <summary>
        ///  Fires a bullet
        /// </summary>
        public void Fire()
        {            
            if (bullet == null)
            {
                Vector3 bulletDir = Vector3.Transform(tankFront, Matrix.CreateFromAxisAngle(tankRight, 0f) * Matrix.CreateFromAxisAngle(tankNormal, -turretRotationXAmount));
                bulletDir.Normalize();
                bullet = new Bullet(gDevice, contentManager, tankModel.Root.Transform.Translation, 0f, bulletDir, terrainVertices, this);
            }
        }

        /// <summary>
        /// Rotates the turret to the Right
        /// </summary>
        public void RotateTurretRight()
        {
            turretRotationXAmount -= 0.1f;
        }

        /// <summary>
        /// Rotates the turret to the Left
        /// </summary>
        public void RotateTurretLeft()
        {
            turretRotationXAmount += 0.1f;
        }

        /// <summary>
        ///     Applies the transformations to the tank
        /// </summary>
        public void ApplyRotation()
        {
            tankRotation = Matrix.CreateRotationY(steerRotation);
            tankDirection = Vector3.Transform(tankInitialDirection, tankRotation);

            tankRotation.Up = Helper.NormalFollow(tankModel.Root.Transform.Translation.X,
                                                  tankModel.Root.Transform.Translation.Z,
                                                  terrainVertices );

            tankRotation.Right = Vector3.Cross(tankRotation.Forward, 
                                               Helper.NormalFollow(tankModel.Root.Transform.Translation.X,
                                                                   tankModel.Root.Transform.Translation.Z,
                                                                   terrainVertices));

            tankRotation.Right = Vector3.Normalize(tankRotation.Right);
            tankRotation.Forward = Vector3.Cross(Helper.NormalFollow(tankModel.Root.Transform.Translation.X,
                                                                   tankModel.Root.Transform.Translation.Z,
                                                                   terrainVertices), 
                                                 tankRotation.Right);            

            tankRotation.Forward = Vector3.Normalize(tankRotation.Forward);

            WorldMatrix = tankRotation * Matrix.CreateTranslation(TankPosition);

            tankFront = tankRotation.Forward;
            tankRight = tankRotation.Right;
            tankNormal = tankRotation.Up;         
        }       

        /// <summary>
        ///     Updates tank details
        /// </summary>
        public void TankDetailUpdate(GameTime gametime)
        {
            if (bullet != null)
            {
                bullet.Update(gametime);

                if (!bullet.IsALive)
                {
                    bullet = null;
                }
            }

            if (TankPosition.X <= 0 || TankPosition.X >= Helper.MapHeight - 2 ||
                TankPosition.Z <= 0 || TankPosition.Z >= Helper.MapWidth - 2 )
            {
                TankPosition = tankInitialPosition;
            }

            particleManager.UpdateDust(gametime);
        }

        /// <summary>
        ///     Detects colision between tanks
        /// </summary>
        /// <param name="enemyPosition"></param>
        /// <returns></returns>
        public void Colision(Vector3 enemyPosition)
        {
            BoundingSphere tankColision = new BoundingSphere(TankPosition, 2.5f);
            BoundingSphere enemyColision = new BoundingSphere(enemyPosition, 2.5f);

            if (tankColision.Intersects(enemyColision))
                TankPosition = tankInitialPosition;
        }
    }
}
