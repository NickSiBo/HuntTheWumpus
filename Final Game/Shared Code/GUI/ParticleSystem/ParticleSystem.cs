﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuntTheWumpus.SharedCode.GUI.ParticleSystem
{
    class ParticleSystem
    {
        protected static Random random = new Random();
        public Vector2 EmitterLocation { get; set; }
        protected List<Particle> particles;
        protected List<Texture2D> textures;
        protected int lastGeneratedTime;
        protected double timeBetweenCreation;
        private int particleCap;

        public int NumberParticles
        {
            get { return this.particles.Count; }
        }

        public ParticleSystem(List<Texture2D> textures, Vector2 location, int rate, int particleCap)
        {
            EmitterLocation = location;
            this.textures = textures;
            this.particles = new List<Particle>();
            this.timeBetweenCreation = 1000D / rate;
            this.particleCap = particleCap;
            random = new Random();
        }

        public virtual void Update(GameTime time)
        {
            for (double i = lastGeneratedTime; i < time.TotalGameTime.Milliseconds; i += timeBetweenCreation)
            {
                if (NumberParticles < particleCap)
                {
                    particles.Add(GenerateNewParticle());
                }
            }
            this.lastGeneratedTime = time.TotalGameTime.Milliseconds;
            particles = particles.Where(p =>
            {
                p.Update(time);
                return p.IsAlive;
            }).ToList<Particle>();

            // Hack to make sure we don't bog down with particles
            // TODO: Make it so that we don't need this
            GC.Collect();
        }

        protected virtual Particle GenerateNewParticle()
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(
                                    1f * (float)(random.NextDouble() * 2 - 1),
                                    1f * (float)(random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            Color color = new Color(
                        (float)random.NextDouble(),
                        (float)random.NextDouble(),
                        (float)random.NextDouble());
            float size = (float)random.NextDouble();
            int ttl = 100 + random.Next(1000);

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl, 1);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
        }
    }
}
