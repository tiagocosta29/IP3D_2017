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
        ///     Reference to the Bot
        /// </summary>
        public Bot BotEnemy;

        /// <summary>
        ///     Player constructor
        /// </summary>
        /// <param name="content"></param>
        /// <param name="vertices"></param>
        public Player(GraphicsDevice device, ContentManager content, VertexPositionNormalTexture[] vertices)
        {
            PlayerTank = new Tank(device, content, vertices, "RetroTank", new Vector3(50f, 50f, 50f));
        }

        /// <summary>
        ///     Updates the player controlls
        /// </summary>
        public void Update(GameTime gameTime)
        {
            var kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.W))
            {
                curretSpeed = MathHelper.Lerp(curretSpeed, GameConfig.TankTopSpeed, GameConfig.TankAcceleration);
                PlayerTank.MoveForward(curretSpeed, gameTime);
                PlayerTank.IsMoving = true;
            }
            else if (kb.IsKeyDown(Keys.S))
            {
                curretSpeed = MathHelper.Lerp(curretSpeed, GameConfig.TankTopSpeed, GameConfig.TankAcceleration);
                PlayerTank.MoveBackwards(curretSpeed);
            }
            else
            {
                PlayerTank.IsMoving = false;
            }

            if (kb.IsKeyDown(Keys.A))
                PlayerTank.RotateTankLeft();
            else if (kb.IsKeyDown(Keys.D))
                PlayerTank.RotateTankRight();


            if (kb.IsKeyDown(Keys.Right))
                PlayerTank.RotateTurretRight();
            else if(kb.IsKeyDown(Keys.Left))
                PlayerTank.RotateTurretLeft();

            if (kb.IsKeyDown(Keys.Space))
            {
                Console.WriteLine("FIRE!!!");
                PlayerTank.Fire();
            }

            PlayerTank.ApplyRotation();

            PlayerTank.TankDetailUpdate(gameTime);

            if (BotEnemy != null)
                PlayerTank.Colision(BotEnemy.BotTank.TankPosition);
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
