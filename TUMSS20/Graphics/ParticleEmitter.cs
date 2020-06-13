using Comora;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUMSS20.Graphics
{
    public class ParticleEmitter
    {
        private Random random;
        public Vector2 EmitterLocation { get; set; }
        private List<Particle> particles;
        private Texture2D texture;

        public ParticleEmitter(Texture2D texture, Vector2 location)
        {
            EmitterLocation = location;
            this.texture = texture;
            particles = new List<Particle>();
            random = new Random();
        }

        private Particle GenerateNewParticle()
        {
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(
                    3f * (float)(random.NextDouble() * 2 - 1),
                    3f * (float)(random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 15.5f * (float)(random.NextDouble() * 2 - 1);
            float size = (float)random.NextDouble();
            int ttl = 10 + random.Next(45);

            return new Particle(texture, position, velocity, angle, angularVelocity, size, ttl);
        }

        public void Update()
        {
            int total = 10;

            for (int i = 0; i < total; i++)
            {
                particles.Add(GenerateNewParticle());
            }

            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        public void Draw(Camera camera, SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Begin(camera);
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch, color);
            }
            spriteBatch.End();
        }
    }
}
