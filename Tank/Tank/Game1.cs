using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tank
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        /// <summary>
        ///     The Graphics Device Manager
        /// </summary>
        private GraphicsDeviceManager graphics;

        /// <summary>
        ///     The sprite Batch
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        ///     The Terrain class
        /// </summary>
        private Terrain terrain;

        /// <summary>
        ///     The Camera Class
        /// </summary>
        private Camera camera;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            camera = new Camera(GraphicsDevice);
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            System.IO.FileStream stream = new System.IO.FileStream(@"Content\heightMap.jpg", System.IO.FileMode.Open);
            var heightMap = Texture2D.FromStream(GraphicsDevice, stream);
            stream.Dispose();

            stream = new System.IO.FileStream(@"Content\terrainTexture.jpg", System.IO.FileMode.Open);
            var texture = Texture2D.FromStream(GraphicsDevice, stream);
            stream.Dispose();

            terrain = new Terrain(heightMap, texture, GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Updates the camera
            camera.Update(gameTime);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            terrain.Draw(GraphicsDevice, camera);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
