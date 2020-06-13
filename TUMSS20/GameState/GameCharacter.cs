using Comora;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using TUMSS20.Graphics;

namespace TUMSS20.GameState
{
    public class GameCharacter
    {
        private Vector2 position;
        private float velocity;
        private Texture2D texture;
        private Texture2D particleTexture;
        private const float gravity = 2.0f;
        private ParticleEmitter particleEmitter;

        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        public int Height
        {
            get
            {
                return texture.Height;
            }
        }

        public int Width
        {
            get
            {
                return texture.Height;
            }
        }

        public GameCharacter(int screenHeight, ContentManager contentManager)
        {
            velocity = 1.0f;
            position = new Vector2(0, screenHeight / 2);
            texture = contentManager.Load<Texture2D>("character");
            particleTexture = contentManager.Load<Texture2D>("pointlight");
            particleEmitter = new ParticleEmitter(particleTexture, position);
        }

        public void Update(GameTime gameTime, int points)
        {
            position.X += velocity;

            // make the character go down automatically
            position.Y += gravity;
            particleEmitter.EmitterLocation = position;
            particleEmitter.Update();
        }

        public void Draw(Camera camera, GameTime gameTime, SpriteBatch spriteBatch, Color color)
        {
            particleEmitter.Draw(camera, spriteBatch, color);

            spriteBatch.Begin(camera);
            spriteBatch.Draw(texture, position, color);
            spriteBatch.End();
        }

        public void Impulse()
        {
            position.Y -= 5;
        }
    }
}
