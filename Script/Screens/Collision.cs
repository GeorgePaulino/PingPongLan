using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Screens;
using Myra;

namespace PingPong
{
    public class CollisionScreen : GameScreen
    {
        private CollisionComponent _collisionComponent;
        private List<IEntity> _entities = new List<IEntity>();
        public readonly Random random = new Random(Guid.NewGuid().GetHashCode());
        const int MapWidth = 500;
        const int MapHeight = 500;

        private new MainGame Game => (MainGame)base.Game;
        public CollisionScreen(MainGame game) : base(game) 
        {
            _collisionComponent = new CollisionComponent(new RectangleF(0, 0, MapWidth, MapHeight));
        }

        public override void Initialize()
        {
            Game.graphics.PreferredBackBufferHeight = MapHeight;
            Game.graphics.PreferredBackBufferWidth = MapWidth;
            Game.graphics.ApplyChanges();

            for (var i = 0; i < 150; i++)
            {
                var size = random.Next(20, 40);
                var position = new Point2(random.Next(-MapWidth, MapWidth * 2), random.Next(0, MapHeight));
                if (i % 2 == 0)
                {
                    _entities.Add(new CircleEntity(random, new CircleF(position, size)));
                }
                else
                {
                    _entities.Add(new SquareEntity(random, new RectangleF(position, new Size2(size, size))));
                }
            }

            foreach (IEntity entity in _entities)
            {
                _collisionComponent.Insert(entity);
            }
        }

        public override void Update(GameTime gameTime) 
        { 
            foreach (IEntity entity in _entities)
            {
                entity.Update(gameTime);
            }
            _collisionComponent.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.DarkGray);
            Game.spriteBatch.Begin();
            foreach (IEntity entity in _entities)
            {
                entity.Draw(Game.spriteBatch);
            }
            Game.spriteBatch.End();
        }
    }
}