using Microsoft.Xna.Framework;
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
    public class QTE
    {
        private List<Keys> relevantKeys;
        private List<Keys> pressedKeys;
        private int elapsedMs;
        private string keyString;
        private bool failed;
        private KeyboardState oldState;
        private float keyLabelY;

        public bool Failed
        {
            get
            {
                return false;// failed;
            }
        }

        public bool Passed 
        {
            get;
            set;
        }

        public bool Invert
        {
            get;
            set;
        }

        public QTE()
        {
            elapsedMs = 0;
            failed = false;
            relevantKeys = new List<Keys>();
            pressedKeys = new List<Keys>();
            keyLabelY = 25.0f;
            GenerateKeys();
            SetInvertColors();
        }

        private void GenerateKeys()
        {
            Random rnd = new Random();

            for (int i = 0; i < rnd.Next(2, 4); i++)
            {
                int currentKey = rnd.Next((int)Keys.A, (int)Keys.Z);
                relevantKeys.Add((Keys)currentKey);
            }

            var keyCodes = from k in relevantKeys select k.ToString();
            keyString = string.Join(" ", keyCodes);
        }

        private void SetInvertColors()
        {
            Random rnd = new Random();
            Invert = (rnd.NextDouble() >= 0.5);
        }

        public void HandleInput(GameTime time)
        {
            if (elapsedMs < 100)
            {
                return;
            }

            KeyboardState keyboardState = Keyboard.GetState();

            List<Keys> currentPressedKeys = new List<Keys>(keyboardState.GetPressedKeys());

            foreach (Keys pressedKey in currentPressedKeys)
            {
                int keyCode = (int)pressedKey;
                if (keyCode < (int)Keys.A && keyCode > (int)Keys.Z)
                {
                    continue;
                }

                if (keyboardState.IsKeyDown(pressedKey) && oldState.IsKeyUp(pressedKey))
                {
                    pressedKeys.Add(pressedKey);
                }
            }

            oldState = keyboardState;
        }

        private void ValidateKeys()
        {
           /* if (relevantKeys.Count != pressedKeys.Count)
            {
                failed = true;
                return;
            }

            for (int i = 0; i < pressedKeys.Count; i++)
            {
                if (i >= relevantKeys.Count)
                {
                    failed = true;
                    return;
                }

                if (relevantKeys[i] != pressedKeys[i])
                {
                    failed = true;
                    return;
                }
            }*/

            Passed = true;
        }

        public void Update(GameTime time)
        {
            elapsedMs += time.ElapsedGameTime.Milliseconds;

            if (elapsedMs >= 2000)
            {
                ValidateKeys();
            }

            keyLabelY = 25.0f + (float)Math.Sin((float)elapsedMs / 100.0f) * 5.0f;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.Begin();
            Text.DrawCenteredString(graphics, spriteBatch, font, 0.0f, "MODIFY YOUR COLOR NOW!", Color.White);
            Text.DrawCenteredString(graphics, spriteBatch, font, keyLabelY, string.Format("Press {0}", keyString), Color.White);
            spriteBatch.End();
        }
    }
}
