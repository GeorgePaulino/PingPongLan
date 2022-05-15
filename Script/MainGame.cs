using System.Reflection.Metadata;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using Myra;

namespace PingPong
{
    public class MainGame : Game
    {
        public int screen = 0;
        public SpriteBatch spriteBatch;
        public GraphicsDeviceManager graphics;

        public MainGame()
        {
            // Preferences
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = false;


            // Variables
            graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            this.Window.Title = "Ping Pong";
            ScreenController.Set(this);
            base.Initialize();
            ScreenController.LoadTitlecreen();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            MyraEnvironment.Game = this;
        }

        protected override void Update(GameTime gameTime)
        {
            if(screen != 1 && Keyboard.GetState().IsKeyDown(Keys.Escape)) 
            {
                ScreenController.LoadTitlecreen();
            }
            else if (screen == 1 && Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
