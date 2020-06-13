using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUMSS20.GameState
{
    public abstract class BaseGameState
    {
        protected GameStateManager gameStateManager;

        public void InjectStateManager(GameStateManager gameStateManager)
        {
            this.gameStateManager = gameStateManager;
        }

        public abstract void Init(GraphicsDeviceManager graphics, ContentManager contentManager);

        public abstract void SetActive();

        public abstract void HandleInput(GameTime time);

        public abstract void Update(GameTime time);

        public abstract void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameTime time);
    }
}
