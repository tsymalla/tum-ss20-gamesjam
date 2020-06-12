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
using TUMSS20.Graphics;

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
        private PointLightManager pointLightManager;
        private Camera camera;
        private int playedMs;

        private bool isInQTE;
        private int timeElapsedSinceQTE;
        private int nextQTESecondsDelay;
        private QTE qte;

        public override void Init(GraphicsDeviceManager graphics, ContentManager contentManager)
        {
            points = 0;
            playedMs = 0;
            timeElapsedSinceQTE = 0;
            screenWidth = graphics.PreferredBackBufferWidth;
            screenHeight = graphics.PreferredBackBufferHeight;

            character = new GameCharacter(screenHeight, contentManager);
            defaultFont = contentManager.Load<SpriteFont>("DefaultFont");
            wall = contentManager.Load<Texture2D>("wall");

            pointLightManager = new PointLightManager(contentManager);

            camera = new Camera(graphics.GraphicsDevice);
            RestartQTE();
        }

        private void RestartQTE()
        {
            timeElapsedSinceQTE = 0;
            isInQTE = false;
            qte = null;

            var rnd = new Random();
            nextQTESecondsDelay = rnd.Next(3, 5);
        }

        private void CheckQTE()
        {
            // check seconds elapsed
            if ((timeElapsedSinceQTE / 1000) >= nextQTESecondsDelay)
            {
                ExecuteQTE();
            }
        }

        private void ExecuteQTE()
        {
            isInQTE = true;
            qte = new QTE();
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

        private void DrawPoints(SpriteBatch spriteBatch)
        {
            string pointString = string.Format("Points: {0}", points);
            var size = defaultFont.MeasureString(pointString);

            var posX = screenWidth - size.X;
            var posY = screenHeight - wall.Height - size.Y;
            spriteBatch.DrawString(defaultFont, string.Format("Points: {0}", points), new Vector2(posX, posY), Constants.GAME_FOREGROUND_COLOR);
        }

        public override void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime time)
        {
            pointLightManager.Draw(spriteBatch);

            // Draw player followed by camera
            character.Draw(time, spriteBatch);

            bool handleQTE = (qte != null && isInQTE);
     
            if (handleQTE && qte.Invert)
            {
                BlendState blendInvert = new BlendState()
                {
                    ColorSourceBlend = Blend.Zero,
                    ColorDestinationBlend = Blend.InverseSourceColor,
                };
                spriteBatch.Begin(SpriteSortMode.Deferred, blendInvert);
            }
            else
            {
                spriteBatch.Begin();
            }

            DrawWall(spriteBatch, false);
            DrawWall(spriteBatch, true);
            DrawPoints(spriteBatch);
            spriteBatch.End();

            if (handleQTE)
            {
                qte.Draw(spriteBatch, defaultFont);
                return;
            }
        }

        public override void HandleInput(GameTime time)
        {
            if (isInQTE)
            {
                qte.HandleInput(time);
                return;
            }

            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                character.Impulse();
            }
        }

        public override void Update(GameTime time)
        {
            if (isInQTE)
            {
                if (qte.Failed)
                {
                    GameOver();
                }
                else if (qte.Passed)
                {
                    RestartQTE();
                }
                else
                {
                    qte.Update(time);
                }

                return;
            }

            camera.Update(time);
            camera.Position = character.Position;

            playedMs += time.TotalGameTime.Milliseconds;
            if (playedMs > 0)
            {
                points = playedMs / 10000;

                // update qte timer
                timeElapsedSinceQTE += time.ElapsedGameTime.Milliseconds;
                CheckQTE();
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

            GameOver();
        }

        private void GameOver()
        {
            var gameOver = new GameOverState();
            gameOver.SetTotalPoints(points);
            gameStateManager.PushState(gameOver);
        }
    }
}
