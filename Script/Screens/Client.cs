using System.Runtime.InteropServices.ComTypes;
using System.Data;
using System.Collections.Generic;
using System;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Collisions;

namespace PingPong
{
    public class ClientScreen : GameScreen
    {

        NetClient client;

        private new MainGame Game => (MainGame)base.Game;
        public ClientScreen(MainGame game) : base(game) { }

        public override void Initialize()
        {
            var config = new NetPeerConfiguration("Ping Pong");
            client = new NetClient(config);
            client.Start();
            client.Connect(host: "10.0.0.107", port: 12345);
        }

        public override void LoadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime)
        {
        }
    }
}