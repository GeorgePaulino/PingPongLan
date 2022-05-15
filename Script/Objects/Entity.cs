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
        public bool[] motions = new bool[2];
        public Point2? point = null;
        public Texture2D texture;
        public Color color;
        public Vector2 Direction;
        public float Velocity = 1;
        public IShapeF Bounds { get; set;}
        public PaddleEntity(RectangleF rectangle, Color color)
        {
            Bounds = rectangle;
            this.color = color;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if(point != null)
            {
                RectangleF rect = (RectangleF)Bounds;
                rect.Position = (Vector2)point;
                Bounds = rect;
            }
            //spriteBatch.Draw(texture, Bounds.Position, Color.White);
            spriteBatch.DrawRectangle((RectangleF)Bounds, color);
        }

        public virtual void Update(GameTime gameTime)
        {
            if(motions[0]) 
            {
                if(Bounds.Position.Y <= 0) Velocity = 0;
                else Velocity = 1;
                Direction.Y = -1;
            }
            else if(motions[1]) 
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
        public Vector2? point = null;
        public Texture2D texture;
        public Color color;
        public Vector2 Direction;
        public float Velocity = 1;
        public IShapeF Bounds { get; set; }
        public BallEntity(CircleF rectangle, Color color, bool wait = true)
        {
            Bounds = rectangle;
            this.color = color;
            Direction = new Vector2(Utilities.Random.Next(0, 2) == 0 ? 1 : -1, Utilities.Random.Next(-1, 2));
            this.wait = wait;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture, Bounds.Position, Color.White);
            if(point != null)
            {
                CircleF circle = (CircleF)Bounds;
                circle.Position = (Vector2) point;
                Bounds = circle;
            }
            spriteBatch.DrawCircle((CircleF)Bounds, 8, color);
        }

        bool wait = false;
        int initTime = 0;
        public virtual void Update(GameTime gameTime)
        {
            if(initTime == 0 && wait) initTime = gameTime.TotalGameTime.Seconds;
            if(gameTime.TotalGameTime.Seconds - initTime <= 5 && wait) return;
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
}