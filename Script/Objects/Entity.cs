using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;


namespace PingPong
{
    public interface IEntity : ICollisionActor
    {
        public void Draw(SpriteBatch spriteBatch);
        public void Update(GameTime gameTime);
    }

    public abstract class GenericEntity : IEntity
    {
        public Texture2D texture;
        protected Color color;
        public IShapeF Bounds { get; set; }        
        
        protected Vector2 direction;
        protected float velocity = 1;

        public Vector2? prePositionedValue = null;

        public virtual void Update(GameTime gameTime)
        {
            Bounds.Position += direction * velocity * gameTime.GetElapsedSeconds() * Utilities.baseVelocity;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if(prePositionedValue != null)
            {
                IShapeF shape = (IShapeF)Bounds;
                shape.Position = (Vector2) prePositionedValue;
                Bounds = shape;
            }
        }

        public virtual void OnCollision(CollisionEventArgs collisionInfo)
        {
            velocity += 0.1f;
        }

    }

    public class PadleEntity : GenericEntity
    {
        // 0 - Up | 1 - Down
        public bool[] motions = new bool[2];
        
        public PadleEntity(RectangleF rectangle, Color color)
        {
            Bounds = rectangle;
            this.color = color;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(texture, (Rectangle)(RectangleF)Bounds, Color.White);
            //spriteBatch.DrawRectangle((RectangleF)Bounds, color);
        }

        public override void Update(GameTime gameTime)
        {
            
            direction.Y = 0;
            if(motions[0]) 
            {
                if(Bounds.Position.Y <= 0) direction.Y = 0;
                else direction.Y = -1;
            }
            else if(motions[1]) 
            {
                if(Bounds.Position.Y + 120 >= Utilities.screenBounds[1]) direction.Y = 0;
                else direction.Y = 1;
            }
            base.Update(gameTime);
        }

        public override void OnCollision(CollisionEventArgs collisionInfo)
        {
            base.OnCollision(collisionInfo);
        }
    }

    public class BallEntity : GenericEntity
    {
        public SoundEffect impactSoundEffect;
        public int markPoint = 0;
        private bool _waitInit = false;
        private int _initTime = 0;


        public BallEntity(CircleF rectangle, Color color, bool _waitInit = true)
        {
            Bounds = rectangle;
            this.color = color;
            
            direction = new Vector2(Utilities.random.Next(0, 2) == 0 ? 1 : -1, Utilities.random.Next(-1, 2));

            this._waitInit = _waitInit;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(texture, (CircleF)Bounds, Color.White);
            //spriteBatch.DrawCircle((CircleF)Bounds, 8, color);
        }

        public override void Update(GameTime gameTime)
        {
            if(_initTime == 0 && _waitInit) _initTime = gameTime.TotalGameTime.Seconds;
            if(gameTime.TotalGameTime.Seconds - _initTime <= 3 && _waitInit) return;

            base.Update(gameTime);
            
            if(Bounds.Position.Y <= 0 || 
                Bounds.Position.Y >= Utilities.screenBounds[1]) 
            {
                direction.Y *= -1;
                impactSoundEffect.Play();
            }
            if(Bounds.Position.X <= 0) markPoint = 1;
            else if (Bounds.Position.X >= Utilities.screenBounds[0]) markPoint = 2;
        }

        public override void OnCollision(CollisionEventArgs collisionInfo)
        {
            impactSoundEffect.Play(0.2f, 0, 0);
            velocity += 0.1f;
            direction.X *= -1;
            direction.Y = ((float)(Bounds.Position.Y - ((PadleEntity)collisionInfo.Other).Bounds.Position.Y) / 120f) * 2f - 1f;
            Bounds.Position -= collisionInfo.PenetrationVector;
        }
    }
}