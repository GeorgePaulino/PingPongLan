using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;

using MonoGame.Extended;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Modifiers.Containers;
using MonoGame.Extended.Particles.Modifiers.Interpolators;
using MonoGame.Extended.Particles.Profiles;
using MonoGame.Extended.TextureAtlases;

namespace PingPong
{

    public class ParticleController
    {
        
        private ParticleContainer _particleContainer = new ParticleContainer();
        
        private List<ParticleControllerObj> _controllerObjs = new List<ParticleControllerObj>();
        
        class ParticleControllerObj
        {
            public ParticleEffect particle;
            public float time = 0;
        }

        public void AddParticle(bool red, Vector2 pos)
        {
            ParticleEffect particle = _particleContainer.Particle(red);
            particle.Position = pos;
            _controllerObjs.Add(new ParticleControllerObj() { particle = particle });
        }

        public void LoadContent(GraphicsDevice graphics)
        {
            _particleContainer.LoadContent(graphics);
        }

        public void Update(GameTime gameTime)
        {
            int removeds = 0;
            for (int i = 0; i < _controllerObjs.Count; i++)
            {
                var obj = _controllerObjs[i - removeds];
                obj.particle.Update(gameTime.GetElapsedSeconds());
                obj.time += gameTime.GetElapsedSeconds();
                if (obj.time > 2)
                {
                    obj.particle.Dispose();
                    obj = null;
                    _controllerObjs.RemoveAt(i);
                    removeds++;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(blendState: BlendState.AlphaBlend);
            foreach (var obj in _controllerObjs)
            {
                spriteBatch.Draw(obj.particle);
            }
            spriteBatch.End();
        }

    }
    public class ParticleContainer
    {
        private Texture2D particleTexture;

        public void LoadContent(GraphicsDevice graphics)
        {
            particleTexture = new Texture2D(graphics, 1, 1);
            particleTexture.SetData(new[] { Color.White });
        }

        public ParticleEffect Particle(bool red)
        {
            TextureRegion2D textureRegion = new TextureRegion2D(particleTexture);

            return new ParticleEffect(autoTrigger: false)
            {
                Position = new Vector2(100, 240),
                Emitters = new List<ParticleEmitter>
                {
                    new ParticleEmitter(textureRegion, 1000, TimeSpan.FromSeconds(2),
                        Profile.Spray(new Vector2(red ? 1 : -1, 0), 8))
                    {
                        Parameters = new ParticleReleaseParameters
                        {
                            Speed = new Range<float>(0f, 200f),
                            Quantity = 30,
                            Rotation = new Range<float>(0f, 1f),
                            Scale = new Range<float>(3.0f, 4.0f)
                        },
                        Modifiers =
                        {
                            new AgeModifier
                            {
                                Interpolators =
                                {
                                    new ColorInterpolator
                                    {
                                        StartValue = new HslColor(red ? 1f : 205f, 0.7f, 0.6f),
                                        EndValue = new HslColor(red ? 1f : 205f, 1f, 0.6f)
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}