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
    public sealed class GameStateManager
    {
        private Stack<BaseGameState> stateStack;
        private ContentManager contentManager;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private BaseGameState CurrentState
        {
            get
            {
                if (!NoState)
                {
                    return stateStack.Peek();
                }

                return null;
            }
        }

        public bool NoState
        {
            get
            {
                return stateStack.Count == 0;
            }
        }

        public GameStateManager(ContentManager contentManager, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            this.contentManager = contentManager;
            this.graphics = graphics;
            this.spriteBatch = spriteBatch;
            stateStack = new Stack<BaseGameState>();
        }

        public void PushState(BaseGameState state)
        {
            stateStack.Push(state);
            CurrentState.Init(graphics, contentManager);
            CurrentState.InjectStateManager(this);
        }

        public void PopState()
        {
            stateStack.Pop();
        }

        public void HandleInput(GameTime time)
        {
            if (!NoState)
            {
                CurrentState.HandleInput(time);
            }
        }

        public void Update(GameTime time)
        {
            if (!NoState)
            {
                CurrentState.Update(time);
            }
        }

        public void Draw(GameTime time)
        {
            if (!NoState)
            {
                CurrentState.Draw(graphics, spriteBatch, time);
            }
        }

        public void Restart()
        {
            while (stateStack.Count > 0)
            {
                stateStack.Pop();
            }

            PushState(new StartMenuState());
        }
    }
}
