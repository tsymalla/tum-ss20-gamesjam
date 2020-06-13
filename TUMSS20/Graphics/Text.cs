using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUMSS20.Graphics
{
    public class Text
    {

        public static void DrawCenteredString(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, SpriteFont font, float offsetY, string text, Color color)
        {
            int screenWidth = graphics.PreferredBackBufferWidth;
            int screenHeight = graphics.PreferredBackBufferHeight;

            Vector2 measuredSize = font.MeasureString(text);
            Vector2 position = new Vector2((screenWidth / 2) - (measuredSize.X / 2), (screenHeight / 2) - (measuredSize.Y * 2) + offsetY);
            spriteBatch.DrawString(font, text, position, color);
        }
    }
}
