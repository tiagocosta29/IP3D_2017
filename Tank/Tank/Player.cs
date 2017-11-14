using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Tank
{       
    class Player
    {
        /// <summary>
        ///  Tank placeholder
        /// </summary>
        public Tank PlayerTank { get; set; }

        /// <summary>
        ///     Current speed of the tank
        /// </summary>
        private float curretSpeed = 0;

        /// <summary>
        ///     Player constructor
        /// </summary>
        /// <param name="content"></param>
        /// <param name="vertices"></param>
        public Player(ContentManager content, VertexPositionNormalTexture[] vertices)
        {
            PlayerTank = new Tank(content, vertices);
        }

        /// <summary>
        ///     Updates the player controlls
        /// </summary>
        public void Update()
        {
            var kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.Y))
            {
                curretSpeed = MathHelper.Lerp(curretSpeed, GameConfig.TankTopSpeed, GameConfig.TankAcceleration);
                PlayerTank.MoveForward(curretSpeed);
            }

            if (kb.IsKeyDown(Keys.H))
            {
                curretSpeed = MathHelper.Lerp(curretSpeed, GameConfig.TankTopSpeed, GameConfig.TankAcceleration);
                PlayerTank.MoveBackwards(curretSpeed);
            }

            //else
            //{

            //    MathHelper.Lerp(curretSpeed, -0.2f, GameConfig.TankDeceleration);
            //    playerTank.MoveForward(curretSpeed);
            //}

            if (kb.IsKeyDown(Keys.G))
                PlayerTank.RotateTankLeft();
            if (kb.IsKeyDown(Keys.B))
                PlayerTank.RotateTurretRight();


            PlayerTank.ApplyRotation();    

        }

        /// <summary>
        ///     Draws the player tank
        /// </summary>
        /// <param name="camera"></param>
        public void Draw(Camera camera)
        {
            PlayerTank.DrawTank(camera);
        }

    }
}
