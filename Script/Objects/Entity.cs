using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;


namespace PingPong
{
    public interface IEntity : ICollisionActor
    {
        public void Draw(SpriteBatch spriteBatch);
        public void Update(GameTime gameTime);
    }

    public class PaddleEntity : IEntity
    {
        // 0 - Up | 1 - Down
        public Keys[] keys = new Keys[2];
        public Texture2D texture;
        public Color color;
        public Vector2 Direction;
        public float Velocity = 1;
        public IShapeF Bounds { get; }
        public PaddleEntity(RectangleF rectangle, Color color)
        {
            Bounds = rectangle;
            this.color = color;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture, Bounds.Position, Color.White);
            spriteBatch.DrawRectangle((RectangleF)Bounds, color);
        }

        public virtual void Update(GameTime gameTime)
        {
            if(Keyboard.GetState().IsKeyDown(keys[0])) 
            {
                if(Bounds.Position.Y <= 0) Velocity = 0;
                else Velocity = 1;
                Direction.Y = -1;
            }
            else if(Keyboard.GetState().IsKeyDown(keys[1])) 
            {
                if(Bounds.Position.Y + 120 >= Utilities.ScreenBounds[1]) Velocity=0;
                else Velocity = 1;
                Direction.Y = 1;
            }
            else 
            {
                Direction.Y = 0;
                Velocity = 0;
            }
            
            Bounds.Position += Direction * Velocity * gameTime.GetElapsedSeconds() * Utilities.BaseVelocity;
        }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            Velocity += 0.1f;
        }
    }

    public class BallEntity : IEntity
    {
        public int Win = 0;
        public Texture2D texture;
        public Color color;
        public Vector2 Direction;
        public float Velocity = 1;
        public IShapeF Bounds { get; }
        public BallEntity(CircleF rectangle, Color color)
        {
            Bounds = rectangle;
            this.color = color;
            Direction = new Vector2(Utilities.Random.Next(0, 2) == 0 ? 1 : -1, Utilities.Random.Next(-1, 2));
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture, Bounds.Position, Color.White);
            spriteBatch.DrawCircle((CircleF)Bounds, 8, color);
        }

        public virtual void Update(GameTime gameTime)
        {
            Bounds.Position += Direction * Velocity * gameTime.GetElapsedSeconds() * Utilities.BaseVelocity;
            if(Bounds.Position.Y <= 0 || Bounds.Position.Y >= Utilities.ScreenBounds[1]) 
            {
                Direction.Y *= -1;
            }
            if(Bounds.Position.X <= 0)
            {
                Win = 1;
            }
            else if (Bounds.Position.X >= Utilities.ScreenBounds[0])
            {
                Win = 2;
            }
        }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            PaddleEntity p = (PaddleEntity) collisionInfo.Other;
            Direction.X *= -1;
            Direction.Y = ((float)(Bounds.Position.Y - p.Bounds.Position.Y) / 120f) * 2f - 1f;
            Velocity += 0.1f;
            Bounds.Position -= collisionInfo.PenetrationVector;
        }
    }

    public class SquareEntity : IEntity
    {
        private readonly Random _random;
        public Vector2 Velocity;
        public IShapeF Bounds { get; }

        public SquareEntity(Random random, RectangleF rectangleF)
        {
            _random = random;
            Bounds = rectangleF;
            RandomizeVelocity();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle((RectangleF)Bounds, Color.Blue, 3);
        }

        public virtual void Update(GameTime gameTime)
        {
            Bounds.Position += Velocity * gameTime.GetElapsedSeconds() * 50;
        }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            Velocity.X *= -1;
            Velocity.Y *= -1;
            Bounds.Position -= collisionInfo.PenetrationVector;
        }

        private void RandomizeVelocity()
        {
            Velocity.X = _random.Next(-1, 2);
            Velocity.Y = _random.Next(-1, 2);
        }
    }

    public class CircleEntity : IEntity
    {
        private readonly Random _random;
        public Vector2 Velocity;
        public IShapeF Bounds { get; }

        public CircleEntity(Random random, CircleF circleF)
        {
            _random = random;
            Bounds = circleF;
            RandomizeVelocity();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawCircle((CircleF)Bounds, 360, Color.Red, 3f);
        }

        public void Update(GameTime gameTime)
        {
            Bounds.Position += Velocity * gameTime.GetElapsedSeconds() * 30;
        }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            RandomizeVelocity();
            Bounds.Position -= collisionInfo.PenetrationVector;
        }


        private void RandomizeVelocity()
        {
            Velocity.X = _random.Next(-1, 2);
            Velocity.Y = _random.Next(-1, 2);
        }
    }
}