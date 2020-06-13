using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TUMSS20.Graphics;

namespace TUMSS20.GameState
{
    public class QTE
    {
        private Constants.ELEMENT selectedElement;
        private int elapsedMs;
        private string elementString;
        private KeyboardState oldState;
        private float elementLabelY;

        public int TimeoutSeconds
        {
            get;
            set;
        }

        public bool Failed
        {
            get;
            set;
        }

        public bool Passed 
        {
            get;
            set;
        }

        public Constants.ELEMENT ChosenElement
        {
            get;
            set;
        }

        public QTE(Constants.ELEMENT selectedElement, int currentScore)
        {
            elapsedMs = 0;
            Failed = false;
            elementLabelY = 25.0f;
            ChoseElement();
            ChoseTimeout(currentScore);
            this.selectedElement = selectedElement;
        }
        
        private void ChoseElement()
        {
            ChosenElement = Constants.ChoseElement(true, ChosenElement);

            var validElements = Constants.ELEMENT_MAPPING[ChosenElement];
            var validElementNames = from element in validElements select Constants.ELEMENT_NAMES[element];

            elementString = string.Join(", ", validElementNames);
        }

        private void ChoseTimeout(int currentScore)
        {
            if (currentScore < 10)
            {
                TimeoutSeconds = 5;
            }
            else if (currentScore >= 10 && currentScore < 20)
            {
                TimeoutSeconds = 4;
            }
            else if (currentScore >= 20 && currentScore < 30)
            {
                TimeoutSeconds = 3;
            }
            else
            {
                TimeoutSeconds = 2;
            }
        }

        public void HandleInput(GameTime time)
        {
            if (elapsedMs < 100)
            {
                return;
            }

            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
            {
                int currentIndex = (int)selectedElement;
                ++currentIndex;
                if (currentIndex >= Constants.ELEMENT_COUNT)
                {
                    currentIndex = 0;
                }

                selectedElement = (Constants.ELEMENT)currentIndex;
            }

            oldState = keyboardState;
        }

        private void Validate()
        {
            List<Constants.ELEMENT> validElements = Constants.ELEMENT_MAPPING[ChosenElement];
            Passed = validElements.Contains(selectedElement);
            Failed = !Passed;
        }

        public void Update(GameTime time)
        {
            elapsedMs += time.ElapsedGameTime.Milliseconds;

            if ((elapsedMs / 1000) >= TimeoutSeconds)
            {
                Validate();
            }

            elementLabelY = 25.0f + (float)Math.Sin((float)elapsedMs / 100.0f) * 5.0f;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.Begin();
            Text.DrawCenteredString(graphics, spriteBatch, font, 0.0f, "MODIFY YOUR ELEMENT NOW!", Color.White);
            Text.DrawCenteredString(graphics, spriteBatch, font, elementLabelY, string.Format("Current element: {0}", Constants.ELEMENT_NAMES[selectedElement]), Color.White);
            Text.DrawCenteredString(graphics, spriteBatch, font, 45.0f, string.Format("Applicable elements: {0}", elementString), Color.White);
            spriteBatch.End();
        }
    }
}
