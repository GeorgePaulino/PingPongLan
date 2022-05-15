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
            desktop = new Desktop();
            TittleGUI();
        }

        public void TittleGUI()
        {
            TitleUI grid = new TitleUI();
            grid.Normal.Click += delegate { ScreenController.LoadPingPongScreen(); };
            grid.Lan.Click += delegate { LanGUI(); };
            desktop.Root = grid;
        }

        public void LanGUI()
        {
            LanUI grid = new LanUI();
            grid.LanGame.Click += delegate {
                if(String.IsNullOrEmpty(grid.LanText.Text)) ScreenController.LoadServerScreen();
                else ScreenController.LoadClientScreen(grid.LanText.Text);
            };
            grid.Back.Click += delegate { TittleGUI(); };
            desktop.Root = grid;
        }

        public override void Update(GameTime gameTime) { }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);
            desktop.Render();
        }
    }
}