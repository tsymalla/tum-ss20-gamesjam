using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace TUMSS20.GameState
{
    public class StartMenuState : BaseGameState
    {
        private SpriteFont defaultFont;

        public override void Init(GraphicsDeviceManager graphics, ContentManager contentManager)
        {
            defaultFont = contentManager.Load<SpriteFont>("DefaultFont");
        }

        public override void HandleInput(GameTime time)
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                gameStateManager.PushState(new GameState());
            }
        }

        public override void Update(GameTime time)
        {
        }
        public override void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime time)
        {
            var halfScreenWidth = graphics.PreferredBackBufferWidth / 2;
            var halfScreenHeight = graphics.PreferredBackBufferHeight / 2;
            var center = new Vector2(halfScreenWidth, halfScreenHeight);
            const string titleString = "ColorCave";
            var textSize = defaultFont.MeasureString(titleString);

            center.X -= textSize.X;
            center.Y -= textSize.Y;

            spriteBatch.Begin();
            spriteBatch.DrawString(defaultFont, "ColorCave", center, Color.White);
            spriteBatch.End();
        }
    }
}
