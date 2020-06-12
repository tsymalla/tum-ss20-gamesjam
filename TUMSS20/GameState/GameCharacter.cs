using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TUMSS20.GameState
{
    public class GameCharacter
    {
        private Vector2 position;
        private float velocity;
        private Texture2D texture;
        private const float gravity = 2.0f;

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

        public GameCharacter(int screenHeight, ContentManager contentManager)
        {
            velocity = 1.0f;
            position = new Vector2(0, screenHeight / 2);
            texture = contentManager.Load<Texture2D>("character");
        }

        public void Update(GameTime gameTime, int points)
        {
            if (points > 0)
            {
                velocity += (float)Math.Sin((float)points / 1000.0f);
            }

            position.X += velocity;

            // make the character go down automatically
            position.Y += gravity;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        public void Impulse()
        {
            position.Y -= 5;
        }
    }
}
