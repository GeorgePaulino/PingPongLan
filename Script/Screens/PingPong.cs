using System.Runtime.InteropServices.ComTypes;
using System.Data;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Collisions;

namespace PingPong
{
    public class PingPongScreen : GameScreen
    {
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
        public PingPongScreen(MainGame game) : base(game) { }

        public override void Initialize()
        {
            Game.graphics.PreferredBackBufferWidth = Utilities.ScreenBounds[0];
            Game.graphics.PreferredBackBufferHeight = Utilities.ScreenBounds[1];
            Game.graphics.ApplyChanges();

            collisionComponent = new CollisionComponent(new RectangleF(0, 0, Utilities.ScreenBounds[0], Utilities.ScreenBounds[1]));

            padles[0] = new PaddleEntity(new RectangleF(new Point2(50, 190), new Size2(20, 120)), Color.Red);
            padles[0].keys = Utilities.keys[0];
            padles[1] = new PaddleEntity(new RectangleF(new Point2(750, 190), new Size2(20, 120)), Color.Blue);
            padles[1].keys = Utilities.keys[1];
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