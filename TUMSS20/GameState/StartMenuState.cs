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
        private Texture2D titleScreen;

        public override void Init(GraphicsDeviceManager graphics, ContentManager contentManager)
        {
            titleScreen = contentManager.Load<Texture2D>("title");
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
            spriteBatch.Begin();
            spriteBatch.Draw(titleScreen, new Vector2(0, 0), Color.Red);
            spriteBatch.End();
        }
    }
}
