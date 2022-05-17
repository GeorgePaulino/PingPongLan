using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Screens;
using Myra;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;

namespace PingPong
{
    public class TitleScreen : GameScreen
    {
        private Desktop _desktop;

        private TitleUI titleGrid;
        private LanUI lanUIGrid;
        
        private new MainGame Game => (MainGame)base.Game;
        public TitleScreen(MainGame game) : base(game) { }

        public override void Initialize()
        {
            Game.screen = 1;
            Game.graphics.PreferredBackBufferWidth = Utilities.screenBounds[0];
            Game.graphics.PreferredBackBufferHeight = Utilities.screenBounds[1];
            Game.graphics.ApplyChanges();
            _desktop = new Desktop();
        }

        public override void LoadContent()
        {
            titleGrid = new TitleUI();
            titleGrid.initNormalGameBtn.Click += delegate { ScreenController.LoadPingPongScreen(); };
            titleGrid.lanUIBtn.Click += delegate { _desktop.Root = lanUIGrid; };
            lanUIGrid = new LanUI();
            lanUIGrid.initLanButton.Click += delegate {
                if(String.IsNullOrEmpty(lanUIGrid.IpInput.Text)) ScreenController.LoadServerScreen();
                else ScreenController.LoadClientScreen(lanUIGrid.IpInput.Text);
            };
            lanUIGrid.backButton.Click += delegate { _desktop.Root = titleGrid; };
            _desktop.Root = titleGrid;        
        }

        public override void Update(GameTime gameTime) { }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);
            _desktop.Render();
        }
    }
}