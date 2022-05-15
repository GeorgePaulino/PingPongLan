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
    public class ServerScreen : GameScreen
    {

        NetServer server;

        private new MainGame Game => (MainGame)base.Game;
        public ServerScreen(MainGame game) : base(game) { }

        public override void Initialize()
        {
            var config = new NetPeerConfiguration("Ping Pong") { Port = 12345 };
            server = new NetServer(config);
            server.Start();
            Console.WriteLine("Karaí");
        }

        public override void LoadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            Console.WriteLine(server.ConnectionsCount);
            NetIncomingMessage message;
            while ((message = server.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        // handle custom messages
                        var data = new MsgData();
                        message.ReadAllFields(data);
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        // handle connection status messages
                        //switch (message.SenderConnection.Status)
                        //{

                        //}
                        break;

                    case NetIncomingMessageType.DebugMessage:
                        // handle debug messages
                        // (only received when compiled in DEBUG mode)
                        Console.WriteLine(message.ReadString());
                        break;

                    /* .. */
                    default:
                        Console.WriteLine("unhandled message with type: "
                            + message.MessageType);
                        break;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
        }
    }
}