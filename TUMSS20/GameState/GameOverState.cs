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

namespace TUMSS20.GameState
{
    public class GameOverState : BaseGameState
    {
        private SpriteFont defaultFont;
        private int totalPoints = 0;
        const int TIME_DELAY_SECONDS = 3;
        private int totalMsSpent = 0;

        private bool IsDelayPassed
        {
            get
            {
                return (totalMsSpent / 1000) >= TIME_DELAY_SECONDS;
            }
        }

        public override void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime time)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(defaultFont, "Game over!", new Vector2(50, 50), Color.White);
            spriteBatch.DrawString(defaultFont, string.Format("You scored {0} points! Try better next time.", totalPoints), new Vector2(50, 80), Color.White);
            spriteBatch.DrawString(defaultFont, "Press Space to replay.", new Vector2(50, 110), Color.White);
            spriteBatch.End();
        }

        public override void HandleInput(GameTime time)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && IsDelayPassed)
            {
                gameStateManager.Restart();
            }
        }

        public override void Init(GraphicsDeviceManager graphics, ContentManager contentManager)
        {
            defaultFont = contentManager.Load<SpriteFont>("DefaultFont");
        }

        public void SetTotalPoints(int points)
        {
            totalPoints = points;
        }

        public override void Update(GameTime time)
        {
            totalMsSpent += (int)time.ElapsedGameTime.TotalMilliseconds;
        }
    }
}
