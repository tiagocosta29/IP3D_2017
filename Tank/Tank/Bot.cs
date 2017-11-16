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
    class Bot
    {
        /// <summary>
        ///  Tank placeholder
        /// </summary>
        public Tank BotTank { get; set; }

        /// <summary>
        ///     Current speed of the tank
        /// </summary>
        private float curretSpeed = 0;

        /// <summary>
        ///     Player constructor
        /// </summary>
        /// <param name="content"></param>
        /// <param name="vertices"></param>
        public Bot(GraphicsDevice device, ContentManager content, VertexPositionNormalTexture[] vertices)
        {
            BotTank = new Tank(device, content, vertices, "Player2", new Vector3(60f, 50f, 60f));
        }


        /// <summary>
        ///     Updates the bot tank
        /// </summary>
        public void Update()
        {            
            BotTank.ApplyRotation();
        }
        
        /// <summary>
        ///     Draws the bot tank
        /// </summary>
        /// <param name="camera"></param>
        public void Draw(Camera camera)
        {
            BotTank.DrawTank(camera);
        }

    }
}
