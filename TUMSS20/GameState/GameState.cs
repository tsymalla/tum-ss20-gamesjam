using Comora;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUMSS20.GameState
{
    public class GameState : BaseGameState
    {
        private SpriteFont defaultFont;
        private Texture2D wall;
        private int screenWidth;
        private int screenHeight;
        private int points;
        private GameCharacter character;
        private Camera camera;
        private int playedMs;

        public override void Init(GraphicsDeviceManager graphics, ContentManager contentManager)
        {
            points = 0;
            playedMs = 0;
            screenWidth = graphics.PreferredBackBufferWidth;
            screenHeight = graphics.PreferredBackBufferHeight;

            character = new GameCharacter(screenHeight, contentManager);
            defaultFont = contentManager.Load<SpriteFont>("DefaultFont");
            wall = contentManager.Load<Texture2D>("wall");

            camera = new Camera(graphics.GraphicsDevice);
        }

        private void DrawWall(SpriteBatch spriteBatch, bool mirrored)
        {
            int yStart = 0;

            if (mirrored)
            {
                yStart = screenHeight - wall.Height;
            }

            int wallWidth = wall.Width;
            for (int x = 0; x < screenWidth / wallWidth; x++)
            {
                spriteBatch.Draw(wall, new Vector2(x * wallWidth, yStart), Color.White);
            }
        }

        public override void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime time)
        {
            // Draw points
            spriteBatch.Begin();
            spriteBatch.DrawString(defaultFont, string.Format("Points: {0}", points), new Vector2(10, 10), Color.White);
            DrawWall(spriteBatch, false);
            DrawWall(spriteBatch, true);
            spriteBatch.End();

            // Draw player followed by camera
            spriteBatch.Begin();
            character.Draw(time, spriteBatch);
            spriteBatch.End();
        }

        public override void HandleInput(GameTime time)
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                character.Impulse();
            }
        }

        public override void Update(GameTime time)
        {
            camera.Update(time);
            camera.Position = character.Position;

            playedMs += time.TotalGameTime.Milliseconds;
            if (playedMs > 0)
            {
                points = playedMs / 10000;
            }

            character.Update(time, points);
            CheckGameLost();
        }

        private void CheckGameLost()
        {
            // check boundaries
            var posY = character.Position.Y;
            var wallHeight = wall.Height;

            if (posY > wallHeight && posY + character.Height < screenHeight - wallHeight)
            {
                return;
            }

            var gameOver = new GameOverState();
            gameOver.SetTotalPoints(points);
            gameStateManager.PushState(gameOver);
        }
    }
}
