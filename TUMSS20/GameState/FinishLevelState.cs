using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using TUMSS20.Audio;
using TUMSS20.Graphics;

namespace TUMSS20.GameState
{
    public class FinishLevelState : BaseGameState
    {
        private SpriteFont defaultFont;
        private int totalScore = 0;
        const int TIME_DELAY_SECONDS = 1;
        private int totalMsSpent = 0;
        private float scoreLabelY;
        private Color color;

        private bool IsDelayPassed
        {
            get
            {
                return (totalMsSpent / 1000) >= TIME_DELAY_SECONDS;
            }
        }

        public override void Init(GraphicsDeviceManager graphics, ContentManager contentManager)
        {
            defaultFont = contentManager.Load<SpriteFont>("DefaultFont");
            scoreLabelY = 80;
        }

        public void SetTotalScore(int points)
        {
            totalScore = points;
        }

        public void SetColor(Color color)
        {
            this.color = color;
        }

        public override void HandleInput(GameTime time)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && IsDelayPassed)
            {
                AudioCache.Instance.Play("pick");

                Constants.CurrentLevel++;
                Constants.TotalScore += totalScore;
                gameStateManager.Restart();
            }
        }

        public override void Update(GameTime time)
        {
            totalMsSpent += (int)time.ElapsedGameTime.TotalMilliseconds;
            scoreLabelY = 45.0f + (float)Math.Sin((float)totalMsSpent / 100.0f) * 5.0f;
        }

        public override void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime time)
        {
            graphics.GraphicsDevice.Clear(color);

            spriteBatch.Begin();
            Text.DrawCenteredString(graphics, spriteBatch, defaultFont, 0, string.Format("Finished Level {0}!", Constants.CurrentLevel), Color.White);
            Text.DrawCenteredString(graphics, spriteBatch, defaultFont, scoreLabelY, string.Format("You scored {0} points!", totalScore), Color.White);
            Text.DrawCenteredString(graphics, spriteBatch, defaultFont, 100, "Press Space to continue.", Color.White);
            spriteBatch.End();
        }

        public override void SetActive()
        {
            AudioCache.Instance.Play("success");
        }
    }
}
