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
        Desktop desktop;
        
        private new MainGame Game => (MainGame)base.Game;
        public TitleScreen(MainGame game) : base(game) { }

        public override void Initialize()
        {
            Game.screen = 1;
            Game.graphics.PreferredBackBufferWidth = Utilities.ScreenBounds[0];
            Game.graphics.PreferredBackBufferHeight = Utilities.ScreenBounds[1];
            Game.graphics.ApplyChanges();
            desktop = new Desktop();
        }

        TitleUI titleGrid;
        LanUI lanUIGrid;

        public override void LoadContent()
        {
            titleGrid = new TitleUI();
            titleGrid.NormalGameBtn.Click += delegate { ScreenController.LoadPingPongScreen(); };
            titleGrid.LanUIBtn.Click += delegate { desktop.Root = lanUIGrid; };
            lanUIGrid = new LanUI();
            lanUIGrid.LanGameBtn.Click += delegate {
                if(String.IsNullOrEmpty(lanUIGrid.IpText.Text)) ScreenController.LoadServerScreen();
                else ScreenController.LoadClientScreen(lanUIGrid.IpText.Text);
            };
            lanUIGrid.BackBtn.Click += delegate { desktop.Root = titleGrid; };
            desktop.Root = titleGrid;        
        }

        public override void Update(GameTime gameTime) { }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);
            desktop.Render();
        }
    }
}