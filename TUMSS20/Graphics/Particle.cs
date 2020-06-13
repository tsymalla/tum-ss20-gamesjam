using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUMSS20.Graphics
{
    public class Particle
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }       
        public Vector2 Velocity { get; set; }
        public float Angle { get; set; }
        public float AngularVelocity { get; set; }
        public float Size { get; set; }
        public int TTL { get; set; }

        public Particle(Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity,  float size, int ttl)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = angularVelocity;
            Size = size;
            TTL = ttl;
        }

        public void Update()
        {
            TTL--;
            Position += Velocity;
            Angle += AngularVelocity;
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            spriteBatch.Draw(Texture, Position, sourceRectangle, color,
                Angle, origin, Size, SpriteEffects.None, 0f);
        }
    }
}
