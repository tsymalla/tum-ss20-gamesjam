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
using TUMSS20.Graphics;

namespace TUMSS20.GameState
{
    public class StartMenuState : BaseGameState
    {
        private Texture2D titleScreen;
        private Texture2D elementTexture;
        private SpriteSheet elementSpriteSheet;

        public override void Init(GraphicsDeviceManager graphics, ContentManager contentManager)
        {
            titleScreen = contentManager.Load<Texture2D>("title");
            elementTexture = contentManager.Load<Texture2D>("elements");
            elementSpriteSheet = new SpriteSheet(elementTexture, Constants.ELEMENT_IMAGE_SIZE);
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

            int elementScale = Constants.ELEMENT_IMAGE_SCALE;
            int totalWidth = Constants.ELEMENT_COUNT * Constants.ELEMENT_IMAGE_SIZE * elementScale;
            int center = (graphics.PreferredBackBufferWidth / 2) - (totalWidth / 2);

            for (int i = 0; i < Constants.ELEMENT_COUNT; i++)
            {
                elementSpriteSheet.Draw(i, new Vector2(center + (i * Constants.ELEMENT_IMAGE_SIZE * elementScale), 10), elementScale, spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
