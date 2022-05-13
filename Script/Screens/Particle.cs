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
        ParticleContainer particleContainer = new ParticleContainer();
        public List<ParticleControllerObj> controllerObjs = new List<ParticleControllerObj>();
        public class ParticleControllerObj
        {
            public ParticleEffect particle;
            public float time = 0;
        }

        public void LoadContent(GraphicsDevice graphics)
        {
            particleContainer.LoadContent(graphics);
        }

        public void AddParticle(bool red, Vector2 pos)
        {
            ParticleEffect particle = red ? particleContainer.RedParticle() : particleContainer.BlueParticle();
            particle.Position = pos;
            controllerObjs.Add(new ParticleControllerObj() { particle = particle });
        }

        public void Update(GameTime gameTime)
        {
            int removeds = 0;
            for (int i = 0; i < controllerObjs.Count; i++)
            {
                var obj = controllerObjs[i - removeds];
                obj.particle.Update(gameTime.GetElapsedSeconds());
                obj.time += gameTime.GetElapsedSeconds();
                if (obj.time > 2)
                {
                    obj.particle.Dispose();
                    obj = null;
                    controllerObjs.RemoveAt(i);
                    removeds++;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(blendState: BlendState.AlphaBlend);
            foreach (var obj in controllerObjs)
            {
                spriteBatch.Draw(obj.particle);
            }
            spriteBatch.End();
        }

    }
    public class ParticleContainer
    {
        public ParticleEffect redParticle;
        public ParticleEffect blueParticle;
        private Texture2D particleTexture;


        public ParticleEffect RedParticle()
        {
            TextureRegion2D textureRegion = new TextureRegion2D(particleTexture);

            return new ParticleEffect(autoTrigger: false)
            {
                Position = new Vector2(100, 240),
                Emitters = new List<ParticleEmitter>
                {
                    new ParticleEmitter(textureRegion, 1000, TimeSpan.FromSeconds(2),
                        Profile.Spray(new Vector2(1, 0), 8))
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
                                        StartValue = new HslColor(1f, 0.7f, 0.6f),
                                        EndValue = new HslColor(1f, 1f, 0.6f)
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        public ParticleEffect BlueParticle()
        {
            TextureRegion2D textureRegion = new TextureRegion2D(particleTexture);


            return new ParticleEffect(autoTrigger: false)
            {
                Position = new Vector2(100, 240),
                Emitters = new List<ParticleEmitter>
                {
                    new ParticleEmitter(textureRegion, 1000, TimeSpan.FromSeconds(2),
                        Profile.Spray(new Vector2(-1, 0), 8))
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
                                        StartValue = new HslColor(205f, 0.7f, 0.6f),
                                        EndValue = new HslColor(205f, 1f, 0.6f)
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        public void LoadContent(GraphicsDevice graphics)
        {
            particleTexture = new Texture2D(graphics, 1, 1);
            particleTexture.SetData(new[] { Color.White });
        }
    }
}