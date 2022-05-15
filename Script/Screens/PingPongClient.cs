using System.Runtime.InteropServices.ComTypes;
using System.Data;
using System.Collections.Generic;
using System;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Collisions;

namespace PingPong
{
    public class PingPongClientScreen : GameScreen
    {
        NetClient client;
        ParticleController particleController = new ParticleController();

        private CollisionComponent collisionComponent;
        private List<IEntity> entities = new List<IEntity>();

        private SpriteFont font;
        private int[] points = new int[2];
        public void AddPoint(int index)
        {
            points[index] += 1;
        }
        private PaddleEntity[] padles = new PaddleEntity[2];
        private BallEntity ball;

        private new MainGame Game => (MainGame)base.Game;
        public PingPongClientScreen(MainGame game) : base(game) { }

        public override void Initialize()
        {
            var config = new NetPeerConfiguration("Ping Pong");
            client = new NetClient(config);
            client.Start();
            client.Connect(host: "10.0.0.107", port: 12345);

            Game.graphics.PreferredBackBufferWidth = Utilities.ScreenBounds[0];
            Game.graphics.PreferredBackBufferHeight = Utilities.ScreenBounds[1];
            Game.graphics.ApplyChanges();

            collisionComponent = new CollisionComponent(new RectangleF(0, 0, Utilities.ScreenBounds[0], Utilities.ScreenBounds[1]));

            padles[0] = new PaddleEntity(new RectangleF(new Point2(50, 190), new Size2(20, 120)), Color.Red);
            padles[1] = new PaddleEntity(new RectangleF(new Point2(750, 190), new Size2(20, 120)), Color.Blue);
            ball = new BallEntity(new CircleF(new Point2(395, 245), 10), Color.Green);
            entities.Add(padles[0]);
            entities.Add(padles[1]);
            entities.Add(ball);

            foreach(var e in entities) { collisionComponent.Insert(e); }
        }

        public override void LoadContent() 
        {
            particleController.LoadContent(Game.GraphicsDevice);
            font = Content.Load<SpriteFont>("Score");
        }

        public override void Update(GameTime gameTime) 
        {
            var send = client.CreateMessage();
            send.WriteAllFields(new ClientMessage(){up = Keyboard.GetState().IsKeyDown(Utilities.keys[1][0]), 
                down = Keyboard.GetState().IsKeyDown(Utilities.keys[1][1])});
            client.SendMessage(send, NetDeliveryMethod.ReliableOrdered);
            
            NetIncomingMessage message;
            while ((message = client.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        // handle custom messages
                        var data = new MsgData();
                        message.ReadAllFields(data);
                        padles[0].point = new Point2(data.pad1X, data.pad1Y);
                        padles[1].point = new Point2(data.pad2X, data.pad2Y);
                        ball.point = new Point2(data.ballX, data.ballY);
                        points[0] = data.score1;
                        points[1] = data.score2;
                        ball.Win = data.win;
                        break;

                    case NetIncomingMessageType.StatusChanged: break;

                    case NetIncomingMessageType.DebugMessage:
                        Console.WriteLine(message.ReadString());
                        break;
                    default:
                        Console.WriteLine("unhandled message with type: " + message.MessageType);
                        break;
                }
            }

            particleController.Update(gameTime);
            foreach(var e in entities) { e.Update(gameTime); }
            collisionComponent.Update(gameTime);
            if(ball.Win != 0)
            {
                AddPoint(ball.Win - 1);
                particleController.AddParticle(ball.Win == 1, ball.Bounds.Position);
                entities.Remove(ball);
                collisionComponent.Remove(ball);
                ball = new BallEntity(new CircleF(new Point2(395, 245), 10), Color.Green);
                entities.Add(ball);
                collisionComponent.Insert(ball);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);
            particleController.Draw(Game.spriteBatch);
            Game.spriteBatch.Begin();
            
            Game.spriteBatch.DrawString(font, points[1].ToString(), new Vector2(150, 20), Color.White);
            Game.spriteBatch.DrawString(font, points[0].ToString(), new Vector2(650, 20), Color.White);
            padles[0].Draw(Game.spriteBatch);
            padles[1].Draw(Game.spriteBatch);
            ball.Draw(Game.spriteBatch);
            
            Game.spriteBatch.End();
        }
    }
}