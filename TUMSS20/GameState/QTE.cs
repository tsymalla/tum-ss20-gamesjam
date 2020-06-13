using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TUMSS20.Audio;
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
        private SpriteSheet elementSheet;
        private int timeLeftMs;

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
        public Constants.ELEMENT SelectedElement
        {
            get
            {
                return selectedElement;
            }
        }

        public QTE(Constants.ELEMENT selectedElement, int currentScore, SpriteSheet elementSheet)
        {
            elapsedMs = 0;
            Failed = false;
            elementLabelY = 25.0f;
            this.elementSheet = elementSheet;
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
            int relevantScore = currentScore;
            if (Constants.CurrentLevel > 1)
            {
                relevantScore += Constants.CurrentLevel * 5;
            }

            if (relevantScore < 20)
            {
                TimeoutSeconds = 5;
            }
            else if (relevantScore >= 20 && relevantScore < 35)
            {
                TimeoutSeconds = 4;
            }
            else if (relevantScore >= 35 && relevantScore < 50)
            {
                TimeoutSeconds = 3;
            }
            else if (relevantScore >= 50 && relevantScore < 65)
            {
                TimeoutSeconds = 2;
            }
            else
            {
                TimeoutSeconds = 1;
            }

            timeLeftMs = TimeoutSeconds * 1000;
        }

        public void HandleInput(GameTime time)
        {
            if (elapsedMs < 100 || Passed)
            {
                return;
            }

            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
            {
                AudioCache.Instance.Play("pick");

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

            timeLeftMs -= time.ElapsedGameTime.Milliseconds;
            timeLeftMs = Math.Max(timeLeftMs, 1);

            elementLabelY = 155.0f + (float)Math.Sin((float)elapsedMs / 100.0f) * 2.0f;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.Begin();
            Text.DrawCenteredString(graphics, spriteBatch, font, 0.0f, "Modify your element!", Color.White);
            Text.DrawCenteredString(graphics, spriteBatch, font, 85.0f, string.Format("I chose: {0}", Constants.ELEMENT_NAMES[ChosenElement]), Color.White);
            Text.DrawCenteredString(graphics, spriteBatch, font, 125.0f, string.Format("Beat me with: {0}", elementString), Color.White);
            Text.DrawCenteredString(graphics, spriteBatch, font, elementLabelY, string.Format("Your choice: {0}", Constants.ELEMENT_NAMES[selectedElement]), Color.White);
            Text.DrawCenteredString(graphics, spriteBatch, font, 210.0f, string.Format("Seconds left: {0}", (timeLeftMs / 1000)), Color.White);

            int elementScale = Constants.ELEMENT_IMAGE_SCALE;
            var relevantElements = Constants.ELEMENT_MAPPING[ChosenElement];
            int elementSize = Constants.ELEMENT_IMAGE_SIZE * elementScale;
            int totalWidth = relevantElements.Count * elementSize;
            int center = (graphics.PreferredBackBufferWidth / 2) - (totalWidth / 2);

            int chosenX = center - elementSize;
            float elementY = 55.0f;
            elementSheet.Draw((int)ChosenElement, new Vector2(chosenX, elementY), elementScale, spriteBatch);

            int i = 0;
            foreach (var applicableElement in relevantElements)
            {
                var index = (int)applicableElement;
                elementSheet.Draw(index, new Vector2(center + ((i + 1) * elementSize), elementY), elementScale, spriteBatch);
                i++;
            }

            spriteBatch.DrawString(font, "vs", new Vector2(chosenX + (elementSize * 1.3f), elementY + ((float)elementSize / 2.0f)), Color.White);

            spriteBatch.End();
        }
    }
}
