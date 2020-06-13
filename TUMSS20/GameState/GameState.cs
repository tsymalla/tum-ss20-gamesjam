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
        private int screenWidth;
        private int screenHeight;
        private int score;
        private GameCharacter character;
        private PointLightManager pointLightManager;
        private Camera camera;
        private int playedMs;
        private Wall topWall;
        private Wall bottomWall;

        private bool isInQTE;
        private int timeElapsedSinceQTE;
        private int nextQTESecondsDelay;
        private QTE qte;

        public override void Init(GraphicsDeviceManager graphics, ContentManager contentManager)
        {
            score = 0;
            playedMs = 0;
            timeElapsedSinceQTE = 0;
            screenWidth = graphics.PreferredBackBufferWidth;
            screenHeight = graphics.PreferredBackBufferHeight;

            character = new GameCharacter(screenHeight, contentManager);
            defaultFont = contentManager.Load<SpriteFont>("DefaultFont");

            pointLightManager = new PointLightManager(contentManager);

            topWall = new Wall(contentManager, screenWidth, screenHeight, false);
            bottomWall = new Wall(contentManager, screenWidth, screenHeight, true);

            camera = new Camera(graphics.GraphicsDevice);
            camera.LoadContent();
            camera.Debug.IsVisible = true;

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

        private void DrawScore(SpriteBatch spriteBatch)
        {
            string scoreString = string.Format("Score: {0}", score);
            var size = defaultFont.MeasureString(scoreString);

            var posX = screenWidth - size.X;
            var posY = screenHeight - size.Y;
            spriteBatch.DrawString(defaultFont, scoreString, new Vector2(posX, posY), Color.White);
        }

        public override void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime time)
        {
            pointLightManager.Draw(spriteBatch);

            // Draw player followed by camera
            character.Draw(camera, time, spriteBatch);

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
                spriteBatch.Begin(camera);
            }

            topWall.Draw(spriteBatch);
            bottomWall.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();
            DrawScore(spriteBatch);
            spriteBatch.End();
            spriteBatch.Draw(camera.Debug);

            if (handleQTE)
            {
                qte.Draw(graphics, spriteBatch, defaultFont);
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

            topWall.Update(character.Position, character.Width, time, score);
            bottomWall.Update(character.Position, character.Width, time, score);

            camera.Update(time);
            camera.Position = character.Position;

            playedMs += time.TotalGameTime.Milliseconds;
            if (playedMs > 0)
            {
                score = playedMs / 10000;

                // update qte timer
                timeElapsedSinceQTE += time.ElapsedGameTime.Milliseconds;
                CheckQTE();
            }

            character.Update(time, score);
            CheckGameLost();
        }

        private void CheckGameLost()
        {
            // check boundaries
            if (!topWall.HasCollided(character.Position, character.Height) && !bottomWall.HasCollided(character.Position, character.Height))
            {
                return;
            }

            GameOver();
        }

        private void GameOver()
        {
            var gameOver = new GameOverState();
            gameOver.SetTotalScore(score);
            gameStateManager.PushState(gameOver);
        }
    }
}
